using Autofac;
using QuickStart.Infra.DI.Attributes;
using QuickStart.Infra.DI.Models;
using QuickStart.Infra.DI.Utils;
using System.Reflection;

namespace QuickStart.Infra.DI
{
    internal sealed class ComponentLoader
    {
        /// <summary>
        /// 加载并注册组件
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblies"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void LoadAndRegisterComponents(ContainerBuilder builder, IList<Assembly> assemblies)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            var lstComponentDetails = ResolveComponents(assemblies);
            foreach (var component in lstComponentDetails)
            {
                if (component.ComponentType.IsGenericTypeDefinition)
                {
                    RegisterGenericComponent(builder, component);
                }
                else
                {
                    RegisterComponent(builder, component);
                }
            }
        }

        /// <summary>
        /// 解析要注册的组件
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static IList<ComponentDetail> ResolveComponents(IList<Assembly> assemblies)
        {
            IList<ComponentDetail> components = new List<ComponentDetail>();
            if (assemblies == null || assemblies.Count == 0)
            {
                return components;
            }

            foreach (Assembly assembly in assemblies)
            {
                var publicTypes = assembly.GetExportedTypes();
                if (publicTypes == null || publicTypes.Length == 0)
                {
                    continue;
                }
                foreach (Type type in publicTypes)
                {
                    var compomentAttr = type.GetCustomAttribute<ComponentAttribute>();
                    if (compomentAttr == null)
                    {
                        continue;
                    }
                    if (type.IsClass == false || type.IsAbstract)
                    {
                        throw new Exception($"以下组件不支持注入：{type.Name}，仅支持注入非抽象class类型组件");
                    }
                    if (type.IsSubclassOf(typeof(Delegate)))
                    {
                        throw new Exception($"委托类型组件不支持注入：{type.Name}，仅支持注入非抽象class类型组件");
                    }

                    //获取组件所有对外暴露服务
                    Type[] exposeServices;
                    if (compomentAttr.ExposeServices != null && compomentAttr.ExposeServices.Length > 0)
                    {
                        exposeServices = compomentAttr.ExposeServices;
                    }
                    else
                    {
                        exposeServices = ReflectionUtil.GetParentTypes(type).ToArray();
                    }
                    components.Add(new ComponentDetail()
                    {
                        ComponentType = type,
                        LifeCycleEnum = compomentAttr.LifeCycleEnum,
                        IsRegisterSelf = compomentAttr.RegisterSelf,
                        ExposeServiceTypes = exposeServices,
                        Order = compomentAttr.Order,
                    });
                }
            }

            return components.OrderBy(x => x.Order).ToList();
        }

        /// <summary>
        /// 注册泛型组件
        /// </summary>
        /// <param name="component"></param>
        private static void RegisterGenericComponent(ContainerBuilder builder, ComponentDetail component)
        {
            var rb = builder.RegisterGeneric(component.ComponentType);
            if (component.IsRegisterSelf)
            {
                rb.AsSelf();
            }
            rb.RegisterExposeService(component.ExposeServiceTypes)
                .PropertiesAutowired()
                .SetComponentLifecycle(component.LifeCycleEnum);

        }

        /// <summary>
        /// 注册组件
        /// </summary>
        /// <param name="component"></param>
        private static void RegisterComponent(ContainerBuilder builder, ComponentDetail component)
        {
            var rb = builder.RegisterType(component.ComponentType);
            if (component.IsRegisterSelf)
            {
                rb.AsSelf();
            }
            rb.RegisterExposeService(component.ExposeServiceTypes)
                .PropertiesAutowired()
                .SetComponentLifecycle(component.LifeCycleEnum);
        }
    }
}
