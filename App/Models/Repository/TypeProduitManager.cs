using App.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Models.Repository;

//public class TypeProduitManager(AppDbContext context) : GenericManager<TypeProduit>, IDataRepository<TypeProduit>
//{
//    // Place pour des méthodes spécifiques aux types de produits, aucune en ce moment
//}
public class TypeProduitManager(AppDbContext context) : IDataRepository<TypeProduit>
{
    public async Task<ActionResult<IEnumerable<TypeProduit>>> GetAllAsync()
    {
        return await context.TypeProduits.ToListAsync();
    }

    public async Task<ActionResult<TypeProduit?>> GetByIdAsync(int id)
    {
        return await context.TypeProduits.FindAsync(id);
    }

    public async Task<ActionResult<TypeProduit?>> GetByStringAsync(string str)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(TypeProduit entity)
    {
        await context.TypeProduits.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TypeProduit entityToUpdate, TypeProduit entity)
    {
        context.TypeProduits.Attach(entityToUpdate);
        context.Entry(entityToUpdate).CurrentValues.SetValues(entity);

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TypeProduit entity)
    {
        context.TypeProduits.Remove(entity);
        await context.SaveChangesAsync();
    }
}

