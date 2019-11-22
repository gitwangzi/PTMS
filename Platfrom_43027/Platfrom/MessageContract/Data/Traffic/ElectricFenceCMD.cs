/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b1cc0edf-c955-4c7a-8bc0-0b36ab39c964      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Monitor
/////    Project Description:    
/////             Class Name: ElectricFence
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/30 15:22:44
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/30 15:22:44
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
    /// Setting Electric Fence c170
    /// </summary>
    [Serializable]
    [DataContract]
    public class ElectricFenceCMD : DownwardBase
    {
        public ElectricFenceCMD()
        {
            OperType = 2;
        }

        /// <summary>
        /// Send Time
        /// </summary>
        [DataMember]
        public DateTime SendTime { get; set; }

        /// <summary>
        /// Fence Id
        /// Each Fence Has one Code,ASCII Code,Int
        /// </summary>
        [DataMember]
        public int FenceId { get; set; }

        /// <summary>
        /// Operation Type 
        /// ASCII Code 0-9
        /// 1:Add,2;Modify ,3:Delete
        /// </summary>
        [DataMember]
        public int OperType { get; set; }

        /// <summary>
        /// BIT0=0,close Fence
        /// BIT0=1,use fence
        /// BIT1=1,overspeed alarm
        /// BIT2=1,lowspeed alarm
        /// BIT3=1,infence alarm
        /// BIT4=1,outfence alarm
        /// BIT5=0,time limited invalid
        /// BIT5=1,time limited avalid
        /// </summary>
        [DataMember]
        public int Action { get; set; }

        /// <summary>
        /// Speed 
        /// ACSII Code,Min-Max,km/h.For example 30-40,use the first package
        /// </summary>
        [DataMember]
        public string Speed { get; set; }

        /// <summary>
        /// Valid Time
        /// ACSII,Start Time - End Time, in Minutes. for example 8:30-21:30
        /// </summary>
        [DataMember]
        public string ValidTime { get; set; }

        /// <summary>
        /// Package ID
        /// ASCII Code,Max Length is 5 bytes
        /// </summary>
        [DataMember]
        public int PacketID { get; set; }

        /// <summary>
        /// Data Package Number ID
        /// ASCII Code,Max Length is 5 bytes
        /// </summary>
        [DataMember]
        public int PacketTotal { get; set; }

        /// <summary>
        /// Package ID
        /// ASCII Code, Max Length 5 BYTES
        /// </summary>
        [DataMember]
        public int PacketSeq { get; set; }

        /// <summary>
        /// Electric Fence ID
        /// ASCII Code,Max Byte 800,Each Point including Latitude and Longitude in one pair ,not Allow in two Data Package.GPS is According Longitude
        /// Latitude To Arrange，East Longitude is E, West Longitude is W,North Latitude is N, South Latitude is S, the Format is dddmm.mmmm as ddd°mm.mmmm'
        /// For Instance: 
        /// E11433.6666N02455.8888E11434.7777N02456.9999
        /// is E114°33.6666' N24°55.8888' and E114°34.7777' N24°56.9999'
        /// </summary>
        [DataMember]
        public string Fence { get; set; }

        /// <summary>
        /// Fence Name
        /// </summary>
        [DataMember]
        public string FenceName { get; set; }
        /// <summary>
        /// Context
        /// </summary>
        [DataMember]
        public string Context { get; set; }

        [DataMember]
        public AreaType AreaType { get; set; }

        ///// <summary>
        ///// 99dc[Cmd Length],[MDVR Card ID],[Message ID],Cmd Key,[Send Time],Electric FenceID,
        ////Operation Type,Ablity,Speed Threashold,Time Threashold,Data Package ID,Data Package Number,Package ID,Data of Eletric Fence#
        ///// </summary>
        ///// <returns></returns>
        //public override string ToString()
        //{
        //    StringBuilder strCmd = new StringBuilder();
        //    string action = string.Empty;
        //    if (this.PacketTotal == 0)
        //        this.PacketTotal = 1;
        //    if (this.PacketSeq == 0)
        //        this.PacketSeq = 1;

        //    //if (this.Action > 0)
        //    //{
        //    //    string strBinary = Convert.ToString(this.Action, 16);
        //    //    action = strBinary.Length == 1 ? "0" + strBinary : strBinary;
        //    //}

        //    strCmd.Append("99dcXXXX,")
        //        .Append(this.DvId)
        //        .Append(",")
        //        .Append(this.MsgId)
        //        .Append(",")
        //        .Append("C107")
        //        .Append(",")
        //        .Append(this.SendTime.ToString("yyMMdd HHmmss"))
        //        .Append(",")
        //        .Append(this.FenceId)
        //        .Append(",")
        //        .Append(this.OperType)
        //        .Append(",")
        //        .Append(this.Action)
        //        .Append(",")
        //        .Append(this.Speed)
        //        .Append(",")
        //        .Append(this.ValidTime)
        //        .Append(",")
        //        .Append(this.PacketID)
        //        .Append(",")
        //        .Append(this.PacketTotal)
        //        .Append(",")
        //        .Append(this.PacketSeq)
        //        .Append(",")
        //        .Append(this.Fence)
        //        .Append("#");
        //    ////Replace Length,Reduce 8 bytes(99dcxxxx)
        //    strCmd.Replace("XXXX", (strCmd.Length - 8).ToString("D5").Substring(1));
        //    this.Context = strCmd.ToString();
        //    return strCmd.ToString();
        //}

        public string ToString(ElectricFenceOperType OperationType)
        {
            StringBuilder strCmd = new StringBuilder();
            string action = string.Empty;
            if (this.PacketTotal == 0)
                this.PacketTotal = 1;
            if (this.PacketSeq == 0)
                this.PacketSeq = 1;

            //if (this.Action > 0)
            //{
            //    string strBinary = Convert.ToString(this.Action, 16);
            //    action = strBinary.Length == 1 ? "0" + strBinary : strBinary;
            //}

            strCmd.Append("99dcXXXX,")
                .Append(this.DvId)
                .Append(",")
                .Append(string.Format("{0}:{1}", this.MsgId, (int)OperationType))
                .Append(",")
                .Append("C107")
                .Append(",")
                .Append(this.SendTime.ToString("yyMMdd HHmmss"))
                .Append(",")
                .Append(this.FenceId)
                .Append(",")
                .Append(this.OperType)
                .Append(",")
                .Append(this.Action)
                .Append(",")
                .Append(this.Speed)
                .Append(",")
                .Append(this.ValidTime)
                .Append(",")
                .Append(this.PacketID)
                .Append(",")
                .Append(this.PacketTotal)
                .Append(",")
                .Append(this.PacketSeq)
                .Append(",")
                .Append(this.Fence)
                .Append("#");
            ////Replace Length,Reduce 8 bytes(99dcxxxx)
            strCmd.Replace("XXXX", (strCmd.Length - 8).ToString("D5").Substring(1));
            this.Context = strCmd.ToString();
            return strCmd.ToString();
        }

        public override string ToString()
        {
            return ToString((ElectricFenceOperType)this.OperType);
        }
    }
}
