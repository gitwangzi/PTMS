using Gsafety.Common.Util;
using Gsafety.PTMS.Base.Contract.Data;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 338ff3f7-b720-4612-9123-88b169bbdc9f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Monitor
/////    Project Description:    
/////             Class Name: C80Reply
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/3 16:23:27
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/3 16:23:27
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
    [Serializable]
    [DataContract]
    public class C80Reply : ReplyBaseModel
    {
        public C80Reply(string str)
        {
            string[] array = str.Substring(0, str.LastIndexOf("#")).Split(',');

            this.MdvrCoreId = array[1];
            //this.AssociationSetID = array[2].Substring(0, 32);
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
            this.Cmd = array[3];// command type
            this.GpsValid = array[5];
            this.Longitude = array[6];
            this.Latitude = array[7];
            this.Speed = array[8];
            this.Direction = array[9];

            this.OriginalCmd = array[20];
            this.ReplyType = int.Parse(array[22]);
            this.ReplyResult = int.Parse(array[23]);
            //this.OperType = int.Parse(array[24]);        

            var gpsTime = ConvertHelper.ConvertStrToDate(array[4], "yyMMdd HHmmss");
            if (gpsTime != null)
            {
                this.GpsTime = gpsTime.Value;
            }

            var originalTime = ConvertHelper.ConvertStrToDate(array[21], "yyMMdd HHmmss");
            if (originalTime != null)
            {
                this.OriginalTime = originalTime.Value;
            }
        }

        [DataMember]
        public int ReplyResult { get; set; }



        /// <summary>
        /// Error Code
        /// </summary>
        [DataMember]
        public string ErrorCode { get; set; }

        /// <summary>
        /// Fail Because
        /// </summary>
        [DataMember]
        public string ErrorMsg { get; set; }
    }
}
