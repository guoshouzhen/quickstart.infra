using QuickStart.Infra.DI.Enums;

namespace QuickStart.Infra.DI.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentAttribute : Attribute
    {
        /// <summary>
        /// 组件生命周期
        /// </summary>
        public LifeCycleEnum LifeCycleEnum { get; set; } = LifeCycleEnum.InstancePerLifetimeScope;
        /// <summary>
        /// 组件注入顺序，越小越先被注入
        /// </summary>
        public int Order { get; set; } = int.MaxValue;
        /// <summary>
        /// 组件对外暴露的服务，以指定的服务为准，不指定则自动扫描
        /// </summary>
        public Type[]? ExposeServices { get; set; }
        /// <summary>
        /// 是否将组件自身注册为服务
        /// </summary>
        public bool RegisterSelf { get; set; } = false;
    }
}
