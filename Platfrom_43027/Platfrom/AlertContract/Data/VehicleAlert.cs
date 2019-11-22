/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 12828b86-475e-4248-8d23-9751c41aeb44      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Contract
/////    Project Description:    
/////             Class Name: VehicleAlertSimple
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/20 10:00:26
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/26 10:00:26
/////            Modified by: BilongLiu
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
    public class VehicleAlert
    {
        /// <summary>
        /// ALERT ID（ PRIMARY KEY）
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string MdvrCoreId { get; set; }
        
        [DataMember]
        public string VehicleId { get; set; }
        
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
        public string Speed { get; set; }
        
        [DataMember]
        public string Direction { get; set; }
        
        [DataMember]
        public string GpsValid { get; set; }
        
        [DataMember]
        public int Status { get; set; }
        
        [DataMember]
        public string Province { get; set; }
        
        [DataMember]
        public string City { get; set; }

        [DataMember]
        public int VehicleType { get; set; }
        
        [DataMember]
        public string VehicleOwner { get; set; }
        
        [DataMember]
        public string Owner_Phone { get; set; }

        /// <summary>
        /// CMD TYPE
        /// </summary>
        [DataMember]
        public string Cmd { get; set; }

        /// <summary>
        /// SUITEINFOID（PRIMARY KET）
        /// </summary>
        [DataMember]
        public string SuiteInfoId { get; set; }

        /// <summary>
        /// WHETHER ALERT HAPPENED PLACE
        /// </summary>
        [DataMember]
        public bool? IsMonitor { get; set; }

        /// <summary>
        /// WHETHER NEAREST PLACE
        /// </summary>
        [DataMember]
        public bool? IsLocation { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Id)))
            {
                builder.AppendLine("Id:" + Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
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
            if (!string.IsNullOrEmpty(Convert.ToString(GpsValid)))
            {
                builder.AppendLine("GpsValid:" + GpsValid.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Province)))
            {
                builder.AppendLine("Province:" + Province.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(City)))
            {
                builder.AppendLine("City:" + City.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleType)))
            {
                builder.AppendLine("VehicleType:" + VehicleType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleOwner)))
            {
                builder.AppendLine("VehicleOwner:" + VehicleOwner.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Owner_Phone)))
            {
                builder.AppendLine("Owner_Phone:" + Owner_Phone.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Cmd)))
            {
                builder.AppendLine("Cmd:" + Cmd.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteInfoId)))
            {
                builder.AppendLine("SuiteInfoId:" + SuiteInfoId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IsMonitor)))
            {
                builder.AppendLine("IsMonitor:" + IsMonitor.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IsLocation)))
            {
                builder.AppendLine("IsLocation:" + IsLocation.ToString());
            }
            return builder.ToString();
        }

    }
}
