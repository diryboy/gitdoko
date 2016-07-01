using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gitdoko.Filters;

namespace gitdoko.Controllers
{
    [VerifyProjectAccessible]
    [Route(ProjectId + "/[controller]/{name?}/[action]")]
    public abstract class TopicController : Controller
    {
        private const string ProjectId = VerifyProjectAccessibleAttribute.ProjectIdentifierRouteTemplate;

        [FromRoute]
        public string ProjectOwner { get; set; }

        [FromRoute]
        public string ProjectName { get; set; }

        [Route("/[controller]s/[action]")]
        public abstract Task<IActionResult> Mine( int page );

        [Route("/[controller]s/[action]")]
        public abstract Task<IActionResult> Involved( int page );

        [Route("/" + ProjectId + "/[controller]s/Involved")]
        public abstract Task<IActionResult> InvolvedInProject( int page );

        [Route("/" + ProjectId + "/[controller]s")]
        public abstract Task<IActionResult> Index( int page );

        [HttpGet]
        [Route("/" + ProjectId + "/[controller]s/[action]")]
        public abstract Task<IActionResult> Create();
    }
}
