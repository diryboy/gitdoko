using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gitdoko.Filters;
using gitdoko.ViewModels;
using gitdoko.Models;
using Microsoft.AspNetCore.Authorization;

namespace gitdoko.Controllers
{
    [VerifyProjectAccessible]
    [AutoValidateAntiforgeryToken]
    [Route(ProjectIdTemplate + "/[controller]")]
    public abstract class TopicController<TEditViewModel> : Controller
    {
        protected const string ProjectIdTemplate = VerifyProjectAccessibleAttribute.ProjectIdentifierRouteTemplate;
        private const string UserNameTemplate = VerifyUserExistsAttribute.UserNameRouteTemplate;

        [FromRoute]
        public string ProjectOwner { get; set; }

        [FromRoute]
        public string ProjectName { get; set; }

        [VerifyUserExists]
        [Route("/" + UserNameTemplate + "/[controller]s")]
        public abstract Task<IActionResult> Authored( User author, int page );

        [VerifyUserExists]
        [Route("/" + UserNameTemplate + "/[action]/[controller]s")]
        public abstract Task<IActionResult> Involved( User involvedUser, int page );

        [Route("/" + ProjectIdTemplate + "/[controller]s")]
        public abstract Task<IActionResult> Index( TopicSearchLimits limits );

        [HttpGet]
        [Route("[action]")]
        public abstract Task<IActionResult> Create();

        [HttpPost]
        [Route("[action]")]
        public abstract Task<IActionResult> Create( TEditViewModel viewModel );

        [Route("{number}")]
        public abstract Task<IActionResult> Read( int number );

        [HttpGet, Authorize]
        [Route("{number}/[action]")]
        public abstract Task<IActionResult> Update( int number );

        [HttpPost, Authorize]
        [Route("{number}/[action]")]
        public abstract Task<IActionResult> Update( int number, TEditViewModel viewModel );

        [HttpPost, Authorize]
        [Route("{number}/[action]")]
        public abstract Task<IActionResult> Close( int number );

        [HttpPost, Authorize]
        [Route("{number}/[action]")]
        public abstract Task<IActionResult> Delete( int number );
    }
}
