/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 19e98536-9dd2-46ef-ab90-c265830619a0      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Alarm
/////    Project Description:    
/////             Class Name: ECU911Dispose
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/12/2 16:02:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/12/2 16:02:41
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data
{
    public class ECU911Dispose
    {
        public string AlarmID { get; set; }
        /// <summary>
        /// Forwarded Flag
        /// </summary>
        public Nullable<short> ForwardedFlag { get; set; }
        public Nullable<short> AlarmFlag { get; set; }
        public Nullable<System.DateTime> DisposeTime { get; set; }
        public string DisposeStaff { get; set; }
        public string Ecu911Center { get; set; }
        public string IncidentId { get; set; }
        /// <summary>
        /// Forward Dest
        /// </summary>
        public string ForwardDest { get; set; }
        /// <summary>
        /// Forward Time
        /// </summary>
        public Nullable<System.DateTime> ForwardTime { get; set; }
        /// <summary>
        /// Alarm Address
        /// </summary>
        public string AlarmAddress { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmID)))
            {
                builder.AppendLine("AlarmID:" + AlarmID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ForwardedFlag)))
            {
                builder.AppendLine("ForwardedFlag:" + ForwardedFlag.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmFlag)))
            {
                builder.AppendLine("AlarmFlag:" + AlarmFlag.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DisposeTime)))
            {
                builder.AppendLine("DisposeTime:" + DisposeTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DisposeStaff)))
            {
                builder.AppendLine("DisposeStaff:" + DisposeStaff.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Ecu911Center)))
            {
                builder.AppendLine("Ecu911Center:" + Ecu911Center.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IncidentId)))
            {
                builder.AppendLine("IncidentId:" + IncidentId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ForwardDest)))
            {
                builder.AppendLine("ForwardDest:" + ForwardDest.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ForwardTime)))
            {
                builder.AppendLine("ForwardTime:" + ForwardTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmAddress)))
            {
                builder.AppendLine("AlarmAddress:" + AlarmAddress.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Content)))
            {
                builder.AppendLine("Content:" + Content.ToString());
            }
            return builder.ToString();
        }

    }
}
