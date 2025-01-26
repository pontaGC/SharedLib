using System.Diagnostics.CodeAnalysis;

namespace PgcSharedLib.Rules;

/// <summary>
/// One rule that a target object must satisfy.
/// </summary>
/// <typeparam name="TTarget">The type of object to check.</typeparam>
/// <typeparam name="TError">The type of error if rule fails.</typeparam>
public interface IRule<in TTarget, TError>
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
    RuleResult<TError> Apply(TTarget? target);
}
