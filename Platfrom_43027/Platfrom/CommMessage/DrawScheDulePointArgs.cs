using Gsafety.PTMS.ServiceReference.TrafficManageService;
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
    /// <summary>
    /// 绘制行驶计划监控点信息
    /// </summary>
    public class DrawScheDulePointArgs
    {
        /// <summary>
        /// 界面增加的信息，传给gis，gis绘制点后给经纬度赋值
        /// </summary>
        public StopScheDulePoint AddStopScheDuleInfo { get; set; }
        /// <summary>
        /// 行驶计划信息
        /// </summary>
        public StopScheDule SleStopScheDuleInfo { get; set; }
    }
}
