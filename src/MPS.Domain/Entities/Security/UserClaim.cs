using Microsoft.AspNetCore.Identity;
using Moba.Domain.Core.Interfaces;

namespace Moba.Domain.Entities.Security
{
    public class UserClaim : IdentityUserClaim<string>, IEntity
    {
        #region  ctor
        public UserClaim()
        {

        }
        #endregion


    }
}