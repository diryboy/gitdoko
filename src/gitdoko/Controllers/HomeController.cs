using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace gitdoko.Controllers
{
    [Route("[action]")]
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            if ( User.Identity.IsAuthenticated )
            {
                // watched projects' feeds
                // owned & involved projects
            }
            else
            {
                // Browse public projects
            }

            return View();
        }
    }
}
