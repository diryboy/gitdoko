﻿using System;
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

        public VerifyTopicAccessibleAttribute( TopicOperation operation = TopicOperation.Read )
        {
            Operation = operation;
        }

        public IFilterMetadata CreateInstance( IServiceProvider serviceProvider )
        {
            var db = serviceProvider.GetService(typeof(AppDbContext)) as AppDbContext;
            if ( db != null )
            {
                return new VerifyTopicAccessibleFilter(db, Operation);
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

            public VerifyTopicAccessibleFilter( AppDbContext db, TopicOperation operation )
            {
                AppDb = db;
                Operation = operation;
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
                    //TODO: check operation

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