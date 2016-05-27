using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using gitdoko.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace gitdoko.Controllers
{
    [Route("[action]")]
    public class AccountController : Controller
    {
        [Authorize]
        public IActionResult Account()
        {
            return Content($"Welcome to your account, {User.Identity.Name}!");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp
        (
            [FromServices] UserManager<User> userManager,
            string name,
            string password
        )
        {
            var result = await userManager.CreateAsync(new User { UserName = name }, password);

            if ( !result.Succeeded )
            {
                return Content(result.Errors.Aggregate("", ( s, e ) => $"{s} <|> {e.Description}"));
            }

            return View(); // need redirect regardless of return url
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn
        (
             [FromServices] SignInManager<User> signInManager,
             string name,
             string password
        )
        {
            var result = await signInManager.PasswordSignInAsync(name, password, isPersistent: false, lockoutOnFailure: false);

            if ( !result.Succeeded )
            {
                return Content("Failed to sign in.");
            }

            return View(); // need redirect regardless of return url
        }
    }
}
