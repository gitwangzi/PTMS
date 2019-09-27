/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a3dd934c-aa33-43cc-869a-1d4ae4d4345f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Maintain
/////    Project Description:    
/////             Class Name: UpgradeStatus
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/28 10:38:32
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/28 10:38:32
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
    public class UpgradeStatusCMD : DownwardBase
    {
        [DataMember]
        public DateTime SendTime { get; set; }

        /// <summary>
        /// Convert To CMD
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder strCmd = new StringBuilder();

            strCmd.Append("99dcXXXX,")
                .Append(this.DvId)
                .Append(",")
                .Append(this.MsgId)
                .Append(",")
                .Append("C602")
                .Append(",")
                .Append(this.SendTime.ToString("yyMMdd HHmmss"))
                .Append("#");
            ////Replace Length,Reduce 8 bytes(99dcxxxx)
            strCmd.Replace("XXXX", (strCmd.Length - 8).ToString("D5").Substring(1));
            return strCmd.ToString();
        }
    }
}
