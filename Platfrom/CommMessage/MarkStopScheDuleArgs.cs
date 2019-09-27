using Gsafety.Common.CommMessage.Controls;
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
    /// 标绘行驶计划
    /// </summary>
    public class MarkStopScheDuleArgs
    {
        /// <summary>
        /// 行驶计划ID
        /// </summary>
        public string StopScheDuleID { get; set; }
        /// <summary>
        /// 行驶计划名称
        /// </summary>
        public string StopScheDuleName { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool bShow { get; set; }
        /// <summary>
        /// 标绘的符号样式
        /// </summary>
        public SymbolParams MarkSymbolParm;
    }
}
