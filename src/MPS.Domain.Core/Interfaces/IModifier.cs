using System;

namespace MPS.Domain.Core.Interfaces
{
    public interface IModifier<TRegistererId,TUser>
    {
        DateTime LastModifiedDate { get; set; }
        TRegistererId ModifierId { get; set; }
        TUser ModifierUser { get; set; }
    }
}