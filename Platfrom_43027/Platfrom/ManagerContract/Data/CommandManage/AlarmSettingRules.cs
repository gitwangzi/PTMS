/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: f9d3a055-b95c-431e-959f-8dc8cd0a381f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: AlarmSettingRules
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/20 13:38:39
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/20 13:38:39
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Manager.Contract.Data.CommandManage
{
   [DataContract]
    public class AlarmSettingRules
    {
        ///<summary>
        ///RuleID
        ///</summary>
        [DataMember]
       public string Alarm_RuleID;

       ///<summary>
       ///RuleName
       ///</summary>
       [DataMember]
       public string Alarm_RuleName;
        ///<summary>
        ///ButtonTime
        ///</summary>
       [DataMember]
       public short? Alarm_ButtonTime;
       ///<summary>
       ///NormalAlarm
       ///</summary>
        [DataMember]
       public int? Alarm_Normal;
       ///<summary>
       ///Creator
       ///</summary>
        [DataMember]
       public string Alarm_Creator;
       ///<summmary>
       ///CreateTime
       ///</summary>
        [DataMember]
       public DateTime? Alarm_CreateTime;
       ///<summary>
       ///Description
       ///</summary>
        [DataMember]
       public string Alarm_Description;

        ///<summary>
        ///Alarm_VehcileCount
        ///</summary>
        [DataMember]
        public int Alarm_VehcileCount;

        [DataMember]
        public short Alarm_IsDefault;

        [DataMember]
        public short Alarm_Valid;

       public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
                      if (!string.IsNullOrEmpty(Convert.ToString( Alarm_RuleID)))
            {
                builder.AppendLine(" Alarm_RuleID:" +  Alarm_RuleID.ToString());
            }
                     
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_RuleName)))
            {
                builder.AppendLine("Alarm_RuleName:" + Alarm_RuleName.ToString());
            }
                     
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_ButtonTime)))
            {
                builder.AppendLine("Alarm_ButtonTime:" + Alarm_ButtonTime.ToString());
            }
                     
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_Normal)))
            {
                builder.AppendLine("Alarm_Normal:" + Alarm_Normal.ToString());
            }
                     
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_Creator)))
            {
                builder.AppendLine("Alarm_Creator:" + Alarm_Creator.ToString());
            }
                     
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_CreateTime)))
            {
                builder.AppendLine("Alarm_CreateTime:" + Alarm_CreateTime.ToString());
            }
                     
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_Description)))
            {
                builder.AppendLine("Alarm_Description:" + Alarm_Description.ToString());
            }
                     
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_VehcileCount)))
            {
                builder.AppendLine("Alarm_VehcileCount:" + Alarm_VehcileCount.ToString());
            }
                     
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_IsDefault)))
            {
                builder.AppendLine("Alarm_IsDefault:" + Alarm_IsDefault.ToString());
            }
                     
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_Valid)))
            {
                builder.AppendLine("Alarm_Valid:" + Alarm_Valid.ToString());
            }

            return builder.ToString();
        }

    }
}
