using API.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IUserAuthService
    {
        ReadUserDTO Authenticate(LoginDTO model, string ipAddress);
        ReadUserDTO RefreshToken(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);
    }
}
