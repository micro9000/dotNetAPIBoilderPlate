using API.DTO.User;
using API.Services;
using API_DataAccess.DataAccess.Contracts;
using API_DataAccess.DataAccess.Core;
using API_DataAccess.Model;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly ILogger<UsersController> _log;
        private readonly IMapper _mapper;
        private readonly IUserData _userData;
        private readonly IUserAuthService _userAuthService;

        public UsersController(ILogger<UsersController> log,
                                IMapper mapper,
                                IUserData userData,
                                IUserAuthService userAuthService)
        {
            _log = log;
            _userData = userData;
            _userAuthService = userAuthService;
            _mapper = mapper;
        }

        [HttpPost("authenticate")]
        public ActionResult<ReadUserDTO> Authenticate([FromBody] LoginDTO loginDTO)
        {
            var response = _userAuthService.Authenticate(loginDTO, ipAddress());

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public ActionResult<ReadUserDTO> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _userAuthService.RefreshToken(refreshToken, ipAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public ActionResult RevokeToken([FromBody] RevokeTokenRequestDTO revokeTokenInput)
        {
            var token = revokeTokenInput.RefreshToken ?? System.Net.WebUtility.UrlDecode(Request.Cookies["refreshToken"]);

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = _userAuthService.RevokeToken(token, ipAddress());

            if (!response)
                return NotFound(new { message = "Token not found - " + token });

            return Ok(new { message = "Token revoked" });
        }


        [HttpGet]
        [Authorize(RoleKey.admin, RoleKey.superuser)]
        public ActionResult<IEnumerable<ReadUserDTO>> Get()
        {
            var users = _userData.GetAll_exclude_deleted();

            var usersDTO = _mapper.Map<IEnumerable<UserBaseDTO>>(users);
            return Ok(usersDTO);
        }


        [HttpGet("{id}")]
        //[Authorize(RoleKey.admin)]
        public ActionResult<ReadUserDTO> GetById(long id)
        {

            //if (UserDetails == null || id != UserDetails.Id && CheckIfUserHasAdminRole(UserDetails.Roles))
            //    return Unauthorized(new { message = "Unauthorized" });

            // only allow admins to access other user records
            //var currentUserId = int.Parse(User.Identity.Name);
            //if (id != currentUserId && !User.IsInRole("admin"))
            //    return Forbid();

            var user = _userData.GetById(id).Result;

            var userDTO = _mapper.Map<UserBaseDTO>(user);
            return Ok(userDTO);
        }

        // helper methods

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

    }
}
