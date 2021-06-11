using System;
using System.Reflection.Metadata;

namespace MPS.Domain.Core.Interfaces
{
    public interface IRegisterer<TRegistererId, TUser>
    {
        DateTime RegisterDate { get; set; }
        TRegistererId RegistererId { get; set; }
        TUser RegistererUser { get; set; }
    }
}