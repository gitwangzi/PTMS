using Gsafety.PTMS.Media.Common.Loggers;
using System;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Utility
{
    public static class DisposeExtensions
    {
        /// <summary>
        ///     IDisposable.Dispose() should not throw exceptions; this will catch them if they do.
        /// </summary>
        /// <param name="disposable"></param>
        public static void DisposeSafe(this IDisposable disposable)
        {
            if (null == disposable)
                return;

            try
            {
                disposable.Dispose();
            }
            catch (Exception ex)
            {
                LoggerInstance.Debug("DisposeExtensions.DisposeSafe() for {0} failed: {1}", disposable.GetType().FullName, ex.Message);
            }
        }

        /// <summary>
        ///     "Async over sync" wrapper for DisposeSafe().
        ///     See http://blogs.msdn.com/b/pfxteam/archive/2012/04/13/10293638.aspx
        /// </summary>
        /// <param name="disposable"></param>
        /// <returns></returns>
        public static Task DisposeAsync(this IDisposable disposable)
        {
            if (null == disposable)
                return TplTaskExtensions.CompletedTask;

            return TaskExt.Run(() => disposable.DisposeSafe());
        }

        /// <summary>
        ///     DisposeSafe() the object in the background ("async over sync").
        /// </summary>
        /// <param name="disposable"></param>
        /// <param name="description"></param>
        public static void DisposeBackground(this IDisposable disposable, string description)
        {
            TaskCollector.Default.Add(disposable.DisposeAsync(), description);
        }
    }
}
