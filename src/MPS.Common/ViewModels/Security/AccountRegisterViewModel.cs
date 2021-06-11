using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MPS.Common.Attributes;
using MPS.Domain.Entities.Security;

namespace MPS.Common.ViewModels.Security
{
    public class RegisterWithPhoneViewModel
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "شماره تلفن")]
        [Remote("IsPhoneNumberInUse", "Auth", AdditionalFields = "__RequestVerificationToken", HttpMethod = "POST")]
        [RegularExpression(@"(\+98|0)?9\d{9}",ErrorMessage = "لطفه شماره معتبر وارد نمایید")]
        public string PhoneNumber { get; set; }
        public bool RulesChecked { get; set; }
    }
    
    [DtoFor(typeof(User))]
    public class AccountRegisterViewModel
    {
        // [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        // [Display(Name = "نام کاربری")]
        // [Remote("IsUserNameInUse", "Auth", AdditionalFields = "__RequestVerificationToken", HttpMethod = "POST")]
        // public string UserName { get; set; }

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