using AdminPanel.Controllers;
using AdminPanel.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AdminPanel
{
    public class AuthMiddleware : IMiddleware
    {
        private readonly string _userGroup;

        public AuthMiddleware(string userGroup) 
            => _userGroup = userGroup;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Заглушка, которая не даст аутентифицироваться локальным пользователям
            // произойдет повторный запрос учетных данных
            if (context.User.Identity.IsAuthenticated)
            {
                if (context.User.Identity is WindowsIdentity identity)
                {
                    var groups = identity.Groups.Select(c => c.Translate(typeof(NTAccount)).Value).ToList();

                    if (groups.Contains(_userGroup))
                        await next(context);
                    else
                        await context.ChallengeAsync();
                }
            }
            else
                await next(context);
        }
    }
}
