using API.Entities;
using Microsoft.EntityFrameworkCore;
using RideshareApp.DataAccess.EFCore.Infrastructure.Contracts;

namespace RideshareApp.DataAccess.EFCore.Infrastructure.Impl
{
    public class UserRepository : BaseAsyncRepository<User, DataContext>, IUserRepository
    {
        public UserRepository(DataContext aDbContext) : base(aDbContext)
        {
        }

        public async Task<User> SelectUserWithRolesAsync(int id)
        {
            var result = await _dbContext.Users.AsNoTracking()
                .Include(u => u.UserRoles)
                    .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(obj => obj.Id == id);
            return result;
        }

        public async Task<User> GetUserByLogin(string login)
        {
            return await _dbContext.Users.AsNoTracking()
                .Include(u => u.UserRoles)
                    .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(obj => obj.Login == login);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _dbContext.Users.AsNoTracking()
                .Include(u => u.UserRoles)
                    .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(obj => obj.Email == email);
        }

        public async Task<IList<User>> GetUsersByRole(int roleId)
        {
            return await _dbContext.Users.AsNoTracking()
                .Include(u => u.UserRoles)
                    .ThenInclude(x => x.Role)
                .Where(x => x.UserRoles.Any(ur => ur.RoleId == roleId))
                .ToArrayAsync();
        }


    }
}
