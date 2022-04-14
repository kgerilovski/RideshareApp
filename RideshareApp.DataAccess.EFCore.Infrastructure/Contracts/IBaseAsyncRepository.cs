using Microsoft.EntityFrameworkCore;

namespace RideshareApp.DataAccess.EFCore.Infrastructure.Contracts
{
    public interface IBaseAsyncRepository<TEntity, TPk, TContext>
    where TEntity : class
    where TPk : struct
    where TContext : DbContext
    {
        IQueryable<TEntity> SelectAllAsync();

        Task<TEntity> SelectAsync(TPk id);

        Task<TEntity> InsertAsync(TEntity aEntity);

        Task<TEntity> UpdateAsync(TEntity aEntity);

        Task<TEntity> DeleteAsync(TPk id);

        TContext GetDbContext();

    }
}
