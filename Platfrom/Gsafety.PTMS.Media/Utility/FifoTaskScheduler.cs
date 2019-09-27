using Gsafety.PTMS.Media.Common.Loggers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Utility
{
    public sealed class FifoTaskScheduler : TaskScheduler, IDisposable
    {
        // See LimitedConcurrencyLevelTaskScheduler in ParallelExtensionsExtras.  We don't
        // have ThreadPool in this PCL, so we use SignalTask (which uses the default scheduler).
        readonly LinkedList<Task> _tasks = new LinkedList<Task>();
        readonly SignalTask _workerTask;

        public FifoTaskScheduler(CancellationToken cancellationToken)
        {
            _workerTask = new SignalTask(Worker, cancellationToken);
        }

        public override int MaximumConcurrencyLevel
        {
            get { return 1; }
        }

        Task Worker()
        {
            try
            {
                for (; ; )
                {
                    Task task;

                    lock (_tasks)
                    {
                        if (0 == _tasks.Count)
                            return TplTaskExtensions.CompletedTask;

                        task = _tasks.First.Value;
                        _tasks.RemoveFirst();
                    }

                    TryExecuteTask(task);
                }
            }
            catch (Exception ex)
            {
                LoggerInstance.Exception("FifoTaskScheduler.Worker() failed " + ex.ToString());
            }

            return TplTaskExtensions.CompletedTask;
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            lock (_tasks)
            {
                return _tasks.ToArray();
            }
        }

        protected override void QueueTask(Task task)
        {
            lock (_tasks)
            {
                _tasks.AddLast(task);
            }

            _workerTask.Fire();
        }

        protected override bool TryDequeue(Task task)
        {
            lock (_tasks)
            {
                return _tasks.Remove(task);
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }

        public void Dispose()
        {
            using (_workerTask)
            { }

            Task[] tasks;

            lock (_tasks)
            {
                tasks = _tasks.ToArray();
                _tasks.Clear();
            }

            Debug.Assert(0 == tasks.Length, "FifoTaskScheduler: Pending tasks abandoned");
        }
    }
}
