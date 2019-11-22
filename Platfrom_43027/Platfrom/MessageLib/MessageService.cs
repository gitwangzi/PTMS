using Gsafety.PTMS.Message.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.MessageLib
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public partial class MessageService : IMessageServiceExt
    {
        public MessageService()
        {
            _queue = "Message.Private." + Dns.GetHostName();

            Init();
        }


    }
}
