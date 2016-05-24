using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace gitdoko.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return Content("Your profile!");
        }

        [Route("[action]")]
        public IActionResult SignUp()
        {
            return Content("Want in? Do it!");
        }

        [Route("[action]")]
        public IActionResult SignIn()
        {
            return Content("Already in? Come back!");
        }

        [HttpPost]
        public IActionResult Create()
        {
            return Content("You are now +1!");
        }
    }
}
