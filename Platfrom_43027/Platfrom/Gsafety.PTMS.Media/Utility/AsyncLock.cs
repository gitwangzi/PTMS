using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Utility
{
    public sealed class AsyncLock : IDisposable
    {
        readonly object _lock = new object();
        bool _isLocked;
        Queue<TaskCompletionSource<IDisposable>> _pending = new Queue<TaskCompletionSource<IDisposable>>();

        #region IDisposable Members

        public void Dispose()
        {
            TaskCompletionSource<IDisposable>[] pending;

            lock (_lock)
            {
                if (null == _pending)
                    return;

                CheckInvariant();

                _isLocked = true;

                if (0 == _pending.Count)
                {
                    _pending = null;
                    return;
                }

                pending = _pending.ToArray();
                _pending.Clear();

                _pending = null;
            }

            foreach (var tcs in pending)
                tcs.TrySetCanceled();
        }

        #endregion

        [Conditional("DEBUG")]
        void CheckInvariant()
        {
            Debug.Assert(_isLocked || (null != _pending && 0 == _pending.Count),
                "Either we are locked or we have an empty queue");
        }

        public IDisposable TryLock()
        {
            ThrowIfDisposed();

            lock (_lock)
            {
                CheckInvariant();

                if (_isLocked)
                    return null;

                _isLocked = true;

                return new Releaser(this);
            }
        }

        public Task<IDisposable> LockAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            cancellationToken.ThrowIfCancellationRequested();

            TaskCompletionSource<IDisposable> tcs;

            lock (_lock)
            {
                CheckInvariant();

                if (!_isLocked)
                {
                    _isLocked = true;

                    return TaskExt.FromResult<IDisposable>(new Releaser(this));
                }

                tcs = new TaskCompletionSource<IDisposable>();

                _pending.Enqueue(tcs);
            }

            if (!cancellationToken.CanBeCanceled)
                return tcs.Task;

            return CancellableTaskAsync(tcs, cancellationToken);
        }

        async Task<IDisposable> CancellableTaskAsync(TaskCompletionSource<IDisposable> tcs, CancellationToken cancellationToken)
        {
            using (cancellationToken.Register(
                () =>
                {
                    bool wasPending;

                    lock (_lock)
                    {
                        CheckInvariant();

                        wasPending = _pending.Remove(tcs);
                    }

                    if (wasPending)
                    {
                        var task = TaskExt.Run(() => tcs.TrySetCanceled());

                        TaskCollector.Default.Add(task, "AsyncLock Propagate cancel");
                    }
                }))
            {
                return await tcs.Task.ConfigureAwait(false);
            }
        }

        void Release()
        {
            for (; ; )
            {
                TaskCompletionSource<IDisposable> tcs;

                lock (_lock)
                {
                    CheckInvariant();

                    Debug.Assert(_isLocked, "AsyncLock.Release() was unlocked");

                    if (null == _pending)
                        return; // We have been disposed (so stay locked)

                    if (0 == _pending.Count)
                    {
                        _isLocked = false;
                        return;
                    }

                    tcs = _pending.Dequeue();
                }

                if (tcs.TrySetResult(new Releaser(this)))
                    return;
            }
        }

        void ThrowIfDisposed()
        {
            if (null != _pending)
                return;

            throw new ObjectDisposedException(GetType().Name);
        }

        #region Nested type: Releaser

        sealed class Releaser : IDisposable
        {
            AsyncLock _asyncLock;

            public Releaser(AsyncLock asynclock)
            {
                _asyncLock = asynclock;
            }

            #region IDisposable Members

            public void Dispose()
            {
                var asyncLock = Interlocked.Exchange(ref _asyncLock, null);

                if (null != asyncLock)
                    asyncLock.Release();
            }

            #endregion
        }

        #endregion
    }
}
