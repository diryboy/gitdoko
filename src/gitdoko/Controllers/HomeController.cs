using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace gitdoko.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if ( User.Identity.IsAuthenticated )
            {
                return Content($"Hi, {User.Identity.Name}! You proved your id by using {User.Identity.AuthenticationType}.");
            }
            else
            {
                return Content("It works!");
            }
        }
    }
}
