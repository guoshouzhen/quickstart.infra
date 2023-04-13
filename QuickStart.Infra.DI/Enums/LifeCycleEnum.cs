namespace QuickStart.Infra.DI.Enums
{
    public enum LifeCycleEnum
    {
        /// <summary>
        /// Transient.
        /// </summary>
        InstancePerDependency = 1,
        /// <summary>
        /// Scoped.
        /// </summary>
        InstancePerLifetimeScope = 2,
        /// <summary>
        /// Singleton.
        /// </summary>
        SingleInstance = 3,
    }
}
