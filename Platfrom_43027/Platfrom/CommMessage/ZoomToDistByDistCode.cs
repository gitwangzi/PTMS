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
    /// change to provice city  based on place code
    /// </summary>
    public class ZoomToDistByDistCode
    {
        /// <summary>
        ///layer type
        /// </summary>
        public ZoomToDistType DISTTYPE { get; set; }
        /// <summary>
        /// place code
        /// </summary>
        public string DISTCODE { get; set; }
    }
}
