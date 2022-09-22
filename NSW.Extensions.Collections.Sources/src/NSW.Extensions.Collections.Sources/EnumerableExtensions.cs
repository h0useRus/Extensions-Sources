using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace NSW.Extensions.Internal
{
    /// <summary>
    /// <see cref="IEnumerable{T}"/> extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static partial class EnumerableExtensions
    {
        /// <summary>
        /// Check source <see cref="IEnumerable{T}"/> is not null or empty.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="source">Source enumerable</param>
        /// <returns></returns>
        public static bool NotNullOrEmpty<T>(this IEnumerable<T>? source)
        {
            return source != null && source.Any();
        }
        /// <summary>
        /// Check source <see cref="IEnumerable{T}"/> is not null or empty.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="source">Source enumerable</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns></returns>
        public static bool NotNullOrEmpty<T>(this IEnumerable<T>? source, Func<T, bool> predicate)
        {
            return source != null && source.Any(predicate);
        }
        /// <summary>
        /// Split <see cref="IEnumerable{T}"/> to sections
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="source">Source enumerable</param>
        /// <param name="length">Section size</param>
        /// <returns>Enumerable of sections</returns>
        public static IEnumerable<IEnumerable<T>> Section<T>(this IEnumerable<T>? source, int length)
        {
            if(source==null || length <= 0)
                yield break; 

            var section = new List<T>(length);

            foreach (var item in source)
            {
                section.Add(item);

                if (section.Count == length)
                {
                    yield return section.AsReadOnly();
                    section = new List<T>(length);
                }
            }

            if (section.Count > 0)
                yield return section.AsReadOnly();
        }
        
        /// <summary>
        /// Convenience method for retrieving a specific page of items within a collection.
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="source">Enumerable to page</param>
        /// <param name="pageIndex">The index of the page to get.</param>
        /// <param name="pageSize">The size of the pages.</param>
        /// <returns>Specific page of items</returns>
        public static IEnumerable<T> GetPage<T>(this IEnumerable<T>? source, int pageIndex, int pageSize)
        {
            if(source is null || pageIndex < 0 || pageSize <= 0) return Enumerable.Empty<T>();
            return source.Skip(pageIndex * pageSize).Take(pageSize);
        }
        ///<summary>
        /// Returns enumerable object based on target, which does not contains null references.
        /// If target is null reference, returns empty enumerable object.
        ///</summary>
        ///<typeparam name = "T">Type of items in target.</typeparam>
        ///<param name = "target">Target enumerable object. Can be null.</param>
        ///<example>
        /// <code>
        /// object[] items = null;
        /// foreach(var item in items.NotNull()){
        /// // result of items.NotNull() is empty but not null enumerable
        /// }
        /// 
        /// object[] items = new object[]{ null, "Hello World!", null, "Good bye!" };
        /// foreach(var item in items.NotNull()){
        /// // result of items.NotNull() is enumerable with two strings
        /// }
        /// </code>
        ///</example>
        public static IEnumerable<T> IgnoreNulls<T>(this IEnumerable<T>? target)
        {
            if (target is null)
                yield break;

            foreach (var item in target.Where(item => item is not null))
                yield return item;
        }
        /// <summary>
        /// Removes matching items from a sequence
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static IEnumerable<T> RemoveWhere<T>(this IEnumerable<T>? source, Predicate<T>? predicate)
        {
            if (source is null || predicate is null)
                yield break;

            foreach (var t in source)
                if (!predicate(t))
                    yield return t;
        }
        /// <summary>
        /// Return <see cref="Enumerable.Empty{TResult}"/> is <paramref name="source"/> is <c>null</c>.
        /// </summary>
        /// <param name="source">The source.</param>
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source) => source ?? Enumerable.Empty<T>();
    }
}
