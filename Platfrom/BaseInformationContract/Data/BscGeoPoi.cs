using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
	///<summary>
	///POI
	///</summary>
	[DataContract]
	public class BscGeoPoi
	{
        int _id;
		///<summary>
		///主键
		///</summary>
		[DataMember]
        public int ID
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
		///地理位置名称
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

        double _longitude;
		///<summary>
		///经度
		///</summary>
		[DataMember]
		public double Longitude
		{
			get
			{
				return _longitude;
			}
			set
			{
				 _longitude= value;
			}
		}

        double _latidue;
		///<summary>
		///纬度
		///</summary>
		[DataMember]
        public double Latidue
		{
			get
			{
				return _latidue;
			}
			set
			{
				 _latidue= value;
			}
		}

		string _address;
		///<summary>
		///地址
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

		string _contry;
		///<summary>
		///所属国家
		///</summary>
		[DataMember]
		public string Contry
		{
			get
			{
				return _contry;
			}
			set
			{
				 _contry= value;
			}
		}

        int _property;
		///<summary>
		///所属类型
		///</summary>
		[DataMember]
		public int Property
		{
			get
			{
				return _property;
			}
			set
			{
				 _property= value;
			}
		}

		decimal _datastatus;
		///<summary>
		///数据状态
		///</summary>
		[DataMember]
		public decimal Datastatus
		{
			get
			{
				return _datastatus;
			}
			set
			{
				 _datastatus= value;
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
			if (!string.IsNullOrEmpty(Convert.ToString(Longitude)))
			{
				builder.AppendLine("Longitude:" + Longitude.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Latidue)))
			{
				builder.AppendLine("Latidue:" + Latidue.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Address)))
			{
				builder.AppendLine("Address:" + Address.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Contry)))
			{
				builder.AppendLine("Contry:" + Contry.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Property)))
			{
				builder.AppendLine("Property:" + Property.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Datastatus)))
			{
				builder.AppendLine("Datastatus:" + Datastatus.ToString());
			}

			return builder.ToString();
		}

	}
}

