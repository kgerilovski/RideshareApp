using Microsoft.AspNetCore.Identity;
using RideshareApp.DTO.User;
using RideshareApp.Entities;

namespace RideshareApp.Services.Infrastructure.Services
{
    public interface IRoleService : IBaseAsyncService<RoleDTO, RoleDTO, Role, int>
    {
        Task<IdentityResult> AssignToRole(int userId, string roleName);
        Task<IdentityResult> UnassignRole(int userId, string roleName);
        Task<IList<string>> GetRoles(int userId);
    }
}
