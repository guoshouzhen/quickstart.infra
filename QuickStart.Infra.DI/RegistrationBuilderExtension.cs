using Autofac.Builder;
using QuickStart.Infra.DI.Enums;

namespace QuickStart.Infra.DI
{
    internal static class RegistrationBuilderExtension
    {
        /// <summary>
        /// Register exposed services by component.
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
        /// Configure component life cycle.
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
                //Transient.
                case LifeCycleEnum.InstancePerDependency:
                    rb.InstancePerDependency();
                    break;
                //Singleton.
                case LifeCycleEnum.SingleInstance:
                    rb.SingleInstance();
                    break;
                //Scoped.(Default)
                default:
                    rb.InstancePerLifetimeScope();
                    break;
            }

            return rb;
        }
    }
}
