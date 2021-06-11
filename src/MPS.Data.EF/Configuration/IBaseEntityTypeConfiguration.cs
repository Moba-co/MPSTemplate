using Microsoft.EntityFrameworkCore;

namespace MPS.Data.EF.Configuration
{
    public interface IBaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : class
    {
        
    }
}