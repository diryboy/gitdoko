using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using gitdoko.Extensions;
using gitdoko.Models;
using gitdoko.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace gitdoko.Controllers
{
    [Route("[action]")]
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private readonly UserManager<User> UserManager;
        private readonly SignInManager<User> SignInManager;

        public AccountController( UserManager<User> um, SignInManager<User> sm )
        {
            UserManager = um;
            SignInManager = sm;
        }

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
        public async Task<IActionResult> SignUp( SignUpViewModel form )
        {
            if ( !ModelState.IsValid )
            {
                return View();
            }

            var userCreation = await UserManager.CreateAsync(new User { UserName = form.UserName }, form.Password);

            if ( userCreation.Succeeded )
            {
                return await SignIn(form);
            }

            ModelState.AddErrors(userCreation.Errors, e => e.Description);
            return View();
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn( SignInViewModel form )
        {
            if ( !ModelState.IsValid )
            {
                return View();
            }

            var signIn = await SignInManager.PasswordSignInAsync(form.UserName, form.Password, form.RememberMe, lockoutOnFailure: false);

            if ( signIn.Succeeded )
            {
                return View();
            }

            return View(); // need redirect regardless of return url
        }
    }
}
