using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gs.PTMS.MessageCenterService
{
    class DataManager
    {
        public static Dictionary<string, Dictionary<string, SocketConnection>> Clients = new Dictionary<string, Dictionary<string, SocketConnection>>();

        public static Dictionary<string, Dictionary<string, SocketConnection>> Organization = new Dictionary<string, Dictionary<string, SocketConnection>>();

        public static Dictionary<string, Dictionary<string, SocketConnection>> Users = new Dictionary<string, Dictionary<string, SocketConnection>>();

        public static Dictionary<string, Dictionary<string, SocketConnection>> Vehicles = new Dictionary<string, Dictionary<string, SocketConnection>>();

        public static Dictionary<string, SocketConnection> SocketClients = new Dictionary<string, SocketConnection>();
    }
}
