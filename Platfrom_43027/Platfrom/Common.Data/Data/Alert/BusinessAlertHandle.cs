using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;

namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///已处理业务类告警
    ///</summary>
    [DataContract]
    public class BusinessAlertHandle
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

        string _businessalertid;
        ///<summary>
        ///告警ＩＤ
        ///</summary>
        [DataMember]
        public string BusinessAlertID
        {
            get
            {
                return _businessalertid;
            }
            set
            {
                _businessalertid = value;
            }
        }

        string _handleuser;
        ///<summary>
        ///处理人
        ///</summary>
        [DataMember]
        public string HandleUser
        {
            get
            {
                return _handleuser;
            }
            set
            {
                _handleuser = value;
            }
        }

        string _content;
        ///<summary>
        ///备注
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

        DateTime _handletime;
        ///<summary>
        ///处理时间
        ///</summary>
        [DataMember]
        public DateTime HandleTime
        {
            get
            {
                return _handletime;
            }
            set
            {
                _handletime = value;
            }
        }

        int? _alertLevel;
        [DataMember]
        public int? AlertLevel
        {
            get
            {
                return _alertLevel;
            }
            set
            {
                _alertLevel = value;
            }
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(BusinessAlertID)))
            {
                builder.AppendLine("BusinessAlertID:" + BusinessAlertID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(HandleUser)))
            {
                builder.AppendLine("HandleUser:" + HandleUser.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Content)))
            {
                builder.AppendLine("Content:" + Content.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(HandleTime)))
            {
                builder.AppendLine("HandleTime:" + HandleTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlertLevel)))
            {
                builder.AppendLine("AlertLevel:" + AlertLevel.ToString());
            }

            return builder.ToString();
        }

    }
}

