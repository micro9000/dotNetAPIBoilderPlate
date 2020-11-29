using AutoMapper;
using API_DataAccess.Model;
using API.DTO.User;

namespace API.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<User, ReadUserDTO>();
            CreateMap<User, UserBaseDTO>();
            CreateMap<UserRefreshToken, UserRefreshTokenDTO>();
        }
    }
}
