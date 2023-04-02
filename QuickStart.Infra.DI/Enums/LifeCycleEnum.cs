namespace QuickStart.Infra.DI.Enums
{
    public enum LifeCycleEnum
    {
        /// <summary>
        /// 瞬时
        /// </summary>
        InstancePerDependency = 1,
        /// <summary>
        /// 作用域单例
        /// </summary>
        InstancePerLifetimeScope = 2,
        /// <summary>
        /// 单例
        /// </summary>
        SingleInstance = 3,
    }
}
