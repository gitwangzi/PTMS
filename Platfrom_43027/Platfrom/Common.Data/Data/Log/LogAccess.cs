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
    public class LogAccess
    {
        string _id;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

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

        DateTime _logintime;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public DateTime LoginTime
        {
            get
            {
                return _logintime;
            }
            set
            {
                _logintime = value;
            }
        }

        string _loginuser;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string LoginUser
        {
            get
            {
                return _loginuser;
            }
            set
            {
                _loginuser = value;
            }
        }

        DateTime? _logouttime;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public DateTime? LogoutTime
        {
            get
            {
                return _logouttime;
            }
            set
            {
                _logouttime = value;
            }
        }

        string _sessionid;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string SessionID
        {
            get
            {
                return _sessionid;
            }
            set
            {
                _sessionid = value;
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

        decimal _usertype;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public decimal UserType
        {
            get
            {
                return _usertype;
            }
            set
            {
                _usertype = value;
            }
        }


        string _showRoleName;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string ShowRoleName
        {
            get
            {
                return _showRoleName;
            }
            set
            {
                _showRoleName = value;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ClientID)))
            {
                builder.AppendLine("ClientID:" + ClientID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(LoginTime)))
            {
                builder.AppendLine("LoginTime:" + LoginTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(LoginUser)))
            {
                builder.AppendLine("LoginUser:" + LoginUser.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(LogoutTime)))
            {
                builder.AppendLine("LogoutTime:" + LogoutTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SessionID)))
            {
                builder.AppendLine("SessionID:" + SessionID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserID)))
            {
                builder.AppendLine("UserID:" + UserID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserType)))
            {
                builder.AppendLine("UserType:" + UserType.ToString());
            }

            return builder.ToString();
        }

    }
}

