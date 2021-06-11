using Microsoft.AspNetCore.Identity;
using Moba.Domain.Core.Interfaces;

namespace Moba.Domain.Entities.Security
{
    public class UserRole : IdentityUserRole<string>, IEntity
    {
        #region  ctor
        public UserRole()
        {
            
        }
        #endregion

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }

    }
}