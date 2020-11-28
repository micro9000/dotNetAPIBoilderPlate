using API.DTO.User;
using API.Services;
using API_DataAccess.DataAccess.Core;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
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

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public ActionResult<ReadUserDTO> Authenticate([FromBody] LoginDTO loginDTO)
        {
            var response = _userAuthService.Authenticate(loginDTO, ipAddress());

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }


        [AllowAnonymous]
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


        [HttpPost("revoke-token")]
        public ActionResult RevokeToken([FromBody] RevokeTokenRequestDTO revokeTokenInput)
        {
            var token = revokeTokenInput.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = _userAuthService.RevokeToken(token, ipAddress());

            if (!response)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }


        [HttpGet("getall")]
        public ActionResult<IEnumerable<ReadUserDTO>> GetAll()
        {
            var users = _userData.GetAll_exclude_deleted();

            var usersDTO = _mapper.Map<IEnumerable<ReadUserBaseDTO>>(users);
            return Ok(usersDTO);
        }

        [HttpGet("{id}")]
        public ActionResult<ReadUserDTO> GetById(int id)
        {
            var user = _userData.Get(id);

            var userDTO = _mapper.Map<ReadUserBaseDTO>(user);
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
