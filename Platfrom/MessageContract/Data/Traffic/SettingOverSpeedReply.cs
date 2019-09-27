/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c0715006-9243-410c-80f4-ecb29afc661c      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Traffic
/////    Project Description:    
/////             Class Name: SettingOverSpeedReply
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/6 10:10:43
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/6 10:10:43
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
    /// <summary>
    /// Set Electric Fence C170
    /// </summary>
    [Serializable]
    [DataContract]
    public class SettingOverSpeedReply : ReplyBaseModel
    {
        public SettingOverSpeedReply()
        {
        }

        public SettingOverSpeedReply(string str)
        {
            ////the Sign Of Success,Start Or End Sign , Error Code ,Fail Reason
            string[] array = str.Substring(0, str.LastIndexOf("#")).Split(',');

            this.MdvrCoreId = array[1];   ////MdvrCoreId
            if (array[2].Contains(":"))
            {
                this.AssociationSetID = array[2].Split(':')[0];
                this.OperType = array[2].Split(':')[1]; //operation type
            }
            else
            {
                this.AssociationSetID = array[2].Remove(array[2].Length-13,13);
                this.OperType = "0"; //operation type
            }
            this.Cmd = array[3];
            this.GpsValid = array[5];     ////GpsValid
            this.Longitude = array[6];    ////Longitude
            this.Latitude = array[7];     ////Latitude
            this.Speed = array[8];        ////Speed
            this.Direction = array[9];    ////Direction

            this.OriginalCmd = array[20]; ////Original Command
            this.ReplyType = int.Parse(array[22]);
            this.ReplyResult = int.Parse(array[23]);     //// Success Flag	
            this.SettingFlay = int.Parse(array[24]);        //// Start Flag
            this.ErrorCode = array[25];                  //// Error Code
            this.ErrorMsg = array[26];                   //// Fail Reason

            var gpsTime = ConvertHelper.ConvertStrToDate(array[4], "yyMMdd HHmmss");
            if (gpsTime != null)
            {
                this.GpsTime = gpsTime.Value;     ////GPS Time
            }

            var originalTime = ConvertHelper.ConvertStrToDate(array[21], "yyMMdd HHmmss");
            if (originalTime != null)
            {
                this.OriginalTime = originalTime.Value;     ////Origin Time
            }
        }

        /// <summary>
        /// Success Flag
        /// 0:Fail
        /// 1:Success
        /// </summary>
        [DataMember]
        public int ReplyResult { get; set; }

        /// <summary>
        /// Start Flag
        /// 0:Abandon OverSpeed Alarm
        /// 1:Start OverSpeed Alarm By param
        /// 2:Start OverSpeed Alarm By Security Suite Param
        /// </summary>
        [DataMember]
        public int SettingFlay { get; set; }

        /// <summary>
        /// Error Code
        /// </summary>
        [DataMember]
        public string ErrorCode { get; set; }

        /// <summary>
        /// Error Mesage	
        /// </summary>
        [DataMember]
        public string ErrorMsg { get; set; }
    }
}
