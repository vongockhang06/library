using System.ComponentModel.DataAnnotations;

namespace library.ViewModels.Account
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "Enter your username.")]
        public string Username { get; set; } = "";
        [Required(ErrorMessage = "Enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
        public string? ReturnUrl { get; set; }

    }
}
