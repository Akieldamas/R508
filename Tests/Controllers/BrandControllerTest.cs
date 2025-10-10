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
[TestSubject(typeof(BrandController))]
[TestCategory("integration")]
public class BrandControllerTest
{
    private readonly AppDbContext _context;
    private readonly BrandController _brandController;
    private readonly IMapper _mapper;
    private int _brandId; // Accessible partout dans la classe

    public BrandControllerTest()
    {
        // Création du contexte de la DB et du manager
        _context = new AppDbContext(); 
        BrandManager manager = new(_context);

        // Configuration d'AutoMapper
        _mapper = MapperInstance.GetInstance();

        // Création du controller à tester
        _brandController = new BrandController(_mapper, manager);
    }

    [TestMethod]
    public void ShouldGetBrand()
    {
        // Given : Une Marque en DB
        Brand brandInDb = new()
        {
            NameBrand = "Ikea"
        };

        _context.Brands.Add(brandInDb);
        _context.SaveChanges();

        // When : On appelle la méthode GET de l'API pour récupérer le produit
        ActionResult<BrandDTO> action = _brandController.Get(brandInDb.IdBrand).GetAwaiter().GetResult();

        // Then : On récupère le produit et le code de retour est 200
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action.Value, typeof(BrandDTO));
        Assert.AreEqual(_mapper.Map<BrandDTO>(brandInDb), action.Value);
    }

    [TestMethod]
    public void ShouldDeleteBrand()
    {
        // Given : Une Marque en DB
        Brand brandInDb = new()
        {
            NameBrand = "Ikea"
        };

        _context.Brands.Add(brandInDb);
        _context.SaveChanges();

        // When : On souhaite supprimer un produit depuis l'API
        IActionResult action = _brandController.Delete(brandInDb.IdBrand).GetAwaiter().GetResult();

        // Then : Le produit a bien été supprimé et le code HTTP est NO_CONTENT (204)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NoContentResult));
        Assert.IsNull(_context.Brands.Find(brandInDb.IdBrand));
    }

    [TestMethod]
    public void ShouldNotDeleteBranddBecauseBrandDoesNotExist()
    {

        // Given : Une Marque en DB
        Brand brandInDb = new()
        {
            NameBrand = "Ikea"
        };

        // When : On souhaite supprimer un produit depuis l'API
        IActionResult action = _brandController.Delete(brandInDb.IdBrand).GetAwaiter().GetResult();

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
    public void GetBrandShouldReturnNotFound()
    {
        // When : On appelle la méthode get de mon api pour récupérer le produit
        ActionResult<BrandDTO> action = _brandController.Get(0).GetAwaiter().GetResult();

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
    //    Marque marqueInDb = _context.Marques.Find(action.Value.IdBrand);
    //    Marque marque_from_controller = action.Value;

    //    Assert.IsNotNull(marqueInDb);
    //    Assert.IsNotNull(marque_from_controller);
    //    Assert.IsInstanceOfType(action.Result, typeof(CreatedAtActionResult));
    //    Assert.AreEqual(marqueInDb.NomMarque, marque_from_controller.NomMarque);
    //} (Ne marche pas, problème de mapping avec AutoMapper, à revoir plus tard)

    [TestMethod]
    public void ShouldUpdateBrand()
    {
        // Given : Une marque à mettre à jour
        Brand brandToEdit = new()
        {
            NameBrand = "Ikea"
        };

        _context.Brands.Add(brandToEdit);
        _context.SaveChanges();

        // Une fois enregistré, on modifie certaines propriétés 
        brandToEdit.NameBrand = "Carnival";

        // When : On appelle la méthode PUT du controller pour mettre à jour le produit
        IActionResult action = _brandController.Update(brandToEdit.IdBrand, brandToEdit).GetAwaiter().GetResult();

        // Then : On vérifie que le produit a bien été modifié et que le code renvoyé et NO_CONTENT (204)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NoContentResult));

        Brand editedbrandInDb = _context.Brands.Find(brandToEdit.IdBrand);

        Assert.IsNotNull(editedbrandInDb);
        Assert.AreEqual(brandToEdit, editedbrandInDb);
    }

    [TestMethod]
    public void ShouldNotUpdateBrandBecauseIdInUrlIsDifferent()
    {
        // Given : Une marque à mettre à jour
        Brand brandToEdit = new()
        {
            NameBrand = "Ikea"
        };

        _context.Brands.Add(brandToEdit);
        _context.SaveChanges();

        brandToEdit.NameBrand = "Auchan";
        // When : On appelle la méthode PUT du controller pour mettre à jour le produit,
        // mais en précisant un ID différent de celui du produit enregistré
        IActionResult action = _brandController.Update(0, brandToEdit).GetAwaiter().GetResult();

        // Then : On vérifie que l'API renvoie un code BAD_REQUEST (400)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(BadRequestResult));
    }

    [TestMethod]
    public void ShouldNotUpdateBrandBecauseBrandDoesNotExist()
    {
        // Given : Une marque à mettre à jour
        Brand brandToEdit = new()
        {
            NameBrand = "Ikea"
        };


        // When : On appelle la méthode PUT du controller pour mettre à jour un produit qui n'est pas enregistré
        IActionResult action = _brandController.Update(brandToEdit.IdBrand, brandToEdit).GetAwaiter().GetResult();

        // Then : On vérifie que l'API renvoie un code NOT_FOUND (404)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NotFoundResult));
    }

    [TestCleanup]
    public void Cleanup()
    {
        var brands = _context.Brands.Where(p => p.IdBrand == _brandId).ToList();
        if (brands.Any())
        {
            _context.Brands.RemoveRange(brands);
            _context.SaveChanges();
        }


        _context.Brands.RemoveRange(brands);
        _context.SaveChanges();

        var brand = _context.Brands.Find(_brandId);
        if (brand != null)
        {
            _context.Brands.Remove(brand);
            _context.SaveChanges();
        }
    }
}