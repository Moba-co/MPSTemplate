using Microsoft.AspNetCore.Identity;
using MPS.Domain.Core.Interfaces;

namespace MPS.Domain.Entities.Security
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