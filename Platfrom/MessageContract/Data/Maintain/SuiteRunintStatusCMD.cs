/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 53120e17-aa75-46e2-85a5-86f471664bef      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Maintain
/////    Project Description:    
/////             Class Name: TransparentInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/28 10:51:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/28 10:51:14
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

namespace Gsafety.PTMS.Message.Contract.Data
{
    [DataContract]
    [Serializable]
    public class SuiteRunintStatusCMD : DownwardBase
    {
        [DataMember]
        public string SuiteRunintStatusID { get; set; }

        [DataMember]
        public DateTime SendTime { get; set; }

        [DataMember]
        public string VehicleId { get; set; }

        /// <summary>
        /// Context
        /// </summary>
        [DataMember]
        public string Context { get; set; }

        /// <summary>
        /// Convert Model to Command
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //this.SuiteRunintStatusID = Guid.NewGuid().ToString();
            StringBuilder strCmd = new StringBuilder();

            strCmd.Append("99dcXXXX,")
                .Append(this.DvId)
                .Append(",")
                .Append(this.MsgId)
                .Append(",")
                .Append("C601")
                .Append(",")
                .Append(this.SendTime.ToString("yyMMdd HHmmss"))
                .Append(",")
                .Append("4")//touchuan Message Type, ASCII code Integer,1:meter,2: Ad Screen,3:Install the Acceptance Test,4:Operation Information 5:Upgrade Result
                .Append(",")
                .Append(this.SuiteRunintStatusID)   ////Primary Key ID
                .Append("#");
            ////Replace Length,Reduce 8 bytes(99dcxxxx)
            strCmd.Replace("XXXX", (strCmd.Length - 8).ToString("D5").Substring(1));
            this.Context = strCmd.ToString();
            return strCmd.ToString();
        }
    }
}
