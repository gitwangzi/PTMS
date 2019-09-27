using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;

namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///拾到物
    ///</summary>
    [DataContract]
    public class FoundRegistry
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

        string _founder;
        ///<summary>
        ///报告人
        ///</summary>
        [DataMember]
        public string Founder
        {
            get
            {
                return _founder;
            }
            set
            {
                _founder = value;
            }
        }

        string _founderidcard;
        ///<summary>
        ///拾获人ID
        ///</summary>
        [DataMember]
        public string FounderIDCard
        {
            get
            {
                return _founderidcard;
            }
            set
            {
                _founderidcard = value;
            }
        }

        string _foundphone;
        ///<summary>
        ///拾获人电话
        ///</summary>
        [DataMember]
        public string FoundPhone
        {
            get
            {
                return _foundphone;
            }
            set
            {
                _foundphone = value;
            }
        }

        DateTime _foundtime;
        ///<summary>
        ///发现时间
        ///</summary>
        [DataMember]
        public DateTime FoundTime
        {
            get
            {
                return _foundtime;
            }
            set
            {
                _foundtime = value;
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

        string _keyword;
        ///<summary>
        ///关键字
        ///</summary>
        [DataMember]
        public string Keyword
        {
            get
            {
                return _keyword;
            }
            set
            {
                _keyword = value;
            }
        }

        string _lostname;
        ///<summary>
        ///丢失人
        ///</summary>
        [DataMember]
        public string LostName
        {
            get
            {
                return _lostname;
            }
            set
            {
                _lostname = value;
            }
        }

        string _lostphone;
        ///<summary>
        ///丢失者电话
        ///</summary>
        [DataMember]
        public string LostPhone
        {
            get
            {
                return _lostphone;
            }
            set
            {
                _lostphone = value;
            }
        }

        string _address;
        ///<summary>
        ///地点
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

        decimal _status;
        ///<summary>
        ///状态
        //0 未解决
        //1 解决
        ///</summary>
        [DataMember]
        public decimal Status
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

        DateTime? _claimtime;
        ///<summary>
        ///领取时间
        ///</summary>
        [DataMember]
        public DateTime? ClaimTime
        {
            get
            {
                return _claimtime;
            }
            set
            {
                _claimtime = value;
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
            if (!string.IsNullOrEmpty(Convert.ToString(Founder)))
            {
                builder.AppendLine("Founder:" + Founder.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FounderIDCard)))
            {
                builder.AppendLine("FounderIDCard:" + FounderIDCard.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FoundPhone)))
            {
                builder.AppendLine("FoundPhone:" + FoundPhone.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FoundTime)))
            {
                builder.AppendLine("FoundTime:" + FoundTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Content)))
            {
                builder.AppendLine("Content:" + Content.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Keyword)))
            {
                builder.AppendLine("Keyword:" + Keyword.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(LostName)))
            {
                builder.AppendLine("LostName:" + LostName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(LostPhone)))
            {
                builder.AppendLine("LostPhone:" + LostPhone.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Address)))
            {
                builder.AppendLine("Address:" + Address.ToString());
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

