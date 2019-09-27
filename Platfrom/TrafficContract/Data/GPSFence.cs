/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2abc06df-3388-4688-a47b-46c421f5b52d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Contract.Data
/////    Project Description:    
/////             Class Name: GPSFence
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/10 15:30:22
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/10 15:30:22
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
    public class GPSFence
    {
        /// <summary>
        ///  GPSID    
        /// </summary>
        [DataMember]
        public string GPSID { get; set; }

        [DataMember]
        public decimal FenceID { get; set; }

        [DataMember]
        public string FenceName { get; set; }

        [DataMember]
        public string MDVR_CORE_SN { get; set; }

        [DataMember]
        public string VEHICLE_ID { get; set; }

        [DataMember]
        public string CircleCenter { get; set; }

        [DataMember]
        public decimal? RAIDUMS { get; set; }

        [DataMember]
        public Nullable<short> ALERT_TYPE { get; set; }

        [DataMember]
        public Nullable<short> FENCE_TYPE { get; set; }

        [DataMember]
        public bool IsInFence { get; set; }

        [DataMember]
        public string TimeLimit { get; set; }
        /// <summary>
        /// (10,11,12;20,21,22,30,31)
        /// </summary>
        [DataMember]
        public short? Status { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(GPSID)))
            {
                builder.AppendLine("GPSID:" + GPSID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FenceID)))
            {
                builder.AppendLine("FenceID:" + FenceID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FenceName)))
            {
                builder.AppendLine("FenceName:" + FenceName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MDVR_CORE_SN)))
            {
                builder.AppendLine("MDVR_CORE_SN:" + MDVR_CORE_SN.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VEHICLE_ID)))
            {
                builder.AppendLine("VEHICLE_ID:" + VEHICLE_ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CircleCenter)))
            {
                builder.AppendLine("CircleCenter:" + CircleCenter.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RAIDUMS)))
            {
                builder.AppendLine("RAIDUMS:" + RAIDUMS.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ALERT_TYPE)))
            {
                builder.AppendLine("ALERT_TYPE:" + ALERT_TYPE.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FENCE_TYPE)))
            {
                builder.AppendLine("FENCE_TYPE:" + FENCE_TYPE.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IsInFence)))
            {
                builder.AppendLine("IsInFence:" + IsInFence.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(TimeLimit)))
            {
                builder.AppendLine("TimeLimit:" + TimeLimit.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            return builder.ToString();
        }

    }
}
