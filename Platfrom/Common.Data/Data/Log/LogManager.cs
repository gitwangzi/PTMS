using System;
using System.Runtime.Serialization;
using System.Text;
namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///外部访问日志
    ///</summary>
    [DataContract]
    public class LogManager
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
        ///客户账户
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

        string _clientname;
        ///<summary>
        ///客户账户
        ///</summary>
        [DataMember]
        public string ClientName
        {
            get
            {
                return _clientname;
            }
            set
            {
                _clientname = value;
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

        string _managerid;
        ///<summary>
        ///管理员账户名
        ///</summary>
        [DataMember]
        public string ManagerID
        {
            get
            {
                return _managerid;
            }
            set
            {
                _managerid = value;
            }
        }

        string _manager;
        ///<summary>
        ///管理员姓名
        ///</summary>
        [DataMember]
        public string Manager
        {
            get
            {
                return _manager;
            }
            set
            {
                _manager = value;
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
            if (!string.IsNullOrEmpty(Convert.ToString(Content)))
            {
                builder.AppendLine("Content:" + Content.ToString());
            }
           
            if (!string.IsNullOrEmpty(Convert.ToString(ManagerID)))
            {
                builder.AppendLine("ManagerID:" + ManagerID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Manager)))
            {
                builder.AppendLine("Manager:" + Manager.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }

            return builder.ToString();
        }

    }
}

