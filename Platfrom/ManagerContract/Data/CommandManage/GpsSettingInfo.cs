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
/////             Class Name: GpsSettingInfo
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
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Manager.Contract.Data.CommandManage
{
   [DataContract]
    public class GpsSettingInfo
    {
        ///<summary>
        ///Gps_RuleID
        ///</summary>
        [DataMember]
        public string Gps_RuleID;

        ///<summary>
        ///Gps_RuleName
        ///</summary>
        [DataMember]
       public string Gps_RuleName;
        ///<summary>
        ///Gps_IfMonitor
        ///</summary>
        [DataMember]
        public short? Gps_IfMonitor;
        ///<summary>
        ///Gps_UploadSum
        ///</summary>
        [DataMember]
        public int? Gps_UploadSum;
        ///<summary>
        ///Gps_UploadType
        ///</summary>
        [DataMember]
        public short? Gps_UploadType;
        ///<summary>
        ///Gps_CreateTime
        ///</summary>
        [DataMember]
        public DateTime? Gps_CreateTime;
        ///<summary>
        ///Gps_RuleName
        ///</summary>
        [DataMember]
        public string Gps_Creator;
        ///<summary>
        ///Gps_RuleName
        ///</summary>
        [DataMember]
        public string Gps_Description;
        ///Gps_RuleName
        ///</summary>
        [DataMember]
        public int? Gps_Distance;
        ///Gps_RuleName
        ///</summary>
        [DataMember]
        public int? Gps_Time;

        ///Gps_VehicleCount
        ///</summary>
        [DataMember]
        public int Gps_VehicleCount;

        [DataMember]
        public short Gps_IsDefault;

        [DataMember]
        public short Gps_Valid;

            public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_RuleID)))
            {
                builder.AppendLine("Gps_RuleID:" + Gps_RuleID.ToString());
            }
            
           
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_RuleName)))
            {
                builder.AppendLine("Gps_RuleName:" + Gps_RuleName.ToString());
            }
            
           
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_IfMonitor)))
            {
                builder.AppendLine("Gps_IfMonitor:" + Gps_IfMonitor.ToString());
            }
            
           
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_UploadSum)))
            {
                builder.AppendLine("Gps_UploadSum:" + Gps_UploadSum.ToString());
            }
            
           
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_UploadType)))
            {
                builder.AppendLine("Gps_UploadType:" + Gps_UploadType.ToString());
            }
            
           
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_CreateTime)))
            {
                builder.AppendLine("Gps_CreateTime:" + Gps_CreateTime.ToString());
            }
            
           
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_Creator)))
            {
                builder.AppendLine("Gps_Creator:" + Gps_Creator.ToString());
            }
            
           
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_Description)))
            {
                builder.AppendLine("Gps_Description:" + Gps_Description.ToString());
            }
            
           
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_Distance)))
            {
                builder.AppendLine("Gps_Distance:" + Gps_Distance.ToString());
            }
            
           
            if (!string.IsNullOrEmpty(Convert.ToString( Gps_Time)))
            {
                builder.AppendLine(" Gps_Time:" +  Gps_Time.ToString());
            }
            
           
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_VehicleCount)))
            {
                builder.AppendLine("Gps_VehicleCount:" + Gps_VehicleCount.ToString());
            }
            
           
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_IsDefault)))
            {
                builder.AppendLine("Gps_IsDefault:" + Gps_IsDefault.ToString());
            }
            
           
            if (!string.IsNullOrEmpty(Convert.ToString( Gps_Valid)))
            {
                builder.AppendLine(" Gps_Valid:" +  Gps_Valid.ToString());
            }
            return builder.ToString();
        }  

    }
}
