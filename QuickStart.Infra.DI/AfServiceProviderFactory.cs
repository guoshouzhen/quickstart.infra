using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace QuickStart.Infra.DI
{
    /// <summary>
    /// Implementation of the IServiceProviderFactory, used to take over the .netcore default IOC container.
    /// </summary>
    public sealed class AfServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var containerBuilder = new ContainerBuilder();
            //Register all component scanned.
            containerBuilder.RegisterModule<AssemblyAutofacModule>();
            //Register all services in service collection.
            containerBuilder.Populate(services);
            return containerBuilder;
        }

        /// <summary>
        /// Create a service provider instance.
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            if (containerBuilder == null)
            {
                throw new ArgumentNullException(nameof(containerBuilder));
            }
            return new AutofacServiceProvider(containerBuilder.Build());
        }
    }
}
