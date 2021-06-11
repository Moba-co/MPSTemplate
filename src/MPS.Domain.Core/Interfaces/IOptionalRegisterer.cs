using System;

namespace Moba.Domain.Core.Interfaces
{
    public interface IOptionalRegisterer<TRegistererId, TUser>
        where TRegistererId : struct
    {
        DateTime? RegisterDate { get; set; }
        TRegistererId? RegistererId { get; set; }
        TUser RegistererUser { get; set; }
    }
}