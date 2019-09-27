using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gs.PTMS.MessagePolicyService
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketPolicyServer SPServer =
                //Policy file path is defined in App.config of the application
            new SocketPolicyServer(ConfigurationManager.AppSettings["PolicyFilePath"]);

            Console.WriteLine("Policy server is started.");

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            SPServer.Close();
            Console.WriteLine("Policy server shut down.");
        }
    }
}
