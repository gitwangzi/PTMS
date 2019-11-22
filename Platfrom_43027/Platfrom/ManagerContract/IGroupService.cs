/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e40ba9c8-8022-4783-ad5e-07c0b77b6fae      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract
/////    Project Description:    
/////             Class Name: IGroupService
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/7/31 11:00:43
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/27 11:00:43
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Manager.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;

namespace Gsafety.PTMS.Manager.Contract
{
    /// <summary>
    /// Safe Group Service 
    /// </summary>
    [ServiceContract]
    public interface IGroupService
    {
        /// <summary>
        /// Add Safe Group
        /// </summary>
        /// <param name="group">Safe Group Name</param>
        /// <returns>Operation Result</returns>
        [OperationContract]
        SingleMessage<Boolean> AddGroup(string groupName);
        /// <summary>
        /// Delete Safe Group
        /// </summary>
        /// <param name="groupName">Safe Group Name</param>
        /// <returns>Operation Result</returns>
        [OperationContract]
        SingleMessage<Boolean> DeleteGroup(string groupName);
        /// <summary>
        /// Get All Safe Group Name
        /// </summary>
        /// <returns>Safe Group List</returns>
        [OperationContract]
        MultiMessage<String> GetAllGroupNames();
        /// <summary>
        /// Get All User Information In Group
        /// </summary>
        /// <param name="groupName">Group Name</param>
        /// <returns>User Information List</returns>
        [OperationContract]
        MultiMessage<ADAccountInfo> GetAccountInfoByGroupName(string groupName);
        /// <summary>
        /// Get User Information in Multile Safe Groups
        /// </summary>
        /// <param name="list">Group List </param>
        /// <returns>User Informaiton List</returns>
        [OperationContract]
        MultiMessage<ADAccountInfo> GetAccountInfoByGrouplist(List<String> list);

        /// <summary>
        /// Select By Safe Group ,User ID and User Name
        /// </summary>
        /// <param name="strGroupName"></param>
        /// <param name="loginName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<ADAccountInfo> GetAccountInfoByGroupAndUserLoginName(string strGroupName, string strLoginName, string strUserName);
    }
}
