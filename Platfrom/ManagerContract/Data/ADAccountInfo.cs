/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: f20e4bb1-e58d-43b5-8eaf-dd920e5cb769      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract.Data;
/////    Project Description:    
/////             Class Name: ADAccountInfoModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/7/31 10:16:21
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/7/31 10:16:21
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Gsafety.PTMS.Base.Contract.Data;

namespace Gsafety.PTMS.Manager.Contract.Data
{
    /// <summary>
    /// AD Info
    /// </summary>
    [DataContract]
    public class ADAccountInfo
    {
        /// <summary>
        /// UserName
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// DisplayName
        /// </summary>
        [DataMember]
        public string DisplayName { get; set; }
        /// <summary>
        /// UserPassword
        /// </summary>
        [DataMember]
        public string UserPassword { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        [DataMember]
        public string Phone { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// SecurityGroup
        /// </summary>
        [DataMember]
        public string SecurityGroup { get; set; }
        /// <summary>
        /// UserLoginName
        /// </summary>
        [DataMember]
        public string UserLoginName { get; set; }
        /// <summary>
        /// OrgCode
        /// </summary>
        [DataMember]
        public string OrgCode { get; set; }
        /// <summary>
        /// OrgName
        /// </summary>
        [DataMember]
        public string OrgName { get; set; }
        /// <summary>
        /// Company
        /// </summary>
        [DataMember]
        public string Company { get; set; }
        /// <summary>
        /// ProvinceCode
        /// </summary>
        [DataMember]
        public string ProvinceCode { get; set; }
        /// <summary>
        /// ProvinceName
        /// </summary>
        [DataMember]
        public string ProvinceName { get; set; }
        /// <summary>
        /// CityCode
        /// </summary>
        [DataMember]
        public string CityCode { get; set; }
        /// <summary>
        /// CityName
        /// </summary>
        [DataMember]
        public string CityName { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [DataMember]
        public string Email { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        [DataMember]
        public string Address { get; set; }
        /// <summary>
        /// Fax
        /// </summary>
        [DataMember]
        public string Fax { get; set; }
        /// <summary>
        /// PostalCode
        /// </summary>
        [DataMember]
        public string PostalCode { get; set; }
        /// <summary>
        /// Level
        /// </summary>
        [DataMember]
        public int Level { get; set; }
        /// <summary>
        /// ManagedRegionCode
        /// </summary>
        [DataMember]
        public string ManagedRegionCode { get; set; }

        [DataMember]
        public UserInfoMessageHeader UserInfo { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(UserName)))
            {
                builder.AppendLine("UserName:" + UserName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DisplayName)))
            {
                builder.AppendLine("DisplayName:" + DisplayName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserPassword)))
            {
                builder.AppendLine("UserPassword:" + UserPassword.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Phone)))
            {
                builder.AppendLine("Phone:" + Phone.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Description)))
            {
                builder.AppendLine("Description:" + Description.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SecurityGroup)))
            {
                builder.AppendLine("SecurityGroup:" + SecurityGroup.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserLoginName)))
            {
                builder.AppendLine("UserLoginName:" + UserLoginName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OrgCode)))
            {
                builder.AppendLine("OrgCode:" + OrgCode.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OrgName)))
            {
                builder.AppendLine("OrgName:" + OrgName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Company)))
            {
                builder.AppendLine("Company:" + Company.ToString());
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
            if (!string.IsNullOrEmpty(Convert.ToString(Email)))
            {
                builder.AppendLine("Email:" + Email.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Address)))
            {
                builder.AppendLine("Address:" + Address.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Fax)))
            {
                builder.AppendLine("Fax:" + Fax.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(PostalCode)))
            {
                builder.AppendLine("PostalCode:" + PostalCode.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Level)))
            {
                builder.AppendLine("Level:" + Level.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ManagedRegionCode)))
            {
                builder.AppendLine("ManagedRegionCode:" + ManagedRegionCode.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserInfo)))
            {
                builder.AppendLine("UserInfo:" + UserInfo.ToString());
            }
            return builder.ToString();
        }

    }
}
