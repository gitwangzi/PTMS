/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 31dda820-3d8a-4a55-a2cd-1486826d3955      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Contract
/////    Project Description:    
/////             Class Name: IVehicle
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/7 17:31:09
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/22 17:31:09
/////            Modified by: BilongLiu
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using System.IO;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Enum;

namespace Gsafety.PTMS.BaseInformation.Contract
{
    [ServiceContract]
    public interface IVehicleService
    {

        #region

        ///// <summary>
        ///// Get Vehicle By ID
        ///// </summary>
        ///// <param name="id">ID</param>
        ///// <returns>Vehicles</returns>
        //[OperationContract]
        //SingleMessage<Vehicle> GetVehicleByID(string id);

        ///// <summary>
        ///// Get Vehicles
        ///// </summary>
        //[OperationContract]
        //MultiMessage<Vehicle> GetVehicles();

        ///// <summary>
        ///// Get Instal Securit Vehicles
        ///// </summary>
        //[OperationContract]
        //MultiMessage<Vehicle> GetInstalSecuritVehicles();


        ///// <summary>
        ///// Batch Add Vehicle
        ///// </summary>
        ///// <param name="vehicleList"></param>
        ///// <param name="key"></param>
        ///// <param name="count"></param>
        ///// <returns></returns>
        //[OperationContract]
        //MultiMessage<Vehicle> BatchAddVehicle(List<Vehicle> vehicleList, string key, int count);

        ///// <summary>
        ///// Check Vehicle Install Status By VehicleId
        ///// </summary>
        ///// <param name="Id"></param>
        ///// <returns></returns>
        //[OperationContract]
        //SingleMessage<Boolean> CheckVehicleInstallStatusByVehicleId(string vehicleId);
        #endregion
        /// <summary>
        /// Add Vehicle
        /// </summary>
        /// <param name="vehicle">vehicle info</param>
        /// <returns>Result</returns>	
        [OperationContract]
        SingleMessage<Boolean> AddVehicle(Vehicle vehicle);

        /// <summary>
        /// Delete Vehicle
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>Result</returns>
        [OperationContract]
        SingleMessage<Boolean> DeleteVehicle(Vehicle model);

        /// <summary>
        /// Update Vehicle
        /// </summary>
        /// <param name="vehicle">vehicle </param>
        /// <returns>Result</returns>
        [OperationContract]
        SingleMessage<Boolean> UpdateVehicle(Vehicle vehicle);

        /// <summary>
        /// Get Instal Securit Vehicle By MDVRID
        /// </summary>
        /// <param name="mdvrID"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Vehicle> GetInstalSecuritVehicleByMDVRID(string mdvrID);

        /// <summary>
        ///Get Vehicles Fuzzy
        /// </summary>
        /// <param name="carNumber">vehicleId</param>
        /// <param name="districtCode">districtCode</param>
        /// <param name="companyId">companyId</param>
        /// <param name="type">VehicleType</param>
        /// <param name="owner">owner</param>
        /// <param name="installed">InstallStatusType</param>
        /// <param name="page">PagingInfo</param>
        /// <returns>Vehicle List</returns>
        //[OperationContract]
        //MultiMessage<Vehicle> GetVehiclesFuzzy(string vehicleId, string districtCode, VehicleTypeEnum? type, string owner, InstallStatusType? status, PagingInfo page);

        ///// <summary>
        /////Get Vehicles Fuzzy Ex
        ///// </summary>
        ///// <param name="carNumber">vehicleId</param>
        ///// <param name="districtCode">districtCode</param>
        ///// <param name="page">PagingInfo</param>
        ///// <returns>Vehicle List</returns>
        //[OperationContract]
        //MultiMessage<Vehicle> GetVehiclesFuzzyEx(string vehicleId, string districtCode, InstallStatusType? installStatus, PagingInfo page);

        /// <summary>
        /// Check Install Vehicle
        /// </summary>
        /// <param name="carNumber">vehicleId</param>
        /// <returns>VehicleCheckResult</returns>
        [OperationContract]
        SingleMessage<VehicleCheckResultExt> CheckInstallVehicleForSuite(string vehicleId, string clientID);


        /// <summary>
        /// Check Install Vehicle
        /// </summary>
        /// <param name="carNumber">vehicleId</param>
        /// <returns>VehicleCheckResult</returns>
        [OperationContract]
        SingleMessage<VehicleCheckResultExt> CheckInstallVehicleForGPS(string vehicleId, string clientID);

        /// <summary>
        /// Update Vehicle Status By VehicleId
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> UpdateVehicleStatusByVehicleId(string vehicleId, int status, string note);

        /// <summary>
        /// Check Vehicle Exist By VehicleId
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> CheckVehicleExistByVehicleId(string vehicleId);

        /// <summary>
        /// Batch Add
        /// </summary>
        /// <param name="vehicleList"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> BatchAdd(List<Vehicle> vehicleList);

        /// <summary>
        /// Get Instal Securit Vehicles By Authority
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        byte[] GetInstalSecuritVehiclesByAuthority();

        [OperationContract]
        SingleMessage<bool> CheckVehicleExistByVehicleSn(string vehicleSn);

        /// <summary>
        /// Check ID and SN exist 
        /// </summary>
        /// <param name="vehicleList"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<Vehicle> CheckVehicleExist(List<Vehicle> vehicleList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VehicleId"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Vehicle> GetBscVehicle(string VehicleId);



        /// <summary>
        /// getchauffeurVehicle
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<VVehicle> GetChauffeurVehiclePageList(PagingInfo page,string clientID,string vehicleNum);

        /// <summary>
        /// getchauffeurVehicle
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<Vehicle> GetChauffeurVehicle(string clientID);

        [OperationContract]
        MultiMessage<Vehicle> GetInstallVehiclesByAuthority(List<string> organizations);

        [OperationContract]
        SingleMessage<Vehicle> GetInstallVehicle(string organization, string vehicleid);
    }
}
