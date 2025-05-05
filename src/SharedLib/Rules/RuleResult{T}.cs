using System.Diagnostics.CodeAnalysis;

namespace SharedLib.Rules
{
    /// <summary>
    /// The result to apply one rule to the target object.
    /// </summary>
    /// <typeparam name="TError">The type of the error object if rule fails.</typeparam>
    public class RuleResult<TError>
    {
        private RuleResult(string ruleName, bool isPassed, TError error)
        {
            this.RuleName = ruleName ?? string.Empty;
            this.IsPassed = isPassed;
            this.Error = error;
        }

        /// <summary>
        /// Get a name of the rule to apply.
        /// </summary>
        [NotNull]
        public string RuleName { get; }

        /// <summary>
        /// Gets a value indicating whether the rule is complied or not.
        /// </summary>
        public bool IsPassed { get; }

        /// <summary>
        /// Gets an error indicatihg a violation of the rule.
        /// </summary>
        public TError Error { get; }

        /// <summary>
        /// Creates a passed result.
        /// </summary>
        /// <param name="ruleName">The name of rule.</param>
        /// <returns>An instance of passed result.</returns>
        public static RuleResult<TError> Passed(string ruleName)
        {
            return new RuleResult<TError>(ruleName, true, default);
        }

        /// <summary>
        /// Creates a failed result.
        /// </summary>
        /// <param name="ruleName">The name of rule.</param>
        /// <param name="error">The error indicatihg a violation of the rule.</param>
        /// <returns>An instance of failed result.</returns>
        public static RuleResult<TError> Failed(string ruleName, TError error)
        {
            return new RuleResult<TError>(ruleName, false, error);
        }
    }
}
