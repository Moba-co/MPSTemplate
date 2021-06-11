using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Moba.Common.ViewModels.Security;
using Moba.Domain.Entities.Security;

namespace Moba.Services.Interfaces.EntityServices.Security
{
    public interface IAuthService
    {
        string GetCurrentUserId();
        Task<bool> IsLoginAsync();
        Task Login(AccountLoginViewModel model);
        Task LogOutAsync();
        Task<IEnumerable<IdentityError>> RegisterUserAsync(User xUser, string xPassword);
        Task<IEnumerable<IdentityError>> ResetPasswordAsync(User xUser, string xNewPassword);
        Task<UserViewModel> GetCurrentUser();
        Task<bool> ResetMemberPasswordAsync(User xUser, string xNewPassword, string xOldPassword);
    }
}