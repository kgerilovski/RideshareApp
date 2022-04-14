using Microsoft.EntityFrameworkCore;
using RideshareApp.DataAccess.EFCore.Infrastructure.Contracts;
using RideshareApp.Entities;

namespace RideshareApp.DataAccess.EFCore.Infrastructure.Impl
{
    public abstract class BaseAsyncRepository<TEntity, TContext> : IBaseAsyncRepository<TEntity, int, TContext>
        where TEntity : BaseEntity, new()
        where TContext : DbContext
    {
        protected readonly TContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAsyncRepository{T}"/> class.
        /// </summary>
        /// <param name="aDbContext"></param>
        protected BaseAsyncRepository(TContext aDbContext)
        {
            this._dbContext = aDbContext;
        }

        public TContext GetDbContext()
        {
            return this._dbContext;
        }

        public virtual IQueryable<TEntity> SelectAllAsync()
        {
            var result = this._dbContext.Set<TEntity>().AsNoTracking();
            return result;
        }

        public virtual async Task<TEntity> SelectAsync(int id)
        {
            var result = await this._dbContext.Set<TEntity>().AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public virtual async Task<TEntity> InsertAsync(TEntity aEntity)
        {
            _dbContext.Entry(aEntity).State = EntityState.Added;
            await _dbContext.SaveChangesAsync();
            return aEntity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity aEntity)
        {
            _dbContext.Entry(aEntity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return aEntity;
        }

        public virtual async Task<TEntity> DeleteAsync(int id)
        {
            TEntity repoObj = await this.SelectAsync(id);
            _dbContext.Entry(repoObj).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
            return repoObj;
        }
    }
}
