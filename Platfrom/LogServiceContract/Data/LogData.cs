using System;
using System.Runtime.Serialization;
using System.Text;
namespace LogServiceContract.Data
{
    ///<summary>
    ///视频日志
    ///</summary>
    [DataContract]
    public class LogData
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

        string _clientid;
        ///<summary>
        ///客户账号
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

        string _username;
        ///<summary>
        ///用户名
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

        string _msgid;
        ///<summary>
        ///消息ID
        ///</summary>
        [DataMember]
        public string MsgID
        {
            get
            {
                return _msgid;
            }
            set
            {
                _msgid = value;
            }
        }

        string _usertype;
        ///<summary>
        ///用户类型
        ///</summary>
        [DataMember]
        public string UserType
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

        decimal _contenttype;
        ///<summary>
        ///内容类型
        ///</summary>
        [DataMember]
        public decimal Contenttype
        {
            get
            {
                return _contenttype;
            }
            set
            {
                _contenttype = value;
            }
        }

        string _userdept;
        ///<summary>
        ///用户部门
        ///</summary>
        [DataMember]
        public string UserDept
        {
            get
            {
                return _userdept;
            }
            set
            {
                _userdept = value;
            }
        }

        string _mdvrcoresn;
        ///<summary>
        ///芯片号
        ///</summary>
        [DataMember]
        public string MdvrCoreSn
        {
            get
            {
                return _mdvrcoresn;
            }
            set
            {
                _mdvrcoresn = value;
            }
        }

        DateTime _accesstime;
        ///<summary>
        ///时间
        ///</summary>
        [DataMember]
        public DateTime AccessTime
        {
            get
            {
                return _accesstime;
            }
            set
            {
                _accesstime = value;
            }
        }

        string _vehicleid;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string VehicleId
        {
            get
            {
                return _vehicleid;
            }
            set
            {
                _vehicleid = value;
            }
        }

        string _channel;
        ///<summary>
        ///通道号
        ///</summary>
        [DataMember]
        public string Channel
        {
            get
            {
                return _channel;
            }
            set
            {
                _channel = value;
            }
        }

        DateTime _starttime;
        ///<summary>
        ///起始时间
        ///</summary>
        [DataMember]
        public DateTime StartTime
        {
            get
            {
                return _starttime;
            }
            set
            {
                _starttime = value;
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

        string _filename;
        ///<summary>
        ///文件或URL
        ///</summary>
        [DataMember]
        public string FileName
        {
            get
            {
                return _filename;
            }
            set
            {
                _filename = value;
            }
        }

        string _extened1;
        ///<summary>
        ///扩展字段1
        ///</summary>
        [DataMember]
        public string Extened1
        {
            get
            {
                return _extened1;
            }
            set
            {
                _extened1 = value;
            }
        }

        string _extened2;
        ///<summary>
        ///扩展字段2
        ///</summary>
        [DataMember]
        public string Extened2
        {
            get
            {
                return _extened2;
            }
            set
            {
                _extened2 = value;
            }
        }

        string _extened3;
        ///<summary>
        ///扩展字段３
        ///</summary>
        [DataMember]
        public string Extened3
        {
            get
            {
                return _extened3;
            }
            set
            {
                _extened3 = value;
            }
        }

        DateTime _createtime;
        ///<summary>
        ///创建时间
        ///</summary>
        [DataMember]
        public DateTime CreateTime
        {
            get
            {
                return _createtime;
            }
            set
            {
                _createtime = value;
            }
        }

        string _organization_ID;
        ///<summary>
        ///扩展字段３
        ///</summary>
        [DataMember]
        public string Organization_ID
        {
            get
            {
                return _organization_ID;
            }
            set
            {
                _organization_ID = value;
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
            if (!string.IsNullOrEmpty(Convert.ToString(UserName)))
            {
                builder.AppendLine("UserName:" + UserName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MsgID)))
            {
                builder.AppendLine("MsgID:" + MsgID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserType)))
            {
                builder.AppendLine("UserType:" + UserType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Contenttype)))
            {
                builder.AppendLine("Contenttype:" + Contenttype.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserDept)))
            {
                builder.AppendLine("UserDept:" + UserDept.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreSn)))
            {
                builder.AppendLine("MdvrCoreSn:" + MdvrCoreSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AccessTime)))
            {
                builder.AppendLine("AccessTime:" + AccessTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Channel)))
            {
                builder.AppendLine("Channel:" + Channel.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StartTime)))
            {
                builder.AppendLine("StartTime:" + StartTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(EndTime)))
            {
                builder.AppendLine("EndTime:" + EndTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FileName)))
            {
                builder.AppendLine("FileName:" + FileName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Extened1)))
            {
                builder.AppendLine("Extened1:" + Extened1.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Extened2)))
            {
                builder.AppendLine("Extened2:" + Extened2.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Extened3)))
            {
                builder.AppendLine("Extened3:" + Extened3.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Organization_ID)))
            {
                builder.AppendLine("Organization_ID:" + Organization_ID.ToString());
            }
            return builder.ToString();
        }

    }
}

