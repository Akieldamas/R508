using App.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Models.Repository;

//public class TypeProduitManager(AppDbContext context) : GenericManager<TypeProduit>, IDataRepository<TypeProduit>
//{
//    // Place pour des méthodes spécifiques aux types de produits, aucune en ce moment
//}
public class ProductTypeManager(AppDbContext context): IDataRepository<ProductType, int, string>
{
    //public ProductTypeManager(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<ProductType>> GetAllAsync()
    {
        return await context.ProductTypes.ToListAsync();
    }

    public async Task<ProductType?> GetByIdAsync(int id)
    {
        return await context.ProductTypes.FindAsync(id);
    }

    public async Task<ProductType?> GetByKeyAsync(string str)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(ProductType entity)
    {
        await context.ProductTypes.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProductType entityToUpdate, ProductType entity)
    {
        context.ProductTypes.Attach(entityToUpdate);
        context.Entry(entityToUpdate).CurrentValues.SetValues(entity);

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProductType entity)
    {
        context.ProductTypes.Remove(entity);
        await context.SaveChangesAsync();
    }
}

