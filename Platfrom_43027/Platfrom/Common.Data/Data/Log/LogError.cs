using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class LogError
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

        DateTime _createTime;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public DateTime CreateTime
        {
            get
            {
                return _createTime;
            }
            set
            {
                _createTime = value;
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

       
        string _errorReason;
        [DataMember]
        public string ErrorReason
        {
            get
            {
                return _errorReason;
            }
            set
            {
                _errorReason = value;
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
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("LoginTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(LoginUser)))
            {
                builder.AppendLine("LoginUser:" + LoginUser.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ErrorReason)))
            {
                builder.AppendLine("LogoutTime:" + ErrorReason.ToString());
            }

            return builder.ToString();
        }

    }
}
