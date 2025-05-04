using System.Diagnostics.CodeAnalysis;

namespace SharedLib.Rules;

/// <summary>
/// One rule that a target object must satisfy.
/// </summary>
/// <typeparam name="T">The type of object to check.</typeparam>
/// <typeparam name="TError">The type of error if rule fails.</typeparam>
public interface IRule<in T, TError>
{
    /// <summary>
    /// Gets a rule name.
    /// </summary>
    [NotNull]
    string Name { get; }

    /// <summary>
    /// Checks whether the given object satisfies this rule.
    /// </summary>
    /// <param name="target">The target object to check.</param>
    /// <returns>The checked result.</returns>
    RuleResult<TError> Apply(T? target);
}
