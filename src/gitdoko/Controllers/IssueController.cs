using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gitdoko.Models;
using gitdoko.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace gitdoko.Controllers
{
    public class IssueController : TopicController<TopicEditViewModel>
    {
        public override async Task<IActionResult> Authored( User author, int page )
        {
            return View();
        }

        public override async Task<IActionResult> Involved( User involvedUser, int page )
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

        public override Task<IActionResult> Create( TopicEditViewModel viewModel )
        {
            throw new NotImplementedException();
        }

        public override async Task<IActionResult> Read( int number )
        {
            return Content($"View issue {number} of {ProjectOwner}/{ProjectName}");
        }

        public override Task<IActionResult> Update( int number )
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> Update( int number, TopicEditViewModel viewModel )
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> Close( int number )
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> Delete( int number )
        {
            throw new NotImplementedException();
        }
    }
}
