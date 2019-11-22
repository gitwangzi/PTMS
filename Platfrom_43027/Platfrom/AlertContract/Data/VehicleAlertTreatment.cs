/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: cec477c6-2a50-464e-a23b-fc4def3c405a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(BilongLiu)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract.Data
/////    Project Description:    
/////             Class Name: AlarmTreatment
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/26 11:09:31
/////      Class Description:  
/////======================================================================
/////          Modified Time: 
/////            Modified by: 
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Alert.Contract.Data
{
    [DataContract]
    public class VehicleAlertTreatment
    {
        [DataMember]
        public string MDVRID { get; set; }
        
        [DataMember]
        public int Alerttype { get; set; }
        
        [DataMember]
        public string AlertId { get; set; }
        
        [DataMember]
        public string DisposeStaff { get; set; }
        
        [DataMember]
        public string Content { get; set; }
        
        [DataMember]
        public DateTime AlertTime { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(MDVRID)))
            {
                builder.AppendLine("MDVRID:" + MDVRID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Alerttype)))
            {
                builder.AppendLine("Alerttype:" + Alerttype.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlertId)))
            {
                builder.AppendLine("AlertId:" + AlertId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DisposeStaff)))
            {
                builder.AppendLine("DisposeStaff:" + DisposeStaff.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Content)))
            {
                builder.AppendLine("Content:" + Content.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlertTime)))
            {
                builder.AppendLine("AlertTime:" + AlertTime.ToString());
            }
            return builder.ToString();
        }

    }
    	
}
