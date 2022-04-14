using API.Entities;


namespace RideshareApp.DataAccess.EFCore.Infrastructure.Contracts
{
    public interface IUserRepository : IBaseAsyncRepository<User, int, DataContext>
    {
        Task<User> SelectUserWithRolesAsync(int id);
        Task<User> GetUserByLogin(string login);
        Task<User> GetUserByEmail(string email);
        Task<IList<User>> GetUsersByRole(int roleId);
    }
}
