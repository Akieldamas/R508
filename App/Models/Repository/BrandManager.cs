using App.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Models.Repository;

//public class MarqueManager(AppDbContext context) : GenericManager<Marque>, IDataRepository<Marque>
//{
//    // Place pour des méthodes spécifiques aux marques, aucune en ce moment
//}


public class BrandManager(AppDbContext context): IDataRepository<Brand, int, string>
{
    public async Task<IEnumerable<Brand>> GetAllAsync()
    {
        return await context.Brands.ToListAsync();
    }

    public async Task<Brand?> GetByIdAsync(int id)
    {
        return await context.Brands.FindAsync(id);
    }

    public async Task<Brand?> GetByKeyAsync(string str)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(Brand entity)
    {
        await context.Brands.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Brand entityToUpdate, Brand entity)
    {
        context.Brands.Attach(entityToUpdate);
        context.Entry(entityToUpdate).CurrentValues.SetValues(entity);

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Brand entity)
    {
        context.Brands.Remove(entity);
        await context.SaveChangesAsync();
    }
}

