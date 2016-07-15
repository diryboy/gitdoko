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
    public class VerifyTopicExistsAttribute : TypeFilterAttribute
    {
        public const string TopicNumberRouteTemplate = "{topicNumber}";
        private const string Key_TopicNumber = "topicNumber";

        public VerifyTopicExistsAttribute() : base(typeof(VerifyTopicExistsFilter)) { }

        private class VerifyTopicExistsFilter : IAsyncActionFilter
        {
            private readonly AppDbContext AppDb;

            public VerifyTopicExistsFilter( AppDbContext db )
            {
                AppDb = db;
            }

            public Task OnActionExecutionAsync( ActionExecutingContext context, ActionExecutionDelegate next )
            {
                var items = context.HttpContext.Items;
                object oProject = null;
                object oTopicNumber = null;
                if ( !items.TryGetValue(VerifyProjectAccessibleAttribute.HttpContextItemKey_VerifiedProject, out oProject)
                  || !context.RouteData.Values.TryGetValue(Key_TopicNumber, out oTopicNumber) )
                {
                    return next();
                }
                else
                {
                    return VerifyTopicExistsAsync(context, next, (Project)oProject, (int)oTopicNumber);
                }
            }

            private async Task VerifyTopicExistsAsync( ActionExecutingContext context, ActionExecutionDelegate next, Project project, int topicNumber )
            {
                var topic = await AppDb.Topics.FirstOrDefaultAsync(t => t.Project == project && t.TopicNumber == topicNumber);
                if ( topic != null )
                {
                    var topicParamName = context.ActionDescriptor.Parameters.FirstOrDefault(p => p.ParameterType == typeof(Topic))?.Name;
                    if ( topicParamName != null )
                    {
                        context.ActionArguments[topicParamName] = topic;
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
