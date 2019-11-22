/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: ea9a6e4d-51a0-4442-a7cc-69937252d605      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: TEST(ZhangY)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Monitor.Contract.Data
/////    Project Description:    
/////             Class Name: VehicleOnOffTime
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/10 13:57:51
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/10 13:57:51
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Monitor.Contract.Data
{
    [DataContract]
    public class VehicleOnOffTime
    {
        [DataMember]
        public string Vehicle_ID { get; set; }

        [DataMember]
        public string Mdvr_Core_SN { get; set; }

        [DataMember]
        public DateTime Online_Time { get; set; }

        [DataMember]
        public DateTime Offline_Time { get; set; }

        [DataMember]
        public decimal Online_Timespan { get; set; }

        [DataMember]
        public decimal Distance { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Vehicle_ID)))
            {
                builder.AppendLine("Vehicle_ID:" + Vehicle_ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Mdvr_Core_SN)))
            {
                builder.AppendLine("Mdvr_Core_SN:" + Mdvr_Core_SN.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Online_Time)))
            {
                builder.AppendLine("Online_Time:" + Online_Time.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Offline_Time)))
            {
                builder.AppendLine("Offline_Time:" + Offline_Time.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Online_Timespan)))
            {
                builder.AppendLine("Online_Timespan:" + Online_Timespan.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Distance)))
            {
                builder.AppendLine("Distance:" + Distance.ToString());
            }
            return builder.ToString();
        }

    }
}