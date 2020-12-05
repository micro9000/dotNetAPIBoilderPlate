using API_DataAccess.DataAccess.Internal;
using API_DataAccess.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_DataAccess.DataAccess.Contracts
{
    public interface IUserData : IRepository<User>
    {
        User Login(string userName, string password);
        User GetById(long id);

        List<User> GetAll_exclude_deleted();

        List<Role> GetRoles(long userId);
    }
}
