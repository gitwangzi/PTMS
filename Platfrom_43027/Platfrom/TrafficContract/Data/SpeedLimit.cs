/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b11f25fe-32b6-4361-96c9-04bf61b0be46      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Contract.Data
/////    Project Description:    
/////             Class Name: SpeedLimit
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/30 13:45:00
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/30 13:45:00
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Traffic.Contract.Data
{
    [DataContract]
    public class SpeedLimit
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string NAME { get; set; }
        [DataMember]
        public string VEHICLE_ID { get; set; }
        [DataMember]
        public decimal MAX_SPEED { get; set; }
        [DataMember]
        public decimal MIN_SPEED { get; set; }
        [DataMember]
        public int DURATION { get; set; }
        [DataMember]
        public DateTime START_TIME { get; set; }
        [DataMember]
        public System.DateTime END_TIME { get; set; }
        [DataMember]
        public DateTime CreateTime { get; set; }
        [DataMember]
        public string Creator { get; set; }
        [DataMember]
        public short? Valid { get; set; }
        [DataMember]
        public int SpeedVehicleCount { get; set; }
        [DataMember]
        public bool IsUsed { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(NAME)))
            {
                builder.AppendLine("NAME:" + NAME.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VEHICLE_ID)))
            {
                builder.AppendLine("VEHICLE_ID:" + VEHICLE_ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MAX_SPEED)))
            {
                builder.AppendLine("MAX_SPEED:" + MAX_SPEED.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MIN_SPEED)))
            {
                builder.AppendLine("MIN_SPEED:" + MIN_SPEED.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DURATION)))
            {
                builder.AppendLine("DURATION:" + DURATION.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(START_TIME)))
            {
                builder.AppendLine("START_TIME:" + START_TIME.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(END_TIME)))
            {
                builder.AppendLine("END_TIME:" + END_TIME.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Creator)))
            {
                builder.AppendLine("Creator:" + Creator.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Valid)))
            {
                builder.AppendLine("Valid:" + Valid.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SpeedVehicleCount)))
            {
                builder.AppendLine("SpeedVehicleCount:" + SpeedVehicleCount.ToString());
            }
            return builder.ToString();
        }

    }
}
