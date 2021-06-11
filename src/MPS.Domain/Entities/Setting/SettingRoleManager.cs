using System;
using MPS.Domain.Core.Interfaces;
using MPS.Domain.Core.Services;
using MPS.Domain.Core;
using MPS.Domain.Entities.Security;

namespace MPS.Domain.Entities.Setting
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