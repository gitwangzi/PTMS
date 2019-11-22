using Media;
using Gsafety.PTMS.Media.RTSP.Common;
using Gsafety.PTMS.Media.RTSP.Common.Collections.Generic;
using Gsafety.PTMS.Media.RTSP.Common.Extensions.TimeSpan;
using System;
using System.Text;
namespace Gsafety.PTMS.Media.RTSP.Http
{
    /// <summary>
    /// Header Definitions from RFC7231
    /// https://tools.ietf.org/html/RFC7231
    /// </summary>
    public sealed class HttpHeaders
    {
        internal const char HyphenSign = (char)ASCII.HyphenSign, SemiColon = (char)ASCII.SemiColon, Comma = (char)ASCII.Comma;

        internal static string[] TimeSplit = new string[] { HyphenSign.ToString(), SemiColon.ToString() };

        internal static char[] SpaceSplit = new char[] { (char)ASCII.Space, Comma };

        internal static char[] ValueSplit = new char[] { (char)ASCII.EqualsSign, SemiColon };

        public const string Allow = "Allow";
        public const string Accept = "Accept";
        public const string AcceptCredentials = "Accept-Credentials";
        public const string AcceptEncoding = "Accept-Encoding";
        public const string AcceptLanguage = "Accept-Language";
        public const string AcceptRanges = "Accept-Ranges";
        public const string Authorization = "Authorization";
        public const string AuthenticationInfo = "Authentication-Info";
        public const string Bandwidth = "Bandwidth";
        public const string Blocksize = "Blocksize";
        public const string CacheControl = "Cache-Control";
        public const string Conference = "Conference";
        public const string Connection = "Connection";
        public const string ConnectionCredentials = "Connection-Credentials";
        public const string ContentBase = "Content-Base";
        public const string ContentEncoding = "Content-Encoding";
        public const string ContentLanguage = "Content-Language";
        public const string ContentLength = "Content-Length";
        public const string ContentLocation = "Content-Location";
        public const string ContentDisposition = "Content-Disposition";
        public const string ContentType = "Content-Type";
        public const string Date = "Date";
        public const string Expires = "Expires";
        public const string From = "From";
        public const string Host = "Host";
        public const string LastModified = "Last-Modified";
        public const string IfMatch = "If-Match";
        public const string IfModifiedSince = "If-Modified-Since";
        public const string IfNoneMatch = "If-None-Match";
        public const string Location = "Location";
        public const string MTag = "MTag";
        public const string MediaProperties = "Media-Properties";
        public const string MediaRange = "Media-Range";
        public const string PipelinedRequests = "Pipelined-Requests";
        public const string ProxyAuthenticate = "Proxy-Authenticate";
        public const string ProxyAuthenticationInfo = "Proxy-Authentication-Info";
        public const string ProxyAuthorization = "Proxy-Authorization";
        public const string ProxyRequire = "Proxy-Require";
        public const string ProxySupported = "Proxy-Supported";
        public const string Public = "Public";
        //Private / Prividgled?
        public const string Range = "Range";
        public const string Referrer = "Referrer";
        public const string Require = "Require";
        public const string RequestStatus = " Request-Status";
        public const string RetryAfter = "Retry-After";
        public const string RtpInfo = "RTP-Info";
        public const string Scale = "Scale";
        public const string Session = "Session";
        public const string Server = "Server";
        public const string Speed = "Speed";
        public const string Supported = "Supported";
        
        //https://www.ietf.org/rfc/rfc2616.txt @ 14.39 TE
        public const string TE = "TE"; //Extension Transfer Encodings..

        public const string TerminateReason = "Terminate-Reason";
        public const string Transport = "Transport";
        public const string Trailer = "Trailer";
        public const string TransferEncoding = "Transfer-Encoding";
        public const string Unsupported = "Unsupported";
        public const string UserAgent = "User-Agent";
        public const string Via = "Via";
        public const string WWWAuthenticate = "WWW-Authenticate";

        HttpHeaders() { }
    }
}
