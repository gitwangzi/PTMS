using Gs.PTMS.MessageCenterService;
using MessageCenterService.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCenterService
{
    public class MessageHandler
    {
        public virtual void OnMessage(Message message, SocketConnection connection)
        {

        }
    }
}
