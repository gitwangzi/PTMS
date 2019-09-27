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
    /// 标绘行驶计划中的监控点
    /// </summary>
    public class MarkTrafficScheDulePointArgs
    {
        /// <summary>
        /// 监控点ID(唯一)
        /// </summary>
        public string PointID { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string PX { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string PY { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool bShow { get; set; }
        /// <summary>
        /// 标绘符号样式
        /// </summary>
        public SymbolParams MarkSymbolParm { get; set; }
    }
}
