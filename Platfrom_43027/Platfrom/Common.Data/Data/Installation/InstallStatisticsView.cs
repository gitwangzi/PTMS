using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace Gsafety.PTMS.Common.Data
{
	///<summary>
	///
	///</summary>
	[DataContract]
	public class InstallStatisticsView
	{
		string _devicesn;
		///<summary>
		///
		///</summary>
		[DataMember]
		public string DeviceSn
		{
			get
			{
				return _devicesn;
			}
			set
			{
				 _devicesn= value;
			}
		}

		decimal _devicetype;
		///<summary>
		///
		///</summary>
		[DataMember]
		public decimal DeviceType
		{
			get
			{
				return _devicetype;
			}
			set
			{
				 _devicetype= value;
			}
		}

		string _organizationname;
		///<summary>
		///
		///</summary>
		[DataMember]
		public string OrganizationName
		{
			get
			{
				return _organizationname;
			}
			set
			{
				 _organizationname= value;
			}
		}

		string _orgnizationid;
		///<summary>
		///
		///</summary>
		[DataMember]
		public string OrgnizationID
		{
			get
			{
				return _orgnizationid;
			}
			set
			{
				 _orgnizationid= value;
			}
		}

		DateTime _starttime;
		///<summary>
		///
		///</summary>
		[DataMember]
		public DateTime StartTime
		{
			get
			{
				return _starttime;
			}
			set
			{
				 _starttime= value;
			}
		}

		string _station;
		///<summary>
		///
		///</summary>
		[DataMember]
		public string Station
		{
			get
			{
				return _station;
			}
			set
			{
				 _station= value;
			}
		}

		string _stationname;
		///<summary>
		///
		///</summary>
		[DataMember]
		public string StationName
		{
			get
			{
				return _stationname;
			}
			set
			{
				 _stationname= value;
			}
		}

		decimal _step;
		///<summary>
		///
		///</summary>
		[DataMember]
		public decimal Step
		{
			get
			{
				return _step;
			}
			set
			{
				 _step= value;
			}
		}

		string _vehicleid;
		///<summary>
		///
		///</summary>
		[DataMember]
		public string VehicleID
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

		string _vehicletype;
		///<summary>
		///
		///</summary>
		[DataMember]
		public string VehicleType
		{
			get
			{
				return _vehicletype;
			}
			set
			{
				 _vehicletype= value;
			}
		}

        [DataMember]
        public int Count { get; set; }


		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			if (!string.IsNullOrEmpty(Convert.ToString(DeviceSn)))
			{
				builder.AppendLine("DeviceSn:" + DeviceSn.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(DeviceType)))
			{
				builder.AppendLine("DeviceType:" + DeviceType.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(OrganizationName)))
			{
				builder.AppendLine("OrganizationName:" + OrganizationName.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(OrgnizationID)))
			{
				builder.AppendLine("OrgnizationID:" + OrgnizationID.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(StartTime)))
			{
				builder.AppendLine("StartTime:" + StartTime.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Station)))
			{
				builder.AppendLine("Station:" + Station.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(StationName)))
			{
				builder.AppendLine("StationName:" + StationName.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Step)))
			{
				builder.AppendLine("Step:" + Step.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
			{
				builder.AppendLine("VehicleID:" + VehicleID.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(VehicleType)))
			{
				builder.AppendLine("VehicleType:" + VehicleType.ToString());
			}

			return builder.ToString();
		}

	}
}

