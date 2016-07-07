using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gitdoko.Filters;

namespace gitdoko.Controllers
{
    [VerifyProjectAccessible]
    public abstract class TopicController : Controller
    {
        protected const string ProjectIdTemplate = VerifyProjectAccessibleAttribute.ProjectIdentifierRouteTemplate;

        [FromRoute]
        public string ProjectOwner { get; set; }

        [FromRoute]
        public string ProjectName { get; set; }

        [Route("[controller]s/[action]")]
        public abstract Task<IActionResult> Mine( int page );

        [Route("[controller]s/[action]")]
        public abstract Task<IActionResult> Involved( int page );

        [Route(ProjectIdTemplate + "/[controller]s/Involved")]
        public abstract Task<IActionResult> InvolvedInProject( int page );

        [Route(ProjectIdTemplate + "/[controller]s")]
        public abstract Task<IActionResult> Index( int page );

        [HttpGet]
        [Route(ProjectIdTemplate + "/[controller]s/[action]")]
        public abstract Task<IActionResult> Create();

        [Route(ProjectIdTemplate + "/[controller]/{number}")]
        public abstract Task<IActionResult> Read( int number );
    }
}
