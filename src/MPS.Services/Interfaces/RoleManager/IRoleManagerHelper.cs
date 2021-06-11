using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using MPS.Common.ViewModels.Security.RoleManager;

namespace MPS.Services.Interfaces.RoleManager
{
    public interface IRoleManagerHelper
    {
        public IList<ActionAndControllerNameViewModel> AreaAndActionAndControllerNamesList(Assembly assembly);
        public IList<string> GetAllAreasNames();
        public Task<string> DataBaseRoleValidationGuid();
    }
}