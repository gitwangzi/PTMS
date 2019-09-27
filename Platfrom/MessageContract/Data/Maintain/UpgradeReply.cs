/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 911f9868-a79e-416d-88af-82ba5c3038b7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Monitor
/////    Project Description:    
/////             Class Name: UpgradeReply
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/30 10:54:29
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/30 10:54:29
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
    public class UpgradeReply : ReplyBaseModel
    {

        public UpgradeReply(string str)
        {
            ////99dcxxxx,T0001,Johnny,V0,[Location and State],C152,070729 234015,0,0,FFFFFF0A,No Apply#
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
                this.ErrorCode = array[24];
                this.Message = array[25];

                this.Context = str;                    //// Original Context

                var gpsTime = ConvertHelper.ConvertStrToDate(array[4], "yyMMdd HHmmss");
                if (gpsTime != null)
                {
                    this.GpsTime = gpsTime.Value;     ////Gps Time
                }

                var originalTime = ConvertHelper.ConvertStrToDate(array[21], "yyMMdd HHmmss");
                if (originalTime != null)
                {
                    this.OriginalTime = originalTime.Value;     ////original Message Time
                }
            }
        }

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

        /// <summary>
        /// Context
        /// </summary>
        [DataMember]
        public string Context { get; set; }
    }
}
