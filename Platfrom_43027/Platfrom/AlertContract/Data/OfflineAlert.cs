/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 01ffacdd-11b2-431b-a612-2fb2b01f2156      
/////             clrversion: 4.0.30319.34011
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Contract.Data
/////    Project Description:    
/////             Class Name: OfflineAlert
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/2/22 04:27:47
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/2/22 04:27:47
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Alert.Contract.Data
{
    public class OfflineAlert
    {
        /// <summary>
        /// ALERT ID（PRIMARY KEY）
        /// </summary>
        public string Id { get; set; }
        
        public string MdvrCoreId { get; set; }
        
        public string SuiteId { get; set; }
        /// <summary>
        /// （PK）
        /// </summary>
        public string SuiteInfoId { get; set; }
        
        public string VehicleId { get; set; }
        
        public short? SuiteStatus { get; set; }
        
        public short? AlertType { get; set; }
        
        public DateTime? AlertTime { get; set; }
        
        public string GpsValid { get; set; }
        
        public string DistrictCode { get; set; }

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
            if (!string.IsNullOrEmpty(Convert.ToString(AlertTime)))
            {
                builder.AppendLine("AlertTime:" + AlertTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GpsValid)))
            {
                builder.AppendLine("GpsValid:" + GpsValid.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DistrictCode)))
            {
                builder.AppendLine("DistrictCode:" + DistrictCode.ToString());
            }
            return builder.ToString();
        }

    }
}
