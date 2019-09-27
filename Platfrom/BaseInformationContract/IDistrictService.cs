/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: f6c2526a-110c-48f8-a140-d09c78a02103      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Contract
/////    Project Description:    
/////             Class Name: IDistrict
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/19 17:39:50
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/22 17:39:50
/////            Modified by: BilongLiu
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;

namespace Gsafety.PTMS.BaseInformation.Contract
{
    /// <summary>
    /// District interface
    /// </summary>
    [ServiceContract]
    public interface IDistrictService
    {
        #region
        ///// <summary>
        ///// Get User Authority By Name
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //[OperationContract]
        //SingleMessage<UserAuthority> GetUserAuthorityByName(string name);

        ///// <summary>
        ///// Get Distric By Code
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //[OperationContract]
        //SingleMessage<District> GetDistricByCode(string code);
        #endregion
        /// <summary>
        /// Get All District
        /// </summary>
        /// <returns>District List</returns>
        [OperationContract]
        MultiMessage<District> GetDistrict();

        /// <summary>
        /// Get Province And City
        /// </summary>
        /// <returns>District List</returns>
        [OperationContract]
        MultiMessage<District> GetProvinceAndCity();






        /// <summary>
        /// Add User Authority
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> AddUserAuthority(UserAuthority userAuthority);

        /// <summary>
        /// Delete User Authority
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> DeleteUserAuthority(string loginName);

        /// <summary>
        /// Update User Authority
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> UpdateUserAuthority(UserAuthority userAuthority);

        /// <summary>
        /// Get User Authority Fuzzy
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<UserAuthority> GetUserAuthorityFuzzy(string loginName, PagingInfo page);

        /// <summary>
        /// Get District By Authority
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<District> GetDistrictByAuthority();

        /// <summary>
        /// Get FuncItem
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<FuncItem> GetFuncItem();

        /// <summary>
        /// Get Func By Role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<RoleFuncs> GetFuncByRole(string roleName);

        /// <summary>
        /// Update Role Func
        /// </summary>
        /// <param name="roleFunc"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> UpdateRoleFunc(RoleFuncs roleFunc);
    }
}
