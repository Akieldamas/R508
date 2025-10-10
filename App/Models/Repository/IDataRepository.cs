using Microsoft.AspNetCore.Mvc;

namespace App.Models.Repository;

public interface IDataRepository<TEntity, TIdentifier, TKey>
    : ReadableRepository<TEntity, TIdentifier>,
    WriteableRepository<TEntity>,
    SearchableRepository<TEntity, TKey>;

public interface SearchableRepository<TEntity, TKey>
{
    Task<TEntity?> GetByKeyAsync(TKey str);
}

public interface ReadableRepository<TEntity, TIdentifier>
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(TIdentifier id);
}

public interface WriteableRepository<TEntity>
{
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entityToUpdate, TEntity entity);
    Task DeleteAsync(TEntity entity);
}