using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace Gsafety.PTMS.Manager.Contract.Data
{
    ///<summary>
    ///用户表
    ///</summary>
    [DataContract]
    public class GUser
    {
        string _id;
        ///<summary>
        ///主键
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

        string _account;
        ///<summary>
        ///账户名
        ///</summary>
        [DataMember]
        public string Account
        {
            get
            {
                return _account;
            }
            set
            {
                _account = value;
            }
        }

        string _user_name;
        ///<summary>
        ///用户名
        ///</summary>
        [DataMember]
        public string UserName
        {
            get
            {
                return _user_name;
            }
            set
            {
                _user_name = value;
            }
        }

        DateTime _create_time;
        ///<summary>
        ///创建时间
        ///</summary>
        [DataMember]
        public DateTime CreateTime
        {
            get
            {
                return _create_time;
            }
            set
            {
                _create_time = value;
            }
        }

        string _password;
        ///<summary>
        ///密码
        ///</summary>
        [DataMember]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        string _phone;
        ///<summary>
        ///电话
        ///</summary>
        [DataMember]
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
            }
        }

        string _mobile;
        ///<summary>
        ///手机
        ///</summary>
        [DataMember]
        public string Mobile
        {
            get
            {
                return _mobile;
            }
            set
            {
                _mobile = value;
            }
        }

        string _email;
        ///<summary>
        ///邮箱
        ///</summary>
        [DataMember]
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }

        string _address;
        ///<summary>
        ///地址
        ///</summary>
        [DataMember]
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }

        string _description;
        ///<summary>
        ///描述
        ///</summary>
        [DataMember]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        string _role_id;
        ///<summary>
        ///角色
        ///</summary>
        [DataMember]
        public string RoleID
        {
            get
            {
                return _role_id;
            }
            set
            {
                _role_id = value;
            }
        }

        string _creator;
        ///<summary>
        ///创建人
        ///</summary>
        [DataMember]
        public string Creator
        {
            get
            {
                return _creator;
            }
            set
            {
                _creator = value;
            }
        }

        bool _is_client_create;
        ///<summary>
        ///是否客户创建
        ///</summary>
        [DataMember]
        public bool IsClientCreate
        {
            get
            {
                return _is_client_create;
            }
            set
            {
                _is_client_create = value;
            }
        }

        string _department;
        ///<summary>
        ///部门
        ///</summary>
        [DataMember]
        public string Department
        {
            get
            {
                return _department;
            }
            set
            {
                _department = value;
            }
        }

        string _client_id;
        ///<summary>
        ///客户账号
        ///</summary>
        [DataMember]
        public string ClientID
        {
            get
            {
                return _client_id;
            }
            set
            {
                _client_id = value;
            }
        }

        int _rolecategory;
        ///<summary>
        //
        ///</summary>
        [DataMember]
        public int RoleCategory
        {
            get
            {
                return _rolecategory;
            }
            set
            {
                _rolecategory = value;
            }
        }

        string _roleName;
        ///<summary>
        ///客户账号
        ///</summary>
        [DataMember]
        public string RoleName
        {
            get
            {
                return _roleName;
            }
            set
            {
                _roleName = value;
            }
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Account)))
            {
                builder.AppendLine("Account:" + Account.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserName)))
            {
                builder.AppendLine("UserName:" + UserName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Password)))
            {
                builder.AppendLine("Password:" + Password.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Phone)))
            {
                builder.AppendLine("Phone:" + Phone.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Mobile)))
            {
                builder.AppendLine("Mobile:" + Mobile.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Email)))
            {
                builder.AppendLine("Email:" + Email.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Address)))
            {
                builder.AppendLine("Address:" + Address.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Description)))
            {
                builder.AppendLine("Description:" + Description.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RoleID)))
            {
                builder.AppendLine("RoleID:" + RoleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Creator)))
            {
                builder.AppendLine("Creator:" + Creator.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IsClientCreate)))
            {
                builder.AppendLine("IsClientCreate:" + IsClientCreate.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Department)))
            {
                builder.AppendLine("Department:" + Department.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ClientID)))
            {
                builder.AppendLine("ClientID:" + ClientID.ToString());
            }

            return builder.ToString();
        }
    }
}

