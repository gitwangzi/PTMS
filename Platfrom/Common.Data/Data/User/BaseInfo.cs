using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    [Serializable]
    public class BaseInfo
    {
        string _clientid = string.Empty;

        [DataMember]
        public string ClientID
        {
            get { return _clientid; }
            set { _clientid = value; }
        }

        string _userid = string.Empty;
        [DataMember]
        public string UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }

        int _messagetype = 0;
        [DataMember]
        public int MessageType
        {
            get { return _messagetype; }
            set { _messagetype = value; }
        }
    }
}
