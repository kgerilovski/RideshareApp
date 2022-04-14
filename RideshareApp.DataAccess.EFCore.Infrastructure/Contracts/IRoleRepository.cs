using RideshareApp.Entities;

namespace RideshareApp.DataAccess.EFCore.Infrastructure.Contracts
{
    public interface IRoleRepository : IBaseAsyncRepository<Role, int, DataContext>
    {
        Task<Role> GetByName(string name);
    }
}
