using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Media.RTSP
{
    public class RtspLogSwitch
    {
        public static bool IsLogIncomingRtpPacket { get; set; }

        public static bool IsLogIncomingRtcpPacket { get; set; }

        public static bool IsLogOutgoingRtcpPacket { get; set; }

        public static bool IsLogRtpDataWith0x { get; set; }
    }
}
