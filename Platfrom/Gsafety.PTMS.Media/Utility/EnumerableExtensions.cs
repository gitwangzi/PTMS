using System.Collections.Generic;
using System.Linq;

namespace Gsafety.PTMS.Media.Utility
{
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Return the first item if there is exactly one item in the enumerable.  Unlike .SingleOrDefault(), exceptions will
        ///     not be thrown if there is more than one item or if the enumerable itself is null.  Exceptions may still result
        ///     from enumerating the enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T SingleOrDefaultSafe<T>(this IEnumerable<T> items)
        {
            if (null == items)
                return default(T);

            var list = items as IList<T>;

            if (null != list)
                return 1 == list.Count ? list[0] : default(T);

            using (var itemEnum = items.GetEnumerator())
            {
                if (!itemEnum.MoveNext())
                    return default(T);

                var item = itemEnum.Current;

                return itemEnum.MoveNext() ? default(T) : item;
            }
        }

        /// <summary>
        /// Check if two sequences are the same.  Unlike SequenceEqual(), two nulls
        /// are considered equivalent.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool SequencesAreEquivalent<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (null == a || null == b)
                return false;

            return a.SequenceEqual(b);
        }
    }
}
