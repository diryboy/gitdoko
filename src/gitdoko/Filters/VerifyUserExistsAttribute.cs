using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gitdoko.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace gitdoko.Filters
{
    public class VerifyUserExistsAttribute : TypeFilterAttribute
    {
        public const string UserNameRouteTemplate = "{userName}";
        private const string Key_UserName = "userName";

        public VerifyUserExistsAttribute() : base(typeof(VerifyUserExistsFilter)) { }

        private class VerifyUserExistsFilter : IAsyncActionFilter
        {
            private readonly AppDbContext AppDb;

            public VerifyUserExistsFilter( AppDbContext db )
            {
                AppDb = db;
            }

            public Task OnActionExecutionAsync( ActionExecutingContext context, ActionExecutionDelegate next )
            {
                var routeValues = context.RouteData.Values;
                if ( !routeValues.ContainsKey(Key_UserName) )
                {
                    return next();
                }
                else
                {
                    return VerifyUserExistsAsync(context, next);
                }
            }

            private async Task VerifyUserExistsAsync( ActionExecutingContext context, ActionExecutionDelegate next )
            {
                var userName = (string)context.RouteData.Values[Key_UserName];
                if ( await AppDb.Users.AnyAsync(user => user.UserName == userName) )
                {
                    await next();
                }
                else
                {
                    context.Result = new NotFoundResult();
                }
            }
        }
    }
}
