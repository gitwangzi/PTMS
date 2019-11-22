/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0541e157-89d1-49ad-a387-36bf9cb6ad94      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(BilongLiu)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Monitor.Contract
/////    Project Description:    
/////             Class Name: AlertInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/1 15:34:43
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/1 15:34:43
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Gsafety.PTMS.Monitor.Contract.Data
{
    [DataContract]
    public class VedioFile
    {
        [DataMember]
        public string VehicleId { get; set; }

        [DataMember]
        public string MdvrCoreId { get; set; }

        [DataMember]
        public string FileId { get; set; }

        [DataMember]
        public int FileSize { get; set; }

        [DataMember]
        public int? Flag { get; set; }

        [DataMember]
        public DateTime StartTime { get; set; }

        [DataMember]
        public DateTime EndTime { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FileId)))
            {
                builder.AppendLine("FileId:" + FileId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FileSize)))
            {
                builder.AppendLine("FileSize:" + FileSize.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Flag)))
            {
                builder.AppendLine("Flag:" + Flag.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StartTime)))
            {
                builder.AppendLine("StartTime:" + StartTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(EndTime)))
            {
                builder.AppendLine("EndTime:" + EndTime.ToString());
            }
            return builder.ToString();
        }

    }
}
