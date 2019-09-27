/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 17087896-09c3-45cb-ac11-5ef204a65860      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract.Data
/////    Project Description:    
/////             Class Name: InstallLogInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 7/31/2013 3:24:31 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/31/2013 3:24:31 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Manager.Contract.Data
{
    [DataContract]
    public class InstallLogInfo
    {
        [DataMember]
        public string RecordStaff { get; set; }

        [DataMember]
        public string SetupStaff { get; set; }

        [DataMember]
        public DateTime InstalledTime { get; set; }

        [DataMember]
        public DateTime StartTime { get; set; }

        [DataMember]
        public string Vechicle_ID { get; set; }

        [DataMember]
        public string SuiteID { get; set; }

        [DataMember]
        public string SetupStation { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(RecordStaff)))
            {
                builder.AppendLine("RecordStaff:" + RecordStaff.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SetupStaff)))
            {
                builder.AppendLine("SetupStaff:" + SetupStaff.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(InstalledTime)))
            {
                builder.AppendLine("InstalledTime:" + InstalledTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StartTime)))
            {
                builder.AppendLine("StartTime:" + StartTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Vechicle_ID)))
            {
                builder.AppendLine("Vechicle_ID:" + Vechicle_ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteID)))
            {
                builder.AppendLine("SuiteID:" + SuiteID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SetupStation)))
            {
                builder.AppendLine("SetupStation:" + SetupStation.ToString());
            }
            return builder.ToString();
        }

    }
}
