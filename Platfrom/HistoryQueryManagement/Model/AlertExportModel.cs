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

namespace Gsafety.Ant.HistoryQueryManagement.Model
{
    public class AlertExportModel
    {
        public string VehicleId { get; set; }

        public string SuiteID { get; set; }

        public DateTime? AlertTime { get; set; }

        public string AlertType { get; set; }

        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Speed { get; set; }
        public string Direction { get; set; }

    }
}
