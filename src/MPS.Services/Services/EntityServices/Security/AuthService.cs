using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moba.Common.ViewModels.Security;
using Moba.Data.EF.Interfaces.UOW;
using Moba.Domain.Entities.Security;
using Moba.Services.Interfaces.EntityServices.Security;
// ReSharper disable PossibleNullReferenceException

namespace Moba.Services.Services.EntityServices.Security
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapper mapper, IUnitOfWork uow) : base(uow, mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> IsLoginAsync()
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated && (await _userManager.FindByIdAsync(GetCurrentUserId().ToString())) != null;
        }

        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<UserViewModel> GetCurrentUser()
        {
            if (!await IsLoginAsync()) return null;
            var user = await _userManager.FindByIdAsync(GetCurrentUserId());
            if (user.IsDeleted)
                await LogOutAsync();
            else
                return _mapper.Map<UserViewModel>(user);
            return null;
        }

        public async Task<IEnumerable<IdentityError>> RegisterUserAsync(User xUser, string xPassword)
        {
            var result = await _userManager.CreateAsync(xUser, xPassword);
            return !result.Succeeded ? result.Errors : Enumerable.Empty<IdentityError>();
        }

        public async Task<IEnumerable<IdentityError>> ResetPasswordAsync(User xUser, string xNewPassword)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(xUser);
            var result = await _userManager.ResetPasswordAsync(xUser, token, xNewPassword);
            return !result.Succeeded ? result.Errors : Enumerable.Empty<IdentityError>();
        }
        public async Task Login(AccountLoginViewModel model)
        {
            await _signInManager.PasswordSignInAsync(model.PhoneNumber.Trim(), model.Password.Trim(), false, true);
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> ResetMemberPasswordAsync(User xUser, string xNewPassword, string xOldPassword)
        {
            var xUserName = await _userManager.FindByNameAsync(xUser.UserName);
            if (!await _userManager.CheckPasswordAsync(xUserName, xOldPassword)) return false;
            var token = await _userManager.GeneratePasswordResetTokenAsync(xUserName);
            await _userManager.ResetPasswordAsync(xUserName, token, xNewPassword);
            return true;
        }
    }
}