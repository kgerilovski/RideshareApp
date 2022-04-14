using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RideshareApp.DataAccess.EFCore;
using RideshareApp.DataAccess.EFCore.Infrastructure.Contracts;
using RideshareApp.DTO.User;
using RideshareApp.Services.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideshareApp.Services
{
    public class UserService : BaseAsyncService<UserDTO, UserDTO, User, int, DataContext>, IUserService
    {
        protected readonly IUserRepository userRepository;
        protected readonly UserManager<User> _userManager;
        protected readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, UserManager<User> userManager, IMapper mapper)
            : base(userRepository, mapper)
        {
            this.userRepository = userRepository;
            this._userManager = userManager;
            _mapper = mapper;
        }

        protected override bool IsChildRecord(int id, List<string> aParentsList)
        {
            return false;
        }
    }
}
