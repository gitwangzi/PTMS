using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
using Gsafety.PTMS.Common.Data.Enum;
namespace Gsafety.PTMS.PublicService.Contract
{
    ///<summary>
    ///LED消息
    ///</summary>
    [DataContract]
    public class RunMdvrMessage
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
        public string ClientId
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

        DateTime _createtime;
        ///<summary>
        ///创建人
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

        string _messagetitle;
        ///<summary>
        ///内容
        ///</summary>
        [DataMember]
        public string MessageTitle
        {
            get
            {
                return _messagetitle;
            }
            set
            {
                _messagetitle = value;
            }
        }

        string _creator;
        ///<summary>
        ///内容
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


        string _title;
        ///<summary>
        ///内容
        ///</summary>
        [DataMember]
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ClientId)))
            {
                builder.AppendLine("ClientId:" + ClientId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Content)))
            {
                builder.AppendLine("Content:" + Content.ToString());
            }

            if (!string.IsNullOrEmpty(Convert.ToString(MessageType)))
            {
                builder.AppendLine("MessageType:" + MessageType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }

            return builder.ToString();
        }

    }
}

