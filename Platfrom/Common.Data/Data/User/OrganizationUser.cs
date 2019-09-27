using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace Gsafety.PTMS.Message.Contract
{
	///<summary>
	///组织机构用户关系表
	///</summary>
	[DataContract]
	public class OrganizationUser
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

		string _userid;
		///<summary>
		///用户
		///</summary>
		[DataMember]
		public string UserId
		{
			get
			{
				return _userid;
			}
			set
			{
				 _userid= value;
			}
		}

		string _organizationid;
		///<summary>
		///机构
		///</summary>
		[DataMember]
		public string OrganizationId
		{
			get
			{
				return _organizationid;
			}
			set
			{
				 _organizationid= value;
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


		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			if (!string.IsNullOrEmpty(Convert.ToString(ID)))
			{
				builder.AppendLine("ID:" + ID.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(UserId)))
			{
				builder.AppendLine("UserId:" + UserId.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(OrganizationId)))
			{
				builder.AppendLine("OrganizationId:" + OrganizationId.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
			{
				builder.AppendLine("CreateTime:" + CreateTime.ToString());
			}

			return builder.ToString();
		}

	}
}

