using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PWChange2.Models
{
    public partial class User
    {
        [Required(ErrorMessage = "Required")]
        [Display(Name = "User Name")]
        public String UserName { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        public String CurrentPassword { get; set; }
        [Required(ErrorMessage = "Required")]
        [MinLength(8, ErrorMessage = "8 Caracters or more must be entered into this field")]
        [RegularExpression("(?=.*[A-Z])(?=.*\\d)(?!.*(.)\\1\\1)[a-zA-Z0-9_!./#$\\*]{0,30}$", ErrorMessage = "Password Does Not meet complexity Requirements")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public String NewPassword { get; set; }
        [Required(ErrorMessage = "Required")]
        [MinLength(8, ErrorMessage = "8 Caracters or more must be entered into this field")]
        [Compare("NewPassword", ErrorMessage = "New Password and Confirm Password Fields are not the same, please re-enter")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public String ConfirmPassword { get; set; }




        public Boolean confirmNewPwEqualsConfirmPw()
        {
            if (this.NewPassword.Equals(this.ConfirmPassword))
            {
                return true;
            }
            return false;
        }

    }
}


