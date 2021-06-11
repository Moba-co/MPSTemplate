using MPS.Common.DTOs;
using MPS.Common.ViewModels.Security;
using MPS.Data.EF.Helpers;
using MPS.Data.EF.Services;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace MPS.Services.Interfaces.EntityServices.Security
{
    public interface IRoleService
    {
        RoleViewModel GetByName(string roleName);
        Task<ReturnMessageDto> AddAsync(RoleViewModel model);
        Task DeleteAsync(string id);
        Task<RoleViewModel> GetByIdAsync(string id);
        Task<ReturnMessageDto> EditAsync(RoleViewModel model);
        Task<PagingResult<TViewModel>> GetsAsync<TViewModel>(bool needDbCount, RoleSearchParameters prameters = null, MapBy<TViewModel> mapBy = null);
        Task<List<GeneralSelectListItem>> GetForSelectListAsync();
        Task SoftDelete(RoleViewModel model);
        RoleViewModel GetActionAndControllerName(Assembly assembly, string id = null);
        Task<ReturnMessageDto> UpdateValidationGuid();

    }
}
