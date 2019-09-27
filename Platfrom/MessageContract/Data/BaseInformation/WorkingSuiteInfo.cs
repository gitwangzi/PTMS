/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3ca957e2-f40e-4137-b5e8-0474e4bbee56      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Contract.Data
/////    Project Description:    
/////             Class Name: SuiteInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/23 16:10:49
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/23 16:10:49
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// Working Suite Information
    /// </summary>
    [DataContract]
    [Serializable]
    public class WorkingSuiteInfo
    {
        [DataMember]
        public virtual string ClientId { get; set; }

        [DataMember]
        public virtual string OrgnizationId { get; set; }

        /// <summary>
        /// Mdvr Core Id
        /// </summary>
        [DataMember]
        public virtual string MdvrCoreId { get; set; }


        /// <summary>
        /// Suite Id
        /// </summary>
        public string SuiteId { get; set; }

        /// <summary>
        /// Safety Suite Status
        /// </summary>
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// District Code
        /// </summary>
        [DataMember]
        public string DistrictCode { get; set; }

        /// <summary>
        /// Vehicle Id
        /// </summary>
        [DataMember]
        public string VehicleId { get; set; }

        /// <summary>
        /// Province Code
        /// </summary>
        [DataMember]
        public string ProvinceCode { get; set; }

        /// <summary>
        /// Province Name
        /// </summary>
        [DataMember]
        public string ProvinceName { get; set; }

        /// <summary>
        /// City Code
        /// </summary>
        [DataMember]
        public string CityCode { get; set; }

        /// <summary>
        /// City Name
        /// </summary>
        [DataMember]
        public string CityName { get; set; }

        /// <summary>
        /// Suite Info ID
        /// </summary>
        [DataMember]
        public string SuiteInfoID { get; set; }

        /// <summary>
        /// Vehicle Type
        /// </summary>
        [DataMember]
        public int VehicleType { get; set; }

        /// <summary>
        /// Vehicle Sn ID 
        /// </summary>
        [DataMember]
        public string VehicleSn { get; set; }

        /// <summary>
        /// Brand Model
        /// </summary>
        [DataMember]
        public string BrandModel { get; set; }

        /// <summary>
        /// Mobile Number
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }

        /// <summary>
        /// Operation Lincese
        /// </summary>
        [DataMember]
        public string OperationLincese { get; set; }

        /// <summary>
        /// Note
        /// </summary>
        [DataMember]
        public string Note { get; set; }

        /// <summary>
        /// Owner Name
        /// </summary>
        [DataMember]
        public string Owner { get; set; }

        /// <summary>
        /// Start Year
        /// </summary>
        [DataMember]
        public string StartYear { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteId)))
            {
                builder.AppendLine("SuiteId:" + SuiteId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DistrictCode)))
            {
                builder.AppendLine("DistrictCode:" + DistrictCode.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ProvinceCode)))
            {
                builder.AppendLine("ProvinceCode:" + ProvinceCode.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ProvinceName)))
            {
                builder.AppendLine("ProvinceName:" + ProvinceName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CityCode)))
            {
                builder.AppendLine("CityCode:" + CityCode.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CityName)))
            {
                builder.AppendLine("CityName:" + CityName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteInfoID)))
            {
                builder.AppendLine("SuiteInfoID:" + SuiteInfoID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleType)))
            {
                builder.AppendLine("VehicleType:" + VehicleType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleSn)))
            {
                builder.AppendLine("VehicleSn:" + VehicleSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(BrandModel)))
            {
                builder.AppendLine("BrandModel:" + BrandModel.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Mobile)))
            {
                builder.AppendLine("Mobile:" + Mobile.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OperationLincese)))
            {
                builder.AppendLine("OperationLincese:" + OperationLincese.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Note)))
            {
                builder.AppendLine("Note:" + Note.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Owner)))
            {
                builder.AppendLine("Owner:" + Owner.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StartYear)))
            {
                builder.AppendLine("StartYear:" + StartYear.ToString());
            }
            return builder.ToString();
        }

    }
}
