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
	public class MobileWorking
	{
		string _clientid;
		///<summary>
		///
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

		string _mobilenumber;
		///<summary>
		///
		///</summary>
		[DataMember]
		public string MobileNumber
		{
			get
			{
				return _mobilenumber;
			}
			set
			{
				 _mobilenumber= value;
			}
		}

		decimal _onlineflag;
		///<summary>
		///
		///</summary>
		[DataMember]
		public decimal OnlineFlag
		{
			get
			{
				return _onlineflag;
			}
			set
			{
				 _onlineflag= value;
			}
		}

		string _organizationid;
		///<summary>
		///
		///</summary>
		[DataMember]
		public string OrganizationID
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

		DateTime _switchtime;
		///<summary>
		///
		///</summary>
		[DataMember]
		public DateTime SwitchTime
		{
			get
			{
				return _switchtime;
			}
			set
			{
				 _switchtime= value;
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


		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			if (!string.IsNullOrEmpty(Convert.ToString(ClientID)))
			{
				builder.AppendLine("ClientID:" + ClientID.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(MobileNumber)))
			{
				builder.AppendLine("MobileNumber:" + MobileNumber.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(OnlineFlag)))
			{
				builder.AppendLine("OnlineFlag:" + OnlineFlag.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(OrganizationID)))
			{
				builder.AppendLine("OrganizationID:" + OrganizationID.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(SwitchTime)))
			{
				builder.AppendLine("SwitchTime:" + SwitchTime.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
			{
				builder.AppendLine("VehicleID:" + VehicleID.ToString());
			}

			return builder.ToString();
		}

	}
}

