﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gitdoko.Filters;
using gitdoko.ViewModels;
using gitdoko.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace gitdoko.Controllers
{
    [AutoValidateAntiforgeryToken]
    [VerifyProjectAccessible, Route(ProjectIdRoute + "/[controller]")]
    public abstract class TopicController<TModel, TCreateViewModel, TEditViewModel> : Controller
        where TModel : Topic
    {
        protected const string ProjectIdRoute = VerifyProjectAccessibleAttribute.ProjectIdentifierRouteTemplate;
        protected const string UserNameRoute = VerifyUserExistsAttribute.UserNameRouteTemplate;
        protected const string TopicNumberRoute = VerifyTopicAccessibleAttribute.TopicNumberRouteTemplate;

        protected readonly AppDbContext AppDb;
        protected readonly UserManager<User> UserManager;

        public TopicController( AppDbContext db, UserManager<User> um )
        {
            AppDb = db;
            UserManager = um;
        }

        [FromRoute]
        public string ProjectOwner { get; set; }

        [FromRoute]
        public string ProjectName { get; set; }

        [Route("/" + ProjectIdRoute + "/[controller]s")]
        public virtual async Task<IActionResult> Index( Project project, TopicSearchLimits limits )
        {
            var topics = from t in AppDb.Topics
                         where t.GetType() == typeof(TModel) && t.Project == project
                         select t;

            return View(await topics.ToListAsync());
        }

        [HttpGet]
        [Route("[action]")]
        public virtual async Task<IActionResult> Create()
            => View();

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public virtual async Task<IActionResult> Add( Project project, TCreateViewModel viewModel )
        {
            TModel topic = CreateTopicFromViewModel(viewModel);

            topic.Project = project;
            topic.Creator = await UserManager.GetUserAsync(User);
            topic.TopicNumber = project.NextTopicId;
            AppDb.Topics.Add(topic);
            project.NextTopicId++;
            await AppDb.SaveChangesAsync();

            return RedirectToDetails(topic);
        }

        [VerifyTopicAccessible(IncludeCreator = true), Route(TopicNumberRoute)]
        public virtual async Task<IActionResult> Details( Topic topic )
        {
            return View(topic);
        }

        [HttpGet]
        [VerifyTopicAccessible(TopicOperation.Update), Route(TopicNumberRoute + "/[action]")]
        public virtual async Task<IActionResult> Edit( Topic topic )
            => View(CreateEditViewModelFromTopic(topic));

        [HttpPost]
        [VerifyTopicAccessible(TopicOperation.Update), Route(TopicNumberRoute + "/[action]")]
        public virtual async Task<IActionResult> Update( Topic topic, TEditViewModel viewModel )
        {
            var model = (TModel)topic;
            UpdateTopicFromViewModel(model, viewModel);
            model.LastEditedBy = await UserManager.GetUserAsync(User);
            model.LastEditedOn = DateTime.UtcNow;
            await AppDb.SaveChangesAsync();

            return RedirectToDetails(topic);
        }

        [HttpPost]
        [VerifyTopicAccessible(TopicOperation.Close), Route(TopicNumberRoute + "/[action]")]
        public virtual async Task<IActionResult> Close( Topic topic )
        {
            topic.ClosedOn = DateTime.UtcNow;
            await AppDb.SaveChangesAsync();

            return RedirectToDetails(topic);
        }

        [HttpPost]
        [VerifyTopicAccessible(TopicOperation.Delete), Route(TopicNumberRoute + "/[action]")]
        public virtual async Task<IActionResult> Remove( Topic topic )
        {
            AppDb.Topics.Remove(topic);
            await AppDb.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { projectOwner = ProjectOwner, projectName = ProjectName });
        }

        protected abstract TModel CreateTopicFromViewModel( TCreateViewModel viewModel );

        protected abstract TEditViewModel CreateEditViewModelFromTopic( Topic topic );

        protected abstract void UpdateTopicFromViewModel( TModel topic, TEditViewModel viewModel );

        protected IActionResult RedirectToDetails( Topic topic )
        {
            return RedirectToAction(nameof(Details), new { topicNumber = topic.TopicNumber });
        }
    }
}
