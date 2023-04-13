using System.Reflection;

namespace QuickStart.Infra.DI.Utils
{
    public class ReflectionUtil
    {
        /// <summary>
        /// Get all parent classes and implemented interfaces of a type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetParentTypes(Type type)
        {
            if (type == null)
            {
                yield break;
            }
            foreach (var item in GetSuperClass(type))
            {
                yield return item;
            }
            foreach (var item in GetImplementedInterfaces(type))
            {
                yield return item;
            }
        }

        /// <summary>
        /// Get all parent classes of a type, except the object class.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetSuperClass(Type type)
        {
            var currBaseType = type.BaseType;
            while (currBaseType != null && currBaseType != typeof(object))
            {
                yield return currBaseType;
                currBaseType = currBaseType.BaseType;
            }
        }

        /// <summary>
        /// Get all implemented interfaces of a type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetImplementedInterfaces(Type type)
        {
            if (type.IsGenericTypeDefinition)
            {
                return type.GetTypeInfo().ImplementedInterfaces.Where(x => x.IsGenericType).Select(x => x.GetGenericTypeDefinition());
            }
            else
            {
                return type.GetTypeInfo().ImplementedInterfaces.Where(x => x != typeof(IDisposable));
            }
        }
    }
}
