using System;
using Moba.Domain.Core.Interfaces;
using Moba.Domain.Core.Services;
using Moba.Domain.Core;
using Moba.Domain.Entities.Security;

namespace Moba.Domain.Entities.Setting
{
    public class SettingRoleManager : BaseEntity<string>, IRegisterer<string, User>

    {
        #region ctor
        public SettingRoleManager()
        {
        }
        #endregion

        public string Value { get; set; }


        #region  Implementation_Interfaces
        public DateTime RegisterDate { get; set; }
        public string RegistererId { get; set; }
        public virtual User RegistererUser { get; set; }
        #endregion
    }
}