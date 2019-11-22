/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 34f24c15-2924-4622-8e53-abcc8ea355f9      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Contract.Data
/////    Project Description:    
/////             Class Name: CarSpeed
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/30 13:49:20
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/30 13:49:20
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
    public class CarSpeed
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string SPEED_ID { get; set; }
        [DataMember]
        public string VEHICLE_ID { get; set; }
        [DataMember]
        public string MDVR_CORE_SN { get; set; }
        [DataMember]
        public short VALID { get; set; }
        [DataMember]
        public DateTime? CreateTime { get; set; }
        [DataMember]
        public string CreateUser { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SPEED_ID)))
            {
                builder.AppendLine("SPEED_ID:" + SPEED_ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VEHICLE_ID)))
            {
                builder.AppendLine("VEHICLE_ID:" + VEHICLE_ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MDVR_CORE_SN)))
            {
                builder.AppendLine("MDVR_CORE_SN:" + MDVR_CORE_SN.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VALID)))
            {
                builder.AppendLine("VALID:" + VALID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateUser)))
            {
                builder.AppendLine("CreateUser:" + CreateUser.ToString());
            }
            return builder.ToString();
        }

    }
}
