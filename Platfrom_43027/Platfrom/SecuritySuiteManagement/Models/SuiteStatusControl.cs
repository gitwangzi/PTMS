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

namespace Gsafety.PTMS.SecuritySuite.Models
{
    public class SuiteStatusControl
    {
        public string VehicleID {get;set;}
        public string SuiteID { get; set; }
        public short CurrentStatus { get; set; }
        public short ChangedStatus { get; set; }
        public string UserInfo { get; set; }
        public string MdvrCID { get; set; }
        public string Suite_info_id { get; set; }
    }
}
