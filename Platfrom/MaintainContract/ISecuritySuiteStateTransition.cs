/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 22ddb6a6-8a78-4178-bdd1-81eb92e64399      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIUXP
/////                 Author: TEST(liuxp)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Contract
/////    Project Description:    
/////             Class Name: ISecuritySuiteStateTransition
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 10:52:25
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/8 10:52:25
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;


namespace Gsafety.PTMS.Maintain.Contract
{
    /// <summary>
    /// 安全套件状态转换接口
    /// </summary>
    public interface ISecuritySuiteStateTransition
    {
        /// <summary>
        /// 设置安全套件状态转换, 如果套件无法转换，则返回false，throw exception
        /// </summary>
        /// <param name="securitySuite"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [OperationContract]
        bool SetSecuritySuiteState(Gsafety.PTMS.BaseInformation.Contract.Data.DeviceSuite securitySuite, 
            Data.SecuritySuiteState from, Data.SecuritySuiteState to);

    }
}
