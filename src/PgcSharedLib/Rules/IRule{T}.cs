namespace PgcSharedLib.Rules
{
    /// <summary>
    /// One rule that a target object must satisfy.
    /// The <c>Error</c> property can be used as an error message.
    /// </summary>
    /// <typeparam name="T">The type of object to check.</typeparam>
    public interface IRule<T> : IRule<T, string>
    {
    }
}
