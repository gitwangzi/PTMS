using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Gsafety.PTMS.Manager.Contract.Data
{
    ///<summary>
    ///角色表
    ///</summary>
    [DataContract]
    public class Role
    {
        public Role()
        {
            FuncItems = new List<string>();
        }

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

        string _description;
        ///<summary>
        ///描述
        ///</summary>
        [DataMember]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        bool _ispredefined;
        ///<summary>
        ///是否为预定义
        ///</summary>
        [DataMember]
        public bool IsPredefined
        {
            get
            {
                return _ispredefined;
            }
            set
            {
                _ispredefined = value;
            }
        }

        int _rolecategory;
        ///<summary>
        ///角色类型
        ///</summary>
        [DataMember]
        public int RoleCategory
        {
            get
            {
                return _rolecategory;
            }
            set
            {
                _rolecategory = value;
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

        [DataMember]
        public List<string> FuncItems { get; set; }

        /// <summary>
        /// 是否可编辑
        /// </summary>
        [DataMember]
        public bool Editable { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Name)))
            {
                builder.AppendLine("Name:" + Name.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ClientID)))
            {
                builder.AppendLine("ClientID:" + ClientID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Description)))
            {
                builder.AppendLine("Description:" + Description.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IsPredefined)))
            {
                builder.AppendLine("IsPredefined:" + IsPredefined.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RoleCategory)))
            {
                builder.AppendLine("RoleCategory:" + RoleCategory.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Creator)))
            {
                builder.AppendLine("Creator:" + Creator.ToString());
            }

            return builder.ToString();
        }
    }
}

