namespace PgcSharedLib.Rules;

/// <summary>
/// The list of <see cref="IRule{T}"/>.
/// </summary>
/// <typeparam name="T">The type of an object to check.</typeparam>
/// <remarks>This is a thread-safe list.</remarks>
public class RuleList<T> : SynchronizedList<IRule<T>>
{
    /// <summary>
    /// Applies the rules with the given rule name to the target object with lazy evaluation.
    /// </summary>
    /// <param name="ruleName">The name of rule to apply.</param>
    /// <param name="target">The target object to check.</param>
    /// <returns>The enumerable to error if rule fails.</returns>
    public IEnumerable<string> Apply(string ruleName, T? target)
    {
        var rulesByName = this.Where(x => x.Name == ruleName).ToArray();
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
    public IEnumerable<string> Apply(T? target)
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
