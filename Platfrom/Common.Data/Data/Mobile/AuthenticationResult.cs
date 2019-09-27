using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    public class AuthenticationResult
    {
        string authcode = string.Empty;

        public string AuthCode
        {
            get { return authcode; }
            set { authcode = value; }
        }

        string name = string.Empty;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string clientid = string.Empty;

        public string ClientID
        {
            get { return clientid; }
            set { clientid = value; }
        }

        string userid = string.Empty;

        public string UserID
        {
            get { return userid; }
            set { userid = value; }
        }
    }
}
