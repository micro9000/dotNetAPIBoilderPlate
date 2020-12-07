using API_DataAccess.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly IList<RoleKey> _roles;

    public AuthorizeAttribute(params RoleKey[] roles)
    {
        _roles = roles ?? new RoleKey[] { };
    }

    private bool CheckUserRoles(List<Role> roles)
    {
        foreach(Role role in roles)
        {
            if (_roles.Contains(role.RoleKey))
                return true;
        }

        return false;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (User)context.HttpContext.Items["User"];
        if (user == null || (_roles.Any() && this.CheckUserRoles(user.Roles) == false))
        {
            // not logged in or role not authorized
            context.Result = new JsonResult(new { message = "Unauthorized - AuthorizeAttribute" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
