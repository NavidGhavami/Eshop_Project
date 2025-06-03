using Eshop.Domain.Entities.Common;

namespace Eshop.Domain.Repository;

public interface IGenericRepository<TEntity> : IAsyncDisposable where TEntity : BaseEntity
{
    IQueryable<TEntity> GetQuery();
    Task AddEntity(TEntity entity);
    Task AddRangeEntities(List<TEntity>  entities);
    Task<TEntity> GetEntityById(long entityId);
    void EditEntity(TEntity entity);    
    void EditEntityByUser(TEntity entity, string username);
    void DeleteEntity(TEntity entity);
    Task DeleteEntityById(long entityId);
    void DeletePermanent(TEntity entity);
    void DeletePermanentEntities(List<TEntity> entities);
    void DeletePhysically(TEntity entity);
    Task SaveChanges();

}