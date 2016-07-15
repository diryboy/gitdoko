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
    [VerifyProjectAccessible]
    [AutoValidateAntiforgeryToken]
    [Route(ProjectIdRoute + "/[controller]")]
    public abstract class TopicController<TModel, TEditViewModel> : Controller
        where TModel : Topic, new()
        where TEditViewModel : TopicEditViewModel
    {
        protected const string ProjectIdRoute = VerifyProjectAccessibleAttribute.ProjectIdentifierRouteTemplate;
        protected const string UserNameRoute = VerifyUserExistsAttribute.UserNameRouteTemplate;
        protected const string TopicNumberRoute = VerifyTopicExistsAttribute.TopicNumberRouteTemplate;

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
            => View(await AppDb.Topics.Where(t => t.Creator == author && t.GetType() == typeof(TModel)).ToListAsync());

        [VerifyUserExists, Route("/" + UserNameRoute + "/[action]/[controller]s")]
        public virtual Task<IActionResult> Involved( User involvedUser, int page )
        {
            throw new NotImplementedException();
        }

        [Route("/" + ProjectIdRoute + "/[controller]s")]
        public virtual async Task<IActionResult> Index( TopicSearchLimits limits )
            => View(await AppDb.Topics.Where(t => t.GetType() == typeof(TModel)).ToListAsync());

        [HttpGet]
        [Route("[action]")]
        public virtual async Task<IActionResult> Create()
            => View();

        [HttpPost]
        [Route("[action]")]
        public virtual async Task<IActionResult> Create( TEditViewModel viewModel )
        {
            TModel topic = null;
            CreateOrUpdateTopicFromViewMode(ref topic, viewModel);

            AppDb.Topics.Add(topic);
            await AppDb.SaveChangesAsync();

            return RedirectToRead(topic);
        }

        [Route(TopicNumberRoute)]
        public virtual async Task<IActionResult> Read( Topic topic )
            => View(topic);

        [HttpGet, Authorize]
        [VerifyTopicExists, Route(TopicNumberRoute + "/[action]")]
        public virtual async Task<IActionResult> Update( Topic topic )
            => View(topic);

        [HttpPost, Authorize]
        [VerifyTopicExists, Route(TopicNumberRoute + "/[action]")]
        public virtual async Task<IActionResult> Update( Topic topic, TEditViewModel viewModel )
        {
            var model = (TModel)topic;
            CreateOrUpdateTopicFromViewMode(ref model, viewModel);
            await AppDb.SaveChangesAsync();

            return RedirectToRead(topic);
        }

        [HttpPost, Authorize]
        [VerifyTopicExists, Route(TopicNumberRoute + "/[action]")]
        public virtual async Task<IActionResult> Close( Topic topic )
        {
            topic.ClosedOn = DateTime.UtcNow;
            await AppDb.SaveChangesAsync();

            return RedirectToRead(topic);
        }

        [HttpPost, Authorize]
        [VerifyTopicExists, Route(TopicNumberRoute + "/[action]")]
        public virtual async Task<IActionResult> Delete( Topic topic )
        {
            AppDb.Topics.Remove(topic);
            await AppDb.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { projectOwner = ProjectOwner, projectName = ProjectName });
        }

        protected abstract void CreateOrUpdateTopicFromViewMode( ref TModel topic, TEditViewModel viewModel );

        protected IActionResult RedirectToRead( Topic topic )
        {
            return RedirectToAction(nameof(Read), new { topicNumber = topic.TopicNumber });
        }
    }
}
