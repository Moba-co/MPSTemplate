using Moba.Common.Attributes;
using Moba.Common.ViewModels.Security.RoleManager;
using Moba.Domain.Entities.Security;
using System.Collections.Generic;

namespace Moba.Common.ViewModels.Security
{
    [DtoFor(typeof(Role))]
    public class RoleViewModel
    {
        public RoleViewModel()
        {
            ActionAndControllerNames = new List<ActionAndControllerNameViewModel>();
        }
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description {get;set;}
        
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSelected { get; set; }

        public IList<ActionAndControllerNameViewModel> ActionAndControllerNames { get; set; }

    }
    public class RoleSearchParameters
    {
        public string Name { get; set; } = "";
        public string SortBy { get; set; } = "RegisterDate";
        public string SortType { get; set; } = "Des";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
        public string Id { get; set; } = "";


    }
}