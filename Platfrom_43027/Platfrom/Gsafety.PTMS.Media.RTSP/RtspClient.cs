using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.Media.RTSP.Common;
using System.Net;
using System.Net.Sockets;
using Gsafety.PTMS.Media.RTSP.RTCP;
using Gsafety.PTMS.Media.RTSP.RTP;
using Gsafety.PTMS.Media.RTSP.Sdp;
using System.Threading;
using System.Text;
using Gsafety.PTMS.Media.RTSP.Extensions;
using Gsafety.PTMS.Media.RTSP.Common.Extensions.Exception;
using Gsafety.PTMS.Media.RTSP.Common.Extensions.TimeSpan;
using Gsafety.PTMS.Media.RTSP.Common.Extensions;
using Gsafety.PTMS.Media.RTSP.Common.Extensions.IPAddress;
using System.Threading.Tasks;
using Gsafety.PTMS.Media.RTSP.Sdp.Lines;
using System.Diagnostics;
using System.IO;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media.RTSP
{
    /// <summary>
    /// Implements RFC 2326
    /// http://www.ietf.org/rfc/rfc2326.txt
    /// Provides facilities for communication with an RtspServer to establish one or more Rtp Transport Channels.
    /// </summary>
    public class RtspClient : BaseDisposable, ISocketReference
    {
        /// <summary>
        /// Handle the configuration required for the given socket
        /// </summary>
        /// <param name="socket"></param>
        internal static void ConfigureRtspSocket(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException("Socket");

            //Don't buffer send.
            ExceptionExtensions.ResumeOnError(() => socket.SendBufferSize = 0);

            //Don't buffer receive.
            ExceptionExtensions.ResumeOnError(() => socket.ReceiveBufferSize = 0);
        }

        public const int DefaultBufferSize = RtspMessage.MaximumLength * 2;

        public const double DefaultProtocolVersion = 1.0;

        public static readonly TimeSpan DefaultConnectionTime = TimeSpan.FromMilliseconds(500);

        public static readonly TimeSpan DefaultSessionTimeout = TimeSpan.FromSeconds(60);

        public enum ClientProtocolType
        {
            Tcp = ProtocolType.Tcp,
            Reliable = Tcp,
            Udp = ProtocolType.Udp,
            Unreliable = Udp,
            Http = 2,
            Secure = 4
        }

        #region Fields

        #region Internal Read Only

        internal readonly Guid InternalId = Guid.NewGuid();

        internal readonly ManualResetEvent m_InterleaveEvent;

        internal readonly List<MediaDescription> m_Playing = new List<MediaDescription>(); //Each entry should have it's own start time? Created property?

        //Really needs to be Connection or session will also need to refer to a connection
        internal readonly Dictionary<string, RtspSession> m_Sessions = new Dictionary<string, RtspSession>();

        #endregion

        #region Private

        ClientProtocolType m_RtspProtocol;

        RtspMessage m_LastTransmitted;

        /// <summary>
        /// The current location the media
        /// </summary>
        Uri m_InitialLocation, m_PreviousLocation, m_CurrentLocation;

        /// <summary>
        /// The buffer this client uses for all requests 4MB * 2
        /// </summary>
        MemorySegment m_Buffer;

        /// <summary>
        /// The remote IPAddress to which the Location resolves via Dns
        /// </summary>
        IPAddress m_RemoteIP;

        /// <summary>
        /// The remote RtspEndPoint
        /// </summary>
        EndPoint m_RemoteRtsp;

        /// <summary>
        /// The socket used for Rtsp Communication
        /// </summary>
        Socket m_RtspSocket;

        /// <summary>
        /// The protcol in which Rtsp data will be transpored from the server
        /// </summary>
        ProtocolType m_RtpProtocol;

        /// <summary>
        /// The session description associated with the media at Location
        /// </summary>
        SessionDescription m_SessionDescription;

        /// <summary>
        /// Keep track of timed values.
        /// </summary>
        TimeSpan m_RtspSessionTimeout = DefaultSessionTimeout,
            m_ConnectionTime = TimeSpanExtensions.InfiniteTimeSpan,
            m_LastServerDelay = TimeSpanExtensions.InfiniteTimeSpan,
            m_LastMessageRoundTripTime = DefaultConnectionTime;

        /// <summary>
        /// Keep track of certain values.
        /// </summary>
        int m_SentBytes, m_ReceivedBytes,
             m_RtspPort,
             m_CSeq, m_RCSeq, //-1 values, rtsp 2. indicates to start at 0...
             m_SentMessages, m_ReTransmits,
             m_ReceivedMessages,
             m_PushedMessages,
             m_MaximumTransactionAttempts = (int)TimeSpanExtensions.MicrosecondsPerMillisecond,//10
             m_SocketPollMicroseconds;

        //Todo, Two timers? should use a single thread instead....
        Timer m_KeepAliveTimer, m_ProtocolMonitor;

        DateTime? m_BeginConnect, m_EndConnect, m_StartedPlaying;

        #endregion

        #region Internal Private

        internal string m_UserAgent = "Siverlight RTSP Client", m_SessionId = string.Empty;//, m_TransportMode;

        internal RtpClient m_RtpClient;

        #endregion

        #region Public

        /// <summary>
        /// As given by the OPTIONS response or set otherwise.
        /// </summary>
        public readonly HashSet<string> SupportedFeatures = new HashSet<string>();

        /// <summary>
        /// Values which will be set in the Required tag.
        /// </summary>
        public readonly HashSet<string> RequiredFeatures = new HashSet<string>();

        /// <summary>
        /// Any additional headers which may be required by the RtspClient.
        /// </summary>
        public readonly Dictionary<string, string> AdditionalHeaders = new Dictionary<string, string>();

        /// <summary>
        /// Gets the methods supported by the server recieved in the options request.
        /// </summary>
        public readonly HashSet<string> SupportedMethods = new HashSet<string>();

        #endregion

        #endregion

        #region Properties

        #region Automatically Implemented

        /// <summary>
        /// Gets or sets a value indicating of the RtspSocket should be left open when Disposing.
        /// </summary>
        public bool LeaveOpen { get; set; }

        /// <summary>
        /// The version of Rtsp the client will utilize in messages
        /// </summary>
        public double ProtocolVersion { get; set; }

        /// <summary>
        /// Indicates if the <see cref="StartedPlaying"/> property will be set as a result of handling the Play event.
        /// </summary>
        public bool HandlePlayEvent { get; set; }

        /// <summary>
        /// Indicates if the <see cref="StartedPlaying"/> will not have a value as a result of handling the Stop event.
        /// </summary>
        public bool HandleStopEvent { get; set; }

        /// <summary>
        /// Allows the order of media to be determined when <see cref="StartPlaying"/>  is called
        /// </summary>
        public Action<IEnumerable<MediaDescription>> SetupOrder { get; set; }

        /// <summary>
        /// Gets or Sets the method which is called when the <see cref="RtspSocket"/> is created, 
        /// typically during the call to <see cref="Connect"/>
        /// By default <see cref="ConfigureRtspSocket"/> is utilized.
        /// </summary>
        public Action<Socket> ConfigureSocket { get; set; }

        /// <summary>
        /// Indicates if the client will try to automatically reconnect during send or receive operations.
        /// </summary>
        public bool AutomaticallyReconnect { get; set; }

        /// <summary>
        /// Indicates if the client will automatically disconnect the RtspSocket after StartPlaying is called.
        /// </summary>
        public bool AutomaticallyDisconnect { get; set; }

        /// <summary>
        /// Indicates if the client will send a <see cref="KeepAliveRequest"/> during <see cref="StartPlaying"/> if no data is flowing immediately after the PLAY response is recieved.
        /// </summary>
        public bool SendKeepAliveImmediatelyAfterStartPlaying { get; set; }

        /// <summary>
        /// Indicates if the client will add the Timestamp header to outgoing requests.
        /// </summary>
        public bool TimestampRequests { get; set; }

        /// <summary>
        /// Indicates if the client will use the Timestamp header to incoming responses.
        /// </summary>
        public bool CalculateServerDelay { get; set; }

        /// <summary>
        /// Indicates if the client will send the Blocksize header during the SETUP request.
        /// The value of which will reflect the <see cref="Buffer.Count"/>
        /// </summary>
        public bool SendBlocksize { get; set; }

        /// <summary>
        /// Indicates if the Date header should be sent during requests.
        /// </summary>
        public bool DateRequests { get; set; }

        /// <summary>
        /// Indicates if the RtspClient will send the UserAgent header.
        /// </summary>
        public bool SendUserAgent { get; set; }

        //Maybe AllowHostChange
        //public bool IgnoreRedirectOrFound { get; set; }

        /// <summary>
        /// Indicates if the client will take any `X-` headers and use them in future requests.
        /// </summary>
        public bool EchoXHeaders { get; set; }

        /// <summary>
        /// Indicates if the client will process messages which are pushed during the session.
        /// </summary>
        public bool IgnoreServerSentMessages { get; set; }

        /// <summary>
        /// Indicates if Keep Alive Requests will be sent
        /// </summary>
        public bool DisableKeepAliveRequest { get; set; }

        /// <summary>
        /// Gets or Sets a value which indicates if the client will attempt an alternate style of connection if one cannot be established successfully.
        /// Usually only useful under UDP when NAT prevents RTP packets from reaching a client, it will then attempt TCP or HTTP transport.
        /// </summary>
        public bool AllowAlternateTransport { get; set; }

        #endregion

        #region Computed Properties

        /// <summary>
        /// Gets or sets the maximum amount of microseconds the <see cref="RtspSocket"/> will wait before performing an operations.
        /// </summary>
        public int SocketPollMicroseconds
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_SocketPollMicroseconds; }
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            set { m_SocketPollMicroseconds = value; }
        }

        /// <summary>
        /// Gets the remote <see cref="EndPoint"/>
        /// </summary>
        public EndPoint RemoteEndpoint
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_RemoteRtsp; }
        }

        /// <summary>
        /// Indicates if the RtspClient is currently sending or receiving data.
        /// </summary>
        public bool InUse
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get
            {
                return false == m_InterleaveEvent.WaitOne(0) && m_InterleaveEvent.WaitOne(ConnectionTime) == false; //m_InterleaveEvent.Wait(1);
            }
        }

        /// <summary>
        /// Gets or Sets the socket used for communication
        /// </summary>
        internal protected Socket RtspSocket
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_RtspSocket; }
            set
            {
                m_RtspSocket = value;

                //Ensure not connected if the socket is removed
                if (m_RtspSocket == null)
                {
                    m_BeginConnect = m_EndConnect = null;
                    m_ConnectionTime = TimeSpanExtensions.InfiniteTimeSpan;
                    return;
                }

                if (m_RtspSocket.Connected)
                {
                    m_BeginConnect = m_EndConnect = DateTime.UtcNow;
                    m_ConnectionTime = TimeSpan.Zero;
                    m_RemoteRtsp = m_RtspSocket.RemoteEndPoint;

                    if (m_RemoteRtsp is IPEndPoint)
                    {
                        IPEndPoint remote = (IPEndPoint)m_RemoteRtsp;
                        m_RemoteIP = remote.Address;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or Sets the buffer used for data reception
        /// </summary>
        internal protected MemorySegment Buffer
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_Buffer; }
            set { m_Buffer = value; }
        }

        /// <summary>
        /// Indicates if the RtspClient shares the <see cref="RtspSocket"/> with the underlying Transport.
        /// </summary>
        public bool SharesSocket
        {
            get
            {
                //The socket is shared with the GC
                if (IsDisposed) return true;

                // A null or disposed client or one which is no longer connected cannot share the socket
                if (IDisposedExtensions.IsNullOrDisposed(m_RtpClient) || false == m_RtpClient.IsActive) return false;

                //The socket is shared if there is a context using the same socket
                var context = m_RtpClient.GetContextBySocket(m_RtspSocket);

                return false == IDisposedExtensions.IsNullOrDisposed(context) && context.IsActive && context.HasAnyRecentActivity;
            }
        }

        /// <summary>
        /// Indicates the amount of messages which were transmitted more then one time.
        /// </summary>
        public int RetransmittedMessages
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_ReTransmits; }
        }

        /// <summary>
        /// The amount of <see cref="RtspMessage"/>'s sent by this instance.
        /// </summary>
        public int MessagesSent
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_SentMessages; }
        }

        /// <summary>
        /// The amount of <see cref="RtspMessage"/>'s receieved by this instance.
        /// </summary>
        public int MessagesReceived
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_ReceivedMessages; }
        }

        /// <summary>
        /// The amount of messages pushed by the remote party
        /// </summary>
        public int MessagesPushed
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_PushedMessages; }
        }

        /// <summary>
        /// The amount of time taken to connect to the remote party.
        /// </summary>
        public TimeSpan ConnectionTime
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_ConnectionTime; }
        }

        /// <summary>
        /// The amount of time taken since the response was received to the last <see cref="RtspMessage"/> sent.
        /// </summary>
        public TimeSpan LastMessageRoundTripTime
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_LastMessageRoundTripTime; }
        }

        /// <summary>
        /// If indicated by the remote party the value of the 'delay' header from the Timestamp header.
        /// </summary>
        public TimeSpan LastServerDelay
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_LastServerDelay; }
        }

        /// <summary>
        /// Indicates if the client has been assigned a <see cref="SessionId"/>
        /// </summary>
        public bool HasSession
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_Sessions.Count() > 0; }
        }

        /// <summary>
        /// Gets the value of the Session header as it was seen in a response.
        /// When set will override any existing Session header previously seen.
        /// </summary>
        public string SessionId
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_SessionId; }
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            set { m_SessionId = value; }
        }

        /// <summary>
        /// Any SessionId's received in a response.
        /// </summary>
        public IEnumerable<string> SessionIds
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_Sessions.Keys; }
        }

        /// <summary>
        /// If playing, the TimeSpan which represents the time this media started playing from.
        /// </summary>
        public TimeSpan? StartTime
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get
            {
                return IDisposedExtensions.IsNullOrDisposed(Client) ? null : (TimeSpan?)Client.TransportContexts.Max(tc => tc.MediaStartTime);
            }
        }

        /// <summary>
        /// If playing, the TimeSpan which represents the time the media will end.
        /// </summary>
        public TimeSpan? EndTime
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get
            {
                return IDisposedExtensions.IsNullOrDisposed(Client) ? null : (TimeSpan?)Client.TransportContexts.Max(tc => tc.MediaEndTime);
            }
        }

        /// <summary>
        /// If playing, indicates if the RtspClient is playing from a live source which means there is no absolute start or end time and seeking may not be supported.
        /// </summary>
        public bool LivePlay
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return EndTime == TimeSpanExtensions.InfiniteTimeSpan; }
        }

        /// <summary>
        /// Indicates if there is any media being played by the RtspClient at the current time.
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                //If started playing
                if (m_Playing.Count > 0 && m_StartedPlaying.HasValue)
                {
                    //Try to determine playing status from the transport
                    try
                    {
                        //If not playing anymore do nothing
                        if (EndTime != TimeSpanExtensions.InfiniteTimeSpan &&
                            DateTime.UtcNow - m_StartedPlaying.Value > EndTime)
                        {
                            return false;
                        }

                        //return true;

                        //If the media is playing the RtspClient is only playing if the socket is shared or the Transport is connected.
                        return SharesSocket || m_RtpClient.IsActive;
                    }
                    catch (Exception ex)
                    {
                        LoggerInstance.Exception(ToString() + "@IsPlaying - " + ex.ToString());
                    }
                }

                //The RtspClient is not playing
                return false;
            }
        }

        /// <summary>
        /// The DateTime in which the client started playing if playing, otherwise null.
        /// </summary>
        public DateTime? StartedPlaying
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_StartedPlaying; }
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            internal protected set
            {
                m_StartedPlaying = value;
            }
        }

        /// <summary>
        /// The amount of time in seconds the KeepAlive request will be sent to the server after connected.
        /// If a GET_PARAMETER request is not supports OPTIONS will be sent instead.
        /// </summary>
        public TimeSpan RtspSessionTimeout
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_RtspSessionTimeout; }
            set
            {
                m_RtspSessionTimeout = value;

                if (m_RtspSessionTimeout <= TimeSpan.Zero)
                {
                    //Don't send a request to keep the connection alive
                    DisableKeepAliveRequest = true;

                    if (m_KeepAliveTimer != null) m_KeepAliveTimer.Dispose();

                    m_KeepAliveTimer = null;
                }

                //This is probably wrong, the time should be relative to all requests and not just the last...
                if (m_KeepAliveTimer != null) m_KeepAliveTimer.Change(m_LastTransmitted != null && m_LastTransmitted.Transferred.HasValue ? (m_RtspSessionTimeout - (DateTime.UtcNow - m_LastTransmitted.Created)) : m_RtspSessionTimeout, TimeSpanExtensions.InfiniteTimeSpan);
            }
        }

        /// <summary>
        /// Gets or Sets amount the fraction of time the client will wait during a responses for a response without blocking.
        /// If less than or equal to 0 the value 1 will be used.
        /// </summary>
        public int ResponseTimeoutInterval
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_MaximumTransactionAttempts; }
            set { m_MaximumTransactionAttempts = Binary.Clamp(value, 1, int.MaxValue); }
        }

        //The last RtspMessage received by the RtspClient from the remote EndPoint.
        public RtspMessage LastTransmitted
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_LastTransmitted; }
        }

        /// <summary>
        /// The ClientProtocolType the RtspClient is using Reliable (Tcp), Unreliable(Udp) or Http(Tcp)
        /// </summary>
        public ClientProtocolType RtspProtocol
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_RtspProtocol; }
        }

        /// <summary>
        /// The ProtocolType the RtspClient will setup for underlying RtpClient.
        /// </summary>
        public ProtocolType RtpProtocol
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_RtpProtocol; }
        }

        /// <summary>
        /// Gets or sets the current location to the Media on the Rtsp Server and updates Remote information and ClientProtocol if required by the change.
        /// If the RtspClient was listening then it will be stopped and started again
        /// </summary>
        public Uri CurrentLocation
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_CurrentLocation; }
            set
            {
                try
                {
                    //If Different
                    if (m_CurrentLocation != value)
                    {

                        if (m_InitialLocation == null) m_InitialLocation = value;

                        //Backup the current location, (needs history list?)
                        m_PreviousLocation = m_CurrentLocation;

                        bool wasPlaying = IsPlaying;

                        if (wasPlaying) StopPlaying();

                        m_CurrentLocation = value;

                        m_RemoteIP = IPAddress.Parse(m_CurrentLocation.DnsSafeHost);

                        m_RtspPort = m_CurrentLocation.Port;

                        //Validate ports, should throw? should also use default port for scheme
                        if (m_RtspPort <= ushort.MinValue || m_RtspPort > ushort.MaxValue) m_RtspPort = RtspMessage.ReliableTransportDefaultPort;

                        //Determine protocol
                        if (m_CurrentLocation.Scheme == RtspMessage.ReliableTransportScheme) m_RtspProtocol = ClientProtocolType.Tcp;
                        else if (m_CurrentLocation.Scheme == RtspMessage.UnreliableTransportScheme) m_RtspProtocol = ClientProtocolType.Udp;
                        else m_RtspProtocol = ClientProtocolType.Http;

                        //Make a IPEndPoint 
                        m_RemoteRtsp = new IPEndPoint(m_RemoteIP, m_RtspPort);

                        //Should take into account current time with StartTime?
                        if (wasPlaying) StartPlaying().ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    TaggedExceptionExtensions.RaiseTaggedException(this, "Could not resolve host from the given location. See InnerException.", ex);

                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the Uri which was used first with this instance.
        /// </summary>
        public Uri InitialLocation
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_InitialLocation; }
        }

        /// <summary>
        /// Gets the Uri which was used directly before the <see cref="CurrentLocation"/> with this instance.
        /// </summary>
        public Uri PreviousLocation
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_PreviousLocation; }
        }

        /// <summary>
        /// Indicates if the RtspClient is connected to the remote host
        /// </summary>
        /// <notes>May want to do a partial receive for 1 byte which would take longer but indicate if truly connected. Udp may not be Connected.</notes>
        public bool IsConnected
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return false == IsDisposed && m_ConnectionTime >= TimeSpan.Zero && m_RtspSocket != null /*&& m_RtspSocket.Connected*/; }
        }

        /// <summary>
        /// The amount of bytes sent by the RtspClient
        /// </summary>
        public int BytesSent
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_SentBytes; }
        }

        /// <summary>
        /// The amount of bytes recieved by the RtspClient
        /// </summary>
        public int BytesRecieved
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_ReceivedBytes; }
        }

        /// <summary>
        /// The current SequenceNumber of the RtspClient
        /// </summary>
        public int ClientSequenceNumber
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_CSeq; }
        }

        /// <summary>
        /// The current SequenceNumber of the remote RTSP party
        /// </summary>
        public int RemoteSequenceNumber
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_RCSeq; }
        }

        /// <summary>
        /// Gets the <see cref="MediaDescription"/>'s which pertain to media which is currently playing.
        /// </summary>
        public IEnumerable<MediaDescription> PlayingMedia
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_Playing.AsEnumerable(); }
        }

        /// <summary>
        /// Gets or Sets the <see cref="SessionDescription"/> describing the media at <see cref="CurrentLocation"/>.
        /// </summary>
        public SessionDescription SessionDescription
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_SessionDescription; }
            set
            {
                //if (value == null) throw new ArgumentNullException("The SessionDescription cannot be null.");
                m_SessionDescription = value;
            }
        }

        /// <summary>
        /// The RtpClient associated with this RtspClient
        /// </summary>
        public RtpClient Client
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_RtpClient; }
        }

        /// <summary>
        /// The UserAgent sent with every RtspRequest
        /// </summary>
        public string UserAgent
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_UserAgent; }
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            set { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException("UserAgent cannot consist of only null or whitespace."); m_UserAgent = value; }
        }

        #endregion

        #endregion

        #region Constructor / Destructor

        /// <summary>
        /// Creates a RtspClient on a non standard Rtsp Port
        /// </summary>
        /// <param name="location">The absolute location of the media</param>
        /// <param name="rtspPort">The port to the RtspServer is listening on</param>
        /// <param name="rtpProtocolType">The type of protocol the underlying RtpClient will utilize and will not deviate from the protocol is no data is received, if null it will be determined from the location Scheme</param>
        /// <param name="existing">An existing Socket</param>
        /// <param name="leaveOpen"><see cref="LeaveOpen"/></param>
        public RtspClient(Uri location, ClientProtocolType? rtpProtocolType = null, int bufferSize = DefaultBufferSize, Socket existing = null, bool leaveOpen = false, int maximumTransactionAttempts = (int)TimeSpanExtensions.MicrosecondsPerMillisecond, bool shouldDispose = true)
            : base(shouldDispose)
        {
            if (location == null) throw new ArgumentNullException("location");

            if (false == location.IsAbsoluteUri)
            {
                throw new Exception("false == location.IsAbsoluteUri");
            }

            //Check the Scheme
            if (false == location.Scheme.StartsWith(RtspMessage.MessageIdentifier, StringComparison.CurrentCultureIgnoreCase)
                &&
               false == location.Scheme.StartsWith(System.Uri.UriSchemeHttp, StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentException("Uri Scheme must start with rtsp or http", "location");

            //Set the location and determines the m_RtspProtocol and IP Protocol.
            CurrentLocation = location;

            //If the client has specified a Protcol to use then use it
            if (rtpProtocolType.HasValue)
            {
                //Determine if this means anything for Rtp Transport and set the field
                if (rtpProtocolType.Value == ClientProtocolType.Tcp || rtpProtocolType.Value == ClientProtocolType.Http)
                {
                    m_RtpProtocol = ProtocolType.Tcp;
                }
                else if (rtpProtocolType.Value == ClientProtocolType.Udp)
                {
                    m_RtpProtocol = ProtocolType.Udp;
                }
                else throw new ArgumentException("Must be Tcp or Udp.", "protocolType");
            }

            //If there is an existing socket
            if (existing != null)
            {
                RtspSocket = existing;
            }

            //Cases of anything less than or equal to 0 mean use the existing ReceiveBufferSize if possible.
            if (bufferSize <= 0) bufferSize = m_RtspSocket != null ? m_RtspSocket.ReceiveBufferSize : 0;

            //Create the segment given the amount of memory required if possible
            if (bufferSize > 0) m_Buffer = new MemorySegment(bufferSize);
            else m_Buffer = new MemorySegment(DefaultBufferSize); //Use 8192 bytes

            //If leave open is set the socket will not be disposed.
            LeaveOpen = leaveOpen;

            //Set the protocol version to use in requests.
            ProtocolVersion = DefaultProtocolVersion;

            m_RtpClient = new RtpClient(m_Buffer);
            m_RtpClient.InterleavedData += ProcessInterleaveData;

            ConfigureSocket = ConfigureRtspSocket;

            HandlePlayEvent = HandleStopEvent = true;

            m_MaximumTransactionAttempts = maximumTransactionAttempts;

            m_InterleaveEvent = new ManualResetEvent(true);
        }

        ~RtspClient()
        {
            Dispose();
        }

        #endregion

        #region Events

        public delegate void RtspClientAction(RtspClient sender, object args);

        public delegate void RequestHandler(RtspClient sender, RtspMessage request);

        public delegate void ResponseHandler(RtspClient sender, RtspMessage request, RtspMessage response);

        public event RtspClientAction OnConnect;

        internal protected void OnConnected()
        {
            if (IsDisposed) return;

            RtspClientAction action = OnConnect;

            if (action == null) return;

            foreach (RtspClientAction handler in action.GetInvocationList())
            {
                try { handler(this, EventArgs.Empty); }
                catch { continue; }
            }

        }

        public event RequestHandler OnRequest;

        internal protected void Requested(RtspMessage request)
        {
            if (IsDisposed) return;

            RequestHandler action = OnRequest;

            if (action == null) return;

            foreach (RequestHandler handler in action.GetInvocationList())
            {
                try { handler(this, request); }
                catch { continue; }
            }
        }

        public event ResponseHandler OnResponse;

        internal protected void Received(RtspMessage request, RtspMessage response)
        {
            if (IsDisposed) return;

            ResponseHandler action = OnResponse;

            if (action == null) return;

            foreach (ResponseHandler handler in action.GetInvocationList())
            {
                try { handler(this, request, response); }
                catch { continue; }
            }
        }

        public event RtspClientAction OnDisconnect;

        internal void OnDisconnected()
        {
            if (IsDisposed) return;

            RtspClientAction action = OnDisconnect;

            if (action == null) return;

            foreach (RtspClientAction handler in action.GetInvocationList())
            {
                try { handler(this, EventArgs.Empty); }
                catch { continue; }
            }
        }

        public event RtspClientAction OnPlay;

        internal protected void OnPlaying(MediaDescription mediaDescription = null)
        {
            if (IsDisposed) return;

            //Is was not already playing then set the value
            if (HandlePlayEvent && false == m_StartedPlaying.HasValue)
            {
                //Set started playing
                m_StartedPlaying = DateTime.UtcNow;

                //Active the RtpClient
                //if (false == IDisposedExtensions.IsNullOrDisposed(m_RtpClient)) m_RtpClient.Activate();
            }

            RtspClientAction action = OnPlay;

            if (action == null) return;

            foreach (RtspClientAction handler in action.GetInvocationList())
            {
                try { handler(this, mediaDescription); }
                catch { continue; }
            }
        }

        public event RtspClientAction OnStop;

        internal protected void OnStopping(MediaDescription mediaDescription = null)
        {
            if (IsDisposed) return;

            //Is was already playing then set the value
            if (HandleStopEvent && mediaDescription == null && true == m_StartedPlaying.HasValue || m_Playing.Count == 0) m_StartedPlaying = null;

            RtspClientAction action = OnStop;

            if (action == null) return;

            foreach (RtspClientAction handler in action.GetInvocationList())
            {
                try { handler(this, mediaDescription); }
                catch { continue; }
            }
        }

        public event RtspClientAction OnPause;

        internal protected void OnPausing(MediaDescription mediaDescription = null)
        {
            if (IsDisposed) return;

            RtspClientAction action = OnPause;

            if (action == null) return;

            foreach (RtspClientAction handler in action.GetInvocationList())
            {
                try { handler(this, mediaDescription); }
                catch { continue; }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// DisconnectsSockets, Connects and optionally reconnects the Transport if reconnectClient is true.
        /// </summary>
        /// <param name="reconnectClient"></param>
        internal protected virtual async Task Reconnect(bool reconnectClient = true)
        {
            DisconnectSocket();

            await Connect();

            if (reconnectClient && IsPlaying && false == m_RtpClient.IsActive) m_RtpClient.Activate();
        }

        /// <summary>
        /// Handles Interleaved Data for the RtspClient by parsing the given memory for a valid RtspMessage.
        /// </summary>
        /// <param name="sender">The RtpClient instance which called this method</param>
        /// <param name="memory">The memory to parse</param>
        async void ProcessInterleaveData(object sender, byte[] data, int offset, int length)
        {
            if (length == 0) return;

            //Cache offset and count, leave a register for received data (should be calulated with length)
            int received = 0;

            unchecked
            {
                //Validate the data received
                RtspMessage interleaved = new RtspMessage(data, offset, length);

                LoggerInstance.Debug("Success Receive Message :\n" + interleaved.ToString());

                //Determine what to do with the interleaved message
                switch (interleaved.MessageType)
                {
                    //Handle new requests or responses
                    case RtspMessageType.Request:
                    case RtspMessageType.Response:
                        {
                            //Calculate the length of what was received
                            received = length;

                            if (received > 0)
                            {
                                //Increment for messages received
                                ++m_ReceivedMessages;

                                //If not playing an interleaved stream, Complete the message if not complete (Should maybe check for Content-Length)
                                while (false == SharesSocket && false == interleaved.IsComplete)
                                {
                                    //Take in some bytes from the socket
                                    int justReceived = await interleaved.CompleteFrom(m_RtspSocket, m_Buffer);

                                    if (justReceived == 0) break;

                                    //Incrment for justReceived
                                    received += justReceived;

                                    //Ensure we are not doing to much receiving
                                    if (interleaved.ContentLength > 0 && received > RtspMessage.MaximumLength + interleaved.ContentLength) break;
                                }

                                //Update counters
                                m_ReceivedBytes += received;

                                //Disposes the last message if it exists.
                                if (m_LastTransmitted != null)
                                {
                                    m_LastTransmitted.Dispose();

                                    m_LastTransmitted = null;
                                }

                                //Store the last message
                                m_LastTransmitted = interleaved;

                                //Need a method to get a Session by a Message.
                                //Update the messge on the session..

                                //if the message was a request and is complete handle it now.
                                if (m_LastTransmitted.MessageType == RtspMessageType.Request &&
                                    false == InUse)
                                {
                                    throw new Exception("await ProcessServerSentRequest(m_LastTransmitted)");
                                    //await ProcessServerSentRequest(m_LastTransmitted);
                                }
                            }

                            goto default;
                        }
                    case RtspMessageType.Invalid:
                        {
                            //Dispose the invalid message
                            interleaved.Dispose();

                            interleaved = null;

                            //If playing and interleaved stream AND the last transmitted message is NOT null and is NOT Complete then attempt to complete it
                            if (false == IDisposedExtensions.IsNullOrDisposed(m_LastTransmitted))
                            {
                                //RtspMessage local = m_LastTransmitted;

                                //Take note of the length of the last transmitted message.
                                int lastLength = m_LastTransmitted.Length;

                                //Create a memory segment and complete the message as required from the buffer.
                                using (var memory = new MemorySegment(data, offset, length))
                                {
                                    //Use the data recieved to complete the message and not the socket
                                    int justReceived = false == IDisposedExtensions.IsNullOrDisposed(m_LastTransmitted) ? await m_LastTransmitted.CompleteFrom(null, memory) : 0;

                                    //If anything was received
                                    if (justReceived > 0)
                                    {
                                        //Account for what was just recieved.
                                        received += justReceived;

                                        //No data was consumed don't raise another event.
                                        if (false == IDisposedExtensions.IsNullOrDisposed(m_LastTransmitted) && lastLength == m_LastTransmitted.Length) received = 0;
                                    }

                                    //handle the completion of a request sent by the server if allowed.
                                    if (received > 0 &&
                                        false == IDisposedExtensions.IsNullOrDisposed(m_LastTransmitted) &&
                                        m_LastTransmitted.MessageType == RtspMessageType.Request &&
                                        false == InUse) //dont handle if waiting for a resposne...
                                    {
                                        throw new Exception("await ProcessServerSentRequest(m_LastTransmitted)");
                                        //Process the pushed message
                                        //await ProcessServerSentRequest(m_LastTransmitted);

                                        //then continue
                                    }
                                }
                            }
                            goto default;
                        }
                    default:
                        {
                            //If anything was received
                            if (received > 0)
                            {
                                //Release the m_Interleaved event if it was set
                                if (false == m_InterleaveEvent.WaitOne(0))
                                {
                                    //Thus allowing threads blocked by it to proceed.
                                    m_InterleaveEvent.Set();
                                }
                                else if (false == IDisposedExtensions.IsNullOrDisposed(m_LastTransmitted) &&
                                    m_LastTransmitted.MessageType == RtspMessageType.Response) //and was a response
                                {
                                    //Otherwise indicate a message has been received now. (for responses only)
                                    Received(m_LastTransmitted, null);
                                }

                                //Handle any data remaining in the buffer
                                if (received < length)
                                {
                                    //(Must ensure Length property of RtspMessage is exact).
                                    ProcessInterleaveData(sender, data, received, length - received);
                                }
                            }
                            return;
                        }
                }
            }
        }

        /// <summary>
        /// Increments and returns the current <see cref="ClientSequenceNumber"/>
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        internal int NextClientSequenceNumber() { return ++m_CSeq; }

        public async Task StartPlaying(TimeSpan? start = null, TimeSpan? end = null, IEnumerable<MediaType> mediaTypes = null)
        {
            if (IsPlaying) return;

            if (false == IsConnected)
                await Connect();

            //Send the options if nothing was received before
            if (m_ReceivedMessages == 0)
                using (var options = await SendOptions())
                {
                    if (options == null || options.RtspStatusCode != RtspStatusCode.OK) TaggedExceptionExtensions.RaiseTaggedException(options, "Options Response was null or not OK. See Tag.");
                }

            //Check if Describe is allowed or that a SessionDescription is present.
            if (false == SupportedMethods.Contains(RtspMethod.DESCRIBE.ToString()) && SessionDescription == null) TaggedExceptionExtensions.RaiseTaggedException(SupportedMethods, "SupportedMethods does not allow Describe and SessionDescription is null. See Tag with SupportedMessages.");

            //Check for automatic disconnect
            if (AutomaticallyDisconnect) await Disconnect(true);

        Describe:
            //Send describe if we need a session description
            if (SessionDescription == null) using (var describe = await SendDescribe())
                {
                    if (describe == null || describe.RtspStatusCode != RtspStatusCode.OK) TaggedExceptionExtensions.RaiseTaggedException(describe, "Describe Response was null or not OK. See Tag.");

                    describe.IsPersistent = false;
                }

        Setup:
            //Determine if any context was present or created.
            bool hasContext = false, triedAgain = false;

            List<MediaDescription> setupMedia = new List<MediaDescription>();

            //Get the media descriptions in the session description to setup
            IEnumerable<MediaDescription> toSetup = SessionDescription.MediaDescriptions;

            //If a SetupOrder has been defined then use it
            if (SetupOrder != null)
            {
                SetupOrder(toSetup);
            }

            //For each MediaDescription in the SessionDecscription (ordered by the media type) and then reversed to ensure wms rtx going first (but it doesn't seem to matter anyway)
            //What could be done though is to use the detection of the rtx track to force interleaved playback.
            foreach (MediaDescription md in toSetup)
            {
                //Don't setup unwanted streams
                if (mediaTypes != null && false == mediaTypes.Any(t => t == md.MediaType)) continue;

                //If transport was already setup then see if the transport has a context for the media
                if (Client != null)
                {
                    //Get the context for the media
                    var context = Client.GetContextForMediaDescription(md);

                    //If there is a context which is not already playing and has not ended
                    if (context != null) if (false == m_Playing.Contains(context.MediaDescription))
                        {
                            //If the context is no longer receiving (should be a property on TransportContext but when pausing the RtpClient doesn't know about this)
                            if (context.TimeReceiving == context.TimeSending && context.TimeSending == TimeSpanExtensions.InfiniteTimeSpan)
                            {
                                //Remove the context
                                Client.TryRemoveContext(context);

                                //Dispose it
                                context.Dispose();

                                //remove the reference
                                context = null;
                            }
                        }
                        else
                        {
                            setupMedia.Add(md);
                            hasContext = true;

                            continue;
                        }
                }

                //Send a setup while there was a bad request or no response.
                do using (RtspMessage setup = await SendSetup(md))
                    {
                        if (setup == null)
                        {
                            if (triedAgain)
                            {
                                await Reconnect();

                                continue;
                            }

                            triedAgain = true;

                            if (InUse) continue;

                            m_InterleaveEvent.WaitOne(TimeSpanExtensions.OneMillisecond);
                            continue;
                        }

                        RtspStatusCode setupStatusCode = setup.RtspStatusCode;

                        //Stop trying to setup when the transport is not supported.
                        if (setupStatusCode == RtspStatusCode.UnsupportedTransport) break;
                        //If the setup was okay
                        else if (setupStatusCode <= RtspStatusCode.OK)
                        {
                            hasContext = true;
                            setupMedia.Add(md);
                            break;
                        }
                        else if (setupStatusCode == RtspStatusCode.NotFound || setupStatusCode == RtspStatusCode.Unauthorized)
                        {
                            //Sometimes the host is not yet ready, this could be true for cases when hosts uses dynamic uri's which don't yet exists during pipelining etc.
                            if (false == triedAgain)
                            {
                                triedAgain = true;

                                if (InUse) continue;

                                m_InterleaveEvent.WaitOne(TimeSpanExtensions.OneMillisecond);
                            }
                            else
                            {
                                await Reconnect();

                                SessionDescription.Dispose();

                                SessionDescription = null;

                                goto Describe;
                            }

                            continue;
                        }
                    } while (IsConnected); //2 attempts each attempt...
            }

            //If we have a play context then send the play request.
            if (false == hasContext) throw new InvalidOperationException("Cannot Start Playing, No Tracks Setup.");

            //set to false for play request.
            triedAgain = false;

            //Ensure service is avilable.
            bool serviceUnavailable = false;

            //Send the play request while a OKAY response was not received
            do using (RtspMessage play = await SendPlay(InitialLocation, start ?? StartTime, end ?? EndTime))
                {
                    //Check for a response
                    bool hasResponse = false == IDisposedExtensions.IsNullOrDisposed(play) && play.MessageType == RtspMessageType.Response;

                    //If there was a response
                    if (hasResponse)
                    {
                        RtspStatusCode playStatusCode = play.RtspStatusCode;

                        //If the response was a success
                        if (playStatusCode <= RtspStatusCode.OK)
                        {
                            break;
                        }
                        else if (play.RtspStatusCode == RtspStatusCode.ServiceUnavailable)
                        {
                            if (serviceUnavailable && triedAgain) throw new InvalidOperationException("Cannot Start Playing, ServiceUnavailable");

                            else continue;
                        }
                        else if (playStatusCode == RtspStatusCode.MethodNotAllowed || playStatusCode == RtspStatusCode.MethodNotValidInThisState)
                        {
                            //If already tried again then retry setup.
                            if (triedAgain) goto Setup;
                        }

                        //Set triedAgain
                        triedAgain = true;
                    }
                    else //No response...
                    {
                        if (triedAgain)
                        {
                            if (serviceUnavailable)
                            {
                                serviceUnavailable = false;

                                continue;
                            }

                            //Disconnect and Reconnect
                            await Reconnect();
                        }

                        triedAgain = true;
                    }

                    if (InUse) continue;

                    m_InterleaveEvent.WaitOne(TimeSpanExtensions.OneMillisecond);

                    continue;

                } while (IsConnected && false == m_RtpClient.IsActive);

            //Enumerate the setup media and add it to the playing list.
            foreach (var media in setupMedia)
                if (false == m_Playing.Contains(media))
                    m_Playing.Add(media);

            //Fire an event
            OnPlaying();

            //Should be an option..
            //Initiate a keep alive now if data is still not flowing.
            if (SendKeepAliveImmediatelyAfterStartPlaying && Client.TotalBytesReceieved == 0) SendKeepAliveRequest(null);

            //TimeSpan halfSessionTimeWithConnection = TimeSpan.FromTicks(m_RtspSessionTimeout.Subtract(m_ConnectionTime).Ticks >> 1);

            TimeSpan halfSessionTimeWithConnection = TimeSpan.FromTicks(TimeSpan.FromSeconds(m_RtspSessionTimeout.TotalSeconds - 1).Subtract(m_ConnectionTime).Ticks / 3);

            //If dueTime is zero (0), callback is invoked immediately. If dueTime is negative one (-1) milliseconds, callback is not invoked; the timer is disabled, but can be re-enabled by calling the Change method.
            //Setup a timer to send any requests to keep the connection alive and ensure media is flowing.
            //Subtract against the connection time... the averge rtt would be better
            if (m_KeepAliveTimer == null) m_KeepAliveTimer = new Timer(new TimerCallback(SendKeepAliveRequest), null, halfSessionTimeWithConnection, TimeSpanExtensions.InfiniteTimeSpan);

            //Watch for pushed messages.
            m_ProtocolMonitor = new System.Threading.Timer(new TimerCallback(MonitorProtocol), null, m_ConnectionTime.Add(LastMessageRoundTripTime), TimeSpanExtensions.InfiniteTimeSpan);

            //m_RtpProtocol == ProtocolType.Udp
            if (AutomaticallyDisconnect) DisconnectSocket();
        }

        public async Task StopPlaying(MediaDescription mediaDescription, bool force = false)
        {
            //If the media was playing
            if (false == force && PlayingMedia.Contains(mediaDescription))
            {
                using (RtspMessage resposne = await SendTeardown(mediaDescription, false, force)) ;
            }
        }

        public async Task StopPlaying(bool disconnectSocket = true)
        {
            if (IsDisposed || false == IsPlaying) return;
            try
            {
                await Disconnect(disconnectSocket).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LoggerInstance.Exception(ex);
            }
        }

        public async Task Pause(MediaDescription mediaDescription = null, bool force = false)
        {
            //Don't pause if playing.
            if (false == force && false == IsPlaying) return;

            var context = Client.GetContextForMediaDescription(mediaDescription);

            //Dont pause media which is not setup unless forced.
            if (false == force && mediaDescription != null && context == null) return;

            //context.Goodbye = null;

            //Send the pause.
            await SendPause(mediaDescription, force);
        }

        /// <summary>
        /// Sends a SETUP if not already setup and then a PLAY for the given.
        /// If nothing is given this would be equivalent to calling <see cref="StartPlaying"/>
        /// </summary>
        /// <param name="mediaDescription"></param>
        public async Task Play(MediaDescription mediaDescription = null, TimeSpan? startTime = null, TimeSpan? endTime = null, string rangeType = "npt")
        {
            bool playing = IsPlaying;
            //If already playing and nothing was given then there is nothing to do
            if (playing && mediaDescription == null) return;
            else if (false == playing) //We are not playing and nothing was given.
            {
                //Start playing everything
                await StartPlaying();

                //do nothing else
                return;
            }

            var context = Client.GetContextForMediaDescription(mediaDescription);

            //Dont setup media which is already setup.
            if (mediaDescription != null && context == null) return;

            //setup the media description
            using (var setupResponse = await SendSetup(mediaDescription))
            {
                //If the response was OKAY
                if (setupResponse != null && setupResponse.RtspStatusCode == RtspStatusCode.OK)
                {
                    //Send the PLAY.
                    using (await SendPlay(mediaDescription, startTime, endTime, rangeType)) ;
                }
            }
        }

        /// <summary>
        /// If <see cref="IsConnected"/> and not forced an <see cref="InvalidOperationException"/> will be thrown.
        /// 
        /// <see cref="DisconnectSocket"/> is called if there is an existing socket.
        /// 
        /// Creates any required client socket stored the time the call was made and calls <see cref="ProcessEndConnect"/> unless an unsupported Proctol is specified.
        /// </summary>
        /// <param name="force">Indicates if a previous existing connection should be disconnected.</param>
        public virtual async Task Connect(bool force = false)
        {
            try
            {
                //If not forcing and is already connected or started to connect return
                if (false == force && IsConnected || m_BeginConnect.HasValue) return;

                //If there is an RtpClient already connected then attempt to find a socket used by the client with the EndPoint
                //required to be connected to
                if (m_RtpClient != null && m_RtpClient.IsActive)
                {
                    //Todo, should be interface.
                    foreach (RtpClient.TransportContext transportContext in m_RtpClient.GetTransportContexts())
                    {
                        //If disposed continue, should be handled in GetTransportContexts()..
                        if (IDisposedExtensions.IsNullOrDisposed(transportContext) || false == transportContext.IsActive) continue;

                        //Get the sockets in reference by the context
                        foreach (Socket socket in ((ISocketReference)transportContext).GetReferencedSockets())
                        {
                            //Check for the socket to not be disposed...
                            if (false == socket.Connected) continue;

                            IPEndPoint ipendPoint = (IPEndPoint)socket.RemoteEndPoint;

                            if (ipendPoint.Address.Equals(m_RemoteIP) && ipendPoint.Port == m_RtspPort && socket.Connected)
                            {
                                //Assign the socket (Update ConnectionTime etc)>
                                RtspSocket = socket;

                                return;
                            }
                        }
                    }
                }

                //Deactivate any existing previous socket and erase connect times.
                if (m_RtspSocket != null) DisconnectSocket();

                //Based on the ClientProtocolType
                switch (m_RtspProtocol)
                {
                    case ClientProtocolType.Http:
                    case ClientProtocolType.Tcp:
                        {
                            //Create the socket
                            m_RtspSocket = new Socket(m_RemoteIP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                            break;
                        }
                    case ClientProtocolType.Udp:
                        {
                            //Create the socket
                            m_RtspSocket = new Socket(m_RemoteIP.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                            break;
                        }
                    default: throw new NotSupportedException("The given ClientProtocolType is not supported.");
                }

                if (ConfigureSocket != null) ConfigureSocket(m_RtspSocket);

                //We started connecting now.
                m_BeginConnect = DateTime.UtcNow;

                //Handle the connection attempt (Assumes there is already a RemoteRtsp value)
                await ProcessEndConnect(null);

            }
            catch (Exception ex)
            {
                LoggerInstance.Exception(ex);
                throw;
            }
        }

        /// <summary>
        /// Calls Connect on the underlying socket.
        /// 
        /// Marks the time when the connection was established.
        /// 
        /// Increases the <see cref="SocketWriteTimeout"/> AND <see cref="SocketReadTimeout"/> by the time it took to establish the connection in milliseconds * 2.
        /// 
        /// </summary>
        /// <param name="state">Ununsed.</param>
        protected virtual async Task ProcessEndConnect(object state, int multiplier = 2)//should be vaarible in class
        {
            try
            {
                if (m_RemoteRtsp == null) throw new InvalidOperationException("A remote end point must be assigned");

                //Try to connect
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.RemoteEndPoint = m_RemoteRtsp;

                var tcs = new TaskCompletionSource<bool>();
                CancellationToken cancelltionToken = new CancellationToken();
                var cancelRegistration = cancelltionToken.Register(() => Socket.CancelConnectAsync(args));

                EventHandler<SocketAsyncEventArgs> completeHandler = (sender, eventArgs) =>
                {
                    cancelRegistration.Dispose();

                    if (args.SocketError != SocketError.Success)
                    {
                        if (SocketError.OperationAborted == args.SocketError || SocketError.ConnectionAborted == args.SocketError)
                            tcs.TrySetCanceled();
                        else
                            tcs.TrySetException(new WebException("Socket to " + " failed: " + args.SocketError));
                    }

                    var isOk = false;
                    if (args.BytesTransferred > 0)
                    {
                        isOk = true;
                    }
                    tcs.TrySetResult(isOk);
                };

                args.Completed += completeHandler;

                if (!m_RtspSocket.ConnectAsync(args))
                {
                    completeHandler(m_RtspSocket, args);
                }

                await tcs.Task.ConfigureAwait(false);

                //Sample the clock after connecting
                m_EndConnect = DateTime.UtcNow;

                //Calculate the connection time.
                m_ConnectionTime = m_EndConnect.Value - m_BeginConnect.Value;

                OnConnected();
            }
            catch { throw; }
        }

        /// <summary>
        /// If <see cref="IsConnected"/> nothing occurs.
        /// Disconnects the RtspSocket if Connected and <see cref="LeaveOpen"/> is false.  
        /// Sets the <see cref="ConnectionTime"/> to <see cref="Utility.InfiniteTimepan"/> so IsConnected is false.
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void DisconnectSocket(bool force = false)
        {
            //If not connected and not forced return
            if (false == IsConnected && false == force) return;

            //Raise an event
            OnDisconnected();

            //If there is a socket
            if (m_RtspSocket != null)
            {
                //If LeaveOpen was false and the socket is not shared.
                if (false == LeaveOpen && false == SharesSocket)
                {
                    //Dispose the socket
                    m_RtspSocket.Dispose();
                }

                //Set the socket to null (no longer will Share Socket)
                m_RtspSocket = null;
            }

            //Indicate not connected.
            m_BeginConnect = m_EndConnect = null;

            m_ConnectionTime = TimeSpanExtensions.InfiniteTimeSpan;
        }

        /// <summary>
        /// Stops Sending any KeepAliveRequests.
        /// 
        /// Stops the Protocol Switch Timer.
        /// 
        /// If <see cref="IsPlaying"/> is true AND there is an assigned <see cref="SessionId"/>,
        /// Stops any playing media by sending a TEARDOWN for the current <see cref="CurrentLocation"/> 
        /// 
        /// Disconnects any connected Transport which is still connected.
        /// 
        /// Calls DisconnectSocket.
        /// </summary>
        public async Task Disconnect(bool disconnectSocket = false)
        {
            //Get rid of the timers
            if (m_KeepAliveTimer != null)
            {
                m_KeepAliveTimer.Dispose();
                m_KeepAliveTimer = null;
            }

            if (m_ProtocolMonitor != null)
            {
                m_ProtocolMonitor.Dispose();
                m_ProtocolMonitor = null;
            }

            //Determine if we need to do anything
            if (IsPlaying && false == string.IsNullOrWhiteSpace(m_SessionId))
            {
                //Send the Teardown
                try
                {
                    //Don't really care if the response is received or not (indicate to close the connection)
                    await SendTeardown(null, true).ConfigureAwait(false);
                }
                catch
                {
                }
                finally
                {
                    m_SessionId = string.Empty;
                }
            }

            if (Client != null && Client.IsActive) await Client.Deactivate();

            if (disconnectSocket) DisconnectSocket();
        }

        #endregion

        #region Rtsp
        public virtual void Timestamp(RtspMessage message)
        {
            string timestamp = (DateTime.UtcNow - m_EndConnect ?? TimeSpan.Zero).TotalSeconds.ToString(System.Globalization.CultureInfo.InvariantCulture);

            message.SetHeader(RtspHeaders.Timestamp, timestamp);
        }

        public async Task<RtspMessage> SendRtspMessage(RtspMessage message, bool useClientProtocolVersion = true, bool hasResponse = true, int attempts = 0)
        {
            //Indicate a send has not been attempted
            var error = SocketError.SocketError;

            //Indicate the sequence number has not been observed
            var sequenceNumber = -1;

            //Don't try to send if already disposed.
            CheckDisposed();

            bool wasBlocked = false;

            //Check for illegal feeding of turtles
            if (false == IDisposedExtensions.IsNullOrDisposed(message) && string.Compare("REGISTER", message.MethodString, System.StringComparison.OrdinalIgnoreCase) == 0 && false == string.IsNullOrWhiteSpace(UserAgent)) throw new InvalidOperationException("Please don't feed the turtles.");

            unchecked
            {
                try
                {
                    int retransmits = 0, attempt = attempts, //The attempt counter itself
                        sent = 0, received = 0, //counter for sending and receiving locally
                        offset = 0, length = 0;

                    //Half of the session timeout in milliseconds
                    int halfTimeout = (int)(m_RtspSessionTimeout.TotalMilliseconds / 2);

                    byte[] buffer = null;

                    #region Check for a message

                    bool wasConnected = IsConnected;

                    //If there is no message to send then check for response
                    if (message == null) goto Connect;

                    #endregion

                    #region useClientProtocolVersion

                    //Ensure the request version matches the protocol version of the client if enforceVersion is true.
                    if (useClientProtocolVersion && message.Version != ProtocolVersion) message.Version = ProtocolVersion;

                    #endregion

                    #region Additional Headers

                    //Use any additional headers if given
                    if (AdditionalHeaders.Count > 0) foreach (var additional in AdditionalHeaders) message.AppendOrSetHeader(additional.Key, additional.Value);

                    #endregion

                    #region CSeq

                    //Get the next Sequence Number and set it in the request. (If not already present)
                    if (false == message.ContainsHeader(RtspHeaders.CSeq)) sequenceNumber = message.CSeq = NextClientSequenceNumber();
                    else sequenceNumber = message.CSeq;

                    #endregion

                    #region ContentEncoding

                    //Add the content encoding header if required
                    if (false == message.ContainsHeader(RtspHeaders.ContentEncoding) &&
                        message.ContentEncoding.WebName != RtspMessage.DefaultEncoding.WebName)
                        message.SetHeader(RtspHeaders.ContentEncoding, message.ContentEncoding.WebName);

                    #endregion

                    #region DateRequests

                    //Set the Date header if required
                    if (DateRequests && false == message.ContainsHeader(RtspHeaders.Date))
                        message.SetHeader(RtspHeaders.Date, DateTime.UtcNow.ToString("r"));

                    #endregion

                    #region SessionId

                    //Set the Session header if required and not already contained.
                    if (false == string.IsNullOrWhiteSpace(m_SessionId) &&
                        false == message.ContainsHeader(RtspHeaders.Session)) message.SetHeader(RtspHeaders.Session, m_SessionId);

                    #endregion

                    #region SendUserAgent

                    //Add the user agent if required
                    if (SendUserAgent &&
                        false == message.ContainsHeader(RtspHeaders.UserAgent))
                    {
                        message.SetHeader(RtspHeaders.UserAgent, m_UserAgent);
                    }


                    #endregion

                Timestamp:
                    #region Timestamp
                    //If requests should be timestamped
                    if (TimestampRequests) Timestamp(message);

                    //Take note of the timestamp of the message out
                    string timestampSent = message[RtspHeaders.Timestamp];

                    //Get the bytes of the request
                    buffer = m_RtspProtocol == ClientProtocolType.Http ? RtspMessage.ToHttpBytes(message) : message.ToBytes();

                    offset = m_Buffer.Offset;

                    length = buffer.Length;
                    #endregion

                Connect:
                    #region Connect
                    //Wait for any existing requests to finish first
                    wasBlocked = InUse;

                    if (false == wasConnected && false == (wasConnected = IsConnected))
                        await Connect();

                    //Othewise we are connected
                    if (false == (wasConnected = IsConnected)) return null;

                    //Set the block if a response is required.
                    if (hasResponse && false == wasBlocked) m_InterleaveEvent.Reset();

                    //If nothing is being sent this is a receive only operation
                    if (message == null) goto NothingToSend;

                    #endregion

                Send:
                    #region Send
                    //If the message was Transferred previously
                    if (message.Transferred.HasValue)
                    {
                        //Make the message not Transferred
                        message.Transferred = null;

                        //Increment counters for retransmit
                        ++retransmits;

                        ++m_ReTransmits;
                    }

                    //If we can write before the session will end
                    if (IsConnected)
                    {
                        CancellationToken cancelltionToken = new CancellationToken();
                        var tcs = new TaskCompletionSource<int>();

                        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                        args.BufferList = new[] { new ArraySegment<byte>(buffer, offset, length) };
                        args.RemoteEndPoint = m_RemoteRtsp;

                        var cancelRegistration = cancelltionToken.Register(() => Socket.CancelConnectAsync(args));

                        EventHandler<SocketAsyncEventArgs> completeHandler = (sender, eventArgs) =>
                        {
                            cancelRegistration.Dispose();

                            if (args.SocketError != SocketError.Success)
                            {
                                if (SocketError.OperationAborted == args.SocketError || SocketError.ConnectionAborted == args.SocketError)
                                    tcs.TrySetCanceled();
                                else
                                    tcs.TrySetException(new WebException("Socket to " + " failed: " + args.SocketError));
                            }

                            tcs.TrySetResult(args.BytesTransferred);
                        };

                        args.Completed += completeHandler;

                        if (!m_RtspSocket.SendAsync(args))
                        {
                            completeHandler(m_RtspSocket, args);
                        }

                        var sendCount = await tcs.Task;
                        sent += sendCount;
                        error = args.SocketError;

                        LoggerInstance.Debug("Send Message Success : \n" + message.ToString());
                    }

                    #region Auto Reconnect

                    if (AutomaticallyReconnect &&
                        (error == SocketError.ConnectionAborted || error == SocketError.ConnectionReset))
                    {
                        if (error == SocketError.ConnectionReset)
                        {
                            if (wasConnected && false == IsConnected)
                            {
                                await Reconnect(true);

                                goto Send;
                            }
                        }

                        throw new SocketException((int)error);
                    }

                    #endregion

                    //If this is not a re-transmit
                    if (sent >= length)
                    {
                        //Set the time when the message was transferred if this is not a retransmit.
                        message.Transferred = DateTime.UtcNow;

                        //Fire the event (sets Transferred)
                        Requested(message);

                        //Increment for messages sent or the messages retransmitted.
                        ++m_SentMessages;

                        //Increment our byte counters for Rtsp
                        m_SentBytes += sent;

                        //Attempt to receive so start attempts back at 0
                        /*sent = */
                        attempt = 0;

                        //Release the reference to the array
                        buffer = null;
                    }
                    else if (sent < length && ++attempt < m_MaximumTransactionAttempts)
                    {
                        goto Send;
                    }

                    #endregion

                NothingToSend:
                    #region NothingToSend
                    //Check for no response.
                    if (false == hasResponse) return null;

                    //If the socket is shared the response will be propagated via an event.
                    if (SharesSocket) goto Wait;
                    #endregion

                //Receive some data (only referenced by the check for disconnection)
                Receive:
                    #region Receive

                    //Let cache clear
                    m_InterleaveEvent.WaitOne(TimeSpanExtensions.OneMillisecond);

                    //If IsConnected and we can receive 
                    if (IsConnected) /*|| message != null && attempts == m_ResponseTimeoutInterval*/
                    {

                        CancellationToken cancelltionToken = new CancellationToken();
                        var tcs = new TaskCompletionSource<int>();

                        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                        args.BufferList = new[] { new ArraySegment<byte>(m_Buffer.Array, offset, m_Buffer.Count) };

                        var cancelRegistration = cancelltionToken.Register(() => Socket.CancelConnectAsync(args));

                        EventHandler<SocketAsyncEventArgs> completeHandler = (sender, eventArgs) =>
                        {
                            cancelRegistration.Dispose();

                            if (args.SocketError != SocketError.Success)
                            {
                                if (SocketError.OperationAborted == args.SocketError || SocketError.ConnectionAborted == args.SocketError)
                                    tcs.TrySetCanceled();
                                else
                                    tcs.TrySetException(new WebException("Socket to " + " failed: " + args.SocketError));
                            }

                            tcs.TrySetResult(args.BytesTransferred);
                        };

                        args.Completed += completeHandler;

                        if (!m_RtspSocket.ReceiveAsync(args))
                        {
                            completeHandler(m_RtspSocket, args);
                        }

                        var receiveCount = await tcs.Task;
                        received += receiveCount;
                        error = args.SocketError;
                    }

                    #region Auto Reconnect

                    if (AutomaticallyReconnect &&
                        (error == SocketError.ConnectionAborted || error == SocketError.ConnectionReset))
                    {
                        //Check for the host to have dropped the connection
                        if (error == SocketError.ConnectionReset)
                        {
                            //Check if the client was connected already
                            if (wasConnected && false == IsConnected)
                            {
                                await Reconnect(true);

                                goto Receive;
                            }
                        }

                        throw new SocketException((int)error);
                    }

                    #endregion

                    //If anything was received
                    if (received > 0)
                    {
                        if (m_RtpClient != null && m_Buffer.Array[offset] == Gsafety.PTMS.Media.RTSP.RTP.RtpClient.BigEndianFrameControl)
                        {
                            LoggerInstance.Debug("发送命令代码中接收到RTP数据了");

                            received -= await m_RtpClient.ProcessFrameData(m_Buffer.Array, offset, received, m_RtspSocket);

                            if (received < 0) received = 0;
                        }
                        else
                        {
                            //Otherwise just process the data via the event.
                            ProcessInterleaveData(this, m_Buffer.Array, offset, received);
                        }
                    } //Nothing was received, if the socket is not shared
                    else if (false == SharesSocket)
                    {
                        //Check for non fatal exceptions and continue to wait
                        if (++attempt <= m_MaximumTransactionAttempts && error != SocketError.ConnectionAborted || error != SocketError.ConnectionReset)
                        {
                            //We don't share the socket so go to recieve again (note if this is the timer thread this can delay outgoing requests)
                            goto Wait;
                        }

                        if (message != null) throw new SocketException((int)error);
                        else return null;
                    }

                    #endregion

                //Wait for the response while the amount of data received was less than RtspMessage.MaximumLength
                Wait:
                    #region Waiting for response, Backoff or Retransmit
                    DateTime lastAttempt = DateTime.UtcNow;

                    //Wait while
                    while (false == IsDisposed &&//The client connected and is not disposed AND
                        //There is no last transmitted message assigned AND it has not already been disposed
                        IDisposedExtensions.IsNullOrDisposed(m_LastTransmitted)
                        //AND the client is still allowed to wait
                        && ++attempt <= m_MaximumTransactionAttempts)
                    {
                        //Wait a small amount of time for the response because the cancellation token was not used...
                        if (IsDisposed)
                        {
                            return null;
                        }
                        else
                        {
                            //Wait a little more
                            m_InterleaveEvent.WaitOne(TimeSpanExtensions.OneTick);
                        }

                        //Check for any new messages
                        if (false == IDisposedExtensions.IsNullOrDisposed(m_LastTransmitted)) goto HandleResponse;

                        //Calculate how much time has elapsed
                        TimeSpan taken = DateTime.UtcNow - lastAttempt;

                        //If more time has elapsed than allowed by reading
                        if (taken > m_LastMessageRoundTripTime)
                        {
                            //Check if we can back off further
                            if (taken.TotalMilliseconds >= halfTimeout) break;
                            else
                            {
                                //Backoff
                                /*pollTime += (int)(TimeSpanExtensions.MicrosecondsPerMillisecond */

                                //Ensure the client transport is connected if previously playing and it has since disconnected.
                                if (IsPlaying &&
                                    m_RtpClient != null &&
                                    false == m_RtpClient.IsActive) m_RtpClient.Activate();
                            }

                            //If the client was not disposed re-trasmit the request if there is not a response pending already.
                            //Todo allow an option for this feature? (AllowRetransmit)
                            if (false == IsDisposed && m_LastTransmitted == null /*&& request.Method != RtspMethod.PLAY*/)
                            {
                                //handle re-transmission under UDP
                                if (m_RtspSocket.ProtocolType == ProtocolType.Udp)
                                {
                                    //Change the Timestamp if TimestampRequests is true
                                    if (TimestampRequests)
                                    {
                                        //Reset what to send.
                                        sent = 0;

                                        goto Timestamp;
                                    }

                                    //Reset what was sent so far.
                                    sent = 0;

                                    //Retransmit the exact same data
                                    goto Send;
                                }
                            }
                        }

                        //If not sharing socket trying to receive again.
                        if (false == SharesSocket)
                        {
                            m_InterleaveEvent.WaitOne(TimeSpanExtensions.OneTick);

                            //If we have a message to send and did not send it then goto send.
                            //message.Transferred.HasValue
                            if (message != null && sent == 0) goto Send;
                            goto Receive;
                        }
                    }

                    #endregion

                HandleResponse:
                    #region HandleResponse

                    //Update counters for any data received.
                    m_ReceivedBytes += received;

                    //If nothing was received wait for cache to clear.
                    if (null == m_LastTransmitted)
                    {
                        m_InterleaveEvent.WaitOne(TimeSpanExtensions.OneTick);
                    }
                    else if (message != null) //If there was a message sent
                    {
                        //Could also check session header.

                        //Obtain the CSeq of the response if present.
                        int sequenceNumberSent = message.CSeq, sequenceNumberReceived = m_LastTransmitted.CSeq;

                        //If the sequence number was present and did not match then wait again
                        if (sequenceNumberReceived != sequenceNumberSent)
                        {
                            //Reset the block
                            m_InterleaveEvent.Reset();

                            //Mark disposed
                            m_LastTransmitted.Dispose();

                            //Remove the message to avoid confusion
                            m_LastTransmitted = null;

                            //Allow more waiting
                            attempt = received = 0;

                            goto Wait;
                        }
                    }

                    //Check for the response if there was a message sent.
                    if (hasResponse &&
                        message != null && m_LastTransmitted != null &&
                        m_LastTransmitted.MessageType == RtspMessageType.Response)
                    {
                        //Calculate the amount of time taken to receive the message.
                        TimeSpan lastMessageRoundTripTime = (m_LastTransmitted.Created - (message.Transferred ?? message.Created));

                        //Assign it
                        m_LastMessageRoundTripTime = lastMessageRoundTripTime.Duration();

                        switch (m_LastTransmitted.RtspStatusCode)
                        {
                            case RtspStatusCode.OK:
                                {
                                    //Ensure message is added to supported methods.
                                    SupportedMethods.Add(message.MethodString);

                                    break;
                                }
                            case RtspStatusCode.NotImplemented:
                                {
                                    SupportedMethods.Remove(m_LastTransmitted.MethodString);

                                    break;
                                }
                            case RtspStatusCode.MethodNotValidInThisState:
                                {
                                    if (m_LastTransmitted.ContainsHeader(RtspHeaders.Allow)) MonitorProtocol();

                                    break;
                                }
                            case RtspStatusCode.Unauthorized:
                                {
                                    break;
                                }
                            case RtspStatusCode.RtspVersionNotSupported:
                                {
                                    //if enforcing the version
                                    if (useClientProtocolVersion)
                                    {
                                        //Read the version from the response
                                        ProtocolVersion = m_LastTransmitted.Version;

                                        //Send the request again. SHOULD USE out error, 
                                        return await SendRtspMessage(message, useClientProtocolVersion);
                                    }

                                    //break
                                    break;
                                }
                            default: break;
                        }

                        #region EchoXHeaders

                        //If the client should echo X headers
                        if (EchoXHeaders)
                        {
                            //iterate for any X headers 

                            foreach (var xHeader in m_LastTransmitted.GetHeaders().Where(h => h.Length > 2 && h[1] == ASCII.HyphenSign && char.ToLower(h[0]) == 'x'))
                            {
                                //If contained already then update
                                if (AdditionalHeaders.ContainsKey(xHeader))
                                {
                                    AdditionalHeaders[xHeader] += ((char)ASCII.SemiColon).ToString() + m_LastTransmitted.GetHeader(xHeader).Trim();
                                }
                                else
                                {
                                    //Add
                                    AdditionalHeaders.Add(xHeader, m_LastTransmitted.GetHeader(xHeader).Trim());
                                }
                            }
                        }

                        #endregion

                        #region Parse Session Header

                        //For any other request besides teardown update the sessionId and timeout
                        if (message.RtspMethod != RtspMethod.TEARDOWN)
                        {
                            //Get the header.
                            string sessionHeader = m_LastTransmitted[RtspHeaders.Session];

                            //If there is a session header it may contain the option timeout
                            if (false == string.IsNullOrWhiteSpace(sessionHeader))
                            {
                                //Check for session and timeout

                                //Get the values
                                string[] sessionHeaderParts = sessionHeader.Split(RtspHeaders.SemiColon);

                                int headerPartsLength = sessionHeaderParts.Length;

                                //Check if a valid value was given
                                if (headerPartsLength > 0)
                                {
                                    //Trim it of whitespace
                                    string value = sessionHeaderParts.LastOrDefault(p => false == string.IsNullOrWhiteSpace(p));

                                    //If we dont have an exiting id then this is valid if the header was completely recieved only.
                                    if (false == string.IsNullOrWhiteSpace(value) &&
                                        true == string.IsNullOrWhiteSpace(m_SessionId) ||
                                        value[0] != m_SessionId[0])
                                    {
                                        //Get the SessionId if present
                                        m_SessionId = sessionHeaderParts[0].Trim();

                                        //Check for a timeout
                                        if (sessionHeaderParts.Length > 1)
                                        {
                                            int timeoutStart = 1 + sessionHeaderParts[1].IndexOf(Gsafety.PTMS.Media.RTSP.Sdp.SessionDescription.EqualsSign);
                                            if (timeoutStart >= 0 && int.TryParse(sessionHeaderParts[1].Substring(timeoutStart), out timeoutStart))
                                            {
                                                //Should already be set...
                                                if (timeoutStart <= 0)
                                                {
                                                    m_RtspSessionTimeout = TimeSpan.FromSeconds(60);//Default
                                                }
                                                else
                                                {
                                                    m_RtspSessionTimeout = TimeSpan.FromSeconds(timeoutStart);
                                                }
                                            }
                                        }
                                    }

                                    //done
                                }
                                else if (string.IsNullOrWhiteSpace(m_SessionId))
                                {
                                    //The timeout was not present
                                    m_SessionId = sessionHeader.Trim();

                                    m_RtspSessionTimeout = TimeSpan.FromSeconds(60);//Default
                                }
                            }
                        }

                        #endregion

                        #region CalculateServerDelay

                        if (CalculateServerDelay)
                        {
                            string timestamp;

                            RtspHeaders.TryParseTimestamp(m_LastTransmitted[RtspHeaders.Timestamp], out timestamp, out m_LastServerDelay);

                            timestamp = null;
                        }

                        #endregion

                        #region UpdateSession

                        //Update the session related
                        RtspSession related;

                        if (m_Sessions.TryGetValue(m_SessionId, out related))
                        {
                            related.UpdateMessages(message, m_LastTransmitted);

                            related = null;
                        }

                        #endregion

                        //Raise an event for the message received
                        Received(message, m_LastTransmitted);
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    LoggerInstance.Exception((ToString() + "@SendRtspMessage: " + ex.ToString()));
                }
                finally
                {
                    //Unblock (should not be needed)
                    if (false == wasBlocked) m_InterleaveEvent.Set();
                }

                //Return the result
                return message != null && m_LastTransmitted != null && message.CSeq == m_LastTransmitted.CSeq ? m_LastTransmitted : null;

            }
        }

        /// <summary>
        /// Sends the Rtsp OPTIONS request
        /// </summary>
        /// <param name="useStar">The OPTIONS * request will be sent rather then one with the <see cref="RtspClient.CurrentLocation"/></param>
        /// <returns>The <see cref="RtspMessage"/> as a response to the request</returns>
        public async Task<RtspMessage> SendOptions(bool useStar = false, string sessionId = null)
        {
            using (var options = new RtspMessage(RtspMessageType.Request)
            {
                RtspMethod = RtspMethod.OPTIONS,
                Location = useStar ? null : CurrentLocation,
                IsPersistent = true,
            })
            {
                if (false == string.IsNullOrWhiteSpace(sessionId)) options.SetHeader(RtspHeaders.Session, sessionId);

                RtspMessage response = await SendRtspMessage(options);

                if (response != null)
                {
                    //Get the Public header which indicates the methods supported by the client
                    string publicMethods = response[RtspHeaders.Public];

                    //If there is Not such a header then return the response
                    if (false == string.IsNullOrWhiteSpace(publicMethods))
                    {
                        //Process values in the Public header.
                        foreach (string method in publicMethods.Split(RtspHeaders.Comma))
                        {
                            SupportedMethods.Add(method.Trim());
                        }
                    }

                    //Should have a way to keep the allowed seperate.
                    string allowedMethods = response[RtspHeaders.Allow];

                    //If there is Not such a header then return the response
                    if (false == string.IsNullOrWhiteSpace(allowedMethods))
                    {
                        //Process values in the Public header.
                        foreach (string method in allowedMethods.Split(RtspHeaders.Comma))
                        {
                            SupportedMethods.Add(method.Trim());
                        }
                    }

                    //Some servers only indicate different features at the SETUP level...

                    string supportedFeatures = response[RtspHeaders.Supported];

                    //If there is Not such a header then return the response
                    if (false == string.IsNullOrWhiteSpace(supportedFeatures))
                    {
                        //Process values in the Public header.
                        foreach (string method in supportedFeatures.Split(RtspHeaders.Comma))
                        {
                            SupportedFeatures.Add(method.Trim());
                        }
                    }
                }
                else if (false == IsPlaying) TaggedExceptionExtensions.RaiseTaggedException(this, "Unable to get options, See InnerException.", new TaggedException<RtspMessage>(response, "See Tag for Response."));

                options.IsPersistent = false;

                return response;
            }
        }

        /// <summary>
        /// Assigns the SessionDescription returned from the server
        /// </summary>
        /// <returns></returns>
        public async Task<RtspMessage> SendDescribe()
        {
            RtspMessage response = null;

            try
            {
                using (RtspMessage describe = new RtspMessage(RtspMessageType.Request)
                {
                    RtspMethod = RtspMethod.DESCRIBE,
                    Location = CurrentLocation,
                    IsPersistent = true
                })
                {
                    describe.SetHeader(RtspHeaders.Accept, SessionDescription.MimeType);

                Describe:
                    response = await SendRtspMessage(describe);

                    //Handle no response
                    //If the remote end point is just sending Interleaved Binary Data out of no where it is possible to continue without a SessionDescription

                    if (response == null) TaggedExceptionExtensions.RaiseTaggedException(describe, "Unable to describe media, no response to DESCRIBE request. The request is in the Tag property.");
                    else response.IsPersistent = true;

                    //Hanlde NotFound
                    if (response.RtspStatusCode == RtspStatusCode.NotFound) TaggedExceptionExtensions.RaiseTaggedException(describe, "Unable to describe media, NotFound. The response is in the Tag property.");

                    if (false == response.IsComplete)
                    {
                        //Wait for complete responses
                        if (SharesSocket)
                        {
                            m_InterleaveEvent.WaitOne();
                        }
                        else
                        {
                            await response.CompleteFrom(m_RtspSocket, m_Buffer);
                        }
                    }

                    //Only handle responses for the describe request sent when sharing the socket
                    if (response.CSeq != describe.CSeq)
                    {
                        describe.RemoveHeader(RtspHeaders.Timestamp);

                        goto Describe;
                    }

                    //don't handle erroneous responses
                    if (describe.RtspStatusCode > RtspStatusCode.OK)
                    {
                        return describe;
                    }

                    if (response.RtspStatusCode <= RtspStatusCode.OK ||
                        response.RtspStatusCode == RtspStatusCode.Found ||
                        response.RtspMethod == RtspMethod.REDIRECT)
                    {
                        //Determine if there is a new location
                        string newLocation = response.GetHeader(RtspHeaders.Location);

                        if (false == string.IsNullOrWhiteSpace(newLocation))
                        {
                            newLocation = newLocation.Trim();
                        }

                        //We start at our location
                        Uri baseUri = CurrentLocation;

                        //Get the contentBase header
                        string contentBase = response[RtspHeaders.ContentBase];

                        //If it was present
                        if (false == string.IsNullOrWhiteSpace(contentBase) &&
                            //Try to create it from the string
                            Uri.TryCreate(contentBase, UriKind.RelativeOrAbsolute, out baseUri))
                        {
                            //If it was not absolute
                            if (false == baseUri.IsAbsoluteUri)
                            {
                                //Try to make it absolute and if not try to raise an exception
                                if (false == Uri.TryCreate(CurrentLocation, baseUri, out baseUri))
                                {
                                    TaggedExceptionExtensions.RaiseTaggedException(contentBase, "See Tag. Can't parse ContentBase header.");
                                }
                            }

                            //The new location is given by
                            newLocation = baseUri.ToString();
                        }

                        Uri parsedLocation;

                        //Try to parse it if not null or empty
                        if (false == string.IsNullOrWhiteSpace(newLocation) &&
                            Uri.TryCreate(baseUri, newLocation, out parsedLocation) &&
                            parsedLocation != CurrentLocation) // and not equal the existing location
                        {

                            if (parsedLocation.IsAbsoluteUri &&
                                parsedLocation.OriginalString.Last() == (char)ASCII.ForwardSlash)
                            {
                                //Redirect to the Location by setting Location. (Allows a new host)
                                m_CurrentLocation = new Uri(parsedLocation.OriginalString.Substring(0, parsedLocation.OriginalString.Length - 1));
                            }
                            else
                            {
                                m_CurrentLocation = parsedLocation;
                            }

                            //Send a new describe
                            if (response.RtspStatusCode == RtspStatusCode.Found || response.RtspMethod == RtspMethod.REDIRECT)
                            {
                                //the old response would possibly leak.
                                response.IsPersistent = false;

                                return response = await SendDescribe() ?? response;
                            }
                        }
                    }

                    string contentType = response[RtspHeaders.ContentType];

                    //Handle any not ok response (allow Continue)
                    //Handle MultipleChoice for Moved or ContentType...
                    if (response.RtspStatusCode >= RtspStatusCode.MultipleChoices && false == string.IsNullOrEmpty(contentType) && string.Compare(contentType.TrimStart(), SessionDescription.MimeType, StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        TaggedExceptionExtensions.RaiseTaggedException(response.RtspStatusCode, "Unable to describe media. The StatusCode is in the Tag property.");
                    }

                    //Try to create a session description even if there was no contentType so long as one was not specified against sdp.
                    m_SessionDescription = new SessionDescription(response.Body);

                    //No longer being used.
                    describe.IsPersistent = false;
                }
            }
            catch (TaggedException<RtspClient>)
            {
                throw;
            }
            catch (TaggedException<SessionDescription> sde)
            {
                TaggedExceptionExtensions.RaiseTaggedException(this, "Unable to describe media, Session Description Exception Occured.", sde);
            }
            catch (Exception ex)
            {
                if (ex is ITaggedException) throw ex;
                TaggedExceptionExtensions.RaiseTaggedException(this, "An error occured", ex);
            }

            return response;
        }

        /// <summary>
        /// Sends a request which will remove the session given from the server using a TEARDOWN * request.
        /// </summary>
        /// <param name="sessionId">The sessionId to remove, if null the current <see cref="SessionId"/> will be used if possible.</param>
        /// <param name="closeConnection">Indicates if the `Connection` header of the request should be set to 'Close'</param>
        /// <returns></returns>
        public virtual async Task<RtspMessage> RemoveSession(string sessionId, bool closeConnection = false)
        {
            using (var teardown = new RtspMessage(RtspMessageType.Request))
            {
                teardown.RtspMethod = RtspMethod.TEARDOWN;

                if (closeConnection) teardown.SetHeader(RtspHeaders.Connection, "Close");

                sessionId = sessionId ?? m_SessionId;

                if (false == string.IsNullOrWhiteSpace(sessionId)) teardown.SetHeader(RtspHeaders.Session, sessionId);

                //Calling on stopping here indicates all sessions end...
                //SHould get the session by id and then use it's media description in the event.
                OnStopping();

                try { return await SendRtspMessage(teardown); }
                finally { m_SessionId = null; }
            }
        }

        public async Task<RtspMessage> SendTeardown(MediaDescription mediaDescription = null, bool disconnect = false, bool force = false)
        {
            RtspMessage response = null;

            //Check if the session supports pausing a specific media item
            if (mediaDescription != null && false == SessionDescription.SupportsAggregateMediaControl(CurrentLocation)) throw new InvalidOperationException("The SessionDescription does not allow aggregate control.");

            //only send a teardown if not forced and the client is playing
            if (false == force && false == IsPlaying) return response;

            try
            {
                //If there is a client then stop the flow of this media now with RTP
                if (m_RtpClient != null)
                {
                    //Send a goodbye for all contexts if the mediaDescription was not given
                    if (mediaDescription == null)
                    {
                        if (false == SharesSocket)
                            await m_RtpClient.Deactivate().ConfigureAwait(false);
                        else
                            await m_RtpClient.SendGoodbyes().ConfigureAwait(false);
                    }
                    else//Find the context for the description
                    {
                        //Get a context
                        RtpClient.TransportContext context = m_RtpClient.GetContextForMediaDescription(mediaDescription);

                        //If context was determined then send a goodbye
                        if (context != null)
                        {
                            //Send a goodbye now (but still allow reception)
                            await m_RtpClient.SendGoodbye(context);

                            //Remove the reference
                            context = null;
                        }
                    }
                }

                //Keep track of whats playing
                if (mediaDescription == null)
                {
                    m_Playing.Clear();

                    //LeaveOpen = false;
                }
                else m_Playing.Remove(mediaDescription);

                //The media is stopping now.
                OnStopping(mediaDescription);

                //Return the result of the Teardown
                using (var teardown = new RtspMessage(RtspMessageType.Request)
                {
                    RtspMethod = RtspMethod.TEARDOWN,
                    Location = mediaDescription != null ? mediaDescription.GetAbsoluteControlUri(CurrentLocation, SessionDescription) : CurrentLocation
                })
                {
                    //Set the close header if disconnecting
                    if (disconnect) teardown.SetHeader(RtspHeaders.Connection, "close");

                    //Send the request and if not closing the connecting then wait for a response
                    return await SendRtspMessage(teardown, true, false == disconnect);
                }
            }
            catch (TaggedException<RtspClient>)
            {
                return response;
            }
            catch
            {
                throw;
            }
            finally
            {
                //Ensure the sessionId is invalided when no longer playing if not forced
                if (false == force && false == IsPlaying) m_SessionId = null;
            }
        }

        public async Task<RtspMessage> SendSetup(MediaDescription mediaDescription)
        {
            if (mediaDescription == null) throw new ArgumentNullException("mediaDescription");

            return await SendSetup(mediaDescription.GetAbsoluteControlUri(CurrentLocation, SessionDescription), mediaDescription);
        }

        internal async Task<RtspMessage> SendSetup(Uri location, MediaDescription mediaDescription)
        {
            if (location == null) throw new ArgumentNullException("location");

            if (mediaDescription == null) throw new ArgumentNullException("mediaDescription");

            bool needsRtcp = true, multiplexing = false;

            try
            {
                using (RtspMessage setup = new RtspMessage(RtspMessageType.Request)
                {
                    RtspMethod = RtspMethod.SETUP,
                    Location = location ?? CurrentLocation
                })
                {
                    //Values in the header we need
                    int clientRtpPort = -1, clientRtcpPort = -1,
                        serverRtpPort = -1, serverRtcpPort = -1,
                        localSsrc = 0,
                        remoteSsrc = 0;

                    IPAddress sourceIp = m_RtspSocket.AddressFamily == AddressFamily.InterNetwork ? IPAddress.Any : IPAddress.IPv6Any,
                        destinationIp = sourceIp;

                    string mode;

                    bool interleaved = true;
                    if (interleaved == false)
                    {
                        throw new Exception("必须使用TCP");
                    }

                    byte dataChannel = 0, controlChannel = 1;

                    //Todo, get this from the connection address.
                    int ttl = 255;

                    //8? should be determine by rtcp enabled and the type of packets, rtcp 4, rtp 12
                    int minimumPacketSize = 8, maximumPacketSize = (ushort)m_Buffer.Count;

                    //This NEEDS TO BE BASED AROUND THE LIMITS OF THE NETWORK MTU AND MSS
                    if (SendBlocksize) setup.SetHeader(RtspHeaders.Blocksize, m_Buffer.Count.ToString());

                    //Required: header (RequiredFeatures)
                    if (RequiredFeatures.Count > 0 && false == setup.ContainsHeader(RtspHeaders.Require))
                    {
                        setup.SetHeader(RtspHeaders.Require, string.Join(SessionDescription.SpaceString, RequiredFeatures));
                    }

                    //If there is already a RtpClient with at-least 1 TransportContext
                    if (false == IDisposedExtensions.IsNullOrDisposed(m_RtpClient))
                    {
                        RtpClient.TransportContext lastContext = m_RtpClient.GetTransportContexts().Last();

                        if (lastContext != null && lastContext.IsActive)
                        {
                            setup.SetHeader(RtspHeaders.Transport, RtspHeaders.TransportHeader(RtpClient.RtpAvpProfileIdentifier + "/TCP", localSsrc != 0 ? localSsrc : (int?)null, null, null, null, null, null, true, false, null, true, dataChannel = (byte)(lastContext.DataChannel + 2), (needsRtcp ? (byte?)(controlChannel = (byte)(lastContext.ControlChannel + 2)) : null), null));
                        }
                        else
                        {
                            setup.SetHeader(RtspHeaders.Transport, RtspHeaders.TransportHeader(RtpClient.RtpAvpProfileIdentifier + "/TCP", localSsrc != 0 ? localSsrc : (int?)null, null, null, null, null, null, true, false, null, true, dataChannel, (needsRtcp ? (byte?)controlChannel : null), null));
                        }
                    }

                Setup:
                    //Get the response for the setup
                    RtspMessage response = await SendRtspMessage(setup);

                    //Get a session ready                    
                    RtspSession session;

                    //Create a RtspSession if there is not already one assoicated with the current sessionId
                    if (false == m_Sessions.TryGetValue(SessionId, out session))
                    {
                        //Create a session
                        session = new RtspSession(setup, response)
                        {
                            EnableKeepAliveRequest = false == DisableKeepAliveRequest,
                            ControlLocation = location
                        };

                        //Only if there was a sessionId found
                        if (false == string.IsNullOrWhiteSpace(session.SessionId))
                        {
                            //Add the session created
                            m_Sessions.Add(SessionId, session);
                        }
                    }

                    if (null == response)
                        return response;

                    //Response not OK， Try Again
                    if (response.RtspStatusCode != RtspStatusCode.OK)
                    {
                        if (response.RtspStatusCode == RtspStatusCode.SessionNotFound && //If the session was not found
                             false == string.IsNullOrWhiteSpace(m_SessionId))//And there IS an existing session id) //And setup has not already been attempted two times.
                        {
                            //Erase the old session id
                            m_SessionId = string.Empty;

                            //Attempt the setup again
                            return await SendSetup(location, mediaDescription);
                        }
                        else //Not Ok and not Session Not Found
                        {
                            //If there was an initial location and that location's host is different that the current location's host
                            if (m_InitialLocation != null && location.Host != m_InitialLocation.Host)
                            {
                                //Try to use the old location
                                location = mediaDescription.GetAbsoluteControlUri(m_InitialLocation, SessionDescription);

                                goto Setup;
                            }

                            return response;
                        }
                    }

                    string blockSize = response[RtspHeaders.Blocksize];

                    if (false == string.IsNullOrWhiteSpace(blockSize))
                    {
                        //Extract the value (Should account for ';' in some way)
                        blockSize = ASCII.ExtractNumber(blockSize.Trim());

                        try
                        {
                            //Parse it...
                            maximumPacketSize = int.Parse(blockSize, System.Globalization.NumberStyles.Integer);

                            //If the packets cannot fit in the buffer
                            if (maximumPacketSize > m_Buffer.Count)
                            {
                                //Try to allow processing
                                TaggedExceptionExtensions.RaiseTaggedException(maximumPacketSize, "Media Requires a Larger Buffer. (See Tag for value)");
                            }
                        }
                        catch (Exception ex)
                        {
                            TaggedExceptionExtensions.RaiseTaggedException(response, "BlockSize of the response needs consideration. (See Tag for response)", ex);
                        }
                    }

                NoResponse:

                    //We SHOULD have a valid TransportHeader in the response
                    //Get the transport header from the response if present.
                    session.TransportHeader = response != null ? response[RtspHeaders.Transport] : null;

                    //If there was no return transport header then we don't know what ports to utilize for reception.
                    if (string.IsNullOrWhiteSpace(session.TransportHeader))
                    {
                        //Discover them when receiving from the host
                        serverRtpPort = 0;

                        serverRtcpPort = 0;
                    }
                    else
                    {
                        //Check for the RTP token to ensure the underlying tranport is supported.
                        //Eventually any type such as RAW etc will be supported.
                        if (false == session.TransportHeader.Contains("RTP")
                        ||
                        false == RtspHeaders.TryParseTransportHeader(session.TransportHeader,
                        out remoteSsrc, out sourceIp, out serverRtpPort, out serverRtcpPort, out clientRtpPort, out clientRtcpPort,
                        out interleaved, out dataChannel, out controlChannel, out mode, out destinationIp, out ttl))
                            TaggedExceptionExtensions.RaiseTaggedException(this, "Cannot setup media, Invalid Transport Header in Rtsp Response: " + session.TransportHeader);
                    }

                    //Create the context (determine if the session rangeLine may also be given here, if it gets parsed once it doesn't need to be parsed again)
                    RtpClient.TransportContext created = null;

                    //If there is a client which is not disposed
                    if (m_RtpClient != null && false == m_RtpClient.IsDisposed)
                    {
                        //Obtain a context via the given data channel or control channel
                        created = m_RtpClient.GetContextByChannels(dataChannel, controlChannel);

                        //If the control channel is the same then just update the socket used by the context.
                        if (created != null &&
                            false == created.IsDisposed)
                        {
                            //created's Rtp and Rtcp Socket could be changed right here...
                            if (m_RtspSocket != null)
                                await created.Initialize(m_RtspSocket, m_RtspSocket);

                            created.ApplicationContext = SessionId;
                            session.Context = created;
                            created = null;

                            return response;
                        }
                    }

                    //If a context was not already created
                    if (created == null || created.IsDisposed)
                    {
                        //Create the context if required.. (Will be created with Sdp Address)
                        created = await RtpClient.TransportContext.FromMediaDescription(SessionDescription, dataChannel, controlChannel, mediaDescription, needsRtcp, remoteSsrc, remoteSsrc != 0 ? 0 : 2, null, sourceIp);

                        //Set the identity to what we indicated to the server.
                        created.SynchronizationSourceIdentifier = localSsrc;

                        created.MinimumPacketSize = minimumPacketSize;
                        created.MaximumPacketSize = maximumPacketSize;
                    }

                    //If there is not a client
                    if (m_RtpClient == null || m_RtpClient.IsDisposed)
                    {
                        //Create a Duplexed reciever using the RtspSocket sharing the RtspClient's buffer's properties
                        m_RtpClient = new RtpClient(new MemorySegment(m_Buffer));
                        m_RtpClient.InterleavedData += ProcessInterleaveData;
                    }

                    //If the source address contains the NAT IpAddress or the source is the same then just use the source.
                    //if (IPAddress.Equals(sourceIp, ((IPEndPoint)m_RemoteRtsp).Address))
                    //{
                    //Create from the existing socket (may need reuse port)
                    await created.Initialize(m_RtspSocket, m_RtspSocket);
                    //}
                    //else
                    //{
                    //    throw new Exception("created.Initialize(multicast ? SocketExtensions.GetFirstMulticastIPAddress(sourceIp.AddressFamily) : SocketExtensions.GetFirstUnicastIPAddress(sourceIp.AddressFamily)");
                    //}

                    //if a context was created add it
                    if (created != null)
                    {
                        await m_RtpClient.AddContext(created, false == multiplexing, false == multiplexing);

                        //Store the sessionId in the ApplicationContext.
                        created.ApplicationContext = SessionId;

                        //Store the context in the session
                        session.Context = created;
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                TaggedExceptionExtensions.RaiseTaggedException(this, "Unable to setup media. See InnerException", ex);

                return m_LastTransmitted;
            }
        }

        protected virtual async void MonitorProtocol(object state = null)
        {
            if (m_ProtocolMonitor == null) return;

            bool keepAlives = DisableKeepAliveRequest;

            //If not already Disposed and the protocol was not already specified as or configured to TCP
            if (false == IsDisposed &&  //And
                IsPlaying) //Still playing
            {
                //Monitor the protocol for incoming messages
                if (false == SharesSocket && false == InUse)
                {
                    DisableKeepAliveRequest = true;

                    using (var response = await SendRtspMessage(null, false, true, m_MaximumTransactionAttempts))
                    {
                        if (response != null)
                            LoggerInstance.Debug(ToString() + "@MonitorProtocol:  Received =>" + response.ToString());
                    }

                    DisableKeepAliveRequest = keepAlives;
                }

                //If protocol switch is still allowed AND still playing
                if (false == IsDisposed && AllowAlternateTransport && IsPlaying && m_RtpProtocol != ProtocolType.Tcp)
                {
                    //Filter the contexts which have received absolutely NO data.
                    var contextsWithoutFlow = Client.GetTransportContexts().Where(tc => tc != null &&
                        m_Playing.Contains(tc.MediaDescription) &&
                        tc.TotalBytesReceieved == 0 && tc.TotalPacketsSent == 0
                        && tc.TimeActive > tc.ReceiveInterval);

                    //If there are any context's which are not flowing but are playing
                    if (contextsWithoutFlow.Count() >= m_Playing.Count)// and the amount of them is greater than or equal to what the rtsp client is playing
                    {
                        try
                        {
                            //If the client has not already switched to Tcp
                            if (m_RtpProtocol != ProtocolType.Tcp)
                            {
                                //Ensure Tcp protocol
                                m_RtpProtocol = ProtocolType.Tcp;
                            }
                            else
                            {
                                //Ensure Udp protocol
                                m_RtpProtocol = ProtocolType.Udp;
                            }

                            //Stop sending them for now
                            if (false == keepAlives) DisableKeepAliveRequest = true;

                            //Wait for any existing request to complete
                            while (InUse) m_InterleaveEvent.WaitOne(TimeSpanExtensions.OneTick);

                            LoggerInstance.Debug(ToString() + "@MonitorProtocol: StopPlaying");

                            //Stop all playback
                            StopPlaying(false);

                            //If this is not done then when StartPlaying is called again 
                            //SendSetup will may find a Context which exists with the same ssrc.
                            //It should be determined then if the context can be updated or not with the new socket.
                            //It would only save a small amount of memory
                            m_RtpClient.DisposeAndClearTransportContexts();

                            //Cache
                            while (IsPlaying || InUse)
                            {
                                m_InterleaveEvent.WaitOne(TimeSpanExtensions.OneTick);

                                LoggerInstance.Debug(ToString() + "@MonitorProtocol: Waiting for IsPlaying to be false.");
                            }

                            LoggerInstance.Debug(ToString() + "@MonitorProtocol: StartPlaying");

                            //Start again
                            await StartPlaying();

                            //Restore value
                            DisableKeepAliveRequest = keepAlives;
                        }
                        catch (Exception ex)
                        {
                            LoggerInstance.Debug(ToString() + "@MonitorProtocol: " + ex.Message);
                        }
                    }
                }
            }

            //If there is still a timer change it based on the last messages round trip time, should be relative to all messages...
            if (false == IsDisposed && m_ProtocolMonitor != null)
                try { m_ProtocolMonitor.Change(m_ConnectionTime.Add(LastMessageRoundTripTime), TimeSpanExtensions.InfiniteTimeSpan); }
                catch (Exception ex)
                {
                    LoggerInstance.Debug(ToString() + "@MonitorProtocol: " + ex.Message);
                }
        }

        public async Task<RtspMessage> SendPlay(MediaDescription mediaDescription, TimeSpan? startTime = null, TimeSpan? endTime = null, string rangeType = "npt")
        {
            if (mediaDescription == null) throw new ArgumentNullException("mediaDescription");

            //Check if the session supports pausing a specific media item
            if (false == SessionDescription.SupportsAggregateMediaControl(CurrentLocation)) throw new InvalidOperationException("The SessionDescription does not allow aggregate control.");

            var context = Client.GetContextForMediaDescription(mediaDescription);

            if (context == null) throw new InvalidOperationException("The given mediaDescription has not been SETUP.");

            //Check if the media was previsouly playing
            if (mediaDescription != null && false == m_Playing.Contains(mediaDescription))
            {
                //Keep track of whats playing
                m_Playing.Add(mediaDescription);

                //Raise an event now.
                OnPlaying(mediaDescription);
            }

            //Send the play request
            return await SendPlay(mediaDescription.GetAbsoluteControlUri(CurrentLocation, SessionDescription), startTime ?? context.MediaStartTime, endTime ?? context.MediaEndTime, rangeType);
        }

        public async Task<RtspMessage> SendPlay(Uri location = null, TimeSpan? startTime = null, TimeSpan? endTime = null, string rangeType = "npt", bool force = false)
        {
            try
            {
                using (RtspMessage play = new RtspMessage(RtspMessageType.Request)
                    {
                        RtspMethod = RtspMethod.PLAY,
                        Location = location ?? CurrentLocation
                    })
                {
                    if (startTime.HasValue || endTime.HasValue)
                        play.SetHeader(RtspHeaders.Range, RtspHeaders.RangeHeader(startTime, endTime, rangeType));
                    else if (false == string.IsNullOrWhiteSpace(rangeType))
                    {
                        play.SetHeader(RtspHeaders.Range, rangeType);
                    }

                    play.SetHeader(RtspHeaders.Speed, RtspHeaders.SpeedHeader(0.5));

                    var response = await SendRtspMessage(play);

                    //Handle allowed problems with reception of the play response if already playing
                    if (false == IsPlaying && (response == null || response != null && response.MessageType == RtspMessageType.Response))
                    {
                        //No response or invalid range.
                        if (response == null || response.RtspStatusCode == RtspStatusCode.InvalidRange)
                        {
                            play.RemoveHeader(RtspHeaders.Range);
                            play.RemoveHeader(RtspHeaders.CSeq);
                            play.RemoveHeader(RtspHeaders.Timestamp);
                            return await SendRtspMessage(play);
                        }
                        else if (response.RtspStatusCode <= RtspStatusCode.OK)
                        {
                            if (false == IDisposedExtensions.IsNullOrDisposed(m_RtpClient))
                            {
                                //Connect the client now.
                                m_RtpClient.Activate();
                            }

                            //Get the rtp-info header
                            string rtpInfo = response[RtspHeaders.RtpInfo];

                            string[] rtpInfos;

                            //If parsing of the header succeeded
                            if (RtspHeaders.TryParseRtpInfo(rtpInfo, out rtpInfos))
                            {
                                foreach (string rtpInfoValue in rtpInfos)
                                {
                                    Uri uri;
                                    int? rtpTime;
                                    int? seq;
                                    int? ssrc;

                                    //If any value which was needed was found.
                                    if (RtspHeaders.TryParseRtpInfo(rtpInfoValue, out uri, out seq, out rtpTime, out ssrc))
                                    {
                                        if (ssrc.HasValue)
                                        {
                                            //Get the context created with the ssrc defined above
                                            RtpClient.TransportContext context = m_RtpClient.GetContextBySourceId(ssrc.Value);

                                            //If that context is not null then allow it's ssrc to change now.
                                            if (context != null)
                                            {
                                                context.RemoteSynchronizationSourceIdentifier = ssrc.Value;

                                                if (seq.HasValue) context.RecieveSequenceNumber = seq.Value;

                                                if (rtpTime.HasValue) context.RtpTimestamp = rtpTime.Value;

                                                context = null;
                                            }
                                        }
                                        else if (uri != null)
                                        {
                                            //Get the context created with from the media description with the same resulting control uri
                                            RtpClient.TransportContext context = m_RtpClient.GetTransportContexts().FirstOrDefault(tc => tc.MediaDescription.GetAbsoluteControlUri(CurrentLocation, SessionDescription) == uri);

                                            //If that context is not null then allow it's ssrc to change now.
                                            if (context != null)
                                            {
                                                if (ssrc.HasValue) context.RemoteSynchronizationSourceIdentifier = ssrc.Value;
                                                if (seq.HasValue) context.RecieveSequenceNumber = seq.Value;
                                                if (rtpTime.HasValue) context.RtpTimestamp = rtpTime.Value;

                                                context = null;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    return response;
                }
            }
            catch { throw; }
        }

        public async Task<RtspMessage> SendPause(MediaDescription mediaDescription = null, bool force = false)
        {
            //Ensure media has been setup unless forced.
            if (mediaDescription != null && false == force)
            {
                //Check if the session supports pausing a specific media item
                if (false == SessionDescription.SupportsAggregateMediaControl(CurrentLocation)) throw new InvalidOperationException("The SessionDescription does not allow aggregate control.");

                //Get a context for the media
                var context = Client.GetContextForMediaDescription(mediaDescription);

                //If there is no context then throw an exception.
                if (context == null) throw new InvalidOperationException("The given mediaDescription has not been SETUP.");

                context = null;
            }

            //Keep track of whats playing
            if (mediaDescription == null) m_Playing.Clear();
            else m_Playing.Remove(mediaDescription);

            //Fire the event now
            OnPausing(mediaDescription);

            //Send the pause request, determining if the request is for all media or just one.
            return await SendPause(mediaDescription != null ? mediaDescription.GetAbsoluteControlUri(CurrentLocation, SessionDescription) : CurrentLocation, force);
        }

        public async Task<RtspMessage> SendPause(Uri location = null, bool force = false)
        {
            //If the server doesn't support it
            if (false == SupportedMethods.Contains(RtspMethod.PAUSE.ToString()) && false == force) throw new InvalidOperationException("Server does not support PAUSE.");

            //if (!Playing) throw new InvalidOperationException("RtspClient is not Playing.");
            using (RtspMessage pause = new RtspMessage(RtspMessageType.Request)
            {
                RtspMethod = RtspMethod.PAUSE,
                Location = location ?? CurrentLocation
            })
            {
                return await SendRtspMessage(pause);
            }
        }

        internal async void SendKeepAliveRequest(object state)
        {
            bool wasPlaying = false, wasConnected = false;

            try
            {
                //Thrown an exception if IsDisposed
                if (IsDisposed || m_KeepAliveTimer == null) return;

                wasPlaying = IsPlaying;

                //Save the state of the connection
                wasConnected = IsConnected;

                //If the keep alive request feature is not disabled and the session times out if not kept alive
                if (wasPlaying && IsPlaying &&
                    false == DisableKeepAliveRequest &&
                    m_RtspSessionTimeout > TimeSpan.Zero)
                {
                    //Don't send a keep alive if the stream is ending before the next keep alive would be sent.
                    if (EndTime.HasValue && EndTime.Value != TimeSpanExtensions.InfiniteTimeSpan &&
                        EndTime.Value - ((DateTime.UtcNow - m_StartedPlaying.Value)) <= m_RtspSessionTimeout) return;

                    while (InUse) m_InterleaveEvent.WaitOne(TimeSpanExtensions.OneTick);

                    DisableKeepAliveRequest = true;

                    foreach (var session in m_Sessions)
                    {
                        //If the session itself doesn't support keep alive then continue.
                        if (false == session.Value.EnableKeepAliveRequest) continue;

                        if (SupportedMethods.Contains(RtspMethod.GET_PARAMETER.ToString()))
                        {
                            LoggerInstance.Info("Send KeepAlive Heart Thread ：");
                            using (var response = await SendGetParameter(null, null, session.Value.SessionId, false))
                            {
                                if (response != null)
                                {
                                    LoggerInstance.Info("Get Response KeepAlive Heart Thread ：" + response.ToString());
                                }
                            }
                        }
                        else if (SupportedMethods.Contains(RtspMethod.OPTIONS.ToString())) //If at least options is supported
                        {
                            using (await SendOptions(session.Value.ControlLocation == RtspMessage.Wildcard, session.Value.SessionId)) ;
                        }
                        else if (SupportedMethods.Contains(RtspMethod.PLAY.ToString())) //If at least PLAY is supported
                        {
                            using (await SendPlay())
                            {
                            }
                        }
                    }

                    DisableKeepAliveRequest = false;
                }

                //Only perform these actions if playing anything.
                if (wasPlaying)
                {
                    //Raise events for ended media.
                    foreach (var context in Client.GetTransportContexts())
                    {
                        if (context == null || context.IsDisposed || context.IsContinious || context.TimeReceiving < context.MediaEndTime) continue;

                        //Remove from the playing media and if it was contained raise an event.
                        if (m_Playing.Remove(context.MediaDescription)) OnStopping(context.MediaDescription);
                    }

                    bool aggregateControl = SessionDescription.SupportsAggregateMediaControl(CurrentLocation);

                    //Iterate the played items looking for ended media.
                    for (int i = 0, e = m_Playing.Count; i < e; ++i)
                    {
                        if (e > m_Playing.Count) break;

                        var mediaDescription = m_Playing[i];

                        //Get a context
                        var context = Client.GetContextForMediaDescription(mediaDescription);

                        //If there is a context ensure it has not ended and has recieved data within the context receive interval.
                        if (context == null ||
                            false == context.IsDisposed ||
                            context.Goodbye == null ||
                            true == context.IsContinious ||
                            context.TimeSending < context.MediaEndTime) continue;

                        //Teardown the media if the session supports AggregateControl
                        //(Todo, Each context may have it's own sessionId)
                        //Also the Server may have already stopped the media...
                        if (aggregateControl && m_Playing.Contains(mediaDescription)) using (SendTeardown(mediaDescription, true)) ;
                        else if (m_Playing.Remove(mediaDescription))
                        {//Otherwise Remove from the playing media and if it was contained raise an event.
                            OnStopping(mediaDescription);
                        }

                        //If there was a context for the media ensure it is removed and disposed from the underlying transport.
                        if (context != null)
                        {
                            Client.TryRemoveContext(context);
                            context.Dispose();
                            context = null;
                        }
                    }


                    //Ensure media is still flowing if still playing otherwise raise the stopping event.
                    if (IsPlaying)
                        await EnsureMediaFlows();
                    else if
                        (wasPlaying) OnStopping(); //Ensure not already raised?
                }

                //Determine next time to send a keep alive
                if (m_KeepAliveTimer != null && IsPlaying)
                {
                    //Todo, Check if the media will end before the next keep alive is due before sending.

                    if (m_LastMessageRoundTripTime < m_RtspSessionTimeout)
                        m_KeepAliveTimer.Change(TimeSpan.FromTicks(TimeSpan.FromSeconds(m_RtspSessionTimeout.TotalSeconds - 1).Subtract(m_LastMessageRoundTripTime + m_ConnectionTime).Ticks / 3), TimeSpanExtensions.InfiniteTimeSpan);
                }

            }
            catch (Exception ex)
            {
                LoggerInstance.Debug(ToString() + "@SendKeepAlive: " + ex.Message);
            }
        }

        public async Task EnsureMediaFlows()
        {

            if (InUse) return;

            DisableKeepAliveRequest = true;

            //If not waiting to switch protocols
            if (m_ProtocolMonitor == null && IsPlaying)
            {

                //If not playing anymore do nothing
                if (EndTime != TimeSpanExtensions.InfiniteTimeSpan &&
                    DateTime.UtcNow - m_StartedPlaying.Value > EndTime)
                {
                    StopPlaying();

                    return;
                }

                //Determine what contexts are playing and have set a goodbye
                var contextsWithGoodbye = Client.GetTransportContexts().Where(tc => tc != null &&
                    m_Playing.Contains(tc.MediaDescription) &&
                    tc.Goodbye != null);

                //If there are such contexts
                if (m_InterleaveEvent.WaitOne(0) && IsPlaying && contextsWithGoodbye.Any())
                {
                    //If the server doens't support pause then we cant pause.
                    bool supportPause = SupportedMethods.Contains(RtspMethod.PAUSE.ToString());

                    //If any media was pausedOrStopped.
                    bool pausedOrStoppedAnything = false;

                    //If we cannot stop a single media item we will set this to true.
                    bool stopAll = false == SessionDescription.SupportsAggregateMediaControl(CurrentLocation);

                    //Iterate all inactive contexts.
                    if (false == stopAll) foreach (var context in contextsWithGoodbye.ToArray())
                        {
                            //Ensure still in playing
                            if (false == m_Playing.Contains(context.MediaDescription) || context.HasAnyRecentActivity) continue;

                            //Send a pause request if not already paused and the server supports PAUSE and there has been any activity on the context
                            if (supportPause)
                            {
                                //If not going to be playing anymore do nothing
                                if (context.TimeRemaining >= context.ReceiveInterval + m_LastMessageRoundTripTime + m_LastServerDelay) continue;

                                //If the context is not continious and there is no more time remaining do nothing
                                if (false == context.IsContinious && context.TimeRemaining <= TimeSpan.Zero) continue;

                                //Send the PAUSE request
                                using (var pauseResponse = await SendPause(context.MediaDescription))
                                {

                                    //If the paused request was not a sucess then it's probably due to an aggregate operation
                                    //Determine if we have to stop everything.
                                    if (pauseResponse == null || pauseResponse.RtspStatusCode <= RtspStatusCode.OK)
                                    {

                                        //Sometime the server doesn't respond to pause or the response gets lost.
                                        if (pauseResponse == null || pauseResponse.MessageType == RtspMessageType.Invalid)
                                        {
                                            //Wait up until the time another request is sent.
                                            m_InterleaveEvent.WaitOne(m_RtspSessionTimeout);

                                            //Need a way to be able to check the request's sequence number..

                                            //if(m_LastTransmitted != null && m_LastTransmitted.CSeq == pauseReqeust.SequnceNumber)

                                            stopAll = true;
                                        }
                                        else   //See if everything has to be stopped.
                                            stopAll = pauseResponse.RtspStatusCode == RtspStatusCode.AggregateOpperationNotAllowed;

                                        //If the request failed then nothing was paused yet
                                        pausedOrStoppedAnything = false;

                                        //Could move this logic to the SendPause method which would check the response status code before returning the response and then wouldn't raise the Pause event.

                                        //Ensure external state is observed
                                        m_Playing.Add(context.MediaDescription);

                                        OnPlaying(context.MediaDescription);
                                    }
                                }
                            }
                            else
                            {
                                //If not going to be playing anymore do nothing
                                if (context.TimeRemaining >= context.ReceiveInterval + m_LastMessageRoundTripTime + m_LastServerDelay) continue;

                                //If the context is not continious and there is no more time remaining do nothing
                                if (false == context.IsContinious && context.TimeRemaining <= TimeSpan.Zero) continue;

                                //We can't pause so STOP JUST THIS MEDIA
                                using (var teardownResponse = await SendTeardown(context.MediaDescription))
                                {
                                    //If the Teardown was not a success then it's probably due to an aggregate operation.
                                    //If the paused request was not a sucess then it's probably due to an aggregate operation
                                    //Determine if we have to stop everything.
                                    if (teardownResponse == null || teardownResponse.RtspStatusCode <= RtspStatusCode.OK)
                                    {

                                        //Sometime the server doesn't respond to pause or the response gets lost.
                                        if (teardownResponse == null || teardownResponse.MessageType == RtspMessageType.Invalid)
                                        {
                                            stopAll = true;
                                        }
                                        else   //See if everything has to be stopped.
                                            stopAll = teardownResponse.RtspStatusCode == RtspStatusCode.AggregateOpperationNotAllowed;

                                        //If the request failed then nothing was paused yet
                                        pausedOrStoppedAnything = false;

                                        //Could move this logic to the SendPause method which would check the response status code before returning the response and then wouldn't raise the Pause event.

                                        //Ensure external state is observed
                                        m_Playing.Add(context.MediaDescription);

                                        OnPlaying(context.MediaDescription);
                                    }
                                }
                            }

                            //If we have to stop everything and the server doesn't support pause then stop iterating.
                            if (stopAll) break;

                            //The media was paused or stopped, so play it again if anything was received
                            if (pausedOrStoppedAnything && context.TotalBytesReceieved > 0)
                            {
                                //Ensure the context state allows for sending again.
                                context.Goodbye = null;

                                //Try to play the media again
                                try { await Play(context.MediaDescription); }
                                catch
                                {
                                    //Ensure external state is observed, the media is still playing
                                    m_Playing.Add(context.MediaDescription);

                                    OnPlaying(context.MediaDescription);
                                }
                            }
                        }

                    //If everything needs to stop.
                    if (stopAll && IsPlaying &&
                        EndTime.HasValue &&
                        EndTime.Value != TimeSpanExtensions.InfiniteTimeSpan &&
                        //And there is enough time to attempt
                        DateTime.UtcNow - m_StartedPlaying.Value > EndTime.Value.Subtract(m_LastMessageRoundTripTime.Add(m_ConnectionTime.Add(m_LastServerDelay)))
                        && contextsWithGoodbye.All(tc => tc != null && false == tc.HasAnyRecentActivity))
                    {
                        if (supportPause)
                        {
                            //Pause all media
                            await Pause();

                            //Start playing again
                            await StartPlaying();
                        }
                        else
                        {
                            //If still connected
                            if (IsConnected)
                            {
                                //Just send a play to continue receiving whatever media is still sending.
                                using (await SendPlay())
                                {
                                }
                            }
                            else
                            {
                                //Stop playing everything
                                StopPlaying();

                                //Start playing everything
                                await StartPlaying();
                            }
                        }
                    }
                }
            }
            DisableKeepAliveRequest = false;
        }

        public async Task<RtspMessage> SendGetParameter(string body = null, string contentType = null, string sessionId = null, bool force = false)
        {
            //…Content-type: application/x-rtsp-packetpair for WMS

            //If the server doesn't support it
            if (false == SupportedMethods.Contains(RtspMethod.GET_PARAMETER.ToString()) && false == force) throw new InvalidOperationException("Server does not support GET_PARAMETER.");

            //Need a session id
            using (RtspMessage get = new RtspMessage(RtspMessageType.Request)
            {
                RtspMethod = RtspMethod.GET_PARAMETER,
                Location = CurrentLocation,
                Body = body ?? string.Empty
            })
            {
                if (false == string.IsNullOrWhiteSpace(contentType))
                    get.SetHeader(RtspHeaders.ContentType, contentType);

                if (false == string.IsNullOrWhiteSpace(sessionId))
                    get.SetHeader(RtspHeaders.Session, sessionId);

                return await SendRtspMessage(get);
            }
        }
        #endregion

        public override string ToString()
        {
            return string.Join(((char)ASCII.HyphenSign).ToString(), base.ToString(), InternalId);
        }

        #region IDisposable

        /// <summary>
        /// Stops sending any Keep Alive Immediately and calls <see cref="StopPlaying"/>.
        /// If the <see cref="RtpClient"/> is not null:
        /// Removes the <see cref="ProcessInterleaveData"/> event
        /// Disposes the RtpClient and sets it to null.
        /// Disposes and sets the Buffer to null.
        /// Disposes and sets the InterleavedEvent to null.
        /// Disposes and sets the m_LastTransmitted to null.
        /// Disposes and sets the <see cref="RtspSocket"/> to null if <see cref="LeaveOpen"/> allows.
        /// Removes connection times so <see cref="IsConnected"/> is false.
        /// Stops raising any events.
        /// Removes any <see cref="Logger"/>
        /// </summary>
        public async override void Dispose()
        {
            if (IsDisposed || false == ShouldDispose) return;

            if (m_ProtocolMonitor != null)
            {
                m_ProtocolMonitor.Dispose();

                m_ProtocolMonitor = null;
            }

            DisableKeepAliveRequest = true;

            await StopPlaying();



            if (m_RtpClient != null)
            {
                m_RtpClient.InterleavedData -= ProcessInterleaveData;

                if (false == m_RtpClient.IsDisposed)
                    m_RtpClient.Dispose();

                m_RtpClient = null;
            }

            //Finally set Disposed to true
            base.Dispose(true);

            if (m_Buffer != null)
            {
                m_Buffer.Dispose();
                m_Buffer = null;
            }

            if (m_LastTransmitted != null)
            {
                m_LastTransmitted.Dispose();
                m_LastTransmitted = null;
            }

            m_InterleaveEvent.Dispose();

            if (m_RtspSocket != null)
            {
                if (false == LeaveOpen) m_RtspSocket.Dispose();
                m_RtspSocket = null;
            }

            if (m_SessionDescription != null)
            {
                m_SessionDescription.Dispose();

                m_SessionDescription = null;
            }

            m_BeginConnect = m_EndConnect = null;

            OnConnect = null;
            OnDisconnect = null;
            OnStop = null;
            OnPlay = null;
            OnPause = null;
            OnRequest = null;
            OnResponse = null;
        }

        #endregion

        IEnumerable<Socket> ISocketReference.GetReferencedSockets()
        {
            if (IsDisposed) yield break;

            yield return m_RtspSocket;
        }
    }
}
