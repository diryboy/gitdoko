using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using gitdoko.Extensions;
using gitdoko.Filters;
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
        public IActionResult Settings()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp( SignUpViewModel form, string returnUrl )
        {
            if ( ModelState.IsValid )
            {
                var userCreation = await UserManager.CreateAsync(new User { UserName = form.UserName }, form.Password);

                if ( userCreation.Succeeded )
                {
                    return await SignIn(form, returnUrl);
                }

                ModelState.AddErrors(userCreation.Errors, e => e.Description);
            }

            return View();
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn( SignInViewModel form, string returnUrl )
        {
            if ( ModelState.IsValid )
            {
                var signIn = await SignInManager.PasswordSignInAsync(form.UserName, form.Password, form.RememberMe, lockoutOnFailure: false);

                if ( !signIn.Succeeded )
                {
                    ModelState.AddModelError("", "Your SignIn attempt was not sucessful.");
                }
                else if ( String.IsNullOrWhiteSpace(returnUrl) )
                {
                    return RedirectToAction(nameof(ProfileController.Index), "Profile", new { userName = form.UserName });
                }
                else if ( !Url.IsLocalUrl(returnUrl) )
                {
                    return Redirect("/");
                }
            }

            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await SignInManager.SignOutAsync();

            return Redirect("/");
        }
    }
}
