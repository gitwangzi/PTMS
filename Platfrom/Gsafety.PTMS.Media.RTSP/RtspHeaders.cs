#region Copyright
/*
This file came from Managed Media Aggregation, You can always find the latest version @ https://net7mma.codeplex.com/
  
 Julius.Friedman@gmail.com / (SR. Software Engineer ASTI Transportation Inc. http://www.asti-trans.com)

Permission is hereby granted, free of charge, 
 * to any person obtaining a copy of this software and associated documentation files (the "Software"), 
 * to deal in the Software without restriction, 
 * including without limitation the rights to :
 * use, 
 * copy, 
 * modify, 
 * merge, 
 * publish, 
 * distribute, 
 * sublicense, 
 * and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * 
 * JuliusFriedman@gmail.com should be contacted for further details.

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
 * 
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
 * TORT OR OTHERWISE, 
 * ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 * v//
 */
#endregion

#region Using Statements
using Media;
using Gsafety.PTMS.Media.RTSP.Common;
using Gsafety.PTMS.Media.RTSP.Common.Extensions.TimeSpan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace Gsafety.PTMS.Media.RTSP
{
    /// <summary>
    /// Header Definitions from RFC2326
    /// https://tools.ietf.org/html/rfc2326
    /// </summary>
    public sealed class RtspHeaders
    {
        //Already defined in HttpHeaders.
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

        //Oops
        public const string ContentLocation = "Content-Location";

        public const string ContentType = "Content-Type";
        public const string CSeq = "CSeq";
        public const string Date = "Date";
        /*
         Event-Type has the following ABNF:
         Event-Type = "Event-Type" ":" event-type
         event-type = "Session-Description"
         / "End-Of-Stream"
         / "Error"
         / extension-event-type
         extension-event-type = token 
         */
        public const string EventType = "Event-Type";
        public const string From = "From";
        public const string Expires = "Expires";
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
        public const string TerminateReason = "Terminate-Reason";
        public const string Timestamp = "Timestamp";
        public const string Transport = "Transport";
        public const string Unsupported = "Unsupported";
        public const string UserAgent = "User-Agent";
        public const string Via = "Via";
        public const string WWWAuthenticate = "WWW-Authenticate";

        private RtspHeaders() { }

        //Todo, Most parse header functions could be changed to use a ParseHeader function which gives an array of values which can then be parsed further.
        //Create header would do the reverse.

        //internal static string CreateHeader(string key, string value) => key + ':' + CreateHeaderValue(";", value);

        //internal static string CreateHeaderValue(string sep, params string[] values)
        //{
        //    return string.Join(sep, values);
        //}

        //Something like this for the reverse
        //string[] ParseHeaderValues(string delemit, string source) => source.Split(delemit);

        //string[] ParseHeaderValues(int count) => source.Split(delemit, count);

        /// <summary>
        /// Parses a RFCXXXX range string often used in SDP to describe start and end times.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static bool TryParseRange(string value, out string type, out TimeSpan start, out TimeSpan end)
        {
            return Gsafety.PTMS.Media.RTSP.Sdp.SessionDescription.TryParseRange(value, out type, out start, out end);
        }

        /// <summary>
        /// Creates a RFCXXX range string
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="type"></param>
        /// <param name="format"></param>
        /// <param name="timePart">optional time= utc-time. `;time=19970123T143720Z`</param>
        /// <returns></returns>
        public static string RangeHeader(TimeSpan? start, TimeSpan? end, string type = "npt", string timePart = null)
        {
            return type +
                ((char)ASCII.EqualsSign).ToString() +
                (start.HasValue && end.HasValue && end.Value != TimeSpanExtensions.InfiniteTimeSpan ?
                start.Value.TotalSeconds.ToString(System.Globalization.CultureInfo.InvariantCulture) : "now") +
                '-' +
                (end.HasValue && end.Value > TimeSpan.Zero ?
                end.Value.TotalSeconds.ToString(System.Globalization.CultureInfo.InvariantCulture) : string.Empty) +
                (false == string.IsNullOrWhiteSpace(timePart) ? (((char)ASCII.SemiColon).ToString() + timePart) : string.Empty);
        }
     
        public static bool TryParseTransportHeader(string value, out int ssrc,
            out System.Net.IPAddress source,
            out int serverRtpPort, out int serverRtcpPort,
            out int clientRtpPort, out int clientRtcpPort,
            out bool interleaved, out byte dataChannel, out byte controlChannel,
            out string mode, out System.Net.IPAddress destination, out int ttl)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new InvalidOperationException("value cannot be null or whitespace.");

            ssrc = 0;

            //I think ttl should have a default of 0 in this case, but it probably doesn't matter in most cases.
            ttl = sbyte.MaxValue;

            source = destination = System.Net.IPAddress.Any;
            serverRtpPort = serverRtcpPort = clientRtpPort = clientRtcpPort = 0;
            dataChannel = 0;
            controlChannel = 1;
            interleaved = true;
            mode = string.Empty;

            try
            {
                //Get the recognized parts of information from the transportHeader
                string[] parts = value.Split(SemiColon);

                for (int i = 0, e = parts.Length; i < e; ++i)
                {
                    string[] subParts = parts[i].Split((char)ASCII.EqualsSign);

                    switch (subParts[0].ToLowerInvariant())
                    {
                        case "mode":
                            {
                                mode = subParts[1];

                                continue;
                            }
                        //The header may indicate a source address different from that of the connection's address
                        case "source":
                            {
                                string sourcePart = subParts[1];

                                //Ensure not empty
                                if (string.IsNullOrWhiteSpace(sourcePart)) continue;

                                //Attempt to parse the token as an IPAddress
                                if (false == System.Net.IPAddress.TryParse(sourcePart, out source))
                                {
                                    throw new Exception("false == System.Net.IPAddress.TryParse(sourcePart, out source)");
                                }

                                continue;
                            }
                        //The header may indicate the the destination address is different from that of the connection's address
                        case "destination":
                            {
                                string destinationPart = subParts[1];

                                //Ensure not empty
                                if (string.IsNullOrWhiteSpace(destinationPart)) continue;

                                //Attempt to parse the token as an IPAddress
                                if (false == System.Net.IPAddress.TryParse(destinationPart, out destination))
                                {
                                    throw new Exception("false == System.Net.IPAddress.TryParse(sourcePart, out source)");
                                }

                                continue;
                            }
                        case "ttl":
                            {
                                if (false == int.TryParse(subParts[1].Trim(), out ttl))
                                {
                                    TaggedExceptionExtensions.RaiseTaggedException(ttl, "See Tag. Cannot Parse a ttl datum as given.");
                                }

                                //Could just clamp.
                                if (ttl < byte.MinValue || ttl > byte.MaxValue)
                                    TaggedExceptionExtensions.RaiseTaggedException(ttl, "See Tag. Invalid ttl datum as given.");

                                continue;
                            }
                        case "ssrc":
                            {
                                string ssrcPart = subParts[1].Trim();

                                if (false == int.TryParse(ssrcPart, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out ssrc) &&
                                    false == int.TryParse(ssrcPart, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out ssrc))
                                {
                                    TaggedExceptionExtensions.RaiseTaggedException(ssrcPart, "See Tag. Cannot Parse a ssrc datum as given.");
                                }

                                continue;
                            }
                        //This parameter provides the RTP/RTCP port pair for a multicastsession. It is specified as a range, e.g., port=3456-3457.
                        case "port":
                            {
                                string[] multicastPorts = subParts[1].Split(HyphenSign);

                                int multicastPortsLength = multicastPorts.Length;

                                if (multicastPortsLength > 0)
                                {
                                    clientRtpPort = serverRtpPort = int.Parse(multicastPorts[0], System.Globalization.CultureInfo.InvariantCulture);
                                    if (multicastPortsLength > 1) clientRtcpPort = serverRtcpPort = int.Parse(multicastPorts[1], System.Globalization.CultureInfo.InvariantCulture);
                                    else clientRtcpPort = clientRtpPort; //multiplexing
                                }

                                continue;

                            }
                        case "client_port":
                            {
                                string[] clientPorts = subParts[1].Split(HyphenSign);

                                int clientPortsLength = clientPorts.Length;

                                if (clientPortsLength > 0)
                                {
                                    clientRtpPort = int.Parse(clientPorts[0], System.Globalization.CultureInfo.InvariantCulture);
                                    if (clientPortsLength > 1) clientRtcpPort = int.Parse(clientPorts[1], System.Globalization.CultureInfo.InvariantCulture);
                                    else clientRtcpPort = clientRtpPort; //multiplexing
                                }

                                continue;
                            }
                        case "server_port":
                            {
                                string[] serverPorts = subParts[1].Split(HyphenSign);

                                int serverPortsLength = serverPorts.Length;

                                if (serverPortsLength > 0)
                                {
                                    serverRtpPort = int.Parse(serverPorts[0], System.Globalization.CultureInfo.InvariantCulture);
                                    if (serverPortsLength > 1) serverRtcpPort = int.Parse(serverPorts[1], System.Globalization.CultureInfo.InvariantCulture);
                                    else serverRtcpPort = serverRtpPort; //multiplexing
                                }

                                continue;
                            }
                        case "interleaved":
                            {
                                interleaved = true;

                                //Should only be for Tcp
                                string[] channels = subParts[1].Split(TimeSplit, StringSplitOptions.RemoveEmptyEntries);

                                int channelsLength = channels.Length;

                                if (channelsLength > 1)
                                {
                                    //DataChannel
                                    dataChannel = byte.Parse(channels[0], System.Globalization.CultureInfo.InvariantCulture);
                                    //Control Channel
                                    if (channelsLength > 1) controlChannel = byte.Parse(channels[1], System.Globalization.CultureInfo.InvariantCulture);
                                }

                                continue;
                            }
                        default: continue;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string TransportHeader(string connectionType, int? ssrc, //todo, reorder and add destination and port
            System.Net.IPAddress source,
            int? clientRtpPort, int? clientRtcpPort,
            int? serverRtpPort, int? serverRtcpPort,
            bool? unicast, bool? multicast,
            int? ttl,
            bool? interleaved, byte? dataChannel, byte? controlChannel,
            string mode = null) //string[] others?
        {
            if (string.IsNullOrWhiteSpace(connectionType)) throw new ArgumentNullException("connectionType");

            if (unicast.HasValue && multicast.HasValue && unicast.Value == multicast.Value) throw new InvalidOperationException("unicast and multicast cannot have the same value.");

            StringBuilder builder = null;

            try
            {
                builder = new StringBuilder();

                builder.Append(connectionType);

                if (source != null)
                {
                    builder.Append(SemiColon);

                    builder.Append("source=");
                    builder.Append(source);
                }

                if (unicast.HasValue && unicast.Value == true)
                {
                    builder.Append(SemiColon);

                    builder.Append("unicast");
                }
                else if (multicast.HasValue && multicast.Value == true)
                {
                    builder.Append(SemiColon);

                    builder.Append("multicast");
                }

                //Should eventually also allow for rtcp only but how..

                /*
                  client_port:
                  This parameter provides the unicast RTP/RTCP port pair on
                  which the client has chosen to receive media data and control
                  information.  It is specified as a range, e.g.,
                  client_port=3456-3457.

                 */

                if (clientRtpPort.HasValue)
                {
                    builder.Append(SemiColon);

                    builder.Append("client_port=");
                    builder.Append(clientRtpPort.Value);

                    if (clientRtcpPort.HasValue)
                    {
                        builder.Append(HyphenSign);
                        builder.Append(clientRtcpPort);
                    }
                } //else if

                if (serverRtpPort.HasValue)
                {
                    builder.Append(SemiColon);

                    builder.Append("server_port=");
                    builder.Append(serverRtpPort.Value);

                    if (serverRtcpPort.HasValue)
                    {
                        builder.Append(HyphenSign);
                        builder.Append(serverRtcpPort);
                    }
                } //else if

                //

                if (interleaved.HasValue && interleaved.Value == true)
                {
                    builder.Append(SemiColon);

                    builder.Append("interleaved=");

                    if (dataChannel.HasValue)
                    {
                        builder.Append(dataChannel);
                    }

                    if (controlChannel.HasValue)
                    {
                        if (dataChannel.HasValue) builder.Append(HyphenSign);

                        builder.Append(controlChannel);
                    }
                }

                if (ttl.HasValue)
                {
                    builder.Append(SemiColon);

                    builder.Append("ttl=");

                    builder.Append(ttl);
                }

                /*
                 ssrc:
                  The ssrc parameter indicates the RTP SSRC [24, Sec. 3] value
                  that should be (request) or will be (response) used by the
                  media server.
                  This parameter is only valid for unicast
                  transmission. It identifies the synchronization source to be
                  associated with the media stream.
                 */

                if (ssrc.HasValue)
                {
                    builder.Append(SemiColon);

                    builder.Append("ssrc=");

                    builder.Append(ssrc.Value.ToString("X"));
                }

                //mode datum

                if (false == string.IsNullOrWhiteSpace(mode))
                {
                    builder.Append(SemiColon);

                    builder.Append("mode=\"");

                    builder.Append(mode);

                    builder.Append((char)ASCII.DoubleQuote);
                }

                return builder.ToString();
            }
            catch { throw; }
            finally { builder = null; }
        }

        /// <summary>
        /// Parses a RFC2326 Rtp-Info header, if two sub headers are present only the values from the last header are returned.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="url"></param>
        /// <param name="seq"></param>
        /// <param name="rtpTime"></param>
        /// <returns></returns>
        ///NEEDS TO RETURN []s instead of single values out Uri[], etc.
        ///Or make en enumerator...
        public static bool TryParseRtpInfo(string value, out Uri url, out int? seq, out int? rtpTime, out int? ssrc)
        {
            url = null;

            seq = rtpTime = ssrc = null;

            if (string.IsNullOrWhiteSpace(value)) return false;

            try
            {
                string[] allParts = value.Split(Comma);

                for (int i = 0, e = allParts.Length; i < e; ++i)
                {

                    string part = allParts[i];

                    if (string.IsNullOrWhiteSpace(part)) continue;

                    foreach (var token in part.Split(SemiColon))
                    {
                        string[] subParts = token.Split(ValueSplit);

                        if (subParts.Length < 2) continue;

                        switch (subParts[0].Trim().ToLowerInvariant())
                        {
                            case "url":
                                {
                                    //UriDecode?

                                    url = new Uri(subParts[1].Trim(), UriKind.RelativeOrAbsolute);

                                    continue;
                                }
                            case "seq":
                                {
                                    int parsed;

                                    if (int.TryParse(subParts[1].Trim(), out parsed))
                                    {
                                        seq = parsed;
                                    }

                                    continue;
                                }
                            case "rtptime":
                                {

                                    int parsed;

                                    if (int.TryParse(subParts[1].Trim(), out parsed))
                                    {
                                        rtpTime = parsed;
                                    }

                                    continue;
                                }
                            case "ssrc":
                                {
                                    string ssrcPart = subParts[1].Trim();

                                    uint id;

                                    if (false == uint.TryParse(ssrcPart, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out id) &&
                                        false == uint.TryParse(ssrcPart, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out id))
                                    {
                                        TaggedExceptionExtensions.RaiseTaggedException(ssrcPart, "See Tag. Cannot Parse a ssrc datum as given.");
                                    }

                                    ssrc = (int)id;

                                    continue;
                                }
                            default: continue;
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryParseRtpInfo(string value, out string[] values)
        {
            values = null;

            if (string.IsNullOrWhiteSpace(value)) return false;

            values = value.Split(Comma);

            return true;
        }

        /// <summary>
        /// Creates a RFC2326 Rtp-Info header
        /// </summary>
        /// <param name="url"></param>
        /// <param name="seq"></param>
        /// <param name="rtpTime"></param>
        /// <returns></returns>
        public static string RtpInfoHeader(Uri url, int? seq, int? rtpTime, int? ssrc)
        {
            StringBuilder result = new StringBuilder();

            if (url != null)
            {
                result.Append("url=");

                result.Append(url.ToString());
            }

            //result.Append(string.Join(";", 
            //    seq.HasValue ? "seq=" + seq.Value : string.Empty, 
            //    rtpTime.HasValue ? "rtptime=" + rtpTime.Value : string.Empty, 
            //    ssrc.HasValue ? "ssrc=" + ssrc.Value.ToString("X") : string.Empty));

            if (seq.HasValue)
            {
                //When url is null there will be a preceeding ';'
                result.Append(SemiColon);

                result.Append("seq=");

                result.Append(seq.Value);
            }

            if (rtpTime.HasValue)
            {
                //When uri & seq are null there will be a preceeding ';'
                result.Append(SemiColon);

                result.Append("rtptime=");

                result.Append(rtpTime.Value);
            }

            if (ssrc.HasValue)
            {
                //When uri & seq & rtoTime are null there will be a preceeding ';'
                result.Append(SemiColon);

                result.Append("ssrc=");

                result.Append(ssrc.Value.ToString("X"));
            }

            return result.ToString();
        }

        public static bool TryParseScale(out double result) { throw new NotImplementedException(); }

        public static string ScaleHeader(double scale) { return scale.ToString(RtspMessage.VersionFormat, System.Globalization.CultureInfo.InvariantCulture); }

        public static bool TryParseSpeed(out double result) { throw new NotImplementedException(); }

        public static string SpeedHeader(double speed) { return speed.ToString(RtspMessage.VersionFormat, System.Globalization.CultureInfo.InvariantCulture); }

        public static bool TryParseBlockSize(out int result) { throw new NotImplementedException(); }

        public static string BlockSizeHeader(int blockSize) { return blockSize.ToString(); }

        public static string TimestampHeader(string timestamp, TimeSpan? delay, string delayFormat = null)
        {
            return delay.HasValue
                ?
                string.Join(SemiColon.ToString(), timestamp, false == string.IsNullOrWhiteSpace(delayFormat) ? delay.Value.ToString(delayFormat) : delay.Value.ToString())
                :
                timestamp;
        }

        public static bool TryParseTimestamp(string value, out string timestamp, out TimeSpan delay)
        {
            timestamp = null;

            delay = TimeSpanExtensions.InfiniteTimeSpan;

            //If there was a Timestamp header
            if (false == string.IsNullOrWhiteSpace(value))
            {
                string timestampHeader = value.Trim();

                //check for the delay token
                int indexOfDelay = timestampHeader.IndexOf("delay=");

                //if present
                if (indexOfDelay >= 0)
                {
                    double delaySeconds = double.NaN;

                    timestamp = timestampHeader.Substring(0, indexOfDelay);

                    if (double.TryParse(timestampHeader.Substring(indexOfDelay + 6).TrimEnd(), out delaySeconds))
                    {
                        //Set the value of the servers delay
                        delay = TimeSpan.FromSeconds(delaySeconds);
                    }
                }
                else
                {
                    //MS servers don't use a ; to indicate delay
                    string[] parts = timestampHeader.Split(RtspMessage.SpaceSplit);

                    int partsLength = parts.Length;

                    if (partsLength > 0)
                    {
                        timestamp = parts[0];

                        //If there was something after the space
                        if (partsLength > 1)
                        {
                            //attempt to calulcate it from the given value
                            double delaySeconds = double.NaN;

                            if (double.TryParse(parts[1].Trim(), out delaySeconds))
                            {
                                //Set the value of the servers delay
                                delay = TimeSpan.FromSeconds(delaySeconds);
                            }
                        }
                    }
                }

                return true;
            }

            return false;
        }

        //internal const string OnDemand = "On-demand";

        //internal const string DynamicOnDemand = "Dynamic On-demand";

        //internal const string Live = "Live";

        //internal const string LiveWithRecording = "Live with Recording";

        internal const string RandomAccess = "Random-Access";

        internal const string BeginningOnly = "Beginning-Only";

        internal const string NoSeeking = "No-seeking";

        internal static string MediaPropertiesHeader() { throw new NotImplementedException(); }

        internal bool TryParseMediaProperties() { throw new NotImplementedException(); }

        internal const string Unlimited = "Unlimited";

        internal const string TimeLimited = "Time-Limited";

        internal const string TimeDuration = "Time-Duration";

        internal static string Retention() { throw new NotImplementedException(); }

        internal const string Immutable = "Immutable";

        internal const string Dynamic = "Dynamic";

        internal const string TimeProgressing = "Time-Progressing";

        internal static string ContentModifcations() { throw new NotImplementedException(); }


    }
}
