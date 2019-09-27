using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Analysis.Storage
{
    partial class StorageService : ServiceBase
    {
        public StorageService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Diagnostics.Debugger.Launch();
            StorageMessage.Start();
        }

        protected override void OnStop()
        {
            StorageMessage.Stop();
        }
    }
}
