using System.Diagnostics;
using System.Linq.Expressions;

namespace SharedLib.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Type"/> object.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Checks whether or not the source type is interface or abstract base class.
        /// </summary>
        /// <param name="source">The source type to check.</param>
        /// <returns><c>true</c>, if the <c>source</c> is interface or abstract base class. Otherwise; <c>false</c>.</returns>
        [DebuggerStepThrough]
        public static bool IsInterfaceOrAbstractBaseClass(this Type source)
        {
            return source.IsInterface || source.IsAbstract;
        }

        /// <summary>
        /// Confirms whether or not the source type has a constructor without args.
        /// </summary>
        /// <param name="source">The source type to confirm.</param>
        /// <returns>A value indicating whether or not the source type has a constructor without args. If <c>source</c> is <c>null</c>, <c>false</c>.</returns>
        [DebuggerStepThrough]
        public static bool HasEmptyArgsConstructor(this Type source)
        {
            return source.GetConstructor(Type.EmptyTypes) != null;
        }

        /// <summary>
        /// Gets an expression to invokes a constructor without args.
        /// </summary>
        /// <param name="source">The source type.</param>
        /// <returns>A <see cref="NewExpression"/> without args, if the <c>source</c> has a constructor without args. Otherwise; <c>null</c>.</returns>
        [DebuggerStepThrough]
        public static NewExpression GetEmptyArgsConstructorExpression(this Type source)
        {
            return source.HasEmptyArgsConstructor() ? Expression.New(source) : null;
        }
    }
}
