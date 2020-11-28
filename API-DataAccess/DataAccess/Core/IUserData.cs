using API_DataAccess.DataAccess.Internal;
using API_DataAccess.Model;
using System.Collections.Generic;

namespace API_DataAccess.DataAccess.Core
{
    public interface IUserData : IRepository<User>
    {
        User Login(string userName, string password);
        List<User> GetAll_exclude_deleted();
    }
}
