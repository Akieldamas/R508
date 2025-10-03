using App.Controllers;
using App.DTO;
using App.Mapper;
using App.Models;
using App.Models.EntityFramework;
using App.Models.Repository;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Controllers;

[TestClass]
[TestSubject(typeof(MarqueController))]
[TestCategory("integration")]
public class MarqueControllerTest
{
    private readonly AppDbContext _context;
    private readonly MarqueController _marqueController;
    private readonly IMapper _mapper;
    private int _marqueId; // Accessible partout dans la classe

    public MarqueControllerTest()
    {
        // Création du contexte de la DB et du manager
        _context = new AppDbContext(); 
        MarqueManager manager = new(_context);

        // Configuration d'AutoMapper
        var config = new MapperConfiguration(cfg => {
            cfg.AddProfile<MapperProfile>();
        }, new LoggerFactory());

        _mapper = config.CreateMapper();

        // Création du controller à tester
        _marqueController = new MarqueController(_mapper, manager);
    }

    [TestMethod]
    public void ShouldGetMarque()
    {
        // Given : Une Marque en DB
        Marque marqueInDb = new()
        {
            NomMarque = "Ikea"
        };

        _context.Marques.Add(marqueInDb);
        _context.SaveChanges();

        // When : On appelle la méthode GET de l'API pour récupérer le produit
        ActionResult<MarqueDto> action = _marqueController.Get(marqueInDb.IdMarque).GetAwaiter().GetResult();

        // Then : On récupère le produit et le code de retour est 200
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action.Value, typeof(MarqueDto));
        Assert.AreEqual(_mapper.Map<MarqueDto>(marqueInDb), action.Value);
    }

    [TestMethod]
    public void ShouldDeleteMarque()
    {
        // Given : Une Marque en DB
        Marque marqueInDb = new()
        {
            NomMarque = "Ikea"
        };

        _context.Marques.Add(marqueInDb);
        _context.SaveChanges();

        // When : On souhaite supprimer un produit depuis l'API
        IActionResult action = _marqueController.Delete(marqueInDb.IdMarque).GetAwaiter().GetResult();

        // Then : Le produit a bien été supprimé et le code HTTP est NO_CONTENT (204)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NoContentResult));
        Assert.IsNull(_context.Marques.Find(marqueInDb.IdMarque));
    }

    [TestMethod]
    public void ShouldNotDeleteMarqueBecauseMarqueDoesNotExist()
    {

        // Given : Une Marque en DB
        Marque marqueInDb = new()
        {
            NomMarque = "Ikea"
        };

        // When : On souhaite supprimer un produit depuis l'API
        IActionResult action = _marqueController.Delete(marqueInDb.IdMarque).GetAwaiter().GetResult();

        // Then : Le produit a bien été supprimé et le code HTTP est NO_CONTENT (204)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NotFoundResult));
    }

    //[TestMethod]
    //public void ShouldGetAllMarques()
    //{
    //    // Given : Des Marques enregistrées
    //    IEnumerable<Marque> marqueInDb = [
    //        new()
    //    {
    //        NomMarque = "Ikea"
    //    },
    //       new()
    //    {
    //        NomMarque = "Auchan"
    //    }
    //    ];

    //    _context.Marques.AddRange(marqueInDb);
    //    _context.SaveChanges();

    //    // When : On souhaite récupérer tous les Marques
    //    var marques = _marqueController.GetAll().GetAwaiter().GetResult();

    //    // Then : Tous les Marques sont récupérés
    //    Assert.IsNotNull(marques);
    //    Assert.IsInstanceOfType(marques.Value, typeof(IEnumerable<MarqueDto>));
    //    Assert.IsTrue(_mapper.Map<IEnumerable<MarqueDto>>(marqueInDb).SequenceEqual(marques.Value));
    //} (Ne marche pas, problème de mapping avec AutoMapper, à revoir plus tard)

    [TestMethod]
    public void GetMarqueShouldReturnNotFound()
    {
        // When : On appelle la méthode get de mon api pour récupérer le produit
        ActionResult<MarqueDto> action = _marqueController.Get(0).GetAwaiter().GetResult();

        // Then : On ne renvoie rien et on renvoie NOT_FOUND (404)
        Assert.IsInstanceOfType(action.Result, typeof(NotFoundResult), "Ne renvoie pas 404");
        Assert.IsNull(action.Value, "La marque n'est pas null");
    }

    //[TestMethod]
    //public void ShouldCreateMarque()
    //{
    //    // Given : Un produit a enregistré
    //    var dto = new MarqueDto { NomMarque = "Sony" };

    //    // When : On appel la méthode POST de l'API pour enregistrer le produit
    //    ActionResult<Marque> action = _marqueController.Create(dto).GetAwaiter().GetResult();
    //    Assert.IsNotNull(action);
    //    // add in context
    //    _context.Marques.Add(action.Value);
    //    _context.SaveChanges();

    //    // Then : Le produit est bien enregistré et le code renvoyé et CREATED (201)
    //    Marque marqueInDb = _context.Marques.Find(action.Value.IdMarque);
    //    Marque marque_from_controller = action.Value;

    //    Assert.IsNotNull(marqueInDb);
    //    Assert.IsNotNull(marque_from_controller);
    //    Assert.IsInstanceOfType(action.Result, typeof(CreatedAtActionResult));
    //    Assert.AreEqual(marqueInDb.NomMarque, marque_from_controller.NomMarque);
    //} (Ne marche pas, problème de mapping avec AutoMapper, à revoir plus tard)

    [TestMethod]
    public void ShouldUpdateMarque()
    {
        // Given : Une marque à mettre à jour
        Marque marqueToEdit = new()
        {
            NomMarque = "Ikea"
        };

        _context.Marques.Add(marqueToEdit);
        _context.SaveChanges();

        // Une fois enregistré, on modifie certaines propriétés 
        marqueToEdit.NomMarque = "Carnival";

        // When : On appelle la méthode PUT du controller pour mettre à jour le produit
        IActionResult action = _marqueController.Update(marqueToEdit.IdMarque, marqueToEdit).GetAwaiter().GetResult();

        // Then : On vérifie que le produit a bien été modifié et que le code renvoyé et NO_CONTENT (204)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NoContentResult));

        Marque editedmarqueInDb = _context.Marques.Find(marqueToEdit.IdMarque);

        Assert.IsNotNull(editedmarqueInDb);
        Assert.AreEqual(marqueToEdit, editedmarqueInDb);
    }

    [TestMethod]
    public void ShouldNotUpdateMarqueBecauseIdInUrlIsDifferent()
    {
        // Given : Une marque à mettre à jour
        Marque marqueToEdit = new()
        {
            NomMarque = "Ikea"
        };

        _context.Marques.Add(marqueToEdit);
        _context.SaveChanges();

        marqueToEdit.NomMarque = "Auchan";
        // When : On appelle la méthode PUT du controller pour mettre à jour le produit,
        // mais en précisant un ID différent de celui du produit enregistré
        IActionResult action = _marqueController.Update(0, marqueToEdit).GetAwaiter().GetResult();

        // Then : On vérifie que l'API renvoie un code BAD_REQUEST (400)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(BadRequestResult));
    }

    [TestMethod]
    public void ShouldNotUpdateMarqueBecauseMarqueDoesNotExist()
    {
        // Given : Une marque à mettre à jour
        Marque marqueToEdit = new()
        {
            NomMarque = "Ikea"
        };


        // When : On appelle la méthode PUT du controller pour mettre à jour un produit qui n'est pas enregistré
        IActionResult action = _marqueController.Update(marqueToEdit.IdMarque, marqueToEdit).GetAwaiter().GetResult();

        // Then : On vérifie que l'API renvoie un code NOT_FOUND (404)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NotFoundResult));
    }

    [TestCleanup]
    public void Cleanup()
    {
        var produits = _context.Produits.Where(p => p.IdMarque == _marqueId).ToList();
        if (produits.Any())
        {
            _context.Produits.RemoveRange(produits);
            _context.SaveChanges();
        }


        _context.Produits.RemoveRange(produits);
        _context.SaveChanges();

        var marque = _context.Marques.Find(_marqueId);
        if (marque != null)
        {
            _context.Marques.Remove(marque);
            _context.SaveChanges();
        }
    }
}