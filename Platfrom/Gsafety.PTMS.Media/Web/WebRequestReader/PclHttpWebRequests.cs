using Gsafety.PTMS.Media.Common.Loggers;
using System;
using System.Net;

namespace Gsafety.PTMS.Media.Web.WebRequestReader
{
    public class PclHttpWebRequests : HttpWebRequestsBase
    {
        public PclHttpWebRequests(ICredentials credentials = null, CookieContainer cookieContainer = null)
            : base(credentials, cookieContainer)
        { }

        public override bool SetReferrer(HttpWebRequest request, Uri referrer)
        {
            try
            {
                if (null != referrer)
                    request.Headers[HttpRequestHeader.Referer] = referrer.ToString();

                return true;
            }
            catch (Exception ex)
            {
                LoggerInstance.Debug("PclHttpWebRequests.SetReferrer() unable to set referrer: " + ex.Message);

                return false;
            }
        }

        public override bool SetIfModifiedSince(HttpWebRequest request, string ifModifiedSince)
        {
            try
            {
                if (null != ifModifiedSince)
                    request.Headers[HttpRequestHeader.IfModifiedSince] = ifModifiedSince;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool SetIfNoneMatch(HttpWebRequest request, string etag)
        {
            try
            {
                if (null != etag)
                    request.Headers[HttpRequestHeader.IfNoneMatch] = etag;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool SetCacheControl(HttpWebRequest request, string cacheControl)
        {
            try
            {
                if (null != cacheControl)
                    request.Headers[HttpRequestHeader.CacheControl] = cacheControl;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected override void SetRange(HttpWebRequest request, long? fromBytes, long? toBytes)
        {
            if (fromBytes.HasValue || toBytes.HasValue)
            {
                request.Headers[HttpRequestHeader.Range] = String.Format("bytes={0}-{1}",
                    fromBytes.HasValue ? fromBytes.ToString() : String.Empty,
                    toBytes.HasValue ? toBytes.ToString() : String.Empty);
            }
        }
    }
}
