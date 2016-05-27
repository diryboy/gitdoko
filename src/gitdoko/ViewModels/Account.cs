using System;
using System.ComponentModel.DataAnnotations;

namespace gitdoko.ViewModels
{
    public class SignInViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class SignUpViewModel : SignInViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
