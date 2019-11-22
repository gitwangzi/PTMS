using Gsafety.PTMS.Integration.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace VideoServerMask
{
    class Program
    {
        static void Main(string[] args)
        {
            Test2();
            Console.Read();
        }

        private static void Test2()
        {
            var xx = new Gsafety.PTMS.Integration.Repository.VedioRepository();
            var result = xx.StatisticsMdvrFlow(new Gsafety.PTMS.Integration.Contract.StatisticsMdvrFlowArgs
              {
                  End_Time = DateTime.Now.AddDays(-1),
                  Mdvr_Id = "1111111111",
                  Start_Time = DateTime.Now.AddDays(-2),
              }, "ant_tester");

        }

        private static void Test1()
        {
            var ip = Dns.GetHostAddresses("").First(x => x.AddressFamily == AddressFamily.InterNetwork);
            System.Net.Sockets.TcpListener _listener = new System.Net.Sockets.TcpListener(ip, 8003);
            Console.WriteLine("Server start...");
            _listener.Start();
            while (true)
            {
                try
                {
                    var client = _listener.AcceptSocket();
                    Task.Factory.StartNew(() =>
                    ClientTest(client)
                    );
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp);
                    _listener.Stop();
                }
            }
        }

        private static void ClientTest(Socket client)
        {
            try
            {
                var buffer = new byte[1024 * 4];
                var count = client.Receive(buffer);
                var ser = new JsonMessageSerializer();
                bool isSucc = false;
                var message = ser.Deserialize(buffer.Take(count).ToList(), out isSucc);
                message.ForEach(x => Console.WriteLine(x));
                client.Send(buffer.Take(count).ToArray());

            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }
    }
}
