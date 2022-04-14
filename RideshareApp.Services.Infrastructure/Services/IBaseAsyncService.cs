using RideshareApp.Entities;

namespace RideshareApp.Services.Infrastructure.Services
{
    public interface IBaseAsyncService<TInDTO, TOutDTO, TEntity, TPk>
    {
        List<TOutDTO> SelectAllAsync();

        Task SaveEntityAsync(BaseEntity entity, bool applyToAllLevels = true);

        Task<TEntity> SelectAsync(TPk id);

        Task<int> InsertAsync(TInDTO inDto);

        Task UpdateAsync(TPk id, TInDTO inDto);

        Task DeleteAsync(TPk id);
    }
}
