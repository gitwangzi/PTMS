using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.Utility;
using Gsafety.PTMS.Media.Web.HttpConnection;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media.Web.HttpConnectionReader
{
    public class HttpConnectionWebReaderManager : IWebReaderManager, IDisposable
    {
        readonly IContentTypeDetector _contentTypeDetector;
        readonly IHttpConnectionFactory _httpConnectionFactory;
        readonly IHttpConnectionRequestFactory _httpConnectionRequestFactory;
        readonly IRetryManager _retryManager;
        readonly IWebReaderManagerParameters _webReaderManagerParameters;
        int _disposed;

        public HttpConnectionWebReaderManager(IHttpConnectionFactory httpConnectionFactory, IHttpConnectionRequestFactory httpConnectionRequestFactory, IWebReaderManagerParameters webReaderManagerParameters, IContentTypeDetector contentTypeDetector, IRetryManager retryManager)
        {
            if (null == httpConnectionFactory)
                throw new ArgumentNullException("httpConnectionFactory");
            if (null == httpConnectionRequestFactory)
                throw new ArgumentNullException("httpConnectionRequestFactory");
            if (null == webReaderManagerParameters)
                throw new ArgumentNullException("webReaderManagerParameters");
            if (null == contentTypeDetector)
                throw new ArgumentNullException("contentTypeDetector");
            if (null == retryManager)
                throw new ArgumentNullException("retryManager");

            _httpConnectionFactory = httpConnectionFactory;
            _httpConnectionRequestFactory = httpConnectionRequestFactory;
            _webReaderManagerParameters = webReaderManagerParameters;
            _contentTypeDetector = contentTypeDetector;
            _retryManager = retryManager;
        }

        public void Dispose()
        {
            if (0 != Interlocked.Exchange(ref _disposed, 1))
                return;

            Dispose(true);

            GC.SuppressFinalize(this);
        }

        internal Task<IHttpConnectionResponse> SendAsync(Uri url, IWebReader parent, CancellationToken cancellationToken, string method = null, ContentType contentType = null, bool allowBuffering = true, Uri referrer = null, long? fromBytes = null, long? toBytes = null)
        {
            var request = CreateRequest(url, referrer, parent, contentType, method, allowBuffering, fromBytes, toBytes);

            return GetAsync(request, cancellationToken);
        }

        protected virtual HttpConnectionWebReader CreateHttpConnectionWebReader(Uri url, IWebReader parent = null, ContentType contentType = null)
        {
            if (null == contentType && null != url)
                contentType = _contentTypeDetector.GetContentType(url).SingleOrDefaultSafe();

            return new HttpConnectionWebReader(this, url, parent == null ? null : parent.BaseAddress, contentType, _contentTypeDetector);
        }

        internal virtual async Task<IHttpConnectionResponse> GetAsync(HttpConnectionRequest request, CancellationToken cancellationToken)
        {
            var connection = _httpConnectionFactory.CreateHttpConnection();

            var requestUrl = request.Url;
            var url = requestUrl;

            var retry = 0;

            for (; ; )
            {
                var task = connection.ConnectAsync(request.Proxy ?? url, cancellationToken).ConfigureAwait(false);
                await task;

                request.Url = url; // TODO: Unhack this...
                var response = await connection.GetAsync(request, true, cancellationToken).ConfigureAwait(false);
                request.Url = requestUrl;

                var status = response.Status;
                if (HttpStatusCode.Moved != status.StatusCode && HttpStatusCode.Redirect != status.StatusCode)
                    return response;

                if (++retry >= 8)
                    return response;

                connection.Close();

                var location = response.Headers["Location"].FirstOrDefault();

                if (!Uri.TryCreate(request.Url, location, out url))
                    return response;
            }
        }

        internal virtual HttpConnectionRequest CreateRequest(Uri url, Uri referrer, IWebReader parent, ContentType contentType, string method = null, bool allowBuffering = false, long? fromBytes = null, long? toBytes = null)
        {
            referrer = referrer ?? GetReferrer(url, parent);

            if (null == url && null != parent)
                url = parent.RequestUri ?? parent.BaseAddress;

            if (null != referrer && (null == url || !url.IsAbsoluteUri))
                url = new Uri(referrer, url);

            var request = _httpConnectionRequestFactory.CreateRequest(url, referrer, contentType, fromBytes, toBytes, _webReaderManagerParameters.DefaultHeaders);

            return request;
        }

        protected static Uri GetReferrer(Uri url, IWebReader parent)
        {
            if (null == parent || null == url)
                return null;

            var parentUrl = parent.RequestUri ?? parent.BaseAddress;

            if (parentUrl == url)
                return null;

            return parentUrl;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
        }

        public virtual IWebReader CreateReader(Uri url, ContentKind contentKind, IWebReader parent = null, ContentType contentType = null)
        {
            return CreateHttpConnectionWebReader(url, parent, contentType);
        }

        public virtual IWebCache CreateWebCache(Uri url, ContentKind contentKind, IWebReader parent = null, ContentType contentType = null)
        {
            var webReader = CreateHttpConnectionWebReader(url, parent, contentType);

            return new HttpConnectionWebCache(webReader, _retryManager);
        }

        public virtual async Task<ContentType> DetectContentTypeAsync(Uri url, ContentKind contentKind, CancellationToken cancellationToken, IWebReader parent = null)
        {
            var contentType = _contentTypeDetector.GetContentType(url).SingleOrDefaultSafe();

            if (null != contentType)
            {
                LoggerInstance.Debug("HttpConnectionWebReaderManager.DetectContentTypeAsync() url ext \"{0}\" type {1}", url, contentType);
                return contentType;
            }

            try
            {
                using (var response = await SendAsync(url, parent, cancellationToken, "HEAD", allowBuffering: false).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                        contentType = _contentTypeDetector.GetContentType(response.ResponseUri, response.Headers["Content-Type"].FirstOrDefault()).SingleOrDefaultSafe();

                    if (null != contentType)
                    {
                        LoggerInstance.Debug("HttpConnectionWebReaderManager.DetectContentTypeAsync() url HEAD \"{0}\" type {1}", url, contentType);
                        return contentType;
                    }
                }
            }
            catch (WebException)
            {
                // Well, HEAD didn't work...
            }

            try
            {
                using (var response = await SendAsync(url, parent, cancellationToken, allowBuffering: false, fromBytes: 0, toBytes: 0).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                        contentType = _contentTypeDetector.GetContentType(response.ResponseUri, response.Headers["Content-Type"].FirstOrDefault()).SingleOrDefaultSafe();

                    if (null != contentType)
                    {
                        LoggerInstance.Debug("HttpConnectionWebReaderManager.DetectContentTypeAsync() url range GET \"{0}\" type {1}", url, contentType);
                        return contentType;
                    }
                }
            }
            catch (WebException)
            {
                // Well, a ranged GET didn't work either.
            }

            try
            {
                using (var response = await SendAsync(url, parent, cancellationToken, allowBuffering: false).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                        contentType = _contentTypeDetector.GetContentType(response.ResponseUri, response.Headers["Content-Type"].FirstOrDefault()).SingleOrDefaultSafe();

                    if (null != contentType)
                    {
                        LoggerInstance.Debug("HttpConnectionWebReaderManager.DetectContentTypeAsync() url GET \"{0}\" type {1}", url, contentType);
                        return contentType;
                    }
                }
            }
            catch (WebException)
            {
                // This just isn't going to work.
            }

            LoggerInstance.Debug("HttpConnectionWebReaderManager.DetectContentTypeAsync() url header \"{0}\" unknown type", url);

            return null;
        }
    }
}
