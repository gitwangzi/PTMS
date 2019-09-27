/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 961b004a-256f-4052-a1bb-9d78f0371b76      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Entity
/////    Project Description:    
/////             Class Name: C70
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/20 17:13:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/20 17:13:14
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Base.Contract.Data;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// ReMomedAlarmC70
    /// </summary>
    [Serializable]
    [DataContract]
    public class ReMomedAlarm : DownwardBase
    {
        /// <summary>
        /// Convert To String 
        /// 99dc[Cmd Length],[MDVR Card ID],[Message ID],CMD Keyword,
        /// [Cmd Send Time],Alert Type,Define Alert ID,Stop Alert Time,One-Button Alert State#
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("99dcXXXX,")  ////99dc[Cmd Length]
              .Append(this.DvId)  ////,[MDVR Card ID]
              .Append(",")
              .Append(this.MsgId) ////[Message ID]
              .Append(",")
              .Append("C70")   ////CMD Keyword
              .Append(",")
              .Append(this.GpsTime.ToString("yyMMdd HHmmss")) ////Cmd Send Time
              .Append(",")
              .Append("02000000")  ////Alert Type
              .Append(",")         ////Define Alert ID
              .Append(",")            ////Stop Alert Time
              .Append(",")
              .Append("0#");       ////One-Button Alert State
            ////Replace Length,Reduce 8 Bytes(99dcxxxx)
            sb.Replace("XXXX", (sb.Length - 8).ToString("D5").Substring(1));
            return sb.ToString();
        }
    }
}
