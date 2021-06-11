using Microsoft.AspNetCore.Identity;
using Moba.Domain.Core.Interfaces;

namespace Moba.Domain.Entities.Security
{
    public class RoleClaim : IdentityRoleClaim<string>, IEntity
    {
        #region  ctor
        public RoleClaim()
        {
        }
        #endregion

    }
}