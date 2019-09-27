using Gsafety.PTMS.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class GPS
    {
        [DataMember]
        public string UID { get; set; }

        [DataMember]
        public string Valid { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Height { get; set; }

        [DataMember]
        public string Speed { get; set; }

        [DataMember]
        public string Direction { get; set; }

        DateTime? _gpstime = null;
        [DataMember]
        public DateTime? GpsTime
        {
            get
            {
                return _gpstime;
            }
            set
            {
                _gpstime = value;
            }
        }

        [DataMember]
        public long AlarmFlag { get; set; }

        [DataMember]
        public long Status { get; set; }

        [DataMember]
        public int Source { get; set; }

        [DataMember]
        public string ClientID { get; set; }

        [DataMember]
        public string VehicleId { get; set; }

        [DataMember]
        public string DistrictCode { get; set; }

        [DataMember]
        public string DeviceID { get; set; }

        [DataMember]
        public int SourceMode { get; set; }

        public string InsertSQL()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Insert Into RUN_VEHICLE_LOCATION(ID,CLIENT_ID,VEHICLE_ID,SOURCE,GPS_VALID,LATITUDE,LONGITUDE,SPEED,DIRECTION,GPS_TIME,DISTRICT_CODE,ALARM_FLAG,STATUS_FLAG,DEVICE_ID,SOURCE_MODE)Values(");
            builder.Append("'" + Guid.NewGuid().ToString() + "',");
            builder.Append("'" + ClientID + "',N'");
            builder.Append(VehicleId + "',");
            builder.Append(Source + ",'");
            builder.Append(Valid + "','");
            builder.Append(Latitude + "','");
            builder.Append(Longitude + "','");
            builder.Append(Speed + "','");
            builder.Append(Direction + "','");
            if (GpsTime != null)            
            {

                builder.Append(this.GpsTime.Value.ToString("yyyy-MM-dd HH:mm:ss") + "','");
              
            }
            builder.Append(DistrictCode + "',");
            builder.Append(AlarmFlag + ",");
            builder.Append(Status);
            builder.Append(",'" + DeviceID + "',");
            builder.Append(SourceMode);
            builder.Append(");");

            return builder.ToString();
        }

    }
}
