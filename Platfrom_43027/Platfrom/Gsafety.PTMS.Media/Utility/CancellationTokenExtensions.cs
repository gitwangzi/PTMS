using Gsafety.PTMS.Media.Common.Loggers;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Utility
{
    public static class CancellationTokenExtensions
    {
        static readonly Task CancelledTask;
        static readonly Task PendingTask = new TaskCompletionSource<object>().Task;

        static CancellationTokenExtensions()
        {
            var tcs = new TaskCompletionSource<object>();

            tcs.TrySetCanceled();

            CancelledTask = tcs.Task;
        }

        static async Task WaitAsync(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object>();

            using (cancellationToken.Register(() => TaskExt.Run(() => tcs.TrySetResult(0))))
            {
                try
                {
                    await tcs.Task.ConfigureAwait(false);
                }
                catch (System.Exception ex)
                {
                }                
            }
        }

        public static Task AsTask(this CancellationToken cancellationToken)
        {
            if (!cancellationToken.CanBeCanceled)
                return PendingTask;

            if (cancellationToken.IsCancellationRequested)
                return CancelledTask;

            return WaitAsync(cancellationToken);
        }

        /// <summary>
        ///     Cancel then dispose without throwing any exceptions.
        /// </summary>
        /// <param name="cancellationTokenSource"></param>
        /// <returns></returns>
        public static void CancelDisposeSafe(this CancellationTokenSource cancellationTokenSource)
        {
            if (null == cancellationTokenSource)
                return;

            CancelSafe(cancellationTokenSource);

            cancellationTokenSource.DisposeSafe();
        }

        /// <summary>
        ///     Cancel without throwing any exceptions.
        /// </summary>
        public static void CancelSafe(this CancellationTokenSource cancellationTokenSource)
        {
            if (null == cancellationTokenSource)
                return;

            try
            {
                if (!cancellationTokenSource.IsCancellationRequested)
                    cancellationTokenSource.Cancel();
            }
            catch (Exception ex)
            {
                LoggerInstance.Debug("CancellationTokenExtensions.CancelSafe() failed: " + ex.Message);
            }
        }

        /// <summary>
        ///     Cancel without throwing any exceptions on the default task scheduler.
        /// </summary>
        public static void BackgroundCancelSafe(this CancellationTokenSource cancellationTokenSource)
        {
            if (null == cancellationTokenSource)
                return;

            try
            {
                if (!cancellationTokenSource.IsCancellationRequested)
                {
                    var t = TaskExt.Run(() =>
                    {
                        try
                        {
                            cancellationTokenSource.Cancel();
                        }
                        catch (Exception ex)
                        {
                            LoggerInstance.Debug("CancellationTokenExtensions.BackgroundCancelSafe() cancel failed: " + ex.Message);
                        }
                    });

                    TaskCollector.Default.Add(t, "CancellationTokenExtensions BackgroundCancelSafe");
                }
            }
            catch (Exception ex)
            {
                LoggerInstance.Debug("CancellationTokenExtensions.BackgroundCancelSafe() failed: " + ex.Message);
            }
        }
    }
}
