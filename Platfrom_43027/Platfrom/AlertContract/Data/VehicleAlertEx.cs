/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1aa14f10-be26-4f19-a86a-46d66c0a4549      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Contract.Data
/////    Project Description:    
/////             Class Name: VehicleAlertEx
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/17 16:12:51
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/17 16:12:51
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Alert.Contract.Data
{
    [DataContract]
    public class VehicleAlertEx:VehicleAlert
    {
        
        [DataMember]
        public new string Id { get; set; }//jj add new 
        
        [DataMember]
        public string AlertId { get; set; }
        
        [DataMember]
        public string DisposeStaff { get; set; }
        
        [DataMember]
        public Nullable<DateTime> DisposeTime { get; set; }
        
        [DataMember]
        public string Content { get; set; }
        
        [DataMember]
        public string BUSINESS_ALERT_ID { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Id)))
            {
                builder.AppendLine("Id:" + Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlertId)))
            {
                builder.AppendLine("AlertId:" + AlertId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DisposeStaff)))
            {
                builder.AppendLine("DisposeStaff:" + DisposeStaff.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DisposeTime)))
            {
                builder.AppendLine("DisposeTime:" + DisposeTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Content)))
            {
                builder.AppendLine("Content:" + Content.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(BUSINESS_ALERT_ID)))
            {
                builder.AppendLine("BUSINESS_ALERT_ID:" + BUSINESS_ALERT_ID.ToString());
            }
            return builder.ToString();
        }

    }
}
