using System.Security.Principal;

namespace OnlinePreferance2_api.Database.Repositories
{
    public interface IGenericRepository<TEntity>
     where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> GetById(int id);

        Task Create(TEntity entity);

        Task Update(int id, TEntity entity);

        Task Delete(int id);
    }
}
