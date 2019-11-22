using System;
using System.Net;

namespace Gsafety.PTMS.Media.Web
{
    public class StatusCodeWebException : WebException
    {
        readonly HttpStatusCode _statusCode;

        public StatusCodeWebException(HttpStatusCode statusCode, string message, Exception innerException = null)
            : base(message, innerException)
        {
            _statusCode = statusCode;
        }

        public HttpStatusCode StatusCode
        {
            get { return _statusCode; }
        }

        public static void ThrowIfNotSuccess(HttpStatusCode statusCode, string message)
        {
            var code = (int)statusCode;

            if (code < 200 || code >= 300)
                throw new StatusCodeWebException(statusCode, message);
        }
    }
}
