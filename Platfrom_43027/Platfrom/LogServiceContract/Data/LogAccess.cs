using System;
using System.Runtime.Serialization;
using System.Text;

namespace LogServiceContract.Data
{
    ///<summary>
    ///访问日志
    ///</summary>
    [DataContract]
    public class LogAccess
    {
        string _id;

        ///<summary>
        ///主键
        ///</summary>
        [DataMember]
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        string _clientid;

        ///<summary>
        ///客户账号
        ///</summary>
        [DataMember]
        public string ClientID
        {
            get { return _clientid; }
            set { _clientid = value; }
        }

        string _organizationid;

        ///<summary>
        ///所属单位（或机构）
        ///</summary>
        [DataMember]
        public string OrganizationID
        {
            get { return _organizationid; }
            set { _organizationid = value; }
        }

        DateTime _logintime;

        ///<summary>
        ///登录时间
        ///</summary>
        [DataMember]
        public DateTime LoginTime
        {
            get { return _logintime; }
            set { _logintime = value; }
        }

        DateTime _logouttime;

        ///<summary>
        ///登出时间
        ///</summary>
        [DataMember]
        public DateTime LogoutTime
        {
            get { return _logouttime; }
            set { _logouttime = value; }
        }

        string _loginuser;

        ///<summary>
        ///登录人
        ///</summary>
        [DataMember]
        public string LoginUser
        {
            get { return _loginuser; }
            set { _loginuser = value; }
        }

        decimal _usertype;

        ///<summary>
        ///用户类型
        ///</summary>
        [DataMember]
        public decimal UserType
        {
            get { return _usertype; }
            set { _usertype = value; }
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
            if (!string.IsNullOrEmpty(Convert.ToString(OrganizationID)))
            {
                builder.AppendLine("OrganizationID:" + OrganizationID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(LoginTime)))
            {
                builder.AppendLine("LoginTime:" + LoginTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(LogoutTime)))
            {
                builder.AppendLine("LogoutTime:" + LogoutTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(LoginUser)))
            {
                builder.AppendLine("LoginUser:" + LoginUser.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserType)))
            {
                builder.AppendLine("UserType:" + UserType.ToString());
            }

            return builder.ToString();
        }
    }

}
