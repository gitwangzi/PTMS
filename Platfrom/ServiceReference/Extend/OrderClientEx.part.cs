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

namespace Gsafety.PTMS.ServiceReference.OrderClientService
{
    public partial class OrderClientEx
    {
        public Visibility ResumeVisibility
        {
            get;
            set;
        }

        public Visibility SuspendedVisibility
        {
            get;
            set;
        }

        public string VersionStr
        {
            get;
            set;
        }
    }
}
