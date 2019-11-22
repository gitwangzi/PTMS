using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
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

namespace Gsafety.Common.CommMessage
{
    public class AlarmLocationAddRemoveArgs : AlarmInfoEx
    {
        //1 draw，0 delete
        public int Op { get; set; }
    }

    public class FullMapArgs
    {
        //1 full，0 small
        public int Op { get; set; }
    }
}
