using Gsafety.Ant.BaseInformation.Repository;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Transactions;
namespace Gs.PTMS.Service
{
    public class ChauffeurService : BaseService, IChauffeurService
    {

        private ChauffeurRepository Repository = new ChauffeurRepository();

        public SingleMessage<bool> AddChauffeurList(List<Chauffeur> chauffeurList)
        {
            try
            {
                Info("AddchauffeurList");
                Info("chauffeurList" + Convert.ToString(chauffeurList));
                //bool temp = Repository.AddInstallStation(chauffeurList);
                //SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = temp };
                //Log<bool>(result);
                return null;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> AddChauffeur(Chauffeur chauffeur)
        {
            try
            {
                Info("AddChauffeur");
                Info("chauffeur" + Convert.ToString(chauffeur));
                chauffeur.CreateTime = DateTime.Now.ToUniversalTime();
                var temp = Repository.AddChauffeur(chauffeur);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = temp.Result, ErrorMsg = temp.ErrorMsg };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> DeleteChauffeur(string id)
        {
            try
            {
                Info("DeleteChauffeur");
                Info("id:" + Convert.ToString(id));
                SingleMessage<bool> result = Repository.DeleteChauffeur(id);
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> UpdateChauffeur(Chauffeur chauffeur)
        {
            try
            {
                Info("UpdateChauffeur");
                Info("chauffeur" + Convert.ToString(chauffeur));
                SingleMessage<bool> result = Repository.UpdateChauffeur(chauffeur);
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public MultiMessage<Chauffeur> GetChauffeurByPage(PagingInfo page, string clientID)
        {
            try
            {
                Info("GetChauffeurByPage");
                Info("page" + Convert.ToString(page));
                int totalRecord;
                UserInfoMessageHeader userInfo = GetUserInfo();
                var temp = Repository.GetChauffeurByPage(userInfo, page, out totalRecord, clientID);
                MultiMessage<Chauffeur> result = new MultiMessage<Chauffeur>() { Result = temp, TotalRecord = totalRecord };
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Chauffeur>() { ExceptionMessage = ex };
            }
        }

        public MultiMessage<Chauffeur> GetChauffeurByCondition(string name, string licence, string icard, PagingInfo page, string clientID)
        {
            try
            {
                Info("GetChauffeurByCondition");
                Info("name:" + Convert.ToString(name) + ";" + "licence:" + Convert.ToString(licence) + ";" + "page:" + Convert.ToString(page));
                int totalRecord;
                var temp = Repository.GetChauffeurByCondition(name, licence, icard, page, out totalRecord, GetUserInfo(), clientID);
                MultiMessage<Chauffeur> result = new MultiMessage<Chauffeur>() { IsSuccess = true, Result = temp, TotalRecord = totalRecord };
                Log<Chauffeur>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Chauffeur>() { ExceptionMessage = ex };
            }
        }

        public MultiMessage<ChauffeurVehicle> GetChauffeurVehicle(string chauffeurID, string clientID)
        {
            try
            {
                Info("GetChauffeurVehicle");
                //Info("page" + Convert.ToString(page));
                var temp = Repository.GetChauffeurVehicle(chauffeurID, clientID);
                MultiMessage<ChauffeurVehicle> result = new MultiMessage<ChauffeurVehicle>() { IsSuccess = true, Result = temp };
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<ChauffeurVehicle>() { ExceptionMessage = ex };
            }
        }

        public MultiMessage<Chauffeur> GetChauffeurByVehicle(string vehicleID, string clientID)
        {
            try
            {
                Info("GetChauffeurByVehicle");
                MultiMessage<Chauffeur> result = null;
                using (var context = new PTMSEntities())
                {
                    result = Repository.GetChauffeurByVehicle(context, vehicleID, clientID);
                }
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Chauffeur>() { ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> SaveChauffeurVehicle(ObservableCollection<ChauffeurVehicle> chauffeurVehicles)
        {
            try
            {
                Info("SaveChauffeurVehicle");
                foreach (var item in chauffeurVehicles)
                {
                    item.CreateTime = DateTime.Now.ToUniversalTime();
                }
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);
                    try
                    {
                        result = Repository.SaveChauffeurVehicle(context, chauffeurVehicles);
                        if (result.IsSuccess)
                        {
                            scope.Complete();
                        }
                    }
                    finally
                    {
                        scope.Dispose();
                    }
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        /// <summary>
        /// Add chauffeur in batch
        /// </summary>
        /// <param name="installBatchList"></param>
        /// <returns></returns>
        public SingleMessage<Boolean> BatchAddChauffeur(List<Chauffeur> chauffeurList)
        {
            try
            {
                Info("BatchAddChauffeur");
                Info("chauffeurList:" + Convert.ToString(chauffeurList));
                var temp = Repository.BatchAdd(chauffeurList);
                Log<bool>(temp);
                return temp;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        /// <summary>
        /// Check chauffeur Exist
        /// </summary>
        /// <param name="installStationList"></param>
        /// <returns></returns>
        public MultiMessage<Chauffeur> CheckChauffeurExist(List<Chauffeur> chauffeurList)
        {
            try
            {
                Info("CheckChauffeurExist");
                Info("chauffeurList:" + Convert.ToString(chauffeurList));
                var temp = Repository.BatchCheckInstallStationExist(chauffeurList);
                Log<Chauffeur>(temp);
                return temp;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Chauffeur>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }
    }
}
