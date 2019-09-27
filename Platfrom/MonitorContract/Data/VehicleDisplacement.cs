/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 018644ec-ffa8-4a70-aa30-94505bf31650      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: TEST(ZhangY)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Monitor.Contract.Data
/////    Project Description:    
/////             Class Name: VehicleDisplacement
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/10 13:58:32
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/10 13:58:32
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
    public class VehicleDisplacement
    {
        [DataMember]
        public string Mdvr_Core_SN { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public double Distance { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Mdvr_Core_SN)))
            {
                builder.AppendLine("Mdvr_Core_SN:" + Mdvr_Core_SN.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Longitude)))
            {
                builder.AppendLine("Longitude:" + Longitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Latitude)))
            {
                builder.AppendLine("Latitude:" + Latitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Distance)))
            {
                builder.AppendLine("Distance:" + Distance.ToString());
            }
            return builder.ToString();
        }

    }
}
