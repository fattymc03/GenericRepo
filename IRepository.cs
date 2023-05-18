using System.Threading.Tasks;

namespace Yrefy.Repositories.Interfaces;

public interface IRepository : IReadOnlyRepository
{
    void SetUsername(string username);
    void ResetUsername();
    void Create<TEntity>(TEntity entity) where TEntity : class;
    void Create<TEntity>(TEntity[] entities) where TEntity : class;
    void Update<TEntity>(TEntity entity) where TEntity : class;
    void Update<TEntity>(TEntity[] entities) where TEntity : class;
    void Upsert<TEntity>(TEntity entity, bool? isUpdate = null) where TEntity : class;
    void Upsert<TEntity>(TEntity[] entities, bool? isUpdate = null) where TEntity : class;
    bool Delete<TEntity>(params object[] ids) where TEntity : class;
    bool Delete<TEntity>(TEntity entity) where TEntity : class;
    int Save();
    Task<int> SaveAsync();
}