/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a617d0d4-4de0-470a-aeee-daf6abec42c4      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Contract.Data
/////    Project Description:    
/////             Class Name: VehicleAlertDetail
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/26 14:09:04
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/26 14:09:04
/////            Modified by:
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
    [DataContract]
    public class VehicleAlertDetail : VehicleAlert
    {
        /// <summary>
        /// provicemsg
        /// </summary>
        [DataMember]
        public string ProvinceName { get; set; }

        /// <summary>
        /// cityname
        /// </summary>
        [DataMember]
        public string CityName { get; set; }
       
        /// <summary>
        /// SuiteId 
        /// </summary>
        [DataMember]
        public string SuiteId { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ProvinceName)))
            {
                builder.AppendLine("ProvinceName:" + ProvinceName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CityName)))
            {
                builder.AppendLine("CityName:" + CityName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteId)))
            {
                builder.AppendLine("SuiteId:" + SuiteId.ToString());
            }
            return builder.ToString();
        }

        
    }
}
