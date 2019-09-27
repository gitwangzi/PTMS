using Gsafety.PTMS.Media.Common.Loggers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Utility
{
    public interface IRetry
    {
        Task<TResult> CallAsync<TResult>(Func<Task<TResult>> operation, CancellationToken cancellationToken);
        Task<bool> CanRetryAfterDelayAsync(CancellationToken cancellationToken);
    }

    public class Retry : IRetry
    {
        static readonly IEnumerable<Exception> NoExceptions = new Exception[0];
        readonly int _delayMilliseconds;
        readonly int _maxRetries;
        readonly IPlatformServices _platformServices;
        readonly Func<Exception, bool> _retryableException;
        int _delay;
        List<Exception> _exceptions;
        int _retry;

        public Retry(int maxRetries, int delayMilliseconds, Func<Exception, bool> retryableException, IPlatformServices platformServices)
        {
            if (maxRetries < 1)
                throw new ArgumentOutOfRangeException("maxRetries", "The number of retries must be positive.");
            if (delayMilliseconds < 0)
                throw new ArgumentOutOfRangeException("delayMilliseconds", "The delay cannot be negative");
            if (null == retryableException)
                throw new ArgumentNullException("retryableException");
            if (null == platformServices)
                throw new ArgumentNullException("platformServices");

            _maxRetries = maxRetries;
            _delayMilliseconds = delayMilliseconds;
            _retryableException = retryableException;
            _platformServices = platformServices;
            _retry = 0;
            _delay = 0;
        }

        #region IRetry Members

        public async Task<TResult> CallAsync<TResult>(Func<Task<TResult>> operation, CancellationToken cancellationToken)
        {
            _retry = 0;
            _delay = _delayMilliseconds;

            if (null != _exceptions)
                _exceptions.Clear();

            for (; ; )
            {
                try
                {
                    return await operation().ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    if (!_retryableException(ex))
                        throw;

                    if (null == _exceptions)
                        _exceptions = new List<Exception>();

                    _exceptions.Add(ex);

                    if (++_retry >= _maxRetries)
                        throw new RetryException("Giving up after " + _retry + " retries", _exceptions);

                    LoggerInstance.Debug("Retry {0} after: {1}", _retry, ex.Message);
                }

                await DelayAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<bool> CanRetryAfterDelayAsync(CancellationToken cancellationToken)
        {
            if (_retry >= _maxRetries)
                return false;

            ++_retry;

            await DelayAsync(cancellationToken).ConfigureAwait(false);

            return true;
        }

        #endregion

        async Task DelayAsync(CancellationToken cancellationToken)
        {
            var actualDelay = (int)(_delay * (0.5 + _platformServices.GetRandomNumber()));

            _delay += _delay;

            await TaskExt.Delay(actualDelay, cancellationToken).ConfigureAwait(false);
        }
    }

    public static class RetryExtensions
    {
        public static Task CallAsync(this IRetry retry, Func<Task> operation, CancellationToken cancellationToken)
        {
            return retry.CallAsync(async () =>
            {
                await operation().ConfigureAwait(false);
                return 0;
            }, cancellationToken);
        }
    }
}
