using App.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Models.Repository;

//public abstract class GenericManager(AppDbContext context): IDataRepository<TEntity, int, string>
//{

//    public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetAllAsync()
//    {
//        return await context.Set<TEntity>().ToListAsync();
//    }

//    public virtual async Task<ActionResult<TEntity?>> GetByIdAsync(int id)
//    {
//        return await context.Set<TEntity>().FindAsync(id);
//    }

//    public virtual async Task<ActionResult<TEntity?>> GetByKeyAsync(string str)
//    {
//        throw new NotImplementedException();
//    }

//    public virtual async Task AddAsync(TEntity entity)
//    {
//        await context.Set<TEntity>().AddAsync(entity);
//        await context.SaveChangesAsync();
//    }

//    public virtual async Task UpdateAsync(TEntity entityToUpdate, TEntity entity)
//    {
//        context.Set<TEntity>().Attach(entityToUpdate);
//        context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
//        await context.SaveChangesAsync();
//    }

//    public virtual async Task DeleteAsync(TEntity entity)
//    {
//        context.Set<TEntity>().Remove(entity);
//        await context.SaveChangesAsync();
//    }
//}
