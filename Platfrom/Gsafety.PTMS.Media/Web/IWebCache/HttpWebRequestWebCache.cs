using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Media.Utility;

namespace Gsafety.PTMS.Media.Web.WebRequestReader
{
    public class HttpWebRequestWebCache : IWebCache
    {
        const string NoCache = "no-cache";

        readonly IHttpWebRequests _httpWebRequests;
        readonly IRetryManager _retryManager;
        readonly HttpWebRequestWebReader _webReader;
        string _cacheControl;
        object _cachedObject;
        string _etag;
        bool _firstRequestCompleted;
        string _lastModified;
        string _noCache;

        public HttpWebRequestWebCache(HttpWebRequestWebReader webReader, IHttpWebRequests httpWebRequests, IRetryManager retryManager)
        {
            if (webReader == null)
                throw new ArgumentNullException("webReader");
            if (null == httpWebRequests)
                throw new ArgumentNullException("httpWebRequests");
            if (null == retryManager)
                throw new ArgumentNullException("retryManager");

            _webReader = webReader;
            _httpWebRequests = httpWebRequests;
            _retryManager = retryManager;
        }

        public IWebReader WebReader
        {
            get { return _webReader; }
        }

        public async Task<TCached> ReadAsync<TCached>(Func<Uri, byte[], TCached> factory, CancellationToken cancellationToken, WebResponse webResponse = null)
            where TCached : class
        {
            if (null == _cachedObject as TCached)
                _cachedObject = null;

            var retry = _retryManager.CreateWebRetry(2, 250);

            await retry
                .CallAsync(() => Fetch(retry, factory, webResponse, cancellationToken), cancellationToken)
                .ConfigureAwait(false);

            return _cachedObject as TCached;
        }

        async Task Fetch<TCached>(IRetry retry, Func<Uri, byte[], TCached> factory, WebResponse webResponse, CancellationToken cancellationToken)
            where TCached : class
        {
            for (; ; )
            {
                var request = CreateRequest();

                using (var response = await _webReader.SendAsync(request, true, cancellationToken, webResponse)
                                                      .ConfigureAwait(false))
                {
                    var statusCode = (int)response.StatusCode;

                    if (statusCode >= 200 && statusCode < 300)
                    {
                        _firstRequestCompleted = true;
                        _cachedObject = factory(response.ResponseUri, await FetchObject(response, cancellationToken).ConfigureAwait(false));
                        return;
                    }

                    if (HttpStatusCode.NotModified == response.StatusCode)
                        return;

                    if (!RetryPolicy.IsRetryable(response.StatusCode))
                        goto fail;

                    if (await retry.CanRetryAfterDelayAsync(cancellationToken).ConfigureAwait(false))
                        continue;

                fail:
                    _cachedObject = null;
                    throw new WebException("Unable to fetch");
                }
            }
        }

        Task<byte[]> FetchObject(HttpWebResponse response, CancellationToken cancellationToken)
        {
            _lastModified = response.Headers["Last-Modified"];

            _etag = response.Headers["ETag"];

            _cacheControl = response.Headers["CacheControl"];

            return response.ReadAsByteArrayAsync(cancellationToken);
        }

        HttpWebRequest CreateRequest()
        {
            var url = WebReader.BaseAddress;

            var haveConditional = false;

            if (null != _cachedObject)
            {
                if (null != _lastModified)
                    haveConditional = true;

                if (null != _etag)
                    haveConditional = true;
            }

            // Do not rotate the nocache query string if the server has an explicit cache policy.
            if (_firstRequestCompleted && (!haveConditional && null == _cacheControl))
                _noCache = "nocache=" + Guid.NewGuid().ToString("N");

            if (null != _noCache)
            {
                var ub = new UriBuilder(url);

                if (string.IsNullOrEmpty(ub.Query))
                    ub.Query = _noCache;
                else
                    ub.Query = ub.Query.Substring(1) + "&" + _noCache;

                url = ub.Uri;
            }

            var request = _webReader.CreateWebRequest(url);

            if (null != _cachedObject && haveConditional)
            {
                haveConditional = false;

                if (_httpWebRequests.SetIfModifiedSince(request, _lastModified))
                    haveConditional = true;

                if (_httpWebRequests.SetIfNoneMatch(request, _etag))
                    haveConditional = true;
            }

            if (!haveConditional)
                _httpWebRequests.SetCacheControl(request, NoCache);

            return request;
        }
    }
}
