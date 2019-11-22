using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gs.PTMS.MessageCenterService
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketManager socketmanager = new SocketManager();
            socketmanager.Start();

            Console.ReadLine();
        }
    }
}
