using QuickStart.Infra.DI.Enums;

namespace QuickStart.Infra.DI.Models
{
    /// <summary>
    /// Component details, used when injecting.
    /// </summary>
    internal struct ComponentDetail
    {
        /// <summary>
        /// Component Type info.
        /// </summary>
        public Type ComponentType { get; set; }
        /// <summary>
        /// Component life cycle.
        /// </summary>
        public LifeCycleEnum LifeCycleEnum { get; set; }
        /// <summary>
        /// Whether to register itself as a service.
        /// </summary>
        public bool IsRegisterSelf { get; set; }
        /// <summary>
        /// Exposed services by the component.
        /// </summary>
        public Type[] ExposeServiceTypes { get; set; }
        /// <summary>
        /// Injection order.
        /// </summary>
        public int Order { get; set; }
    }
}
