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

namespace Gsafety.PTMS.ServiceReference.BscDevSuiteService
{
    public partial class DevSuite
    {
        public bool Editable
        {
            get
            {
                if (this.InstallStatus == InstallStatusType.UnInstall)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
