using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///警情核查表
    ///</summary>
    [DataContract]
    public class ApealDispose
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

        string _alarmid;
        ///<summary>
        ///一键报警表外键
        ///</summary>
        [DataMember]
        public string AlarmID
        {
            get
            {
                return _alarmid;
            }
            set
            {
                _alarmid = value;
            }
        }

        decimal _alarmflag;
        ///<summary>
        ///警情标识
        //0 未处理
        //1 真警
        //2 假警
        ///</summary>
        [DataMember]
        public decimal AlarmFlag
        {
            get
            {
                return _alarmflag;
            }
            set
            {
                _alarmflag = value;
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

        string _disposestaff;
        ///<summary>
        ///处警人
        ///</summary>
        [DataMember]
        public string DisposeStaff
        {
            get
            {
                return _disposestaff;
            }
            set
            {
                _disposestaff = value;
            }
        }

        DateTime _disposetime;
        ///<summary>
        ///处警时间
        ///</summary>
        [DataMember]
        public DateTime DisposeTime
        {
            get
            {
                return _disposetime;
            }
            set
            {
                _disposetime = value;
            }
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmID)))
            {
                builder.AppendLine("AlarmID:" + AlarmID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmFlag)))
            {
                builder.AppendLine("AlarmFlag:" + AlarmFlag.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Content)))
            {
                builder.AppendLine("Content:" + Content.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DisposeStaff)))
            {
                builder.AppendLine("DisposeStaff:" + DisposeStaff.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DisposeTime)))
            {
                builder.AppendLine("DisposeTime:" + DisposeTime.ToString());
            }

            return builder.ToString();
        }

    }
}

