using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
using Gsafety.PTMS.BaseInformation.Contract.Data;
namespace Gsafety.PTMS.BaseInformation.Contract
{
	///<summary>
	///组织机构表
	///</summary>
	[DataContract]
	public class Organization
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
				 _id= value;
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
				 _name= value;
			}
		}

		string _parentid;
		///<summary>
		///父机构
		///</summary>
		[DataMember]
		public string ParentID
		{
			get
			{
				return _parentid;
			}
			set
			{
				 _parentid= value;
			}
		}

		string _contact;
		///<summary>
		///联系人
		///</summary>
		[DataMember]
		public string Contact
		{
			get
			{
				return _contact;
			}
			set
			{
				 _contact= value;
			}
		}

		string _email;
		///<summary>
		///邮箱
		///</summary>
		[DataMember]
		public string Email
		{
			get
			{
				return _email;
			}
			set
			{
				 _email= value;
			}
		}

		string _phone;
		///<summary>
		///电话
		///</summary>
		[DataMember]
		public string Phone
		{
			get
			{
				return _phone;
			}
			set
			{
				 _phone= value;
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
				 _createtime= value;
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
				 _creator= value;
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
				 _clientid= value;
			}
		}

        decimal _valid;
		///<summary>
		///是否有效
        ///1 有效
        ///0 无效
		///</summary>
		[DataMember]
		public decimal Valid
		{
			get
			{
				return _valid;
			}
			set
			{
				 _valid= value;
			}
		}

        bool _isChecked;
        [DataMember]
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;
            }
        }

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
			if (!string.IsNullOrEmpty(Convert.ToString(ParentID)))
			{
				builder.AppendLine("ParentID:" + ParentID.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Contact)))
			{
				builder.AppendLine("Contact:" + Contact.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Email)))
			{
				builder.AppendLine("Email:" + Email.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Phone)))
			{
				builder.AppendLine("Phone:" + Phone.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
			{
				builder.AppendLine("CreateTime:" + CreateTime.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Creator)))
			{
				builder.AppendLine("Creator:" + Creator.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(ClientID)))
			{
				builder.AppendLine("ClientID:" + ClientID.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Valid)))
			{
				builder.AppendLine("Valid:" + Valid.ToString());
			}

			return builder.ToString();
		}

	}
}

