using Gsafety.PTMS.AnalysisLib;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Analysis
{
    partial class BusinessService : ServiceBase
    {
        public BusinessService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            TransforMessage.Start();
        }

        protected override void OnStop()
        {
            TransforMessage.Stop();
        }
    }
}
