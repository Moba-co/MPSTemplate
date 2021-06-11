using System.Collections.Generic;

namespace MPS.Data.EF.Services
{
    public class PagingResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public long Count { get; set; }
    }
}