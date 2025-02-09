namespace PgcSharedLib.Rules;

/// <summary>
/// The list of <see cref="IRule{T, TError}"/>.
/// </summary>
/// <typeparam name="T">The type of the target object to which the rule applies.</typeparam>
/// <typeparam name="TError">The type of the error object indicating the rule fails.</typeparam>
/// <remarks>This is a thread-safe list.</remarks>
public class RuleList<T, TError> : SynchronizedList<IRule<T, TError>>
{
    /// <summary>
    /// Applies the rules with the given rule name to the target object with lazy evaluation.
    /// </summary>
    /// <param name="ruleName">The name of rule to apply.</param>
    /// <param name="target">The target object to check.</param>
    /// <returns>The enumerable to error if rule fails.</returns>
    public IEnumerable<TError> Apply(string ruleName, T? target)
    {
        var rulesByName = this.Where(x => x.Name == ruleName);
        foreach (var rule in rulesByName)
        {
            var applied = rule.Apply(target);
            if (applied.IsPassed == false)
            {
                yield return applied.Error;
            }
        }
    }

    /// <summary>
    /// Applies all rules to the target object with lazy evaluation.
    /// </summary>
    /// <param name="target">The target object to check.</param>
    /// <returns>The enumerable to error if rule fails.</returns>
    public IEnumerable<TError> Apply(T? target)
    {
        var allRules = this.ToArray();
        foreach (var rule in allRules)
        {
            var applied = rule.Apply(target);
            if (applied.IsPassed == false)
            {
                yield return applied.Error;
            }
        }
    }
}
