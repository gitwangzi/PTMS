using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInformation.Contract
{
    [ServiceContract]
    public interface IChauffeurService
    {
        /// <summary>
        /// Add station in batch
        /// </summary>
        /// <param name="installBatchList"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> AddChauffeurList(List<Chauffeur> chauffeurList);

        /// <summary>
        /// Add chauffeur in batch
        /// </summary>
        /// <param name="installBatchList"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> BatchAddChauffeur(List<Chauffeur> chauffeurList);

        /// <summary>
        /// Check chauffeur Exist
        /// </summary>
        /// <param name="installStationList"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<Chauffeur> CheckChauffeurExist(List<Chauffeur> chauffeurList);

        /// <summary>
        /// Add Install Station
        /// </summary>
        /// <param name="setupStation">installStation info</param>
        /// <returns>Result</returns>
        [OperationContract]
        SingleMessage<Boolean> AddChauffeur(Chauffeur chauffeur);

        /// <summary>
        /// Delete Install Station
        /// </summary>
        /// <param name=" id ">ID</param>
        /// <returns>Result</returns>
        [OperationContract]
        SingleMessage<Boolean> DeleteChauffeur(string id);

        /// <summary>
        /// Update Install Station
        /// </summary>
        /// <param name="setupStation">info</param>
        /// <returns>Result</returns>
        [OperationContract]
        SingleMessage<Boolean> UpdateChauffeur(Chauffeur chauffeur);

        /// <summary>
        /// Get Install Stations By Alphabet
        /// </summary>
        /// <param name="page">PagingInfo</param>
        /// <returns>InstallStation List </returns>
        [OperationContract]
        MultiMessage<Chauffeur> GetChauffeurByPage(PagingInfo page, string clientID);

        /// <summary>
        /// Get Install Stations Fuzzy
        /// </summary>
        /// <param name="districtCode">districtCode</param>
        /// <param name="name">name</param>
        /// <param name="page">Page Info</param>
        /// <returns>Result</returns>
        [OperationContract]
        MultiMessage<Chauffeur> GetChauffeurByCondition(string name, string licence, string icard, PagingInfo page, string clientID);

        /// <summary>
        /// Check Install Station Exist
        /// </summary>
        /// <param name="installStationList"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<ChauffeurVehicle> GetChauffeurVehicle(string chauffeurID, string clientID);

        /// <summary>
        /// Check Install Station Exist
        /// </summary>
        /// <param name="installStationList"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> SaveChauffeurVehicle(ObservableCollection<ChauffeurVehicle> chauffeurVehicle);

        /// <summary>
        /// get ChauffeurByVehicle
        /// </summary>
        /// <param name="vehicleID"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<Chauffeur> GetChauffeurByVehicle(string vehicleID, string clientID);
    }
}
