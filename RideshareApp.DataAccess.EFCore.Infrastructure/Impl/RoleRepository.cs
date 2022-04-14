using Microsoft.EntityFrameworkCore;
using RideshareApp.DataAccess.EFCore.Infrastructure.Contracts;
using RideshareApp.Entities;

namespace RideshareApp.DataAccess.EFCore.Infrastructure.Impl
{
    public class RoleRepository : BaseAsyncRepository<Role, DataContext>, IRoleRepository
    {
        public RoleRepository(DataContext context) : base(context)
        {
        }

        public async Task<Role> GetByName(string name)
        {
            var result = await this.SelectAllAsync().FirstOrDefaultAsync(obj => obj.Name == name);
            return result;
        }
    }
}
