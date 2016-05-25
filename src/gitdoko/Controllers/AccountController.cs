using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gitdoko.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return Content("Your profile!");
        }

        [AllowAnonymous]
        [Route("[action]")]
        public IActionResult SignUp()
        {
            return Content("Want in? Do it!");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("[action]")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("[action]")]
        public IActionResult SignIn( string name, string password, string returnUrl )
        {
            return Content($"{name}, you are now +1!");
        }
    }
}
