using Gsafety.PTMS.Media.Common.Loggers;
using System;
using System.Diagnostics;
using System.Threading;

namespace Gsafety.PTMS.Media.Utility
{
    public sealed class TaskTimer : IDisposable
    {
        SingleUseTaskTimer _timer;

        public void Dispose()
        {
            Cancel();
        }

        public void SetTimer(Action callback, TimeSpan expiration)
        {
            var timer = new SingleUseTaskTimer(callback, expiration);

            SetTimer(timer);
        }

        public void Cancel()
        {
            SetTimer(null);
        }

        void SetTimer(SingleUseTaskTimer timer)
        {
            timer = Interlocked.Exchange(ref _timer, timer);

            if (null != timer)
                CleanupTimer(timer);
        }

        static void CleanupTimer(SingleUseTaskTimer timer)
        {
            try
            {
                timer.Cancel();

                timer.Dispose();
            }
            catch (Exception ex)
            {
                LoggerInstance.Exception("Timer.Dispose(): " + ex.ToString());
            }
        }
    }
}
