using Gsafety.PTMS.Media.Common.Loggers;
using System.Collections.Generic;
using System.Diagnostics;

namespace Gsafety.PTMS.Media.Utility
{
    public static class QueueExtensions
    {
        /// <summary>
        ///     Remove an item from a queue.  This is expensive, since it copies the queue, clears it, then re-enqueue
        ///     everything but the requested item.  Think about finding a more suitable data structure if this needs
        ///     to happen often or if the queue is large.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool Remove<T>(this Queue<T> queue, T item)
            where T : class
        {
            if (!queue.Contains(item))
                return false;

            var items = queue.ToArray();

            queue.Clear();

            var foundItem = false;

            foreach (var x in items)
            {
                if (ReferenceEquals(x, item))
                {
                    if (foundItem)
                        LoggerInstance.Debug("QueueExtensions.Remove() multiple matches");

                    foundItem = true;

                    continue;
                }

                queue.Enqueue(x);
            }

            return foundItem;
        }
    }
}
