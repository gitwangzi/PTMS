/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ebe8e57b-7f1d-449c-b241-a8ae8d3d0a45      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract.Data
/////    Project Description:    
/////             Class Name: AlarmInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 11:07:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/24 11:07:41
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.Common.Util;

namespace Gsafety.PTMS.Alarm.Contract.Data
{
    /// <summary>
    /// V61
    /// </summary>
    [DataContract]
    [Serializable]
    public class AlarmInfo
    {
        /// <summary>
        /// alarm Id（key）
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// alarm Id（key）
        /// </summary>
        [DataMember]
        public string ECUId { get; set; }

        /// <summary>
        /// car NO
        /// </summary>
        [DataMember]
        public string VehicleId { get; set; }

        [DataMember]
        public Nullable<DateTime> AlarmTime { get; set; }

        [DataMember]
        public string ProvinceName { get; set; }

        [DataMember]
        public string CityName { get; set; }

        /// <summary>
        /// regioncode
        /// </summary>
        [DataMember]
        public string DistrictCode { get; set; }

        [DataMember]
        public string SuiteInfoID { get; set; }

        public int SuiteStatus { get; set; }

        [DataMember]
        public int BusinessType { get; set; }

        [DataMember]
        public string SuiteID { get; set; }

        [DataMember]
        public string MdvrCoreId { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Speed { get; set; }

        [DataMember]
        public string Direction { get; set; }

        [DataMember]
        public Nullable<DateTime> GpsTime { get; set; }

        [DataMember]
        public string GpsValid { get; set; }

        [DataMember]
        public int AlarmStatus { get; set; }

        [DataMember]
        public short? IsTrueAlarm { get; set; }

        [DataMember]
        public DateTime HandleTime { get; set; }

        [DataMember]
        public int HandleResult { get; set; }

        /// <summary>
        /// UID
        /// </summary>
        [DataMember]
        public string AlarmGuid { get; set; }

        [DataMember]
        public string Context { get; set; }

        [DataMember]
        public int VehicleType { get; set; }

        [DataMember]
        public string VehicleOwner { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public int ButtonNum { get; set; }

        [DataMember]
        public bool EnableVisible { get; set; }

        [DataMember]
        public short Source { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Id)))
            {
                builder.AppendLine("Id:" + Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ECUId)))
            {
                builder.AppendLine("ECUId:" + ECUId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmTime)))
            {
                builder.AppendLine("AlarmTime:" + AlarmTime.ToString());
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
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteInfoID)))
            {
                builder.AppendLine("SuiteInfoID:" + SuiteInfoID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteStatus)))
            {
                builder.AppendLine("SuiteStatus:" + SuiteStatus.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(BusinessType)))
            {
                builder.AppendLine("BusinessType:" + BusinessType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteID)))
            {
                builder.AppendLine("SuiteID:" + SuiteID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(BusinessType)))
            {
                builder.AppendLine("BusinessType:" + BusinessType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Longitude)))
            {
                builder.AppendLine("Longitude:" + Longitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Latitude)))
            {
                builder.AppendLine("Latitude:" + Latitude.ToString());
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
            if (!string.IsNullOrEmpty(Convert.ToString(GpsValid)))
            {
                builder.AppendLine("GpsValid:" + GpsValid.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmStatus)))
            {
                builder.AppendLine("AlarmStatus:" + AlarmStatus.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IsTrueAlarm)))
            {
                builder.AppendLine("IsTrueAlarm:" + IsTrueAlarm.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(HandleTime)))
            {
                builder.AppendLine("HandleTime:" + HandleTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(HandleResult)))
            {
                builder.AppendLine("HandleResult:" + HandleResult.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmGuid)))
            {
                builder.AppendLine("AlarmGuid:" + AlarmGuid.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Context)))
            {
                builder.AppendLine("Context:" + Context.ToString());
            }
            
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleType)))
            {
                builder.AppendLine("VehicleType:" + VehicleType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleOwner)))
            {
                builder.AppendLine("VehicleOwner:" + VehicleOwner.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Phone)))
            {
                builder.AppendLine("Phone:" + Phone.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ButtonNum)))
            {
                builder.AppendLine("ButtonNum:" + ButtonNum.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(EnableVisible)))
            {
                builder.AppendLine("EnableVisible:" + EnableVisible.ToString());
            }
            return builder.ToString();
        }
    }
}
