using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MPS.Common.ViewModels.Security;
using MPS.Domain.Entities.Security;

namespace MPS.Services.Interfaces.EntityServices.Security
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