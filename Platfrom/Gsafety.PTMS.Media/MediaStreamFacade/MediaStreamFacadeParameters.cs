using Gsafety.PTMS.Media.RTSP;
using System;

namespace Gsafety.PTMS.Media
{
    public class MediaStreamFacadeParameters
    {
        public static TimeSpan DefaultStartTimeout = TimeSpan.FromSeconds(5);

        public MediaStreamFacadeParameters()
        {
            CreateTimeout = DefaultStartTimeout;
            UseRtspStreamMediaManager = true;
        }

        public Func<IMediaStreamFacadeBase> Factory { get; set; }

        /// <summary>
        ///     Use the socket-based <see cref="Gsafety.PTMS.Media.Web.HttpConnection.HttpConnection" /> instead of the
        ///     platform's default HTTP client (usually HttpClient).
        ///     UseRtspStreamMediaManager为True时，此参数无效
        /// </summary>
        public bool UseHttpConnection { get; set; }

        /// <summary>
        /// 是否播放RTSP协议流，默认为True
        /// </summary>
        public bool UseRtspStreamMediaManager { get; set; }

        /// <summary>
        ///     Cancel playback if it takes longer than this to create the media stream source.
        /// </summary>
        public TimeSpan CreateTimeout { get; set; }

        public bool IsLogIncomingRtpPacket
        {
            get
            {
                return RtspLogSwitch.IsLogIncomingRtpPacket;
            }
            set
            {
                RtspLogSwitch.IsLogIncomingRtpPacket = value;
            }
        }

        public bool IsLogIncomingRtcpPacket
        {
            get
            {
                return RtspLogSwitch.IsLogIncomingRtcpPacket;
            }
            set
            {
                RtspLogSwitch.IsLogIncomingRtcpPacket = value;
            }
        }

        public bool IsLogOutgoingRtcpPacket
        {
            get
            {
                return RtspLogSwitch.IsLogOutgoingRtcpPacket;
            }
            set
            {
                RtspLogSwitch.IsLogOutgoingRtcpPacket = value;
            }
        }

        public bool IsLogRtpDataWith0x
        {
            get
            {
                return RtspLogSwitch.IsLogRtpDataWith0x;
            }
            set
            {
                RtspLogSwitch.IsLogRtpDataWith0x = value;
            }
        }
    }
}
