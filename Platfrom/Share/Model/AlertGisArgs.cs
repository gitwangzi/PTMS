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

namespace Gsafety.PTMS.Share
{
    public class AlertGisArgs : EventArgs
    {
        public string VehicleID
        {
            get;
            set;
        }

        //0 警情处理完毕
        //1 新加警情
        public int Alert { get; set; }
    }
}
