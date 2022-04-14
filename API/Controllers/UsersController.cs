using API.Controllers.Common;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideshareApp.DTO.User;
using RideshareApp.Services.Infrastructure.Services;

namespace API.Controllers
{
    [AllowAnonymous]
    [Route("users")]
    public class UsersController : BaseApiCrudController<UserDTO, UserDTO, User, int>
    {
        protected readonly IUserService _userService;
        protected readonly IRoleService _roleService;
        protected readonly JwtManager jwtManager;
        protected readonly IAuthenticationService authService;

        public UsersController(IUserService userService, IRoleService roleService, JwtManager jwtManager, IAuthenticationService authService) : base(userService)
        {
            _userService = userService;
            _roleService = roleService;
            this.jwtManager = jwtManager;
            this.authService = authService;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return base.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            return await base.Get(id);
        }
    }
}
