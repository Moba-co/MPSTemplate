using System;
namespace MPS.Domain.Core.Services
{
    public static class IdGenerator
    {
        public static string NewGuid(int count = 0)
        {
            var id = count == 0 ? Guid.NewGuid().ToString().Replace("-", "") : Guid.NewGuid().ToString().Replace("-", "").Substring(0, count);
            return id;
        }
    }
}