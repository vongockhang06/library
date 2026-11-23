using System.ComponentModel.DataAnnotations;

namespace library.ViewModels.Account
{
    public class SignUpViewModel
    {
        [Display(Name = "Full name")]
        [Required(ErrorMessage = "Enter your full name"),
            StringLength(30, ErrorMessage = "Use up to 30 characters")]
        public string FullName { get; set; } = "";



        [Required(ErrorMessage = "Enter your username."),
            StringLength(20, ErrorMessage = "Use up to 20 characters")]
        public string Username { get; set; } = "";



        [Required(ErrorMessage = "Enter your email."),
            EmailAddress(ErrorMessage = "Enter valid email."),
            StringLength(100, ErrorMessage = "Use up to 100 characters")]
        public string Email { get; set; } = "";



        [Display(Name = "Phone number")]
        [Required(ErrorMessage = "Enter phone number"),
            RegularExpression(@"^\d{10}$", ErrorMessage = "Enter exactly 10 digits.")]
        public string PhoneNumber { get; set; } = "";


        [Required(ErrorMessage = "Create a password."), MinLength(8, ErrorMessage = "Use at least 8 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}
