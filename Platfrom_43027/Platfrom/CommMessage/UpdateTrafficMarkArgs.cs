using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Bases.Enums;
using System;
using System.Collections.Generic;
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
    /// update traffice management draw 
    /// </summary>
    public class UpdateTrafficMarkArgs
    {
        /// <summary>
        /// primary key id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// type
        /// </summary>
        public TrafficFeature nType { get; set; }
        /// <summary>
        /// fujia traffice rules information
        /// </summary>
        public object TrafficInfo { get; set; }
    }
}
