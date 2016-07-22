using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gitdoko.Filters;
using gitdoko.ViewModels;
using gitdoko.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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

        public TopicController( AppDbContext db )
        {
            AppDb = db;
        }

        [FromRoute]
        public string ProjectOwner { get; set; }

        [FromRoute]
        public string ProjectName { get; set; }

        [VerifyUserExists, Route("/" + UserNameRoute + "/[controller]s")]
        public virtual async Task<IActionResult> Authored( User author, int page )
            => View("List", await AppDb.Topics.Where(t => t.Creator == author && t.GetType() == typeof(TModel)).ToListAsync());

        [VerifyUserExists, Route("/" + UserNameRoute + "/[action]/[controller]s")]
        public virtual async Task<IActionResult> Involved( User involvedUser, int page )
        {
            throw new NotImplementedException();
            return View("List", await AppDb.Topics.Where(
                t => t.Creator == involvedUser
                && t.Discussions.Any(d => d.Creator == involvedUser)
                && t.GetType() == typeof(TModel)).ToListAsync());
        }

        [Route("/" + ProjectIdRoute + "/[controller]s")]
        public virtual async Task<IActionResult> Index( TopicSearchLimits limits )
            => View("List", await AppDb.Topics.Where(t => t.GetType() == typeof(TModel)).ToListAsync());

        [HttpGet]
        [Route("[action]")]
        public virtual async Task<IActionResult> Create()
            => View();

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public virtual async Task<IActionResult> Create( TCreateViewModel viewModel )
        {
            TModel topic = CreateTopicFromViewMode(viewModel);

            AppDb.Topics.Add(topic);
            await AppDb.SaveChangesAsync();

            return RedirectToRead(topic);
        }

        [VerifyTopicAccessible, Route(TopicNumberRoute)]
        public virtual async Task<IActionResult> Read( Topic topic )
            => View(topic);

        [HttpGet]
        [VerifyTopicAccessible(TopicOperation.Update), Route(TopicNumberRoute + "/[action]")]
        public virtual async Task<IActionResult> Update( Topic topic )
            => View(CreateEditViewModelFromTopic(topic));

        [HttpPost]
        [VerifyTopicAccessible(TopicOperation.Update), Route(TopicNumberRoute + "/[action]")]
        public virtual async Task<IActionResult> Update( Topic topic, TEditViewModel viewModel )
        {
            var model = (TModel)topic;
            UpdateTopicFromViewMode(model, viewModel);
            await AppDb.SaveChangesAsync();

            return RedirectToRead(topic);
        }

        [HttpPost]
        [VerifyTopicAccessible(TopicOperation.Close), Route(TopicNumberRoute + "/[action]")]
        public virtual async Task<IActionResult> Close( Topic topic )
        {
            topic.ClosedOn = DateTime.UtcNow;
            await AppDb.SaveChangesAsync();

            return RedirectToRead(topic);
        }

        [HttpPost]
        [VerifyTopicAccessible(TopicOperation.Delete), Route(TopicNumberRoute + "/[action]")]
        public virtual async Task<IActionResult> Delete( Topic topic )
        {
            AppDb.Topics.Remove(topic);
            await AppDb.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { projectOwner = ProjectOwner, projectName = ProjectName });
        }

        protected abstract TModel CreateTopicFromViewMode( TCreateViewModel viewModel );

        protected abstract TEditViewModel CreateEditViewModelFromTopic( Topic topic );

        protected abstract void UpdateTopicFromViewMode( TModel topic, TEditViewModel viewModel );


        protected IActionResult RedirectToRead( Topic topic )
        {
            return RedirectToAction(nameof(Read), new { topicNumber = topic.TopicNumber });
        }
    }
}
