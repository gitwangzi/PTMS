using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.Utility;
using Gsafety.PTMS.Media.Web.HttpConnection;

namespace Gsafety.PTMS.Media.Web.HttpConnectionReader
{
    public sealed class HttpConnectionWebReader : IWebReader
    {
        readonly Uri _baseAddress;
        readonly IContentTypeDetector _contentTypeDetector;
        readonly Uri _referrer;
        readonly HttpConnectionWebReaderManager _webReaderManager;

        public HttpConnectionWebReader(HttpConnectionWebReaderManager webReaderManager, Uri baseAddress, Uri referrer, ContentType contentType, IContentTypeDetector contentTypeDetector)
        {
            if (null == webReaderManager)
                throw new ArgumentNullException("webReaderManager");
            if (contentTypeDetector == null)
                throw new ArgumentNullException("contentTypeDetector");

            _webReaderManager = webReaderManager;
            _baseAddress = baseAddress;
            _referrer = referrer;
            ContentType = contentType;
            _contentTypeDetector = contentTypeDetector;
        }


        public Uri BaseAddress
        {
            get { return _baseAddress; }
        }

        public Uri RequestUri { get; private set; }

        public ContentType ContentType { get; private set; }

        public IWebReaderManager Manager
        {
            get { return _webReaderManager; }
        }

        public void Dispose()
        { }

        public async Task<IWebStreamResponse> GetWebStreamAsync(Uri url, bool waitForContent, CancellationToken cancellationToken,
            Uri referrer = null, long? from = null, long? to = null, WebResponse webResponse = null)
        {
            var request = _webReaderManager.CreateRequest(url, referrer, this, ContentType, allowBuffering: waitForContent, fromBytes: from, toBytes: to);

            var response = await _webReaderManager.GetAsync(request, cancellationToken).ConfigureAwait(false);

            Update(url, response, webResponse);

            return new HttpConnectionWebStreamResponse(response);
        }

        public async Task<byte[]> GetByteArrayAsync(Uri url, CancellationToken cancellationToken, WebResponse webResponse = null)
        {
            if (null != _baseAddress && !url.IsAbsoluteUri)
                url = new Uri(_baseAddress, url);

            using (var response = await _webReaderManager.SendAsync(url, this, cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();

                Update(url, response, webResponse);

                using (var ms = new MemoryStream())
                {
                    await response.ContentReadStream.CopyToAsync(ms, 4096, cancellationToken).ConfigureAwait(false);

                    return ms.ToArray();
                }
            }
        }

        public async Task<IHttpConnectionResponse> SendAsync(HttpConnectionRequest request, bool allowBuffering, CancellationToken cancellationToken, WebResponse webResponse = null)
        {
            var url = request.Url;

            var response = await _webReaderManager.GetAsync(request, cancellationToken);

            Update(url, response, webResponse);

            return response;
        }

        public HttpConnectionRequest CreateWebRequest(Uri url, Uri referrer = null)
        {
            return _webReaderManager.CreateRequest(url, referrer ?? _referrer, this, ContentType);
        }

        void Update(Uri url, IHttpConnectionResponse response, WebResponse webResponse)
        {
            if (null != webResponse)
            {
                webResponse.RequestUri = response.ResponseUri;
                webResponse.ContentLength = response.Status.ContentLength >= 0 ? response.Status.ContentLength : null;
                webResponse.Headers = GetHeaders(response.Headers);

                webResponse.ContentType = _contentTypeDetector.GetContentType(response.ResponseUri, response.Headers["Content-Type"].FirstOrDefault()).SingleOrDefaultSafe();
            }

            if (url != BaseAddress)
                return;

            RequestUri = response.ResponseUri;

            if (null == ContentType)
                ContentType = _contentTypeDetector.GetContentType(RequestUri, response.Headers["Content-Type"].FirstOrDefault()).SingleOrDefaultSafe();
        }

        IEnumerable<KeyValuePair<string, IEnumerable<string>>> GetHeaders(ILookup<string, string> headers)
        {
            return headers.Select(h => new KeyValuePair<string, IEnumerable<string>>(h.Key, h));
        }

        public override string ToString()
        {
            var contentType = null == ContentType ? "<unknown>" : ContentType.ToString();

            if (null != RequestUri && RequestUri != BaseAddress)
                return string.Format("HttpConnectionReader {0} [{1}] ({2})", BaseAddress, RequestUri, contentType);

            return string.Format("HttpConnectionReader {0} ({1})", BaseAddress, contentType);
        }
    }
}
