using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gitdoko.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace gitdoko.Controllers
{
    public class IssueController : TopicController
    {
        public override async Task<IActionResult> Authored( string userName, int page )
        {
            return View("List");
        }

        public override async Task<IActionResult> Involved( string userName, int page )
        {
            return View("List");
        }

        public override async Task<IActionResult> Index( TopicSearchLimits limits )
        {
            return Content($"Issue index for {ProjectOwner}/{ProjectName}");
        }

        public override async Task<IActionResult> Create()
        {
            return Content($"Create issue for {ProjectOwner}/{ProjectName}");
        }

        public override async Task<IActionResult> Read( int number )
        {
            return Content($"View issue {number} of {ProjectOwner}/{ProjectName}");
        }
    }
}
