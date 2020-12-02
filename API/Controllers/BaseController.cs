using API_DataAccess.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        public User UserDetails => (User)HttpContext.Items["User"];

        protected bool CheckIfUserHasAdminRole(List<Role> roles)
        {
            var userRoles = roles.Select(r => r.RoleKey);
            return userRoles.Contains(RoleKey.admin);
        }

    }
}
