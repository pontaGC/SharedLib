using System.Diagnostics;

namespace SharedLib.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IList{T}"/>.
    /// </summary>
    public static class ListExtension
    {
        /// <summary>
        /// Adds an object to the head of the list.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the source list.</typeparam>
        /// <param name="source">The list to add.</param>
        /// <param name="item">
        /// The object to be added to the end of the <see cref="IList{T}"/>.
        /// The value can be <c>null</c> for reference types.
        /// </param>
        [DebuggerStepThrough]
        public static void AddToHead<TSource>(this IList<TSource> source, TSource item)
        {
            source?.Insert(0, item);
        }

        /// <summary>
        /// Removes the last element of the list.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the source list.</typeparam>
        /// <param name="source">The list to remove.</param>
        /// <returns><c>true</c> if the last element has been removed, otherwise, <c>false</c>.</returns>
        [DebuggerStepThrough]
        public static bool RemoveLast<T>(this IList<T> source)
        {
            if (source is null || !source.Any())
            {
                return false;
            }

            if (source.IsReadOnly)
            {
                // The read-only list cannot be modified
                return false;
            }

            var lastIndex = source.Count - 1;
            if (lastIndex < 0)
            {
                return false;
            }

            source.RemoveAt(lastIndex);
            return true;
        }
    }
}
