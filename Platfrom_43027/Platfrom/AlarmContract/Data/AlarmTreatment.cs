/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: cec477c6-2a50-464e-a23b-fc4def3c405a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract.Data
/////    Project Description:    
/////             Class Name: AlarmTreatment
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 11:09:31
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/24 11:09:31
/////            Modified by: BilongLiu
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Alarm.Contract.Data
{
    [DataContract]
    public class AlarmTreatment
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string AlarmId { get; set; }

        [DataMember]
        public string DisposeStaff { get; set; }

        [DataMember]
        public Nullable<DateTime> DisposeTime { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public int? IsTrueAlarm { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Id)))
            {
                builder.AppendLine("Id:" + Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmId)))
            {
                builder.AppendLine("AlarmId:" + AlarmId.ToString());
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
            if (!string.IsNullOrEmpty(Convert.ToString(IsTrueAlarm)))
            {
                builder.AppendLine("IsTrueAlarm:" + IsTrueAlarm.ToString());
            }
            return builder.ToString();
        }
    }
}
