using QuickStart.Infra.DI.Enums;

namespace QuickStart.Infra.DI.Models
{
    internal struct ComponentDetail
    {
        public Type ComponentType { get; set; }
        public LifeCycleEnum LifeCycleEnum { get; set; }
        public bool IsRegisterSelf { get; set; }
        public Type[] ExposeServiceTypes { get; set; }
        public int Order { get; set; }
    }
}
