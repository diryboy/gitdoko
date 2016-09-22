using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gitdoko.Filters;
using gitdoko.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gitdoko.Controllers
{
    [AutoValidateAntiforgeryToken]
    [VerifyProjectAccessible, Route(VerifyProjectAccessibleAttribute.ProjectIdentifierRouteTemplate + "/[controller]/[action]")]
    public class LabelController : Controller
    {
        private readonly AppDbContext AppDb;

        public LabelController( AppDbContext db )
        {
            AppDb = db;
        }

        [Route("/" + VerifyProjectAccessibleAttribute.ProjectIdentifierRouteTemplate + "/[controller]s")]
        public async Task<IActionResult> Index( Project project )
        {
            var labels = from l in AppDb.Labels
                         where l.Project == project
                         select l;

            return View(await labels.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Add( Project project, Label label )
        {
            label.Project = project;
            AppDb.Labels.Add(label);
            var n = await AppDb.SaveChangesAsync();

            return n == 1 ? Ok() : StatusCode(500);
        }

        [HttpPost]
        public async Task<IActionResult> Update( Project project, Label label )
        {
            AppDb.Update(label);
            var n = await AppDb.SaveChangesAsync();

            return n == 1 ? Ok() : StatusCode(500);
        }

        [HttpPost]
        public async Task<IActionResult> Remove( Project project, Label label )
        {
            AppDb.Remove(label);
            var n = await AppDb.SaveChangesAsync();

            return n == 1 ? Ok() : StatusCode(500);
        }
    }
}
