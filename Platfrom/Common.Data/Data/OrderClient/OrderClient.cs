using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;

namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///客户
    ///</summary>
    [DataContract]
    [Serializable]
    public class OrderClient
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

        string _name;
        ///<summary>
        ///名称
        ///</summary>
        [DataMember]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        DateTime _begintime;
        ///<summary>
        ///开始时间
        ///</summary>
        [DataMember]
        public DateTime BeginTime
        {
            get
            {
                return _begintime;
            }
            set
            {
                _begintime = value;
            }
        }

        DateTime _endtime;
        ///<summary>
        ///结束时间
        ///</summary>
        [DataMember]
        public DateTime EndTime
        {
            get
            {
                return _endtime;
            }
            set
            {
                _endtime = value;
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

        string _contact;
        ///<summary>
        ///联系人
        ///</summary>
        [DataMember]
        public string Contact
        {
            get
            {
                return _contact;
            }
            set
            {
                _contact = value;
            }
        }

        TansferModeEnum _tansfermode;
        ///<summary>
        ///保留字段 0：不转警 1：直接转警 2：接警判断后转警
        ///</summary>
        [DataMember]
        public TansferModeEnum TansferMode
        {
            get
            {
                return _tansfermode;
            }
            set
            {
                _tansfermode = value;
            }
        }

        StatusEnum _status;
        ///<summary>
        ///状态
        ///0 正常
        ///1 暂停
        ///</summary>
        [DataMember]
        public StatusEnum Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        int _usercount;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public int UserCount
        {
            get
            {
                return _usercount;
            }
            set
            {
                _usercount = value;
            }
        }

        int _devicecount;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public int DeviceCount
        {
            get
            {
                return _devicecount;
            }
            set
            {
                _devicecount = value;
            }
        }

        [DataMember]
        private int _platformversion;
        public int PlatformVersion
        {
            get
            {
                return _platformversion;
            }
            set
            {
                _platformversion = value;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Name)))
            {
                builder.AppendLine("Name:" + Name.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(BeginTime)))
            {
                builder.AppendLine("BeginTime:" + BeginTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(EndTime)))
            {
                builder.AppendLine("EndTime:" + EndTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Address)))
            {
                builder.AppendLine("Address:" + Address.ToString());
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
            if (!string.IsNullOrEmpty(Convert.ToString(Contact)))
            {
                builder.AppendLine("Contact:" + Contact.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(TansferMode)))
            {
                builder.AppendLine("TansferMode:" + TansferMode.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserCount)))
            {
                builder.AppendLine("UserCount:" + UserCount.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DeviceCount)))
            {
                builder.AppendLine("DeviceCount:" + DeviceCount.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(PlatformVersion)))
            {
                builder.AppendLine("PlatformVersion:" + PlatformVersion.ToString());
            }

            return builder.ToString();
        }

    }
}

