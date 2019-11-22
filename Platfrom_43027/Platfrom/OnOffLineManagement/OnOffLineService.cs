using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace OnOffLineManagement
{
    public partial class OnOffLineService : ServiceBase
    {
        public OnOffLineService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            DealWithMessage.Start();
        }

        protected override void OnStop()
        {
            DealWithMessage.Stop();
        }
    }
}
