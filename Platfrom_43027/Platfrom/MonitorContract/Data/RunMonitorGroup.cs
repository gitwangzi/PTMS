using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
namespace Gsafety.PTMS.Monitor.Contract.Data
{
    ///<summary>
    ///分组
    ///</summary>
    [DataContract]
    public class RunMonitorGroup
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

        string _aduser;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string AdUser
        {
            get
            {
                return _aduser;
            }
            set
            {
                _aduser = value;
            }
        }

        string _groupname;
        ///<summary>
        ///名称
        ///</summary>
        [DataMember]
        public string GroupName
        {
            get
            {
                return _groupname;
            }
            set
            {
                _groupname = value;
            }
        }

        decimal _grouptype;
        ///<summary>
        ///1 实时监控分组
        //2 报警分组
        //3 告警分组
        ///</summary>
        [DataMember]
        public decimal GroupType
        {
            get
            {
                return _grouptype;
            }
            set
            {
                _grouptype = value;
            }
        }

        decimal _isdefault;
        ///<summary>
        ///是否默认
        ///</summary>
        [DataMember]
        public decimal IsDefault
        {
            get
            {
                return _isdefault;
            }
            set
            {
                _isdefault = value;
            }
        }

        decimal _groupindex;
        ///<summary>
        ///排序
        ///</summary>
        [DataMember]
        public decimal GroupIndex
        {
            get
            {
                return _groupindex;
            }
            set
            {
                _groupindex = value;
            }
        }

        string _note;
        ///<summary>
        ///备注
        ///</summary>
        [DataMember]
        public string Note
        {
            get
            {
                return _note;
            }
            set
            {
                _note = value;
            }
        }

        List<RunMonitorGroupVehicle> groupVehicle;
        [DataMember]
        public List<RunMonitorGroupVehicle> GroupVehicle
        {
            get
            {
                return groupVehicle;
            }
            set
            {
                groupVehicle = value;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AdUser)))
            {
                builder.AppendLine("AdUser:" + AdUser.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GroupName)))
            {
                builder.AppendLine("GroupName:" + GroupName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GroupType)))
            {
                builder.AppendLine("GroupType:" + GroupType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IsDefault)))
            {
                builder.AppendLine("IsDefault:" + IsDefault.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GroupIndex)))
            {
                builder.AppendLine("GroupIndex:" + GroupIndex.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Note)))
            {
                builder.AppendLine("Note:" + Note.ToString());
            }

            return builder.ToString();
        }

    }
}

