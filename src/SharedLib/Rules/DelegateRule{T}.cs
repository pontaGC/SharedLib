using System.Diagnostics.CodeAnalysis;

namespace SharedLib.Rules
{
    /// <summary>
    /// The delegate rule.
    /// </summary>
    /// <typeparam name="T">The type of the target object to which the rule applies.</typeparam>
    public class DelegateRule<T> : IRule<T>
    {
        private readonly DelegateRule<T, string> rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateRule{T}"/> class.
        /// </summary>
        /// <param name="ruleName">The rule name.</param>
        /// <param name="applyRule">The check function that the target object satisfy a rule.</param>
        /// <param name="getError">The function to get error if rule fails.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ruleName"/> or <paramref name="applyRule"/> or <paramref name="getError"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="ruleName"/> is an empty string.</exception>
        public DelegateRule(string ruleName, Predicate<T> applyRule, Func<T, string> getError)
        {
            this.rule = new DelegateRule<T, string>(ruleName, applyRule, getError);
        }

        /// <inheritdoc />
        [NotNull]
        public string Name => this.rule.Name;

        /// <inheritdoc />
        public RuleResult<string> Apply(T? target)
        {
            return this.rule.Apply(target);
        }
    }
}