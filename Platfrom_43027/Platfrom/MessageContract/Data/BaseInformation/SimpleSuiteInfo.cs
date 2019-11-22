/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6f2c813c-5320-4e10-8d3e-cd5b918752b6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-GUOH
/////                 Author: TEST(guoh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.BaseInformation
/////    Project Description:    
/////             Class Name: SimpleSuiteInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 9/5/2013 4:49:43 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/5/2013 4:49:43 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// Security Suite Info
    /// </summary>
    [Serializable]
    [DataContract]
    public class SimpleSuiteInfo
    {
        /// <summary>
        /// Mdvr Core Id
        /// </summary>
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// SuiteInfo Id
        /// </summary>
        [DataMember]
        public string SuiteInfoId { get; set; }

        /// <summary>
        /// Vehicle Id
        /// </summary>
        [DataMember]
        public string VehicleId { get; set; }

        /// <summary>
        /// District Code
        /// </summary>
        [DataMember]
        public string DistrictCode { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteInfoId)))
            {
                builder.AppendLine("SuiteInfoId:" + SuiteInfoId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DistrictCode)))
            {
                builder.AppendLine("DistrictCode:" + DistrictCode.ToString());
            }
            return builder.ToString();
        }

    }
}
