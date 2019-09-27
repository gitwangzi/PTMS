using Gsafety.PTMS.TransferLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TransferService
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Gsafety.PTMS.TransferService.TransferService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
