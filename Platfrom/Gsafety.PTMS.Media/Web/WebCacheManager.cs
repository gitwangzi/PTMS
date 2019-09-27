using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.Utility;
using System.Diagnostics;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media.Web
{
    public interface IWebCacheManager : IDisposable
    {
        Task FlushAsync();

        Task<TCached> ReadAsync<TCached>(Uri uri, Func<Uri, byte[], TCached> factory, ContentKind contentKind, ContentType contentType, CancellationToken cancellationToken)
            where TCached : class;
    }

    public class WebCacheManager : IWebCacheManager
    {
        readonly Dictionary<Uri, CacheEntry> _cache = new Dictionary<Uri, CacheEntry>();
        readonly IWebReader _webReader;
        CancellationTokenSource _flushCancellationTokenSource = new CancellationTokenSource();

        public WebCacheManager(IWebReader webReader)
        {
            if (null == webReader)
                throw new ArgumentNullException("webReader");

            _webReader = webReader;
        }

        public async Task FlushAsync()
        {
            _flushCancellationTokenSource.Cancel();

            CacheEntry[] cacheEntries;

            lock (_cache)
            {
                cacheEntries = _cache.Values.ToArray();
                _cache.Clear();
            }

            try
            {
                await TaskExt.WhenAll(cacheEntries.Where(c => null != c.ReadTask).Select(c => c.ReadTask));
            }
            catch (OperationCanceledException)
            { }
            catch (Exception ex)
            {
                LoggerInstance.Exception("WebCacheManager.FlushAsync() exception: " + ex.ExtendedMessage());
            }

            foreach (var cacheEntry in cacheEntries)
                cacheEntry.WebCache.WebReader.Dispose();

            var fcts = _flushCancellationTokenSource;

            _flushCancellationTokenSource = new CancellationTokenSource();

            fcts.Dispose();
        }

        public Task<TCached> ReadAsync<TCached>(Uri uri, Func<Uri, byte[], TCached> factory, ContentKind contentKind, ContentType contentType, CancellationToken cancellationToken)
            where TCached : class
        {
            CacheEntry cacheEntry;
            TaskCompletionSource<TCached> tcs = null;

            lock (_cache)
            {
                if (_cache.TryGetValue(uri, out cacheEntry))
                {
                    if (cacheEntry.ReadTask.IsCompleted && cacheEntry.Age > TimeSpan.FromSeconds(5))
                    {
                        tcs = new TaskCompletionSource<TCached>();

                        cacheEntry.ReadTask = tcs.Task;
                    }
                }
                else
                {
                    var wc = _webReader.CreateWebCache(uri, contentKind, contentType);

                    tcs = new TaskCompletionSource<TCached>();

                    cacheEntry = new CacheEntry
                    {
                        WebCache = wc,
                        ReadTask = tcs.Task
                    };

                    _cache[uri] = cacheEntry;
                }
            }

            if (null == tcs)
                return (Task<TCached>)cacheEntry.ReadTask;

            var task = cacheEntry.WebCache.ReadAsync(factory, cancellationToken);

            task.ContinueWith(t =>
            {
                cacheEntry.ResetTime();

                var ex = t.Exception;

                if (null != ex)
                    tcs.TrySetCanceled();
                else if (t.IsCanceled)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(task.Result);
            }, cancellationToken);

            return tcs.Task;
        }

        class CacheEntry
        {

            Stopwatch _lastUpdate;

            public TimeSpan Age
            {
                get { return _lastUpdate.Elapsed; }
            }

            public void ResetTime()
            {
                _lastUpdate = Stopwatch.StartNew();
            }

            public Task ReadTask;
            public IWebCache WebCache;
        }

        public void Dispose()
        {
            _webReader.Dispose();
        }
    }
}
