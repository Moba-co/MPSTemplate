using System.ComponentModel.DataAnnotations;

namespace MPS.Common.ViewModels.Security
{
    public class ForgotPassWordViewModel
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "شماره تلفن")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public string Code { get; set; }
        
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "رمز عبوز")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تکرار کلمه عبور")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "گذرواژه و تکرار ان با هم یکسان نیستند")]
        public string ConfirmPassword { get; set; }
    }
}