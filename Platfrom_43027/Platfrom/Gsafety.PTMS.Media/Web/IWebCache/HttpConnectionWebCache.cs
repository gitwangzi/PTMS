using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Media.Utility;
using Gsafety.PTMS.Media.Web.HttpConnection;

namespace Gsafety.PTMS.Media.Web.HttpConnectionReader
{
    public class HttpConnectionWebCache : IWebCache
    {
        const string NoCache = "no-cache";

        readonly IRetryManager _retryManager;
        readonly HttpConnectionWebReader _webReader;
        string _cacheControl;
        object _cachedObject;
        string _etag;
        bool _firstRequestCompleted;
        string _lastModified;
        string _noCache;

        public HttpConnectionWebCache(HttpConnectionWebReader webReader, IRetryManager retryManager)
        {
            if (webReader == null)
                throw new ArgumentNullException("webReader");
            if (null == retryManager)
                throw new ArgumentNullException("retryManager");

            _webReader = webReader;
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
                    if (response.IsSuccessStatusCode)
                    {
                        _firstRequestCompleted = true;
                        _cachedObject = factory(response.ResponseUri, await FetchObject(response, cancellationToken).ConfigureAwait(false));
                        return;
                    }

                    var statusCode = response.Status.StatusCode;

                    if (HttpStatusCode.NotModified == statusCode)
                        return;

                    if (!RetryPolicy.IsRetryable(statusCode))
                        goto fail;

                    if (await retry.CanRetryAfterDelayAsync(cancellationToken).ConfigureAwait(false))
                        continue;

                fail:
                    _cachedObject = null;
                
                    response.EnsureSuccessStatusCode();
                    
                    throw new WebException("Unable to fetch " + request.Url);
                }
            }
        }

        async Task<byte[]> FetchObject(IHttpConnectionResponse response, CancellationToken cancellationToken)
        {
            _lastModified = response.Headers["Last-Modified"].FirstOrDefault();

            _etag = response.Headers["ETag"].FirstOrDefault();

            _cacheControl = response.Headers["CacheControl"].FirstOrDefault();

            using (var ms = new MemoryStream())
            {
                await response.ContentReadStream.CopyToAsync(ms, 4096, cancellationToken).ConfigureAwait(false);

                return ms.ToArray();
            }
        }

        HttpConnectionRequest CreateRequest()
        {
            var url = WebReader.BaseAddress;

            var haveConditional = false;

            if (null == _cachedObject)
            {
                _lastModified = null;
                _etag = null;
            }

            if (null != _lastModified)
                haveConditional = true;

            if (null != _etag)
                haveConditional = true;

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

            var headers = new List<KeyValuePair<string, string>>();

            if (null != _lastModified)
                headers.Add(new KeyValuePair<string, string>("If-Modified-Since", _lastModified));

            if (null != _etag)
                headers.Add(new KeyValuePair<string, string>("If-None-Match", _etag));

            if (!haveConditional)
                headers.Add(new KeyValuePair<string, string>("Cache-Control", NoCache));

            if (headers.Count > 0)
                request.Headers = headers;

            return request;
        }
    }
}
