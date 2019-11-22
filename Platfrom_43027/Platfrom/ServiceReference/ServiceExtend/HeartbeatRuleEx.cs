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

namespace Gsafety.PTMS.ServiceReference.CommandManageService
{
    public partial class HeartbeatRule
    {
        public int Index
        {
            get;
            set;
        }

        public bool UpdateEnable
        {
            get;
            set;
        }

        public bool DeleteEnable
        {
            set;
            get;
        }
    }
}
