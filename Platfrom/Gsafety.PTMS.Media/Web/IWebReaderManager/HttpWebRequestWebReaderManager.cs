using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.Utility;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media.Web.WebRequestReader
{
    public class HttpWebRequestWebReaderManager : IWebReaderManager, IDisposable
    {
        readonly IContentTypeDetector _contentTypeDetector;
        readonly IHttpWebRequests _httpWebRequests;
        readonly IRetryManager _retryManager;
        readonly IWebReaderManagerParameters _webReaderManagerParameters;
        int _disposed;

        public HttpWebRequestWebReaderManager(IHttpWebRequests httpWebRequests, IWebReaderManagerParameters webReaderManagerParameters, IContentTypeDetector contentTypeDetector, IRetryManager retryManager)
        {
            if (null == httpWebRequests)
                throw new ArgumentNullException("httpWebRequests");
            if (null == webReaderManagerParameters)
                throw new ArgumentNullException("webReaderManagerParameters");
            if (null == contentTypeDetector)
                throw new ArgumentNullException("contentTypeDetector");
            if (null == retryManager)
                throw new ArgumentNullException("retryManager");

            _httpWebRequests = httpWebRequests;
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

        public virtual IWebReader CreateReader(Uri url, ContentKind contentKind, IWebReader parent, ContentType contentType)
        {
            return CreateHttpWebRequestWebReader(url, parent, contentType);
        }

        public virtual IWebCache CreateWebCache(Uri url, ContentKind contentKind, IWebReader parent = null, ContentType contentType = null)
        {
            var webReader = CreateHttpWebRequestWebReader(url, parent, contentType);

            return new HttpWebRequestWebCache(webReader, _httpWebRequests, _retryManager);
        }

        public virtual async Task<ContentType> DetectContentTypeAsync(Uri url, ContentKind contentKind, CancellationToken cancellationToken, IWebReader parent = null)
        {
            var contentType = _contentTypeDetector.GetContentType(url).SingleOrDefaultSafe();

            if (null != contentType)
            {
                LoggerInstance.Debug("HttpWebRequestWebReaderManager.DetectContentTypeAsync() url ext \"{0}\" type {1}", url, contentType);
                return contentType;
            }

            try
            {
                using (var response = await SendAsync(url, parent, cancellationToken, "HEAD", allowBuffering: false).ConfigureAwait(false))
                {
                    contentType = _contentTypeDetector.GetContentType(response.ResponseUri, response.Headers[HttpRequestHeader.ContentType]).SingleOrDefaultSafe();

                    if (null != contentType)
                    {
                        LoggerInstance.Debug("HttpWebRequestWebReaderManager.DetectContentTypeAsync() url HEAD \"{0}\" type {1}", url, contentType);
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
                    contentType = _contentTypeDetector.GetContentType(response.ResponseUri, response.Headers[HttpRequestHeader.ContentType]).SingleOrDefaultSafe();

                    if (null != contentType)
                    {
                        LoggerInstance.Debug("HttpWebRequestWebReaderManager.DetectContentTypeAsync() url range GET \"{0}\" type {1}", url, contentType);
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
                    contentType = _contentTypeDetector.GetContentType(response.ResponseUri, response.Headers[HttpRequestHeader.ContentType]).SingleOrDefaultSafe();

                    if (null != contentType)
                    {
                        LoggerInstance.Debug("HttpWebRequestWebReaderManager.DetectContentTypeAsync() url GET \"{0}\" type {1}", url, contentType);
                        return contentType;
                    }
                }
            }
            catch (WebException)
            {
                // This just isn't going to work.
            }

            LoggerInstance.Debug("HttpWebRequestWebReaderManager.DetectContentTypeAsync() url header \"{0}\" unknown type", url);

            return null;
        }

        internal async Task<HttpWebResponse> SendAsync(Uri url, IWebReader parent, CancellationToken cancellationToken, string method = null, ContentType contentType = null, bool allowBuffering = true, Uri referrer = null, long? fromBytes = null, long? toBytes = null)
        {
            var request = CreateRequest(url, referrer, parent, contentType, method, allowBuffering, fromBytes, toBytes);

            return await request.SendAsync(cancellationToken).ConfigureAwait(false);
        }

        protected virtual HttpWebRequestWebReader CreateHttpWebRequestWebReader(Uri url, IWebReader parent = null, ContentType contentType = null)
        {
            if (null == contentType && null != url)
                contentType = _contentTypeDetector.GetContentType(url).SingleOrDefaultSafe();

            return new HttpWebRequestWebReader(this, url, null == parent ? null : parent.BaseAddress, contentType, _contentTypeDetector);
        }

        internal virtual HttpWebRequest CreateRequest(Uri url, Uri referrer, IWebReader parent, ContentType contentType, string method = null, bool allowBuffering = false, long? fromBytes = null, long? toBytes = null)
        {
            referrer = referrer ?? GetReferrer(parent);

            if (null == url && null != parent)
                url = parent.RequestUri ?? parent.BaseAddress;

            if (null != referrer && (null == url || !url.IsAbsoluteUri))
                url = new Uri(referrer, url);

            var request = _httpWebRequests.CreateWebRequest(url, referrer, method, contentType, allowBuffering, fromBytes, toBytes);

            if (null != _webReaderManagerParameters.DefaultHeaders)
            {
                foreach (var header in _webReaderManagerParameters.DefaultHeaders)
                {
                    try
                    {
                        request.Headers[header.Key] = header.Value;
                    }
                    catch (ArgumentException ex)
                    {
                        LoggerInstance.Debug("HttpWebRequestWebReaderManager.CreateRequest({0}) header {1}={2} failed: {3}",
                            url, header.Key, header.Value, ex.ExtendedMessage());
                    }
                }
            }
            return request;
        }

        protected static Uri GetReferrer(IWebReader parent)
        {
            return null == parent ? null : parent.RequestUri ?? parent.BaseAddress;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
        }
    }
}
