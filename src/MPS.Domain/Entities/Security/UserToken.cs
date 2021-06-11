using Microsoft.AspNetCore.Identity;
using Moba.Domain.Core.Interfaces;

namespace Moba.Domain.Entities.Security
{
    public class UserToken : IdentityUserToken<string> , IEntity
    {
        #region  ctor
        public UserToken()
        {

        }
        #endregion


    }
}