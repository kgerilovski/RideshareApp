using API.Entities;
using RideshareApp.DTO.User;

namespace RideshareApp.Services.Infrastructure.Services
{
    public interface IUserService : IBaseAsyncService<UserDTO, UserDTO, User, int>
    {
    }
}
