using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Gsafety.PTMS.Media.Web;

namespace Gsafety.PTMS.Media.Utility
{
    public static class RetryPolicy
    {
        static HttpStatusCode[] RetryCodes = new[]
                                             {
                                                 HttpStatusCode.GatewayTimeout,
                                                 HttpStatusCode.RequestTimeout,
                                                 HttpStatusCode.InternalServerError
                                             }.OrderBy(v => v).ToArray();

        static readonly WebExceptionStatus[] WebRetryCodes = new[]
                                                             {
                                                                 WebExceptionStatus.ConnectFailure,
                                                                 WebExceptionStatus.SendFailure
                                                             }.OrderBy(v => v).ToArray();

        public static bool IsRetryable(HttpStatusCode code)
        {
            return Array.BinarySearch(RetryCodes, code) >= 0;
        }

        public static bool IsRetryable(WebExceptionStatus code)
        {
            return Array.BinarySearch(WebRetryCodes, code) >= 0;
        }

        public static bool IsWebExceptionRetryable(Exception ex)
        {
            var webException = ex as WebException;
            if (null == webException)
                return false;

            if (IsRetryable(webException.Status))
                return true;

            var httpResponse = webException.Response as HttpWebResponse;

            if (null != httpResponse)
                return IsRetryable(httpResponse.StatusCode);

            var statusCodeWebException = webException as StatusCodeWebException;

            if (null != statusCodeWebException)
                return IsRetryable(statusCodeWebException.StatusCode);

            return false;
        }

        public static void ChangeRetryableStatusCodes(IEnumerable<HttpStatusCode> addCodes, IEnumerable<HttpStatusCode> removeCodes)
        {
            // No HashSet<> in PCLs...
            var hs = new Dictionary<HttpStatusCode, bool>();

            for (; ; )
            {
                var retryCodes = RetryCodes;

                foreach (var code in retryCodes)
                    hs[code] = true;

                if (null != addCodes)
                {
                    foreach (var code in addCodes)
                        hs[code] = true;
                }

                if (null != removeCodes)
                {
                    foreach (var code in removeCodes)
                        hs.Remove(code);
                }

                var newRetryCodes = hs.Keys.OrderBy(v => v).ToArray();

                if (retryCodes == Interlocked.CompareExchange(ref RetryCodes, newRetryCodes, retryCodes))
                    return;

                hs.Clear();
            }
        }
    }
}
