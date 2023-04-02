using System.Reflection;

namespace QuickStart.Infra.DI.Utils
{
    public class ReflectionUtil
    {
        /// <summary>
        /// 获取类型所有的父类和实现的接口
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
        /// 获取类型所有的父类，object类除外
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
        /// 获取所有实现的接口
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
