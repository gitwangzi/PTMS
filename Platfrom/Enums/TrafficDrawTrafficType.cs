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

namespace Gsafety.PTMS.Bases.Enums
{
    /// <summary>
    /// TrafficDrawFenceType
    /// </summary>
    public enum TrafficDrawType
    {
        /// <summary>
        /// Point
        /// </summary>
        Point = 1,
        /// <summary>
        /// Polygon
        /// </summary>
        Polygon = 2,
        /// <summary>
        /// Line
        /// </summary>
        Line = 3,
         
        /// <summary>
        /// 矩形
        /// </summary>
        Rectangle = 4,

        /// <summary>
        /// 圆
        /// </summary>
        Circular = 5

    }
}
