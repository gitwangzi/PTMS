using Gsafety.PTMS.TransferLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.TransferService
{
    partial class TransferService : ServiceBase
    {
        public TransferService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Diagnostics.Debugger.Launch();
            TransferProcess.Start();
        }

        protected override void OnStop()
        {
            TransferProcess.Stop();
        }
    }
}
