using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media.Web.WebRequestReader
{
    public abstract class HttpWebRequestsBase : IHttpWebRequests
    {
        // TODO: We need to encode all these headers properly.

        static bool _canSetAllowReadStreamBuffering = true;
        readonly CookieContainer _cookieContainer;
        readonly ICredentials _credentials;

        protected HttpWebRequestsBase(ICredentials credentials, CookieContainer cookieContainer)
        {
            _credentials = credentials;
            _cookieContainer = cookieContainer;
        }

  
        public virtual HttpWebRequest CreateWebRequest(Uri url, Uri referrer = null, string method = null, ContentType contentType = null, bool allowBuffering = true, long? fromBytes = null, long? toBytes = null)
        {
            var request = WebRequest.CreateHttp(url);

            if (null != method)
                request.Method = method;

            SetDefaultCookies(request);

            SetDefaultCredentials(request);

            SetReferrer(request, referrer);

            SetContentType(request, contentType);

            SetBuffering(request, allowBuffering);

            SetRange(request, fromBytes, toBytes);

            return request;
        }

        public abstract bool SetIfModifiedSince(HttpWebRequest request, string ifModifiedSince);
        public abstract bool SetIfNoneMatch(HttpWebRequest request, string etag);
        public abstract bool SetCacheControl(HttpWebRequest request, string cacheControl);

        protected abstract void SetRange(HttpWebRequest request, long? fromBytes, long? toBytes);

        protected virtual void SetBuffering(HttpWebRequest request, bool allowBuffering)
        {
            if (_canSetAllowReadStreamBuffering && request.AllowReadStreamBuffering != allowBuffering)
            {
                try
                {
                    request.AllowReadStreamBuffering = allowBuffering;
                }
                catch (InvalidOperationException ex)
                {
                    LoggerInstance.Debug("HttpWebRequestsBase.SetBuffering() unable to set AllowReadStreamBuffering to {0}: {1}", allowBuffering, ex.Message);
                    _canSetAllowReadStreamBuffering = false;
                }
            }
        }

        protected virtual void SetContentType(HttpWebRequest request, ContentType contentType)
        {
            if (null != contentType)
            {
                if (null != contentType.AlternateMimeTypes && contentType.AlternateMimeTypes.Count > 0)
                    request.Accept = string.Join(", ", new[] { contentType.MimeType }.Concat(contentType.AlternateMimeTypes));
                else
                    request.Accept = contentType.MimeType;
            }
        }

        public abstract bool SetReferrer(HttpWebRequest request, Uri referrer);

        protected virtual bool SetCredentials(HttpWebRequest request, ICredentials credentials)
        {
            request.Credentials = credentials;

            return true;
        }

        protected virtual bool SetDefaultCredentials(HttpWebRequest request)
        {
            return SetCredentials(request, _credentials);
        }

        protected virtual bool SetCookies(HttpWebRequest request, CookieContainer cookieContainer)
        {
            if (null != cookieContainer && request.SupportsCookieContainer)
            {
                request.CookieContainer = cookieContainer;
                return true;
            }

            return false;
        }

        protected virtual bool SetDefaultCookies(HttpWebRequest request)
        {
            return SetCookies(request, _cookieContainer);
        }
    }
}
