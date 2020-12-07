using API.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IUserService
    {
        Task Register(RegisterUserRequestDTO model, string origin);
        bool VerifyEmail(string token);
        Task ForgotPassword(ForgotPasswordRequestDTO model, string origin);
        bool ValidateResetToken(ValidateResetTokenRequestDTO model);
        bool ResetPassword(ResetPasswordRequestDTO model);
        ReadUserDTO Create(CreateUserRequestDTO model);
        ReadUserDTO Update(long userId, UpdateUserRequestDTO model);
        void Delete(long id);
    }
}
