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
    public class AppMessageVehicle
    {
        string _id;
        ///<summary>
        ///ID
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

        string _chauffeurid;
        ///<summary>
        ///司机ID
        ///</summary>
        [DataMember]
        public string ChauffeurID
        {
            get
            {
                return _chauffeurid;
            }
            set
            {
                _chauffeurid = value;
            }
        }

        string _chauffeurname;
        ///<summary>
        ///司机名
        ///</summary>
        [DataMember]
        public string ChauffeurName
        {
            get
            {
                return _chauffeurname;
            }
            set
            {
                _chauffeurname = value;
            }
        }

        string _vehicleid;
        ///<summary>
        ///车牌
        ///</summary>
        [DataMember]
        public string VehicleID
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

        string _clientid;
        ///<summary>
        ///客户ID
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

        string _messageid;
        ///<summary>
        ///ID
        ///</summary>
        [DataMember]
        public string MessageID
        {
            get
            {
                return _messageid;
            }
            set
            {
                _messageid = value;
            }
        }

        string _messagetitle;
        ///<summary>
        ///标题
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

        DateTime? _sendtime;
        ///<summary>
        ///发送时间
        ///</summary>
        [DataMember]
        public DateTime? SendTime
        {
            get
            {
                return _sendtime;
            }
            set
            {
                _sendtime = value;
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

        int _status;
        ///<summary>
        ///状态
        ///</summary>
        [DataMember]
        public int Status
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

        string _showState;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string ShowState
        {
            get
            {
                return _showState;
            }
            set
            {
                _showState = value;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ChauffeurID)))
            {
                builder.AppendLine("ChauffeurID:" + ChauffeurID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ChauffeurName)))
            {
                builder.AppendLine("ChauffeurName:" + ChauffeurName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
            {
                builder.AppendLine("VehicleID:" + VehicleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ClientID)))
            {
                builder.AppendLine("ClientID:" + ClientID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MessageID)))
            {
                builder.AppendLine("MessageID:" + MessageID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MessageTitle)))
            {
                builder.AppendLine("MessageTitle:" + MessageTitle.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Message)))
            {
                builder.AppendLine("Message:" + Message.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MessageType)))
            {
                builder.AppendLine("MessageType:" + MessageType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SendTime)))
            {
                builder.AppendLine("SendTime:" + SendTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }

            return builder.ToString();
        }

    }
}

