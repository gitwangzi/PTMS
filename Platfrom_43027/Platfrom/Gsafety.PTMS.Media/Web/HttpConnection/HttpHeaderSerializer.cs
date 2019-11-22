using System;
using System.IO;
using System.Text;

namespace Gsafety.PTMS.Media.Web.HttpConnection
{
    public interface IHttpHeaderSerializer
    {
        void WriteHeader(Stream stream, string method, HttpConnectionRequest request);
    }

    public class HttpHeaderSerializer : IHttpHeaderSerializer
    {
        const string HttpEol = "\r\n";
        readonly Encoding _headerEncoding;
        readonly string _userAgentLine;

        public HttpHeaderSerializer(IUserAgentEncoder userAgentEncoder, IHttpEncoding httpEncoding)
        {
            if (null == userAgentEncoder)
                throw new ArgumentNullException("userAgentEncoder");
            if (null == httpEncoding)
                throw new ArgumentNullException("httpEncoding");

            var userAgent = userAgentEncoder.UsAsciiUserAgent;

            if (!string.IsNullOrWhiteSpace(userAgent))
                _userAgentLine = "User-Agent: " + userAgent.Trim();

            _headerEncoding = httpEncoding.HeaderEncoding;
        }

        public void WriteHeader(Stream stream, string method, HttpConnectionRequest request)
        {
            var url = request.Url;

            //using (var tw = new StreamWriter(stream, _headerEncoding, 1024, true))
            using (var tw = new StreamWriter(stream, _headerEncoding, 1024))
            {
                tw.NewLine = HttpEol;

                var requestTarget = GetRequestTarget(request);
                var host = GetHost(url);

                tw.WriteLine(method.ToUpperInvariant() + " " + requestTarget + " HTTP/1.1");
                tw.WriteLine("Host: " + host);
                tw.WriteLine(request.KeepAlive ? "Connection: Keep-Alive" : "Connection: Close");

                if (null != request.Referrer && request.Referrer.IsAbsoluteUri)
                    tw.WriteLine("Referer:" + request.Referrer.AbsoluteUri);

                if (request.RangeFrom.HasValue || request.RangeTo.HasValue)
                    tw.WriteLine("Range: bytes={0}-{1}", request.RangeFrom, request.RangeTo);

                if (null != _userAgentLine)
                    tw.WriteLine(_userAgentLine);

                if (!string.IsNullOrWhiteSpace(request.Accept))
                    tw.WriteLine("Accept: " + request.Accept.Trim());

                if (null != request.Cookies)
                {
                    var cookieHeader = request.Cookies.GetCookieHeader(url);

                    if (!string.IsNullOrWhiteSpace(cookieHeader))
                        tw.WriteLine("Cookies: " + cookieHeader);
                }

                if (null != request.Headers)
                {
                    foreach (var header in request.Headers)
                    {
                        var value = header.Value;

                        if (string.IsNullOrWhiteSpace(value))
                            value = string.Empty;
                        else
                            value = value.Trim();

                        tw.WriteLine(header.Key.Trim() + ": " + value);
                    }
                }

                tw.WriteLine();

                tw.Flush();
            }
        }

        static string GetHost(Uri url)
        {
            //return url.IsDefaultPort ? url.DnsSafeHost : url.DnsSafeHost + ':' + url.Port;
            return url.DnsSafeHost + ':' + url.Port;
        }

        static string GetRequestTarget(HttpConnectionRequest request)
        {
            var url = request.Url;
            var proxy = request.Proxy;

            if (null != proxy)
            {
                var ub = new UriBuilder(url) { Fragment = null };

                return ub.Uri.AbsoluteUri;
            }

            //var target = url.PathAndQuery;           
            var target = url.AbsolutePath + url.Query;

            return target;
        }
    }
}
