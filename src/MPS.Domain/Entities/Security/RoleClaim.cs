using Microsoft.AspNetCore.Identity;
using MPS.Domain.Core.Interfaces;

namespace MPS.Domain.Entities.Security
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