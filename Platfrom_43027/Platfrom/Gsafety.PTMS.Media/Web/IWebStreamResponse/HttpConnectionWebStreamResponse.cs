using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Media.Web.HttpConnection;

namespace Gsafety.PTMS.Media.Web.HttpConnectionReader
{
    public sealed class HttpConnectionWebStreamResponse : IWebStreamResponse
    {
        readonly IHttpStatus _httpStatus;
        readonly IHttpConnectionResponse _response;

        public HttpConnectionWebStreamResponse(IHttpConnectionResponse response)
        {
            if (null == response)
                throw new ArgumentNullException("response");
            if (null == response.Status)
                throw new ArgumentException("Not status in response", "response");

            _response = response;
            _httpStatus = _response.Status;
        }

        public HttpConnectionWebStreamResponse(IHttpStatus httpStatus)
        {
            if (null == httpStatus)
                throw new ArgumentNullException("httpStatus");

            _httpStatus = httpStatus;
        }

        public void Dispose()
        {
            using (_response)
            { }
        }

        public bool IsSuccessStatusCode
        {
            get { return _httpStatus.IsSuccessStatusCode; }
        }

        public Uri ActualUrl
        {
            get { return null == _response ? null : _response.ResponseUri; }
        }

        public int HttpStatusCode
        {
            get { return (int)_httpStatus.StatusCode; }
        }

        public long? ContentLength
        {
            get { return null == _response ? null : _response.Status.ContentLength >= 0 ? _response.Status.ContentLength : null; }
        }

        public void EnsureSuccessStatusCode()
        {
            _httpStatus.EnsureSuccessStatusCode();
        }

        public Task<Stream> GetStreamAsync(CancellationToken cancellationToken)
        {
            return TaskExt.FromResult(_response.ContentReadStream);
        }
    }
}
