using System.Linq.Expressions;

namespace Aeon_Web.Data.Repository.Abstractions;

public interface IGenericRepository<TEntity> where TEntity : class
{
    public Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);

    public Task<TEntity?> GetByIdAsync(object id);

    public Task CreateAsync(TEntity entity);

    public Task DeleteAsync(object id);

    public Task DeleteAsync(TEntity entityToDelete);

    public Task UpdateAsync(TEntity entityToUpdate);
}