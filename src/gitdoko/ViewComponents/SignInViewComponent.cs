using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gitdoko.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace gitdoko.ViewComponents
{
    public class SignInViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke( bool showValidationSummary = false )
        {
            return View(new SignInViewModel { ShowValidationSummary = showValidationSummary });
        }
    }
}
