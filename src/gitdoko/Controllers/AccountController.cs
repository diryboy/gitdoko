using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            return Content($"Welcome to your account, {User.Identity.Name}!");
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
        public async Task<IActionResult> SignIn( string name, string password )
        {
            var issuer = nameof(gitdoko);
            var authenticationType = "Hardcode";
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, name, ClaimValueTypes.String, issuer),
            }, authenticationType));

            await HttpContext.Authentication.SignInAsync("Cookies", principal);

            return View();
        }
    }
}
