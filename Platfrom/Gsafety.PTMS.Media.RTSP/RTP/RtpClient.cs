﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.Media.RTSP.Common;
using Gsafety.PTMS.Media.RTSP.RTCP;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Gsafety.PTMS.Media.RTSP.Common.Extensions.Exception;
using Gsafety.PTMS.Media.RTSP.Common.Extensions.Linq;
using Gsafety.PTMS.Media.RTSP.Common.Extensions.TimeSpan;
using Gsafety.PTMS.Media.RTSP.Ntp;
using Gsafety.PTMS.Media.RTSP.Common.Extensions.Thread;
using Gsafety.PTMS.Media.RTSP.Extensions;
using Media;
using Gsafety.PTMS.Media.RTSP.Common.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Gsafety.PTMS.Media.Common.Loggers;
using Gsafety.PTMS.Media.RTSP.Sdp;
using Gsafety.PTMS.Media.RTSP.Sdp.Lines;

namespace Gsafety.PTMS.Media.RTSP.RTP
{
    /// <summary>
    /// Provides an implementation of the <see cref="http://tools.ietf.org/html/rfc3550"> Real Time Protocol </see>.
    /// A RtpClient typically allows one <see cref="System.Net.Socket"/> to communicate (via RTP) to another <see cref="System.Net.Socket"/> via <see cref="RtpClient.TransportContext"/>'s in which some <see cref="SessionDescription"/> has been created.
    /// </summary>
    public class RtpClient : BaseDisposable, IThreadReference
    {
        #region Constants / Statics
        public const string RtpProtcolScheme = "rtp", AvpProfileIdentifier = "avp", RtpAvpProfileIdentifier = "RTP/AVP";

        //Udp Hole Punch
        //Might want a seperate method for this... (WakeupRemote)
        //Most routers / firewalls will let traffic back through if the person from behind initiated the traffic.
        //Send some bytes to ensure the reciever is awake and ready... (SIP / RELOAD / ICE / STUN / TURN may have something specific and better)
        static byte[] WakeUpBytes = new byte[] { 0x70, 0x70, 0x70, 0x70 };

        //Choose better name,,, 
        //And depending on how memory is aligned 36 may be a palindrome
        internal const byte BigEndianFrameControl = 36;//, // ASCII => $,  Hex => 24  Binary => (00)100100

        /// <summary>
        /// Describes the size (in bytes) of the 
        /// [MAGIC , CHANNEL, {LENGTH}] octets which preceed any TCP RTP / RTCP data When multiplexing data on a single TCP port over RTSP.
        /// </summary>
        internal const int InterleavedOverhead = 4;
        //RTP/AVP/TCP Specifies only the Length bytes in network byte order. e.g. 2 bytes

        /// <summary>
        /// The default time assocaited with Rtcp report intervals for RtpClients. (Almost 5 seconds)
        /// </summary>
        public static readonly TimeSpan DefaultReportInterval = TimeSpan.FromSeconds(4.96);

        /// <summary>
        /// Read the RFC2326 amd RFC4751 Frame header.
        /// Returns the amount of bytes in the frame.
        /// Outputs the channel of the frame in the channel variable.
        /// </summary>
        /// <param name="buffer">The data containing the RFC4751 frame</param>
        /// <param name="offset">The offset in the </param>
        /// <param name="channel">The byte which will contain the channel if the reading succeeded</param>
        /// <param name="readFrameByte">Indicates if the frameByte should be read (RFC2326)</param>
        /// <param name="frameByte">Indicates the frameByte to read</param>
        /// <returns> -1 If the buffer does not contain a RFC2326 / RFC4751 frame at the offset given</returns>
        internal static int TryReadFrameHeader(byte[] buffer, int offset, out byte channel, byte? frameByte = BigEndianFrameControl, bool readChannel = true)
        {
            //Must be assigned
            channel = default(byte);

            if (buffer == null) return -1;

            //Todo, Native, Unsafe
            //If the buffer does not start with the magic byte this is not a RFC2326 frame, it could be a RFC4571 frame
            if (frameByte.HasValue && buffer[offset++] != frameByte) return -1; //goto ReadLengthOnly;

            //Todo, Native, Unsafe
            //Assign the channel if reading framed.
            if (readChannel) channel = buffer[offset++];

            //Return the result of reversing the Unsigned 16 bit integer at the offset
            var length = Binary.ReadU16(buffer, offset, Binary.IsLittleEndian);
            return length;
        }
        #endregion

        #region Nested Types TransportContext

        /// <summary>
        ///Contains the information and assets relevent to each stream in use by a RtpClient
        /// </summary>
        public class TransportContext : BaseDisposable, ISocketReference
        {
            #region Statics

            internal static byte[] CreateApplicationLayerFraming(TransportContext context)
            {
                //Determine  how many bytes, independent uses 2 where as rtsp uses 4

                //Determine if RFC4571 via the Connection line etc.

                int size = InterleavedOverhead;

                byte[] result = new byte[size];

                return result;
            }

            internal static void ConfigureRtpRtcpSocket(Socket socket) //,ILogging = null
            {
                if (socket == null) throw new ArgumentNullException("socket");

                //Don't buffer sending
                ExceptionExtensions.ResumeOnError(() => socket.SendBufferSize = 0);

                if (socket.ProtocolType == ProtocolType.Tcp)
                {
                    //Don't buffer receiving
                    ExceptionExtensions.ResumeOnError(() => socket.ReceiveBufferSize = 0);
                }
                else if (socket.ProtocolType == ProtocolType.Udp)
                {
                    //Set max ttl for slower networks
                    ExceptionExtensions.ResumeOnError(() => socket.Ttl = 255);
                }
            }

            public static async Task<TransportContext> FromMediaDescription(SessionDescription sessionDescription, byte dataChannel, byte controlChannel, MediaDescription mediaDescription, bool rtcpEnabled = true, int remoteSsrc = 0, int minimumSequentialpackets = 2, IPAddress localIp = null, IPAddress remoteIp = null, int? rtpPort = null, int? rtcpPort = null, bool connect = false, Socket existingSocket = null, Action<Socket> configure = null)
            {
                //Must have a mediaDescription
                if (mediaDescription == null) throw new ArgumentNullException("mediaDescription");

                //If there is no sdp there must be a local and remoteIp
                if (sessionDescription == null && (localIp == null || remoteIp == null)) throw new InvalidOperationException("Must have a sessionDescription or the localIp and remoteIp cannot be established.");

                //If no remoteIp was given attempt to parse it from the sdp
                if (remoteIp == null)
                {
                    SessionDescriptionLine cLine = mediaDescription.ConnectionLine;

                    //Try the sesion level if the media level doesn't have one
                    if (cLine == null) cLine = sessionDescription.ConnectionLine;

                    //Attempt to parse the IP, if failed then throw an exception.
                    if (cLine == null || false == IPAddress.TryParse(new SessionConnectionLine(cLine).Host, out remoteIp))
                        throw new InvalidOperationException("Cannot determine remoteIp from ConnectionLine");
                }

                if (localIp != null && localIp.AddressFamily != remoteIp.AddressFamily)
                    throw new InvalidOperationException("local and remote address family must match, please create an issue and supply a capture.");

                //If no remoteSsrc was given then check for one
                if (remoteSsrc == 0)
                {
                    SessionDescriptionLine ssrcLine = mediaDescription.SsrcLine;

                    //To use typed line

                    if (ssrcLine != null)
                    {
                        string part = ssrcLine.GetPart(1);

                        if (false == string.IsNullOrWhiteSpace(part))
                        {
                            remoteSsrc = part[0] == '-' ? (int)uint.Parse(part) : int.Parse(part);
                        }
                    }
                }

                //Create the context
                TransportContext tc = new TransportContext(dataChannel, controlChannel, RFC3550.Random32(SourceDescriptionReport.PayloadType), mediaDescription,
                    rtcpEnabled, remoteSsrc, minimumSequentialpackets);

                int reportReceivingEvery = 0,
                    reportSendingEvery = 0,
                    asData = 0;

                //If rtcp is enabled
                if (rtcpEnabled)
                {
                    //Set to the default interval
                    reportSendingEvery = reportReceivingEvery = (int)DefaultReportInterval.TotalMilliseconds;

                    //Todo should be using the BandwidthLine type and IsDisabled property of instance
                    //Then would have access to BandwidthTypeString on instance also.

                    //If any bandwidth lines were parsed
                    if (SessionBandwidthLine.TryParseBandwidthDirectives(mediaDescription, out reportReceivingEvery, out reportSendingEvery, out asData))
                    {
                        //Determine if rtcp is disabled in the media description
                        bool rtcpDisabled = reportReceivingEvery == 0 && reportSendingEvery == 0;

                        //If Rtcp is not disabled then this will set the read and write timeouts.
                        if (false == rtcpDisabled)
                        {
                            tc.IsRtcpEnabled = true;

                            if (reportReceivingEvery > 0) tc.m_ReceiveInterval = TimeSpan.FromSeconds(reportReceivingEvery / TimeSpanExtensions.MicrosecondsPerMillisecond);

                            if (reportSendingEvery > 0) tc.m_SendInterval = TimeSpan.FromSeconds(reportSendingEvery / TimeSpanExtensions.MicrosecondsPerMillisecond);
                        }//Disable rtcp (already checked to be enabled)
                        else if (rtcpEnabled)
                            tc.IsRtcpEnabled = false;
                    }
                }

                var rangeInfo = mediaDescription.RangeLine ?? (sessionDescription != null ? sessionDescription.RangeLine : null);

                if (rangeInfo != null && rangeInfo.m_Parts.Count > 0)
                {
                    string type;

                    SessionDescription.TryParseRange(rangeInfo.m_Parts.Last(), out type, out tc.m_StartTime, out tc.m_EndTime);
                }

                //https://www.ietf.org/rfc/rfc3605.txt

                //rtcpAttribute indicates if RTCP should use a special port and not be dervied from the RtpPort algorithmically 

                //"a=rtcp:" 

                /*
                 
                  Example encodings could be:

                    m=audio 49170 RTP/AVP 0
                    a=rtcp:53020

                    m=audio 49170 RTP/AVP 0
                    a=rtcp:53020 IN IP4 126.16.64.4

                    m=audio 49170 RTP/AVP 0
                    a=rtcp:53020 IN IP6 2001:2345:6789:ABCD:EF01:2345:6789:ABCD
                 
                 */

                SessionDescriptionLine rtcpLine = mediaDescription.RtcpLine;

                if (rtcpLine != null)
                {
                    throw new NotImplementedException("Make a thread if you need rtcp AttributeField support immediately.");
                }

                //Handle connect
                if (connect)
                {
                    //Determine if a socket was given or if it will be created.
                    bool hasSocket = existingSocket != null;

                    //If a configuration has been given then set that configuration in the TransportContext.
                    if (configure != null) tc.ConfigureSocket = configure;

                    //Check for udp if no existing socket was given
                    if (false == hasSocket && string.Compare(mediaDescription.MediaProtocol, RtpClient.RtpAvpProfileIdentifier, System.StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        //Find a local port
                        int localPort = SocketExtensions.ProbeForOpenPort(ProtocolType.Udp);

                        if (localPort < 0) throw new ArgumentOutOfRangeException("Cannot find an open port.");

                        //Create the sockets and connect
                        await tc.Initialize(localIp, remoteIp, //LocalIP, RemoteIP
                             localPort == 0 ? localPort : localPort++, //LocalRtp
                             localPort == 0 ? localPort : localPort++, //LocalRtcp                            
                             rtpPort ?? mediaDescription.MediaPort, //RemoteRtp
                             rtcpPort ?? (mediaDescription.MediaPort != 0 ? mediaDescription.MediaPort + 1 : mediaDescription.MediaPort)); //RemoteRtcp
                    }
                    else if (hasSocket)//If had a socket use it
                    {
                        await tc.Initialize(existingSocket);
                    }
                    else //Create the sockets and connect (TCP)
                    {
                        tc.Initialize(localIp, remoteIp, rtpPort ?? mediaDescription.MediaPort);
                    }
                }

                //Return the context created
                return tc;
            }

            public static GoodbyeReport CreateGoodbyeReport(TransportContext context, byte[] reasonForLeaving = null, int? ssrc = null, RFC3550.SourceList sourcesLeaving = null)
            {
                LoggerInstance.Debug("TransportContext  Create GoodbyeReport");

                return new GoodbyeReport(context.Version, ssrc ?? (int)context.SynchronizationSourceIdentifier, sourcesLeaving, reasonForLeaving);
            }

            /// <summary>
            /// Creates a <see cref="SendersReport"/> from the given context and updates the RtpExpectedPrior and RtpReceivedPrior accordingly.
            /// Note, If empty is false and no previous <see cref="SendersReport"/> was sent then the report will be empty anyway.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="empty">Specifies if the report should have any report blocks if possible</param>
            /// <returns>The report created</returns>
            public static SendersReport CreateSendersReport(TransportContext context, bool empty, bool rfc = true)
            {
                LoggerInstance.Debug("TransportContext  Create SendersReport");
                //Create a SendersReport
                SendersReport result = new SendersReport(context.Version, 0, context.SynchronizationSourceIdentifier);

                //Use the values from the TransportChannel (Use .NtpTimestamp = 0 to Disable NTP)[Should allow for this to be disabled]
                result.NtpTimestamp = context.SenderNtpTimestamp + context.SenderNtpOffset;

                if (result.NtpTimestamp == 0) result.NtpDateTime = DateTime.UtcNow;

                //Note that in most cases this timestamp will not be equal to the RTP timestamp in any adjacent data packet.  Rather, it MUST be  calculated from the corresponding NTP timestamp using the relationship between the RTP timestamp counter and real time as maintained by periodically checking the wallclock time at a sampling instant.
                result.RtpTimestamp = context.SenderRtpTimestamp;

                //If no data has been received this value will be 0, set it to the expected value based on the time.
                if (result.RtpTimestamp == 0) result.RtpTimestamp = (int)NetworkTimeProtocol.DateTimeToNptTimestamp32(result.NtpDateTime);

                //Counters
                result.SendersOctetCount = (int)(rfc ? context.RfcRtpBytesSent : context.RtpBytesSent);
                result.SendersPacketCount = (int)context.RtpPacketsSent;

                //Ensure there is a remote party
                //If source blocks are included include them and calculate their statistics
                if (false == empty && false == context.InDiscovery && context.IsValid && context.TotalPacketsSent > 0)
                {
                    uint fraction, lost;

                    RFC3550.CalculateFractionAndLoss(ref context.RtpBaseSeq, ref context.RtpMaxSeq, ref context.RtpSeqCycles, ref context.ValidRtpPacketsReceived, ref context.RtpReceivedPrior, ref context.RtpExpectedPrior, out fraction, out lost);

                    //Create the ReportBlock based off the statistics of the last RtpPacket and last SendersReport
                    result.Add(new ReportBlock((int)context.RemoteSynchronizationSourceIdentifier,
                        (byte)fraction,
                        (int)lost,
                        context.SendSequenceNumber,
                        (int)context.SenderJitter,
                        //The middle 32 bits out of 64 in the NTP timestamp (as explained in Section 4) received as part of the most recent RTCP sender report (SR) packet from source SSRC_n. If no SR has been received yet, the field is set to zero.
                        (int)((context.SenderNtpTimestamp >> 16) << 32),
                        //The delay, expressed in units of 1/65536 seconds, between receiving the last SR packet from source SSRC_n and sending this reception report block. If no SR packet has been received yet from SSRC_n, the DLSR field is set to zero.
                        context.LastRtcpReportSent > TimeSpan.MinValue ? (int)context.LastRtcpReportSent.TotalSeconds / ushort.MaxValue : 0));
                }

                return result;
            }

            /// <summary>
            /// Creates a <see cref="ReceiversReport"/> from the given context and updates the RtpExpectedPrior and RtpReceivedPrior accordingly.
            /// </summary>
            /// <param name="context">The context</param>
            /// <param name="empty">Indicates if the report should be empty</param>
            /// <returns>The report created</returns>
            public static ReceiversReport CreateReceiversReport(TransportContext context, bool empty)
            {
                //LoggerInstance.Debug("TransportContext  Create ReceiversReport");
                ReceiversReport result = new ReceiversReport(context.Version, 0, context.SynchronizationSourceIdentifier);

                if (false == empty && false == context.InDiscovery && context.IsValid && context.TotalRtpPacketsReceieved > 0)
                {
                    uint fraction, lost;

                    RFC3550.CalculateFractionAndLoss(ref context.RtpBaseSeq, ref context.RtpMaxSeq, ref context.RtpSeqCycles, ref context.ValidRtpPacketsReceived, ref context.RtpReceivedPrior, ref context.RtpExpectedPrior, out fraction, out lost);

                    //Create the ReportBlock based off the statistics of the last RtpPacket and last SendersReport
                    result.Add(new ReportBlock((int)context.RemoteSynchronizationSourceIdentifier,
                        (byte)fraction,
                        (int)lost,
                        context.RecieveSequenceNumber,
                        (int)context.RtpJitter >> 4,//The last report may not be null but may be disposed and time is probably invalid if so, in such a case use LastRtcpReportRecieved                    
                        (int)(false == IDisposedExtensions.IsNullOrDisposed(context.SendersReport) ? NetworkTimeProtocol.DateTimeToNptTimestamp32(context.SendersReport.NtpDateTime) : context.LastRtcpReportReceived > TimeSpan.MinValue ? NetworkTimeProtocol.DateTimeToNptTimestamp32(DateTime.UtcNow - context.LastRtcpReportReceived) : 0),
                        (context.SendersReport != null ? ((DateTime.UtcNow - context.SendersReport.Created).Seconds / ushort.MaxValue) * 1000 : 0) //If also sending senders reports this logic may not be correct
                    ));
                }

                return result;
            }

            /// <summary>
            /// Creates a <see cref="SourceDescriptionReport"/> from the given context.
            /// If <paramref name="cName"/> is null then <see cref="SourceDescriptionItem.CName"/> will be used.
            /// </summary>
            /// <param name="context">The context</param>
            /// <param name="cName">The optional cName to use</param>
            /// <returns>The created report</returns>
            public static SourceDescriptionReport CreateSourceDescriptionReport(TransportContext context, SourceDescriptionReport.SourceDescriptionItem cName = null, IEnumerable<SourceDescriptionReport.SourceDescriptionItem> items = null)
            {
                //LoggerInstance.Debug("TransportContext  Create SourceDescriptionReport");
                var itemResult = LinqExtensions.Yield((cName ?? SourceDescriptionReport.SourceDescriptionItem.CName)).Concat(items ?? System.Linq.Enumerable.Empty<Gsafety.PTMS.Media.RTSP.RTCP.SourceDescriptionReport.SourceDescriptionItem>());

                return new SourceDescriptionReport(context.Version) 
                { 
                    new Gsafety.PTMS.Media.RTSP.RTCP.SourceDescriptionReport.SourceDescriptionChunk((int)context.SynchronizationSourceIdentifier,itemResult)
                };
            }

            #endregion

            #region Fields

            /// <summary>
            /// The version of packets which the TransportContents handles
            /// </summary>
            public int Version = 2;

            /// <summary>
            /// The amount of <see cref="RtpPacket"/>'s which must be received before IsValid is true.
            /// </summary>
            public int MinimumSequentialValidRtpPackets = RFC3550.DefaultMinimumSequentalRtpPackets;

            public int MaxMisorder = RFC3550.DefaultMaxMisorder;

            public int MaxDropout = RFC3550.DefaultMaxDropout;

            /// <summary>
            /// The channels which identity the TransportContext.
            /// </summary>
            public byte DataChannel, ControlChannel;

            /// <summary>
            /// Indicates if Rtp is enabled on the TransportContext
            /// </summary>
            public bool IsRtpEnabled = true;

            /// <summary>
            /// Indicates if Rtcp will be used on this TransportContext
            /// </summary>
            public bool IsRtcpEnabled = true;

            //The EndPoints connected to (once connected don't need the Ports unless 0 is used to determine the port)
            internal protected EndPoint LocalRtp, LocalRtcp, RemoteRtp, RemoteRtcp;

            //bytes and packet counters
            internal long RfcRtpBytesSent, RfcRtpBytesRecieved,
                         RtpBytesSent, RtpBytesRecieved,
                         RtcpBytesSent, RtcpBytesRecieved,
                         RtpPacketsSent, RtcpPacketsSent,
                         RtpPacketsReceived, RtcpPacketsReceived;

            //The current, highest received and highest sent Sequence numbers recieved by the RtpClient
            internal ushort m_SequenceNumber, m_LastSentSequenceNumber, RtpMaxSeq;

            //Used for Rtp and Rtcp Transport Calculations (Should be moved into State Structure)
            internal uint RtpTransit, SenderTransit,
                //Count of bytes recieved prior to the reception of a report
                RtpReceivedPrior,
                //Count of bytes expected prior to the recpetion of a report
                RtpExpectedPrior,
                //The amount of times the Seq number has cycled
                RtpSeqCycles,
                //The amount of base RTP Sequences encountered
                RtpBaseSeq,
                //Rtp Probation value
                RtpProbation,
                //The amount of bad RTP Sequences encountered
                RtpBadSeq,
                //Jitter value
                RtpJitter, SenderJitter,
                //Valid amount of rtp packets recieved 
                ValidRtpPacketsReceived;

            internal TimeSpan m_SendInterval = DefaultReportInterval, m_ReceiveInterval = DefaultReportInterval,
                m_InactiveTime = TimeSpanExtensions.InfiniteTimeSpan,
                m_StartTime = TimeSpan.Zero, m_EndTime = TimeSpanExtensions.InfiniteTimeSpan;

            //When packets are succesfully transferred the DateTime (utc) is copied in these variables and will reflect the point in time in which  the last 
            internal DateTime m_FirstPacketReceived, m_FirstPacketSent,
                m_LastRtcpIn, m_LastRtcpOut,  //Rtcp packets were received and sent
                m_LastRtpIn, m_LastRtpOut, //Rtp packets were received and sent
                m_Initialized;//When initialize was called.

            /// <summary>
            /// Keeps track of any failures which occur when sending or receieving data.
            /// </summary>
            internal protected int m_FailedRtpTransmissions, m_FailedRtcpTransmissions, m_FailedRtpReceptions, m_FailedRtcpReceptions;

            /// <summary>
            /// Used to ensure packets are allowed.
            /// </summary>
            ushort m_MimumPacketSize = 8, m_MaximumPacketSize = ushort.MaxValue;

            #endregion

            #region Properties

            public Action<Socket> ConfigureSocket { get; set; }

            /// <summary>
            /// Sets or gets the applications-specific state associated with the TransportContext.
            /// </summary>
            public Object ApplicationContext { get; set; }

            /// <summary>
            /// Gets or sets the MemorySegment used by this context.
            /// </summary>
            public MemorySegment ContextMemory { get; set; }

            /// <summary>
            /// The smallest packet which may be sent or recieved on the TransportContext.
            /// </summary>
            public int MinimumPacketSize
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return (int)m_MimumPacketSize; }
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                set { m_MimumPacketSize = (ushort)value; }
            }

