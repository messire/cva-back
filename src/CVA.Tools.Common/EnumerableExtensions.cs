namespace CVA.Tools.Common;

/// <summary>
/// Provides extension methods for working with enumerable collections.
/// </summary>
public static class EnumerableExtensions
{
    /// <param name="first">The first collection to compare.</param>
    /// <typeparam name="T">The type of the elements in the collections.</typeparam>
    extension<T>(IEnumerable<T>? first) where T : notnull
    {
        /// <summary>
        /// Determines whether two enumerable collections are scrambled equivalents, meaning they contain the same elements
        /// in any order and with the same frequency.
        /// </summary>
        /// <param name="second">The second collection to compare.</param>
        /// <param name="comparer">An optional equality comparer for comparing elements.</param>
        /// <returns>
        /// Returns <c>true</c> if the two collections contain the same elements in any order with the same frequency;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool ScrambledEquals(IEnumerable<T>? second, IEqualityComparer<T>? comparer = null)
        {
            var list1 = (first ?? []).ToList();
            var list2 = (second ?? []).ToList();

            if (list1.Count != list2.Count) return false;

            var cnt = new Dictionary<T, int>(comparer);
            foreach (var key in list1.Where(key => !cnt.TryAdd(key, 1)))
            {
                cnt[key]++;
            }

            foreach (var key in list2)
            {
                if (!cnt.TryGetValue(key, out var value)) return false;
                cnt[key] = --value;
            }

            return cnt.Values.All(c => c == 0);
        }

        /// <summary>
        /// Executes the specified action on each element of the enumerable collection.
        /// </summary>
        /// <param name="action">The action to perform on each element in the collection.</param>
        public void ForEach(Action<T> action)
        {
            if (first == null) return;

            foreach (var item in first)
            {
                action(item);
            }
        }
    }
}