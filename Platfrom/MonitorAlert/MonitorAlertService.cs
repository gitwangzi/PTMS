using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.MonitorAlert
{
    partial class MonitorAlertService : ServiceBase
    {
        public MonitorAlertService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ////Service start
            MonitorAlertMessage.Start();
        }

        protected override void OnStop()
        {
            ////Service stop
            MonitorAlertMessage.Shop();
        }
    }
}
