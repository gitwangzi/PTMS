using Gsafety.PTMS.Media.Common.Loggers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Utility
{
    public class TaskCollector
    {
        public static readonly TaskCollector Default = new TaskCollector();
        readonly object _lock = new object();
        readonly Dictionary<Task, string> _tasks = new Dictionary<Task, string>();

        //[Conditional("DEBUG")]
        public void Add(Task task, string description)
        {
            if (task.IsCompleted)
            {
                ReportException(task, description);

                return;
            }

            lock (_lock)
            {
                Debug.Assert(!_tasks.ContainsKey(task));
                _tasks[task] = description;
            }

            task.ContinueWith(Cleanup);
        }

        [Conditional("DEBUG")]
        public void Wait()
        {
            KeyValuePair<Task, string>[] tasks;

            lock (_lock)
            {
                tasks = _tasks.ToArray();
            }

            if (null == tasks || 0 == tasks.Length)
                return;

            try
            {
                TaskExt.WhenAll(tasks.Select(t => t.Key)).Wait();
            }
            catch (Exception ex)
            {
                LoggerInstance.Debug("TaskCollector.Wait() Task wait failed: " + ex.ExtendedMessage());
            }
        }

        void Cleanup(Task task)
        {
            Debug.Assert(task.IsCompleted);

            var wasRemoved = false;
            string description;

            lock (_lock)
            {
                if (_tasks.TryGetValue(task, out description))
                    wasRemoved = _tasks.Remove(task);
            }

            Debug.Assert(wasRemoved, description ?? "No description");

            ReportException(task, description);
        }

        static void ReportException(Task task, string description)
        {
            try
            {
                var ex = task.Exception;

                if (null != ex)
                {
                    LoggerInstance.Debug("TaskCollector.Cleanup() task {0} failed: {1}{2}{3}", description, ex.ExtendedMessage(), Environment.NewLine, ex.StackTrace);

                    //if (Debugger.IsAttached)
                    //    Debugger.Break();
                }
            }
            catch (Exception ex2)
            {
                LoggerInstance.Debug("TaskCollector.Cleanup() cleanup of task {0} failed: {1}", description, ex2.ExtendedMessage());

                //if (Debugger.IsAttached)
                //    Debugger.Break();
            }
        }
    }
}
