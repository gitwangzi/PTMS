using Gsafety.PTMS.MessageLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MessageServiceExt
{
    class Program
    {
        static void Main(string[] args)
        {
            string runmode = "WindowsService";
            if (System.Configuration.ConfigurationManager.AppSettings["RunMode"] != null)
            {
                runmode = System.Configuration.ConfigurationManager.AppSettings["RunMode"].ToString();
            }
            if (runmode == "Console")
            {
                using (ServiceHost servicehost = new ServiceHost(typeof(MessageService)))
                {
                    servicehost.Open();
                    Console.WriteLine("Start....");
                    Console.ReadLine();
                    servicehost.Close();
                    Console.WriteLine("End....");
                }
            }
            else if (runmode == "WindowsService")
            {
                ServiceBase[] ServicesToRun = new ServiceBase[] 
                { 
                    new MessageServiceExt()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
