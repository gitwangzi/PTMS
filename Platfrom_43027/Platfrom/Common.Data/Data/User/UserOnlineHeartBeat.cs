using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [Serializable]
    public class UserOnlineHeartBeat
    {
        List<string> sessionids = new List<string>();

        public List<string> SessionIDs
        {
            get { return sessionids; }
            set { sessionids = value; }
        }
    }
}
