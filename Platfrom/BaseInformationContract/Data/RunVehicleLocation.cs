using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
	///<summary>
	///车位位置
	///</summary>
	[DataContract]
	public class RunVehicleLocation
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
		public string ClientId
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

		decimal _source;
		///<summary>
		///来源
		///</summary>
		[DataMember]
		public decimal Source
		{
			get
			{
				return _source;
			}
			set
			{
				 _source= value;
			}
		}

		string _gpsvalid;
		///<summary>
		///GPS是否有效
///V：不一定可靠；
///A:有效可靠；
///N：无GPS模块；
		///</summary>
		[DataMember]
		public string GpsValid
		{
			get
			{
				return _gpsvalid;
			}
			set
			{
				 _gpsvalid= value;
			}
		}

		string _latitude;
		///<summary>
		///纬度
		///</summary>
		[DataMember]
		public string Latitude
		{
			get
			{
				return _latitude;
			}
			set
			{
				 _latitude= value;
			}
		}

		string _longitude;
		///<summary>
		///经度
		///</summary>
		[DataMember]
		public string Longitude
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

		string _speed;
		///<summary>
		///速度
		///</summary>
		[DataMember]
		public string Speed
		{
			get
			{
				return _speed;
			}
			set
			{
				 _speed= value;
			}
		}

		string _direction;
		///<summary>
		///方向
		///</summary>
		[DataMember]
		public string Direction
		{
			get
			{
				return _direction;
			}
			set
			{
				 _direction= value;
			}
		}

		Nullable<System.DateTime> _gpstime;
		///<summary>
		///GPS时间
		///</summary>
		[DataMember]
		public Nullable<System.DateTime> GpsTime
		{
			get
			{
				return _gpstime;
			}
			set
			{
				 _gpstime= value;
			}
		}

		string _districtcode;
		///<summary>
		///行政区域代码
		///</summary>
		[DataMember]
		public string DistrictCode
		{
			get
			{
				return _districtcode;
			}
			set
			{
				 _districtcode= value;
			}
		}

		decimal _alarmflag;
		///<summary>
		///是否为报警
		///</summary>
		[DataMember]
		public decimal AlarmFlag
		{
			get
			{
				return _alarmflag;
			}
			set
			{
				 _alarmflag= value;
			}
		}

		string _vehicleid;
		///<summary>
		///车牌号
		///</summary>
		[DataMember]
		public string VehicleId
		{
			get
			{
				return _vehicleid;
			}
			set
			{
				 _vehicleid= value;
			}
		}

		decimal _statusflag;
		///<summary>
		///
		///</summary>
		[DataMember]
		public decimal StatusFlag
		{
			get
			{
				return _statusflag;
			}
			set
			{
				 _statusflag= value;
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
			if (!string.IsNullOrEmpty(Convert.ToString(Source)))
			{
				builder.AppendLine("Source:" + Source.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(GpsValid)))
			{
				builder.AppendLine("GpsValid:" + GpsValid.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Latitude)))
			{
				builder.AppendLine("Latitude:" + Latitude.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Longitude)))
			{
				builder.AppendLine("Longitude:" + Longitude.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Speed)))
			{
				builder.AppendLine("Speed:" + Speed.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Direction)))
			{
				builder.AppendLine("Direction:" + Direction.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(GpsTime)))
			{
				builder.AppendLine("GpsTime:" + GpsTime.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(DistrictCode)))
			{
				builder.AppendLine("DistrictCode:" + DistrictCode.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(AlarmFlag)))
			{
				builder.AppendLine("AlarmFlag:" + AlarmFlag.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
			{
				builder.AppendLine("VehicleId:" + VehicleId.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(StatusFlag)))
			{
				builder.AppendLine("StatusFlag:" + StatusFlag.ToString());
			}

			return builder.ToString();
		}

	}
}

