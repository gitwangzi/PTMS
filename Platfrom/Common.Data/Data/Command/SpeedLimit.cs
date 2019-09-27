using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///超速表
    ///</summary>
    [DataContract]
    public class SpeedLimit
    {
        string _id;
        ///<summary>
        ///唯一标识
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

        decimal _maxspeed;
        ///<summary>
        ///最大速度（km/h）
        ///</summary>
        [DataMember]
        public decimal MaxSpeed
        {
            get
            {
                return _maxspeed;
            }
            set
            {
                _maxspeed = value;
            }
        }

        decimal _duration;
        ///<summary>
        ///持续时间（单位秒）
        ///</summary>
        [DataMember]
        public decimal Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
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

        DateTime _createtime;
        ///<summary>
        ///入库时间
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

        bool _valid;
        ///<summary>
        ///是否有效
        //1 有效
        //0 无效
        ///</summary>
        [DataMember]
        public bool Valid
        {
            get
            {
                return _valid;
            }
            set
            {
                _valid = value;
            }
        }

        bool _isVisible;
        [DataMember]
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
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
            if (!string.IsNullOrEmpty(Convert.ToString(Name)))
            {
                builder.AppendLine("Name:" + Name.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MaxSpeed)))
            {
                builder.AppendLine("MaxSpeed:" + MaxSpeed.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Duration)))
            {
                builder.AppendLine("Duration:" + Duration.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Creator)))
            {
                builder.AppendLine("Creator:" + Creator.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Valid)))
            {
                builder.AppendLine("Valid:" + Valid.ToString());
            }

            return builder.ToString();
        }

    }
}

