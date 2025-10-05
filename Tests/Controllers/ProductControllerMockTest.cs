using System;
using System.Collections.Generic;
using System.Linq;
using App.Controllers;
using App.DTO;
using App.Mapper;
using AutoMapper;
using App.Models;
using App.Models.Repository;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using App.Models.EntityFramework;

namespace Tests.Controllers;

[TestClass]
[TestSubject(typeof(ProductController))]
[TestCategory("mock")]
public class ProductControllerMockTest
{
    private readonly AppDbContext context;
    private readonly ProductController _productController;
    private readonly Mock<IDataRepository<Product>>  _productManager;
    private readonly IMapper _mapper;
    
    public ProductControllerMockTest()
    {
        context = new AppDbContext();

        _productManager = new Mock<IDataRepository<Product>>();
        var config = new MapperConfiguration(cfg => { 
            cfg.AddProfile<MapperProfile>();
        }, new LoggerFactory());

        _mapper = config.CreateMapper();
        _productController = new ProductController(_mapper, _productManager.Object);
    }


    //[TestMethod]
    //public void ShouldGetProduct()
    //{
    //    // Given : Un produit en enregistré
    //    Produit produitInDb = new()
    //    {
    //        IdProduct = 30,
    //        NameProduct = "Chaise",
    //        Description = "Une superbe chaise",
    //        NamePhoto = "Une superbe chaise bleu",
    //        UriPhoto = "https://ikea.fr/chaise.jpg",
    //        StockReal = 1
    //    };

    //    _produitManager
    //        .Setup(manager => manager.GetByIdAsync(produitInDb.IdProduct))
    //        .ReturnsAsync(produitInDb);

    //    // When : On appelle la méthode GET de l'API pour récupérer le produit
    //    ActionResult<Produit> action = _productController.Get(produitInDb.IdProduct).GetAwaiter().GetResult();

    //    // Then : On récupère le produit et le code de retour est 200
    //    _produitManager.Verify(manager => manager.GetByIdAsync(produitInDb.IdProduct), Times.Once);

    //    Assert.IsNotNull(action);
    //    Assert.IsInstanceOfType(action.Value, typeof(Produit));

    //    Produit returnProduct = action.Value;
    //    Assert.AreEqual(_mapper.Map<Produit>(produitInDb), returnProduct);
    //}     //} (Problème avec Produit due to blazor) => IEnumerable<Produit> n'est pas reconnu

    [TestMethod]
    public void ShouldDeleteProduct()
    {
        // Given : Un produit enregistré
        Product productInDb = new()
        {
            IdProduct = 20,
            NameProduct = "Chaise",
            Description = "Une superbe chaise",
            NamePhoto = "Une superbe chaise bleu",
            UriPhoto = "https://ikea.fr/chaise.jpg"
        };

        _productManager
            .Setup(manager => manager.GetByIdAsync(productInDb.IdProduct))
            .ReturnsAsync(productInDb);

        _productManager
            .Setup(manager => manager.DeleteAsync(productInDb));

        // When : On souhaite supprimer un produit depuis l'API
        IActionResult action = _productController.Delete(productInDb.IdProduct).GetAwaiter().GetResult();
        
        // Then : Le produit a bien été supprimé et le code HTTP est NO_CONTENT (204)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NoContentResult));

        _productManager.Verify(manager => manager.GetByIdAsync(productInDb.IdProduct), Times.Once);
        _productManager.Verify(manager => manager.DeleteAsync(productInDb), Times.Once);
    }
    
