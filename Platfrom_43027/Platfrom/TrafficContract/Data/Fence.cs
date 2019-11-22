/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6006d856-ff35-4230-902b-fd3933620dfe      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Contract.Data
/////    Project Description:    
/////             Class Name: Fence
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/29 11:14:05
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/29 11:14:05
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
    public class Fence
    {
        /// <summary>
        /// OID
        /// </summary>
        [DataMember]
        public decimal OBJECTID { get; set; }

        [DataMember]
        public string NAME { get; set; }

        [DataMember]
        public Nullable<short> ALERT_TYPE { get; set; }

        [DataMember]
        public Nullable<short> FENCE_TYPE { get; set; }

        [DataMember]
        public string SPEED_LIMIT { get; set; }
 
        [DataMember]
        public string TIME_LIMIT { get; set; }

        [DataMember]
        public string PTS { get; set; }

        [DataMember]
        public bool IsmarkFenceGraphic { get; set; }

        [DataMember]
        public decimal? RAIDUMS { get; set; }

        [DataMember]
        public DateTime? CREATTIME { get; set; }

        [DataMember]
        public string CREATUSER { get; set; }

        [DataMember]
        public string CIRCLE_CENTER { get; set; }

        [DataMember]
        public string ADDRESSTEXT { get; set; }

        [DataMember]
        public short? Valid { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(OBJECTID)))
            {
                builder.AppendLine("OBJECTID:" + OBJECTID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(NAME)))
            {
                builder.AppendLine("NAME:" + NAME.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ALERT_TYPE)))
            {
                builder.AppendLine("ALERT_TYPE:" + ALERT_TYPE.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FENCE_TYPE)))
            {
                builder.AppendLine("FENCE_TYPE:" + FENCE_TYPE.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SPEED_LIMIT)))
            {
                builder.AppendLine("SPEED_LIMIT:" + SPEED_LIMIT.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(TIME_LIMIT)))
            {
                builder.AppendLine("TIME_LIMIT:" + TIME_LIMIT.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(PTS)))
            {
                builder.AppendLine("PTS:" + PTS.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IsmarkFenceGraphic)))
            {
                builder.AppendLine("IsmarkFenceGraphic:" + IsmarkFenceGraphic.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RAIDUMS)))
            {
                builder.AppendLine("RAIDUMS:" + RAIDUMS.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CREATTIME)))
            {
                builder.AppendLine("CREATTIME:" + CREATTIME.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CREATUSER)))
            {
                builder.AppendLine("CREATUSER:" + CREATUSER.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CIRCLE_CENTER)))
            {
                builder.AppendLine("CIRCLE_CENTER:" + CIRCLE_CENTER.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ADDRESSTEXT)))
            {
                builder.AppendLine("ADDRESSTEXT:" + ADDRESSTEXT.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Valid)))
            {
                builder.AppendLine("Valid:" + Valid.ToString());
            }
            return builder.ToString();
        }

    }
}
