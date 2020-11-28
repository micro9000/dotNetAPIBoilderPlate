using API.DTO.User;
using API.SettingModel;
using API_DataAccess.DataAccess.Core;
using API_DataAccess.Model;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
            _userData = userData;
            _userRefreshTokenData = userRefreshTokenData;
            _authenticationSettings = authenticationSettings.Value;
            _mapper = mapper;
        }

        public ReadUserDTO Authenticate(LoginDTO model, string ipAddress)
        {
            var user = this._userData.Login(model.UserName, model.Password);

            if (user == null) return null;

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = generateJwtToken(user);
            var refreshToken = generateRefreshToken(ipAddress, user.Id);

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

            var newRefreshToken = generateRefreshToken(ipAddress, refreshToken.UserId);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            // save the new token and update last token refresh token
            _userRefreshTokenData.Add(newRefreshToken);
            _userRefreshTokenData.Update(refreshToken);

            // generate new jwt
            var jwtToken = generateJwtToken(refreshToken.UserData);

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


        // helper methods

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authenticationSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private UserRefreshToken generateRefreshToken(string ipAddress, long userId)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new UserRefreshToken
                {
                    UserId = userId,
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
    }
}
