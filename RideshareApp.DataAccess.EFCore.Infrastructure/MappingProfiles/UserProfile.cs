using API.Entities;
using AutoMapper;
using RideshareApp.DTO.User;
using RideshareApp.Entities;

namespace RideshareApp.DataAccess.EFCore.Infrastructure.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();

            CreateMap<UserRole, UserRoleDTO>().ReverseMap()
               .ForMember(d => d.User, opt => opt.Ignore())
               .ForMember(d => d.Role, opt => opt.Ignore());
        }
    }
}
