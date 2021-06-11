using System;
using System.Collections.Generic;
using MPS.Domain.Core.Services;
using Microsoft.AspNetCore.Identity;
using MPS.Domain.Core.Interfaces;

namespace MPS.Domain.Entities.Security
{
    public class Role : IdentityRole<string>, IRegisterer<string,User>, IModifier<string,User> , ISoftDelete, IEntity
    {
        #region  ctor
        public Role()
        {
            Id = IdGenerator.NewGuid();
        }

        public Role(string role) : base(role)
        {
            Id = IdGenerator.NewGuid();
        }
        #endregion

        public string Description { get; set; }

        public DateTime RegisterDate { get; set; }
        public string RegistererId { get; set; }
        public virtual User RegistererUser { get; set; }

        public DateTime LastModifiedDate { get; set; }
        public string ModifierId { get; set; }
        public virtual User ModifierUser { get; set; }
        public bool IsDeleted { get; set; }


        #region Navigation
        public ICollection<UserRole> UsersInThisRole { get; set; }
        #endregion
    }
}