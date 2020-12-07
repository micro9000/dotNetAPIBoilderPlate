using API_DataAccess.DataAccess.Internal;
using API_DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_DataAccess.DataAccess.Contracts
{
    public interface IUserRoleData : IRepository<UserRole>
    {
        UserRole Get(long userRoleId, long userId);
        Role GetRole(long userRoleId, long userId);

        bool Delete(long userId, long userRoleId);

        List<Role> GetRolesByUser(long userId);

        List<User> GetUsersByRole(RoleKey role);


    }
}
