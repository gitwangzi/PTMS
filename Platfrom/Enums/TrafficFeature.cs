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
    /// TrafficFeature
    /// </summary>
    public enum TrafficFeature
    {
        Traffic_NoFeature = -1,

        /// <summary>
        /// Traffic_PolygonFence
        /// </summary>
        Traffic_PolygonFence = 2,

        Traffic_Route=3,

        /// <summary>
        /// Traffic_SpeedLimit
        /// </summary>
        Traffic_SpeedLimit = 6,

        /// <summary>
        /// Traffic_ThemeMap
        /// </summary>
        Traffic_ThemeMap = 8
    }
}
