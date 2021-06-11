using System;

namespace MPS.Domain.Core.Interfaces
{
    public interface IOptionalModifier<TModifierId,TUser>
        where TModifierId: struct
        where TUser : struct
    {
        DateTime? LastModifiedDate { get; set; }
        TModifierId? ModifierId { get; set; }
        TUser? ModifierUser { get; set; }
    }
}