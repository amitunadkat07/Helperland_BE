using System.ComponentModel.DataAnnotations;

namespace Helperland.Entity.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is Required!")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email Address!")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is Required!")]
        public string? Password { get; set; }
    }
}
