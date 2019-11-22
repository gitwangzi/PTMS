using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [Serializable]
    public class SendCommand
    {
        public SendCommand()
        {
            Records = new List<SendRecord>();
        }

        private string _clientid;
        public string ClientID
        {
            get
            {
                return _clientid;
            }
            set
            {
                _clientid = value;
            }
        }

        string _username = string.Empty;

        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        public List<SendRecord> Records
        {
            get;
            set;
        }
    }
}
