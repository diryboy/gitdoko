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
    public enum TopicOperation
    {
        Read,
        Update,
        Close,
        Delete
    }

    public class VerifyTopicAccessibleAttribute : Attribute, IFilterFactory
    {
        public const string TopicNumberRouteTemplate = "{topicNumber}";
        private const string Key_TopicNumber = "topicNumber";
        private readonly TopicOperation Operation;

        public bool IsReusable => false;

        public bool IncludeCreator { get; set; }

        public VerifyTopicAccessibleAttribute( TopicOperation operation = TopicOperation.Read )
        {
            Operation = operation;
        }

        public IFilterMetadata CreateInstance( IServiceProvider serviceProvider )
        {
            if ( serviceProvider.GetService(typeof(AppDbContext)) is AppDbContext db )
            {
                return new VerifyTopicAccessibleFilter(db, Operation, IncludeCreator);
            }
            else
            {
                throw new ArgumentNullException(nameof(db), "Configure AppDbContext in DI!"); //TODO: Find correct behavior.
            }
        }

        private class VerifyTopicAccessibleFilter : IAsyncActionFilter
        {
            private readonly AppDbContext AppDb;
            private readonly TopicOperation Operation;
            private readonly bool IncludeCreator;

            public VerifyTopicAccessibleFilter( AppDbContext db, TopicOperation operation, bool includeCreator )
            {
                AppDb = db;
                Operation = operation;
                IncludeCreator = includeCreator;
            }

            public Task OnActionExecutionAsync( ActionExecutingContext context, ActionExecutionDelegate next )
            {
                if ( context.HttpContext.Items.TryGetValue(VerifyProjectAccessibleAttribute.HttpContextItemKey_VerifiedProject, out var oProject)
                    && context.RouteData.Values.TryGetValue(Key_TopicNumber, out var oTopicNumber) )
                {
                    return VerifyTopicExistsAsync(context, next, (Project)oProject, (string)oTopicNumber);
                }
                else
                {
                    return next();
                }
            }

            private async Task VerifyTopicExistsAsync( ActionExecutingContext context, ActionExecutionDelegate next, Project project, string strTopicNumber )
            {
                if ( !Int32.TryParse(strTopicNumber, out var topicNumber) )
                {
                    context.Result = new NotFoundResult();
                    return;
                }

                IQueryable<Topic> topics = AppDb.Topics;
                if ( IncludeCreator )
                {
                    topics = topics.Include(t => t.Creator);
                }
                var topic = await topics.FirstOrDefaultAsync(t => t.Project == project && t.TopicNumber == topicNumber);
                if ( topic == null )
                {
                    context.Result = new NotFoundResult();
                    return;
                }

                //TODO: check operation

                var topicParamName = context.ActionDescriptor.Parameters.FirstOrDefault(p => p.ParameterType == typeof(Topic))?.Name;
                if ( topicParamName != null )
                {
                    context.ActionArguments[topicParamName] = topic;
                }

                await next();
            }
        }
    }
}
