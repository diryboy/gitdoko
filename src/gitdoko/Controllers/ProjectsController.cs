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
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class ProjectsController : Controller
    {
        private readonly DefaultDbContext DbContext;

        public ProjectsController( DefaultDbContext db )
        {
            DbContext = db;
        }

        public async Task<IActionResult> Index()
        {
            var projects = from p in DbContext.Projects
                           where p.Creator.UserName == User.Identity.Name
                           select p;

            return View(await projects.ToListAsync());
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
                var projects = from p in DbContext.Projects
                               where p.Creator.UserName == User.Identity.Name && p.Name == form.Name
                               select p;

                if ( await projects.AnyAsync() )
                {
                    ModelState.AddModelError("", $"Project '{form.Name}' already exists.");
                    return View();
                }

                DbContext.Projects.Add(new Project
                {
                    Creator = new User { UserName = User.Identity.Name },
                    Name = form.Name,
                    Summary = form.Summary,
                    Description = form.Description,
                });

                var i = await DbContext.SaveChangesAsync();

                if ( i < 1 )
                {
                    ModelState.AddModelError("", "Failed to create project.");
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View();
        }
    }
}
