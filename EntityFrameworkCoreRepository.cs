using Microsoft.EntityFrameworkCore;
using TrackerEnabledDbContext.Core;
using Yrefy.Repositories.Interfaces;

namespace Yrefy.Repositories.Core;

public class EntityFrameworkCoreRepository<TContext> : EntityFrameworkReadOnlyRepository<TContext>, IRepository where TContext : TrackerContext
{
    private readonly TContext _context;

    public EntityFrameworkCoreRepository(TContext context) : base(context)
    {
        _context = context;
        ResetUsername();
    }

    public void SetUsername(string username)
    {
        Context.ConfigureUsername(() => username);
    }

    public void ResetUsername()
    {
        _context.ConfigureUsername(() => string.Empty);
    }

    public virtual void Create<TEntity>(TEntity entity) where TEntity : class
    {
        Context.Set<TEntity>().Add(entity);
    }

    public virtual void Create<TEntity>(TEntity[] entities) where TEntity : class
    {
        Context.Set<TEntity>().AddRange(entities);
    }

    public virtual void Update<TEntity>(TEntity entity) where TEntity : class
    {
        Context.Set<TEntity>().Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Update<TEntity>(TEntity[] entities) where TEntity : class
    {
        Context.Set<TEntity>().AttachRange(entities);
        foreach (var entity in entities)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }
    }

    public virtual void Upsert<TEntity>(TEntity[] entities, bool? isUpdate = null) where TEntity : class
    {
        foreach (var entity in entities)
        {
            Upsert(entity, isUpdate);
        }
    }

    public virtual void Upsert<TEntity>(TEntity entity, bool? isUpdate = null) where TEntity : class
    {
        if ((isUpdate ?? false) || Context.Set<TEntity>().Any(x => x == entity))
        {
            Context.Set<TEntity>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }
        else
        {
            Context.Set<TEntity>().Add(entity);
        }
    }

    public virtual bool Delete<TEntity>(params object[] ids) where TEntity : class
    {
        var r = Context.Set<TEntity>().Find(ids);

        return Delete(r);
    }

    public virtual bool Delete<TEntity>(TEntity entity) where TEntity : class
    {
        if (entity == null)
        {
            return false;
        }

        var dbSet = Context.Set<TEntity>();
        if (Context.Entry(entity).State == EntityState.Detached)
        {
            dbSet.Attach(entity);
        }

        dbSet.Remove(entity);

        return true;
    }

    public virtual int Save()
    {
        var recordsUpdated = Context.SaveChanges();

        Context.ChangeTracker.Clear();

        return recordsUpdated;
    }

    public virtual async Task<int> SaveAsync()
    {
        var recordsUpdated = await Context.SaveChangesAsync();

        Context.ChangeTracker.Clear();

        return recordsUpdated;
    }
}