/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d3e96b16-e001-4c17-9df4-1b39b9514ab0      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Monitor
/////    Project Description:    
/////             Class Name: ElectricFenceReply
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/30 16:27:36
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/30 16:27:36
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
using Gsafety.Common.Util;

namespace Gsafety.PTMS.Message.Contract.Data
{
    [DataContract]
    [Serializable]
    public class ElectricFenceReply : ReplyBaseModel
    {
        public ElectricFenceReply(string str)
        {
            ////99dcxxxx,T0001,Johnny,V0,[Location and State],C152,070729 234015,0,0,FFFFFF0A,Not Apply#
            if (!string.IsNullOrEmpty(str))
            {
                string[] array = str.Substring(0, str.LastIndexOf("#")).Split(',');

                this.MdvrCoreId = array[1];   ////MDVR Core ID
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
                this.GpsValid = array[5];     ////GPS Valid
                this.Longitude = array[6];    ////Longitude
                this.Latitude = array[7];     ////Latitude
                this.Speed = array[8];        ////Speed
                this.Direction = array[9];    ////Direction

                this.OriginalCmd = array[20]; ////Original Cmd
                this.ReplyType = int.Parse(array[22]);
                this.ReplyResult = int.Parse(array[23]);

                this.Context = str;           ////Original  Context

                var gpsTime = ConvertHelper.ConvertStrToDate(array[4], "yyMMdd HHmmss");
                if (gpsTime != null)
                {
                    this.GpsTime = gpsTime.Value;     ////GPS Time
                }

                var originalTime = ConvertHelper.ConvertStrToDate(array[21], "yyMMdd HHmmss");
                if (originalTime != null)
                {
                    this.OriginalTime = originalTime.Value;     ////Original Time
                }

                if (this.ReplyResult == 0)
                {
                    this.ErrorNumber = array[27];

                    ////Can not Find Electric Fence
                    if (this.ErrorNumber == "00030031")
                    {
                        this.ReplyResult = 1;
                    }
                }
            }
        }

        public ElectricFenceReply()
        {
        }
        /// <summary>
        /// Success Flag	
        /// 0:fail
        /// 1:success
        /// </summary>
        [DataMember]
        public int ReplyResult { get; set; }

        /// <summary>
        /// Error Number
        /// </summary>
        [DataMember]
        public string ErrorNumber { get; set; }

        /// <summary>
        /// Context
        /// </summary>
        [DataMember]
        public string Context { get; set; }

        public string FenceID { get; set; }

        public AreaType AreaType { get; set; }
    }
}
