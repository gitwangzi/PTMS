using Gsafety.PTMS.Base.Contract.Data;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 26de8bb0-ebb5-47f5-8d3d-521258653f40      
/////             clrversion: 4.0.30319.34014
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Monitor
/////    Project Description:    
/////             Class Name: C30
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/4/12 23:35:17
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/4/12 23:35:17
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
    /// Start and stop GPS location monitoring
    /// </summary>
    [Serializable]
    [DataContract]
    public class C30CMD:DownwardBase
    {
        const string CmdType = "C30";

        [DataMember]
        public DateTime SendTime { get; set; }

        [DataMember]
        public int OpenOrStop { get; set; }

        [DataMember]
        public int DistanceInterval { get; set; }

        [DataMember]
        public int TimeInterval { get; set; }

        [DataMember]
        public int ReportNumber { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        [DataMember]
        public string Context { get; set; }

        /// <summary>
        /// 99dc [instruction length], [MDVR chip number], [message sequence number], command keyword, 
        /// [command transmission time], start and stop markers, distance interval, time interval, the number of returns, the combination of markers #
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder strCmd = new StringBuilder();
            DistanceInterval = 50;
            strCmd.Append("99dcXXXX,")
                .Append(this.DvId)
                .Append(",")
                .Append(this.MsgId)
                .Append(",")
                .Append(CmdType)
                .Append(",")
                .Append(this.SendTime.ToString("yyMMdd HHmmss"))
                .Append(",")
                .Append(OpenOrStop)
                .Append(",")
                .Append(DistanceInterval)
                .Append(",")
                .Append(TimeInterval)
                .Append(",")
                .Append(ReportNumber)
                .Append(",0003")
                .Append("#");
            ////Replace length minus 8 bytes（99dcxxxx）
            strCmd.Replace("XXXX", (strCmd.Length - 8).ToString("D5").Substring(1));
            this.Context = strCmd.ToString();
            return strCmd.ToString();
        }
    }
}
