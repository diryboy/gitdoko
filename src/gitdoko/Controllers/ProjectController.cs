using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using gitdoko.Filters;
using gitdoko.Models;
using gitdoko.ViewModels;

namespace gitdoko.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize, AutoValidateAntiforgeryToken]
    public class ProjectController : Controller
    {
        private readonly AppDbContext AppDb;

        public ProjectController( AppDbContext db )
        {
            AppDb = db;
        }

        [VerifyUserExists]
        [Route("/" + VerifyUserExistsAttribute.UserNameRouteTemplate + "/[controller]s")]
        public async Task<IActionResult> Authored( string userName, int page )
        {
            var projects = from p in AppDb.Projects
                           where p.Creator.UserName == User.Identity.Name
                           select p;

            return View(await projects.ToListAsync());
        }

        [VerifyProjectAccessible]
        [Route("/" + VerifyProjectAccessibleAttribute.ProjectIdentifierRouteTemplate)]
        public IActionResult Index( [FromRoute] string projectOwner, [FromRoute] string projectName )
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create( CreateProjectViewModel form )
        {
            if ( ModelState.IsValid )
            {
                var projects = from p in AppDb.Projects
                               where p.Creator.UserName == User.Identity.Name && p.Name == form.Name
                               select p;

                if ( await projects.AnyAsync() )
                {
                    ModelState.AddModelError("", $"Project '{form.Name}' already exists.");
                    return View();
                }

                AppDb.Projects.Add(new Project
                {
                    Creator = new User { UserName = User.Identity.Name },
                    Name = form.Name,
                    Summary = form.Summary,
                    Description = form.Description,
                });

                var i = await AppDb.SaveChangesAsync();

                if ( i < 1 )
                {
                    ModelState.AddModelError("", "Failed to create project.");
                }
                else
                {
                    return RedirectToAction(nameof(Authored), new { userName = User.Identity.Name });
                }
            }

            return View();
        }
    }
}
