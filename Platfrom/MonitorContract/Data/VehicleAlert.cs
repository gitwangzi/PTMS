/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0541e157-89d1-49ad-a387-36bf9cb6ad94      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
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
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Monitor.Contract.Data
{
    [DataContract]
    public class VehicleAlert
    {

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string VehicleId { get; set; }

        [DataMember]
        public string SuiteInfoId { get; set; }

        [DataMember]
        public string MdvrCoreId { get; set; }

        [DataMember]
        public int? AlertType { get; set; }

        [DataMember]
        public DateTime? AlertTime { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public DateTime? GpsTime { get; set; }

        [DataMember]
        public double? Speed { get; set; }

        [DataMember]
        public string Direction { get; set; }

        [DataMember]
        public int? Status { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Id)))
            {
                builder.AppendLine("Id:" + Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteInfoId)))
            {
                builder.AppendLine("SuiteInfoId:" + SuiteInfoId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlertType)))
            {
                builder.AppendLine("AlertType:" + AlertType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlertTime)))
            {
                builder.AppendLine("AlertTime:" + AlertTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Longitude)))
            {
                builder.AppendLine("Longitude:" + Longitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Latitude)))
            {
                builder.AppendLine("Latitude:" + Latitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GpsTime)))
            {
                builder.AppendLine("GpsTime:" + GpsTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Speed)))
            {
                builder.AppendLine("Speed:" + Speed.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Direction)))
            {
                builder.AppendLine("Direction:" + Direction.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            return builder.ToString();
        }

    }
}
