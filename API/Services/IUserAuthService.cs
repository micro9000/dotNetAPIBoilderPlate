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

        void Register(RegisterUserRequestDTO model, string origin);
        void VerifyEmail(string token);
        void ForgotPassword(ForgotPasswordRequestDTO model, string origin);
        void ValidateResetToken(ValidateResetTokenRequestDTO model);
        void ResetPassword(ResetPasswordRequestDTO model);
        AccountResponse Create(CreateRequest model);
        AccountResponse Update(int id, UpdateRequest model);
        void Delete(int id);
    }
}
