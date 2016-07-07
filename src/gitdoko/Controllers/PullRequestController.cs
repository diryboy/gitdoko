using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gitdoko.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace gitdoko.Controllers
{
    public class PullRequestController : TopicController
    {
        public override async Task<IActionResult> Authored( string userName, int page )
        {
            return View("List");
        }

        public override async Task<IActionResult> Involved( string userName, int page = 1 )
        {
            return View("List");
        }

        public override async Task<IActionResult> Index( TopicSearchLimits limits )
        {
            return Content($"Pull requests for {ProjectOwner}/{ProjectName}");
        }

        public override async Task<IActionResult> Create()
        {
            return Content($"Create pull request for {ProjectOwner}/{ProjectName}");
        }

        public override async Task<IActionResult> Read( int number )
        {
            return Content($"View pull request {number} of {ProjectOwner}/{ProjectName}");
        }
    }
}
