using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Yrefy.Repositories.Interfaces;

public interface IReadOnlyRepository
{
        
    IQueryable<TEntity> GetFiltered<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class;
    IEnumerable<TEntity> GetAll<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class;
    Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class;
    IEnumerable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class;
    Task<IEnumerable<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class;
    Task<IEnumerable<TResult>> GetAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class;
    TEntity GetOne<TEntity>(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class;
    Task<TEntity> GetOneAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class;
    TEntity GetFirst<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class;
    Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class;
    Task<TResult> GetFirstAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class;
    TEntity GetById<TEntity>(params object[] ids) where TEntity : class;
    Task<TEntity> GetByIdAsync<TEntity>(params object[] ids) where TEntity : class;
    int GetCount<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class;
    Task<int> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class;
    bool GetExists<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class;
    Task<bool> GetExistsAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class;
}