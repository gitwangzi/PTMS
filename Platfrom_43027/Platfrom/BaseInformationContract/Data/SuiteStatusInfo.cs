/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 50fb384a-0db6-40c8-a526-ff4dec642dd4      
/////             clrversion: 4.0.30319.34011
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Contract.Data
/////    Project Description:    
/////             Class Name: SuiteInfoToAlertManager
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/2/21 05:22:20
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/2/21 05:22:20
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
    public class SuiteStatusInfo
    {
        public string ClientId { get; set; }//云账号客户ID
        /// <summary>
        /// Mdvr Core Id
        /// </summary>
        public string MdvrCoreId { get; set; }
        /// <summary>
        /// PTMS GPS SN
        /// </summary>
        public string PTMSGpsSn { get; set; }
        /// <summary>
        /// Suite Id
        /// </summary>
        public string SuiteId { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// DistrictCode
        /// </summary>
        public string DistrictCode { get; set; }
        /// <summary>
        /// VehicleId
        /// </summary>
        public string VehicleId { get; set; }
        /// <summary>
        /// SuiteInfoID
        /// </summary>
        public string SuiteInfoID { get; set; }
        /// <summary>
        /// Online Flag
        /// </summary>
        public bool OnlineFlag { get; set; }
        /// <summary>
        /// Vehicle Type
        /// </summary>
        public int VehicleType { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(PTMSGpsSn)))
            {
                builder.AppendLine("PTMSGpsSn:" + PTMSGpsSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteId)))
            {
                builder.AppendLine("SuiteId:" + SuiteId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DistrictCode)))
            {
                builder.AppendLine("DistrictCode:" + DistrictCode.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteInfoID)))
            {
                builder.AppendLine("SuiteInfoID:" + SuiteInfoID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OnlineFlag)))
            {
                builder.AppendLine("OnlineFlag:" + OnlineFlag.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleType)))
            {
                builder.AppendLine("VehicleType:" + VehicleType.ToString());
            }
            return builder.ToString();
        }

    }
}
