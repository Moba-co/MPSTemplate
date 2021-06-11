using Microsoft.AspNetCore.Identity;
using Moba.Domain.Core.Interfaces;

namespace Moba.Domain.Entities.Security
{
    public class UserLogin : IdentityUserLogin<string>, IEntity
    {
        #region  ctor
        public UserLogin()
        {

        }
        #endregion

    }
}