using System;
using System.Net;

namespace Gsafety.PTMS.Media.Web.HttpConnection
{
    public interface IHttpStatus
    {
        bool ChunkedEncoding { get; }
        long? ContentLength { get; }
        HttpStatusCode StatusCode { get; }
        int VersionMajor { get; }
        int VersionMinor { get; }
        string ResponsePhrase { get; }
        string Version { get; }
        bool IsHttp { get; }

        bool IsSuccessStatusCode { get; }
    }

    public sealed class HttpStatus : IHttpStatus
    {

        public bool ChunkedEncoding { get; set; }
        public long? ContentLength { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }
        public string ResponsePhrase { get; set; }
        public string Version { get; set; }
        public bool IsHttp { get; set; }

        public bool IsSuccessStatusCode
        {
            get
            {
                var statusCode = (int)StatusCode;

                return statusCode >= 200 && statusCode <= 299;
            }
        }
    }

    public static class HttpStatusExtensions
    {
        public static void EnsureSuccessStatusCode(this IHttpStatus httpStatus)
        {
            if (null == httpStatus)
                throw new ArgumentNullException("httpStatus");

            if (!httpStatus.IsSuccessStatusCode)
                throw new StatusCodeWebException(httpStatus.StatusCode, httpStatus.ResponsePhrase);
        }
    }
}
