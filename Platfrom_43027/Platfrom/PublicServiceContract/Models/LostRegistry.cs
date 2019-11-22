using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace Gsafety.PTMS.PublicService.Contract
{
	///<summary>
	///丢失登记
	///</summary>
	[DataContract]
	public class LostRegistry
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

		string _lostname;
		///<summary>
		///丢失人
		///</summary>
		[DataMember]
		public string LostName
		{
			get
			{
				return _lostname;
			}
			set
			{
				 _lostname= value;
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
				 _content= value;
			}
		}

		string _keyword;
		///<summary>
		///关键字
		///</summary>
		[DataMember]
		public string Keyword
		{
			get
			{
				return _keyword;
			}
			set
			{
				 _keyword= value;
			}
		}

		string _lostidcard;
		///<summary>
		///丢失人ID
		///</summary>
		[DataMember]
		public string LostIdcard
		{
			get
			{
				return _lostidcard;
			}
			set
			{
				 _lostidcard= value;
			}
		}

		string _lostphone;
		///<summary>
		///电话
		///</summary>
		[DataMember]
		public string LostPhone
		{
			get
			{
				return _lostphone;
			}
			set
			{
				 _lostphone= value;
			}
		}

		string _address;
		///<summary>
		///地点
		///</summary>
		[DataMember]
		public string Address
		{
			get
			{
				return _address;
			}
			set
			{
				 _address= value;
			}
		}

		DateTime _losttime;
		///<summary>
		///丢失时间
		///</summary>
		[DataMember]
		public DateTime LostTime
		{
			get
			{
				return _losttime;
			}
			set
			{
				 _losttime= value;
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
			if (!string.IsNullOrEmpty(Convert.ToString(ClientID)))
			{
				builder.AppendLine("ClientID:" + ClientID.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(LostName)))
			{
				builder.AppendLine("LostName:" + LostName.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Content)))
			{
				builder.AppendLine("Content:" + Content.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Keyword)))
			{
				builder.AppendLine("Keyword:" + Keyword.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(LostIdcard)))
			{
				builder.AppendLine("LostIdcard:" + LostIdcard.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(LostPhone)))
			{
				builder.AppendLine("LostPhone:" + LostPhone.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Address)))
			{
				builder.AppendLine("Address:" + Address.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(LostTime)))
			{
				builder.AppendLine("LostTime:" + LostTime.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
			{
				builder.AppendLine("CreateTime:" + CreateTime.ToString());
			}

			return builder.ToString();
		}

	}
}

