using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Video
{
    public partial class VideoService : ServiceBase
    {
        public VideoService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            VideoMessage.Start();
        }

        protected override void OnStop()
        {
            VideoMessage.End();
        }
    }
}
