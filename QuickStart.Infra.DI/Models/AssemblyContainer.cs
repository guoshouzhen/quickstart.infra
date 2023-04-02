using System.Reflection;

namespace QuickStart.Infra.DI.Models
{
    public class AssemblyContainer
    {
        private static IList<Assembly> assemblies = new List<Assembly>();

        public static void RegisterAssembly(Assembly assembly)
        {
            assemblies.Add(assembly);
        }

        public static IList<Assembly> GetRegisteredAssemblies()
        {
            return assemblies;
        }
    }
}
