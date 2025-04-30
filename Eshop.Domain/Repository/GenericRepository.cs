using Eshop.Domain.Context;
using Eshop.Domain.Entites.Common;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Domain.Repository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity :BaseEntity
{

    #region Fields

    private readonly DatabaseContext _context;
    private readonly DbSet<TEntity> _dbset;

    #endregion

    #region Constructor

    public GenericRepository(DatabaseContext context)
    {
        _context = context;
        _dbset = context.Set<TEntity>();
    }

    #endregion

    public IQueryable<TEntity> GetQuery()
    {
        return _dbset.AsQueryable();
    }

    public async Task AddEntity(TEntity entity)
    {
        entity.CreateDate = DateTime.Now;
        entity.LastUpdateDate = entity.CreateDate;
        await _dbset.AddAsync(entity);
    }

    public async Task AddRangeEntities(List<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            await AddEntity(entity);
        }
    }

    public async Task<TEntity> GetEntityById(long entityId)
    {
        return await _dbset.SingleOrDefaultAsync(x => x.Id == entityId);
    }

    public void EditEntity(TEntity entity)
    {
        entity.LastUpdateDate = DateTime.Now;
        _dbset.Update(entity);
    }

    public void EditEntityByUser(TEntity entity, string username)
    {
        entity.LastUpdateDate = DateTime.Now;
        entity.UserName = username;
        _dbset.Update(entity);
    }

    public void DeleteEntity(TEntity entity)
    {
        entity.IsDelete = true;
        EditEntity(entity);
    }

    public async Task DeleteEntityById(long entityId)
    {
        var entity = await GetEntityById(entityId);
        if (entity != null)
        {
            DeleteEntity(entity);
        }
    }

    public void DeletePermanent(TEntity entity)
    {
        _dbset.Remove(entity);
    }

    public void DeletePermanentEntities(List<TEntity> entities)
    {
        _dbset.RemoveRange(entities);
    }

    public void DeletePhysically(TEntity entity)
    {
        _dbset.Remove(entity);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }



    #region Dispose

    public async ValueTask DisposeAsync()
    {
        // TODO release managed resources here
    }

    #endregion

   
}