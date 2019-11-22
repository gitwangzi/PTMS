using System;
using System.Collections.Generic;
using System.Text;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.Web.HttpConnection;

namespace Gsafety.PTMS.Media.Web.HttpConnectionReader
{
    public interface IHttpConnectionRequestFactory
    {
        HttpConnectionRequest CreateRequest(Uri url, Uri referrer, ContentType contentType, long? fromBytes, long? toBytes, IEnumerable<KeyValuePair<string, string>> headers);
    }

    public class HttpConnectionRequestFactory : IHttpConnectionRequestFactory
    {
        readonly IHttpConnectionRequestFactoryParameters _parameters;

        public HttpConnectionRequestFactory(IHttpConnectionRequestFactoryParameters parameters)
        {
            if (null == parameters)
                throw new ArgumentNullException("parameters");

            _parameters = parameters;
        }

        public virtual HttpConnectionRequest CreateRequest(Uri url, Uri referrer, ContentType contentType, long? fromBytes, long? toBytes, IEnumerable<KeyValuePair<string, string>> headers)
        {
            var request = new HttpConnectionRequest
            {
                Url = url,
                Referrer = referrer,
                RangeFrom = fromBytes,
                RangeTo = toBytes,
                Proxy = _parameters.Proxy,
                Headers = headers,
                Cookies = _parameters.Cookies
            };

            if (null != contentType)
                request.Accept = CreateAcceptHeader(contentType);

            return request;
        }

        protected virtual string CreateAcceptHeader(ContentType contentType)
        {
            var sb = new StringBuilder();

            sb.Append(contentType.MimeType);

            if (null != contentType.AlternateMimeTypes)
            {
                foreach (var mimeType in contentType.AlternateMimeTypes)
                {
                    sb.Append(", ");
                    sb.Append(mimeType);
                }
            }

            sb.Append(", */*; q=0.1");

            var accept = sb.ToString();

            return accept;
        }
    }
}
