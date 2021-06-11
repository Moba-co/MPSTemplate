using Microsoft.EntityFrameworkCore;

namespace Moba.Data.EF.Configuration
{
    public interface IBaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : class
    {
        
    }
}