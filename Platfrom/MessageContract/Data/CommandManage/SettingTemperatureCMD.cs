using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Enum;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: a131fd6e-5181-46aa-8749-89117da25099      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: SettingTemperatureCMD
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/31 15:13:48
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/31 15:13:48
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
    public class SettingTemperatureCMD : DownwardBase
    {
        [DataMember]
        public TemperatureMarkType SendType;

        [DataMember]
        public int? TemperatureType;

        [DataMember]
        public decimal? MinValue;

        [DataMember]
        public decimal? MaxValue;

        [DataMember]
        public DateTime SendTime;

        [DataMember]
        public decimal? Second;

        //public override string ToString()
        //{
        //    StringBuilder strCmd = new StringBuilder();
        //    strCmd.Append("99dcXXXX,")
        //    .Append(this.DvId)
        //        .Append(",")
        //        .Append(this.MsgId)
        //        .Append(",")
        //        .Append(CmType)
        //        .Append(",")
        //        .Append(this.SendTime.ToString("yyMMdd HHmmss"))
        //        .Append(",")
        //        .Append((int)SendType)
        //        .Append(",")
        //        .Append(TemperatureType)
        //        .Append(",")
        //        .Append(MinValue.HasValue ? MinValue.Value.ToString("F2") : "")
        //        .Append(",")
        //        .Append(MaxValue.HasValue ? MaxValue.Value.ToString("F2") : "")
        //        .Append("#");
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
                .Append((int)SendType)
                .Append(",")
                .Append(TemperatureType)
                .Append(",")
                .Append(MinValue.HasValue ? MinValue.Value.ToString("F2") : "")
                .Append(",")
                .Append(MaxValue.HasValue ? MaxValue.Value.ToString("F2") : "")
                .Append(",")
                .Append(Second.HasValue?Second.Value.ToString():"1")
                .Append("#");
            strCmd.Replace("XXXX", (strCmd.Length - 8).ToString("D5").Substring(1));
            return strCmd.ToString();
        }
    }
}
