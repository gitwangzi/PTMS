using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Utility
{
    sealed class SingleUseTaskTimer : IDisposable
    {
        private CancellationTokenSource _cancellationTokenSource;

        // From: http://stackoverflow.com/a/12790048
        public SingleUseTaskTimer(Action callback, TimeSpan expiration)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            TaskExt.Delay((int)expiration.TotalMilliseconds, _cancellationTokenSource.Token)
                .ContinueWith(
                    t => { callback(); },
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.Default);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }

        public void Cancel()
        {
            if (_cancellationTokenSource == null)
            {
                return;
            }

            _cancellationTokenSource.Cancel();
        }
    }
}
