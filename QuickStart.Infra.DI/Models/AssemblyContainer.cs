using System.Reflection;

namespace QuickStart.Infra.DI.Models
{
    /// <summary>
    /// Assembly container, for caching sacnned assemblies.
    /// </summary>
    public class AssemblyContainer
    {
        private static IList<Assembly> assemblies = new List<Assembly>();

        /// <summary>
        /// Add a assembly into container.
        /// </summary>
        /// <param name="assembly"></param>
        public static void RegisterAssembly(Assembly assembly)
        {
            assemblies.Add(assembly);
        }

        /// <summary>
        /// Get all cached assemblies.
        /// </summary>
        /// <returns></returns>
        public static IList<Assembly> GetRegisteredAssemblies()
        {
            return assemblies;
        }
    }
}
