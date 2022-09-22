using System;
using System.Collections.Generic;
using System.Linq;

namespace NSW.Extensions.Internal
{
    /// <summary> Collection sorter </summary>
    /// <typeparam name="T">Collection object type</typeparam>
    internal class Sorter<T>
    {
        private readonly List<Tuple<Func<T, object>, bool>> _sortingExpressions = new();
        /// <summary>
        /// Add ASC sorting rule
        /// </summary>
        /// <param name="keySelector">Sorting key selector</param>
        public void AddAsc(Func<T, object> keySelector) => AddSorting(keySelector, true);
        /// <summary>
        /// Add DESC sorting rule
        /// </summary>
        /// <param name="keySelector">Sorting key selector</param>

        public void AddDesc(Func<T, object> keySelector) => AddSorting(keySelector, false);
        private void AddSorting(Func<T, object> keySelector, bool asc)
        {
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            _sortingExpressions.Add(new Tuple<Func<T, object>, bool>(keySelector, asc));
        }
        /// <summary>
        /// Sort collection
        /// </summary>
        /// <param name="collection">Input collection</param>
        /// <returns>Return collection sorted by added sorting rules by <see cref="AddSorting"/></returns>
        public IEnumerable<T> Sort(IEnumerable<T> collection)
        {
            IOrderedEnumerable<T> sorted = null!;

            for (var i = 0; i < _sortingExpressions.Count; i++)
            {
                if (i == 0)
                {
                    sorted = _sortingExpressions[i].Item2 
                        ? collection.OrderBy(_sortingExpressions[i].Item1) 
                        : collection.OrderByDescending(_sortingExpressions[i].Item1);
                }
                else
                {
                    sorted = _sortingExpressions[i].Item2 
                        ? sorted.ThenBy(_sortingExpressions[i].Item1) 
                        : sorted.ThenByDescending(_sortingExpressions[i].Item1);
                }
            }

            return sorted ?? collection;
        }
    }
}
