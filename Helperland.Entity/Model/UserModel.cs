using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helperland.Entity.Model
{
    public class UserModel
    {
        [Required(ErrorMessage = "First Name is Required!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First Name must contain atleast 3 letters!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name can't contain digits!")]
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "Last Name is Required!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Last Name must contain atleast 3 letters!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name can't contain digits!")]
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "Contact Number is Required!")]
        public string? Contact { get; set; }

        [Required(ErrorMessage = "Email is Required!")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email Address!")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is Required!")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required!")]
        [Compare("Password", ErrorMessage = "Password and confirm password must be equal.")]
        public string? Confpassword { get; set; }

        public int Roleid { get; set; }
    }
}
