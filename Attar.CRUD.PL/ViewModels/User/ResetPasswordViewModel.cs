using System.ComponentModel.DataAnnotations;

namespace Attar.CRUD.PL.ViewModels.User
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Password Is Required")]
        [MinLength(5, ErrorMessage = "Minimum Password Length is 5")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Password didn't matched")]
        public string ConfirmNewPassword { get; set; }
    }
}
