using API.SettingModel;
using API_DataAccess.DataAccess.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthenticationSettings _authSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AuthenticationSettings> authSettings)
        {
            _next = next;
            _authSettings = authSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserData userData)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, userData, token);

            await _next(context);
        }

        private void attachUserToContext (HttpContext context, IUserData userData, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_authSettings.Secret);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = long.Parse(jwtToken.Claims.First(x => x.Type == "userId").Value);

                // attach account to context on successful jwt validation
                context.Items["User"] = userData.GetById(userId);

            }
            catch
            {
                // do nothing if jwt validation fails
                // account is not attached to context so request won't have access to secure routes
            }
        }

    }
}
