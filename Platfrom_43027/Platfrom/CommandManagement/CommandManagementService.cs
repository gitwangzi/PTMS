using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.CommandManagement
{
    public partial class CommandManagementService: ServiceBase
    {
        public CommandManagementService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //CommandManager.Start();
        }

        protected override void OnStop()
        {
            CommandManager.End();
        }
    }
}
