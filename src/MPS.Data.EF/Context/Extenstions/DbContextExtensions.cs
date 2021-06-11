using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace MPS.Data.EF.Context.Extenstions
{
    public static class DbContextExtensions
    {
        public static void AddDbSetFromModel(this ModelBuilder modelBuilder, Assembly targetAssembly, Type tEntity)
        {
            foreach (var currentAssembly in targetAssembly.GetTypes().Where(w => !w.IsAbstract && !w.IsInterface))
            {
                if (!currentAssembly.IsAbstract && !currentAssembly.IsInterface && tEntity.IsAssignableFrom(currentAssembly))
                    modelBuilder.Entity(currentAssembly);
            }
        }
    }
}