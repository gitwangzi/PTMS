using Gsafety.PTMS.Message.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.MessageLib
{
    class DataManager
    {
        public static Dictionary<string, Dictionary<string, IMessageCallBackExt>> Organization = new Dictionary<string, Dictionary<string, IMessageCallBackExt>>();

        public static Dictionary<string, Dictionary<string, IMessageCallBackExt>> Users = new Dictionary<string, Dictionary<string, IMessageCallBackExt>>();

        public static Dictionary<string, Dictionary<string, IMessageCallBackExt>> Vehicles = new Dictionary<string, Dictionary<string, IMessageCallBackExt>>();

        public static Dictionary<string, Dictionary<string, IMessageCallBackExt>> Clients = new Dictionary<string, Dictionary<string, IMessageCallBackExt>>();

        public static Dictionary<string, IMessageCallBackExt> SocketClients = new Dictionary<string, IMessageCallBackExt>();

        public static Dictionary<string, UserModel> Models = new Dictionary<string, UserModel>();
    }
}
