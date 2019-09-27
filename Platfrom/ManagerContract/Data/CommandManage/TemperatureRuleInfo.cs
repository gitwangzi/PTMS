/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 77f884a3-be05-4109-a7fa-f2d0e7ca4276      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: TemperatureRuleInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/30 13:45:11
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/30 13:45:11
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Manager.Contract.Data.CommandManage
{
    [DataContract]
    public class TemperatureRuleInfo : RuleBaseInfo
    {
        [DataMember]
        public TemperatureSettingType SettingType { get; set; }
        [DataMember]
        public short? TemperatureType { get; set; }
        [DataMember]
        public decimal? LowTemperature { get; set; }
        [DataMember]
        public decimal? HighTemperature { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(SettingType)))
            {
                builder.AppendLine("SettingType:" + SettingType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(TemperatureType)))
            {
                builder.AppendLine("TemperatureType:" + TemperatureType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(LowTemperature)))
            {
                builder.AppendLine("LowTemperature:" + LowTemperature.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(HighTemperature)))
            {
                builder.AppendLine("HighTemperature:" + HighTemperature.ToString());
            }
            return builder.ToString();
        }

    }
}
