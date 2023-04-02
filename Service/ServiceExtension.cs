using Microsoft.Extensions.DependencyInjection;
using QuickStart.Infra.DI.Models;
using System.Reflection;

namespace Service
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddService(this IServiceCollection services) 
        {
            AssemblyContainer.RegisterAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
