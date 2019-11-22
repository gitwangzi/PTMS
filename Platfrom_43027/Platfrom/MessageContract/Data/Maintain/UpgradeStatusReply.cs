/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0f344642-43a5-491b-abef-aa95016a1677      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Maintain
/////    Project Description:    
/////             Class Name: UpgradeStatusReply
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/28 11:00:47
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/28 11:00:47
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
using Gsafety.Common.Util;


namespace Gsafety.PTMS.Message.Contract.Data
{
    [DataContract]
    [Serializable]
    public class UpgradeStatusReply : ReplyBaseModel
    {

        public UpgradeStatusReply() { }
        public UpgradeStatusReply(string str)
        {
            ////Answer Safety Suite Cmd From Traffic Center ,Format is :99dc[Cmd Length],[MDVR Card ID],
            ////[Message ID],Cmd Key Word,[Cmd Send Time],Original Cmd Key Word,Original Cmd Upload Time,Ack Mode,Ack Context#
            ////Return Message:Success Flag,DownLoad Size,Error Code
            if (!string.IsNullOrEmpty(str))
            {
                string[] array = str.Split(',');

                this.MdvrCoreId = array[1];          ////Mdvr Core Id

                this.GpsValid = array[5];     ////Gps Valid
                this.Longitude = array[6];    ////Longitude
                this.Latitude = array[7];     ////Latitude
                this.Speed = array[8];        ////Speed
                this.Direction = array[9];    ////Direction

                this.OriginalCmd = array[20];   ////Original Cmd
                this.ReplyType = int.Parse(array[22]);
                this.ReplyResult = int.Parse(array[23]);
                if (this.ReplyResult == 1)   ////Fail
                {
                    ////0x00020050	Receive Upgrade File FAIL
                    ////0x00020051	Check Upgrade File FAIL
                    ////0x00020052	Upgrade File Path Not Exist
                    ////0x00020053	Open Upgrade File FAIL
                    ////0x00020054	Get File Header FAIL
                    ////0x000100A3	 Memery Not Exist
                    ////0x000100A2	 Task Not Exist
                    this.ErrorCode = array[25]; ////Error Code
                    this.Message = array[26];  ////Message
                }

                this.DowmSize = int.Parse(array[24]);  ////Download File Size


                var gpsTime = ConvertHelper.ConvertStrToDate(array[4], "yyMMdd HHmmss");
                if (gpsTime != null)
                {
                    this.GpsTime = gpsTime.Value;     ////gps Time
                }

                var originalTime = ConvertHelper.ConvertStrToDate(array[21], "yyMMdd HHmmss");
                if (originalTime != null)
                {
                    this.OriginalTime = originalTime.Value;     ////original Time
                }
            }
        }

        /// <summary>
        /// Download File Size
        /// </summary>
        [DataMember]
        public int DowmSize { get; set; }

        /// <summary>
        /// Reply Result	
        /// 0:deny
        /// 1:agree
        /// </summary>
        [DataMember]
        public int ReplyResult { get; set; }

        /// <summary>
        /// Error Code	
        /// 0x00010007
        /// 0x00000011
        /// 0x000100A3
        /// New Standard
        /// 11:Doing One-Click Alerm
        /// 13:Resource not Enough
        /// </summary>
        [DataMember]
        public string ErrorCode { get; set; }

        /// <summary>
        /// Description using ASCII. Max Length is 256 Bytes
        /// </summary>
        [DataMember]
        public string Message { get; set; }
    }
}
