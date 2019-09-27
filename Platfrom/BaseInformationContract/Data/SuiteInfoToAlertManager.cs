/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 50fb384a-0db6-40c8-a526-ff4dec642dd4      
/////             clrversion: 4.0.30319.34011
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.Ant.BaseInformation.Contract.Data
/////    Project Description:    
/////             Class Name: SuiteInfoToAlertManager
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/2/21 05:22:20
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/2/21 05:22:20
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Ant.BaseInformation.Contract.Data
{
    public class SuiteInfoToAlertManager
    {
        /// <summary>
        /// 芯片号
        /// </summary>
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// ANTGPS号
        /// </summary>
        public string ANTGpsSn { get; set; }

        /// <summary>
        /// 安全套件号
        /// </summary>
        public string SuiteId { get; set; }

        /// <summary>
        /// 安全套件状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 区域代码
        /// </summary>
        public string DistrictCode { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string VehicleId { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public string CompanyID { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 安全套件表外键
        /// </summary>
        public string SuiteInfoID { get; set; }

        /// <summary>
        /// 安全是否在线
        /// </summary>
        public bool OnlineFlag { get; set; }
    }
}
