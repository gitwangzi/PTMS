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
    public class LogVideo
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

        DateTime _operatetime;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public DateTime OperateTime
        {
            get
            {
                return _operatetime;
            }
            set
            {
                _operatetime = value;
            }
        }

        int _logtype;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public int LogType
        {
            get
            {
                return _logtype;
            }
            set
            {
                _logtype = value;
            }
        }

        string _operatorid;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string OperatorID
        {
            get
            {
                return _operatorid;
            }
            set
            {
                _operatorid = value;
            }
        }

        string _operatorname;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string OperatorName
        {
            get
            {
                return _operatorname;
            }
            set
            {
                _operatorname = value;
            }
        }

        int _channel;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public int Channel
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

        string _vehicleid;
        ///<summary>
        ///
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

        string _mdvrcoresn;
        ///<summary>
        ///
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

        string _suitesn;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string SuiteSn
        {
            get
            {
                return _suitesn;
            }
            set
            {
                _suitesn = value;
            }
        }

        string _content;
        ///<summary>
        ///
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


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OperateTime)))
            {
                builder.AppendLine("OperateTime:" + OperateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(LogType)))
            {
                builder.AppendLine("LogType:" + LogType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OperatorID)))
            {
                builder.AppendLine("OperatorID:" + OperatorID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OperatorName)))
            {
                builder.AppendLine("OperatorName:" + OperatorName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Channel)))
            {
                builder.AppendLine("Channel:" + Channel.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
            {
                builder.AppendLine("VehicleID:" + VehicleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ClientID)))
            {
                builder.AppendLine("ClientID:" + ClientID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreSn)))
            {
                builder.AppendLine("MdvrCoreSn:" + MdvrCoreSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteSn)))
            {
                builder.AppendLine("SuiteSn:" + SuiteSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Content)))
            {
                builder.AppendLine("Content:" + Content.ToString());
            }

            return builder.ToString();
        }

    }
}

