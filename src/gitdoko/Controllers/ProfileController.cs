using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gitdoko.Filters;
using gitdoko.Models;
using Microsoft.EntityFrameworkCore;

namespace gitdoko.Controllers
{
    [VerifyUserExists]
    [Route(VerifyUserExistsAttribute.UserNameRouteTemplate)]
    public class ProfileController : Controller
    {
        private readonly AppDbContext AppDb;

        public ProfileController( AppDbContext db )
        {
            AppDb = db;
        }

        [Route("")]
        public IActionResult Index( User user )
        {
            return View(user);
        }

        [Route("[action]")]
        public async Task<IActionResult> Projects( User author, int page )
        {
            var projects = from p in AppDb.Projects
                           where p.Creator == author
                           select p;

            return View(await projects.ToListAsync());
        }

        [Route("[action]")]
        public async Task<IActionResult> Issues( User author, int page )
        {
            throw new NotImplementedException();
        }

        [Route("Issues/Involved")]
        public async Task<IActionResult> InvolvedIssues( User author, int page )
        {
            throw new NotImplementedException();
        }

        [Route("[action]")]
        public async Task<IActionResult> PullRequests( User author, int page )
        {
            throw new NotImplementedException();
        }

        [Route("PullRequests/Involved")]
        public async Task<IActionResult> InvolvedPullRequests( User author, int page )
        {
            throw new NotImplementedException();
        }
    }
}
