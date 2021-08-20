using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using MPS.Common.DTOs;
using MPS.Common.Helpers;
using MPS.Common.ViewModels.Security;
using MPS.Data.EF.Helpers;
using MPS.Data.EF.Interfaces.UOW;
using MPS.Data.EF.Services;
using MPS.Domain.Core.Services;
using MPS.Domain.Entities.Security;
using MPS.Domain.Entities.Setting;
using MPS.Services.Interfaces.EntityServices.Security;
using MPS.Services.Interfaces.RoleManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MPS.Services.Services.EntityServices.Security
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly IAuthService _authService;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMemoryCache _memoryCache;
        private readonly IRoleManagerHelper _roleManagerHelper;
        public RoleService(IUnitOfWork uow, IMapper mapper, IAuthService authService,
             RoleManager<Role> roleManager, IMemoryCache memoryCache
            , IRoleManagerHelper roleManagerHelper) : base(uow, mapper)
        {
            _authService = authService;
            _roleManager = roleManager;
            _memoryCache = memoryCache;
            _roleManagerHelper = roleManagerHelper;
        }
        public RoleViewModel GetByName(string roleName)
        {
            return (_db.RoleRepository.Get<RoleViewModel>(a => a.Name == roleName)
                .FirstOrDefault());
        }
        public async Task DeleteAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            var roleClaim = _db.RoleClaimRepository.Get(p => p.RoleId == role.Id);
            foreach (var item in roleClaim)
            {
                await _db.RoleClaimRepository.DeleteAsync(item);
            }
            var roleUser= _db.UserRoleRepository.Get(p => p.RoleId == role.Id);
            foreach (var item in roleUser)
            {
                await _db.RoleClaimRepository.DeleteAsync(item);
            }
            await _roleManager.DeleteAsync(role);
        }

        public async Task<ReturnMessageDto> EditAsync(RoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
                return new ReturnMessageDto("نقش پیدا نشد", false, 0);
            
            //var currentUserId = _authService.GetCurrentUserId();
            //role.RegistererId = currentUserId;
            
            role.Name = model.Name;
            role.Description = model.Description;
            role.IsDeleted = model.IsDeleted;
            role.RegisterDate = DateTime.Now;
            var result=await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                var getError = "";
                foreach (var error in result.Errors)
                {
                    getError = error.Description;
                }
                return new ReturnMessageDto(getError, false, 0);
            }
            var requestRoles = model.ActionAndControllerNames.Where(c => c.IsSelected).ToList();
            var roleClaim = _db.RoleClaimRepository.Get(p => p.RoleId == model.Id);
            foreach(var item in roleClaim)
            {
                await _db.RoleClaimRepository.DeleteAsync(item);
            }
            foreach (var requestRole in requestRoles)
            {
                var areaName = (string.IsNullOrEmpty(requestRole.AreaName)) ?
                    "NoArea" : requestRole.AreaName;

                await _roleManager.AddClaimAsync(role,
                    new Claim($"{areaName}|{requestRole.ControllerName}|{requestRole.ActionName}".ToUpper(),
                        true.ToString()));

            }
            return new ReturnMessageDto($"با موفقیت {model.Name} ویرایش شد", true, 0);
        }

        public async Task<RoleViewModel> GetByIdAsync(string id)
        {
            return _mapper.Map<RoleViewModel>(await _roleManager.FindByIdAsync(id));
        }

        public async Task<PagingResult<TViewModel>> GetsAsync<TViewModel>(bool needDbCount, RoleSearchParameters prameters = null, MapBy<TViewModel> mapBy = null)
        {
            prameters ??= new RoleSearchParameters();

            #region OrderBy

            Func<IQueryable<Role>, IOrderedQueryable<Role>> orderBy = prameters.SortBy switch
            {
                "RegisterDate" when prameters.SortType == "Des" => p => p.OrderByDescending(x => x.RegisterDate),
                "RegisterDate" => p => p.OrderBy(x => x.RegisterDate),
                "Name" when prameters.SortType == "Des" => p => p.OrderByDescending(x => x.Name),
                "Name" => p => p.OrderBy(x => x.Name),
                _ => p => p.OrderByDescending(x => x.Id)
            };

            #endregion

            #region Filter

            Expression<Func<Role, bool>> filter = p => !p.IsDeleted;

            if (!string.IsNullOrWhiteSpace(prameters.Name))
            {
                filter = filter.And(p => p.Name.Trim().ToLower().Contains(prameters.Name.Trim().ToLower()));
            }
            if (prameters.Id != "")
            {
                filter = filter.And(p => p.Id != prameters.Id);
            }
            #endregion
            return await _db.RoleRepository.GetAsync(needDbCount, filter, orderBy, mapBy, prameters.Page, prameters.PageSize);
        }

        public async Task<List<GeneralSelectListItem>> GetForSelectListAsync()
        {
            #region OrderBy

            static IOrderedQueryable<Role> OrderBy(IQueryable<Role> p) => p.OrderByDescending(x => x.Name);

            #endregion

            return (await _db.RoleRepository.GetAsync(null, OrderBy)).Select(p => new GeneralSelectListItem
            {
                Text = p.Name,
                Value = p.Id
            }).ToList();
        }
        public async Task SoftDelete(RoleViewModel model)
        {
            model.IsDeleted = true;
            await EditAsync(model);
        }

        public async Task<ReturnMessageDto> AddAsync(RoleViewModel model)
        {
            var role = GetByName(model.Name);
            if (role != null)
                return new ReturnMessageDto("چنین نقشی وجود دارد", false, 0);
            var roleModel = _mapper.Map<Role>(model);
            var currentUserId = _authService.GetCurrentUserId();
            roleModel.RegisterDate = DateTime.Now;
            roleModel.RegistererId = currentUserId;
            roleModel.Id = IdGenerator.NewGuid();
            var result = await _roleManager.CreateAsync(roleModel);
            if (!result.Succeeded)
            {
                var getError = "";
                foreach (var error in result.Errors)
                {
                    getError = error.Description;
                }
                return new ReturnMessageDto(getError, false, 0);
            }
            var requestRoles = model.ActionAndControllerNames.Where(c => c.IsSelected).ToList();
            foreach (var requestRole in requestRoles)
            {
                var areaName = (string.IsNullOrEmpty(requestRole.AreaName)) ?
                    "NoArea" : requestRole.AreaName;

                await _roleManager.AddClaimAsync(roleModel,
                    new Claim($"{areaName}|{requestRole.ControllerName}|{requestRole.ActionName}".ToUpper(),
                        true.ToString()));
            }
            return new ReturnMessageDto("با موفقیت نفق اضافه شد", true, 0);
        }

        public RoleViewModel GetActionAndControllerName(Assembly assembly, string id = null)
        {
            var list = _memoryCache.GetOrCreate("AreaAndActionAndControllerNamesList", p =>
             {
                 p.AbsoluteExpiration = DateTimeOffset.MaxValue;
                 return _roleManagerHelper.AreaAndActionAndControllerNamesList(assembly);
             });
            if (id != null)
            {
                //The code needs to be improved here 
                var roleClaim = _db.RoleClaimRepository.Get(p => p.RoleId == id).Select(p => p.ClaimType);
                foreach (var claims in roleClaim)
                {
                    foreach (var item in list)
                    {
                        var areaName = (string.IsNullOrEmpty(item.AreaName)) ?
                            "NoArea" : item.AreaName;
                        var actionControllerName = $"{ areaName }|{ item.ControllerName}|{ item.ActionName}".ToUpper();
                        if (actionControllerName == claims)
                            item.IsSelected = true;
                    }
                }
            }
            return new RoleViewModel()
            {
                ActionAndControllerNames = list
            };
        }

        public async Task<ReturnMessageDto> UpdateValidationGuid()
        {
            var roleValidationGuidSiteSetting =
                         await _db.SettingRoleManagerRepository.GetFirstAsync(t => t.Id == "RoleValidationGuid");

            if (roleValidationGuidSiteSetting == null)
            {
                await _db.SettingRoleManagerRepository.InsertAsync(new SettingRoleManager()
                {
                    Id = "RoleValidationGuid",
                    Value = Guid.NewGuid().ToString(),
                    RegisterDate = DateTime.Now
                });
            }
            else
            {
                var currentUserId = _authService.GetCurrentUserId();
                roleValidationGuidSiteSetting.Value = Guid.NewGuid().ToString();
                roleValidationGuidSiteSetting.RegisterDate = DateTime.Now;
                roleValidationGuidSiteSetting.RegistererId = currentUserId;
                _db.SettingRoleManagerRepository.Update(roleValidationGuidSiteSetting);
            }
            await _db.SaveChangesAsync();
            _memoryCache.Remove("RoleValidationGuid");
            return new ReturnMessageDto("کوکی کاربر با موفقیت بروزرسانی شد", true, 0);
        }
    }
}
