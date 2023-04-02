using Autofac.Builder;
using QuickStart.Infra.DI.Enums;

namespace QuickStart.Infra.DI
{
    internal static class RegistrationBuilderExtension
    {
        /// <summary>
        /// 注册对外暴露服务
        /// </summary>
        /// <typeparam name="TActivatorData"></typeparam>
        /// <typeparam name="TRegistrationStyle"></typeparam>
        /// <param name="rb"></param>
        /// <param name="exposeTypes"></param>
        /// <returns></returns>
        public static IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> RegisterExposeService<TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> rb, Type[] exposeTypes)
        {
            if (exposeTypes != null)
            {
                foreach (var type in exposeTypes)
                {
                    rb.As(type);
                }
            }

            return rb;
        }

        /// <summary>
        /// 配置组件生命周期
        /// </summary>
        /// <typeparam name="TActivatorData"></typeparam>
        /// <typeparam name="TRegistrationStyle"></typeparam>
        /// <param name="rb"></param>
        /// <param name="lifeCycleEnum"></param>
        /// <returns></returns>
        public static IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> SetComponentLifecycle<TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> rb, LifeCycleEnum lifeCycleEnum)
        {
            switch (lifeCycleEnum)
            {
                //瞬时单例
                case LifeCycleEnum.InstancePerDependency:
                    rb.InstancePerDependency();
                    break;
                //单例
                case LifeCycleEnum.SingleInstance:
                    rb.SingleInstance();
                    break;
                //作用域单例，默认
                default:
                    rb.InstancePerLifetimeScope();
                    break;
            }

            return rb;
        }
    }
}
