using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCenterService.Share
{
    public class RegisterEntity
    {
        string _userid = string.Empty;

        public string UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }
        List<string> _organization = new List<string>();

        public List<string> Organization
        {
            get { return _organization; }
            set { _organization = value; }
        }

        string _usertoken = string.Empty;

        public string UserToken
        {
            get { return _usertoken; }
            set { _usertoken = value; }
        }
    }
}
