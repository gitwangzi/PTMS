using Gsafety.PTMS.MessageLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MessageServiceExt
{
    partial class MessageServiceExt : ServiceBase
    {
        ServiceHost servicehost = null;
        public MessageServiceExt()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            servicehost = new ServiceHost(typeof(MessageService));
            servicehost.Open();
        }

        protected override void OnStop()
        {
            servicehost.Close();
        }
    }
}
