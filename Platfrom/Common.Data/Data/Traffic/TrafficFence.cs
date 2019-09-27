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
	public class TrafficFence
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

		decimal _fencetype;
		///<summary>
		///围栏类型
		///</summary>
		[DataMember]
		public decimal FenceType
		{
			get
			{
				return _fencetype;
			}
			set
			{
				 _fencetype= value;
			}
		}


	

		string _pts;
		///<summary>
		///围栏坐标
		///</summary>
		[DataMember]
		public string Pts
		{
			get
			{
				return _pts;
			}
			set
			{
				 _pts= value;
			}
		}

        int _radius;
		///<summary>
		///半径
		///</summary>
		[DataMember]
		public int Radius
		{
			get
			{
				return _radius;
			}
			set
			{
				 _radius= value;
			}
		}

		string _circlecenter;
		///<summary>
		///监控点
		///</summary>
		[DataMember]
		public string CircleCenter
		{
			get
			{
				return _circlecenter;
			}
			set
			{
				 _circlecenter= value;
			}
		}

		string _shape;
		///<summary>
		///围栏空间点
		///</summary>
		[DataMember]
		public string Shape
		{
			get
			{
				return _shape;
			}
			set
			{
				 _shape= value;
			}
		}

		string _creator;
		///<summary>
		///
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

		DateTime _createtime;
		///<summary>
		///
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

        string _address;
        ///<summary>
        ///
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
                _address = value;
            }
        }

		bool _valid;
		///<summary>
		///
		///</summary>
		[DataMember]
		public bool Valid
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

        private string _start_time;
        [DataMember]
        public string StartTime
        {
            get
            {
                return _start_time;
            }
            set
            {
                _start_time = value;
            }
        }

        private int _over_speed_duration;
        [DataMember]
        public int OverSpeedDuration
        {
            get
            {
                return _over_speed_duration;
            }
            set
            {
                _over_speed_duration = value;
            }
        }

        private int _point_count;
        [DataMember]
        public int PointCount
        {
            get
            {
                return _point_count;
            }
            set
            {
                _point_count = value;
            }
        }

        private string _region_property;
        [DataMember]
        public string RegionProperty
        {
            get
            {
                return _region_property;
            }
            set
            {
                _region_property = value;
            }
        }

        private string _end_time;
        [DataMember]
        public string EndTime
        {
            get
            {
                return _end_time;
            }
            set
            {
                _end_time = value;
            }
        }

        private int _max_speed;
        [DataMember]
        public int MaxSpeed
        {
            get
            {
                return _max_speed;
            }
            set
            {
                _max_speed = value;
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
			if (!string.IsNullOrEmpty(Convert.ToString(Name)))
			{
				builder.AppendLine("Name:" + Name.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(FenceType)))
			{
				builder.AppendLine("FenceType:" + FenceType.ToString());
			}
			
			if (!string.IsNullOrEmpty(Convert.ToString(Pts)))
			{
				builder.AppendLine("Pts:" + Pts.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Radius)))
			{
				builder.AppendLine("Radius:" + Radius.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(CircleCenter)))
			{
				builder.AppendLine("CircleCenter:" + CircleCenter.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Shape)))
			{
				builder.AppendLine("Shape:" + Shape.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Creator)))
			{
				builder.AppendLine("Creator:" + Creator.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
			{
				builder.AppendLine("CreateTime:" + CreateTime.ToString());
			}
            if (!string.IsNullOrEmpty(Convert.ToString(Address)))
            {
                builder.AppendLine("Address:" + Address.ToString());
            }
			if (!string.IsNullOrEmpty(Convert.ToString(Valid)))
			{
				builder.AppendLine("Valid:" + Valid.ToString());
			}

			return builder.ToString();
		}

	}
}

