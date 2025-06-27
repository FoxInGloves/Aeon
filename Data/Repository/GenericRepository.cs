using System.Linq.Expressions;
using Aeon_Web.Data.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Aeon_Web.Data.Repository;

public class GenericRepository<TEntity>(ApplicationDbContext context) : IGenericRepository<TEntity>
    where TEntity : class
{
    protected readonly DbSet<TEntity> EntityDbSet = context.Set<TEntity>();

    public IQueryable<TEntity> GetQuery()
    {
        return EntityDbSet.AsQueryable();
    }
    
    public async Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
        IQueryable<TEntity> query = EntityDbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return orderBy != null ? await orderBy(query).ToListAsync() : await query.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(object id)
    {
        return await EntityDbSet.FindAsync(id);
    }

    public async Task CreateAsync(TEntity entity)
    {
        await EntityDbSet.AddAsync(entity);
    }

    public virtual async Task Delete(object id)
    {
        var entityToDelete = await GetByIdAsync(id);
        if (entityToDelete == null) return;
        Delete(entityToDelete);
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        if (context.Entry(entityToDelete).State == EntityState.Detached)
        {
            EntityDbSet.Attach(entityToDelete);
        }

        EntityDbSet.Remove(entityToDelete);
    }

    public async Task UpdateAsync(TEntity entityToUpdate)
    {
        await Task.Run(() =>
        {
            EntityDbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        });
    }
}