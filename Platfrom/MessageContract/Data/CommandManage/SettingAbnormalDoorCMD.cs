/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4d1f198c-9308-437a-9752-22a00cab4860      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: SettingAbnormalDoorCMD
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/31 16:35:45
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/31 16:35:45
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
    [Serializable]
    [DataContract]
    public class SettingAbnormalDoorCMD : DownwardBase
    {
        [DataMember]
        public string SendValue;
        /// <summary>
        /// Setting time
        /// </summary>
        [DataMember]
        public DateTime SendTime;

        //public override string ToString()
        //{
        //    StringBuilder strCmd = new StringBuilder();
        //    strCmd.Append("99dcXXXX,")
        //       .Append(this.DvId)
        //       .Append(",")
        //       .Append(this.MsgId)
        //       .Append(",")
        //       .Append(CmType)
        //       .Append(",")
        //       .Append(this.SendTime.ToString("yyMMdd HHmmss"))
        //       .Append(",")
        //       .Append(SendValue)
        //       .Append("#");
        //    strCmd.Replace("XXXX", (strCmd.Length - 8).ToString("D5").Substring(1));
        //    return strCmd.ToString();
        //}

        public string ToString(RuleOperationType OperationType)
        {
            StringBuilder strCmd = new StringBuilder();
            strCmd.Append("99dcXXXX,")
               .Append(this.DvId)
               .Append(",")
               .Append(string.Format("{0}:{1}", this.MsgId, (int)OperationType))
               .Append(",")
               .Append(CmType)
               .Append(",")
               .Append(this.SendTime.ToString("yyMMdd HHmmss"))
               .Append(",")
               .Append(SendValue)
               .Append("#");
            strCmd.Replace("XXXX", (strCmd.Length - 8).ToString("D5").Substring(1));
            return strCmd.ToString();
        }
    }
}
