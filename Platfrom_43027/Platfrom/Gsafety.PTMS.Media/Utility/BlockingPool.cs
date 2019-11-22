using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Utility
{
    public sealed class BlockingPool<TItem> : IBlockingPool<TItem>
        where TItem : class, new()
    {
        readonly Queue<TItem> _pool = new Queue<TItem>();
        readonly int _poolSize;
        readonly Queue<CancellationTaskCompletionSource<TItem>> _waiters = new Queue<CancellationTaskCompletionSource<TItem>>();
        int _allocationCount;

#if DEBUG
        readonly List<TItem> _allocationTracker = new List<TItem>();
#endif // DEBUG

        public BlockingPool(int poolSize)
        {
            _poolSize = poolSize;
        }

   
        public Task<TItem> AllocateAsync(CancellationToken cancellationToken)
        {
            lock (_pool)
            {
                if (_pool.Count > 0)
                {
                    var item = _pool.Dequeue();

                    Debug.Assert(!EqualityComparer<TItem>.Default.Equals(default(TItem), item));

                    //LoggerInstance.Debug("BlockingPool.AllocateAsync() Returning pool item: " + item);

                    return TaskExt.FromResult(item);
                }

                if (_allocationCount >= _poolSize)
                {
                    var workHandle = new CancellationTaskCompletionSource<TItem>(wh => _waiters.Remove(wh), cancellationToken);

                    _waiters.Enqueue(workHandle);

#if DEBUG
                    //var sw = Stopwatch.StartNew();

                    //LoggerInstance.Debug("BlockingPool.AllocateAsync() Creating waiter");
                    //workHandle.Task.ContinueWith(t =>
                    //                             {
                    //                                 sw.Stop();

                    //                                 LoggerInstance.Debug("BlockingPool.AllocateAsync() Waiter completed: status {0} elapsed {1}",
                    //                                     t.Status, sw.Elapsed);
                    //                             });
#endif

                    return workHandle.Task;
                }

                ++_allocationCount;
            }

            var newItem = new TItem();

            //LoggerInstance.Debug("BlockingPool.AllocateAsync() Returning new item " + newItem);

#if DEBUG
            _allocationTracker.Add(newItem);
#endif

            return TaskExt.FromResult(newItem);
        }

        public void Free(TItem item)
        {
            //LoggerInstance.Debug("BlockingPool.Free() item: " + item);

            if (EqualityComparer<TItem>.Default.Equals(default(TItem), item))
                throw new ArgumentNullException("item");

            lock (_pool)
            {
#if DEBUG
                Debug.Assert(_allocationTracker.Contains(item), "Unknown item has been freed");
                Debug.Assert(!_pool.Contains(item), "Item is already in pool");
#endif

                while (_waiters.Count > 0)
                {
                    Debug.Assert(0 == _pool.Count, "The pool should be empty when there are waiters");

                    var waiter = _waiters.Dequeue();

                    //LoggerInstance.Debug("BlockingPool.Free() giving to waiter: " + item);

                    if (waiter.TrySetResult(item))
                        return;

                    //LoggerInstance.Debug("BlockingPool.Free() giving to waiter failed for: " + item);
                }

                _pool.Enqueue(item);
            }
        }

        public void Dispose()
        {
            Clear();
        }

        void Clear()
        {
            CancellationTaskCompletionSource<TItem>[] waiters;

            lock (_pool)
            {
                waiters = _waiters.ToArray();
                _waiters.Clear();

                _pool.Clear();
                _allocationCount = 0;

#if DEBUG
                _allocationTracker.Clear();
#endif
            }

            foreach (var waiter in waiters)
                waiter.Dispose();
        }
    }
}
