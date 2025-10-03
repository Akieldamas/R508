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
[TestSubject(typeof(TypeProduitController))]
[TestCategory("integration")]
public class TypeProduitControllerTest
{
    private readonly AppDbContext _context;
    private readonly TypeProduitController _TypeProduitController;
    private readonly IMapper _mapper;
    public TypeProduitControllerTest()
    {
        _context = new AppDbContext();

        TypeProduitManager manager = new(_context);
        var config = new MapperConfiguration(cfg => {
            cfg.AddProfile<MapperProfile>();
        }, new LoggerFactory());

        _mapper = config.CreateMapper();

        _TypeProduitController = new TypeProduitController(_mapper, manager);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.TypeProduits.RemoveRange(_context.TypeProduits);
        _context.SaveChanges();
    }

    [TestMethod]
    public void ShouldGetTypeProduit()
    {
        // Given : Une TypeProduit en DB
        TypeProduit TypeProduitInDb = new()
        {
            NomTypeProduit = "Ikea"
        };

        _context.TypeProduits.Add(TypeProduitInDb);
        _context.SaveChanges();

        // When : On appelle la méthode GET de l'API pour récupérer le produit
        ActionResult<TypeProduitDto> action = _TypeProduitController.Get(TypeProduitInDb.IdTypeProduit).GetAwaiter().GetResult();

        // Then : On récupère le produit et le code de retour est 200
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action.Value, typeof(TypeProduitDto));

