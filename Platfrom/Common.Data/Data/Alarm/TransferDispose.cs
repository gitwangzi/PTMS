using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///警情处理表
    ///</summary>
    [DataContract]
    public class TransferDispose
    {
        string _id = string.Empty;
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

        int? _forwardedflag;
        ///<summary>
        ///0 未转警
        //1 转警中
        //2 已转警
        //3 转警失败
        ///</summary>
        [DataMember]
        public int? ForwardedFlag
        {
            get
            {
                return _forwardedflag;
            }
            set
            {
                _forwardedflag = value;
            }
        }

        int? _alarmflag;
        ///<summary>
        ///警情真假标识
        ///</summary>
        [DataMember]
        public int? AlarmFlag
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

        DateTime? _disposetime;
        ///<summary>
        ///ＶＥＮ９１１处理时间
        ///</summary>
        [DataMember]
        public DateTime? DisposeTime
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

        string _disposestaff;
        ///<summary>
        ///ＶＥＮ９１１警情处理人
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

        string _transfercenter;
        ///<summary>
        ///中心编号
        ///</summary>
        [DataMember]
        public string TransferCenter
        {
            get
            {
                return _transfercenter;
            }
            set
            {
                _transfercenter = value;
            }
        }

        string _incidentid;
        ///<summary>
        ///VEN911警情ID
        ///</summary>
        [DataMember]
        public string IncidentID
        {
            get
            {
                return _incidentid;
            }
            set
            {
                _incidentid = value;
            }
        }

        string _forwarddest;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string ForwardDest
        {
            get
            {
                return _forwarddest;
            }
            set
            {
                _forwarddest = value;
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

        DateTime? _forwardtime;
        ///<summary>
        ///转发时间
        ///</summary>
        [DataMember]
        public DateTime? ForwardTime
        {
            get
            {
                return _forwardtime;
            }
            set
            {
                _forwardtime = value;
            }
        }

        string _alarmaddress;
        ///<summary>
        ///事发地点
        ///</summary>
        [DataMember]
        public string AlarmAddress
        {
            get
            {
                return _alarmaddress;
            }
            set
            {
                _alarmaddress = value;
            }
        }

        string _incidenttype;
        ///<summary>
        ///警情类型
        ///</summary>
        [DataMember]
        public string IncidentType
        {
            get
            {
                return _incidenttype;
            }
            set
            {
                _incidenttype = value;
            }
        }

        string _apprealstaff;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string ApprealStaff
        {
            get
            {
                return _apprealstaff;
            }
            set
            {
                _apprealstaff = value;
            }
        }

        string _disposecontent;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string DisposeContent
        {
            get
            {
                return _disposecontent;
            }
            set
            {
                _disposecontent = value;
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

        bool _istransfer;
        ///<summary>
        ///是否转发
        ///</summary>
        [DataMember]
        public bool IsTransfer
        {
            get
            {
                return _istransfer;
            }
            set
            {
                _istransfer = value;
            }
        }

        int _transfermode;
        ///<summary>
        ///是否转发
        ///</summary>
        [DataMember]
        public int TransferMode
        {
            get
            {
                return _transfermode;
            }
            set
            {
                _transfermode = value;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmID)))
            {
                builder.AppendLine("AlarmID:" + AlarmID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ForwardedFlag)))
            {
                builder.AppendLine("ForwardedFlag:" + ForwardedFlag.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmFlag)))
            {
                builder.AppendLine("AlarmFlag:" + AlarmFlag.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DisposeTime)))
            {
                builder.AppendLine("DisposeTime:" + DisposeTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DisposeStaff)))
            {
                builder.AppendLine("DisposeStaff:" + DisposeStaff.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(TransferCenter)))
            {
                builder.AppendLine("TransferCenter:" + TransferCenter.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IncidentID)))
            {
                builder.AppendLine("IncidentID:" + IncidentID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ForwardDest)))
            {
                builder.AppendLine("ForwardDest:" + ForwardDest.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Content)))
            {
                builder.AppendLine("Content:" + Content.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ForwardTime)))
            {
                builder.AppendLine("ForwardTime:" + ForwardTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmAddress)))
            {
                builder.AppendLine("AlarmAddress:" + AlarmAddress.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IncidentType)))
            {
                builder.AppendLine("IncidentType:" + IncidentType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ApprealStaff)))
            {
                builder.AppendLine("ApprealStaff:" + ApprealStaff.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DisposeContent)))
            {
                builder.AppendLine("DisposeContent:" + DisposeContent.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("InsertTime:" + CreateTime.ToString());
            }

            return builder.ToString();
        }

    }
}

