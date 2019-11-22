using System;
using System.Runtime.Serialization;
using System.Text;
namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///
    ///</summary>
    [DataContract]
    public class LogOperate
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

        string _operatecontent;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string OperateContent
        {
            get
            {
                return _operatecontent;
            }
            set
            {
                _operatecontent = value;
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

        int _operatetype;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public int OperateType
        {
            get
            {
                return _operatetype;
            }
            set
            {
                _operatetype = value;
            }
        }

        string _showRoleName;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string ShowRoleName
        {
            get
            {
                return _showRoleName;
            }
            set
            {
                _showRoleName = value;
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

        [DataMember]
        public string LoginName { get; set; }



        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if(!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if(!string.IsNullOrEmpty(Convert.ToString(ClientID)))
            {
                builder.AppendLine("ClientID:" + ClientID.ToString());
            }
            if(!string.IsNullOrEmpty(Convert.ToString(OperateContent)))
            {
                builder.AppendLine("OperateContent:" + OperateContent.ToString());
            }
            if(!string.IsNullOrEmpty(Convert.ToString(OperateTime)))
            {
                builder.AppendLine("OperateTime:" + OperateTime.ToString());
            }
            if(!string.IsNullOrEmpty(Convert.ToString(OperateType)))
            {
                builder.AppendLine("OperateType:" + OperateType.ToString());
            }
            if(!string.IsNullOrEmpty(Convert.ToString(OperatorID)))
            {
                builder.AppendLine("OperatorID:" + OperatorID.ToString());
            }
            if(!string.IsNullOrEmpty(Convert.ToString(OperatorName)))
            {
                builder.AppendLine("OperatorName:" + OperatorName.ToString());
            }

            return builder.ToString();
        }

    }
}

