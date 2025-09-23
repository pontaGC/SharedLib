using System.Diagnostics;

namespace SharedLib.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/> object.
    /// </summary>
    public static class EnumerableExtensions
    {
        #region Public Methods

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if the sequence is NULL.
        /// Throws <see cref="ArgumentException"/> is the sequence is an empty sequence.
        /// </summary>
        /// <typeparam name="T">The type of the source enumerable.</typeparam>
        /// <param name="target">A sequence of values to check.</param>
        /// <param name="parameterName">The name of the target sequence.</param>
        [DebuggerStepThrough]
        public static void ThrowArgumentNullOrEmptyException<T>(this IEnumerable<T> target, string parameterName)
        {
            if (target is null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (!target.Any())
            {
                throw new ArgumentException(parameterName);
            }
        }

        /// <summary>
        /// Executes an action for each element.
        /// </summary>
        /// <typeparam name="T">The type of the source enumerable.</typeparam>
        /// <param name="source">A sequence of values to invoke an action.</param>
        /// <param name="iterativeAction">An action for each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="iterativeAction"/> is <c>null</c>.</exception>
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> iterativeAction)
        {
            if (iterativeAction is null)
                throw new ArgumentNullException(nameof(iterativeAction));

            if (source is null)
            {
                return;
            }

            foreach (var element in source)
            {
                iterativeAction.Invoke(element);
            }
        }

        /// <summary>
        /// Checks whether or not there is the duplicated key of the source enumerable.
        /// </summary>
        /// <typeparam name="TSource">The type of the source enumerable.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <c>keySelector</c>.</typeparam>
        /// <param name="source">A sequence of values to check.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <returns>
        /// <c>false</c>, if <c>source</c> or <c>keySelector</c> is <c>null</c>.
        /// <c>true</c>, if the duplicated key in the keys selected by <c>keySelector</c> exists. Otherwise; <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsKeyDuplicated<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source is null || keySelector is null)
            {
                return false;
            }

            return source.GroupBy(keySelector).Any(g => g.Count() > 1);
        }

        /// <summary>
        /// Indicates whether a sequence is <c>null</c> or does not contain any element.
        /// </summary>
        /// <typeparam name="T">The type of the source enumerable.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <returns><c>true</c> if a sequence is <c>null</c> or does not contain any element; otherwise, <c>false</c>.</returns>
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source is null || !source.Any();
        }

        /// <summary>
        /// Determines whether there is the same element both the two sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="other">The other sequence.</param>
        /// <returns><c>true</c>, if the same element in the two sequence is found. Otherwise; <c>false</c>.</returns>
        [DebuggerStepThrough]
        public static bool ContainsSameElement<T>(this IEnumerable<T> source, IEnumerable<T> other)
        {

            if (source.IsEmpty() || other.IsEmpty())
            {
                return false;
            }

            var otherElements = other.ToArraySafe();
            return source.Any(sourceElement => otherElements.Contains(sourceElement));
        }

        /// <summary>
        /// Indicates whether a sequence does not contain any element.
        /// </summary>
        /// <typeparam name="TSource">The type of the source enumerable.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <returns><c>true</c> if a sequence does not contain any element; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><c>source</c> is <c>null</c>.</exception>
        [DebuggerStepThrough]
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return !source.Any();
        }

        /// <summary>
        /// Creates an empty <see cref="IEnumerable{T}"/> that has the specified type argument if the source sequence is <c>null</c>.
        /// </summary>
        /// <typeparam name="TResult">The type to assign to the type parameter of the returned generic <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to create an Enumerable empty.</param>
        /// <returns>An empty <see cref="IEnumerable{TResult}"/> whose type argument is <c>TResult</c> if <c>source</c> is <c>null</c>. Otherwise; <c>source</c>.</returns>
        [DebuggerStepThrough]
        public static IEnumerable<TResult> ToEmptyIfNull<TResult>(this IEnumerable<TResult> source)
        {
            return source ?? Enumerable.Empty<TResult>();
        }

        /// <summary>
        /// Creates an array from a <see cref="IEnumerable{T}"/>. If the sequence is <c>null</c>, creates an empty array.
        /// </summary>
        /// <typeparam name="TSource">The type of the source enumerable.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to create an array from.</param>
        /// <returns>An array that contains the elements from the input sequence, if the <c>source</c> is not <c>null</c>. Otherwise; An empty array, <c>Array.Empty()</c>.</returns>
        [DebuggerStepThrough]
        public static T[] ToArraySafe<T>(this IEnumerable<T> source)
        {
            return source is null ? Array.Empty<T>() : source.ToArray();
        }

        /// <summary>
        /// Creates a list from a <see cref="IEnumerable{T}"/>. If the sequence is <c>null</c>, creates an empty list.
        /// </summary>
        /// <typeparam name="TSource">The type of the source enumerable.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to create a list from.</param>
        /// <returns>A list that contains the elements from the input sequence, if the <c>source</c> is not <c>null</c>. Otherwise; A empty list.</returns>
        [DebuggerStepThrough]
        public static List<T> ToSafeList<T>(this IEnumerable<T> source)
        {
            return (source ?? Enumerable.Empty<T>()).ToList();
        }

        /// <summary>
        /// Returns the only element of a sequence that satisfies a specified condition, or a specified default value if no such element exists or more than one element satisfies the condition.
        /// This method throws no exception.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <c>source</c>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to return the single element of.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <param name="defaultValue">The default value to return if the sequence has multiple elements.</param>
        /// <returns>
        /// The single element of the input sequence that satisfies the condition
        /// , or defaultValue if no such element is found or more than one element satisfies the condition.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <c>null</c>.</exception>
        [DebuggerStepThrough]
        public static T SingleOrDefaultSafe<T>(this IEnumerable<T> source, Func<T, bool> predicate, T defaultValue = default)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            if (source is null)
            {
                return defaultValue;
            }

            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var result = enumerator.Current;
                    if (predicate.Invoke(result))
                    {
                        while (enumerator.MoveNext())
                        {
                            if (predicate.Invoke(enumerator.Current))
                            {
                                // Found two elements that satisfies the condition
                                return defaultValue;
                            }
                        }

                        return result;
                    }
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Returns the only element of a sequence, or a default value if the sequence is empty or there is more than one element in the sequence.
        /// This method throws no exception.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <c>source</c>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to return the single element of.</param>
        /// <param name="defaultValue">The default value to return if the sequence has multiple elements.</param>
        /// <returns>The single element of the input sequence, or <c>defaultValue</c> if the sequence contains no elements or more than one element.</returns>
        [DebuggerStepThrough]
        public static T SingleOrDefaultSafe<T>(this IEnumerable<T> source, T defaultValue = default)
        {
            if (source is null)
            {
                return defaultValue;
            }

            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    // Found no element
                    return defaultValue;
                }

                var result = enumerator.Current;
                if (!enumerator.MoveNext())
                {
                    // Found single element
                    return result;
                }
            }

            return defaultValue;
        }

        #endregion
    }
}
