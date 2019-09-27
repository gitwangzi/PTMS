/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4a83d554-1221-4355-9a16-e528c77b11e0      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Contract.Data
/////    Project Description:    
/////             Class Name: VehicleDeviceAlertInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/13 9:49:45
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/26 9:49:45
/////            Modified by: BilongLiu
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Alert.Contract.Data
{
    /// <summary>
    /// DeviceAlert Infomation
    /// </summary>
    [DataContract]
    public class DeviceAlert
    {
        /// <summary>
        /// Alert ID（primary KEY）
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string MdvrCoreId { get; set; }
        
        [DataMember]
        public string SuiteId { get; set; }
        
        [DataMember]
        public string SuiteInfoId { get; set; }
        
        [DataMember]
        public string VehicleId { get; set; }
        
        [DataMember]
        public int? SuiteStatus { get; set; }
        
        [DataMember]
        public int? AlertType { get; set; }
        
        [DataMember]
        public string AlertTypeName { get; set; }
        
        [DataMember]
        public DateTime? AlertTime { get; set; }
        
        [DataMember]
        public string Cmd { get; set; }
        
        [DataMember]
        public string Longitude { get; set; }
        
        [DataMember]
        public string Latitude { get; set; }
        
        [DataMember]
        public DateTime? GpsTime { get; set; }
        
        [DataMember]
        public string Speed { get; set; }
        
        [DataMember]
        public string Direction { get; set; }
        
        [DataMember]
        public string GpsValid { get; set; }
        
        [DataMember]
        public string TagValue { get; set; }
        
        [DataMember]
        public int? Status { get; set; }

        /// <summary>
        /// handle foreign key
        /// </summary>
        [DataMember]
        public string HandleId { get; set; }

        [DataMember]
        public string ClientID { get; set; }

        /// <summary>
        /// traffice rule ID
        /// </summary>
        [DataMember]
        public int RuleId { get; set; }


        [DataMember]
        public string CheckID { get; set; }

        [DataMember]
        public string DistrictCode { get; set; }
        
        [DataMember]
        public string AdditionalInfo { get; set; }

        [DataMember]
        public DateTime? CreateTime { get; set; }


        [DataMember]
        public string ShowType { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Id)))
            {
                builder.AppendLine("Id:" + Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteId)))
            {
                builder.AppendLine("SuiteId:" + SuiteId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteInfoId)))
            {
                builder.AppendLine("SuiteInfoId:" + SuiteInfoId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteStatus)))
            {
                builder.AppendLine("SuiteStatus:" + SuiteStatus.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlertType)))
            {
                builder.AppendLine("AlertType:" + AlertType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlertTypeName)))
            {
                builder.AppendLine("AlertTypeName:" + AlertTypeName.ToString());
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
            if (!string.IsNullOrEmpty(Convert.ToString(TagValue)))
            {
                builder.AppendLine("TagValue:" + TagValue.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(HandleId)))
            {
                builder.AppendLine("HandleId:" + HandleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RuleId)))
            {
                builder.AppendLine("RuleId:" + RuleId.ToString());
            }
            return builder.ToString();
        }

    }
}
