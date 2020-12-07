using API.DTO.User;
using API.Helpers;
using API.SettingModel;
using API_DataAccess.DataAccess.Contracts;
using API_DataAccess.Model;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace API.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IUserData _userData;
        private readonly IUserRefreshTokenData _userRefreshTokenData;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IMapper _mapper;

        public UserAuthService(IUserData userData,
                            IUserRefreshTokenData userRefreshTokenData,
                            IOptions<AuthenticationSettings> authenticationSettings, 
                            IMapper mapper)
        {
            this._userData = userData;
            this._userRefreshTokenData = userRefreshTokenData;
            this._authenticationSettings = authenticationSettings.Value;
            this._mapper = mapper;
        }

        public ReadUserDTO Authenticate(LoginDTO model, string ipAddress)
        {
            var user = this._userData.GetByEmail(model.Email);
            if (user == null || !user.IsVerified || !BC.Verify(model.Password, user.Password))
            {
                throw new AppException("Email or password is incorrect");
            };

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = JwtToken.generateJwtToken(user, _authenticationSettings.Secret);
            var refreshToken = JwtToken.generateRefreshToken(ipAddress, user.Id);

            // save refresh token
            _userRefreshTokenData.Add(refreshToken);

            var userDTO = _mapper.Map<ReadUserDTO>(user);
            userDTO.JwtToken = jwtToken;
            userDTO.RefreshToken = refreshToken.Token;

            return userDTO;
        }

        
        public ReadUserDTO RefreshToken(string token, string ipAddress)
        {
            var refreshToken = _userRefreshTokenData.GetByToken(token);

            if (refreshToken == null) return null;
            if (!refreshToken.IsActive) return null;
            if (refreshToken.UserData == null) return null;

            var newRefreshToken = JwtToken.generateRefreshToken(ipAddress, refreshToken.UserId);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            // save the new token and update last token refresh token
            _userRefreshTokenData.Add(newRefreshToken);
            _userRefreshTokenData.Update(refreshToken);

            // generate new jwt
            var jwtToken = JwtToken.generateJwtToken(refreshToken.UserData, this._authenticationSettings.Secret);

            var userDTO = _mapper.Map<ReadUserDTO>(refreshToken.UserData);
            userDTO.JwtToken = jwtToken;
            userDTO.RefreshToken = refreshToken.Token;

            return userDTO;
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            var refreshToken = _userRefreshTokenData.GetByToken(token);

            if (refreshToken == null) return false;
            if (!refreshToken.IsActive) return false;
            if (refreshToken.UserData == null) return false;

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            _userRefreshTokenData.Update(refreshToken);

            return true;
        }
    }
}
