using Gsafety.PTMS.Bases.Enums;
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

namespace Gsafety.PTMS.Enums
{
    public enum MdvrMsgTypeEnum
    {
        //紧急
        //终端显示器显示
        //终端TTS搏读
        //广告屏显示
        //中心导航信息
        //CAN故障码信息

        [EnumAttribute(ResourceName = "Energency")]
        Energency = 01,
        [EnumAttribute(ResourceName = "TerminalDisplay")]
        TerminalDisplay = 21,
        [EnumAttribute(ResourceName = "TerminalTTSRead")]
        TerminalTTSRead = 31,
        [EnumAttribute(ResourceName = "SCreenDisplay")]
        SCreenDisplay = 41,
        [EnumAttribute(ResourceName = "CenterNavigationInfo")]
        CenterNavigationInfo = 50,
        [EnumAttribute(ResourceName = "CANErrorInfo")]
        CANErrorInfo = 1
    }
}
