using API_DataAccess.Model;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Helpers
{
    public static class JwtToken
    {
        public static string randomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        //private Claim[] GetUserClaims (User user)
        //{
        //    List<Claim> claims = new List<Claim>();
        //    claims.Add(new Claim("userId", user.Id.ToString()));
        //    claims.Add(new Claim(ClaimTypes.Email, user.Email));

        //    foreach (var role in user.Roles)
        //    {
        //        claims.Add(new Claim(ClaimTypes.Role, role.RoleKey.ToString()));
        //    }

        //    return claims.ToArray();
        //}

        public static string generateJwtToken(User user, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("userId", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static UserRefreshToken generateRefreshToken(string ipAddress, long userId)
        {
            return new UserRefreshToken
            {
                UserId = userId,
                Token = randomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }


        //private UserRefreshToken generateRefreshToken(string ipAddress, long userId)
        //{
        //    using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
        //    {
        //        var randomBytes = new byte[64];
        //        rngCryptoServiceProvider.GetBytes(randomBytes);
        //        return new UserRefreshToken
        //        {
        //            UserId = userId,
        //            Token = Convert.ToBase64String(randomBytes),
        //            Expires = DateTime.UtcNow.AddDays(7),
        //            Created = DateTime.UtcNow,
        //            CreatedByIp = ipAddress
        //        };
        //    }
        //}
    }
}
