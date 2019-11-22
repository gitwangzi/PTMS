/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 52b9efbf-5841-42e9-b790-8b7e6bd7d651      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract
/////    Project Description:    
/////             Class Name: IADUserInfoService
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/7/31 10:32:27
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/27 10:32:27
/////            Modified by: BilongLiu
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
    /// Administrator Management
    /// </summary>
    [ServiceContract]
    public interface IADAccountService
    {
        /// <summary>
        /// Create User 
        /// </summary>
        /// <param name="account">Account Information</param>
        /// <returns>Operation Result</returns>
        [OperationContract]
       SingleMessage<Boolean> AddAccount(ADAccountInfo account);
        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="account">Account Information</param>
        /// <returns>Operation Result</returns>
        [OperationContract]
        SingleMessage<Boolean> UpdateAccount(ADAccountInfo account);
        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="accountName">Account Name</param>
        /// <returns>Operation Result</returns>
        [OperationContract]
        SingleMessage<Boolean> DeleteAccount(String accountName);
        /// <summary>
        /// Get User Information
        /// </summary>
        /// <param name="accountName">Account Name</param>
        /// <returns>Operation Result</returns>
        [OperationContract]
        SingleMessage<ADAccountInfo> GetAccount(String accountName);
        /// <summary>
        /// Valid User Information
        /// </summary>
        /// <param name="accountName">Account Info</param>
        /// <param name="password">Password</param>
        /// <returns>Account Information</returns>
        [OperationContract]
        SingleMessage<ADAccountInfo> ValidateUser(string accountName, string password);
        /// <summary>
        /// User Is Exist
        /// </summary>
        /// <param name="accountName">Account Name</param>
        /// <returns>true : Exist, false : Not Exist</returns>
        [OperationContract]
        SingleMessage<Boolean> IsUserExits(string accountName);
        /// <summary>
        /// Active User
        /// </summary>
        /// <param name="accountName">Account Name</param>
        /// <returns>Operation Result</returns>
        [OperationContract]
        SingleMessage<Boolean> EnableAccount(String accountName);
        /// <summary>
        ///Disable User
        /// </summary>
        /// <param name="accountName">Account</param>
        /// <returns>Operation Result</returns>
        [OperationContract]
        SingleMessage<Boolean> DisableAccount(String accountName);
        /// <summary>
        /// Reset User Password
        /// </summary>
        /// <param name="accountName">Account Name</param>
        /// <param name="password">Password</param>
        /// <returns>Operation Result</returns>
        [OperationContract]
        SingleMessage<Boolean> ResetPassword(string accountName, string password);
    }
}
