/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3ed80d60-9c0d-453e-9150-89bc0e20d64c      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.Ant.BaseInformation.Contract.Data
/////    Project Description:    
/////             Class Name: SetupStation
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 10:50:28
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/8 10:50:28
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Gsafety.Ant.BaseInformation.Contract.Data
{
    /// <summary>
    /// 安装点信息
    /// </summary>
    [DataContract]
    public class SetupStation
    {
        /// <summary>
        /// 安装点编号
        /// </summary>
        [DataMember]
        public string ID;

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name;

        /// <summary>
        /// 地址
        /// </summary>
        [DataMember]
        public string Address;

        /// <summary>
        /// 负责人
        /// </summary>
        [DataMember]
        public string Director;

        /// <summary>
        /// 负责人电话
        /// </summary>
        [DataMember]
        public string DirectorPhone;

        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember]
        public string Contact;

        /// <summary>
        /// 联系人电话
        /// </summary>
        [DataMember]
        public string ContactPhone;

        /// <summary>
        /// 邮箱
        /// </summary>
        [DataMember]
        public string Email;

        /// <summary>
        /// 行政区域
        /// </summary>
        [DataMember]
        public Region Region;

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Note;

        /// <summary>
        /// 省编码
        /// </summary>
        [DataMember]
        public string ProvinceCode;

        /// <summary>
        /// 省名称
        /// </summary>
        [DataMember]
        public string ProvinceName;

        /// <summary>
        /// 城市编码
        /// </summary>
        [DataMember]
        public string CityCode;

        /// <summary>
        /// 城市名称
        /// </summary>
        [DataMember]
        public string CityName;
    }
}
