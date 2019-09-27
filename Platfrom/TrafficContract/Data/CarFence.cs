/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7a324bb9-67e4-4e82-847a-1fb2c2b5a760      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Contract
/////    Project Description:    
/////             Class Name: CarFence
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/1 15:45:30
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/1 15:45:30
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Traffic.Contract
{
    [DataContract]
    public class CarFence
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string CarNumber { get; set; }

        [DataMember]
        public string FenceId { get; set; }

        [DataMember]
        public short? Status { get; set; }

        [DataMember]
        public string MDVR_SN_CODE { get; set; }

        [DataMember]
        public DateTime? CreateTime { get; set; }

        [DataMember]
        public string CreateUser { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CarNumber)))
            {
                builder.AppendLine("CarNumber:" + CarNumber.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FenceId)))
            {
                builder.AppendLine("FenceId:" + FenceId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MDVR_SN_CODE)))
            {
                builder.AppendLine("MDVR_SN_CODE:" + MDVR_SN_CODE.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateUser)))
            {
                builder.AppendLine("CreateUser:" + CreateUser.ToString());
            }
            return builder.ToString();
        }

    }
}
