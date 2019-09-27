/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5501a7fe-49e2-4073-95bc-f60763ae7e29      
/////             clrversion: 4.0.30319.34003
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Traffic
/////    Project Description:    
/////             Class Name: TravelPlanCMD
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/1/29 05:18:07
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/1/29 05:18:07
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
    /// Travel Plan  Command
    /// </summary>
    [Serializable]
    [DataContract]
    public class TravelPlanCMD
    {
        /// <summary>
        /// Schedule ID
        /// </summary>
        [DataMember]
        public string ScheduleID { get; set; }

        /// <summary>
        /// Vechile ID
        /// </summary>
        [DataMember]
        public string VechileID { get; set; }

        /// <summary>
        /// WeekDay
        /// </summary>
        [DataMember]
        public string WeekDay { get; set; }

        /// <summary>
        /// Begin Date
        /// </summary>
        [DataMember]
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// End Date
        /// </summary>
        [DataMember]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Tolerance(Second)
        /// </summary>
        [DataMember]
        public short Tolerance { get; set; }

        /// <summary>
        /// Radius(Mile)
        /// </summary>
        [DataMember]
        public short Radius { get; set; }

        /// <summary>
        /// Raptor GPS IMER Number
        /// </summary>
        [DataMember]
        public string RaptorIMEI { get; set; }

        /// <summary>
        /// Operation Type
        /// ASCII 0-9
        /// 1:Add Travel Plan,2:modify Travel Plan3: delete Travel Plan
        /// </summary>
        [DataMember]
        public int OperType { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ScheduleID)))
            {
                builder.AppendLine("ScheduleID:" + ScheduleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VechileID)))
            {
                builder.AppendLine("VechileID:" + VechileID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(WeekDay)))
            {
                builder.AppendLine("WeekDay:" + WeekDay.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(BeginDate)))
            {
                builder.AppendLine("BeginDate:" + BeginDate.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(EndDate)))
            {
                builder.AppendLine("EndDate:" + EndDate.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Tolerance)))
            {
                builder.AppendLine("Tolerance:" + Tolerance.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Radius)))
            {
                builder.AppendLine("Radius:" + Radius.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RaptorIMEI)))
            {
                builder.AppendLine("RaptorIMEI:" + RaptorIMEI.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OperType)))
            {
                builder.AppendLine("OperType:" + OperType.ToString());
            }
            return builder.ToString();
        }

    }
}
