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
using Gsafety.PTMS.Bases.Enums;

namespace Gsafety.Common.CommMessage
{
    /// <summary>
    /// server notice gis draw ,if draw failed or nothing to draw return false，and change object draw status to false
    /// </summary>
    public class UpdateOjectStatusNoMarkArgs
    {
        public object UpdateObject { get; set; }
        public TrafficFeature markType { get; set; } 
    }
}
