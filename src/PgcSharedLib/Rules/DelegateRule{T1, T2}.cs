using System.Diagnostics.CodeAnalysis;

namespace PgcSharedLib.Rules;

/// <summary>
/// The delegate rule.
/// </summary>
public class DelegateRule<TTarget, TError> : IRule<TTarget, TError>
{
    private readonly Predicate<TTarget> applyRule;
    private readonly Func<TTarget, TError> getError;

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateRule{TTarget, TError}"/> class.
    /// </summary>
    /// <param name="ruleName">The rule name.</param>
    /// <param name="applyRule">The check function that the target object satisfy a rule.</param>
    /// <param name="getError">The function to get error if rule fails.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="ruleName"/> or <paramref name="applyRule"/> or <paramref name="getError"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException"><paramref name="ruleName"/> is an empty string.</exception>
    public DelegateRule(string ruleName, Predicate<TTarget> applyRule, Func<TTarget, TError> getError)
    {
        ArgumentException.ThrowIfNullOrEmpty(ruleName);
        ArgumentNullException.ThrowIfNull(applyRule);
        ArgumentNullException.ThrowIfNull(getError);

        Name = ruleName;
        this.applyRule = applyRule;
        this.getError = getError;
    }

    /// <inheritdoc />
    [NotNull]
    public string Name { get; }

    /// <inheritdoc />
    public RuleResult<TError> Apply(TTarget? target)
    {
        var passed = applyRule(target);
        if (passed)
        {
            return RuleResult<TError>.Passed(Name);
        }

        return RuleResult<TError>.Failed(Name, getError(target));
    }
}
