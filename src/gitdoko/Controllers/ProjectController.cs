﻿using System;
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
    [AutoValidateAntiforgeryToken]
    [Route("[controller]/[action]")]
    public class ProjectController : Controller
    {
        private readonly AppDbContext AppDb;
        private readonly UserManager<User> UserManager;

        public ProjectController( AppDbContext db, UserManager<User> um )
        {
            AppDb = db;
            UserManager = um;
        }

        [VerifyProjectAccessible]
        [Route("/" + VerifyProjectAccessibleAttribute.ProjectIdentifierRouteTemplate)]
        public IActionResult Home( Project project )
        {
            return View(project);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add( CreateProjectViewModel form )
        {
            if ( ModelState.IsValid )
            {
                var currentUser = await UserManager.GetUserAsync(User);
                var projects = from p in AppDb.Projects
                               where p.Creator == currentUser && String.Equals(p.Name, form.Name, StringComparison.OrdinalIgnoreCase)
                               select p;

                if ( await projects.AnyAsync() )
                {
                    ModelState.AddModelError("", $"Project '{form.Name}' already exists.");
                    return View();
                }

                AppDb.Projects.Add(new Project
                {
                    Creator = await UserManager.GetUserAsync(User),
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
                    return RedirectToAction(nameof(ProfileController.Projects), "Profile", new { userName = User.Identity.Name });
                }
            }

            return View();
        }
    }
}
