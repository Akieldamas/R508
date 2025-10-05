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
[TestSubject(typeof(ProductTypeController))]
[TestCategory("integration")]
public class ProductTypeControllerTest
{
    private readonly AppDbContext _context;
    private readonly ProductTypeController _ProductTypeController;
    private readonly IMapper _mapper;
    public ProductTypeControllerTest()
    {
        _context = new AppDbContext();

        ProductTypeManager manager = new(_context);
        var config = new MapperConfiguration(cfg => {
            cfg.AddProfile<MapperProfile>();
        }, new LoggerFactory());

        _mapper = config.CreateMapper();

        _ProductTypeController = new ProductTypeController(_mapper, manager);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.ProductTypes.RemoveRange(_context.ProductTypes);
        _context.SaveChanges();
    }

    [TestMethod]
    public void ShouldGetProductType()
    {
        // Given : Une TypeProduit en DB
        ProductType ProductTypeInDb = new()
        {
            NameProductType = "Ikea"
        };

        _context.ProductTypes.Add(ProductTypeInDb);
        _context.SaveChanges();

        // When : On appelle la méthode GET de l'API pour récupérer le produit
        ActionResult<ProductTypeDTO> action = _ProductTypeController.Get(ProductTypeInDb.IdProductType).GetAwaiter().GetResult();

        // Then : On récupère le produit et le code de retour est 200
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action.Value, typeof(ProductTypeDTO));

        ProductTypeDTO returnProductType = action.Value;
        Assert.AreEqual(_mapper.Map<ProductTypeDTO>(ProductTypeInDb), returnProductType);
    }

    [TestMethod]
    public void ShouldDeleteProductType()
    {
        // Given : Une TypeProduit en DB
        ProductType ProductTypeInDd = new()
        {
            NameProductType = "Ikea"
        };

        _context.ProductTypes.Add(ProductTypeInDd);
        _context.SaveChanges();

        // When : On souhaite supprimer un produit depuis l'API
        IActionResult action = _ProductTypeController.Delete(ProductTypeInDd.IdProductType).GetAwaiter().GetResult();

        // Then : Le produit a bien été supprimé et le code HTTP est NO_CONTENT (204)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NoContentResult));
        Assert.IsNull(_context.ProductTypes.Find(ProductTypeInDd.IdProductType));
    }

    [TestMethod]
    public void ShouldNotDeleteProductTypeBecauseProductTypeDoesNotExist()
    {

        // Given : Une TypeProduit en DB
        ProductType ProductTypeInDb = new()
        {
            NameProductType = "Ikea"
        };

        // When : On souhaite supprimer un produit depuis l'API
        IActionResult action = _ProductTypeController.Delete(ProductTypeInDb.IdProductType).GetAwaiter().GetResult();

        // Then : Le produit a bien été supprimé et le code HTTP est NO_CONTENT (204)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NotFoundResult));
    }

    [TestMethod]
    public void ShouldGetAllProductTypes()
    {
        // Given : Des TypeProduits enregistrées
        IEnumerable<ProductType> ProductTypeInDb = [
            new()
            {
                NameProductType = "Ikea"
            },
            new()
            {
                NameProductType = "Auchan"
            }
        ];

        _context.ProductTypes.AddRange(ProductTypeInDb);
        _context.SaveChanges();

        // When : On souhaite récupérer tous les TypeProduits
        var ProductTypes = _ProductTypeController.GetAll().GetAwaiter().GetResult();

        // Then : Tous les TypeProduits sont récupérés
        Assert.IsNotNull(ProductTypes);
        Assert.IsInstanceOfType(ProductTypes.Value, typeof(IEnumerable<ProductTypeDTO>));
        Assert.IsTrue(_mapper.Map<IEnumerable<ProductTypeDTO>>(ProductTypeInDb).SequenceEqual(ProductTypes.Value));
    }

    [TestMethod]
    public void GetProductTypeShouldReturnNotFound()
    {
        // When : On appelle la méthode get de mon api pour récupérer le produit
        ActionResult<ProductTypeDTO> action = _ProductTypeController.Get(0).GetAwaiter().GetResult();

        // Then : On ne renvoie rien et on renvoie NOT_FOUND (404)
        Assert.IsInstanceOfType(action.Result, typeof(NotFoundResult), "Ne renvoie pas 404");
        Assert.IsNull(action.Value, "La TypeProduit n'est pas null");
    }

    [TestMethod]
    public void ShouldCreateProductType()
    {
        // Given : Un produit a enregistré
        ProductType ProductTypeToInsert = new()
        {
            NameProductType = "Ikea"
        };

        // When : On appel la méthode POST de l'API pour enregistrer le produit
        ActionResult<ProductType> action = _ProductTypeController.Create(ProductTypeToInsert).GetAwaiter().GetResult();

        // Then : Le produit est bien enregistré et le code renvoyé et CREATED (201)
        ProductType ProductTypeInDb = _context.ProductTypes.Find(ProductTypeToInsert.IdProductType);

        Assert.IsNotNull(ProductTypeInDb);
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action.Result, typeof(CreatedAtActionResult));
    }

    [TestMethod]
    public void ShouldUpdateProductType()
    {
        // Given : Une TypeProduit à mettre à jour
        ProductType ProductTypeToEdit = new()
        {
            NameProductType = "Ikea"
        };

        _context.ProductTypes.Add(ProductTypeToEdit);
        _context.SaveChanges();

        // Une fois enregistré, on modifie certaines propriétés 
        ProductTypeToEdit.NameProductType = "Carnival";

        // When : On appelle la méthode PUT du controller pour mettre à jour le produit
        IActionResult action = _ProductTypeController.Update(ProductTypeToEdit.IdProductType, ProductTypeToEdit).GetAwaiter().GetResult();

        // Then : On vérifie que le produit a bien été modifié et que le code renvoyé et NO_CONTENT (204)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NoContentResult));

        ProductType editedTypeProduitInDb = _context.ProductTypes.Find(ProductTypeToEdit.IdProductType);

        Assert.IsNotNull(editedTypeProduitInDb);
        Assert.AreEqual(ProductTypeToEdit, editedTypeProduitInDb);
    }

    [TestMethod]
    public void ShouldNotUpdateTypeProduitBecauseIdInUrlIsDifferent()
    {
        // Given : Une TypeProduit à mettre à jour
        ProductType ProductTypeToEdit = new()
        {
            NameProductType = "Ikea"
        };

        _context.ProductTypes.Add(ProductTypeToEdit);
        _context.SaveChanges();

        ProductTypeToEdit.NameProductType = "Auchan";
        // When : On appelle la méthode PUT du controller pour mettre à jour le produit,
        // mais en précisant un ID différent de celui du produit enregistré
        IActionResult action = _ProductTypeController.Update(0, ProductTypeToEdit).GetAwaiter().GetResult();

        // Then : On vérifie que l'API renvoie un code BAD_REQUEST (400)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(BadRequestResult));
    }

    [TestMethod]
    public void ShouldNotUpdateTypeProduitBecauseTypeProduitDoesNotExist()
    {
        // Given : Une TypeProduit à mettre à jour
        ProductType ProductTypeToEdit = new()
        {
            NameProductType = "Ikea"
        };


        // When : On appelle la méthode PUT du controller pour mettre à jour un produit qui n'est pas enregistré
        IActionResult action = _ProductTypeController.Update(ProductTypeToEdit.IdProductType, ProductTypeToEdit).GetAwaiter().GetResult();

        // Then : On vérifie que l'API renvoie un code NOT_FOUND (404)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NotFoundResult));
    }
}