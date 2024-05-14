using System.ComponentModel.DataAnnotations;

namespace Attar.CRUD.PL.ViewModels.User
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
    }
}
