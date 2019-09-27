/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: cec477c6-2a50-464e-a23b-fc4def3c405a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(BilongLiu)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract.Data
/////    Project Description:    
/////             Class Name: AlarmTreatment
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/23 11:09:31
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/23 11:09:31
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Alarm.Contract.Data
{
    [DataContract]
    public class Alarm911Treatment
    {
        [DataMember]
        public string AlarmId { get; set; }
        /// <summary>
        /// ECU-911 Center
        /// </summary>
        [DataMember]
        public string Ecu911Center { get; set; }

        [DataMember]
        public string DisposeStaff { get; set; }

        [DataMember]
        public Nullable<DateTime> DisposeTime { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public int? ALARM_FLAG { get; set; }

        /// <summary>
        /// the alarm happend place 
        /// </summary>
         [DataMember]
        public String ALARM_ADDRESS { get; set; }

        /// <summary>
        /// FORWARD to where (distict code)
        /// </summary>
         [DataMember]
        public string FORWARD_DEST { get; set; }

         [DataMember]
        public DateTime? FORWARD_TIME { get; set; }

         [DataMember]
        public int? FORWARDED_FLAG { get; set; }
        
         [DataMember]
         public string INCIDENT_ID { get; set; }

         public override string ToString()
         {
             StringBuilder builder = new StringBuilder();
             if (!string.IsNullOrEmpty(Convert.ToString(AlarmId)))
             {
                 builder.AppendLine("AlarmId:" + AlarmId.ToString());
             }
             if (!string.IsNullOrEmpty(Convert.ToString(Ecu911Center)))
             {
                 builder.AppendLine("Ecu911Center:" + Ecu911Center.ToString());
             }
             if (!string.IsNullOrEmpty(Convert.ToString(DisposeStaff)))
             {
                 builder.AppendLine("DisposeStaff:" + DisposeStaff.ToString());
             }
             if (!string.IsNullOrEmpty(Convert.ToString(DisposeTime)))
             {
                 builder.AppendLine("DisposeTime:" + DisposeTime.ToString());
             }
             if (!string.IsNullOrEmpty(Convert.ToString(Content)))
             {
                 builder.AppendLine("Content:" + Content.ToString());
             }
             if (!string.IsNullOrEmpty(Convert.ToString(ALARM_FLAG)))
             {
                 builder.AppendLine("ALARM_FLAG:" + ALARM_FLAG.ToString());
             }
             if (!string.IsNullOrEmpty(Convert.ToString(ALARM_ADDRESS)))
             {
                 builder.AppendLine("ALARM_ADDRESS:" + ALARM_ADDRESS.ToString());
             }
             if (!string.IsNullOrEmpty(Convert.ToString(FORWARD_DEST)))
             {
                 builder.AppendLine("FORWARD_DEST:" + FORWARD_DEST.ToString());
             }
             if (!string.IsNullOrEmpty(Convert.ToString(FORWARD_TIME)))
             {
                 builder.AppendLine("FORWARD_TIME:" + FORWARD_TIME.ToString());
             }
             if (!string.IsNullOrEmpty(Convert.ToString(FORWARDED_FLAG)))
             {
                 builder.AppendLine("FORWARDED_FLAG:" + FORWARDED_FLAG.ToString());
             }
             if (!string.IsNullOrEmpty(Convert.ToString(INCIDENT_ID)))
             {
                 builder.AppendLine("INCIDENT_ID:" + INCIDENT_ID.ToString());
             }

             return builder.ToString();
         }

    }
}
