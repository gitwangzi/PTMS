﻿using Gsafety.PTMS.Media.Common.Loggers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Utility
{
    public sealed class AsyncFifoWorker : IDisposable
    {
        readonly object _lock = new object();
        readonly SignalTask _signalTask;
        readonly Queue<WorkHandle> _workQueue = new Queue<WorkHandle>();
        bool _isClosed;
#if DEBUG
        WorkHandle _work;
#endif

        public AsyncFifoWorker(CancellationToken cancellationToken)
        {
            _signalTask = new SignalTask(Worker, cancellationToken);
        }

        public AsyncFifoWorker()
        {
            _signalTask = new SignalTask(Worker);
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (!Close())
                return;

            using (_signalTask)
            { }

            WorkHandle[] workQueue;

            lock (_lock)
            {
                workQueue = _workQueue.ToArray();
                _workQueue.Clear();
            }

            foreach (var work in workQueue)
                work.Dispose();
        }

        #endregion

        async Task Worker()
        {
            for (; ; )
            {
                WorkHandle work;

                lock (_lock)
                {
                    if (_workQueue.Count < 1)
                        return;

                    work = _workQueue.Dequeue();
                }

#if DEBUG
                _work = work;
#endif
                try
                {
                    work.TryDeregister();

                    await work.RunAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    LoggerInstance.Debug("AsyncFifoWorker.Worker() failed: " + ex.ExtendedMessage());
                }

#if DEBUG
                _work = null;
#endif
                work.Dispose();
            }
        }

        void RemoveWork(object workObject)
        {
            var work = (WorkHandle)workObject;

            lock (_lock)
            {
                if (!_workQueue.Remove(work))
                    return;
            }

            work.Dispose();
        }

        public void Post(Func<Task> workFunc, string description, CancellationToken cancellationToken)
        {
            PostWork(workFunc, false, description, cancellationToken);
        }

        public Task PostAsync(Func<Task> workFunc, string description, CancellationToken cancellationToken)
        {
            var work = PostWork(workFunc, true, description, cancellationToken);

            return work.Task;
        }

        WorkHandle PostWork(Func<Task> workFunc, bool createTcs, string description, CancellationToken cancellationToken)
        {
            if (workFunc == null)
                throw new ArgumentNullException("workFunc");

            cancellationToken.ThrowIfCancellationRequested();

            WorkHandle work;

            lock (_lock)
            {
                if (_isClosed)
                    throw new InvalidOperationException("AsyncFifoWorker is closed");

                work = new WorkHandle(workFunc, createTcs ? new TaskCompletionSource<object>() : null);

#if DEBUG
                work.Description = description;
#endif
                _workQueue.Enqueue(work);
            }

            if (!work.Register(RemoveWork, cancellationToken))
            {
                LoggerInstance.Debug("AsyncFifoWorker.PostWork() work already done");

                return work;
            }

            try
            {
                _signalTask.Fire();
            }
            catch (ObjectDisposedException)
            {
                RemoveWork(work);

                if (_workQueue.Count > 0)
                    LoggerInstance.Debug("AsyncFifoWorker.Post() object disposed but there are still {0} pending", _workQueue.Count);

                throw;
            }

            return work;
        }

        public Task CloseAsync()
        {
            Close();

            return _signalTask.WaitAsync();
        }

        bool Close()
        {
            lock (_lock)
            {
                if (_isClosed)
                    return false;

                _isClosed = true;
            }

            return true;
        }

        sealed class WorkHandle : IDisposable
        {
            readonly TaskCompletionSource<object> _taskCompletionSource;
            readonly Func<Task> _work;
            CancellationTokenRegistration _cancellationTokenRegistration;
            int _state;
#if DEBUG
            public string Description { get; set; }
#endif

            public WorkHandle(Func<Task> work, TaskCompletionSource<object> taskCompletionSource)
            {
                if (null == work)
                    throw new ArgumentNullException("work");

                _work = work;
                _taskCompletionSource = taskCompletionSource;
            }

            public Task Task
            {
                get { return null == _taskCompletionSource ? null : _taskCompletionSource.Task; }
            }

            public void Dispose()
            {
                if (State.Disposed == SetState(State.Disposed))
                    return;

                TryDeregister();

                if (null != _taskCompletionSource)
                    _taskCompletionSource.TrySetCanceled();
            }

            public bool Register(Action<WorkHandle> removeWork, CancellationToken cancellationToken)
            {
                if (State.Free != SetState(State.Registered))
                    return false;

                _cancellationTokenRegistration = cancellationToken.Register(w => removeWork((WorkHandle)w), this);

                return true;
            }

            public bool TryDeregister()
            {
                if (State.Registered != SetState(State.Deregistered))
                    return false;

                _cancellationTokenRegistration.DisposeSafe();

                return true;
            }

            State SetState(State state)
            {
                return (State)Interlocked.Exchange(ref _state, (int)state);
            }

            public async Task RunAsync()
            {
                var tcs = _taskCompletionSource;

                try
                {
                    await _work().ConfigureAwait(false);

                    if (null != tcs)
                        tcs.TrySetResult(string.Empty);
                }
                catch (OperationCanceledException)
                {
                    if (null != tcs)
                        tcs.TrySetCanceled();
                }
                catch (Exception ex)
                {
                    if (null != tcs && tcs.TrySetException(ex))
                        return;

                    LoggerInstance.Debug("AsyncFifoWorker.WorkHandle.RunAsync() work should not throw exceptions: " + ex.ExtendedMessage());
                }
            }

            enum State
            {
                Free = 0,
                Registered = 1,
                Deregistered = 2,
                Disposed = 3
            }
        }
    }

    public static class AsyncFifoWorkerExtensions
    {
        public static void Post(this AsyncFifoWorker worker, Action action, string description, CancellationToken cancellationToken)
        {
            worker.Post(() =>
            {
                action();
                return TplTaskExtensions.CompletedTask;
            }, description, cancellationToken);
        }

        public static void Post(this AsyncFifoWorker worker, Task work, string description, CancellationToken cancellationToken)
        {
            worker.Post(() => work, description, cancellationToken);
        }
    }
}
