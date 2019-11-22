using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///消息车辆
    ///</summary>
    [DataContract]
    public class MdvrMessageVehicle
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

        string _messageid;
        ///<summary>
        ///消息
        ///</summary>
        [DataMember]
        public string MessageId
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

        DateTime _sendtime;
        ///<summary>
        ///下发时间
        ///</summary>
        [DataMember]
        public DateTime SendTime
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

        int _status;
        ///<summary>
        ///状态
        //0 等待下发
        //1 下发中
        //2 下发失败
        //3 下发成功
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
        ///状态
        //0 等待下发
        //1 下发中
        //2 下发失败
        //3 下发成功
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


        string _organization;

        [DataMember]
        public string Organization
        {
            get
            {
                return _organization;
            }
            set
            {
                _organization = value;
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

        string _content;
        ///<summary>
        ///内容
        ///</summary>
        [DataMember]
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
            }
        }

        int _messagetype;
        ///<summary>
        ///类型
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

        string _mdvrcoresn;
        ///<summary>
        ///内容
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

        bool _isChecked;
        ///<summary>
        ///内容
        ///</summary>
        [DataMember]
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;
            }
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MessageId)))
            {
                builder.AppendLine("MessageId:" + MessageId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SendTime)))
            {
                builder.AppendLine("SendTime:" + SendTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }

            return builder.ToString();
        }

    }
}

