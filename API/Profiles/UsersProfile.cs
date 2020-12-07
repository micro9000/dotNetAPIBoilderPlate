using AutoMapper;
using API_DataAccess.Model;
using API.DTO.User;

namespace API.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            // Source -> Target
            CreateMap<User, ReadUserDTO>();
            CreateMap<User, UserBaseDTO>();
            CreateMap<UserRefreshToken, UserRefreshTokenDTO>();
            CreateMap<CreateUserRequestDTO, User>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom<RolesCustomResolver>());

            CreateMap<UpdateUserRequestDTO, User>()
                .ForAllMembers(x => x.Condition(
                     (src, dest, prop) => {
                         // ignore null & empty string properties
                         if (prop == null) return false;
                         if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                         return true;
                     }
                ));

            CreateMap<RegisterUserRequestDTO, User>();

            CreateMap<Role, RoleDTO>();
        }
    }
}
