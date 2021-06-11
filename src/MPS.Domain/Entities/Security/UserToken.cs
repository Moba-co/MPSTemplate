using Microsoft.AspNetCore.Identity;
using MPS.Domain.Core.Interfaces;

namespace MPS.Domain.Entities.Security
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