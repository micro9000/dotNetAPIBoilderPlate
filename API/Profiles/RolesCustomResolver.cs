using API.DTO.User;
using API_DataAccess.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Profiles
{
    public class RolesCustomResolver : IValueResolver<CreateUserRequestDTO, User, List<Role>>
    {
        public List<Role> Resolve(CreateUserRequestDTO source, User destination, List<Role> destMember, ResolutionContext context)
        {
            var rolesArr = source.Roles;
            var roleList = new List<Role>();

            foreach(var role in rolesArr)
            {
                roleList.Add(new Role { RoleKey = role });
            }

            return roleList;
        }
    }
}
