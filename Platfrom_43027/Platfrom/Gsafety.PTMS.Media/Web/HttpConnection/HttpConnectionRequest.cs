
using System;
using System.Collections.Generic;
using System.Net;

namespace Gsafety.PTMS.Media.Web.HttpConnection
{
    public class HttpConnectionRequest
    {
        public Uri Url { get; set; }
        public Uri Referrer { get; set; }
        public bool KeepAlive { get; set; }
        public long? RangeFrom { get; set; }
        public long? RangeTo { get; set; }
        public string Accept { get; set; }

        public Uri Proxy { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Headers { get; set; }
        public CookieContainer Cookies { get; set; }
    }
}
