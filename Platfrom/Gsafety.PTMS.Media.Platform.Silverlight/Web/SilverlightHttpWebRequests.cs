using System;
using System.Net;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.Web.WebRequestReader;

namespace Gsafety.PTMS.Media.Web
{
    public class SilverlightHttpWebRequests : PclHttpWebRequests
    {
        public SilverlightHttpWebRequests(ICredentials credentials = null, CookieContainer cookieContainer = null)
            : base(credentials, cookieContainer)
        { }

        public override HttpWebRequest CreateWebRequest(Uri url, Uri referrer = null, string method = null, ContentType contentType = null, bool allowBuffering = true, long? fromBytes = null, long? toBytes = null)
        {
            var request = WebRequest.CreateHttp(url);

            if (null != method)
                request.Method = method;

            SetDefaultCookies(request);

            SetDefaultCredentials(request);

            SetContentType(request, contentType);

            SetBuffering(request, allowBuffering);

            SetRange(request, fromBytes, toBytes);

            return request;
        }
    }
}
