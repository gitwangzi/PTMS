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

namespace GisManagement.Models
{
    public class DisplayHistoricalRoute
    {
        public string VechileId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class GisFixChangeRoute
    {
        public string VechileId { get; set; }
     
    }

    public class GisTraceChangeRoute
    {
        public string VechileId { get; set; }

    }
}
