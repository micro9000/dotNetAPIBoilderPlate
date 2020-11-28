using API_DataAccess.DataAccess.Internal;
using API_DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_DataAccess.DataAccess.Core
{
    public interface IUserRefreshTokenData : IRepository<UserRefreshToken>
    {
        UserRefreshToken GetByToken(string token);

        UserRefreshToken GetByUser(long userId);

        List<UserRefreshToken> GetAllByUser(long userId);
    }
}
