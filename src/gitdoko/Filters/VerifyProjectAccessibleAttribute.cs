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
        public const string ProjectIdentifierRouteTemplate = "{userName}/{projectName}";
        private const string Key_ProjectOwner = "userName";
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

                var projectQuery = from p in AppDb.Projects.Include(p => p.Creator)
                                   where String.Equals(p.Creator.UserName, (string)routeValues[Key_ProjectOwner], StringComparison.OrdinalIgnoreCase)
                                      && String.Equals(p.Name, (string)routeValues[Key_ProjectName], StringComparison.OrdinalIgnoreCase)
                                   select p;

                var projects = await projectQuery.ToArrayAsync();
                if ( projects.Length == 1 )
                {
                    //TODO: if ( targetProject.UserRights ... )

                    var targetProject = projects[0];
                    context.HttpContext.Items[HttpContextItemKey_VerifiedProject] = targetProject;

                    var projectParamName = context.ActionDescriptor.Parameters.FirstOrDefault(p => p.ParameterType == typeof(Project))?.Name;
                    if ( projectParamName != null )
                    {
                        context.ActionArguments[projectParamName] = targetProject;
                    }

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
