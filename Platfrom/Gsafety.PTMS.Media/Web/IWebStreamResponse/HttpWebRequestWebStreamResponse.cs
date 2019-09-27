using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Web.WebRequestReader
{
    public sealed class HttpWebRequestWebStreamResponse : IWebStreamResponse
    {
        readonly int _httpStatusCode;
        readonly HttpWebRequest _request;
        readonly HttpWebResponse _response;
        Task<Stream> _streamTask;

        public HttpWebRequestWebStreamResponse(HttpWebRequest request, HttpWebResponse response)
        {
            if (null == response)
                throw new ArgumentNullException("response");

            _request = request;
            _response = response;
            _httpStatusCode = (int)_response.StatusCode;
        }

        public HttpWebRequestWebStreamResponse(HttpStatusCode statusCode)
        {
            _httpStatusCode = (int)statusCode;
        }

        public void Dispose()
        {
            if (null != _streamTask && _streamTask.IsCompleted)
                _streamTask.Result.Dispose();

            using (_response)
            { }
        }

        public bool IsSuccessStatusCode
        {
            get { return null != _response; }
        }

        public Uri ActualUrl
        {
            get { return null == _response ? null : _response.ResponseUri; }
        }

        public int HttpStatusCode
        {
            get { return _httpStatusCode; }
        }

        public long? ContentLength
        {
            get { return null == _response ? null : _response.ContentLength >= 0 ? _response.ContentLength : null as long?; }
        }

        public void EnsureSuccessStatusCode()
        {
            if (_httpStatusCode < 200 || _httpStatusCode >= 300)
                throw new WebException("Invalid status: " + _httpStatusCode);
        }

        public Task<Stream> GetStreamAsync(CancellationToken cancellationToken)
        {
            if (null != _streamTask)
                return _streamTask;

            using (cancellationToken.Register(r => ((HttpWebRequest)r).Abort(), _request))
            {
                var stream = _response.GetResponseStream();

                _streamTask = TaskExt.FromResult(stream);

                return _streamTask;
            }
        }
    }
}
