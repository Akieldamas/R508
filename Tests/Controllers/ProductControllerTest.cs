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
[TestSubject(typeof(ProductController))]
[TestCategory("integration")]
public class ProductControllerTest
{
    private readonly AppDbContext  _context;
    private readonly ProductController _productController;
    private readonly IMapper _mapper;
    public ProductControllerTest()
    {
        _context = new AppDbContext();
        
        ProductManager manager = new(_context);
        var config = new MapperConfiguration(cfg => {
            cfg.AddProfile<MapperProfile>();
        }, new LoggerFactory());

        _mapper = config.CreateMapper();

        _productController = new ProductController(_mapper, manager);
    }
    
    [TestCleanup]
    public void Cleanup()
    {
        _context.Products.RemoveRange(_context.Products);
        _context.SaveChanges();
    }

    //[TestMethod]
    //public void ShouldGetProduct()
    //{
    //    // Given : Un produit en enregistré
    //    Produit produitInDb = new()
    //    {
    //        NameProduct = "Chaise",
    //        Description = "Une superbe chaise",
    //        NamePhoto = "Une superbe chaise bleu",
    //        UriPhoto = "https://ikea.fr/chaise.jpg",
    //        StockReal = 1
    //    };

    //    _context.Produits.Add(produitInDb);
    //    _context.SaveChanges();

    //    // When : On appelle la méthode GET de l'API pour récupérer le produit
    //    ActionResult<Produit> action = _productController.Get(produitInDb.IdProduct).GetAwaiter().GetResult();

    //    // Then : On récupère le produit et le code de retour est 200
    //    Assert.IsNotNull(action);
    //    Assert.IsInstanceOfType(action.Value, typeof(ProduitDetailDto));

    //    Produit returnProduct = action.Value;
    //    Assert.AreEqual(_mapper.Map<Produit>(produitInDb), returnProduct);
    //} (Problème avec Produit due to blazor) => IEnumerable<Produit> n'est pas reconnu

    [TestMethod]
    public void ShouldDeleteProduct()
    {
        // Given : Un produit enregistré
        Product productInDb = new()
        {
            NameProduct = "Chaise",
            Description = "Une superbe chaise",
            NamePhoto = "Une superbe chaise bleu",
            UriPhoto = "https://ikea.fr/chaise.jpg",
            StockReal = 1

        };

        _context.Products.Add(productInDb);
        _context.SaveChanges();
        
        // When : On souhaite supprimer un produit depuis l'API
        IActionResult action = _productController.Delete(productInDb.IdProduct).GetAwaiter().GetResult();
        
        // Then : Le produit a bien été supprimé et le code HTTP est NO_CONTENT (204)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NoContentResult));
        Assert.IsNull(_context.Products.Find(productInDb.IdProduct));
    }
    
    [TestMethod]
    public void ShouldNotDeleteProductBecauseProductDoesNotExist()
    {
        // Given : Un produit enregistré
        Product productInDb = new()
        {
            NameProduct = "Chaise",
            Description = "Une superbe chaise",
            NamePhoto = "Une superbe chaise bleu",
            UriPhoto = "https://ikea.fr/chaise.jpg"
        };
        
        // When : On souhaite supprimer un produit depuis l'API
        IActionResult action = _productController.Delete(productInDb.IdProduct).GetAwaiter().GetResult();
        
        // Then : Le produit a bien été supprimé et le code HTTP est NO_CONTENT (204)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NotFoundResult));
    }

    //[TestMethod]
    //public void ShouldGetAllProducts()
    //{
    //    // Given : Des produits enregistrées
    //    IEnumerable<Produit> productInDb = [
    //        new()
    //        {
    //            NameProduct = "Chaise",
    //            Description = "Une superbe chaise",
    //            NamePhoto = "Une superbe chaise bleu",
    //            UriPhoto = "https://ikea.fr/chaise.jpg",
    //            StockReal = 1
    //        },
    //        new()
    //        {
    //            NameProduct = "Armoir",
    //            Description = "Une superbe armoire",
    //            NamePhoto = "Une superbe armoire jaune",
    //            UriPhoto = "https://ikea.fr/armoire-jaune.jpg",
    //            StockReal = 1
    //        }
    //    ];

    //    _context.Produits.AddRange(productInDb);
    //    _context.SaveChanges();

    //    // When : On souhaite récupérer tous les produits
    //    var products = _productController.GetAll().GetAwaiter().GetResult();

