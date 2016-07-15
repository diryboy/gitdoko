using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using gitdoko.Models;
using Microsoft.EntityFrameworkCore;

namespace gitdoko.Filters
{
    public class VerifyProjectAccessibleAttribute : TypeFilterAttribute
    {
        public const string HttpContextItemKey_VerifiedProject = "AccessibleVerifiedProject";
        public const string ProjectIdentifierRouteTemplate = "{projectOwner}/{projectName}";
        private const string Key_ProjectOwner = "projectOwner";
        private const string Key_ProjectName = "projectName";

        public VerifyProjectAccessibleAttribute() : base(typeof(VerifyProjectAccessibleFilter)) { }

        private class VerifyProjectAccessibleFilter : IAsyncActionFilter
        {
            private readonly AppDbContext AppDb;

            public VerifyProjectAccessibleFilter( AppDbContext appDb )
            {
                AppDb = appDb;
            }

            public Task OnActionExecutionAsync( ActionExecutingContext context, ActionExecutionDelegate next )
            {
                var routeValues = context.RouteData.Values;
                if ( !routeValues.ContainsKey(Key_ProjectOwner) || !routeValues.ContainsKey(Key_ProjectName) )
                {
                    return next();
                }
                else
                {
                    return VerifyProjectAccessibleAsync(context, next);
                }
            }

            private async Task VerifyProjectAccessibleAsync( ActionExecutingContext context, ActionExecutionDelegate next )
            {
                var routeValues = context.RouteData.Values;

                var projectQuery = from p in AppDb.Projects
                                   where String.Compare(p.Creator.UserName, (string)routeValues[Key_ProjectOwner], true) == 0
                                      && String.Compare(p.Name, (string)routeValues[Key_ProjectName], true) == 0
                                   select p;

                var projects = await projectQuery.ToArrayAsync();
                if ( projects.Length == 1 )
                {
                    var targetProject = projects[0];
                    context.HttpContext.Items[HttpContextItemKey_VerifiedProject] = targetProject;
                    //if ( targetProject.UserRights )
                    //{
                    //}
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
