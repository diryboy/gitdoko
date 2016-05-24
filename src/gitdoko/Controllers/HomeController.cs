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
            return Content("It works!");
        }
    }
}
