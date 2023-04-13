namespace QuickStart.Infra.DI.Attributes
{
    /// <summary>
    /// All compnents marked with this attribute will be injected into IOC container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RepositoryAttribute : ComponentAttribute { }
}