    [TestMethod]
    public void ShouldNotDeleteProductBecauseProductDoesNotExist()
    {
        // Given : Un produit enregistré
        Product productInDb = new()
        {
            IdProduct = 30,
            NameProduct = "Chaise",
            Description = "Une superbe chaise",
            NamePhoto = "Une superbe chaise bleu",
            UriPhoto = "https://ikea.fr/chaise.jpg"
        };
        
        _productManager
            .Setup(manager => manager.GetByIdAsync(productInDb.IdProduct))
            .ReturnsAsync((Product)null);
        
        // When : On souhaite supprimer un produit depuis l'API
        IActionResult action = _productController.Delete(productInDb.IdProduct).GetAwaiter().GetResult();
        
        // Then : Le produit a bien été supprimé et le code HTTP est NO_CONTENT (204)
        _productManager.Verify(manager => manager.GetByIdAsync(productInDb.IdProduct), Times.Once);

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

    //    _produitManager
    //        .Setup(manager => manager.GetAllAsync())
    //        .ReturnsAsync(new ActionResult<IEnumerable<Produit>>(productInDb));

    //    // When : On souhaite récupérer tous les produits
    //    ActionResult<IEnumerable<Produit>> products = _productController.GetAll().GetAwaiter().GetResult();

    //    // Then : Tous les produits sont récupérés
    //    Assert.IsNotNull(products);
    //    Assert.IsInstanceOfType(products.Value, typeof(IEnumerable<Produit>));
    //    Assert.IsTrue(productInDb.SequenceEqual(products.Value));

    //    _produitManager.Verify(manager => manager.GetAllAsync(), Times.Once);
    //}     //} (Problème avec Produit due to blazor) => IEnumerable<Produit> n'est pas reconnu

    [TestMethod]
    public void GetProductShouldReturnNotFound()
    {
        //Given : Pas de produit trouvé par le manager
        //_productManager
        //    .Setup(manager => manager.GetByIdAsync(30))
        //    .ReturnsAsync(new ActionResult<ProductDetailsDTO>((ProductDetailsDTO)null));
        
        //// When : On appelle la méthode get de mon api pour récupérer le produit
        //ActionResult<Product> action = _productController.Get(30).GetAwaiter().GetResult();
        
        //// Then : On ne renvoie rien et on renvoie NOT_FOUND (404)
        //Assert.IsInstanceOfType(action.Result, typeof(NotFoundResult), "Ne renvoie pas 404");
        //Assert.IsNull(action.Value, "Le produit n'est pas null");
        
        _productManager.Verify(manager => manager.GetByIdAsync(30), Times.Once);
    }
    
    [TestMethod]
    public void ShouldCreateProduct()
    {
        // Given : Un produit a enregistré
        Product productToInsert = new()
        {
            IdProduct = 30,
            NameProduct = "Chaise",
            Description = "Une superbe chaise",
            NamePhoto = "Une superbe chaise bleu",
            UriPhoto = "https://ikea.fr/chaise.jpg"
        };

        _productManager
            .Setup(manager => manager.AddAsync(productToInsert));
        
        // When : On appel la méthode POST de l'API pour enregistrer le produit
        ActionResult<Product> action = _productController.Create(productToInsert).GetAwaiter().GetResult();
        
        // Then : Le produit est bien enregistré et le code renvoyé et CREATED (201)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action.Result, typeof(CreatedAtActionResult));

        _productManager.Verify(manager => manager.AddAsync(productToInsert), Times.Once);
    }

    [TestMethod]
    public void ShouldUpdateProduct()
    {
        // Given : Un produit à mettre à jour
        Product produitToEdit = new()
        {
            IdProduct = 20,
            NameProduct = "Bureau",
            Description = "Un super bureau",
            NamePhoto = "Un super bureau bleu",
            UriPhoto = "https://ikea.fr/bureau.jpg"
        };
        
        // Une fois enregistré, on modifie certaines propriétés 
        Product updatedProduit = new()
        {
            IdProduct = 20,
            NameProduct = "Lit",
            Description = "Un super lit",
            NamePhoto = "Un super bureau bleu",
            UriPhoto = "https://ikea.fr/bureau.jpg"
        };

        _productManager
            .Setup(manager => manager.GetByIdAsync(produitToEdit.IdProduct))
            .ReturnsAsync(produitToEdit);

        _productManager
            .Setup(manager => manager.UpdateAsync(produitToEdit, updatedProduit));
        
        // When : On appelle la méthode PUT du controller pour mettre à jour le produit
        IActionResult action = _productController.Update(produitToEdit.IdProduct, produitToEdit).GetAwaiter().GetResult();
        
        // Then : On vérifie que le produit a bien été modifié et que le code renvoyé et NO_CONTENT (204)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NoContentResult));
        
        _productManager.Verify(manager => manager.GetByIdAsync(produitToEdit.IdProduct), Times.Once);
        _productManager.Verify(manager => manager.UpdateAsync(produitToEdit, It.IsAny<Product>()), Times.Once);
    }
    
    [TestMethod]
    public void ShouldNotUpdateProductBecauseIdInUrlIsDifferent()
    {
        // Given : Un produit à mettre à jour
        Product produitToEdit = new()
        {
            IdProduct = 20,
            NameProduct = "Bureau",
            Description = "Un super bureau",
            NamePhoto = "Un super bureau bleu",
            UriPhoto = "https://ikea.fr/bureau.jpg"
        };
        

        // When : On appelle la méthode PUT du controller pour mettre à jour le produit,
        // mais en précisant un ID différent de celui du produit enregistré
        IActionResult action = _productController.Update(1, produitToEdit).GetAwaiter().GetResult();
        
        // Then : On vérifie que l'API renvoie un code BAD_REQUEST (400)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(BadRequestResult));
        
        _productManager.Verify(manager => manager.GetByIdAsync(produitToEdit.IdProduct), Times.Never);
        _productManager.Verify(manager => manager.UpdateAsync(produitToEdit, It.IsAny<Product>()), Times.Never);
    }
    
    [TestMethod]
    public void ShouldNotUpdateProductBecauseProductDoesNotExist()
    {
        // Given : Un produit à mettre à jour qui n'est pas enregistré
        Product produitToEdit = new()
        {
            IdProduct = 20,
            NameProduct = "Bureau",
            Description = "Un super bureau",
            NamePhoto = "Un super bureau bleu",
            UriPhoto = "https://ikea.fr/bureau.jpg"
        };
        
        _productManager
            .Setup(manager => manager.GetByIdAsync(produitToEdit.IdProduct))
            .ReturnsAsync((Product)null);
        
        // When : On appelle la méthode PUT du controller pour mettre à jour un produit qui n'est pas enregistré
        IActionResult action = _productController.Update(produitToEdit.IdProduct, produitToEdit).GetAwaiter().GetResult();
        
        // Then : On vérifie que l'API renvoie un code NOT_FOUND (404)
        Assert.IsNotNull(action);
        Assert.IsInstanceOfType(action, typeof(NotFoundResult));
        
        _productManager.Verify(manager => manager.GetByIdAsync(produitToEdit.IdProduct), Times.Once);
        _productManager.Verify(manager => manager.UpdateAsync(produitToEdit, It.IsAny<Product>()), Times.Never);
    }
}