        TypeProduitDto returnTypeProduit = action.Value;
        Assert.AreEqual(_mapper.Map<TypeProduitDto>(TypeProduitInDb), returnTypeProduit);
    }

    [TestMethod]
    public void ShouldDeleteTypeProduit()
    {
        // Given : Une TypeProduit en DB
        TypeProduit TypeProduitInDb = new()
        {
            NomTypeProduit = "Ikea"
        };

        _context.TypeProduits.Add(TypeProduitInDb);
        _context.SaveChanges();

        // When : On souhaite supprimer un produit depuis l'API
        IActionResult action = _TypeProduitController.Delete(TypeProduitInDb.IdTypeProduit).GetAwaiter().GetResult();

        // Then : Le produit a bien été supprimé et le code HTTP est NO_CONTENT (204)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NoContentResult));
        Assert.IsNull(_context.TypeProduits.Find(TypeProduitInDb.IdTypeProduit));
    }

    [TestMethod]
    public void ShouldNotDeleteTypeProduitBecauseTypeProduitDoesNotExist()
    {

        // Given : Une TypeProduit en DB
        TypeProduit TypeProduitInDb = new()
        {
            NomTypeProduit = "Ikea"
        };

        // When : On souhaite supprimer un produit depuis l'API
        IActionResult action = _TypeProduitController.Delete(TypeProduitInDb.IdTypeProduit).GetAwaiter().GetResult();

        // Then : Le produit a bien été supprimé et le code HTTP est NO_CONTENT (204)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NotFoundResult));
    }

    [TestMethod]
    public void ShouldGetAllTypeProduits()
    {
        // Given : Des TypeProduits enregistrées
        IEnumerable<TypeProduit> TypeProduitInDb = [
            new()
        {
            NomTypeProduit = "Ikea"
        },
           new()
        {
            NomTypeProduit = "Auchan"
        }
        ];

        _context.TypeProduits.AddRange(TypeProduitInDb);
        _context.SaveChanges();

        // When : On souhaite récupérer tous les TypeProduits
        var TypeProduits = _TypeProduitController.GetAll().GetAwaiter().GetResult();

        // Then : Tous les TypeProduits sont récupérés
        Assert.IsNotNull(TypeProduits);
        Assert.IsInstanceOfType(TypeProduits.Value, typeof(IEnumerable<TypeProduitDto>));
        Assert.IsTrue(_mapper.Map<IEnumerable<TypeProduitDto>>(TypeProduitInDb).SequenceEqual(TypeProduits.Value));
    }

    [TestMethod]
    public void GetTypeProduitShouldReturnNotFound()
    {
        // When : On appelle la méthode get de mon api pour récupérer le produit
        ActionResult<TypeProduitDto> action = _TypeProduitController.Get(0).GetAwaiter().GetResult();

        // Then : On ne renvoie rien et on renvoie NOT_FOUND (404)
        Assert.IsInstanceOfType(action.Result, typeof(NotFoundResult), "Ne renvoie pas 404");
        Assert.IsNull(action.Value, "La TypeProduit n'est pas null");
    }

    [TestMethod]
    public void ShouldCreateTypeProduit()
    {
        // Given : Un produit a enregistré
        TypeProduit TypeProduitToInsert = new()
        {
            NomTypeProduit = "Ikea"
        };

        // When : On appel la méthode POST de l'API pour enregistrer le produit
        ActionResult<TypeProduit> action = _TypeProduitController.Create(TypeProduitToInsert).GetAwaiter().GetResult();

        // Then : Le produit est bien enregistré et le code renvoyé et CREATED (201)
        TypeProduit TypeProduitInDb = _context.TypeProduits.Find(TypeProduitToInsert.IdTypeProduit);

        Assert.IsNotNull(TypeProduitInDb);
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action.Result, typeof(CreatedAtActionResult));
    }

    [TestMethod]
    public void ShouldUpdateTypeProduit()
    {
        // Given : Une TypeProduit à mettre à jour
        TypeProduit TypeProduitToEdit = new()
        {
            NomTypeProduit = "Ikea"
        };

        _context.TypeProduits.Add(TypeProduitToEdit);
        _context.SaveChanges();

        // Une fois enregistré, on modifie certaines propriétés 
        TypeProduitToEdit.NomTypeProduit = "Carnival";

        // When : On appelle la méthode PUT du controller pour mettre à jour le produit
        IActionResult action = _TypeProduitController.Update(TypeProduitToEdit.IdTypeProduit, TypeProduitToEdit).GetAwaiter().GetResult();

        // Then : On vérifie que le produit a bien été modifié et que le code renvoyé et NO_CONTENT (204)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NoContentResult));

        TypeProduit editedTypeProduitInDb = _context.TypeProduits.Find(TypeProduitToEdit.IdTypeProduit);

        Assert.IsNotNull(editedTypeProduitInDb);
        Assert.AreEqual(TypeProduitToEdit, editedTypeProduitInDb);
    }

    [TestMethod]
    public void ShouldNotUpdateTypeProduitBecauseIdInUrlIsDifferent()
    {
        // Given : Une TypeProduit à mettre à jour
        TypeProduit TypeProduitToEdit = new()
        {
            NomTypeProduit = "Ikea"
        };

        _context.TypeProduits.Add(TypeProduitToEdit);
        _context.SaveChanges();

        TypeProduitToEdit.NomTypeProduit = "Auchan";
        // When : On appelle la méthode PUT du controller pour mettre à jour le produit,
        // mais en précisant un ID différent de celui du produit enregistré
        IActionResult action = _TypeProduitController.Update(0, TypeProduitToEdit).GetAwaiter().GetResult();

        // Then : On vérifie que l'API renvoie un code BAD_REQUEST (400)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(BadRequestResult));
    }

    [TestMethod]
    public void ShouldNotUpdateTypeProduitBecauseTypeProduitDoesNotExist()
    {
        // Given : Une TypeProduit à mettre à jour
        TypeProduit TypeProduitToEdit = new()
        {
            NomTypeProduit = "Ikea"
        };


        // When : On appelle la méthode PUT du controller pour mettre à jour un produit qui n'est pas enregistré
        IActionResult action = _TypeProduitController.Update(TypeProduitToEdit.IdTypeProduit, TypeProduitToEdit).GetAwaiter().GetResult();

        // Then : On vérifie que l'API renvoie un code NOT_FOUND (404)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NotFoundResult));
    }
}