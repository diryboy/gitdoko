using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gitdoko.Models;
using gitdoko.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [Route("/[controller]s/[action]")]
        public async Task<IActionResult> Mine()
        {
            var projects = from p in AppDb.Projects
                           where p.Creator.UserName == User.Identity.Name
                           select p;

            return View(await projects.ToListAsync());
        }

        [Route("/{projectOwner}/{projectName}")]
        public IActionResult Index([FromRoute] string projectOwner, [FromRoute] string projectName)
        {
            return Content($"Project home for {projectOwner}/{projectName}");
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
                    return RedirectToAction(nameof(Mine));
                }
            }

            return View();
        }
    }
}
