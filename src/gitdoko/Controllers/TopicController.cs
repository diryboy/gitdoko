using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gitdoko.Filters;
using gitdoko.ViewModels;

namespace gitdoko.Controllers
{
    [VerifyProjectAccessible]
    public abstract class TopicController : Controller
    {
        protected const string ProjectIdTemplate = VerifyProjectAccessibleAttribute.ProjectIdentifierRouteTemplate;
        private const string UserNameTemplate = VerifyUserExistsAttribute.UserNameRouteTemplate;

        [FromRoute]
        public string ProjectOwner { get; set; }

        [FromRoute]
        public string ProjectName { get; set; }

        [VerifyUserExists]
        [Route("/" + UserNameTemplate + "/[controller]s")]
        public abstract Task<IActionResult> Authored( string userName, int page );

        [VerifyUserExists]
        [Route("/" + UserNameTemplate + "/[controller]s/[action]")]
        public abstract Task<IActionResult> Involved( string userName, int page );

        [Route(ProjectIdTemplate + "/[controller]s")]
        public abstract Task<IActionResult> Index( TopicSearchLimits limits );

        [HttpGet]
        [Route(ProjectIdTemplate + "/[controller]s/[action]")]
        public abstract Task<IActionResult> Create();

        [Route(ProjectIdTemplate + "/[controller]/{number}")]
        public abstract Task<IActionResult> Read( int number );
    }
}
