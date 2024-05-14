using System.ComponentModel.DataAnnotations;

namespace Attar.CRUD.PL.ViewModels.User
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Username Is Required")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name Is Required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name Is Required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Password Is Required")]
        [MinLength(5, ErrorMessage = "Minimum Password Length is 5")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password didn't matched")]
        public string ConfirmPassword { get; set; }

        public bool IsAgree { get; set; }
    }
}
