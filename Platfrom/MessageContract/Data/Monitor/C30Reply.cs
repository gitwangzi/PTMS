/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 29440ec1-fa09-4942-8d03-9c73b969eed3      
/////             clrversion: 4.0.30319.34014
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Monitor
/////    Project Description:    
/////             Class Name: C30Repaly
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/4/12 23:49:03
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/4/12 23:49:03
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.Common.Util;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Message.Contract.Data
{
    [Serializable]
    [DataContract]
    public class C30Reply : ReplyBaseModel
    {
        public C30Reply()
        {
        }

        public C30Reply(string str)
        {
            ////成功标志,启禁标记,错误代码,失败原因
            string[] array = str.Substring(0, str.LastIndexOf("#")).Split(',');

            this.MdvrCoreId = array[1];   ////芯片号
            if (array[2].Contains(":"))
            {
                this.AssociationSetID = array[2].Split(':')[0];
                this.OperType = array[2].Split(':')[1]; //operation type
            }
            else
            {
                this.AssociationSetID = array[2];
                this.OperType = "0"; //operation type
            }
            this.Cmd = array[3];
            this.GpsValid = array[5];     ////GPS有效性
            this.Longitude = array[6];    ////经度
            this.Latitude = array[7];     ////纬度
            this.Speed = array[8];        ////速度
            this.Direction = array[9];    ////方向

            this.OriginalCmd = array[20]; ////透传信息类型
            this.ReplyType = int.Parse(array[22]);
            this.ReplyResult = int.Parse(array[23]);     //// 成功标志	
            this.SettingFlag = int.Parse(array[24]);        //// 启用标志
            //this.ErrorCode = array[25];                  //// 错误代码	
            //this.ErrorMsg = array[26];                   //// 失败原因	 

            var gpsTime = ConvertHelper.ConvertStrToDate(array[4], "yyMMdd HHmmss");
            if (gpsTime != null)
            {
                this.GpsTime = gpsTime.Value;     ////GPS时间
            }

            var originalTime = ConvertHelper.ConvertStrToDate(array[21], "yyMMdd HHmmss");
            if (originalTime != null)
            {
                this.OriginalTime = originalTime.Value;     ////原始消息时间
            }
        }

        /// <summary>
        /// 成功标志	
        /// 0：设置失败
        /// 1：设置成功
        /// </summary>
        [DataMember]
        public int ReplyResult { get; set; }

        /// <summary>
        /// 错误代码	
        /// </summary>
        [DataMember]
        public string ErrorCode { get; set; }

        /// <summary>
        /// 失败原因	
        /// </summary>
        [DataMember]
        public string ErrorMsg { get; set; }

        public int SettingFlag { get; set; }
    }
}
