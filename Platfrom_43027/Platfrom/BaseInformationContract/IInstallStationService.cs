/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7c22a10b-fda5-42a6-9672-d5a45b45a604      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Contract
/////    Project Description:    
/////             Class Name: ISetupStation
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 10:48:04
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/22 10:48:04
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
using System.Collections.ObjectModel;

namespace Gsafety.PTMS.BaseInformation.Contract
{
    [ServiceContract]
    public interface IInstallStationService
    {
        /// <summary>
        /// Add station in batch
        /// </summary>
        /// <param name="installBatchList"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> BatchAddStation(List<InstallStation> installBatchList);

        /// <summary>
        /// Add Install Station
        /// </summary>
        /// <param name="setupStation">installStation info</param>
        /// <returns>Result</returns>
        [OperationContract]
        SingleMessage<Boolean> AddInstallStation(InstallStation installStation);

        /// <summary>
        /// Delete Install Station
        /// </summary>
        /// <param name=" id ">ID</param>
        /// <returns>Result</returns>
        [OperationContract]
        SingleMessage<Boolean> DeleteInstallStation(string id);

        /// <summary>
        /// Update Install Station
        /// </summary>
        /// <param name="setupStation">info</param>
        /// <returns>Result</returns>
        [OperationContract]
        SingleMessage<Boolean> UpdateInstallStation(InstallStation installStation);

        /// <summary>
        /// Get Install Stations By Alphabet
        /// </summary>
        /// <param name="page">PagingInfo</param>
        /// <returns>InstallStation List </returns>
        [OperationContract]
        MultiMessage<InstallStation> GetInstallStationsByAlphabet(PagingInfo page,string clientID);

        /// <summary>
        /// Get Install Stations Fuzzy
        /// </summary>
        /// <param name="districtCode">districtCode</param>
        /// <param name="name">name</param>
        /// <param name="page">Page Info</param>
        /// <returns>Result</returns>
        [OperationContract]
        MultiMessage<InstallStation> GetInstallStationsFuzzy(string districtCode,string param, string name, PagingInfo page,string clientID);

        /// <summary>
        /// Get Install Stations
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<InstallStation> GetInstallStations(string clientID);

        /// <summary>
        /// Check Install Station Exist By Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> CheckInstallStationExistByName(string name, string clientID);

        /// <summary>
        /// Check Install Detail By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> CheckInstallDetailById(string Id);


        /// <summary>
        /// Check Install Station Exist
        /// </summary>
        /// <param name="installStationList"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<InstallStation> CheckInstallStationExist(List<InstallStation> installStationList);

        /// <summary>
        /// Check Install Station Exist
        /// </summary>
        /// <param name="installStationList"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<InstallStationUser> GetInstallStationUser(string installStationID, string clientID);

        /// <summary>
        /// Check Install Station Exist
        /// </summary>
        /// <param name="installStationList"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> SaveInstallStationUser(ObservableCollection<InstallStationUser> installStationUser);

        /// <summary>
        /// Get Install Stations
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<InstallStation> GetInstallStationsByUser(string userID);
    }
}
