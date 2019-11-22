using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
	///<summary>
	///行政区域
	///</summary>
	[DataContract]
	public class BscDistrict
	{
		string _code;
		///<summary>
		///行政区划代码
		///</summary>
		[DataMember]
		public string Code
		{
			get
			{
				return _code;
			}
			set
			{
				 _code= value;
			}
		}

		string _name;
		///<summary>
		///行政区划名称
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

		string _fullname;
		///<summary>
		///全称
		///</summary>
		[DataMember]
		public string Fullname
		{
			get
			{
				return _fullname;
			}
			set
			{
				 _fullname= value;
			}
		}

		string _shortname;
		///<summary>
		///缩写
		///</summary>
		[DataMember]
		public string Shortname
		{
			get
			{
				return _shortname;
			}
			set
			{
				 _shortname= value;
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
				 _note= value;
			}
		}

		decimal _valid;
		///<summary>
		///是否有效///有效///无效
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


		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			if (!string.IsNullOrEmpty(Convert.ToString(Code)))
			{
				builder.AppendLine("Code:" + Code.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Name)))
			{
				builder.AppendLine("Name:" + Name.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Fullname)))
			{
				builder.AppendLine("Fullname:" + Fullname.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Shortname)))
			{
				builder.AppendLine("Shortname:" + Shortname.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Note)))
			{
				builder.AppendLine("Note:" + Note.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Valid)))
			{
				builder.AppendLine("Valid:" + Valid.ToString());
			}

			return builder.ToString();
		}

	}
}

