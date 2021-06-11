using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using MPS.Domain.Core.Interfaces;
using MPS.Domain.Core.Services;
using MPS.Domain.Entities.Setting;

namespace MPS.Domain.Entities.Security
{
    public class User : IdentityUser<string>, IRegisterer<string, User>, IModifier<string, User>, ISoftDelete, IEntity
    {
        #region  ctor
        public User()
        {
            Id = IdGenerator.NewGuid();
            RegisterDate = DateTime.Now;
            LastModifiedDate = DateTime.Now;
        }
        #endregion

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string ProfileImage { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool Gender { get; set; }
        public bool IsActive { get; set; }
        public string Bio { get; set; }

        #region  Implementation_Interfaces

        public DateTime RegisterDate { get; set; }
        public string RegistererId { get; set; }
        public virtual User RegistererUser { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime LastModifiedDate { get; set; }
        public string ModifierId { get; set; }
        public User ModifierUser { get; set; }

        #endregion

        #region Relations

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<SettingRoleManager> RegisterdSettingRoleManeger { get; set; }
        #endregion

        public string GetFullName => string.Concat(FirstName, " ", LastName);
    }
}