    //    // Then : Tous les produits sont récupérés
    //    Assert.IsNotNull(products);
    //    Assert.IsInstanceOfType(products.Value, typeof(IEnumerable<Produit>));
    //    Assert.IsTrue(_mapper.Map<IEnumerable<Produit>>(productInDb).SequenceEqual(products.Value));
    //} (Problème avec Produit due to blazor) => IEnumerable<Produit> n'est pas reconnu

    [TestMethod]
    public void GetProductShouldReturnNotFound()
    {
        // When : On appelle la méthode get de mon api pour récupérer le produit
        ActionResult<ProductDetailsDTO> action = _productController.Get(0).GetAwaiter().GetResult();
        
        // Then : On ne renvoie rien et on renvoie NOT_FOUND (404)
        Assert.IsInstanceOfType(action.Result, typeof(NotFoundResult), "Ne renvoie pas 404");
        Assert.IsNull(action.Value, "Le produit n'est pas null");
    }
    
    [TestMethod]
    public void ShouldCreateProduct()
    {
        // Given : Un produit a enregistré
        Product productToInsert = new()
        {
            NameProduct = "Chaise",
            Description = "Une superbe chaise",
            NamePhoto = "Une superbe chaise bleu",
            UriPhoto = "https://ikea.fr/chaise.jpg",
            StockReal = 1
        };
        
        // When : On appel la méthode POST de l'API pour enregistrer le produit
        ActionResult<Product> action = _productController.Create(productToInsert).GetAwaiter().GetResult();
        
        // Then : Le produit est bien enregistré et le code renvoyé et CREATED (201)
        Product productInDb = _context.Products.Find(productToInsert.IdProduct);
        
        Assert.IsNotNull(productInDb);
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action.Result, typeof(CreatedAtActionResult));
    }

    [TestMethod]
    public void ShouldUpdateProduct()
    {
        // Given : Un produit à mettre à jour
        Product productToEdit = new()
        {
            NameProduct = "Bureau",
            Description = "Un super bureau",
            NamePhoto = "Un super bureau bleu",
            UriPhoto = "https://ikea.fr/bureau.jpg",
            StockReal = 1
        };
        
        _context.Products.Add(productToEdit);
        _context.SaveChanges();

        // Une fois enregistré, on modifie certaines propriétés 
        productToEdit.NameProduct = "Lit";
        productToEdit.Description = "Un super lit";

        // When : On appelle la méthode PUT du controller pour mettre à jour le produit
        IActionResult action = _productController.Update(productToEdit.IdProduct, productToEdit).GetAwaiter().GetResult();
        
        // Then : On vérifie que le produit a bien été modifié et que le code renvoyé et NO_CONTENT (204)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NoContentResult));
        
        Product editedProductInDb = _context.Products.Find(productToEdit.IdProduct);
        
        Assert.IsNotNull(editedProductInDb);
        Assert.AreEqual(productToEdit, editedProductInDb);
    }
    
    [TestMethod]
    public void ShouldNotUpdateProductBecauseIdInUrlIsDifferent()
    {
        // Given : Un produit à mettre à jour
        Product productToEdit = new()
        {
            NameProduct = "Bureau",
            Description = "Un super bureau",
            NamePhoto = "Un super bureau bleu",
            UriPhoto = "https://ikea.fr/bureau.jpg",
            StockReal = 1
        };
        
        _context.Products.Add(productToEdit);
        _context.SaveChanges();

        productToEdit.NameProduct = "Lit";
        productToEdit.Description = "Un super lit";

        // When : On appelle la méthode PUT du controller pour mettre à jour le produit,
        // mais en précisant un ID différent de celui du produit enregistré
        IActionResult action = _productController.Update(0, productToEdit).GetAwaiter().GetResult();
        
        // Then : On vérifie que l'API renvoie un code BAD_REQUEST (400)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(BadRequestResult));
    }
    
    [TestMethod]
    public void ShouldNotUpdateProductBecauseProductDoesNotExist()
    {
        // Given : Un produit à mettre à jour qui n'est pas enregistré
        Product productToEdit = new()
        {
            IdProduct = 20,
            NameProduct = "Bureau",
            Description = "Un super bureau",
            NamePhoto = "Un super bureau bleu",
            UriPhoto = "https://ikea.fr/bureau.jpg",
            StockReal = 1
        };
        
        // When : On appelle la méthode PUT du controller pour mettre à jour un produit qui n'est pas enregistré
        IActionResult action = _productController.Update(productToEdit.IdProduct, productToEdit).GetAwaiter().GetResult();
        
        // Then : On vérifie que l'API renvoie un code NOT_FOUND (404)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NotFoundResult));
    }
}
