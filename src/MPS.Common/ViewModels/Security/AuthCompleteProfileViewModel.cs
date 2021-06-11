using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MPS.Common.Attributes;
using MPS.Domain.Entities.Security;

namespace MPS.Common.ViewModels.Security
{
    [DtoFor(typeof(User))]
    public class AuthCompleteProfileViewModel
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50,ErrorMessage = "بیشتر از 50 حروف نمی توانید وارد کنید")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50,ErrorMessage = "بیشتر از 50 حروف نمی توانید وارد کنید")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string BirthDate { get; set; }

        public string ProfileImage { get; set; }

        public string Gender { get; set; }
    }
}
