/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: fccd6795-4682-4732-893a-f21f21f88e40      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Traffic
/////    Project Description:    
/////             Class Name: SettingOverSpeedCMD
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/6 10:09:18
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/6 10:09:18
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Enum;

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// Set Overspeed C68
    /// </summary>
    [Serializable]
    [DataContract]
    public class SettingOverSpeedCMD : DownwardBase
    {
        /// <summary>
        /// Send Time
        /// </summary>
        [DataMember]
        public DateTime SendTime { get; set; }

        /// <summary>
        /// Start Flag
        /// 0:Abandon OverSpeed Alarm
        /// 1:Start OverSpeed Alarm By param
        /// 2:Start OverSpeed Alarm By Security Suite Param
        /// </summary>
        [DataMember]
        public int OperType { get; set; }

        /// <summary>
        /// Max Speed
        /// </summary>
        [DataMember]
        public string MaxSpeed { get; set; }

        /// <summary>
        /// Min Speed
        /// </summary>
        [DataMember]
        public string MinSpeed { get; set; }

        /// <summary>
        /// Duration
        /// </summary>
        [DataMember]
        public string Duration { get; set; }

        /// <summary>
        /// Valid Time
        /// </summary>
        [DataMember]
        public string ValidTime { get; set; }

        /// <summary>
        /// OverSpeed ID
        /// </summary>
        [DataMember]
        public string OverSpeedID { get; set; }

        //public override string ToString()
        //{
        //    ////99dc[Cmd Length][MDVR Card ID],[Message ID],Cmd Key ,[Cmd Send Time],Start or End Flag,Min Speed,Max Speed,Span Time,Effective Time#
        //    StringBuilder strCmd = new StringBuilder();
        //    strCmd.Append("99dcXXXX,")
        //        .Append(this.DvId)
        //        .Append(",")
        //        .Append(this.MsgId)
        //        .Append(",")
        //        .Append("C68")
        //        .Append(",")
        //        .Append(this.SendTime.ToString("yyMMdd HHmmss"))
        //        .Append(",")
        //        .Append(this.OperType)
        //        .Append(",")
        //        .Append(this.MinSpeed)
        //        .Append(",")
        //        .Append(this.MaxSpeed)
        //        .Append(",")
        //        .Append(this.Duration)
        //        //////////////According Device  not Realize the Function ,it does not modify field which send to Device
        //        //////////////update dzl 2014-03-21
        //        //.Append(",")
        //        //.Append(this.ValidTime)
        //        .Append("#");
        //    ////Replace Length,Reduce 8 bytes(99dcxxxx)
        //    strCmd.Replace("XXXX", (strCmd.Length - 8).ToString("D5").Substring(1));
        //    return strCmd.ToString();
        //}

        public string ToString(RuleOperationType OperationType)
        {
            ////99dc[Cmd Length][MDVR Card ID],[Message ID],Cmd Key ,[Cmd Send Time],Start or End Flag,Min Speed,Max Speed,Span Time,Effective Time#
            StringBuilder strCmd = new StringBuilder();
            strCmd.Append("99dcXXXX,")
                .Append(this.DvId)
                .Append(",")
                .Append(string.Format("{0}:{1}", this.MsgId, this.OperType))
                .Append(",")
                .Append("C68")
                .Append(",")
                .Append(this.SendTime.ToString("yyMMdd HHmmss"))
                .Append(",")
                .Append(this.OperType)
                .Append(",")
                .Append(this.MinSpeed)
                .Append(",")
                .Append(this.MaxSpeed)
                .Append(",")
                .Append(this.Duration)
                //////////////According Device  not Realize the Function ,it does not modify field which send to Device
                //////////////update dzl 2014-03-21
                //.Append(",")
                //.Append(this.ValidTime)
                .Append("#");
            ////Replace Length,Reduce 8 bytes(99dcxxxx)
            strCmd.Replace("XXXX", (strCmd.Length - 8).ToString("D5").Substring(1));
            return strCmd.ToString();
        }
    }
}
