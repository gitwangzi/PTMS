/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 36ffb084-3524-4186-abf6-89e47a8f5184      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-GUOH
/////                 Author: TEST(guoh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Base.Contract.Data
/////    Project Description:    
/////             Class Name: AlertBaseModel
/////          Class Version: v1.0.0.0
/////            Create Time: 8/23/2013 3:32:13 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/23/2013 3:32:13 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Base.Contract.Data
{
    /// <summary>
    ///  Alarm Basic Class
    /// </summary>
    [Serializable]
    [DataContract]
    public class AlertBaseModel
    {

        /// <summary>
        /// ID(primary key)
        /// </summary>
        [DataMember]
        public string ID { get; set; }

        /// <summary>
        /// MDVR Chip Number
        /// </summary>
        [DataMember]
        public string MdvrCoreSN { get; set; }

        /// <summary>
        /// Security Suite Number
        /// </summary>
        [DataMember]
        public string SuitInfoID { get; set; }

        /// <summary>
        /// Vehicle ID
        /// </summary>
        [DataMember]
        public string VehicleID { get; set; }

        /// <summary>
        /// Security Suite State(init=1,test=2,running=3,error=4,repair=5,damage=6,working=20)
        /// </summary>
        [DataMember]
        public Int16 SuiteStatus { get; set; }

        /// <summary>
        /// Alarm Type
        /// Alarm Type , including device alarm and business alarm
        /// device alarm is:
        ///        Temperature Alarm=11;
        ///        GPS Receiver Error Alarm=12;
        ///        Camera Block Alarm=13;
        ///        Camera No Signal Alarm=14;
        ///        Fire Alarm=15;
        ///        MDVR Card Error Alarm=16;
        ///        Three Times Password Error=17;
        ///        Abnormal Voltage Alarm=18;
        ///        device 72 Hours Offline Alarm=21;
        ///        Accidental Damage Alarm=21
        ///    Business Alarm Type is:
        ///        In Electronic Fence Alarm=11;
        ///        Out Electronic Fence Alarm =12;
        ///        Common Overspeed Alarm=13;
        ///        Common Lowspeed Alarm=14;
        ///        In Fence Overspeed Alarm=15;
        ///        In Fence Lowspeed Alarm=16;
        ///        Out of Orbit Alarm=17;
        ///        Exception of Door Alarm=18;
        ///        Mileage Alarm=21;
        ///        In Electronic Fence Alarm(Platform)=31;
        ///        Out Electronic Fence Alarm(Platform)=32;
        ///        OverSpeed (Platform)=33;
        ///        Speed Recovery(Platform)=34;
        ///        Out of Orbit(Platform)=35;
        ///        In Orbit(Platform)=36;
        ///        OverSpeed In Fence(Platform)=37;
        ///        OverSpeed to Common In Fence(Platform)=38;
        ///        LowSpeed In Fence(Platform)=39;
        ///        LowSpeed to Common In Fence(Platform)=40;
        /// </summary>
        [DataMember]
        public Int16 AlertType { get; set; }

        /// <summary>
        /// Alarm time(format is yyMMdd HHmmss)
        /// </summary>
        [DataMember]
        public DateTime AlertTime { get; set; }

        /// <summary>
        /// Order ID:
        /// 1.Camera No Signal Alarm: V63
        /// 2.Camera Block Alarm: V64
        /// 3.Temperature Alarm: V68
        /// 4.MDVR Card Error Alarm:V69
        /// 5.OverSpeed Alarm:V70
        /// 6.GPS Receiver Error Alarm:V75
        /// 7.Abnormal Voltage Alarm:V78
        /// 8.District Alarm:V79
        /// 9.Fire Alarm:V82
        /// 10.Exception of Door :V83
        /// </summary>
        [DataMember]
        public string Cmd { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        [DataMember]
        public string Longitude { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        [DataMember]
        public string Latitude { get; set; }

        /// <summary>
        /// GPS date(format is yyMMdd HHmmss)
        /// </summary>
        [DataMember]
        public DateTime? GpsTime { get; set; }

        /// <summary>
        /// Vehicle driving speed(km/h)
        /// </summary>
        [DataMember]
        public string Speed { get; set; }

        /// <summary>
        /// Vehicle driving direction(degree)
        /// </summary>
        [DataMember]
        public string Direction { get; set; }

        /// <summary>
        /// GPS availability('A': available, 'V': invailable)
        /// </summary>
        [DataMember]
        public string GpsValid { get; set; }

        /// <summary>
        /// Create Time
        /// </summary>
        [DataMember]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Alarm Completion State('1': init state, '2': checked, '3': operation, '4': finish)
        /// </summary>
        [DataMember]
        public Int16 Status { get; set; }

        /// <summary>
        /// Original Alarm Content of device
        /// </summary>
        [DataMember]
        public string Context { get; set; }

        /// <summary>
        /// Province
        /// </summary>
        [DataMember]
        public string ProvinceName { get; set; }

        /// <summary>
        /// City
        /// </summary>
        [DataMember]
        public string CityName { get; set; }

        /// <summary>
        /// District
        /// </summary>
        [DataMember]
        public string DistrictCode { get; set; }

        /// <summary>
        /// Security Suite ID
        /// </summary>
        [DataMember]
        public string SuiteID { get; set; }

        /// <summary>
        /// Vehicle Type
        /// </summary>
        [DataMember]
        public int VehicleType { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreSN)))
            {
                builder.AppendLine("MdvrCoreSN:" + MdvrCoreSN.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuitInfoID)))
            {
                builder.AppendLine("SuitInfoID:" + SuitInfoID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
            {
                builder.AppendLine("VehicleID:" + VehicleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteStatus)))
            {
                builder.AppendLine("SuiteStatus:" + SuiteStatus.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlertType)))
            {
                builder.AppendLine("AlertType:" + AlertType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlertTime)))
            {
                builder.AppendLine("AlertTime:" + AlertTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Cmd)))
            {
                builder.AppendLine("Cmd:" + Cmd.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Longitude)))
            {
                builder.AppendLine("Longitude:" + Longitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Latitude)))
            {
                builder.AppendLine("Latitude:" + Latitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GpsTime)))
            {
                builder.AppendLine("GpsTime:" + GpsTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Speed)))
            {
                builder.AppendLine("Speed:" + Speed.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Direction)))
            {
                builder.AppendLine("Direction:" + Direction.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GpsValid)))
            {
                builder.AppendLine("GpsValid:" + GpsValid.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Context)))
            {
                builder.AppendLine("Context:" + Context.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ProvinceName)))
            {
                builder.AppendLine("ProvinceName:" + ProvinceName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CityName)))
            {
                builder.AppendLine("CityName:" + CityName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DistrictCode)))
            {
                builder.AppendLine("DistrictCode:" + DistrictCode.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteID)))
            {
                builder.AppendLine("SuiteID:" + SuiteID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleType)))
            {
                builder.AppendLine("VehicleType:" + VehicleType.ToString());
            }
            return builder.ToString();
        }

    }
}
