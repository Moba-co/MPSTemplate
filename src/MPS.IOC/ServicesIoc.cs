using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using MPS.Services.Services;

namespace MPS.IOC
{
    public static class ServicesIoc
    {
        public static void  AddServices(this IServiceCollection services)
        {
            var allServices = typeof(BaseService).Assembly.GetTypes()
                .Where(w => !w.IsAbstract && !w.IsInterface && typeof(BaseService).IsAssignableFrom(w) && w != typeof(BaseService)).ToList();
            foreach (var item in allServices)
            {
                var currentInterface = item.GetInterfaces().FirstOrDefault(f => f.Name.Contains(item.Name));
                if(currentInterface != null)
                {
                    services.AddScoped(currentInterface, item);
                }
            }
        }
    }
}