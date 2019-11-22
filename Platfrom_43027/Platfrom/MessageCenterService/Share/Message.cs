using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCenterService.Share
{
    public class Message
    {
        MessageType _messagetype;

        public MessageType MessageType
        {
            get { return _messagetype; }
            set { _messagetype = value; }
        }

        string _clientID;

        public string ClientID
        {
            get { return _clientID; }
            set { _clientID = value; }
        }

        string _bodytype;
        public string BodyType
        {
            get { return _bodytype; }
            set { _bodytype = value; }
        }

        byte[] _body = null;

        public byte[] Body
        {
            get { return _body; }
            set { _body = value; }
        }
    }
}
