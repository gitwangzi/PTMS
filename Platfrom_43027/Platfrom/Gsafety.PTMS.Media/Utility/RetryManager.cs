using System;

namespace Gsafety.PTMS.Media.Utility
{
    public interface IRetryManager
    {
        IRetry CreateRetry(int maxRetries, int delayMilliseconds, Func<Exception, bool> retryableException);
    }

    public class RetryManager : IRetryManager
    {
        readonly IPlatformServices _platformServices;

        public RetryManager(IPlatformServices platformServices)
        {
            if (null == platformServices)
                throw new ArgumentNullException("platformServices");

            _platformServices = platformServices;
        }

        #region IRetryManager Members

        public IRetry CreateRetry(int maxRetries, int delayMilliseconds, Func<Exception, bool> retryableException)
        {
            return new Retry(maxRetries, delayMilliseconds, retryableException, _platformServices);
        }

        #endregion
    }

    public static class RetryManagerExtensions
    {
        public static IRetry CreateWebRetry(this IRetryManager retryManager, int maxRetries, int delayMilliseconds)
        {
            return retryManager.CreateRetry(maxRetries, delayMilliseconds, RetryPolicy.IsWebExceptionRetryable);
        }
    }
}
