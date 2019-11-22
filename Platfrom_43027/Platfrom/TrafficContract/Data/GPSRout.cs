/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4c3a8e71-7111-41e9-88a0-dd9d75dc4bee      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Contract.Data
/////    Project Description:    
/////             Class Name: GPSRout
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/10 15:05:46
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/10 15:05:46
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Traffic.Contract.Data
{
    [DataContract]
    public class GPSRout
    {
        [DataMember]
        public string GPSID { get; set; }

        [DataMember]
        public decimal RoutID { get; set; }

        [DataMember]
         public string RoutSN { get; set; }

        [DataMember]
        public string MDVR_CORE_SN { get; set; }

        [DataMember]
        public string VEHICLE_ID { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(GPSID)))
            {
                builder.AppendLine("GPSID:" + GPSID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RoutID)))
            {
                builder.AppendLine("RoutID:" + RoutID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RoutSN)))
            {
                builder.AppendLine("RoutSN:" + RoutSN.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MDVR_CORE_SN)))
            {
                builder.AppendLine("MDVR_CORE_SN:" + MDVR_CORE_SN.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VEHICLE_ID)))
            {
                builder.AppendLine("VEHICLE_ID:" + VEHICLE_ID.ToString());
            }
            return builder.ToString();
        }

    }
}
