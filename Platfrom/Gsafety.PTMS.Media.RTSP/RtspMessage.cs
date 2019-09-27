using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Gsafety.PTMS.Media.RTSP.Http;
using Gsafety.PTMS.Media.RTSP.Common;
using Gsafety.PTMS.Media.RTSP.Extensions;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.RTSP
{
    /// <summary>
    /// The status codes utilized in RFC2326 Messages given in response to a request
    /// </summary>
    public enum RtspStatusCode
    {
        Unknown = 0,
        // 1xx Informational.
        Continue = 100,

        // 2xx Success.
        OK = 200,
        Created = 201,
        LowOnStorageSpace = 250,

        // 3xx Redirection.
        MultipleChoices = 300,
        MovedPermanently = 301,
        Found = 302,
        SeeOther = 303,
        NotModified = 304,
        UseProxy = 305,

        // 4xx Client Error.
        BadRequest = 400,
        Unauthorized = 401,
        PaymentRequired = 402,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        NotAcceptable = 406,
        ProxyAuthenticationRequired = 407,
        RequestTimeOut = 408,
        Gone = 410,
        LengthRequired = 411,
        PreconditionFailed = 412,
        RequestMessageBodyTooLarge = 413,
        RequestUriTooLarge = 414,
        UnsupportedMediaType = 415,
        ParameterNotUnderstood = 451,
        Reserved = 452,
        NotEnoughBandwidth = 453,
        SessionNotFound = 454,
        MethodNotValidInThisState = 455,
        HeaderFieldNotValidForResource = 456,
        InvalidRange = 457,
        ParameterIsReadOnly = 458,
        AggregateOpperationNotAllowed = 459,
        OnlyAggregateOpperationAllowed = 460,
        UnsupportedTransport = 461,
        DestinationUnreachable = 462,
        DestinationProhibited = 463,
        DataTransportNotReadyYet = 464,
        NotificationReasonUnknown = 465,
        KeyManagementError = 466,

        ConnectionAuthorizationRequired = 470,
        ConnectionCredentialsNotAcception = 471,
        FaulireToEstablishSecureConnection = 472,

        // 5xx Server Error.
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeOut = 504,
        RtspVersionNotSupported = 505,
        OptionNotSupported = 551,
    }

    /// <summary>
    /// Enumeration to describe the available Rtsp Methods, used in responses
    /// </summary>
    public enum RtspMethod
    {
        UNKNOWN,
        OPTIONS,
        ANNOUNCE,
        DESCRIBE,
        REDIRECT,
        SETUP,
        GET_PARAMETER,
        SET_PARAMETER,
        PLAY,
        PLAY_NOTIFY,
        PAUSE,
        RECORD,
        TEARDOWN
    }

    /// <summary>
    /// Enumeration to indicate the type of RtspMessage
    /// </summary>
    public enum RtspMessageType
    {
        Invalid = 0,
        Request = 1,
        Response = 2,
    }

    /// <summary>
    /// Base class of RtspRequest and RtspResponse
    /// </summary>
    public class RtspMessage : HttpMessage
    {
        #region Statics

        //The scheme of Uri's of RtspMessage's
        public const string ReliableTransportScheme = "rtsp";

        public const int ReliableTransportDefaultPort = 554;

        //The scheme of Uri's of RtspMessage's which are usually being transported via udp
        public const string UnreliableTransportScheme = "rtspu";

        public const int UnreliableTransportDefaultPort = 555;

        //`Secure` RTSP...
        public const string SecureTransportScheme = "rtsps";

        public const int SecureTransportDefaultPort = 322;

        //`Secure` RTSP...
        public const string TcpTransportScheme = "rtspt";

        //The maximum amount of bytes any RtspMessage can contain.
        public const int MaximumLength = 4096;

        //String which identifies a Rtsp Request or Response
        public const string MessageIdentifier = "RTSP";

        internal protected static char[] SpaceSplit = Http.HttpMessage.SpaceSplit;

        public static byte[] ToHttpBytes(RtspMessage message, int majorVersion = 1, int minorVersion = 0, string sessionCookie = null, System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.Unused)
        {
            if (message.MessageType == RtspMessageType.Invalid) return null;

            List<byte> result = new List<byte>();
            byte[] messageBytes;

            if (message.MessageType == RtspMessageType.Request)
            {
                //Get the body of the HttpRequest
                messageBytes = message.ContentEncoding.GetBytes(System.Convert.ToBase64String(message.ToBytes()));

                //Form the HttpRequest Should allow POST and MultiPart
                result.AddRange(message.ContentEncoding.GetBytes("GET " + message.Location + " HTTP " + majorVersion.ToString() + "." + minorVersion.ToString() + CRLF));
                result.AddRange(message.ContentEncoding.GetBytes("Accept:application/x-rtsp-tunnelled" + CRLF));
                result.AddRange(message.ContentEncoding.GetBytes("Pragma:no-cache" + CRLF));
                result.AddRange(message.ContentEncoding.GetBytes("Cache-Control:no-cache" + CRLF));
                result.AddRange(message.ContentEncoding.GetBytes("Content-Length:" + messageBytes.Length + CRLF));

                if (false == string.IsNullOrWhiteSpace(sessionCookie))
                {
                    result.AddRange(message.ContentEncoding.GetBytes("x-sessioncookie: " + System.Convert.ToBase64String(message.ContentEncoding.GetBytes(sessionCookie)) + CRLF));
                }

                result.AddRange(message.ContentEncoding.GetBytes(CRLF));
                result.AddRange(message.ContentEncoding.GetBytes(CRLF));

                result.AddRange(messageBytes);
            }
            else
            {
                //Get the body of the HttpResponse
                messageBytes = message.ContentEncoding.GetBytes(System.Convert.ToBase64String(message.ToBytes()));

                //Form the HttpResponse
                result.AddRange(message.ContentEncoding.GetBytes("HTTP/" + majorVersion.ToString() + "." + minorVersion.ToString() + " " + (int)statusCode + " " + statusCode + CRLF));
                result.AddRange(message.ContentEncoding.GetBytes("Accept:application/x-rtsp-tunnelled" + CRLF));
                result.AddRange(message.ContentEncoding.GetBytes("Pragma:no-cache" + CRLF));
                result.AddRange(message.ContentEncoding.GetBytes("Cache-Control:no-cache" + CRLF));
                result.AddRange(message.ContentEncoding.GetBytes("Content-Length:" + messageBytes.Length + CRLF));
                result.AddRange(message.ContentEncoding.GetBytes("Expires:Sun, 9 Jan 1972 00:00:00 GMT" + CRLF));
                result.AddRange(message.ContentEncoding.GetBytes(CRLF));
                result.AddRange(message.ContentEncoding.GetBytes(CRLF));

                result.AddRange(messageBytes);
            }

            return result.ToArray();
        }

        public static RtspMessage FromHttpBytes(byte[] message, int offset, Encoding encoding = null, bool bodyOnly = false)
        {
            if (message == null) return null;
            if (offset > message.Length) throw new ArgumentOutOfRangeException("offset");

            //Use a default encoding if none was given
            if (encoding == null) encoding = RtspMessage.DefaultEncoding;

            //Parse the HTTP 
            string Message = encoding.GetString(message, offset, message.Length - offset);

            //Find the end of all the headers
            int headerEnd = Message.IndexOf(CRLF + CRLF);

            //Get the Http Body, It occurs after all the headers which ends with \r\n\r\n and is Base64 Encoded.
            string Body = Message.Substring(headerEnd);

            //Might want to provide the headers as an out param /.

            //Get the bytes of the underlying RtspMessage by decoding the Http Body which was encoded in base64
            byte[] rtspMessage = System.Convert.FromBase64String(Body);

            //Done
            return new RtspMessage(rtspMessage);
        }

        public static RtspMessage FromString(string data, System.Text.Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(data)) throw new InvalidOperationException("data cannot be null or whitespace.");

            if (encoding == null) encoding = RtspMessage.DefaultEncoding;

            return new RtspMessage(encoding.GetBytes(data), 0, encoding);
        }

        #endregion

        #region Fields
        int m_CSeq = -1;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="RtspMethod"/> which can be parsed from the <see cref="MethodString"/>
        /// </summary>
        public RtspMethod RtspMethod
        {
            get
            {
                RtspMethod parsed = RtspMethod.UNKNOWN;

                if (false == string.IsNullOrWhiteSpace(MethodString)) Enum.TryParse<RtspMethod>(MethodString, true, out parsed);

                return parsed;
            }
            set { MethodString = value.ToString(); }
        }

        /// <summary>
        /// Indicates the StatusCode of the RtspResponse.
        ///  A value of 200 or less usually indicates success.
        /// </summary>
        public RtspStatusCode RtspStatusCode
        {
            get { return (RtspStatusCode)m_StatusCode; }
            set
            {
                m_StatusCode = (int)value;

                if (false == CanHaveBody) m_Body = string.Empty;
            }
        }

        public override bool IsComplete
        {
            get
            {
                //Disposed is complete 
                if (IsDisposed && false == IsPersistent) return IsDisposed;

                //If the status line was not parsed
                if (false == m_StatusLineParsed &&
                    MessageType == RtspMessageType.Invalid ||  //All requests must have a StatusLine OR
                    m_Buffer != null && m_Buffer.CanRead && // Be parsing the StatusLine
                    m_Buffer.Length <= MinimumStatusLineSize) return false;

                //Messages without complete header sections are not complete
                if (false == ParseHeaders() /*&& m_CSeq == -1*/)
                    return false;

                //Don't check for any required values, only that the end of headers was seen.
                //if (MessageType == HttpMessageType.Request && CSeq == -1 || //All requests must contain a sequence number
                //    //All successful responses should also contain one
                //    MessageType == HttpMessageType.Response && StatusCode <= HttpStatusCode.OK && CSeq == -1) return false;

                //If the message can have a body
                if (CanHaveBody)
                {
                    bool hasNullBody = string.IsNullOrWhiteSpace(m_Body);
                    return false == ParseContentLength(hasNullBody) && hasNullBody && m_ContentLength > 0 ? m_HeadersParsed : ContentEncoding.GetByteCount(m_Body) >= m_ContentLength;
                }

                return true;
            }
        }

        /// <summary>
        /// Indicates if this RtspMessage is a request or a response
        /// </summary>
        public /*new*/ RtspMessageType MessageType { get { return (RtspMessageType)base.MessageType; } internal set { base.MessageType = (Http.HttpMessageType)value; } }

        /// <summary>
        /// Gets or Sets the CSeq of this RtspMessage, if found and parsed; otherwise -1.
        /// </summary>
        public int CSeq
        {
            get
            {
                //Reparse unless already parsed the headers
                ParseSequenceNumber(m_HeadersParsed);

                return m_CSeq;
            }
            set
            {
                //Use the unsigned representation
                if (m_CSeq != value) SetHeader(RtspHeaders.CSeq, ((uint)(m_CSeq = value)).ToString());
            }
        }

        #endregion

        #region Constructor

        static RtspMessage()
        {
            /*
             5004 UDP - used for delivering data packets to clients that are streaming by using RTSPU.
             5005 UDP - used for receiving packet loss information from clients and providing synchronization information to clients that are streaming by using RTSPU.
 
             See also: port 1755 - Microsoft Media Server (MMS) protocol
             */

            //Should be done in RtspMessage constructor...

            if (false == UriParser.IsKnownScheme(RtspMessage.ReliableTransportScheme))
                UriParser.Register(new HttpStyleUriParser(), RtspMessage.ReliableTransportScheme, RtspMessage.ReliableTransportDefaultPort);

            if (false == UriParser.IsKnownScheme(RtspMessage.TcpTransportScheme))
                UriParser.Register(new HttpStyleUriParser(), RtspMessage.TcpTransportScheme, RtspMessage.ReliableTransportDefaultPort);

            if (false == UriParser.IsKnownScheme(RtspMessage.UnreliableTransportScheme))
                UriParser.Register(new HttpStyleUriParser(), RtspMessage.UnreliableTransportScheme, RtspMessage.UnreliableTransportDefaultPort);

            if (false == UriParser.IsKnownScheme(RtspMessage.SecureTransportScheme))
                UriParser.Register(new HttpStyleUriParser(), RtspMessage.SecureTransportScheme, RtspMessage.SecureTransportDefaultPort);
        }

        /// <summary>
        /// Reserved
        /// </summary>
        internal protected RtspMessage() : base(RtspMessage.MessageIdentifier) { }

        /// <summary>
        /// Constructs a RtspMessage
        /// </summary>
        /// <param name="messageType">The type of message to construct</param>
        public RtspMessage(RtspMessageType messageType, double? version = 1.0, Encoding contentEncoding = null, bool shouldDispse = true)
            : base((Http.HttpMessageType)messageType, version, contentEncoding, shouldDispse, RtspMessage.MessageIdentifier)
        {

        }

        /// <summary>
        /// Creates a RtspMessage from the given bytes
        /// </summary>
        /// <param name="bytes">The byte array to create the RtspMessage from</param>
        /// <param name="offset">The offset within the bytes to start creating the message</param>
        public RtspMessage(byte[] bytes, int offset = 0, Encoding encoding = null) : this(bytes, offset, bytes.Length - offset, encoding) { }

        public RtspMessage(MemorySegment data, Encoding encoding = null) : this(data.Array, data.Offset, data.Count, encoding) { }

        /// <summary>
        /// Creates a managed representation of an abstract RtspMessage concept from RFC2326.
        /// </summary>
        /// <param name="packet">The array segment which contains the packet in whole at the offset of the segment. The Count of the segment may not contain more bytes than a RFC2326 message may contain.</param>
        /// <reference>
        /// RFC2326 - http://tools.ietf.org/html/rfc2326 - [Page 19]
        /// 4.4 Message Length
        ///When a message body is included with a message, the length of that
        ///body is determined by one of the following (in order of precedence):
        ///1.     Any response message which MUST NOT include a message body
        ///        (such as the 1xx, 204, and 304 responses) is always terminated
        ///        by the first empty line after the header fields, regardless of
        ///        the entity-header fields present in the message. (Note: An
        ///        empty line consists of only CRLF.)
        ///2.     If a Content-Length header field (section 12.14) is present,
        ///        its value in bytes represents the length of the message-body.
        ///        If this header field is not present, a value of zero is
        ///        assumed.
        ///3.     By the server closing the connection. (Closing the connection
        ///        cannot be used to indicate the end of a request body, since
        ///        that would leave no possibility for the server to send back a
        ///        response.)
        ///Note that RTSP does not (at present) support the HTTP/1.1 "chunked"
        ///transfer coding(see [H3.6]) and requires the presence of the
        ///Content-Length header field.
        ///    Given the moderate length of presentation descriptions returned,
        ///    the server should always be able to determine its length, even if
        ///    it is generated dynamically, making the chunked transfer encoding
        ///    unnecessary. Even though Content-Length must be present if there is
        ///    any entity body, the rules ensure reasonable behavior even if the
        ///    length is not given explicitly.
        /// </reference>        
        public RtspMessage(byte[] data, int offset, int length, Encoding contentEncoding = null, bool shouldDispose = true)
            : base(data, offset, length, contentEncoding, shouldDispose, RtspMessage.MessageIdentifier)
        {


        }

        /// <summary>
        /// Creates a RtspMessage by copying the properties of another.
        /// </summary>
        /// <param name="other">The other RtspMessage</param>
        public RtspMessage(RtspMessage other)
            : base(other)
        {

        }

        #endregion

        #region Methods

        public override IEnumerable<byte> PrepareBody(bool includeEmptyLine = false)
        {

            IEnumerable<byte> result = MemorySegment.EmptyBytes;

            if (IsDisposed && false == IsPersistent) return result;

            if (m_ContentLength > 0)
            {
                //foreach (byte b in ContentEncoding.GetBytes(m_Body)/*.Take(m_ContentLength)*/) yield return b;

                result = Enumerable.Concat(result, ContentEncoding.GetBytes(m_Body));

            }

            if (includeEmptyLine && m_EncodedLineEnds != null)
            {
                //foreach (byte b in m_HeaderEncoding.GetBytes(m_EncodedLineEnds)) yield return b;

                result = Enumerable.Concat(result, m_HeaderEncoding.GetBytes(m_EncodedLineEnds));
            }

            return result;
        }

        override protected internal bool ParseBody(out int remaining, bool force = false) //bool requireContentLength = true
        {
            remaining = 0;

            //If the message is disposed then the body is parsed.
            if (IsDisposed && false == IsPersistent) return false;

            //If the message is invalid or
            if (false == force && MessageType == RtspMessageType.Invalid ||
                //false == string.IsNullOrWhiteSpace(m_Body) || //or body was already started parsing
                IsComplete) return true; //or the message is complete then return true

            //If no headers could be parsed then don't parse the body
            if (false == ParseHeaders()) return false;

            //If there is no ContentLength then do not parse the body, this could be allowed to parse further...
            //requireContentLength && 
            if (m_ContentLength < 0 && false == ParseContentLength()) return false;

            //Empty body or no ContentLength
            //If the message cannot have a body it is parsed.
            if (m_ContentLength == 0 || false == CanHaveBody) return true;

            //Get the decoder to use for the body
            Encoding decoder = ParseContentEncoding(false, FallbackToDefaultEncoding);

            if (decoder == null) return false;

            int existingBodySize = decoder.GetByteCount(m_Body);

            //Calculate how much data remains based on the ContentLength
            remaining = m_ContentLength - existingBodySize;

            if (remaining == 0) return true;

            //If there was no buffer or an unreadable buffer then no parsing can occur
            if (m_Buffer == null || false == m_Buffer.CanRead) return false;

            //Quite possibly should be long
            int max = (int)m_Buffer.Length;

            int position = (int)m_Buffer.Position,
                   available = max - position;

            if (available == 0) return false;

            //Get the array of the memory stream
            byte[] buffer = m_Buffer.GetBuffer();

            //Ensure no control characters were left from parsing of the header values if more data is available then remains
            //only do this one time and only if the Body was not already started parsing.
            if (existingBodySize == 0 && Array.IndexOf<char>(m_EncodedLineEnds, decoder.GetChars(buffer, position, 1)[0]) >= 0)
            {
                ++position;

                --available;
            }

            //Get the body of the message which is the amount of bytes remaining based on the current position in parsing
            if (available > 0)
            {
                if (existingBodySize == 0)
                    m_Body = decoder.GetString(buffer, position, Binary.Min(available, remaining));
                else                     //Append to the existing body
                    m_Body += decoder.GetString(buffer, position, Binary.Min(available, remaining));

                //No longer needed, and would interfere with CompleteFrom logic.
                DisposeBuffer();

                //Body was parsed or started to be parsed.
                return true;
            }

            //The body was not parsed
            return false;
        }

        /// <summary>
        /// Completes the RtspMessage from either the buffer or the socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public override async Task<int> CompleteFrom(System.Net.Sockets.Socket socket, MemorySegment buffer)
        {
            if (IsDisposed && false == IsPersistent) return 0;

            bool hasSocket = socket != null, hasBuffer = false == buffer.IsDisposed && buffer.Count > 0;

            //If there is no socket or no data available in the buffer nothing can be done
            if (false == hasSocket && false == hasBuffer)
            {
                return 0;
            }

            //Don't check IsComplete because of the notion of how a HttpMessage can be received.
            //There may be additional headers which are available before the body          

            int received = 0;

            if (false == hasSocket)
            {
                //Create the buffer if it was null
                if (m_Buffer == null || false == m_Buffer.CanWrite)
                {
                    m_Buffer = new System.IO.MemoryStream();

                    m_HeaderOffset = 0;
                }
                else
                {
                    //Otherwise prepare to append the buffer
                    m_Buffer.Seek(0, System.IO.SeekOrigin.End);

                    //Update the length
                    m_Buffer.SetLength(m_Buffer.Length + buffer.Count);
                }

                //If there was a buffer
                if (buffer != null && false == buffer.IsDisposed && buffer.Count > 0)
                {
                    //Write the new data
                    m_Buffer.Write(buffer.Array, buffer.Offset, received += buffer.Count);

                    //Go to the beginning
                    m_Buffer.Seek(0, System.IO.SeekOrigin.Begin);
                }
            }

            //If the status line was not parsed return the number of bytes written, reparse if there are no headers parsed yet.
            if (false == ParseStatusLine(MessageType == RtspMessageType.Invalid || false == m_StatusLineParsed)) return received;
            else if (m_Buffer != null && m_Buffer.CanSeek) m_Buffer.Seek(m_HeaderOffset, System.IO.SeekOrigin.Begin); // Seek past the status line.

            //Determine if there can be and is a body already
            bool hasNullBody = CanHaveBody && string.IsNullOrWhiteSpace(m_Body);

            //Force the re-parsing of headers unless the body has started parsing.
            if (false == ParseHeaders(hasNullBody)) return received;

            //Reparse any content-length if it was not already parsed or was a 0 value and the body is still null
            if (m_ContentLength <= 0 && false == ParseContentLength(hasNullBody)) return received;

            //Http closes the connection when there is no content-length...

            //If there is a socket
            if (hasSocket)
            {
                //Use the content decoder (reparse if the body was null)
                Encoding decoder = ParseContentEncoding(hasNullBody, FallbackToDefaultEncoding);

                //Calulcate the amount of bytes in the body
                //int encodedBodyCount = decoder.GetByteCount(m_Body);

                //Determine how much remaing
                int remaining;

                //If there are remaining octetes then complete the HttpMessage
                if (false == ParseBody(out remaining, false) && remaining > 0)
                {
                    //Store the error
                    System.Net.Sockets.SocketError error = System.Net.Sockets.SocketError.SocketError;

                    //Keep track of whats received as of yet and where
                    int justReceived = 0, offset = buffer.Offset;

                    //While there is something to receive.
                    while (remaining > 0)
                    {
                        //Receive remaining more if there is a socket otherwise use the remaining data in the buffer when no socket is given.
                        justReceived = await SocketExtensions.AlignedReceive(buffer.Array, offset, remaining, socket);

                        //If anything was present then add it to the body.
                        if (justReceived > 0)
                        {
                            //Concatenate the result into the body
                            m_Body += decoder.GetString(buffer.Array, offset, Binary.Min(remaining, justReceived));

                            //Decrement for what was justReceived
                            remaining -= justReceived;

                            //Increment for what was justReceived
                            received += justReceived;
                        }

                        //If any socket error occured besides a timeout or a block then stop trying to receive.
                        if (error != System.Net.Sockets.SocketError.Success || error != System.Net.Sockets.SocketError.TimedOut || error != System.Net.Sockets.SocketError.TryAgain) break;
                    }
                }
            }
            else ParseBody(true);

            //Return the amount of bytes consumed.
            return received;
        }

        internal protected virtual bool ParseSequenceNumber(bool force = false)
        {
            //If the message is disposed then no parsing can occur
            if (IsDisposed && false == IsPersistent) return false;

            if (false == force && m_CSeq >= 0) return false;

            //See if there is a Content-Length header
            string sequenceNumber = GetHeader(RtspHeaders.CSeq);

            //If the value was null or empty then do nothing
            if (string.IsNullOrWhiteSpace(sequenceNumber)) return false;

            //If there is a header parse it's value.
            //Should use EncodingExtensions
            if (false == int.TryParse(ASCII.ExtractNumber(sequenceNumber), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out m_CSeq))
            {
                return false;
            }

            return true;
        }

        protected override void OnHeaderAdded(string headerName, string headerValue)
        {
            if (string.Compare(headerName, Http.HttpHeaders.TransferEncoding) == 0) throw new InvalidOperationException("Protocol: " + Protocol + ", does not support TrasferEncoding.");

            base.OnHeaderAdded(headerName, headerValue);
        }

        /// <summary>
        /// Called when a header is removed
        /// </summary>
        /// <param name="headerName"></param>
        protected override void OnHeaderRemoved(string headerName, string headerValue)
        {
            if (IsDisposed && false == IsPersistent) return;

            //If there is a null or empty header ignore
            if (string.IsNullOrWhiteSpace(headerName)) return;

            //The lower case invariant name and determine if action is needed
            switch (headerName.ToLowerInvariant())
            {
                case "cseq":
                    {
                        m_CSeq = -1;

                        break;
                    }
                default:
                    {
                        base.OnHeaderRemoved(headerName, headerValue);

                        break;
                    }
            }
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return Created.GetHashCode() ^ (int)((int)MessageType | (int)RtspMethod ^ (int)RtspStatusCode) ^ (string.IsNullOrWhiteSpace(m_Body) ? Length : m_Body.GetHashCode()) ^ (m_Headers.Count ^ CSeq);
        }

        public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(this, obj)) return true;

            if (false == (obj is RtspMessage)) return false;

            RtspMessage other = obj as RtspMessage;

            //Fast path doesn't show true equality.
            //other.Created != Created

            return other.MessageType == MessageType
                &&
                other.Version == Version
                &&
                other.MethodString == MethodString
                //&&
                // other.m_Headers.Count == m_Headers.Count 
                &&
                other.GetHeaders().All(ContainsHeader)
                &&
                other.m_CSeq == m_CSeq
                &&
                string.Compare(other.m_Body, m_Body) == 0;
            //&&               
            //other.Length == Length;
        }

        #endregion

        #region Operators

        public static bool operator ==(RtspMessage a, RtspMessage b)
        {
            object boxA = a, boxB = b;
            return boxA == null ? boxB == null : a.Equals(b);
        }

        public static bool operator !=(RtspMessage a, RtspMessage b) { return false == (a == b); }

        #endregion
    }
}
