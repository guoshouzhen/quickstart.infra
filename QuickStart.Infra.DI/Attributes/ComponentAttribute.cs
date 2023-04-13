using QuickStart.Infra.DI.Enums;

namespace QuickStart.Infra.DI.Attributes
{
    /// <summary>
    /// All compnents marked with this attribute will be injected into IOC container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentAttribute : Attribute
    {
        /// <summary>
        /// Conponent life cycle enum.
        /// </summary>
        public LifeCycleEnum LifeCycleEnum { get; set; } = LifeCycleEnum.InstancePerLifetimeScope;
        /// <summary>
        /// Injection order, component that the smaller will be injected earlier.
        /// </summary>
        public int Order { get; set; } = int.MaxValue;
        /// <summary>
        /// Exposed services by the component, if not specified, will be scanned automatically.
        /// </summary>
        public Type[]? ExposeServices { get; set; }
        /// <summary>
        /// Whether to register itself as a service.
        /// </summary>
        public bool RegisterSelf { get; set; } = false;
    }
}
