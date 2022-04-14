using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RideshareApp.DataAccess.EFCore;
using RideshareApp.DataAccess.EFCore.Infrastructure.Contracts;
using RideshareApp.DTO.User;
using RideshareApp.Entities;
using RideshareApp.Services.Infrastructure.Services;

namespace RideshareApp.Services
{
    public class RoleService : BaseAsyncService<RoleDTO, RoleDTO, Role, int, DataContext>, IRoleService
    {
        protected readonly UserManager<User> userManager;
        protected readonly RoleManager<Role> roleManager;
        protected readonly IMapper _mapper;

        public RoleService(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IRoleRepository roleRepository, 
            IMapper mapper) : base(roleRepository, mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IdentityResult> AssignToRole(int userId, string roleName)
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                var user = await userManager.FindByIdAsync(userId.ToString());
                return await userManager.AddToRoleAsync(user, roleName);
            }

            return IdentityResult.Failed(new IdentityError { Description = "Invalid role name" });
        }

        public async Task<IdentityResult> UnassignRole(int userId, string roleName)
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                var user = await userManager.FindByIdAsync(userId.ToString());
                return await userManager.RemoveFromRoleAsync(user, roleName);
            }

            return IdentityResult.Failed(new IdentityError { Description = "Invalid role name" });
        }

        public Task<IList<string>> GetRoles(int userId)
        {
            return userManager.GetRolesAsync(new User { Id = userId });
        }

        protected override bool IsChildRecord(int aId, List<string> aParentsList)
        {
            return false;
        }
    }
}
