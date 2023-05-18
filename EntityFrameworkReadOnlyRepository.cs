using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Yrefy.Repositories.Interfaces;

namespace Yrefy.Repositories.Core;

public class EntityFrameworkReadOnlyRepository<TContext> : IReadOnlyRepository where TContext : DbContext
{
    protected readonly TContext Context;

    public EntityFrameworkReadOnlyRepository(TContext context)
    {
        Context = context;
    }

    protected virtual IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class
    {
        return Context.Set<TEntity>().AsNoTracking();
    }

    protected virtual IQueryable<TEntity> GetQueryable<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
    {
        var query = GetQuery<TEntity>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null && includeProperties.Any())
        {
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        return query;
    }

    public IQueryable<TEntity> GetFiltered<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
    {
        var result = Context.Set<TEntity>().Where(filter);
        return result;
    }

    protected virtual IQueryable<TResult> GetProjectedQueryable<TEntity, TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
    {
        return GetQueryable(filter, orderBy, skip, take, includeProperties).Select(selector);
    }

    public virtual IEnumerable<TEntity> GetAll<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
    {
        return GetQueryable(null, orderBy, skip, take, includeProperties).ToArray();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
    {
        return await GetQueryable(null, orderBy, skip, take, includeProperties).ToArrayAsync();
    }

    public virtual IEnumerable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
    {
        return GetQueryable(filter, orderBy, skip, take, includeProperties).ToArray();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
    {
        return await GetQueryable(filter, orderBy, skip, take, includeProperties).ToArrayAsync();
    }

    public virtual async Task<IEnumerable<TResult>> GetAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
    {
        return await GetProjectedQueryable(selector, filter, orderBy, skip, take, includeProperties).ToArrayAsync();
    }

    public virtual TEntity GetOne<TEntity>(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
    {
        return GetQueryable(filter, includeProperties: includeProperties).SingleOrDefault();
    }

    public virtual async Task<TEntity> GetOneAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
    {
        return await GetQueryable(filter, includeProperties: includeProperties).SingleOrDefaultAsync();
    }

    public virtual TEntity GetFirst<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
    {
        return GetQueryable(filter, orderBy, includeProperties: includeProperties).FirstOrDefault();
    }

    public virtual async Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
    {
        return await GetQueryable(filter, orderBy, includeProperties: includeProperties).FirstOrDefaultAsync();
    }

    public virtual async Task<TResult> GetFirstAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
    {
        return await GetProjectedQueryable(selector, filter, orderBy, includeProperties: includeProperties).FirstOrDefaultAsync();
    }

    public virtual TEntity GetById<TEntity>(params object[] ids) where TEntity : class
    {
        return Context.Set<TEntity>().Find(ids);
    }

    public virtual async Task<TEntity> GetByIdAsync<TEntity>(params object[] ids) where TEntity : class
    {
        return await Context.Set<TEntity>().FindAsync(ids);
    }

    public virtual int GetCount<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
    {
        return GetQueryable(filter).Count();
    }

    public virtual async Task<int> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
    {
        return await GetQueryable(filter).CountAsync();
    }

    public virtual bool GetExists<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
    {
        return GetQueryable(filter).Any();
    }

    public virtual async Task<bool> GetExistsAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
    {
        return await GetQueryable(filter).AnyAsync();
    }
}