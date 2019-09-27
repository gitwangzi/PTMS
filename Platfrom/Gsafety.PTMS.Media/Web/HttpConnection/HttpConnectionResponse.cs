using System;
using System.IO;
using System.Linq;
using System.Net;

namespace Gsafety.PTMS.Media.Web.HttpConnection
{
    public interface IHttpConnectionResponse : IDisposable
    {
        ILookup<string, string> Headers { get; }
        Stream ContentReadStream { get; }
        IHttpStatus Status { get; }
        Uri ResponseUri { get; }

        bool IsSuccessStatusCode { get; }
        void EnsureSuccessStatusCode();
    }

    public class HttpConnectionResponse : IHttpConnectionResponse
    {
        readonly ILookup<string, string> _headers;
        readonly IHttpStatus _status;
        readonly Uri _url;
        IHttpConnection _connection;
        IHttpReader _reader;
        Stream _stream;

        public HttpConnectionResponse(Uri url, IHttpConnection connection, IHttpReader reader, Stream stream, ILookup<string, string> headers, IHttpStatus status)
        {
            if (null == url)
                throw new ArgumentNullException("url");
            if (null == stream)
                throw new ArgumentNullException("stream");
            if (null == headers)
                throw new ArgumentNullException("headers");
            if (null == status)
                throw new ArgumentNullException("status");

            _url = url;
            _reader = reader;
            _stream = stream;
            _headers = headers;
            _status = status;
            _connection = connection;
        }

        public void Dispose()
        {
            var stream = _stream;

            if (null != stream)
            {
                _stream = null;

                stream.Dispose();
            }

            var reader = _reader;

            if (null != reader)
            {
                _reader = null;

                reader.Dispose();
            }

            var connection = _connection;

            if (null != connection)
            {
                _connection = connection;

                connection.Dispose();
            }
        }

        public ILookup<string, string> Headers
        {
            get { return _headers; }
        }

        public Stream ContentReadStream
        {
            get { return _stream; }
        }

        public IHttpStatus Status
        {
            get { return _status; }
        }

        public Uri ResponseUri
        {
            get { return _url; }
        }

        public bool IsSuccessStatusCode
        {
            get { return null != Status && Status.IsSuccessStatusCode; }
        }

        public void EnsureSuccessStatusCode()
        {
            if (null == Status)
                throw new WebException("No status available", WebExceptionStatus.UnknownError);

            Status.EnsureSuccessStatusCode();
        }
    }
}
