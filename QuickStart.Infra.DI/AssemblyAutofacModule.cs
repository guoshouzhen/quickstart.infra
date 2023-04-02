using Autofac;
using QuickStart.Infra.DI.Models;

namespace QuickStart.Infra.DI
{
    internal sealed class AssemblyAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            ComponentLoader.LoadAndRegisterComponents(builder, AssemblyContainer.GetRegisteredAssemblies());
        }
    }
}
