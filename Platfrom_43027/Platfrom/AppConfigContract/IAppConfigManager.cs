/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ebe8e57b-7f1d-449c-b241-a8ae8d3d0a45      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGJINCAI
/////                 Author: TEST(JinCaiWang)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract.Data
/////    Project Description:    
/////             Class Name: AlarmInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 11:07:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/8 11:07:41
/////            Modified by:
/////   Modified Description: 
/////======================================================================

using Gsafety.PTMS.AppConfig.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Gsafety.PTMS.AppConfig.Contract
{
  
    [ServiceContract]
    public interface IAppConfigManager
    {
        /// <summary>
        /// Get All Section Config
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<AppConfigModel> GetAllSections();

        /// <summary>
        /// Get Section Config By Section
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [OperationContract]
        Dictionary<string,string> GetSectionByName(string name);

        /// <summary>
        /// Add Section
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        [OperationContract]
        void AddSection(AppConfigModel section);

        /// <summary>
        /// Update Section Config
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        [OperationContract]
        void  UpdateSection(AppConfigModel section);

        /// <summary>
        /// Delete Section Config
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        [OperationContract]
        void  DeleteSection(AppConfigModel section);

        /// <summary>
        /// Delete Section Config By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        void  DeleteSectionById(string id);

        /// <summary>
        /// Check Name Is Valid
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="parentName"></param>
        /// <returns></returns>
        [OperationContract]
        bool IsValidName(string id,string name, string parentName);
    }
}
