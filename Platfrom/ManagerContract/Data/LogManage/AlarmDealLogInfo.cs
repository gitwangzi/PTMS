/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1b229b99-72aa-4ddf-a801-0992d2b3863a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract.Data
/////    Project Description:    
/////             Class Name: AlarmDealLogInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 7/31/2013 3:15:10 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 11/19/2013 3:15:10 PM
/////            Modified by:ShiHongsheng
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Manager.Contract.Data
{
    [DataContract]
    public class AlarmDealLogInfo
    {
        /// <summary>
        /// AlarmVihcleID
        /// </summary>
        [DataMember]
        public string AlarmVihcleID { get; set; }

        /// <summary>
        /// AlarmTime
        /// </summary>
        [DataMember]
        public DateTime? AlarmTime { get; set; }

        /// <summary>
        /// DealPerson
        /// </summary>
        [DataMember]
        public string DealPerson { get; set; }

        /// <summary>
        /// DealTime
        /// </summary>
        [DataMember]
        public DateTime? DealTime { get; set; }


        [DataMember]
        public string VehicleType { get; set; }

        [DataMember]
        public string Description { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmVihcleID)))
            {
                builder.AppendLine("AlarmVihcleID:" + AlarmVihcleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmTime)))
            {
                builder.AppendLine("AlarmTime:" + AlarmTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DealPerson)))
            {
                builder.AppendLine("DealPerson:" + DealPerson.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DealTime)))
            {
                builder.AppendLine("DealTime:" + DealTime.ToString());
            }
            return builder.ToString();
        }

    }
}
