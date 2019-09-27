using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace Gsafety.PTMS.PublicService.Contract
{
    ///<summary>
    ///App消息
    ///</summary>
    [DataContract]
    public class RunAppMessage
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
        public string ClientId
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

        string _message;
        ///<summary>
        ///消息
        ///</summary>
        [DataMember]
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
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

        string _vehicleid;
        ///<summary>
        ///车牌号
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

        int _messagetype;
        ///<summary>
        ///消息类型
        ///</summary>
        [DataMember]
        public int MessageType
        {
            get
            {
                return _messagetype;
            }
            set
            {
                _messagetype = value;
            }
        }

        string _messagetitle;
        ///<summary>
        ///消息标题
        ///</summary>
        [DataMember]
        public string MessageTitle
        {
            get
            {
                return _messagetitle;
            }
            set
            {
                _messagetitle = value;
            }
        }

        string _showMsgType;
        ///<summary>
        ///消息标题
        ///</summary>
        [DataMember]
        public string ShowMsgType
        {
            get
            {
                return _showMsgType;
            }
            set
            {
                _showMsgType = value;
            }
        }

        [DataMember]
        public bool CanDelete { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ClientId)))
            {
                builder.AppendLine("ClientId:" + ClientId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Message)))
            {
                builder.AppendLine("Message:" + Message.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MessageType)))
            {
                builder.AppendLine("MessageType:" + MessageType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Creator)))
            {
                builder.AppendLine("Creator:" + Creator.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MessageTitle)))
            {
                builder.AppendLine("MessageTitle:" + MessageTitle.ToString());
            }

            return builder.ToString();
        }

    }
}

