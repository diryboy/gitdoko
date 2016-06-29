using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace gitdoko.Controllers
{
    [Route("{projectOwner}/{projectName}/[controller]/{name?}/[action]")]
    public abstract class TopicController : Controller
    {
        [FromRoute]
        public string ProjectOwner { get; set; }

        [FromRoute]
        public string ProjectName { get; set; }

        [Route("/[controller]s/[action]")]
        public abstract Task<IActionResult> Mine();

        [Route("/[controller]s/[action]")]
        public abstract Task<IActionResult> Involved();

        [Route("/{projectOwner}/{projectName}/[controller]s/Involved")]
        public abstract Task<IActionResult> InvolvedInProject();

        [Route("/{projectOwner}/{projectName}/[controller]s")]
        public abstract Task<IActionResult> Index( int page );

        [HttpGet]
        [Route("/{projectOwner}/{projectName}/[controller]s/[action]")]
        public abstract Task<IActionResult> Create();
    }
}
