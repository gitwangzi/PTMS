using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;

namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///
    ///</summary>
    [DataContract]
    public class UserOnline
    {
        string _clientid;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
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

        DateTime _onlinetime;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public DateTime OnlineTime
        {
            get
            {
                return _onlinetime;
            }
            set
            {
                _onlinetime = value;
            }
        }

        string _username;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string UserName
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }

        string _userid;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string UserID
        {
            get
            {
                return _userid;
            }
            set
            {
                _userid = value;
            }
        }


        string _rolename;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string RoleName
        {
            get
            {
                return _rolename;
            }
            set
            {
                _rolename = value;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ClientID)))
            {
                builder.AppendLine("ClientID:" + ClientID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OnlineTime)))
            {
                builder.AppendLine("OnlineTime:" + OnlineTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserName)))
            {
                builder.AppendLine("UserName:" + UserName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserID)))
            {
                builder.AppendLine("UserID:" + UserID.ToString());
            }

            return builder.ToString();
        }

    }
}

