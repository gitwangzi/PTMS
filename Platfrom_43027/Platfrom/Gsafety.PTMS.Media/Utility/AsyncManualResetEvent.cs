using Gsafety.PTMS.Media.Common.Loggers;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Utility
{
    /// <summary>
    ///     http://blogs.msdn.com/b/pfxteam/archive/2012/02/11/10266920.aspx
    /// </summary>
    public sealed class AsyncManualResetEvent
    {
        volatile TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

        public AsyncManualResetEvent(bool initialState = false)
        {
            if (!initialState)
                return;

            Set();
        }

        public Task WaitAsync()
        {
            return _tcs.Task;
        }

        public void Set() { _tcs.TrySetResult(true); }
        //public void Set()
        //{
        //    var tcs = _tcs;

        //    if (tcs.Task.IsCompleted)
        //        return;

        //    var t = Task.Factory.StartNew(s => ((TaskCompletionSource<bool>)s).TrySetResult(true),
        //        tcs, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.Default);

        //    TaskCollector.Default.Add(t, "AsyncManualResetEvent Set");

        //    tcs.Task.Wait();
        //}

        public void Reset()
        {
            var tcs = _tcs;

            if (!tcs.Task.IsCompleted)
                return;

            var newTcs = new TaskCompletionSource<bool>();

            while (true)
            {
#pragma warning disable 0420
                var currentTcs = Interlocked.CompareExchange(ref _tcs, newTcs, tcs);
#pragma warning restore 0420

                if (tcs == currentTcs)
                {
                    if (!tcs.Task.IsCompleted)
                    {
                        LoggerInstance.Debug("*** AsyncManualResetEvent.Reset(): task completion source was not completed");

                        tcs.TrySetResult(true);
                    }
                }

                tcs = currentTcs;

                if (!tcs.Task.IsCompleted)
                    return;
            }
        }
    }
}
