using System;
using System.Linq;
using MPS.Services.Interfaces.RoleManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using MPS.Data.EF.Interfaces.UOW;
using MPS.Domain.Entities.Security;

namespace MPS.Services.Services.RoleManager
{
    public class DynamicRoleManager : AuthorizationHandler<DynamicRoleRequirement>
    {
        #region ctor
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRoleManagerHelper _roleManagerHelper;
        private readonly IMemoryCache _memoryCache;
        private readonly IDataProtector _protectorToken;

        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _context;
        private readonly SignInManager<User> _signInManager;

        public DynamicRoleManager(IHttpContextAccessor contextAccessor, IRoleManagerHelper roleManagerHelper, IMemoryCache memoryCache,
         IDataProtectionProvider dataProtectionProvider,
         UserManager<User> userManager, IUnitOfWork context, SignInManager<User> signInManager)
        {
            _contextAccessor = contextAccessor;
            _roleManagerHelper = roleManagerHelper;
            _memoryCache = memoryCache;
            _protectorToken = dataProtectionProvider.CreateProtector("RoleValidationGuid");
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }
        #endregion

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DynamicRoleRequirement requirement)
        {
            var httpContext = _contextAccessor.HttpContext;
            if (httpContext == null) return;
            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return;

            var roleValidationGuid = _memoryCache.GetOrCreate("RoleValidationGuid", p =>
            {
                p.AbsoluteExpiration = DateTimeOffset.MaxValue;
                return  _roleManagerHelper.DataBaseRoleValidationGuid();
            });

            SplitUserRequestedUrl(httpContext, out var areaAndActionAndControllerName);

            UnprotectRvgCookieData(httpContext, out var unprotectedRvgCookie);

            if (!IsRoleValidationGuidCookieDataValid(unprotectedRvgCookie, userId,await roleValidationGuid))
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return;

                AddOrUpdateRvgCookie(httpContext,await roleValidationGuid, userId);
            }
            else if (httpContext.User.HasClaim(areaAndActionAndControllerName, true.ToString()))
                context.Succeed(requirement);

            return;
        }

        #region Hellper

        private void SplitUserRequestedUrl(HttpContext httpContext, out string areaAndControllerAndActionName)
        {
            var areaName = httpContext.Request.RouteValues["area"]?.ToString() ?? "NoArea";
            var controllerName = httpContext.Request.RouteValues["controller"] + "Controller";
            var actionName = httpContext.Request.RouteValues["action"]?.ToString();
            areaAndControllerAndActionName = $"{areaName}|{controllerName}|{actionName}".ToUpper();
        }

        private void UnprotectRvgCookieData(HttpContext httpContext, out string unprotectedRvgCookie)
        {
            var protectedRvgCookie = httpContext.Request.Cookies
                .FirstOrDefault(t => t.Key == "RVG").Value;
            unprotectedRvgCookie = null;
            if (string.IsNullOrEmpty(protectedRvgCookie)) return;
            try
            {
                unprotectedRvgCookie = _protectorToken.Unprotect(protectedRvgCookie);
            }
            catch (CryptographicException)
            {
            }
        }

        private bool IsRoleValidationGuidCookieDataValid(string roleValidationGuidCookieData, string validUserId, string validRoleValidationGuid)
            => !string.IsNullOrEmpty(roleValidationGuidCookieData) &&
               SplitUserIdFromRoleValidationGuidCookie(roleValidationGuidCookieData) == validUserId &&
               SplitRoleValidationGuidFromRoleValidarionGuidCookie(roleValidationGuidCookieData) == validRoleValidationGuid;

        private string SplitUserIdFromRoleValidationGuidCookie(string roleValidationGuidCookieData)
            => roleValidationGuidCookieData.Split("|||")[1];

        private string SplitRoleValidationGuidFromRoleValidarionGuidCookie(string roleValidationGuidCookieData)
            => roleValidationGuidCookieData.Split("|||")[0];

        private string CombineRvgWithUserId(string roleValidationGuid, string userId)
            => roleValidationGuid + "|||" + userId;

        private void AddOrUpdateRvgCookie(HttpContext httpContext, string validroleValidationGuid, string validUserId)
        {
            var rvgWithUserId = CombineRvgWithUserId(validroleValidationGuid, validUserId);
            var protectedRoleValidationGuidWithUserId = _protectorToken.Protect(rvgWithUserId);
            httpContext.Response.Cookies.Append("RVG", protectedRoleValidationGuidWithUserId,
                new CookieOptions
                {
                    MaxAge = TimeSpan.FromDays(90),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax
                });
        }


        #endregion
    }
}