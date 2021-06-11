using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Moba.Common.Attributes;
using Moba.Domain.Entities.Security;

namespace Moba.Common.ViewModels.Security
{
    [DtoFor(typeof(User))]
    public class UserViewModel
    {
        #region --Fields--
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Id { get; set; }
        
        public string Username { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime? BirthDate { get; set; }
        public bool IsActive { get; set; }
        public string PhoneNumber { get; set; }
        public string BirthDatePersian { get; set; }
        public bool Gender { get; set; }
        public bool IsDeleted { get; set; }


        public string ProfileImage { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "گذرواژه و تکرار ان با هم یکسان نیستند")]
        public string ConfirmPassword { get; set; }
        public IList<RoleViewModel> Roles { get; set; }

        public string Bio { get; set; }

        public string GetFullName => string.Concat(FirstName, " ", LastName);

        #endregion
    }
    
    public class UserSearchParameters
    {
        public string LastName { get; set; }= "";
        public string FirstName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string NationalCode { get; set; } = "";
        public string AddressText { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public string CityProvinceId { get; set; } = "";
        public string SortBy { get; set; } = "RegisterDate";
        public string SortType { get; set; } = "Des";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
        public string Id { get; set; } = "";

    }
}