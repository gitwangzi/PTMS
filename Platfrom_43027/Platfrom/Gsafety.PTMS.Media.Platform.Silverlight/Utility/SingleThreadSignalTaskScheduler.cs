using Gsafety.PTMS.Media.Common.Loggers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Utility
{
    public sealed class SingleThreadSignalTaskScheduler : TaskScheduler, IDisposable
    {
        readonly object _lock = new object();
        readonly Queue<Task> _tasks = new Queue<Task>();
        readonly Thread _thread;
        bool _isDone;
        bool _isSignaled;
        Action _signalHandler;
        CancellationTokenSource _tokenSource;

        public SingleThreadSignalTaskScheduler(string name, Action signalHandler)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (signalHandler == null)
                throw new ArgumentNullException("signalHandler");

            _signalHandler = signalHandler;

            _tokenSource = new CancellationTokenSource();

            _thread = new Thread(Run)
                      {
                          Name = name
                      };

            _thread.Start(_tokenSource.Token);
        }

        public override int MaximumConcurrencyLevel
        {
            get { return 1; }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _isDone = true;
                Monitor.PulseAll(_lock);
                _signalHandler = null;
            }

            if (null != _thread)
                _tokenSource.Cancel();

            if (_tokenSource != null)
            {
                _tokenSource.Dispose();
            }

            if (null != _tasks)
            {
                // Could we cancel of fail them somehow?
                _tasks.Clear();
            }
        }

        public void Signal()
        {
            lock (_lock)
            {
                if (_isSignaled)
                    return;

                _isSignaled = true;

                Monitor.Pulse(_lock);
            }
        }

        void Run(object token)
        {
            try
            {
                var cancelToken = (CancellationToken)token;

                for (; ; )
                {
                    if (cancelToken != null && cancelToken.IsCancellationRequested)
                    {
                        return;
                    }

                    Action signalHandler;
                    Task task;
                    var wasSignaled = false;

                    lock (_lock)
                    {
                        for (; ; )
                        {
                            if (_isDone || null == _signalHandler)
                                return;

                            signalHandler = _signalHandler;

                            var haveWork = false;
                            task = null;

                            if (_tasks.Count > 0)
                            {
                                task = _tasks.Dequeue();
                                haveWork = true;
                            }

                            if (_isSignaled)
                            {
                                _isSignaled = false;
                                wasSignaled = true;
                                haveWork = true;
                            }

                            if (haveWork)
                                break;

                            Monitor.Wait(_lock);
                        }
                    }

                    if (wasSignaled)
                    {
                        var signalTask = new Task(signalHandler);

                        signalTask.RunSynchronously(this);

                        var taskException = signalTask.Exception;

                        if (null != taskException)
                            LoggerInstance.Debug("SingleThreadSignalTaskScheduler.Run() signal handler failed: " + taskException.ExtendedMessage());
                    }

                    if (null != task)
                        TryExecuteTask(task);
                }
            }
            catch (Exception ex)
            {
                LoggerInstance.Exception("SingleThreadSignalTaskScheduler.Run() failed: " + ex.ToString());

                // Kill the app...?
            }
        }

        protected override void QueueTask(Task task)
        {
            lock (_lock)
            {
                _tasks.Enqueue(task);

                Monitor.Pulse(_lock);
            }
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            lock (_lock)
            {
                return _tasks.ToArray();
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (Thread.CurrentThread != _thread)
                return false;

            if (taskWasPreviouslyQueued)
                return false;

            return TryExecuteTask(task);
        }

        [Conditional("DEBUG")]
        public void ThrowIfNotOnThread()
        {
            if (Thread.CurrentThread != _thread)
                throw new InvalidOperationException("Not running on worker thread");
        }
    }
}
