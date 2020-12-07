using API_DataAccess.DataAccess.Internal;
using API_DataAccess.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_DataAccess.DataAccess.Contracts
{
    public interface IUserData : IRepository<User>
    {
        long GetCount();
        User GetById(long id);
        User GetByEmail(string email);
        User GetByUsername(string username);
        User GetByResetToken(string token);
        User GetByVerificationToken(string token);
        List<User> GetAll_exclude_deleted();
    }
}
