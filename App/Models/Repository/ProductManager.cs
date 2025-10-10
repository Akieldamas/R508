using App.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Models.Repository;

//public class ProductManager(AppDbContext context) : GenericManager<Produit>, IDataRepository<Produit>
//{
//    // Place pour des méthodes spécifiques aux produits, aucune en ce moment
//}

public class ProductManager(AppDbContext context): IDataRepository<Product, int, string>
{

    //public ProductManager(AppDbContext context) : base(context) { }
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await context.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<Product?> GetByKeyAsync(string str)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(Product entity)
    {
        await context.Products.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product entityToUpdate, Product entity)
    {
        context.Products.Attach(entityToUpdate);
        context.Entry(entityToUpdate).CurrentValues.SetValues(entity);

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product entity)
    {
        context.Products.Remove(entity);
        await context.SaveChangesAsync();
    }
}