            /// <summary>
            /// The largest packet which may be sent or recieved on the TransportContext.
            /// </summary>
            public int MaximumPacketSize
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return (int)m_MaximumPacketSize; }
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                set { m_MaximumPacketSize = (ushort)value; }
            }

            public bool HasAnyRecentActivity
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return HasRecentRtpActivity || HasRecentRtcpActivity; }
            }

            public bool HasRecentRtpActivity
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get
                {
                    //Check for Rtp Receive Activity if receiving
                    return HasReceivedRtpWithinReceiveInterval
                        || //Check for Rtp Send Activity if sending
                        HasSentRtpWithinSendInterval;
                }
            }

            public bool HasRecentRtcpActivity
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get
                {
                    //Check for Rtcp Receive Activity if receiving
                    return HasReceivedRtcpWithinReceiveInterval
                        || //Check for Rtcp Send Activity if sending
                        HasSentRtcpWithinSendInterval;
                }
            }

            public bool HasReceivedRtpWithinReceiveInterval
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get
                {
                    return TotalRtpPacketsReceieved >= 0 &&
                        m_LastRtpIn != DateTime.MinValue &&
                        m_ReceiveInterval != TimeSpanExtensions.InfiniteTimeSpan &&
                        LastRtpPacketReceived < m_ReceiveInterval;
                }
            }

            public bool HasSentRtpWithinSendInterval
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get
                {
                    return IsActive && TotalRtpPacketsSent >= 0 &&
                        m_LastRtpOut != DateTime.MinValue &&
                        m_SendInterval != TimeSpanExtensions.InfiniteTimeSpan &&
                        LastRtpPacketSent < m_SendInterval;
                }
            }

            public bool HasReceivedRtcpWithinReceiveInterval
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get
                {
                    return TotalRtcpPacketsReceieved >= 0 &&
                        m_LastRtcpIn != DateTime.MinValue &&
                        m_ReceiveInterval != TimeSpanExtensions.InfiniteTimeSpan &&
                        LastRtcpReportReceived < m_ReceiveInterval;
                }
            }

            public bool HasSentRtcpWithinSendInterval
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get
                {
                    return TotalRtcpPacketsSent >= 0 &&
                        m_LastRtcpOut != DateTime.MinValue &&
                        m_SendInterval != TimeSpanExtensions.InfiniteTimeSpan &&
                        LastRtcpReportSent < m_SendInterval;
                }
            }

            /// <summary>
            /// Indicates if the RemoteParty is known by a unique id other than 0.
            /// </summary>
            //Should also check if receiving...
            internal bool InDiscovery
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return RemoteSynchronizationSourceIdentifier == 0; }
            }

            /// <summary>
            /// Gets or Sets a value which indicates if the Rtp and Rtcp Sockets should be Disposed when Dispose is called.
            /// </summary>
            public bool LeaveOpen { get; set; }

            //Any frames for this channel
            public RtpFrame CurrentFrame { get; internal protected set; }

            public RtpFrame LastFrame { get; internal protected set; }

            /// <summary>
            /// The socket used for Transport of Rtp and Interleaved data
            /// </summary>
            public Socket RtpSocket { get; internal protected set; }

            /// <summary>
            /// The socket used for Transport of Rtcp and Interleaved data
            /// </summary>
            public Socket RtcpSocket { get; internal protected set; }

            /// <summary>
            /// Indicates if the TransportContext has been connected.
            /// </summary>
            public bool IsActive
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (IsRtpEnabled)
                    {
                        return RtpSocket != null;
                        //return RtpSocket != null && LocalRtp != null;
                    }
                    else if (IsRtcpEnabled)
                    {
                        return RtcpSocket != null && LocalRtcp != null;
                    }

                    return false;
                }
            }

            /// <summary>
            /// The maximum amount of bandwidth Rtcp can utilize (of the overall bandwidth available to the TransportContext) during reports
            /// </summary>
            public double MaximumRtcpBandwidthPercentage { get; set; }

            /// <summary>
            /// Indicates if the amount of bandwith currently utilized for Rtcp reporting has exceeded the amount of bandwidth allowed by the <see cref="MaximumRtcpBandwidthPercentage"/> property.
            /// </summary>
            public bool RtcpBandwidthExceeded
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (IsDisposed || false == IsRtcpEnabled) return true;

                    //If disposed no limit is imposed do not check
                    if (MaximumRtcpBandwidthPercentage == 0) return false;

                    long totalReceived = TotalBytesReceieved;

                    if (totalReceived == 0) return false;

                    long totalRtcp = TotalRtcpBytesSent + TotalRtcpBytesReceieved;

                    if (totalRtcp == 0) return false;

                    return totalRtcp >= totalReceived / MaximumRtcpBandwidthPercentage;
                }
            }

            /// <summary>
            /// The amount of time the TransportContext has been sending packets.
            /// </summary>
            public TimeSpan TimeSending
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get
                {
                    return IsDisposed || m_FirstPacketSent == DateTime.MinValue ?
                        TimeSpanExtensions.InfiniteTimeSpan
                        :
                        DateTime.UtcNow - m_FirstPacketSent;
                }
            }

            /// <summary>
            /// The amount of time the TransportContext has been receiving packets.
            /// </summary>
            public TimeSpan TimeReceiving
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get
                {
                    return IsDisposed || m_FirstPacketReceived == DateTime.MinValue ?
                        TimeSpanExtensions.InfiniteTimeSpan
                        :
                        DateTime.UtcNow - m_FirstPacketReceived;
                }
            }

            /// <summary>
            /// The time at which the media starts
            /// </summary>
            public TimeSpan MediaStartTime
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return m_StartTime; }
                internal protected set { m_StartTime = value; }
            }

            /// <summary>
            /// The time at which the media ends
            /// </summary>
            public TimeSpan MediaEndTime
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return m_EndTime; }
                internal protected set { m_EndTime = value; }
            }

            public TimeSpan MediaDuration
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return IsContinious ? TimeSpanExtensions.InfiniteTimeSpan : m_EndTime - m_StartTime; }
            }

            /// <summary>
            /// Indicates if the <see cref="MediaEndTime"/> is <see cref="TimeSpanExtensions.InfiniteTimeSpan"/>. (Has no determined end time)
            /// </summary>
            public bool IsContinious
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return m_EndTime == TimeSpanExtensions.InfiniteTimeSpan; }
            }

            /// <summary>
            /// <see cref="TimeSpanExtensions.InfiniteTimeSpan"/> if <see cref="IsContinious"/>,
            /// othewise the amount of time remaining in the media.
            /// </summary>
            public TimeSpan TimeRemaining
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return IsContinious ? m_EndTime : TimeSpan.FromTicks(m_EndTime.Ticks - (Math.Max(TimeReceiving.Ticks, TimeSending.Ticks))); }
            }

            /// <summary>
            /// Allows getting or setting of the interval which occurs between data transmissions
            /// </summary>
            public TimeSpan SendInterval
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return m_SendInterval; }
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                set { m_SendInterval = value; }
            }

            /// <summary>
            /// Allows gettings or setting of the interval which occurs between data receptions
            /// </summary>
            public TimeSpan ReceiveInterval
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return m_ReceiveInterval; }
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                set { m_ReceiveInterval = value; }
            }

            /// <summary>
            /// Gets the time in which in TranportContext was last active for a send or receive operation
            /// </summary>
            public TimeSpan InactiveTime
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return m_InactiveTime; }
            }


            /// <summary>
            /// Gets the time in which the last Rtcp reports were sent.
            /// </summary>
            public TimeSpan LastRtcpReportSent
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get
                {
                    return m_LastRtcpOut == DateTime.MinValue ? TimeSpan.MinValue : DateTime.UtcNow - m_LastRtcpOut;
                }
            }

            /// <summary>
            /// Gets the time in which the last Rtcp reports were received.
            /// </summary>
            public TimeSpan LastRtcpReportReceived
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get
                {
                    return m_LastRtcpIn == DateTime.MinValue ? TimeSpan.MinValue : DateTime.UtcNow - m_LastRtcpIn;
                }
            }

            /// <summary>
            /// Gets the time in which the last RtpPacket was received.
            /// </summary>
            public TimeSpan LastRtpPacketReceived
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get
                {
                    return m_LastRtpIn == DateTime.MinValue ? TimeSpan.MinValue : DateTime.UtcNow - m_LastRtpIn;
                }
            }

            /// <summary>
            /// Gets the time in which the last RtpPacket was transmitted.
            /// </summary>
            public TimeSpan LastRtpPacketSent
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get
                {
                    return m_LastRtpOut == DateTime.MinValue ? TimeSpan.MinValue : DateTime.UtcNow - m_LastRtpOut;
                }
            }

            /// <summary>
            /// Gets the time since <see cref="Initialize was called."/>
            /// </summary>
            public TimeSpan TimeActive
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get
                {
                    return m_Initialized == DateTime.MinValue ? TimeSpan.MinValue : DateTime.UtcNow - m_Initialized;
                }
            }

            /// <summary>
            /// Indicates the amount of times a failure has occured when sending RtcpPackets
            /// </summary>
            public int FailedRtcpTransmissions
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return m_FailedRtcpTransmissions; }
            }

            /// <summary>
            /// Indicates the amount of times a failure has occured when sending RtpPackets
            /// </summary>
            public int FailedRtpTransmissions
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return m_FailedRtpTransmissions; }
            }

            /// <summary>
            /// Indicates the amount of times a failure has occured when receiving RtcpPackets
            /// </summary>
            public int FailedRtcpReceptions
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return m_FailedRtcpReceptions; }
            }

            /// <summary>
            /// Indicates the amount of times a failure has occured when receiving RtpPackets
            /// </summary>
            public int FailedRtpReceptions
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return m_FailedRtpReceptions; }
            }

            /// <summary>
            /// Corresponds to the ID used by remote systems to identify this TransportContext, a table might be necessary if you want to use a different id in different places
            /// </summary>
            public int SynchronizationSourceIdentifier { get; internal protected set; }

            /// <summary>
            /// Corresponds to the ID used to identify remote parties.            
            /// Use a <see cref="Conference"/> if the size of the group or its members should be limited in some capacity.
            /// </summary>
            public int? RemoteSynchronizationSourceIdentifier { get; internal protected set; }

            /// <summary>
            /// MediaDescription which contains information about the type of Media on the Interleave
            /// </summary>
            public MediaDescription MediaDescription { get; internal protected set; }

            /// <summary>
            /// Determines if the source has recieved at least <see cref="MinimumSequentialValidRtpPackets"/> RtpPackets
            /// </summary>
            public virtual bool IsValid
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return ValidRtpPacketsReceived >= MinimumSequentialValidRtpPackets; }
            }

            /// <summary>
            /// Indicates if the Rtcp is enabled and the LocalRtp is equal to the LocalRtcp
            /// </summary>
            public bool LocalMultiplexing
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return IsDisposed || IsRtcpEnabled == false || LocalRtp == null ? false : LocalRtp.Equals(LocalRtcp); }
            }

            /// <summary>
            /// Indicates if the Rtcp is enabled and the RemoteRtp is equal to the RemoteRtcp
            /// </summary>
            public bool RemoteMultiplexing
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return IsDisposed || IsRtcpEnabled == false || RemoteRtp == null ? false : RemoteRtp.Equals(RemoteRtcp); }
            }

            /// <summary>
            /// <c>false</c> if NOT [RtpEnabled AND RtcpEnabled] AND [LocalMultiplexing OR RemoteMultiplexing]
            /// </summary>
            public bool IsDuplexing
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (IsDisposed) return false;

                    return (IsRtpEnabled && IsRtcpEnabled) && (LocalMultiplexing || RemoteMultiplexing);
                }
            }

            /// <summary>
            /// The last <see cref="ReceiversReport"/> sent or received by this RtpClient.
            /// </summary>
            public ReceiversReport ReceiversReport { get; internal set; }

            /// <summary>
            /// The last <see cref="SendersReport"/> sent or received by this RtpClient.
            /// </summary>
            public SendersReport SendersReport { get; internal set; }

            /// The last <see cref="SourceDescriptionReport"/> sent or received by this RtpClient.
            public SourceDescriptionReport SourceDescription { get; internal set; }

            /// The last <see cref="GoodbyeReport"/> sent or received by this RtpClient.
            public GoodbyeReport Goodbye { get; internal set; }

            /// <summary>
            /// The total amount of packets (both Rtp and Rtcp) receieved
            /// </summary>
            public long TotalPacketsReceived
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return RtpPacketsReceived + RtcpPacketsReceived; }
            }

            /// <summary>
            /// The total amount of packets (both Rtp and Rtcp) sent
            /// </summary>
            public long TotalPacketsSent
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return RtpPacketsSent + RtcpPacketsSent; }
            }

            /// <summary>
            /// The total amount of RtpPackets sent
            /// </summary>
            public long TotalRtpPacketsSent
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return IsDisposed ? 0 : RtpPacketsSent; }
            }

            /// <summary>
            /// The amount of bytes in all rtp packets payloads which have been sent.
            /// </summary>
            public long RtpPayloadBytesSent
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return IsDisposed ? 0 : RtpBytesSent; }
            }

            /// <summary>
            /// The amount of bytes in all rtp packets payloads which have been received.
            /// </summary>
            public long RtpPayloadBytesRecieved
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return IsDisposed ? 0 : RtpBytesRecieved; }
            }

            /// <summary>
            /// The total amount of bytes related to Rtp sent (including headers)
            /// </summary>
            public long TotalRtpBytesSent
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return IsDisposed ? 0 : RtpBytesSent + RtpHeader.Length * RtpPacketsSent; }
            }

            /// <summary>
            /// The total amount of bytes related to Rtp received
            /// </summary>
            public long TotalRtpBytesReceieved
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return IsDisposed ? 0 : RtpBytesRecieved + RtpHeader.Length * RtpPacketsSent; }
            }

            /// <summary>
            /// The total amount of RtpPackets received
            /// </summary>
            public long TotalRtpPacketsReceieved
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return IsDisposed ? 0 : RtpPacketsReceived; }
            }

            /// <summary>
            /// The total amount of RtcpPackets recieved
            /// </summary>
            public long TotalRtcpPacketsSent
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return IsDisposed ? 0 : RtcpPacketsSent; }
            }

            /// <summary>
            /// The total amount of sent bytes related to Rtcp 
            /// </summary>
            public long TotalRtcpBytesSent
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return IsDisposed ? 0 : RtcpBytesSent; }
            }

            /// <summary>
            /// The total amount of received bytes (both Rtp and Rtcp) received
            /// </summary>
            public long TotalBytesReceieved
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return IsDisposed ? 0 : TotalRtcpBytesReceieved + TotalRtpBytesReceieved; }
            }

            /// <summary>
            /// The total amount of received bytes (both Rtp and Rtcp) sent
            /// </summary>
            public long TotalBytesSent
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return IsDisposed ? 0 : TotalRtcpBytesSent + TotalRtpBytesSent; }
            }

            /// <summary>
            /// The total amount of RtcpPackets received
            /// </summary>
            public long TotalRtcpPacketsReceieved
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return IsDisposed ? 0 : RtcpPacketsReceived; }
            }

            /// <summary>
            /// The total amount of bytes related to Rtcp received
            /// </summary>
            public long TotalRtcpBytesReceieved
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return IsDisposed ? 0 : RtcpBytesRecieved; }
            }

            /// <summary>            
            /// Gets the sequence number of the last RtpPacket recieved on this channel
            /// </summary>
            public int RecieveSequenceNumber
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return (short)m_SequenceNumber; }

                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                internal protected set { m_SequenceNumber = (ushort)value; }
            }

            public int SendSequenceNumber
            {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get { return (short)m_LastSentSequenceNumber; }
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                internal protected set { m_LastSentSequenceNumber = (ushort)value; }
            }

            public int RtpTimestamp { get; internal set; }

            public int SenderRtpTimestamp { get; internal set; }

            /// <summary>
            /// The NtpTimestamp from the last SendersReport recieved or created
            /// </summary>
            public long NtpTimestamp { get; internal set; }

            public long SenderNtpTimestamp { get; internal set; }

            public long NtpOffset { get; set; }

            public long SenderNtpOffset { get; set; }

            #endregion

            #region Constructor

            /// <summary>
            /// Creates a TransportContext from the given parameters
            /// </summary>
            /// <param name="dataChannel"></param>
            /// <param name="controlChannel"></param>
            /// <param name="ssrc"></param>
            /// <param name="rtcpEnabled"></param>
            /// <param name="senderSsrc"></param>
            /// <param name="minimumSequentialRtpPackets"></param>
            public TransportContext(byte dataChannel, byte controlChannel, int ssrc, bool rtcpEnabled = true, int senderSsrc = 0, int minimumSequentialRtpPackets = 2, Action<System.Net.Sockets.Socket> configure = null, bool shouldDispose = true)
                : base(shouldDispose)
            {
                if (dataChannel == controlChannel) throw new InvalidOperationException("dataChannel and controlChannel must be unique.");

                if (ssrc == senderSsrc && ssrc != 0) throw new InvalidOperationException("ssrc and senderSsrc must be unique.");

                if (minimumSequentialRtpPackets < 0) throw new InvalidOperationException("minimumSequentialRtpPackets must be >= 0");

                DataChannel = dataChannel;

                ControlChannel = controlChannel;

                SynchronizationSourceIdentifier = ssrc;

                IsRtcpEnabled = rtcpEnabled;

                //If 0 then all packets are answered
                RemoteSynchronizationSourceIdentifier = senderSsrc;

                //MinimumSequentialValidRtpPackets should be equal to 0 when RemoteSynchronizationSourceIdentifier is null I think, this essentially means respond to all inquiries.
                //A confrence may be able to contain this type of behavior better if required.
                MinimumSequentialValidRtpPackets = minimumSequentialRtpPackets;

                //Default bandwidth restriction
                MaximumRtcpBandwidthPercentage = DefaultReportInterval.TotalSeconds;

                //Assign the function responsible for configuring the socket
                ConfigureSocket = configure ?? ConfigureRtpRtcpSocket;
            }

            public TransportContext(byte dataChannel, byte controlChannel, int ssrc, MediaDescription mediaDescription, bool rtcpEnabled = true, int senderSsrc = 0, int minimumSequentialRtpPackets = 2, bool shouldDispose = true)
                : this(dataChannel, controlChannel, ssrc, rtcpEnabled, senderSsrc, minimumSequentialRtpPackets, null, shouldDispose)
            {
                MediaDescription = mediaDescription;
            }

            public TransportContext(byte dataChannel, byte controlChannel, int ssrc, MediaDescription mediaDescription, Socket socket, bool rtcpEnabled = true, int senderSsrc = 0, int minimumSequentialRtpPackets = 2, bool shouldDispose = true)
                : this(dataChannel, controlChannel, ssrc, mediaDescription, rtcpEnabled, senderSsrc, minimumSequentialRtpPackets, shouldDispose)
            {
                RtpSocket = RtcpSocket = socket;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Assigns a Non Zero value to <see cref="SynchronizationSourceIdentifier"/> to a random value based on the given seed.
            /// The value will also be different than <see cref="RemoteSynchronizationSourceIdentifier"/>.
            /// </summary>
            internal protected void AssignIdentity(int seed = SendersReport.PayloadType)
            {
                if (SynchronizationSourceIdentifier == 0)
                {
                    //Generate the id per RFC3550
                    do SynchronizationSourceIdentifier = RFC3550.Random32(seed);
                    while (SynchronizationSourceIdentifier == 0 || SynchronizationSourceIdentifier == RemoteSynchronizationSourceIdentifier);
                }
            }

            /// <summary>
            /// Calculates RTP Interarrival Jitter as specified in RFC 3550 6.4.1.
            /// </summary>
            /// <param name="packet">RTP packet.</param>
            public void UpdateJitterAndTimestamp(RtpPacket packet)
            {
                //Determine to update sent or received values
                bool sentPacket = packet.Transferred.HasValue;

                // RFC 3550 A.8.
                //Determine the time the last packet was sent or received
                TimeSpan arrivalDifference = (sentPacket ? LastRtpPacketSent : LastRtpPacketReceived);

                if (sentPacket)
                {
                    RFC3550.CalulcateJitter(ref arrivalDifference, ref SenderJitter, ref SenderTransit);

                    //Update the Sender RtpTimestamp on the Context
                    SenderRtpTimestamp = packet.Timestamp;

                    //Update the Sender NtpTimestamp on the Context.
                    SenderNtpTimestamp = (long)NetworkTimeProtocol.DateTimeToNptTimestamp(packet.Transferred ?? packet.Created);
                }
                else
                {

                    RFC3550.CalulcateJitter(ref arrivalDifference, ref RtpJitter, ref RtpTransit);

                    //Update the RtpTimestamp on the Context
                    RtpTimestamp = packet.Timestamp;

                    //Update the NtpTimestamp on the Context.
                    NtpTimestamp = (long)NetworkTimeProtocol.DateTimeToNptTimestamp(sentPacket ? packet.Transferred.Value : packet.Created);
                }

                //Context is not inactive.
                m_InactiveTime = TimeSpanExtensions.InfiniteTimeSpan;
            }

            /// <summary>
            /// Performs checks on the packet which can be overriden in a derrived implementation
            /// </summary>
            /// <param name="packet"></param>
            /// <returns></returns>
            public virtual bool ValidatePacketAndUpdateSequenceNumber(RtpPacket packet)
            {

                /*NOTE : 
                 * http://www.ietf.org/rfc/rfc3551.txt
                 * 
                  Static payload type 13 was assigned to the Comfort Noise (CN) payload format defined in RFC 3389.  
                  Payload type 19 was marked reserved because it had been temporarily allocated to an earlier version of Comfort Noise
                  present in some draft revisions of this document.
                 */

                //If there is no Payload return, this prevents injection by utilizing just a RtpHeader which happens to be valid.
                //I can think of no good reason to allow this in this implementation, if required dervive and ensure that RTCP is not better suited for whatever is being done.
                //The underlying goto CheckSequenceNumber is what is used to performed this check currently.
                //if (payloadLength == 0 && packet.PayloadType != 13) return false;
                //else if (packet.PayloadType == 13  || packet.PayloadType == 19) goto CheckSequenceNumber;

                if (packet.Header.IsCompressed || packet.PayloadType == 13) goto CheckSequenceNumber;

                // RFC 3550 A.1. Notes: Each TransportContext instance may be better suited to have a structure which defines this logic.

                //o  RTP version field must equal 2.

                if (packet.Version != Version) return false;

                //o  The payload type must be known, and in particular it must not be equal to SR or RR.

                int check = packet.PayloadType;

                //Check the payload type is known and not equal to sr or rr.
                if (check == SendersReport.PayloadType || check == ReceiversReport.PayloadType || false == MediaDescription.PayloadTypes.Contains(check)) return false;

                //Space complex
                int payloadLength = packet.Payload.Count;

                //o  If the P bit is set, Padding must be less than the total packet length minus the header size.
                if (packet.Padding && payloadLength > 0 && packet.PaddingOctets > payloadLength) return false;

                check = packet.ContributingSourceCount;

                ///  o  The length of the packet must be consistent with CC and payload type (if payloads have a known length this is checked with the IsComplete property).
                if (check > 0 && payloadLength < check * Binary.BytesPerInteger) return false;

                //Only performed to ensure validity
                if (packet.Extension)
                {
                    //o  The X bit must be zero if the profile does not specify that the
                    //   header extension mechanism may be used.  
                    //   Otherwise, the extension
                    //   length field must be less than the total packet size minus the
                    //   fixed header length and padding.

                    //Read the amount of paddingOctets
                    check = packet.PaddingOctets;

                    //Ensure the padding is valid first
                    if (check >= payloadLength) return false;

                    //Ensure the above is also true.
                    if (packet.ExtensionOctets > payloadLength - check) return false;
                }

                #region Notes on RFC3550 Implementation

            /*
                  The validity check can be made stronger requiring more than two
                    packets in sequence.  The disadvantages are that a larger number of
                    initial packets will be discarded (or delayed in a queue) and that
                    high packet loss rates could prevent validation.  However, because
                    the RTCP header validation is relatively strong, if an RTCP packet is
                    received from a source before the data packets, the count could be
                    adjusted so that only two packets are required in sequence.  If
                    initial data loss for a few seconds can be tolerated, an application
                    MAY choose to discard all data packets from a source until a valid
                    RTCP packet has been received from that source.
                 * 
                 * Please Note: This is why packets are stored in the CurrentFrame of the TransportContext. (To avoid loss where possible)
                 * A property exists for disabling the handling of RtpPackets which are incoming or outgoing.
                 * 
                 * Derived implementations may want to perform additional checks noted below inter alia.
                 * 
                 Depending on the application and encoding, algorithms may exploit
                   additional knowledge about the payload format for further validation.
                   For payload types where the timestamp increment is the same for all
                   packets, 
                 * the timestamp values can be predicted from the previous                  ------ Note:
                   packet received from the same source using the sequence number           ------ The source is not valid until MIN_SEQUENTIAL have been received.
                   difference (assuming no change in payload type).                         ------ This implementation maskes no assumptions about the Timestamp property.

                   A strong "fast-path" check is possible since with high probability       ------ Note:
                   the first four octets in the header of a newly received RTP data         ------  This implementation is engineered with the state of mind that certain profiles
                   packet will be just the same as that of the previous packet from the     ------  may REQUIRE that Padding or Extensions only be present in RtpPacket N of a RtpFrame X
                   same SSRC except that the sequence number will have increased by one.    ------  Thus this check is NOT performed. The SequenceNumber of the TransportContext is assigned in the HandleIncomingRtpPacket function AFTER the sender is valid.
                   
                 * Similarly, a single-entry cache may be used for faster SSRC lookups      ------ Note: This implementation utilizes the single-entry cache once MIN_SEQUENTIAL have been received.
                   in applications where data is typically received from one source at a    ------ In scenarios with more then 1 participant is required a Conference class is used.
                   time.
                 */

                #endregion

            CheckSequenceNumber:

                check = packet.SequenceNumber;

                //Return the result of processing the verification of the sequence number according the RFC3550 A.1
                if (UpdateSequenceNumber(check))
                {
                    //Update the SequenceNumber
                    RecieveSequenceNumber = check;

                    return true;
                }

                return false;
            }

            /// <summary>
            /// Performs checks in accorance with RFC3550 A.1 and returns a value indicating if the given sequence number is in state.
            /// </summary>
            /// <param name="sequenceNumber">The sequenceNumber to check.</param>
            /// <returns>True if in state, otherwise false.</returns>
            public bool UpdateSequenceNumber(int sequenceNumber) //,bool probe = false
            {
                ushort val = (ushort)sequenceNumber;

                return RFC3550.UpdateSequenceNumber(ref val, ref RtpBaseSeq, ref RtpMaxSeq, ref RtpBadSeq, ref RtpSeqCycles, ref RtpReceivedPrior, ref RtpProbation, ref ValidRtpPacketsReceived, MinimumSequentialValidRtpPackets, MaxMisorder, MaxDropout);
            }

            /// <summary>
            /// Randomizes the SequenceNumber
            /// </summary>
            public void RandomizeSequenceNumber() { RecieveSequenceNumber = Utility.Random.Next(); }

            #region Initialize

            //Todo allow for Leave Open...

            /// <summary>
            /// Creates the required Udp sockets for the TransportContext and updates the assoicated Properties and Fields
            /// </summary>
            /// <param name="localIp"></param>
            /// <param name="remoteIp"></param>
            /// <param name="localRtpPort"></param>
            /// <param name="localRtcpPort"></param>
            /// <param name="remoteRtpPort"></param>
            /// <param name="remoteRtcpPort"></param>
            /// <param name="punchHole"></param>
            /// <notes>
            /// Attention Freebox Stb Users!!!! -- Todo make an option to allow on the first receive to adjust port?
            /// Please use 0 For remoteRtpPort and remoteRtcpPort as the Freebox Stb does not use the correct Rtp or Rtcp ports indicated in the Describe request.
            /// </notes>
            public async Task Initialize(IPAddress localIp, IPAddress remoteIp, int localRtpPort, int localRtcpPort, int remoteRtpPort = 0, int remoteRtcpPort = 0, bool punchHole = true)
            {
                await Initialize(new IPEndPoint(localIp, localRtpPort), new IPEndPoint(remoteIp, remoteRtpPort), new IPEndPoint(localIp, localRtcpPort), new IPEndPoint(remoteIp, remoteRtcpPort), punchHole);
            }

            public async Task Initialize(IPEndPoint localRtp, IPEndPoint remoteRtp, IPEndPoint localRtcp, IPEndPoint remoteRtcp, bool punchHole = true, int ttl = 255)
            {
                if (IsDisposed || IsActive) return;

                m_Initialized = DateTime.UtcNow;

                if (localRtp.Address.AddressFamily != remoteRtp.Address.AddressFamily) TaggedExceptionExtensions.RaiseTaggedException<TransportContext>(this, "localIp and remoteIp AddressFamily must match.");

                //Erase previously set values on the TransportContext.
                //RtpBytesRecieved = RtpBytesSent = RtcpBytesRecieved = RtcpBytesSent = 0;

                //Set now if not already set
                AssignIdentity();

                try
                {
                    //Create the RtpSocket
                    RtpSocket = new Socket(localRtp.Address.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

                    //Configure it
                    ConfigureSocket(RtpSocket);

                    //Assign the RemoteRtp EndPoint and Bind the socket to that EndPoint
                    await RtpSocket.Connect(RemoteRtp = remoteRtp);

                    ////Handle Multicast joining (Might need to track interface)
                    //if (Extensions.IPEndPoint.IPEndPointExtensions.IsMulticast(remoteRtp))
                    //{
                    //    Extensions.Socket.SocketExtensions.JoinMulticastGroup(RtpSocket, remoteRtp.Address, ttl);
                    //}

                    //Determine if holepunch is required
                    if (punchHole)
                    {
                        //Send some bytes to ensure the result is open, if we get a SocketException the port is closed
                        //new RtpPacket(Version, false, false, false, MediaDescription.MediaFormat, SynchronizationSourceIdentifier, RemoteSynchronizationSourceIdentifier ?? 0, 0, 0, null);
                        try
                        {
                            await SocketExtensions.SendTo(WakeUpBytes, 0, WakeUpBytes.Length, RtpSocket, RemoteRtp);
                            //RtpSocket.SendAsync(WakeUpBytes, 0, WakeUpBytes.Length, SocketFlags.None, RemoteRtp);
                        }
                        catch (SocketException)
                        {
                            //The port was not open, allow the next recieve to determine the port
                            RemoteRtp = new IPEndPoint(((IPEndPoint)RemoteRtp).Address, 0);
                        }//We don't care about the response or any issues during the holePunch
                    }

                    //If Duplexing Rtp and Rtcp (on the same socket)
                    if (remoteRtp == remoteRtcp)
                    {
                        RtcpSocket = RtpSocket;
                    }
                    else if (IsRtcpEnabled)
                    {

                        //Create the RtcpSocket
                        RtcpSocket = new Socket(localRtp.Address.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

                        //Configure it
                        ConfigureSocket(RtcpSocket);

                        //Assign the RemoteRtcp EndPoint and Bind the socket to that EndPoint
                        await RtcpSocket.Connect(RemoteRtcp = remoteRtcp);

                        ////Handle Multicast joining (Might need to track interface)
                        //if (Extensions.IPEndPoint.IPEndPointExtensions.IsMulticast(remoteRtcp))
                        //{
                        //    Extensions.Socket.SocketExtensions.JoinMulticastGroup(RtcpSocket, remoteRtcp.Address, ttl);
                        //}

                        if (punchHole)
                        {
                            //new RtcpPacket(Version, Rtcp.ReceiversReport.PayloadType, 0, 0, SynchronizationSourceIdentifier, 0);
                            try
                            {
                                await SocketExtensions.SendTo(WakeUpBytes, 0, WakeUpBytes.Length, RtcpSocket, RemoteRtcp);
                                //RtcpSocket.SendTo(WakeUpBytes, 0, WakeUpBytes.Length, SocketFlags.None, RemoteRtcp);
                            }
                            catch (SocketException)
                            {
                                //The port was not open, allow the next recieve to determine the port
                                RemoteRtcp = new IPEndPoint(((IPEndPoint)RemoteRtcp).Address, 0);
                            }
                        }
                    }

                    //Setup the receive buffer size for all sockets of this context to use memory defined in excess of the context memory to ensure a high receive rate in udp
                    if (this.ContextMemory != null)
                    {
                        //Ensure the receive buffer size is updated for that context.
                        ISocketReferenceExtensions.SetReceiveBufferSize(((ISocketReference)this), 100 * this.ContextMemory.Count);
                    }

                }
                catch
                {
                    throw;
                }
            }

            #region Tcp

            /// <summary>
            /// Creates the required Tcp socket for the TransportContext and updates the assoicated Properties and Fields
            /// </summary>
            /// <param name="localIp"></param>
            /// <param name="remoteIp"></param>
            /// <param name="remotePort"></param>
            public void Initialize(IPAddress localIp, IPAddress remoteIp, int remotePort)
            {
                Initialize(new IPEndPoint(localIp, remotePort), new IPEndPoint(remoteIp, remotePort));
            }

            /// <summary>
            /// Creates a Tcp socket on from local to remote and sets the RtpSocket and RtcpSocket to that socket.
            /// </summary>
            /// <param name="local"></param>
            /// <param name="remote"></param>
            public async void Initialize(IPEndPoint local, IPEndPoint remote)
            {
                LocalRtp = local;

                RemoteRtp = remote;

                Socket socket = new Socket(local.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                ConfigureSocket(socket);

                await Initialize(socket);

                //This reference is not needed anymore
                socket = null;
            }

            /// <summary>
            /// Uses the given socket for the duplexed data
            /// </summary>
            /// <param name="duplexed">The socket to use</param>
            public async Task Initialize(Socket duplexed)
            {
                //If the socket is not exclusively using the address
                if (false == false)
                {
                    //Duplicte the socket's type for a Rtcp socket.
                    Socket rtcpSocket = new Socket(duplexed.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    //Configure the duplicate
                    ConfigureSocket(rtcpSocket);

                    //Initialize with the duplicate socket
                    await Initialize(duplexed, rtcpSocket);

                    //This reference is no longer needed.
                    rtcpSocket = null;
                }
                else
                    await Initialize(duplexed, duplexed); //Otherwise use the existing socket twice
            }

            #endregion

            #region Existing Sockets (Could be Mixed Tcp and Udp)

            /// <summary>
            /// Used to provide sockets which are already bound and connected for use in rtp and rtcp operations
            /// </summary>
            /// <param name="rtpSocket"></param>
            /// <param name="rtcpSocket"></param>
            public async Task Initialize(Socket rtpSocket, Socket rtcpSocket)
            {
                if (IsDisposed || IsActive) return;
                if (rtpSocket == null) throw new ArgumentNullException("rtpSocket");
                if (rtcpSocket == null) throw new ArgumentNullException("rtcpSocket");

                m_Initialized = DateTime.UtcNow;

                RtpSocket = rtpSocket;
                RtcpSocket = rtcpSocket;

                if (LocalRtp == null) LocalRtp = null;

                if (RemoteRtp == null)
                    RemoteRtp = RtpSocket.RemoteEndPoint;

                if (RemoteRtcp == null)
                {
                    RemoteRtcp = RtcpSocket.RemoteEndPoint;
                }

                if (RemoteRtp != null && false == RtpSocket.Connected) try
                    {
                        await RtpSocket.Connect(RemoteRtp);
                    }
                    catch { }

                AssignIdentity();

                Goodbye = null;
            }

            #endregion

            #endregion

            /// <summary>
            /// Receives data on the given socket
            /// </summary>
            /// <param name="buffer"></param>
            /// <param name="offset"></param>
            /// <param name="count"></param>
            /// <param name="socket"></param>
            /// <param name="remote"></param>
            /// <returns>The amount of bytes received</returns>

            /// <summary>
            /// Closes the Rtp and Rtcp Sockets
            /// </summary>
            public void DisconnectSockets()
            {
                if (false == IsActive || IsDisposed) return;

                if (LeaveOpen)
                {
                    //Maybe should drop multicast group....

                    RtpSocket = RtcpSocket = null;
                }
                else
                {

                    //Maybe should drop multicast groups...

                    //For Udp the RtcpSocket may be the same socket as the RtpSocket if the sender/reciever is duplexing
                    if (RtcpSocket != null && RtpSocket != RtcpSocket) RtcpSocket.Close();

                    //Close the RtpSocket
                    if (RtpSocket != null) RtpSocket.Close();

                    RtpSocket = RtcpSocket = null;
                }

                //Remove the end points
                LocalRtp = LocalRtcp = RemoteRtp = RemoteRtcp = null;

                //Why erase stats?
                //m_FirstPacketReceived = DateTime.MinValue;

                //m_FirstPacketSent = DateTime.MinValue;
            }

            /// <summary>
            /// Resets the RemoteSynchronizationSourceIdentifier and packet counters values.
            /// </summary>
            internal void ResetState()
            {
                if (RemoteSynchronizationSourceIdentifier.HasValue) RemoteSynchronizationSourceIdentifier = null;// default(int);

                //Set all to 0
                RfcRtpBytesSent = RtpPacketsSent = RtpBytesSent = RtcpPacketsSent =
                    RtcpBytesSent = RtpPacketsReceived = RtpBytesRecieved = RtcpBytesRecieved =
                        RtcpPacketsReceived = m_FailedRtcpTransmissions = m_FailedRtpTransmissions = m_FailedRtcpReceptions = m_FailedRtpReceptions = 0;
            }

            /// <summary>
            /// Disposes the TransportContext and all underlying resources.
            /// </summary>
            public override void Dispose()
            {
                if (IsDisposed) return;

                base.Dispose();

                //If the instance should dispose
                if (ShouldDispose)
                {
                    //Disconnect sockets
                    DisconnectSockets();

                    //Remove references to the context memory and the application context
                    ContextMemory = null;

                    ApplicationContext = null;
                }
            }

            #endregion

            IEnumerable<Socket> ISocketReference.GetReferencedSockets()
            {
                if (IsDisposed) yield break;

                if (RtpSocket != null)
                {
                    yield return RtpSocket;

                    if (RtpSocket.ProtocolType == ProtocolType.Tcp || IsDuplexing) yield break;
                }

                if (RtcpSocket != null) yield return RtcpSocket;
            }
        }

        #endregion

        #region Fields

        //Buffer for data
        //Used in ReceiveData, Each TransportContext gets a chance to receive into the buffer, when the recieve completes the data is parsed if there is any then the next TransportContext goes.
        //Doing this in parallel doesn't really offset much because the decoder must be able to handle the data and if you back log the decoder you are just wasting cycles.        
        internal MemorySegment m_Buffer;

        //Each session gets its own thread to send and recieve
        internal Thread m_WorkerThread; // and possibly another for events.

        //This signal determines if the workers will continue each iteration, it may be possible to use int to signal various other states.
        internal bool m_StopRequested, //on or off right now, int could allow levels of threading..
            m_IListSockets; //Indicates if to use the IList send overloads.

        //Collection to handle the dispatch of events.
        //Notes that Collections.Concurrent.Queue may be better suited for this in production until the ConcurrentLinkedQueue has been thoroughly engineered and tested.
        //The context, the item, final, recieved
        readonly ConcurrentLinkedQueue<Tuple<TransportContext, BaseDisposable, bool, bool>> m_EventData = new ConcurrentLinkedQueue<Tuple<TransportContext, BaseDisposable, bool, bool>>();

        //Todo, LinkedQueue and Clock.
        readonly ManualResetEvent m_EventReady = new ManualResetEvent(false); //should be caluclated based on memory and speed. SpinWait uses 10 as a default.

        //Outgoing Packets, Not a Queue because you cant re-order a Queue (in place) and you can't take a range from the Queue (in a single operation)
        //Those things aside, ordering is not performed here and only single packets are iterated and would eliminate the need for removing after the operation.
        //Benchmark with Queue and ConcurrentQueue and a custom impl.
        //IPacket could also work in an implementaiton which sends evertyhing in the outgoing list at one time.
        internal readonly List<RtpPacket> m_OutgoingRtpPackets = new List<RtpPacket>();
        internal readonly List<RtcpPacket> m_OutgoingRtcpPackets = new List<RtcpPacket>();

        /// <summary>
        /// Any TransportContext's which are added go here for removal. This list can never be null.
        /// </summary>
        /// <notes>This possibly should be sorted but sorted lists cannot contain duplicates.</notes>
        internal readonly List<TransportContext> TransportContexts = new List<TransportContext>();

        /// <summary>
        /// Unique id assigned to each RtpClient instance. (16 byte overhead)
        /// </summary>
        internal readonly Guid InternalId = Guid.NewGuid();

        #endregion

        #region Events

        /// <summary>
        /// 非实时视频 数据接收完毕
        /// </summary>
        public Action NotContiniousReceiveFinishedAction;

        /// <summary>
        /// Provides a function signature which is used to process data at a given offset and length.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public delegate void InterleavedDataHandler(object sender, byte[] data, int offset, int length);

        /// <summary>
        /// Provides a funtion signature which is used to process RtpPacket's
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="packet"></param>
        /// <param name="tc"></param>
        public delegate void RtpPacketHandler(object sender, RtpPacket packet = null, TransportContext tc = null);

        /// <summary>
        /// Provides a function signature which is used to process RtcpPacket's
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="packet"></param>
        /// <param name="tc"></param>
        public delegate void RtcpPacketHandler(object sender, RtcpPacket packet = null, TransportContext tc = null);

        /// <summary>
        /// Provides a function signature which is used to process RtpFrame's
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="frame"></param>
        /// <param name="tc"></param>
        /// <param name="final"></param>
        public delegate void RtpFrameHandler(object sender, RtpFrame frame = null, TransportContext tc = null, bool final = false);

        /// <summary>
        /// Raised when Interleaved Data is recieved.
        /// Interleaved data is usually on received when using TCP.
        /// </summary>
        public event InterleavedDataHandler InterleavedData;

        /// <summary>
        /// Raised when a RtpPacket is received
        /// </summary>
        public event RtpPacketHandler RtpPacketReceieved;

        /// <summary>
        /// Raised when a RtcpPacket is received
        /// </summary>
        public event RtcpPacketHandler RtcpPacketReceieved;

        /// <summary>
        /// Raised when a RtpPacket has been sent
        /// </summary>
        public event RtpPacketHandler RtpPacketSent;

        /// <summary>
        /// Raised when a RtcpPacket has been sent
        /// </summary>
        public event RtcpPacketHandler RtcpPacketSent;

        /// <summary>
        /// Raised when a complete RtpFrame was changed due to a packet being added, removed or updated.
        /// </summary>
        public event RtpFrameHandler RtpFrameChanged;

        protected internal virtual async Task HandleIncomingRtcpPacket(object rtpClient, RtcpPacket packet)
        {
            if (RtspLogSwitch.IsLogIncomingRtcpPacket)
            {
                if (packet.PayloadType == SendersReport.PayloadType)
                {
                    LoggerInstance.Debug("接收到Rtcp  " + new SendersReport(packet).ToString());
                }
                else if (packet.PayloadType == ReceiversReport.PayloadType)
                {
                    LoggerInstance.Debug("接收到Rtcp  " + new ReceiversReport(packet).ToString());
                }
                else if (packet.PayloadType == SourceDescriptionReport.PayloadType)
                {
                    LoggerInstance.Debug("接收到Rtcp  " + new SourceDescriptionReport(packet).ToString());
                }
                else if (packet.PayloadType == GoodbyeReport.PayloadType)
                {
                    LoggerInstance.Debug("接收到Rtcp  " + new GoodbyeReport(packet).ToString());
                }
            }

            //Determine if the packet can be handled
            if (IsDisposed || false == RtcpEnabled || IDisposedExtensions.IsNullOrDisposed(packet)) return;

            //Get a context for the packet by the identity of the receiver
            TransportContext transportContext = null;

            int packetLength = packet.Length;

            //Cache the ssrc of the packet's sender.
            int partyId = packet.SynchronizationSourceIdentifier,
                packetVersion = packet.Version;

            //See if there is a context for the remote party. (Allows 0)
            transportContext = GetContextBySourceId(partyId);

            //Raise an event for the rtcp packet received.
            OnRtcpPacketReceieved(packet, transportContext);

            //Compressed or no ssrc
            if (packet.IsCompressed || packetLength < Binary.BytesPerLong || false == HandleIncomingRtcpPackets)
            {
                return;
            }
            else if (transportContext != null && transportContext.Version != packetVersion)
            {
                LoggerInstance.Error(InternalId + "HandleIncomingRtcpPacket Invalid Version, Found =>" + packetVersion + ", Expected =>" + transportContext.Version);
                return;
            }

            //Only if the packet was not addressed to a unique party with the id of 0
            if (partyId != 0 && (transportContext == null || transportContext.InDiscovery)) //The remote party has not yet been identified.
            {
                //Cache the payloadType and blockCount
                int blockCount = packet.BlockCount;

                //Before checking the type ensure there is a party id and block count
                if (blockCount == 0)
                {
                    //Attempt to obtain a context by the identifier in the report block
                    transportContext = GetContextBySourceId(partyId);

                    //If there was a context and the remote party has not yet been identified.
                    if (transportContext != null &&
                        transportContext.InDiscovery &&
                        transportContext.Version == packetVersion)
                    {
                        //Identify the remote party by this id.
                        transportContext.RemoteSynchronizationSourceIdentifier = partyId;

                        LoggerInstance.Error(ToString() + "@HandleIncomingRtcpPacket Set RemoteSynchronizationSourceIdentifier @ " + transportContext.SynchronizationSourceIdentifier + " to=" + transportContext.RemoteSynchronizationSourceIdentifier + "RR blockId=" + partyId);
                    }

                    return;
                }

                //Check the type because there is at least 1 block
                int payloadType = packet.PayloadType;

                if (payloadType == ReceiversReport.PayloadType)
                {
                    //Create a wrapper around the packet to access the ReportBlocks
                    using (ReceiversReport rr = new ReceiversReport(packet, false))
                    {
                        //Iterate each contained ReportBlock
                        foreach (IReportBlock reportBlock in rr)
                        {
                            int blockId = reportBlock.BlockIdentifier;

                            //Attempt to obtain a context by the identifier in the report block
                            transportContext = GetContextBySourceId(blockId);

                            //If there was a context and the remote party has not yet been identified.
                            if (transportContext != null &&
                                transportContext.InDiscovery &&
                                transportContext.Version == packetVersion)
                            {
                                //Identify the remote party by this id.
                                transportContext.RemoteSynchronizationSourceIdentifier = blockId;

                                //Check packet loss...

                                LoggerInstance.Error(ToString() + "@HandleIncomingRtcpPacket Set RemoteSynchronizationSourceIdentifier @ RR " + transportContext.SynchronizationSourceIdentifier + " to=" + transportContext.RemoteSynchronizationSourceIdentifier + "RR blockId=" + blockId);

                                //Stop looking for a context.
                                break;
                            }
                        }
                    }
                }
                else if (payloadType == GoodbyeReport.PayloadType) //The GoodbyeReport report from a remote party
                {
                    //Create a wrapper around the packet to access the source list
                    using (GoodbyeReport gb = new GoodbyeReport(packet, false))
                    {
                        using (RFC3550.SourceList sourceList = gb.GetSourceList())
                        {
                            //Iterate each party leaving
                            foreach (int party in sourceList)
                            {
                                //Attempt to obtain a context by the identifier in the report block
                                transportContext = GetContextBySourceId(party);

                                //If there was a context
                                if (transportContext != null &&
                                    false == transportContext.IsDisposed &&
                                    transportContext.Version == packetVersion)
                                {
                                    //Send report now if possible.
                                    bool reportsSent = await SendReports(transportContext);

                                    LoggerInstance.Error(ToString() + "@HandleIncomingRtcpPacket Recieved Goodbye @ " + transportContext.SynchronizationSourceIdentifier + " from=" + partyId + " reportSent=" + reportsSent);

                                    //Stop looking for a context.
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (payloadType == SendersReport.PayloadType) //The senders report from a remote party                    
                {
                    //Create a wrapper around the packet to access the ReportBlocks
                    using (SendersReport sr = new SendersReport(packet, false))
                    {
                        LoggerInstance.Debug(sr.ToString());

                        //Iterate each contained ReportBlock
                        foreach (IReportBlock reportBlock in sr)
                        {
                            int blockId = reportBlock.BlockIdentifier;

                            //Attempt to obtain a context by the identifier in the report block
                            transportContext = GetContextBySourceId(blockId);

                            //If there was a context and the remote party has not yet been identified.
                            if (transportContext != null &&
                                transportContext.InDiscovery &&
                                transportContext.Version == packetVersion)
                            {
                                //Identify the remote party by this id.
                                transportContext.RemoteSynchronizationSourceIdentifier = blockId;

                                //Check packet loss...

                                LoggerInstance.Error(ToString() + "@HandleIncomingRtcpPacket Set RemoteSynchronizationSourceIdentifier @ SR " + transportContext.SynchronizationSourceIdentifier + " to=" + transportContext.RemoteSynchronizationSourceIdentifier + "RR blockId=" + blockId);

                                //Stop looking for a context.
                                break;
                            }
                        }
                    }
                }
            }

            //If no transportContext could be found
            if (transportContext == null)
            {
                //Attempt to see if this was a rtp packet by using the RtpPayloadType
                int rtpPayloadType = packet.Header.First16Bits.RtpPayloadType;

                if (rtpPayloadType == 13 || (transportContext = GetContextByPayloadType(rtpPayloadType)) != null)
                {
                    LoggerInstance.Error(InternalId + "HandleIncomingRtcpPacket - Incoming RtcpPacket actually was Rtp. Ssrc= " + partyId + " Type=" + rtpPayloadType + " Len=" + packet.Length);

                    //Raise an event for the 'RtpPacket' received. 
                    //Todo Use the existing reference / memory of the RtcpPacket) or provide an implicit way to cast 
                    using (RtpPacket rtp = new RtpPacket(packet.Prepare().ToArray(), 0))
                    {
                        OnRtpPacketReceieved(rtp, transportContext);
                    }

                    //Don't do anything else
                    return;
                }

                LoggerInstance.Error(InternalId + "HandleIncomingRtcpPacket - No Context for packet " + partyId + "@" + packet.PayloadType);
                return;
            }
            //If there is a collision in the unique identifiers
            if (transportContext.SynchronizationSourceIdentifier == partyId)
            {
                //Handle it.
                await HandleIdentityCollision(transportContext);
            }

            //Last Rtcp packet was received right now now.
            transportContext.m_LastRtcpIn = packet.Created;

            //The context is active.
            transportContext.m_InactiveTime = TimeSpanExtensions.InfiniteTimeSpan;

            //Don't worry about overflow
            unchecked
            {
                //Increment packets received for the valid context.
                ++transportContext.RtcpPacketsReceived;

                //Keep track of the the bytes sent in the context
                transportContext.RtcpBytesRecieved += packet.Length;

                //Set the time when the first rtcp packet was recieved
                if (transportContext.m_FirstPacketReceived == DateTime.MinValue) transportContext.m_FirstPacketReceived = packet.Created;
            }
        }

        protected internal virtual async Task HandleIdentityCollision(TransportContext transportContext)
        {

            if (transportContext == null) throw new ArgumentNullException("transportContext");

            if (transportContext.IsDisposed) throw new ObjectDisposedException("transportContext");

            LoggerInstance.Error(InternalId + "HandleCollision - Ssrc=" + transportContext.SynchronizationSourceIdentifier + " - RSsrc=" + transportContext.RemoteSynchronizationSourceIdentifier);

            //Send a goodbye and indicate why.
            await SendGoodbye(transportContext, System.Text.Encoding.UTF8.GetBytes("ssrc"));

            //Assign a new random ssrc which is not equal to the remote parties.
            //Noting that you could use the same ssrc +/-N here also or a base from the number of parties etc.

            //This may deserve an event, 'OnCollision'

            do transportContext.SynchronizationSourceIdentifier = RFC3550.Random32(transportContext.SynchronizationSourceIdentifier);
            while (transportContext.SynchronizationSourceIdentifier == transportContext.RemoteSynchronizationSourceIdentifier);

            //Reset counters from this point forward
            transportContext.ResetState();

            //reset counters?
        }

        /// <summary>
        /// Updates counters and fires a FrameChanged event if required.
        /// </summary>
        /// <param name="sender">The object which raised the event</param>
        /// <param name="packet">The RtpPacket to handle</param>
        protected internal virtual async void HandleIncomingRtpPacket(object sender, RtpPacket packet)
        {
            if (RtspLogSwitch.IsLogIncomingRtpPacket)
            {
                LoggerInstance.Debug("接收到RTP数据 ：" + packet.ToString());
            }

            //Determine if the incoming packet CAN be handled
            if (false == RtpEnabled || IsDisposed || IDisposedExtensions.IsNullOrDisposed(packet)) return;

            TransportContext transportContext = GetContextForPacket(packet);

            //Raise the event for the packet.
            OnRtpPacketReceieved(packet, transportContext);

            //If the client shouldn't handle the packet then return.            
            if (false == HandleIncomingRtpPackets || packet.IsCompressed)
            {
                return;
            }

            #region TransportContext Handles Packet

            //If the context is still null
            if (transportContext == null)
            {
                LoggerInstance.Error(InternalId + "HandleIncomingRtpPacket Unaddressed RTP Packet " + packet.SynchronizationSourceIdentifier + " PT =" + packet.PayloadType + " len =" + packet.Length);
                return;
            }

            //Cache the ssrc
            int partyId = packet.SynchronizationSourceIdentifier;

            //Check for a collision
            if (partyId == transportContext.SynchronizationSourceIdentifier)
            {
                //Handle it
                await HandleIdentityCollision(transportContext);
            }

            //Don't worry about overflow.
            unchecked
            {
                int packetLength = packet.Length;

                //If the packet sequence number is not valid
                if (false == transportContext.ValidatePacketAndUpdateSequenceNumber(packet))
                {
                    //Increment for a failed reception, possibly rename to invalid packets.
                    ++transportContext.m_FailedRtpReceptions;

                    LoggerInstance.Error(InternalId + "HandleIncomingRtpPacket Failed Reception " +
                             "(= " + transportContext.m_FailedRtpReceptions + ") @" + transportContext.SynchronizationSourceIdentifier +
                             " Context seq=" + transportContext.RecieveSequenceNumber +
                             " Packet pt=" + packet.PayloadType +
                            " seq=" + packet.SequenceNumber +
                            " len= " + packet.Length);

                    //Todo, event for OutOfBand(RtpPacket)
                }
                else ++transportContext.ValidRtpPacketsReceived; //Increase the amount of valid rtp packets recieved when ValidatePacketAndUpdateSequenceNumber is true

                //Increment RtpPacketsReceived for the context relating to the packet.
                ++transportContext.RtpPacketsReceived;

                //If InDiscovery and IsValid then stop InDiscovery by setting the remote id
                if (transportContext.InDiscovery && transportContext.IsValid)
                    transportContext.RemoteSynchronizationSourceIdentifier = partyId;

                transportContext.RtpBytesRecieved += packet.Payload.Count;

                transportContext.RfcRtpBytesRecieved += packetLength - (packet.Header.Size + packet.HeaderOctets + packet.PaddingOctets);

                //Set the time when the first RtpPacket was received if required
                if (transportContext.m_FirstPacketReceived == DateTime.MinValue) transportContext.m_FirstPacketReceived = packet.Created;

                //Update the SequenceNumber and Timestamp and calulcate Inter-Arrival (Mark the context as active)
                transportContext.UpdateJitterAndTimestamp(packet);

                //Set the last rtp in after inter-arrival has been calculated.
                transportContext.m_LastRtpIn = packet.Created;

                //If the instance does not handle frame changed events then return
                if (false == HandleFrameChanges) return;

                #region HandleFrameChanges

                int packetTimestamp = packet.Timestamp;

                //If a CurrentFrame was not allocated
                if (transportContext.CurrentFrame == null)
                {
                    //Todo, Clone
                    //make a frame with the copy of the packet
                    transportContext.CurrentFrame = new RtpFrame(packet.Clone(true, true, true, true, false));

                    //The LastFrame changed
                    OnRtpFrameChanged(transportContext.CurrentFrame, transportContext);

                    return;

                }//Check to see if the frame belongs to the last frame
                else if (false == IDisposedExtensions.IsNullOrDisposed(transportContext.LastFrame) &&
                    packetTimestamp == transportContext.LastFrame.Timestamp)
                {
                    //If the packet was added to the frame
                    if (transportContext.LastFrame.TryAdd(packet.Clone(true, true, true, true, false)))
                    {
                        bool final = transportContext.LastFrame.Count >= transportContext.LastFrame.MaxPackets;

                        //The LastFrame changed so fire an event
                        OnRtpFrameChanged(transportContext.LastFrame, transportContext, final);

                        //Backup of frames
                        if (final)
                        {
                            transportContext.LastFrame.Dispose();

                            transportContext.LastFrame = null;
                        }
                    }
                    else
                    {
                        //Could jump to case log
                        LoggerInstance.Error(InternalId + "HandleFrameChanges => transportContext.LastFrame@TryAdd failed, RecieveSequenceNumber = " + transportContext.RecieveSequenceNumber + ", Timestamp = " + packetTimestamp + ". HasMarker = " + transportContext.LastFrame.HasMarker);
                    }

                    return;

                }//Check to see if the frame belongs to a new frame
                else if (false == IDisposedExtensions.IsNullOrDisposed(transportContext.CurrentFrame) &&
                    packetTimestamp != transportContext.CurrentFrame.Timestamp)
                {
                    //Dispose the last frame, it's going out of scope.
                    if (transportContext.LastFrame != null)
                    {
                        //Indicate the frame is going out of scope
                        OnRtpFrameChanged(transportContext.LastFrame, transportContext, true);

                        transportContext.LastFrame.Dispose();

                        transportContext.LastFrame = null;
                    }

                    //Move the current frame to the LastFrame
                    transportContext.LastFrame = transportContext.CurrentFrame;

                    //make a frame with the copy of the packet
                    transportContext.CurrentFrame = new RtpFrame(packet.Clone(true, true, true, true, false));

                    //The current frame changed
                    OnRtpFrameChanged(transportContext.CurrentFrame, transportContext);

                    return;
                }//Check to see if the frame belongs to the current frame
                else if (false == IDisposedExtensions.IsNullOrDisposed(transportContext.CurrentFrame) &&
                   packetTimestamp == transportContext.CurrentFrame.Timestamp)
                {
                    //If the packet was added to the frame
                    if (transportContext.CurrentFrame.TryAdd(packet.Clone(true, true, true, true, false)))
                    {
                        bool final = transportContext.CurrentFrame.Count >= transportContext.CurrentFrame.MaxPackets;

                        //The CurrentFrame changed
                        OnRtpFrameChanged(transportContext.CurrentFrame, transportContext, final);

                        //Backup of frames
                        if (final)
                        {
                            transportContext.LastFrame.Dispose();

                            transportContext.LastFrame = null;
                        }

                    }
                    else
                    {
                        LoggerInstance.Error(InternalId + "HandleFrameChanges => transportContext.CurrentFrame@TryAdd failed, RecieveSequenceNumber = " + transportContext.RecieveSequenceNumber + ", Timestamp = " + packetTimestamp + ". HasMarker = " + transportContext.CurrentFrame.HasMarker);
                    }

                    return;
                }

                LoggerInstance.Error(InternalId + "HandleIncomingRtpPacket HandleFrameChanged @ " + packetTimestamp + " Does not belong to any frame.");

                #endregion
            }

            #endregion
        }

        /// <summary>
        /// Handles the logic of updating counters for the packet sent if <see cref="OutgoingRtcpPacketEventsEnabled"/> is true.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="packet"></param>OutgoingRtcpPacketEventsEnabled
        protected internal virtual void HandleOutgoingRtcpPacket(object sender, RtcpPacket packet = null, TransportContext tc = null)
        {
            if (RtspLogSwitch.IsLogOutgoingRtcpPacket)
            {
                LoggerInstance.Debug("发送RTCP数据 ：\n" + packet.ToString());
            }

            if (IsDisposed || IDisposedExtensions.IsNullOrDisposed(packet) || false == HandleOutgoingRtcpPackets || false == packet.Transferred.HasValue) return;

            #region TransportContext Handles Packet

            TransportContext transportContext = tc ?? GetContextForPacket(packet);

            //if there is no context there is nothing to do.
            if (transportContext == null) return;

            unchecked
            {
                //Update the counters for the amount of bytes in the RtcpPacket including the header and any padding.
                transportContext.RtcpBytesSent += packet.Length;

                //Update the amount of packets sent
                ++transportContext.RtcpPacketsSent;

                //Mark the context as active immediately.
                transportContext.m_InactiveTime = TimeSpanExtensions.InfiniteTimeSpan;

                //Get the time the packet was sent
                DateTime sent = packet.Transferred.Value;

                //Store the last time a RtcpPacket was sent
                transportContext.m_LastRtcpOut = sent;

                //Set the time the first packet was sent.
                if (transportContext.m_FirstPacketSent == DateTime.MinValue) transportContext.m_FirstPacketSent = sent;

                //Attempt to raise the event
                OnRtcpPacketSent(packet, transportContext);
            }

            //Backoff based on ConverganceTime?

            #endregion
        }

        protected internal void OnInterleavedData(byte[] data, int offset, int length)
        {
            if (IsDisposed) return;

            InterleavedDataHandler action = InterleavedData;

            if (action == null || data == null || length == 0) return;

            foreach (InterleavedDataHandler handler in action.GetInvocationList())
            {
                LoggerInstance.Debug("在接收RTP数据的过程中，收到Message");
                try { handler(this, data, offset, length); }
                catch (Exception ex) { LoggerInstance.Exception(ex); }
            }
        }

        private int lineCount;

        /// <summary>
        /// Raises the RtpPacket Handler for Recieving
        /// </summary>
        /// <param name="packet">The packet to handle</param>
        protected internal void OnRtpPacketReceieved(RtpPacket packet, TransportContext tc = null)
        {
            if (RtspLogSwitch.IsLogRtpDataWith0x)
            {
                var dataBuffer = packet.PayloadData.ToArray();

                var sb = new StringBuilder();
                sb.Append(lineCount.ToString().PadLeft(5, '0') + " : ");
                for (int i = 0; i < dataBuffer.Length; i++)
                {
                    var item = dataBuffer[i];
                    sb.Append(item.ToString("X2") + " ");
                    if ((i + 1) % 188 == 0)
                    {
                        LoggerInstance.Debug(sb.ToString());
                        sb = new StringBuilder();
                        lineCount++;
                        sb.Append(lineCount.ToString().PadLeft(5, '0') + " : ");
                    }
                }
            }

            if (IsDisposed || false == IncomingRtpPacketEventsEnabled) return;

            RtpPacketHandler action = RtpPacketReceieved;

            if (action == null || IDisposedExtensions.IsNullOrDisposed(packet)) return;

            bool shouldDispose = packet.ShouldDispose;

            if (shouldDispose) SetShouldDispose(packet, false, false);

            foreach (RtpPacketHandler handler in action.GetInvocationList())
            {
                if (packet.IsDisposed) break;
                try { handler(this, packet, tc); }
                catch (Exception ex) { LoggerInstance.Exception(ex); }
            }

            //Allow the packet to be destroyed if an event did not already change this.
            if (shouldDispose && packet.ShouldDispose == false) BaseDisposable.SetShouldDispose(packet, true, false);
        }

        /// <summary>
        /// Raises the RtcpPacketHandler for Recieving
        /// </summary>
        /// <param name="packet">The packet to handle</param>
        protected internal void OnRtcpPacketReceieved(RtcpPacket packet = null, TransportContext tc = null)
        {
            if (IsDisposed || false == IncomingRtcpPacketEventsEnabled) return;

            RtcpPacketHandler action = RtcpPacketReceieved;

            if (action == null || IDisposedExtensions.IsNullOrDisposed(packet)) return;

            bool shouldDispose = packet.ShouldDispose;

            if (shouldDispose) SetShouldDispose(packet, false, false);

            foreach (RtcpPacketHandler handler in action.GetInvocationList())
            {
                if (packet.IsDisposed) break;
                try { handler(this, packet, tc); }
                catch (Exception ex) { LoggerInstance.Exception(ex); }
            }

            //Allow the packet to be destroyed if an event did not already change this.
            if (shouldDispose && packet.ShouldDispose == false) BaseDisposable.SetShouldDispose(packet, true, false);
        }

        /// <summary>
        /// Raises the RtpFrameHandler for the given frame if FrameEvents are enabled
        /// </summary>
        /// <param name="frame">The frame to raise the RtpFrameHandler with</param>
        internal protected void OnRtpFrameChanged(RtpFrame frame = null, TransportContext tc = null, bool final = false)
        {
            if (IsDisposed || false == FrameChangedEventsEnabled) return;

            RtpFrameHandler action = RtpFrameChanged;

            if (action == null || IDisposedExtensions.IsNullOrDisposed(frame) || frame.IsEmpty) return;

            bool shouldDispose = frame.ShouldDispose;

            if (shouldDispose) SetShouldDispose(frame, false, false);

            foreach (RtpFrameHandler handler in action.GetInvocationList())
            {
                if (IDisposedExtensions.IsNullOrDisposed(frame)) break;
                try { handler(this, frame, tc, final); }
                catch (Exception ex) { LoggerInstance.Exception(ex); }
            }

            //On final events set ShouldDispose to true, do not call Dispose
            if (final && shouldDispose && frame.ShouldDispose == false) BaseDisposable.SetShouldDispose(frame, true, false);
        }

        /// <summary>
        /// Raises the RtcpPacketHandler for Sending
        /// </summary>
        /// <param name="packet">The packet to handle</param>
        internal protected void OnRtcpPacketSent(RtcpPacket packet, TransportContext tc = null)
        {
            if (IsDisposed || false == OutgoingRtcpPacketEventsEnabled) return;

            RtcpPacketHandler action = RtcpPacketSent;

            if (action == null || IDisposedExtensions.IsNullOrDisposed(packet)) return;

            foreach (RtcpPacketHandler handler in action.GetInvocationList())
            {
                if (IDisposedExtensions.IsNullOrDisposed(packet)) break;
                try { handler(this, packet, tc); }
                catch (Exception ex) { LoggerInstance.Exception(ex); }
            }
        }

        //Frame sent.

        #endregion

        #region Properties

        /// <summary>
        /// Used in applications to determine send thresholds.
        /// </summary>
        public int MaximumOutgoingPackets { get; internal protected set; }

        /// <summary>
        /// Gets the number of RtpPacket instances queued to be sent.
        /// </summary>
        public int OutgoingRtpPacketCount
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get
            {
                return m_OutgoingRtpPackets.Count;
            }
        }

        /// <summary>
        /// Gets or sets a value which indicates if the socket operations for sending will use the IList overloads.
        /// </summary>
        public bool IListSockets
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get
            {
                return m_IListSockets;
            }

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            set
            {
                //Todo, the objects may be in use on the curent call
                //if (m_ThreadEvents)
                //{

                //}

                m_IListSockets = value;
            }
        }

        public Action<Thread> ConfigureThread { get; set; }

        /// <summary>
        /// The maximum amount of bandwidth Rtcp can utilize (of the overall bandwidth available to the RtpClient) during reports
        /// </summary>
        public double AverageMaximumRtcpBandwidthPercentage { get; set; }

        /// <summary>
        /// Gets or sets a value which allows the OnRtpPacketEvent to be raised.
        /// </summary>
        public bool IncomingRtpPacketEventsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value which allows the OnRtcpPacketEvent to be raised.
        /// </summary>
        public bool IncomingRtcpPacketEventsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value which allows the OnRtpPacketSent to be raised.
        /// </summary>
        public bool OutgoingRtpPacketEventsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value which allows the OnRtcpPacketSent to be raised.
        /// </summary>
        public bool OutgoingRtcpPacketEventsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value which allows the instance to handle any incoming RtpPackets
        /// </summary>
        public bool HandleIncomingRtpPackets { get; set; }

        /// <summary>
        /// Gets or sets a value which allows the instance to handle any incoming RtcpPackets
        /// </summary>
        public bool HandleIncomingRtcpPackets { get; set; }

        /// <summary>
        /// Gets or sets a value which allows the instance to handle any outgoing RtpPackets
        /// </summary>
        public bool HandleOutgoingRtpPackets { get; set; }

        /// <summary>
        /// Gets or sets a value which allows the instance to handle any outgoing RtcpPackets
        /// </summary>
        public bool HandleOutgoingRtcpPackets { get; set; }

        /// <summary>
        /// Gets or sets a value which prevents <see cref="RtpFrameChanged"/> from being fired.
        /// </summary>
        public bool FrameChangedEventsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value which allows the instance to create a RtpFrame based on the incoming rtp packets.
        /// </summary>
        public bool HandleFrameChanges { get; set; }

        /// <summary>
        /// Gets or sets the value will be used as the CName when creating RtcpReports
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets the list of additional items which will be sent with the SourceDescriptionReport if AverageRtcpBandwidthExceeded is not exceeded.
        /// </summary>
        public readonly List<SourceDescriptionReport.SourceDescriptionItem> AdditionalSourceDescriptionItems = new List<SourceDescriptionReport.SourceDescriptionItem>();

        /// <summary>
        /// Gets a value indicating if the RtpClient is not disposed and the WorkerThread is alive.
        /// </summary>
        public virtual bool IsActive
        {
            get
            {
                return false == IsDisposed && Started != DateTime.MinValue && m_WorkerThread != null && (m_WorkerThread.IsAlive || false == m_StopRequested);
            }
        }

        /// <summary>
        /// Gets a value which indicates if any underlying <see cref="RtpClient.TransportContext"/> owned by this RtpClient instance utilizes Rtcp.
        /// </summary>
        public bool RtcpEnabled { get { return TransportContexts.Any(c => c.IsRtcpEnabled); } }

        /// <summary>
        /// Gets a value which indicates if any underlying <see cref="RtpClient.TransportContext"/> owned by this RtpClient instance utilizes Rtcp.
        /// </summary>
        public bool RtpEnabled { get { return TransportContexts.Any(c => c.IsRtpEnabled); } }

        /// <summary>
        /// Indicates if the amount of bandwith currently utilized for Rtcp reporting has exceeded the amount of bandwidth allowed by the <see cref="AverageMaximumRtcpBandwidthPercentage"/> property.
        /// </summary>
        public bool AverageRtcpBandwidthExceeded
        {
            get
            {
                if (false == RtcpEnabled || IsDisposed) return true;

                //If disposed no limit is imposed do not check
                if (AverageMaximumRtcpBandwidthPercentage == 0) return false;

                int amountOfContexts = TransportContexts.Count();

                if (amountOfContexts == 0) return true;

                //Obtain the summation of the total bytes sent over the amount of context's
                long totalReceived = TotalBytesReceieved;

                if (totalReceived == 0) return false;

                long totalRtcp = TotalRtcpBytesSent + TotalRtcpBytesReceieved;

                if (totalRtcp == 0) return false;

                return totalRtcp >= totalReceived / AverageMaximumRtcpBandwidthPercentage;
            }
        }

        #region Bandwidth and Uptime and Counters

        /// <summary>
        /// The Date and Time the RtpClient was Connected
        /// </summary>
        public DateTime Started { get; private set; }

        public DateTime EventsStarted { get; private set; }

        /// <summary>
        /// The amount of time the RtpClient has been recieving media
        /// </summary>
        public TimeSpan Uptime { get { return DateTime.UtcNow - Started; } }

        /// <summary>
        /// The total amount of RtpPackets sent of all contained TransportContexts
        /// </summary>
        public long TotalRtpPacketsSent { get { return IsDisposed ? 0 : TransportContexts.Sum(c => c.RtpPacketsSent); } }

        /// <summary>
        /// The total amount of Rtp bytes sent of all contained TransportContexts
        /// </summary>
        public long TotalRtpBytesSent { get { return IsDisposed ? 0 : TransportContexts.Sum(c => c.TotalRtpBytesSent); } }

        /// <summary>
        /// The total amount of Rtp bytes received of all contained TransportContexts
        /// </summary>
        public long TotalRtpBytesReceieved { get { return IsDisposed ? 0 : TransportContexts.Sum(c => c.TotalRtpBytesReceieved); } }

        /// <summary>
        /// The total amount of Rtp packets received of all contained TransportContexts
        /// </summary>
        public long TotalRtpPacketsReceieved { get { return IsDisposed ? 0 : TransportContexts.Sum(c => c.RtpPacketsReceived); } }

        /// <summary>
        /// The total amount of Rtcp packets sent of all contained TransportContexts
        /// </summary>
        public long TotalRtcpPacketsSent { get { return IsDisposed ? 0 : TransportContexts.Sum(c => c.RtcpPacketsSent); } }

        /// <summary>
        /// The total amount of Rtcp bytes sent of all contained TransportContexts
        /// </summary>
        public long TotalRtcpBytesSent { get { return IsDisposed ? 0 : TransportContexts.Sum(c => c.RtcpBytesSent); } }

        /// <summary>
        /// The total amount of bytes received of all contained TransportContexts
        /// </summary>
        public long TotalBytesReceieved { get { return IsDisposed ? 0 : TransportContexts.Sum(c => c.TotalBytesReceieved); } }

        /// <summary>
        /// The total amount of bytes sent of all contained TransportContexts
        /// </summary>
        public long TotalBytesSent { get { return IsDisposed ? 0 : TransportContexts.Sum(c => c.TotalBytesSent); } }

        /// <summary>
        /// The total amount of Rtcp packets received of all contained TransportContexts
        /// </summary>
        public long TotalRtcpPacketsReceieved { get { return IsDisposed ? 0 : TransportContexts.Sum(c => c.RtcpPacketsReceived); } }

        /// <summary>
        /// The total amount of Rtcp bytes received of all contained TransportContexts
        /// </summary>
        public long TotalRtcpBytesReceieved { get { return IsDisposed ? 0 : TransportContexts.Sum(c => c.RtcpBytesRecieved); } }

        #endregion

        #endregion

        #region Constructor

        static RtpClient()
        {
            if (false == UriParser.IsKnownScheme(RtpProtcolScheme)) UriParser.Register(new HttpStyleUriParser(), RtpProtcolScheme, 9670);
        }

        RtpClient(bool shouldDispose = true)
            : base(shouldDispose)
        {
            AverageMaximumRtcpBandwidthPercentage = DefaultReportInterval.TotalSeconds;
        }

        /// <summary>
        /// Assigns the events necessary for operation and creates or assigns memory to use as well as inactivtyTimout.
        /// </summary>
        /// <param name="memory">The optional memory segment to use</param>
        /// <param name="incomingPacketEventsEnabled"><see cref="IncomingPacketEventsEnabled"/></param>
        /// <param name="frameChangedEventsEnabled"><see cref="FrameChangedEventsEnabled"/></param>
        public RtpClient(MemorySegment memory = null, bool incomingPacketEventsEnabled = true, bool frameChangedEventsEnabled = true, bool outgoingPacketEvents = true, bool shouldDispose = true)
            : this(shouldDispose)
        {
            if (memory == null)
            {
                //Determine a good size based on the MTU (this should cover most applications)
                //Need an IP or the default IP to ensure the MTU Matches, use 1600 because 1500 is unaligned.
                m_Buffer = new MemorySegment(1600);
            }
            else
            {
                m_Buffer = memory;

                if (m_Buffer.Count < RtpHeader.Length) throw new ArgumentOutOfRangeException("memory", "memory.Count must contain enough space for a RtpHeader");
            }

            //RtpPacketReceieved += new RtpPacketHandler(HandleIncomingRtpPacket);
            //RtcpPacketReceieved += new RtcpPacketHandler(HandleIncomingRtcpPacket);
            //RtpPacketSent += new RtpPacketHandler(HandleOutgoingRtpPacket);
            //RtcpPacketSent += new RtcpPacketHandler(HandleOutgoingRtcpPacket);
            //InterleavedData += new InterleaveHandler(HandleInterleavedData);

            //Allow events to be raised
            HandleIncomingRtpPackets = HandleIncomingRtcpPackets = IncomingRtpPacketEventsEnabled = IncomingRtcpPacketEventsEnabled = incomingPacketEventsEnabled;

            //Fire events for packets received and Allow events to be raised
            HandleOutgoingRtpPackets = HandleOutgoingRtcpPackets = OutgoingRtpPacketEventsEnabled = OutgoingRtcpPacketEventsEnabled = outgoingPacketEvents;

            //Handle frame changes and Allow frame change events to be raised
            HandleFrameChanges = FrameChangedEventsEnabled = frameChangedEventsEnabled;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Adds a the given context to the instances owned by this client. 
        /// Throws a RtpClientException if the given context conflicts in channel either data or control with that of one which is already owned by the instance.
        /// </summary>
        /// <param name="context">The context to add</param>
        public virtual async Task AddContext(TransportContext context, bool checkDataChannel = true, bool checkControlChannel = true, bool checkLocalIdentity = true, bool checkRemoteIdentity = true)
        {
            if (checkDataChannel || checkControlChannel || checkLocalIdentity || checkRemoteIdentity)
                foreach (TransportContext c in TransportContexts)
                {
                    //If checking channels
                    if (checkDataChannel || checkControlChannel)
                    {
                        //If checking the data channel
                        if (checkDataChannel && c.DataChannel == context.DataChannel || c.ControlChannel == context.DataChannel)
                        {
                            TaggedExceptionExtensions.RaiseTaggedException(c, "Requested Data Channel is already in use by the context in the Tag");
                        }

                        //if checking the control channel
                        if (checkControlChannel && c.ControlChannel == context.ControlChannel || c.DataChannel == context.ControlChannel)
                        {
                            TaggedExceptionExtensions.RaiseTaggedException(c, "Requested Control Channel is already in use by the context in the Tag");
                        }

                    }

                    //if chekcking local identifier
                    if (checkLocalIdentity && c.SynchronizationSourceIdentifier == context.SynchronizationSourceIdentifier &&
                        c.MediaDescription.PayloadTypes.Any(pt => context.MediaDescription.PayloadTypes.Contains(pt)))
                    {
                        TaggedExceptionExtensions.RaiseTaggedException(c, "Requested Local SSRC is already in use by the context in the Tag");
                    }

                    //if chekcking remote identifier (and it has been defined)
                    if (checkRemoteIdentity && false == context.InDiscovery && false == c.InDiscovery &&
                        c.RemoteSynchronizationSourceIdentifier == context.RemoteSynchronizationSourceIdentifier &&
                        c.MediaDescription.PayloadTypes.Any(pt => context.MediaDescription.PayloadTypes.Contains(pt)))
                    {
                        TaggedExceptionExtensions.RaiseTaggedException(c, "Requested Remote SSRC is already in use by the context in the Tag");
                    }
                }


            //Add the context (This can introduce incorrect logic if the caller adds the context with channels in a reverse order, e.g. 2-3, 0-1)
            TransportContexts.Add(context);

            //Should check if sending is allowed via the media description
            if (context.IsActive) await SendReports(context);
        }

        /// <summary>
        /// Removes the given <see cref="TransportContext"/>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool TryRemoveContext(TransportContext context)
        {
            try
            {
                return TransportContexts.Remove(context);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets any <see cref="TransportContext"/> used by this instance.
        /// </summary>
        /// <returns>The <see cref="TransportContexts"/> used by this instance.</returns>
        public virtual IEnumerable<TransportContext> GetTransportContexts()
        {
            try
            {
                return TransportContexts.DefaultIfEmpty(); //null
            }
            catch (InvalidOperationException)
            {
                //May duplicate objects already projected, store index or use for construct.
                return GetTransportContexts();
            }
            catch { throw; }
        }

        #region Rtcp

        /// <summary>
        /// Creates any <see cref="RtcpReport"/>'s which are required by the implementation.
        /// The <see cref="SendersReport"/> and <see cref="ReceiversReport"/> (And accompanying <see cref="SourceDescriptionReport"/> if bandwidth allows) are created for the given context.
        /// </summary>
        /// <param name="context">The context to prepare Rtcp reports for</param>
        /// <param name="checkBandwidth">Indicates if the bandwidth of the RtpCliet or Context given should be checked.</param>
        /// <param name="storeReports">Indicates if the reports created should be stored on the corresponding properties of the instace.</param>
        /// <returns>The RtcpReport created.</returns>
        public virtual IEnumerable<RtcpReport> PrepareReports(TransportContext context, bool checkBandwidth = true, bool storeReports = true)
        {
            //Start with a sequence of empty packets
            IEnumerable<RtcpReport> compound = Enumerable.Empty<RtcpReport>();

            int reports = 0;

            //If Rtp data was sent then send a Senders Report.
            if (context.RtpPacketsSent > 0)
            {
                throw new Exception("context.RtpPacketsSent >0");

                //Insert the last SendersReport as the first compound packet
                if (storeReports)
                    compound = Enumerable.Concat(LinqExtensions.Yield((context.SendersReport = TransportContext.CreateSendersReport(context, false))), compound);
                else
                    compound = Enumerable.Concat(LinqExtensions.Yield(TransportContext.CreateSendersReport(context, false)), compound);

                ++reports;
            }

            //If Rtp data was received OR Rtcp data was sent then send a Receivers Report.
            if (context.RtpPacketsReceived > 0 || context.TotalRtcpBytesSent > 0)
            {
                //Insert the last ReceiversReport as the first compound packet
                if (storeReports)
                    compound = Enumerable.Concat(LinqExtensions.Yield((context.ReceiversReport = TransportContext.CreateReceiversReport(context, false))), compound);
                else
                    compound = Enumerable.Concat(LinqExtensions.Yield(TransportContext.CreateReceiversReport(context, false)), compound);

                ++reports;
            }

            //If there are any packets to be sent AND we don't care about bandwidth OR the bandwidth is not exceeded
            if (reports > 0 &&
                (checkBandwidth == false || false == context.RtcpBandwidthExceeded))
            {
                //Include the SourceDescription
                if (storeReports)
                {
                    var cName = string.IsNullOrWhiteSpace(ClientName) ? null :
                        new SourceDescriptionReport.SourceDescriptionItem(SourceDescriptionReport.SourceDescriptionItem.SourceDescriptionItemType.CName, System.Text.Encoding.UTF8.GetBytes(ClientName));
                    context.SourceDescription = TransportContext.CreateSourceDescriptionReport(context, cName, AdditionalSourceDescriptionItems);
                    var temp = LinqExtensions.Yield(context.SourceDescription);
                    compound = Enumerable.Concat(compound, temp);
                }
                else
                    compound = Enumerable.Concat(LinqExtensions.Yield(TransportContext.CreateSourceDescriptionReport(context, (string.IsNullOrWhiteSpace(ClientName) ? null : new SourceDescriptionReport.SourceDescriptionItem(SourceDescriptionReport.SourceDescriptionItem.SourceDescriptionItemType.CName, System.Text.Encoding.UTF8.GetBytes(ClientName))), AdditionalSourceDescriptionItems)), compound);
            }

            //Could also put a Goodbye for inactivity ... :) Currently handled by SendGoodbye, possibly allow for optional parameter where this occurs here.
            if (compound.Count() > 0)
            {
                foreach (var item in compound)
                {
                    LoggerInstance.Debug(Encoding.UTF8.GetString(item.RtcpData.ToArray(), 0, item.RtcpData.Count()));
                }
            }
            return compound;
        }

        /// <summary>
        /// Sends a Goodbye to for all contained TransportContext, which will also stop the process sending or receiving after the Goodbye is sent
        /// </summary>
        public virtual async Task SendGoodbyes()
        {
            foreach (RtpClient.TransportContext tc in TransportContexts)
                await SendGoodbye(tc, null, tc.SynchronizationSourceIdentifier).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a GoodbyeReport and stores it in the <paramref name="context"/> given if the <paramref name="ssrc"/> is also given and is equal to the <paramref name="context.SynchronizationSourceIdentifier"/>
        /// </summary>
        /// <param name="context">The context of the report</param>
        /// <param name="reasonForLeaving">An optional reason why the report is being sent.</param>
        /// <param name="ssrc">The optional identity to use in he report.</param>
        /// <param name="force">Indicates if the call should be forced. <see cref="IsRtcpEnabled"/>, when true the report will also not be stored</param>
        /// <returns></returns>
        internal protected virtual async Task<int> SendGoodbye(TransportContext context, byte[] reasonForLeaving = null, int? ssrc = null, bool force = false, RFC3550.SourceList sourceList = null, bool empty = false)
        {
            //Check if the Goodbye can be sent.
            if (IsDisposed //If the RtpClient is disposed 
                || //OR the context is disposed
                IDisposedExtensions.IsNullOrDisposed(context)
                || //OR the call has not been forced AND the context IsRtcpEnabled AND the context is active
                (false == force && true == context.IsRtcpEnabled && context.IsActive
                && //AND the final Goodbye was sent already
                context.Goodbye != null && context.Goodbye.Transferred.HasValue))
            {
                //Indicate nothing was sent
                return 0;
            }

            //Make a Goodbye, indicate version in Client, allow reason for leaving and optionall other sources
            GoodbyeReport goodBye = TransportContext.CreateGoodbyeReport(context, reasonForLeaving, ssrc ?? context.SynchronizationSourceIdentifier, sourceList);

            if (sourceList == null && empty) goodBye.BlockCount = 0;

            //Store the Goodbye in the context if not forced the ssrc was given and it was for the context given.
            if (false == force && ssrc.HasValue && ssrc.Value == context.SynchronizationSourceIdentifier) context.Goodbye = goodBye;

            //Send the packet and return the amount of bytes which resulted.
            return await SendRtcpPackets(Enumerable.Concat(PrepareReports(context, false, true), LinqExtensions.Yield(goodBye))).ConfigureAwait(false);
        }

        /// <summary>
        /// Selects a TransportContext by matching the SynchronizationSourceIdentifier to the given sourceid
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns>The context which was identified or null if no context was found.</returns>
        internal protected virtual TransportContext GetContextBySourceId(int sourceId)
        {
            if (IsDisposed) return null;
            try
            {
                foreach (RtpClient.TransportContext tc in TransportContexts)
                    if (tc != null && tc.SynchronizationSourceIdentifier == sourceId || tc.RemoteSynchronizationSourceIdentifier == sourceId)
                        return tc;
            }
            catch (InvalidOperationException)
            {
                return GetContextBySourceId(sourceId);
            }
            catch { if (false == IsDisposed) throw; }
            return null;
        }

        //DataChannel ControlChannel or overload?

        ////internal protected virtual TransportContext GetContextByChannel(byte channel)
        ////{
        ////    if (IsDisposed) return null;
        ////    try
        ////    {
        ////        foreach (RtpClient.TransportContext tc in TransportContexts)
        ////            if (tc.DataChannel == channel || tc.ControlChannel == channel) return tc;
        ////    }
        ////    catch (InvalidOperationException) { return GetContextByChannel(channel); }
        ////    catch { if (false == IsDisposed) throw; }
        ////    return null;
        ////}

        internal protected virtual TransportContext GetContextByChannels(params byte[] channels)
        {
            if (IsDisposed) return null;
            try
            {
                foreach (RtpClient.TransportContext tc in TransportContexts)
                    foreach (byte channel in channels)
                        if (tc.DataChannel == channel || tc.ControlChannel == channel) return tc;
            }
            catch (InvalidOperationException) { return GetContextByChannels(channels); }
            catch { if (false == IsDisposed) throw; }
            return null;
        }

        /// <summary>
        /// Selects a TransportContext by using the packet's Channel property
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        internal protected virtual TransportContext GetContextForPacket(RtcpPacket packet)
        {
            if (IsDisposed || IDisposedExtensions.IsNullOrDisposed(packet)) return null;
            //Determine based on reading the packet this is where a RtcpReport class would be useful to allow reading the Ssrc without knownin the details about the type of report
            try
            {
                return GetContextBySourceId(packet.SynchronizationSourceIdentifier);
            }
            catch (InvalidOperationException)
            {
                return GetContextForPacket(packet);
            }
            catch { if (false == IsDisposed) throw; }
            return null;
        }

        /// <summary>
        /// Sends the given packets, this function assumes all packets sent belong to the same party.
        /// </summary>
        /// <param name="packets"></param>
        /// <returns></returns>
        public virtual async Task<int> SendRtcpPackets(IEnumerable<RtcpPacket> packets, TransportContext context)
        {
            var error = SocketError.SocketError;

            if (IsDisposed || packets == null) return 0;

            //If we don't have an transportContext to send on or the transportContext has not been identified or Rtcp is Disabled or there is no remote rtcp end point
            if (IDisposedExtensions.IsNullOrDisposed(context) || context.SynchronizationSourceIdentifier == 0 || false == context.IsRtcpEnabled || context.RemoteRtcp == null)
            {
                //Return
                return 0;
            }

            //how manu bytes sent so far.
            int sent = 0;

            int length = 0;

            if (m_IListSockets)
            {
                List<ArraySegment<byte>> buffers = new System.Collections.Generic.List<ArraySegment<byte>>();

                IList<ArraySegment<byte>> packetBuffers;

                //Try to get the buffer for each packet
                foreach (RtcpPacket packet in packets)
                {
                    //If we can
                    if (packet.TryGetBuffers(out packetBuffers))
                    {
                        //Add those buffers
                        buffers.AddRange(packetBuffers);

                        //Keep track of the length
                        length += packet.Length;
                    }
                    else
                    {
                        //Just send them in their own array.
                        sent += await SendData(RFC3550.ToCompoundBytes(packets).ToArray(), context.ControlChannel, context.RtcpSocket, context.RemoteRtcp);

                        buffers = null;

                        break;
                    }

                }

                //If nothing was sent and the buffers are not null and the socket is tcp use framing.
                if (length > 0 && context.IsActive && sent == 0 && buffers != null)
                {
                    if (context.RtcpSocket.ProtocolType == ProtocolType.Tcp)
                    {
                        //Todo, should have function to create framing to be compatible with RFC4571
                        //Todo, Int can be used as bytes and there may only be 2 bytes required.
                        byte[] framing = new byte[] { BigEndianFrameControl, context.ControlChannel, 0, 0 };

                        Binary.Write16(framing, 2, Binary.IsLittleEndian, (short)length);

                        //Add the framing
                        buffers.Insert(0, new ArraySegment<byte>(framing));
                    }

                    //Send that data.

                    sent += await SocketExtensions.SendTo(buffers, context.RtcpSocket);
                    //sent += context.RtcpSocket.Send(buffers);
                }
            }
            else
            {

                //Iterate the packets
                foreach (RtcpPacket packet in packets)
                {
                    //If the data is not contigious
                    if (false == packet.IsContiguous())
                    {
                        //Just send all packets in their own array by projecting the data (causes an allocation)
                        sent += await SendData(RFC3550.ToCompoundBytes(packets).ToArray(), context.ControlChannel, context.RtcpSocket, context.RemoteRtcp);

                        //Stop here.
                        break;
                    }

                    //Account for the length of the packet
                    length += packet.Length;
                }

                //If nothing was sent then send the data now.
                if (length > 0 && sent == 0)
                {
                    //Send the framing seperately to keep the allocations minimal.

                    //Note, Live555 and LibAV may not be able to handle this, use IListSockets to work around.
                    if (context.RtcpSocket.ProtocolType == ProtocolType.Tcp)
                    {
                        //Todo, should have function to create framing to be compatible with RFC4571
                        //Todo, Int can be used as bytes and there may only be 2 bytes required.
                        byte[] framing = new byte[] { BigEndianFrameControl, context.ControlChannel, 0, 0 };

                        Binary.Write16(framing, 2, Binary.IsLittleEndian, (short)length);

                        while (sent < InterleavedOverhead && (error != SocketError.ConnectionAborted && error != SocketError.ConnectionReset && error != SocketError.NotConnected))
                        {
                            //Send all the framing.
                            //sent += context.RtcpSocket.Send(framing, sent, InterleavedOverhead - sent, out error);
                            sent += await SocketExtensions.SendTo(framing, sent, InterleavedOverhead - sent, context.RtcpSocket);
                        }
                        if (sent > 0)
                        {
                            error = SocketError.Success;
                        }
                        sent = 0;
                    }
                    else error = SocketError.Success;

                    int packetLength;

                    //if the framing was delivered then send the packet
                    if (error == SocketError.Success)
                        foreach (RtcpPacket packet in packets)
                        {
                            //cache the length
                            packetLength = packet.Length;

                            //While there is data to send
                            while (sent < packetLength && error != SocketError.ConnectionAborted && error != SocketError.ConnectionReset)
                            {
                                //Send it.
                                sent += await SocketExtensions.SendTo(packet.Header.First16Bits.m_Memory.Array, packet.Header.First16Bits.m_Memory.Offset + sent, packetLength - sent, context.RtcpSocket);
                                //sent += context.RtcpSocket.Send(packet.Header.First16Bits.m_Memory.Array, packet.Header.First16Bits.m_Memory.Offset + sent, packetLength - sent, SocketFlags.None, out error);
                            }

                            if (sent > 0)
                            {
                                error = SocketError.Success;
                            }
                            //Reset offset.
                            sent = 0;
                        }

                    //Set sent to how many bytes were sent.
                    sent = length + InterleavedOverhead;
                }
            }

            //If the compound bytes were completely sent then all packets have been sent
            if (error == SocketError.Success)
            {
                //Check to see if each packet which was sent
                int csent = 0;

                //Iterate each managed packet to determine if it was completely sent.
                foreach (RtcpPacket packet in packets)
                {
                    //Handle null or disposed packets.
                    if (IDisposedExtensions.IsNullOrDisposed(packet)) continue;

                    //Increment for the length of the packet
                    csent += packet.Length;

                    //If more data was contained then sent don't set Transferred and raise and event
                    if (csent > sent)
                    {
                        ++context.m_FailedRtcpTransmissions;

                        break;
                    }

                    //set sent
                    packet.Transferred = DateTime.UtcNow;

                    //Raise en event
                    HandleOutgoingRtcpPacket(this, packet, context);
                }
            }

            return sent;
        }

        public virtual async Task<int> SendRtcpPackets(IEnumerable<RtcpPacket> packets)
        {
            if (packets == null) return 0;

            TransportContext context = GetContextForPacket(packets.FirstOrDefault());

            return await SendRtcpPackets(packets, context).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends any <see cref="RtcpReport"/>'s immediately for the given <see cref="TransportContext"/> if <see cref="AverageRtcpBandwidthExceeded"/> is false.
        /// </summary>
        /// <param name="context">The <see cref="TransportContext"/> to send a report for</param>
        /// <param name="error"></param>
        /// <param name="force"></param>
        /// <returns>A value indicating if reports were sent</returns>
        internal virtual async Task<bool> SendReports(TransportContext context, bool force = false)
        {
            //Check for the stop signal (or disposal)
            if (false == force && m_StopRequested || IsDisposed ||  //Otherwise
                false == context.IsRtcpEnabled
                || //Or Rtcp Bandwidth for this context or RtpClient has been exceeded
                context.RtcpBandwidthExceeded || AverageRtcpBandwidthExceeded
                || context.Goodbye != null) return false; //No reports can be sent.

            //If forced or the last reports were sent in less time than alloted by the m_SendInterval
            //Indicate if reports were sent in this interval
            return force || context.LastRtcpReportSent == TimeSpan.MinValue || context.LastRtcpReportSent > context.m_SendInterval ?
                await SendRtcpPackets(PrepareReports(context, true, true), context) > 0 : false;
        }

        /// <summary>
        /// Sends a RtcpGoodbye Immediately if the given context:
        /// <see cref="IsRtcpEnabled"/>  and the context has not received a RtcpPacket during the last <see cref="ReceiveInterval"/>.
        /// OR
        /// <see cref="IsRtpEnabled"/> and the context <see cref="IsContinious"/> but <see cref="Uptime"/> is > the <see cref="MediaEndTime"/>
        /// </summary>
        /// <param name="lastActivity">The time the lastActivity has occured on the context (sending or recieving)</param>
        /// <param name="context">The context to check against</param>
        /// <returns>True if the connection is inactive and a Goodebye was attempted to be sent to the remote party</returns>
        internal virtual async Task<bool> SendGoodbyeIfInactive(DateTime lastActivity, TransportContext context)
        {
            bool inactive = false;

            if (IsDisposed
                ||
                m_StopRequested
                ||
                false == RtcpEnabled
                ||
                context.HasRecentRtpActivity
                ||
                context.HasRecentRtcpActivity
                || //If the context has a continous flow OR the general Uptime is less then context MediaEndTime
                (false == context.IsContinious && Uptime < context.MediaEndTime))
            {
                return false;
            }

            //Calulcate for the currently inactive time period
            if (context.Goodbye == null &&
                false == context.HasAnyRecentActivity)
            {
                //Set the amount of time inactive
                context.m_InactiveTime = DateTime.UtcNow - lastActivity;

                //Determine if the context is not inactive too long
                //6.3.5 Timing Out an SSRC
                //I use the recieve interval + the send interval
                //It should be standarly 2 * recieve interval
                if (context.m_InactiveTime >= context.m_ReceiveInterval + context.m_SendInterval)
                {
                    //send a goodbye
                    await SendGoodbye(context, null, context.SynchronizationSourceIdentifier);

                    //mark inactive
                    inactive = true;

                    //Disable further service
                    //context.IsRtpEnabled = context.IsRtcpEnabled = false;
                }
                else if (context.m_InactiveTime >= context.m_ReceiveInterval + context.m_SendInterval)
                {
                    //send a goodbye but don't store it
                    inactive = await SendGoodbye(context) <= 0;
                }
            }

            //indicate a goodbye was sent and a context is now inactive.
            return inactive;
        }

        #endregion

        #region Rtp

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public TransportContext GetContextForMediaDescription(MediaDescription mediaDescription)
        {
            if (IDisposedExtensions.IsNullOrDisposed(mediaDescription)) return null;

            return TransportContexts.FirstOrDefault(c => c.MediaDescription.MediaType == mediaDescription.MediaType && c.MediaDescription.MediaFormat == mediaDescription.MediaFormat);
        }

        /// <summary>
        /// Selects a TransportContext for a RtpPacket by matching the packet's PayloadType to the TransportContext's MediaDescription.MediaFormat
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public TransportContext GetContextForPacket(RtpPacket packet)
        {
            if (IDisposedExtensions.IsNullOrDisposed(packet)) return null;

            return GetContextBySourceId(packet.SynchronizationSourceIdentifier) ?? GetContextByPayloadType(packet.PayloadType);
        }

        /// <summary>
        /// Selects a TransportContext by matching the given payloadType to the TransportContext's MediaDescription.MediaFormat
        /// </summary>
        /// <param name="payloadType"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public TransportContext GetContextByPayloadType(int payloadType)
        {
            return TransportContexts.Count == 0 ? null : TransportContexts.FirstOrDefault(c => false == IDisposedExtensions.IsNullOrDisposed(c) && c.MediaDescription.PayloadTypes.Contains(payloadType));
        }

        /// <summary>
        /// Selects a TransportContext by matching the given socket handle to the TransportContext socket's handle
        /// </summary>
        /// <param name="payloadType"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public TransportContext GetContextBySocket(Socket socket)
        {
            return TransportContexts.Count == 0 ? null : TransportContexts.FirstOrDefault(c => false == IDisposedExtensions.IsNullOrDisposed(c) && c.IsActive && c.RtpSocket != null && c.RtpSocket == socket || c.RtcpSocket != null && c.RtcpSocket == socket);
        }
        #endregion

        /// <summary>
        /// Creates and starts a worker thread which will send and receive data as required.
        /// </summary>
        public virtual void Activate()
        {
            try
            {
                ClientName = "Siverlight Client " + InternalId;

                //If the worker thread is already active then return
                if (false == m_StopRequested && IsActive) return;

                LoggerInstance.Debug("m_RtpClient.Activate()");

                //Create the worker thread
                m_WorkerThread = new Thread(SendReceieve);

                //Configure
                if (ConfigureThread != null)
                {
                    ConfigureThread(m_WorkerThread); //name and ILogging
                }

                //m_WorkerThread.Name = "RtpClient-" + InternalId;
                m_WorkerThread.Name = "RtpClient-SendReceieve";

                //Reset stop signal
                m_StopRequested = false;

                //Start thread
                m_WorkerThread.Start();

                //Wait for thread to actually start
                while (false == IsActive)
                    m_EventReady.WaitOne(TimeSpanExtensions.OneTick);
            }
            catch (ObjectDisposedException) { return; }
            catch (Exception ex)
            {
                LoggerInstance.Exception(ex);
                throw;
            }
        }

        /// <summary>
        /// Sends the Rtcp Goodbye and signals a stop in the worker thread.
        /// </summary>
        public async Task Deactivate()
        {
            try
            {
                if (IsDisposed || false == IsActive) return;

                try
                {
                    await SendGoodbyes();
                }
                catch (System.Exception ex)
                {
                }

                m_StopRequested = true;

                foreach (TransportContext tc in TransportContexts) if (tc.IsActive) tc.DisconnectSockets();

                ThreadExtensions.TryAbortAndFree(ref m_WorkerThread);

                Started = DateTime.MinValue;
            }
            catch (System.Exception ex)
            {

            }
        }

        public void DisposeAndClearTransportContexts()
        {
            //Dispose contexts
            foreach (TransportContext tc in TransportContexts) tc.Dispose();

            //Counters go away with the transportChannels
            TransportContexts.Clear();
        }

        /// <summary>
        /// Returns the amount of bytes read to completely read the application layer framed data
        /// Where a negitive return value indicates no more data remains.
        /// 查找RTP或RTCP的开始标记 $
        /// </summary>
        /// <param name="received">How much data was received</param>
        /// <param name="frameChannel">The output of reading a frameChannel</param>
        /// <param name="context">The context assoicated with the frameChannel</param>
        /// <param name="offset">The reference to offset to look for framing data</param>
        /// <param name="raisedEvent">Indicates if an event was raised</param>
        /// <param name="buffer">The optional buffer to use.</param>
        /// <returns>The amount of bytes the frame data SHOULD have</returns>
        int ReadApplicationLayerFraming(int received, out byte frameChannel, out TransportContext context, int offset, out bool raisedEvent, byte[] buffer = null)
        {

            //There is no relevant TransportContext assoicated yet.
            context = null;

            //The channel of the frame - The Framing Method
            frameChannel = default(byte);

            raisedEvent = false;

            buffer = buffer ?? m_Buffer.Array;

            int bufferLength = buffer.Length, bufferOffset = offset;

            received = Binary.Min(received, bufferLength - bufferOffset);

            //Todo Determine from Context to use control channel and length. (Check MediaDescription)
            //NEEDS TO HANDLE CASES WHERE RFC4571 Framing are in play and no $ or Channel are used....            
            int sessionRequired = InterleavedOverhead;

            if (received <= 0 || received < sessionRequired) return -1;

            //Look for the frame control octet
            int startOfFrame = Array.IndexOf<byte>(buffer, BigEndianFrameControl, bufferOffset, received);

            int frameLength = 0;

            //If not found everything belongs to the upper layer
            if (startOfFrame == -1)
            {
                OnInterleavedData(buffer, bufferOffset, received);

                raisedEvent = true;

                //Indicate the amount of data consumed.
                return received;
            }
            else if (startOfFrame > offset) // If the start of the frame is not at the beginning of the buffer
            {
                //Determine the amount of data which belongs to the upper layer
                int upperLayerData = startOfFrame - bufferOffset;

                OnInterleavedData(buffer, bufferOffset, upperLayerData);

                raisedEvent = true;

                //If there is more data related to upperLayerData it will be evented in the next run. (See RtspClient ProcessInterleaveData notes)
                return upperLayerData;
            }

            //If there is not enough data for a frame header return
            if (bufferOffset + sessionRequired > bufferLength) return -1;

            //The amount of data needed for the frame comes from TryReadFrameHeader
            frameLength = TryReadFrameHeader(buffer, bufferOffset, out frameChannel, BigEndianFrameControl, true);

            //Assign a context if there is a frame of any size
            if (frameLength >= 0)
            {
                //Assign the context
                context = GetContextByChannels(frameChannel);

                //Increase the result by the size of the header
                frameLength += sessionRequired;
            }

            //Return the amount of bytes or -1 if any error occured.
            return frameLength;
        }

        /// <summary>
        /// Sends the given data on the socket remote
        /// </summary>
        /// <param name="data"></param>
        /// <param name="channel"></param>
        /// <param name="socket"></param>
        /// <param name="remote"></param>
        /// <param name="error"></param>
        /// <param name="useFrameControl"></param>
        /// <param name="useChannelId"></param>
        /// <returns></returns>
        internal protected virtual async Task<int> SendData(byte[] data, byte? channel, Socket socket, System.Net.EndPoint remote, int pollTime = 0, bool useFrameControl = true, bool useChannelId = true)
        {
            return await SendData(data, 0, data.Length, channel, socket, remote, pollTime, useFrameControl, useChannelId);
        }

        /// <summary>
        /// Sends the given data on the socket to remote
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="channel"></param>
        /// <param name="socket"></param>
        /// <param name="remote"></param>
        /// <param name="error"></param>
        /// <param name="useFrameControl"></param>
        /// <param name="useChannelId"></param>
        /// <returns></returns>
        internal protected virtual async Task<int> SendData(byte[] data, int offset, int length, byte? channel, Socket socket, System.Net.EndPoint remote, int pollTime = 0, bool useFrameControl = true, bool useChannelId = true)
        {
            //Check there is valid data and a socket which is able to write and that the RtpClient is not stopping
            if (IsDisposed || socket == null || length == 0 || data == null) return 0;

            int sent = 0;

            try
            {
                #region Tcp Application Layer Framing

                //Under Tcp we must frame the data for the given channel
                if (socket.ProtocolType == ProtocolType.Tcp && channel.HasValue)
                {
                    if (useChannelId && useFrameControl)
                    {
                        //Create the framing
                        byte[] framing = new byte[] { BigEndianFrameControl, channel.Value, 0, 0 };

                        //Write the length
                        Binary.Write16(framing, 2, Binary.IsLittleEndian, (short)length);

                        //Send the framing
                        sent += await SocketExtensions.SendTo(framing, 0, InterleavedOverhead, socket, remote);
                    }
                    else
                    {
                        //Build the data
                        IEnumerable<byte> framingData;

                        //The length is always present
                        framingData = Binary.GetBytes((short)length, Binary.IsLittleEndian).Concat(data);

                        int framingLength = 2;

                        if (useChannelId)
                        {
                            framingData = LinqExtensions.Yield(channel.Value).Concat(framingData);
                            ++framingLength;
                        }

                        if (useFrameControl)
                        {
                            framingData = LinqExtensions.Yield(BigEndianFrameControl).Concat(data);
                            ++framingLength;
                        }

                        byte[] framing = framingData.ToArray();
                        sent += await SocketExtensions.SendTo(framing, 0, framingLength, socket, remote);
                    }
                }
                else
                    length = data.Length;

                #endregion

                //Send all the data to the endpoint
                sent += await SocketExtensions.SendTo(data, offset, length, socket, remote);

                return sent; //- Overhead for tcp, may not have to include it.
            }
            catch
            {
                return sent;
            }
        }

        /// <summary>
        /// Recieves data on a given socket and endpoint
        /// </summary>
        /// <param name="socket">The socket to receive data on</param>
        /// <returns>The number of bytes recieved</returns>             
        internal protected virtual async Task<Tuple<SocketError, int>> ReceiveData(Socket socket, EndPoint remote, bool expectRtp = true, bool expectRtcp = true, MemorySegment buffer = null)
        {
            SocketError error = SocketError.SocketError;

            if (buffer == null) buffer = m_Buffer;

            //Ensure the socket can poll
            if (IsDisposed || m_StopRequested || socket == null || remote == null || IDisposedExtensions.IsNullOrDisposed(buffer))
                return new Tuple<SocketError, int>(error, 0);

            //Cache the offset at the time of the call
            int received = 0;

            try
            {
                CancellationToken cancelltionToken = new CancellationToken();
                var tcs = new TaskCompletionSource<int>();

                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.BufferList = new[] { new ArraySegment<byte>(buffer.Array, buffer.Offset, buffer.Count) };
                args.RemoteEndPoint = remote;

                var cancelRegistration = cancelltionToken.Register(() => Socket.CancelConnectAsync(args));

                EventHandler<SocketAsyncEventArgs> completeHandler = (sender, eventArgs) =>
                {
                    cancelRegistration.Dispose();

                    if (args.SocketError != SocketError.Success)
                    {
                        if (SocketError.OperationAborted == args.SocketError || SocketError.ConnectionAborted == args.SocketError)
                            //tcs.TrySetCanceled();
                            tcs.SetResult(0);
                        else
                            tcs.TrySetException(new WebException("Socket to " + " failed: " + args.SocketError));
                    }

                    tcs.TrySetResult(args.BytesTransferred);
                };

                args.Completed += completeHandler;

                if (!socket.ReceiveAsync(args))
                {
                    completeHandler(socket, args);
                }

                var receiveCount = await tcs.Task;
                received = receiveCount;
                error = args.SocketError;

                received = await ProcessFrameData(buffer.Array, buffer.Offset, received, socket);
            }
            catch (SocketException se)
            {
                error = se.SocketErrorCode;
                LoggerInstance.Exception(se);
            }
            catch { throw; }

            return new Tuple<SocketError, int>(error, received);
        }

        /// <summary>
        /// Used to handle Tcp framing, this should be put on the TransportContext or it should allow a way for Transport to be handled, right now this is done in OnInterleavedData
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="socket"></param>
        /// <returns></returns>
        internal protected virtual async Task<int> ProcessFrameData(byte[] buffer, int offset, int count, Socket socket)
        {
            if (count == 0) return 0;

            //If there is no buffer use our own buffer.
            if (buffer == null) buffer = m_Buffer.Array;

            //Determine which TransportContext will receive the data incoming
            TransportContext relevent = null;

            //The channel of the data
            byte frameChannel = 0;

            //Get the length of the given buffer (Should actually use m_Buffer.Count when using our own buffer)
            int bufferLength = buffer.Length,
                //The indicates length of the data
                frameLength = 0,
                //The amount of data remaining in the buffer
                remainingInBuffer = count,//Gsafety.PTMS.Media.RTSP.Extensions.Math.MathExtensions.Clamp(count, count, bufferLength - offset),
                //The amount of data received (which is already equal to what is remaining in the buffer)
                recievedTotal = remainingInBuffer;

            //1 byte with 8 bits can single all of these and would reduce space complexity at the cost of more operations.

            //Determine if Rtp or Rtcp is coming in or some other type (could be combined with expectRtcp and expectRtp == false)
            bool expectRtp = false, expectRtcp = false, incompatible = true, unrelatedData = false, needsHeaderData = false;

            //If anything remains on the socket the value will be calulcated.
            int remainingOnSocket = 0;

            int sessionRequired = InterleavedOverhead;

            //While not disposed and there is data remaining (within the buffer)
            while (false == IsDisposed && remainingInBuffer > 0 && offset >= m_Buffer.Offset)
            {
                //Assume not rtp or rtcp and that the data is compatible with the session
                expectRtp = expectRtcp = incompatible = needsHeaderData = false;

                //If a header can be read
                if (remainingInBuffer >= sessionRequired)
                {
                    //Determine if an event was raised each time there was at least the required amount of data.
                    unrelatedData = false;

                    //Parse the frameLength from the given buffer, when raisedEvent is true that many bytes should be skipped because they are not related to Rtp.
                    //The logic below will proceed to CheckRemainingData because incompatible is false.
                    frameLength = ReadApplicationLayerFraming(remainingInBuffer, out frameChannel, out relevent, offset, out unrelatedData, buffer);

                    //If a frame was found (Including the null packet)
                    if (frameLength >= 0)
                    {
                        //If there WAS a context
                        if (relevent != null)
                        {
                            #region Verify FrameLength

                            //Verify minimum and maximum packet sizes allowed by context. (taking into account the amount of bytes in the ALF)
                            if (frameLength < relevent.MinimumPacketSize + sessionRequired || frameLength > relevent.MaximumPacketSize + sessionRequired)
                            {
                                //mark as incompatible
                                incompatible = true;
                                LoggerInstance.Error(ToString() + "@ProcessFrameData - Irregular Packet of " + frameLength + " for Channel " + frameChannel + " remainingInBuffer=" + remainingInBuffer);

                                //jump
                                goto CheckPacketAttributes;
                            }

                            //If all that remains is the frame header then receive more data. 6 comes from (InterleavedOverhead + CommonHeaderBits.Size)
                            //We need more data to be able to verify the frame.
                            if (remainingInBuffer <= sessionRequired + RFC3550.CommonHeaderBits.Size) //6) //sessionRequired + RFC3550.CommonHeaderBits.Size;
                            {
                                //Remove the context
                                relevent = null;
                                //Set needs header data
                                needsHeaderData = true;
                                LoggerInstance.Debug(ToString() + "@ProcessFrameData - Needs data for packet fields inspection Packet of " + frameLength + " for Channel " + frameChannel + " remainingInBuffer=" + remainingInBuffer);
                                goto CheckRemainingData;
                            }

                            #endregion

                            #region Verify Packet Headers

                            //Using CommonHeaderBits on the data after the Interleaved Frame Header wastes time and memory, just should check with offsets...
                            using (var common = new RFC3550.CommonHeaderBits(buffer, offset + sessionRequired))
                            {
                                //Check the version...
                                incompatible = common.Version != relevent.Version;

                                //If this is a valid context there must be at least a RtpHeader's worth of data in the buffer. 
                                //If this was a RtcpPacket with only 4 bytes it wouldn't have a ssrc and wouldn't be valid to be sent.
                                if (false == incompatible &&
                                    (frameChannel == relevent.DataChannel &&
                                    remainingInBuffer <= RtpHeader.Length + sessionRequired)
                                    ||
                                    (frameChannel == relevent.ControlChannel &&
                                    remainingInBuffer <= RtcpHeader.Length + sessionRequired))
                                {
                                    //Remove the context
                                    relevent = null;
                                    //Mark as incompatible
                                    needsHeaderData = true;
                                    goto EndUsingHeader;
                                }

                                //Perform a set of checks and set weather or not Rtp or Rtcp was expected.                                  
                                if (false == incompatible)
                                {
                                    //Determine if the packet is Rtcp by looking at the found channel and the relvent control channel
                                    if (frameChannel == relevent.ControlChannel)
                                    {
                                        //Rtcp
                                        if (remainingInBuffer <= sessionRequired + RtcpHeader.Length)
                                        {
                                            //Remove the context
                                            relevent = null;
                                            needsHeaderData = true;
                                            goto EndUsingHeader;
                                        }

                                        //Store any rtcp length so we can verify its not 0 and then additionally ensure its value is not larger then the frameLength
                                        int rtcpLen;

                                        using (RtcpHeader header = new RtcpHeader(buffer, offset + sessionRequired))
                                        {
                                            //Get the length in 'words' (by adding one)
                                            //A length of 0 means 1 word
                                            //A length of 65535 means only the header (no ssrc [or payload])
                                            ushort lengthInWordsPlusOne = (ushort)(header.LengthInWordsMinusOne + 1);

                                            //Convert to bytes
                                            rtcpLen = lengthInWordsPlusOne * 4;

                                            //Check that the supposed  amount of contained words is greater than or equal to the frame length conveyed by the application layer framing
                                            //it must also be larger than the buffer
                                            incompatible = rtcpLen > frameLength && rtcpLen > bufferLength;

                                            if (false == incompatible && //It was not already ruled incomaptible
                                                lengthInWordsPlusOne > 0 && //If there is supposed to be SSRC in the packet
                                                ///remainingInBuffer >= 8...
                                                header.Size > RtcpHeader.Length && //The header ACTUALLY contains enough bytes to have a SSRC
                                                false == relevent.InDiscovery)//The remote context knowns the identity of the remote stream                                                 
                                            {
                                                //Determine if Rtcp is expected
                                                //Perform another lookup and check compatibility
                                                expectRtcp = (incompatible = (GetContextBySourceId(header.SendersSynchronizationSourceIdentifier)) == null) ? false : true;
                                            }
                                        }
                                    }

                                    //May be mixing channels...
                                    if (false == expectRtcp)
                                    {
                                        //Rtp
                                        if (remainingInBuffer <= sessionRequired + RtpHeader.Length)
                                        {
                                            //Remove the context
                                            relevent = null;
                                            needsHeaderData = true;
                                            goto EndUsingHeader;
                                        }

                                        //the context by payload type is null is not discovering the identity check the SSRC.
                                        if (GetContextByPayloadType(common.RtpPayloadType) == null && false == relevent.InDiscovery)
                                        {
                                            using (RtpHeader header = new RtpHeader(buffer, offset + InterleavedOverhead))
                                            {
                                                //The context was obtained by the frameChannel
                                                //Use the SSRC to determine where it should be handled.
                                                //If there is no context the packet is incompatible
                                                expectRtp = (incompatible = (GetContextBySourceId(header.SynchronizationSourceIdentifier)) == null) ? false : true;
                                            }
                                        }
                                        else incompatible = false;
                                    }
                                }
                            EndUsingHeader:
                                ;
                            }

                            #endregion
                        }

                        if (needsHeaderData) goto CheckRemainingData;

                    CheckPacketAttributes:

                        if (incompatible)
                        {
                            //If there was a context then incrment for failed receptions
                            if (relevent != null)
                            {
                                if (expectRtp) ++relevent.m_FailedRtpReceptions;
                                if (expectRtcp) ++relevent.m_FailedRtcpReceptions;
                            }

                            LoggerInstance.Error(InternalId + "ProcessFrameData - Incompatible Packet frameLength=" + frameLength + " for Channel " + frameChannel + " remainingInBuffer=" + remainingInBuffer);
                        }
                        //If frameLength was 0 or the frame was larger than we can store then interleave the header for handling if required   
                        //incompatible may not be true here.
                        else if (frameLength == 0 || frameLength > bufferLength)
                        {
                            if (frameLength == 0)
                            {
                                LoggerInstance.Error(InternalId + "ProcessFrameData - Null Packet for Channel " + frameChannel + " remainingInBuffer=" + remainingInBuffer);
                            }
                            else //If there was a context then increment for failed receptions only for large packets
                            {
                                if (expectRtp) ++relevent.m_FailedRtpReceptions;
                                if (expectRtcp) ++relevent.m_FailedRtcpReceptions;
                                if (expectRtp || expectRtcp) LoggerInstance.Error(InternalId + "ProcessFrameData - Large Packet of " + frameLength + " for Channel " + frameChannel + " remainingInBuffer=" + remainingInBuffer);
                            }
                        }
                        else goto CheckRemainingData;
                    }
                    //Determine how much we can move
                    int toMove = Binary.Min(ref remainingInBuffer, ref sessionRequired);

                    //Indicate what was received if not already done
                    if (false == unrelatedData) OnInterleavedData(buffer, offset, toMove);

                    //Move the offset
                    offset += toMove;

                    //Decrease by the length
                    remainingInBuffer -= toMove;

                    //Do another pass
                    continue;

                }
                else//There is not enough data in the buffer as defined by sessionRequired.
                {
                    //unset the frameLength read
                    frameLength = -1;

                    //unset the context read
                    relevent = null;
                }

            //At this point there may be either less sessionRequired or not enough for a complete frame.
            CheckRemainingData:

                //See how many more bytes are required from the wire
                //If the frameLength is less than 0 AND there are less then are sessionRequired remaining in the buffer
                remainingOnSocket = frameLength < 0 && remainingInBuffer < sessionRequired ?
                    sessionRequired - remainingInBuffer //Receive enough to complete the header
                        : //Otherwise if the frameLength larger then what remains in the buffer allow for the buffer to be filled or nothing else remains.
                    frameLength > remainingInBuffer ? frameLength - remainingInBuffer : 0;

                //If there is anymore data remaining on the wire
                if (remainingOnSocket > 0)
                {
                    //Align the buffer if anything remains on the socket.
                    Array.Copy(buffer, offset, buffer, m_Buffer.Offset, remainingInBuffer);

                    //Set the correct offset either way.
                    offset = m_Buffer.Offset + remainingInBuffer;

                    //Store the error if any
                    SocketError error = SocketError.SocketError;

                    //Get all the remaining data
                    while (false == IsDisposed && remainingOnSocket > 0)
                    {
                        //Recieve from the wire the amount of bytes required (up to the length of the buffer)
                        int recievedFromWire = socket == null ? 0 : await SocketExtensions.AlignedReceive(buffer, offset, remainingOnSocket, socket);

                        if (recievedFromWire > 0)
                        {
                            error = SocketError.Success;
                        }

                        //Check for an error and then the allowed continue condition
                        if (error != SocketError.Success && error != SocketError.TryAgain) break;

                        //If nothing was recieved try again.
                        if (recievedFromWire <= 0) continue;

                        //Decrease what is remaining from the wire by what was received
                        remainingOnSocket -= recievedFromWire;

                        //Move the offset
                        offset += recievedFromWire;

                        //Increment received
                        recievedTotal += recievedFromWire;

                        //Incrment remaining in buffer for what was recieved.
                        remainingInBuffer += recievedFromWire;
                    }

                    //If a socket error occured remove the context so no parsing occurs
                    if (error != SocketError.Success)
                    {
                        OnInterleavedData(buffer, offset - remainingInBuffer, remainingInBuffer);
                        return recievedTotal;
                    }

                    //Move back to where the frame started
                    offset -= remainingInBuffer;

                    //if incompatible was marked then continue.
                    if (needsHeaderData) continue;
                }

                //If the event was already raised the frameLength indicates how much to move the offset since offset is not moved by reference.
                if (unrelatedData)
                {
                    offset += frameLength;

                    remainingInBuffer -= frameLength;

                    continue;
                }
                else if (false == IsDisposed && frameLength > 0)
                {
                    //Parse the data in the buffer using only the data related to the packet and not the framing.
                    //using (var memory = new MemorySegment(buffer, offset + sessionRequired, frameLength - sessionRequired))
                    using (MemorySegment memory = new MemorySegment(buffer, offset + sessionRequired, Binary.Min(frameLength - sessionRequired, bufferLength - (offset + sessionRequired))))
                    {
                        //Handle this data
                        await ParseAndHandleData(memory, expectRtcp, expectRtp, memory.Count);

                        //Decrease remaining in buffer
                        remainingInBuffer -= frameLength;

                        //Move the offset
                        offset += frameLength;
                    }

                    //Ensure large frames are completely received by receiving the rest of the frame now. (this only occurs for packets being skipped)
                    if (frameLength > bufferLength)
                    {
                        //Remove the context
                        relevent = null;

                        //Determine how much remains
                        remainingOnSocket = frameLength - bufferLength;

                        //If there is anything left
                        if (remainingOnSocket > 0)
                        {
                            //Set the new length of the frame based on the length of the buffer
                            frameLength -= bufferLength;

                            //Set what is remaining
                            remainingInBuffer = 0;

                            //Use all the buffer
                            offset = m_Buffer.Offset;

                            //go to receive it
                            goto CheckRemainingData;
                        }
                    }
                }
            }

            //Handle any data which remains if not already
            if (false == unrelatedData && remainingInBuffer > 0)
            {
                OnInterleavedData(buffer, offset, remainingInBuffer);
            }
            return recievedTotal;
        }

        /// <summary>
        /// Parses the data in the buffer for valid Rtcp and Rtcp packet instances.
        /// </summary>
        /// <param name="memory">The memory to parse</param>
        /// <param name="from">The socket which received the data into memory and may be used for packet completion.</param>
        internal protected virtual async Task ParseAndHandleData(MemorySegment memory, bool parseRtcp = true, bool parseRtp = true, int? remaining = null)
        {

            if (IDisposedExtensions.IsNullOrDisposed(memory) || memory.Count == 0) return;

            //handle demultiplex scenarios e.g. RFC5761
            if (parseRtcp == parseRtp && memory.Count > RFC3550.CommonHeaderBits.Size)
            {
                using (var header = new RFC3550.CommonHeaderBits(memory))
                {
                    //Just use the payload type to avoid confusion, payload types for Rtcp and Rtp cannot and should not overlap
                    parseRtcp = false == (parseRtp = GetContextByPayloadType(header.RtpPayloadType) != null);
                }
            }

            //Cache start, count and index
            int offset = memory.Offset, count = memory.Count, index = 0,
                //Calulcate remaining
            mRemaining = remaining ?? count - index;

            //If there is nothing left to parse then return
            if (count <= 0) return;

            //If rtcp should be parsed
            if (parseRtcp && mRemaining >= RtcpHeader.Length)
            {
                //parse valid RtcpPackets out of the buffer now, if any packet is not complete it will be completed only if required.
                foreach (RtcpPacket rtcp in RtcpPacket.GetPackets(memory.Array, offset + index, mRemaining))
                {
                    //Raise an event for each packet.
                    await HandleIncomingRtcpPacket(this, rtcp);

                    //Move the offset the length of the packet parsed
                    index += rtcp.Length;

                    mRemaining -= rtcp.Length;
                }
            }

            //If rtp is parsed
            if (parseRtp && mRemaining >= RtpHeader.Length)
            {
                //Create sub memory to maintain the offsets
                using (var subMemory = new MemorySegment(memory.Array, offset + index, mRemaining))
                {
                    //Create the packet from the sub memory
                    using (RtpPacket rtp = new RtpPacket(new RtpHeader(subMemory), new MemorySegment(subMemory.Array, subMemory.Offset + RtpHeader.Length, Binary.Max(0, mRemaining - RtpHeader.Length))))
                    {
                        //Raise the event
                        HandleIncomingRtpPacket(this, rtp);

                        //Move the index past the length of the packet
                        index += rtp.Length;

                        //Calculate the amount of octets remaining in the segment.
                        mRemaining -= rtp.Length;
                    }
                }
            }

            //If not all data was consumed
            if (mRemaining > 0)
            {
                LoggerInstance.Error(ToString() + "@ParseAndCompleteData - Remaining= " + mRemaining);
            }

            return;
        }

        /// <summary>
        /// Entry point of the m_WorkerThread. Handles sending out RtpPackets and RtcpPackets in buffer and handling any incoming RtcpPackets.
        /// Sends a Goodbye and exits if no packets are sent of recieved in a certain amount of time
        /// </summary>
        async void SendReceieve()
        {
            Started = DateTime.UtcNow;

            unchecked
            {
            Begin:
                try
                {
                    LoggerInstance.Debug(ToString() + "@SendRecieve - Begin");

                    DateTime lastOperation = DateTime.UtcNow;

                    bool shouldStop = IsDisposed || m_StopRequested;

                    //Until aborted
                    while (false == shouldStop)
                    {
                        //Keep how much time has elapsed thus far
                        TimeSpan taken = DateTime.UtcNow - lastOperation;

                        //peek thread exit
                        if (shouldStop || IsDisposed || m_StopRequested)
                        {
                            LoggerInstance.Debug("SendReceieve Thread Quit");
                            return;
                        }

                        //Loop each context, newly added contexts will be seen on each iteration
                        for (int i = 0; false == (shouldStop || IsDisposed || m_StopRequested) && i < TransportContexts.Count; ++i)
                        {
                            //Obtain a context
                            TransportContext tc = TransportContexts[i];

                            if (false == tc.IsContinious && tc.TimeRemaining < TimeSpan.Zero)
                            {
                                shouldStop = true;
                            }

                            if (tc.IsContinious && tc.Goodbye != null)
                            {
                                shouldStop = true;
                            }

                            //Check for a context which is able to receive data
                            if (IDisposedExtensions.IsNullOrDisposed(tc)
                                //Active must be true
                                || false == tc.IsActive
                                //If the context does not have continious media it must only receive data for the duration of the media.
                                || false == tc.IsContinious && tc.TimeRemaining < TimeSpan.Zero
                                //There can't be a Goodbye sent or received
                                || tc.Goodbye != null) continue;

                            //Receive Data on the RtpSocket and RtcpSocket, summize the amount of bytes received from each socket.

                            int receivedRtp = 0;

                            bool duplexing = tc.IsDuplexing, rtpEnabled = tc.IsRtpEnabled, rtcpEnabled = tc.IsRtcpEnabled;

                            //If receiving Rtp and the socket is able to read
                            if (rtpEnabled && false == (shouldStop || IsDisposed || m_StopRequested))
                            {
                                //Receive RtpData
                                var receiveResult = await ReceiveData(tc.RtpSocket, tc.RemoteRtp, rtpEnabled, duplexing, tc.ContextMemory);
                                receivedRtp += receiveResult.Item2;
                                //Check if an error occured
                                if (receiveResult.Item1 != SocketError.Success)
                                {
                                    //Increment for failed receptions
                                    ++tc.m_FailedRtpReceptions;

                                    //Log for the error
                                    LoggerInstance.Error(ToString() + "@SendRecieve RtpSocket -" + " lastOperation = " + lastOperation + " taken = " + taken);
                                }
                                else if (receivedRtp >= 0)
                                    lastOperation = DateTime.UtcNow;
                            }
                            else if (rtpEnabled && taken >= tc.m_ReceiveInterval)
                            {
                                LoggerInstance.Error(ToString() + "@SendRecieve - Unable to Poll RtpSocket in tc.m_ReceiveInterval = " + tc.ReceiveInterval + ", taken =" + taken);
                            }

                            //if Rtcp is enabled
                            if (rtcpEnabled && false == (shouldStop || IsDisposed || m_StopRequested))
                            {
                                if (await SendReports(tc) || await SendGoodbyeIfInactive(lastOperation, tc)) lastOperation = DateTime.UtcNow;
                            }
                        }
                    }
                }
                catch (ThreadAbortException)
                {
                    LoggerInstance.Error(ToString() + "@SendRecieve Aborted");
                    Thread.CurrentThread.Abort();
                }
                catch (Exception ex)
                {
                    LoggerInstance.Error(ToString() + "@SendRecieve: " + ex.Message);
                    //goto Begin;
                }
                finally
                {
                    System.Diagnostics.Debug.WriteLine("Exit");
                }
            }
            LoggerInstance.Info(ToString() + "@SendRecieve - Exit");
        }

        #endregion

        #region Overloads

        public override string ToString()
        {
            return string.Join(((char)ASCII.HyphenSign).ToString(), base.ToString(), InternalId);
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Calls <see cref="Deactivate"/> and disposes all contained <see cref="RtpClient.TransportContext"/>.
        /// Stops the raising of any events.
        /// Removes the Logger
        /// </summary>
        public override async void Dispose()
        {
            if (IsDisposed) return;

            await Deactivate();

            base.Dispose();

            if (false == ShouldDispose) return;

            DisposeAndClearTransportContexts();

            //Stop raising events
            RtpPacketSent = null;
            RtcpPacketSent = null;
            RtpPacketReceieved = null;
            RtcpPacketReceieved = null;
            InterleavedData = null;

            //Send abort signal to all threads contained.
            //Todo, maybe offer Delegate AbortDelegate..
            IThreadReferenceExtensions.AbortAndFreeAll(this);

            //Empty packet buffers
            m_OutgoingRtpPackets.Clear();

            //m_OutgoingRtpPackets = null;

            m_OutgoingRtcpPackets.Clear();

            //m_OutgoingRtcpPackets = null;

            AdditionalSourceDescriptionItems.Clear();

            ClientName = "";

            //Remove the buffer
            m_Buffer.Dispose();

            m_Buffer = null;

            LoggerInstance.Error(GetType().Name + "(" + ToString() + ")@Dipose - Complete");
        }

        #endregion

        IEnumerable<System.Threading.Thread> IThreadReference.GetReferencedThreads()
        {
            IEnumerable<System.Threading.Thread> threads = System.Linq.Enumerable.Empty<System.Threading.Thread>();

            if (IsDisposed) return threads;

            if (IsActive) threads = threads.Concat(LinqExtensions.Yield(m_WorkerThread));

            return threads;
        }
    }
}
