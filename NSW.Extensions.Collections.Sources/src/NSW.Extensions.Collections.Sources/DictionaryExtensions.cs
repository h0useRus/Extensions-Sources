using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NSW.Extensions.Internal
{
    /// <summary>
    /// <see cref="IDictionary{TKey,TValue}"/> extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static partial class DictionaryExtensions
    {
        /// <summary>
        /// Return array of dictionary values
        /// </summary>
        /// <typeparam name="TKey">Dictionary key type</typeparam>
        /// <typeparam name="TValue">Dictionary value type</typeparam>
        /// <param name="dictionary">Source dictionary</param>
        /// <returns>Array of dictionary values</returns>
        public static TValue[]? ValuesToArray<TKey, TValue>(this IDictionary<TKey, TValue>? dictionary) => dictionary?.Values.ToArray();

        /// <summary>
        /// Return list of dictionary values
        /// </summary>
        /// <typeparam name="TKey">Dictionary key type</typeparam>
        /// <typeparam name="TValue">Dictionary value type</typeparam>
        /// <param name="dictionary">Source dictionary</param>
        /// <returns>List of dictionary values</returns>
        public static List<TValue>? ValuesToList<TKey, TValue>(this IDictionary<TKey, TValue>? dictionary) => dictionary?.Values.ToList();

        /// <summary>
        /// Return array of dictionary keys
        /// </summary>
        /// <typeparam name="TKey">Dictionary key type</typeparam>
        /// <typeparam name="TValue">Dictionary value type</typeparam>
        /// <param name="dictionary">Source dictionary</param>
        /// <returns>Array of dictionary keys</returns>
        public static TKey[]? KeysToArray<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) => dictionary?.Keys.ToArray();
        /// <summary>
        /// Return list of dictionary keys
        /// </summary>
        /// <typeparam name="TKey">Dictionary key type</typeparam>
        /// <typeparam name="TValue">Dictionary value type</typeparam>
        /// <param name="dictionary">Source dictionary</param>
        /// <returns>List of dictionary keys</returns>
        public static List<TKey>? KeysToList<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) => dictionary?.Keys.ToList();
        /// <summary>
        /// Appends <paramref name="other"/> key/value pairs to <paramref name="dictionary"/>. Conflicting keys are not appended (e.g. 'src' keys has priority).
        /// Operation in mutable, <paramref name="dictionary"/> modified as result. 
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="dictionary">Source dictionary</param>
        /// <param name="other">Appendable dictionary</param>
        /// <remarks>Null safe operation.</remarks>
        public static IDictionary<TKey, TValue> Append<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> other)
        {
            foreach (var pair in other.Where(pair => !dictionary.ContainsKey(pair.Key)))
            {
                dictionary.Add(pair);
            }
            return dictionary;
        }
        /// <summary>
        /// Converts an enumeration of groupings into a Dictionary of those groupings.
        /// </summary>
        /// <typeparam name="TKey">Key type of the grouping and dictionary.</typeparam>
        /// <typeparam name="TValue">Element type of the grouping and dictionary list.</typeparam>
        /// <param name="groupings">The enumeration of groupings from a GroupBy() clause.</param>
        /// <returns>A dictionary of groupings such that the key of the dictionary is TKey type and the value is List of TValue type.</returns>
#pragma warning disable 8714
        public static Dictionary<TKey, List<TValue>> ToDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> groupings)
            => groupings.ToDictionary(group => group.Key, group => group.ToList());
#pragma warning restore 8714
    }
}
