using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.SoftwareUpgrade
{
    partial class SoftwareUpgradeService : ServiceBase
    {
        public SoftwareUpgradeService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            ProcessMessage.Start();
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            ProcessMessage.Stop();
        }
    }
}
