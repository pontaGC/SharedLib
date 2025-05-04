namespace SharedLib.DependencyInjection
{
    /// <summary>
    /// Life style of a registered instance in the <see cref="IDIContainer"/>.
    /// </summary>
    public enum InstanceLifeStyle
    {
        Transient,
        Singleton,
    }
}
