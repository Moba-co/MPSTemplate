using Microsoft.AspNetCore.Identity;
using MPS.Domain.Core.Interfaces;

namespace MPS.Domain.Entities.Security
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