/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4e200be3-4fdf-4138-b5fd-57863aa6536d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIUXP
/////                 Author: TEST(liuxp)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Contract
/////    Project Description:    
/////             Class Name: IMaintainService
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 10:08:11
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/8 10:08:11
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Maintain.Contract;
using Gsafety.PTMS.BaseInformation.Contract.Data;

namespace Gsafety.PTMS.Maintain.Contract
{  
    /// <summary>
    /// 维修流程服务
    /// </summary>
    public interface IMaintainService
    {
        /// <summary>
        /// 将套件报修
        /// </summary>
        /// <param name="securitySuite"></param>
        /// <returns></returns>
        [OperationContract]
        bool ReportToMaintainBySecuritySuite(DeviceSuite securitySuite);
        [OperationContract]
        bool ReportToMaintainByCarNumber(string carNumber);

        /// <summary>
        /// 维修中套件置成报废
        /// </summary>
        /// <param name="securitySuite"></param>
        /// <returns></returns>
        [OperationContract]
        bool ReportToInvalidateBySecuritySuite(DeviceSuite securitySuite);
        [OperationContract]
        bool ReportToInvalidateByCarNumber(string carNumber);

        /// <summary>
        /// 将维修中安全套件置成初始化
        /// </summary>
        /// <param name="securitySuite"></param>
        /// <returns></returns>
        [OperationContract]
        bool ReportToInitialBySecuritySuite(DeviceSuite securitySuite);
        [OperationContract]
        bool ReportToInitialByCarNumber(string carNumber);

        /// <summary>
        /// 将维修中特定套件置成运行（可选）
        /// </summary>
        /// <param name="securitySuite"></param>
        /// <returns></returns>
        [OperationContract]
        bool ReportToRunningBySecuritySuite(DeviceSuite securitySuite);
        [OperationContract]
        bool ReportToRunningByCarNumber(string carNumber);

        /// <summary>
        /// 获取所有维修中安全套件列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<DeviceSuite> GetMaintainLists();

    }
}
