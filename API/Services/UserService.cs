using API.DTO.User;
using API.Helpers;
using API.SettingModel;
using API_DataAccess.DataAccess.Contracts;
using API_DataAccess.Model;
using AutoMapper;
using EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace API.Services
{
    public class UserService : IUserService
    {

        private readonly IUserData _userData;
        private readonly IRoleData _roleData;
        private readonly IUserRoleData _userRoleData;
        private readonly IUserRefreshTokenData _userRefreshTokenData;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public UserService(IUserData userData,
                            IRoleData roleData,
                            IUserRoleData userRoleData,
                            IUserRefreshTokenData userRefreshTokenData,
                            IOptions<AuthenticationSettings> authenticationSettings,
                            IMapper mapper,
                            IEmailSender emailSender)
        {
            this._userData = userData;
            this._roleData = roleData;
            this._userRoleData = userRoleData;
            this._userRefreshTokenData = userRefreshTokenData;
            this._authenticationSettings = authenticationSettings.Value;
            this._mapper = mapper;
            this._emailSender = emailSender;
        }

        public async Task Register(RegisterUserRequestDTO model, string origin)
        {
            var user = _userData.GetByEmail(model.Email);

            if (user != null)
            {
                await SendAlreadyRegisteredEmail(user.Email, origin);
                return;
            }

            var userWithSameUsername = _userData.GetByUsername(model.Username);

            if (userWithSameUsername != null) throw new AppException("Username already taken, please use different username.");

            var newUser = _mapper.Map<User>(model);

            //first registered user is an admin
            bool isFirstAccount = _userData.GetCount() == 0;
            if (isFirstAccount)
                newUser.Roles.Add(new Role { RoleKey = RoleKey.admin });
            else
                newUser.Roles.Add(new Role { RoleKey = RoleKey.superuser });

            newUser.CreatedAt = DateTime.UtcNow;
            newUser.VerificationToken = JwtToken.randomTokenString();

            newUser.Password = BC.HashPassword(model.Password);

            _userData.Add(newUser);

            await SendVerificationEmail(newUser, origin);

        }

        public bool VerifyEmail(string token)
        {
            var user = _userData.GetByVerificationToken(token);

            if (user == null) throw new AppException("Verification failed");

            user.VerifiedAt = DateTime.UtcNow;
            user.VerificationToken = null;

            return _userData.Update(user);
        }

        public ReadUserDTO Create(CreateUserRequestDTO model)
        {
            // validate
            if (_userData.GetByUsername(model.Username) != null)
                throw new AppException($"Username '{model.Username}' is already registered");
            if (_userData.GetByEmail(model.Email) != null)
                throw new AppException($"Email '{model.Email}' is already registered");

            if (model.Roles.Length == 0)
            {
                List<RoleKey> defaultRole = new List<RoleKey>();
                defaultRole.Add(RoleKey.superuser);
                // Default Role
                model.Roles = defaultRole.ToArray();
            }

            // map model to new account object
            var user = _mapper.Map<User>(model);

            user.AcceptTerms = true;
            user.IsVerified = true;
            user.VerifiedAt = DateTime.UtcNow;
            user.Password = BC.HashPassword(model.Password);

            var userId = _userData.Add(user);

            if (userId > 0)
            {
                foreach (var role in user.Roles)
                {
                    var roleDetails = this._roleData.GetByKey(role.RoleKey.ToString());
                    if (roleDetails != null)
                    {
                        var userRole = new UserRole { UserId = userId, RoleId = roleDetails.Id };
                        this._userRoleData.Add(userRole);
                    }
                }
            }

            return _mapper.Map<ReadUserDTO>(user);
        }

        public void Delete(long id)
        {
            var user = _userData.Get(id);

            if (user == null)
                throw new AppException($"User not found");

            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;

            if (_userData.Update(user) == false)
            {
                throw new AppException($"Unable to delete");
            }
        }

        public async Task ForgotPassword(ForgotPasswordRequestDTO model, string origin)
        {
            var user = _userData.GetByEmail(model.Email);

            if (user == null) return;

            // create reset token that expires after 1 day
            user.ResetToken = JwtToken.randomTokenString();
            user.ResetTokenExpiresAt = DateTime.UtcNow.AddHours(24);

            _userData.Update(user);

            await SendPasswordResetEmail(user, origin);
        }

        public bool ValidateResetToken(ValidateResetTokenRequestDTO model)
        {
            var user = _userData.GetByResetToken(model.Token);

            if (user == null) throw new AppException("Invalid token");

            if (user.ResetTokenExpiresAt < DateTime.UtcNow) throw new AppException("Invalid token");

            return true;
        }

        public bool ResetPassword(ResetPasswordRequestDTO model)
        {
            var user = _userData.GetByResetToken(model.Token);
            if (user == null) throw new AppException("Invalid token");
            if (user.ResetTokenExpiresAt < DateTime.UtcNow) throw new AppException("Invalid token");

            user.Password = BC.HashPassword(model.Password);
            user.PasswordResetAt = DateTime.UtcNow;
            user.ResetToken = null;
            user.ResetTokenExpiresAt = DateTime.MinValue;

            return _userData.Update(user);
        }


        public ReadUserDTO Update(long userId, UpdateUserRequestDTO model)
        {
            var user = GetUserDetails(userId);

            // validate email
            if (model.Email != user.Email && _userData.GetByEmail(model.Email) != null)
                throw new AppException($"Email '{model.Email}' is already taken");

            // hash password if it was entered
            if (string.IsNullOrEmpty(model.Password) == false)
                user.Password = BC.HashPassword(model.Password);

            _mapper.Map(model, user);
            user.UpdatedAt = DateTime.UtcNow;

            this._userData.Update(user);

            return _mapper.Map<ReadUserDTO>(user);
        }



        // Helper methods

        private User GetUserDetails(long userId)
        {
            var user = this._userData.GetById(userId);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }


        private async Task SendVerificationEmail(User user, string origin)
        {
            string message;

            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/account/verify-email?token={user.VerificationToken}";
                message = $@"<p>Please click the below link to verify your email address:</p>
                             <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                             <p><code>{user.VerificationToken}</code></p>";
            }

            //, IFormFileCollection files
            var resetPassMessage = new Message(
                    new string[] { user.Email },
                    "Sign-up Verification API - Verify Email",
                    message,
                    new FormFileCollection());
            await _emailSender.SendEmailAsync(resetPassMessage);
        }


        private async Task SendAlreadyRegisteredEmail(string email, string origin)
        {
            string message;

            if (!string.IsNullOrEmpty(origin))
            {
                message = $@"<p>If you don't know your password please visit the <a href=""{origin}/account/forgot-password"">forgot password</a> page.</p>";
            }
            else
            {
                message = "<p>If you don't know your password you can reset it via the <code>/accounts/forgot-password</code> api route.</p>";
            }

            //, IFormFileCollection files
            var resetPassMessage = new Message(
                    new string[] { email },
                    "Sign-up Verification API - Email already registered",
                    message,
                    new FormFileCollection());
            await _emailSender.SendEmailAsync(resetPassMessage);
        }


        private async Task SendPasswordResetEmail (User user, string origin)
        {
            string message;

            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/account/reset-password?token={user.ResetToken}";
                message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                             <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to reset your password with the <code>/accounts/reset-password</code> api route:</p>
                             <p><code>{user.ResetToken}</code></p>";
            }

            //, IFormFileCollection files
            var resetPassMessage = new Message(
                    new string[] { user.Email }, 
                    "Reset password email",
                    message,
                    new FormFileCollection());
            await _emailSender.SendEmailAsync(resetPassMessage);
        }

    }
}
