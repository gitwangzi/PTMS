using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
namespace Gs.PTMS.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class InstallStationService : BaseService, IInstallStationService
    {
        private InstallStationRepository Repository = new InstallStationRepository();
        public SingleMessage<bool> AddInstallStation(InstallStation installStation)
        {
            try
            {
                Info("AddInstallStation");
                Info("installStation" + Convert.ToString(installStation));
                installStation.CreateTime = DateTime.UtcNow;
                SingleMessage<bool> result = Repository.AddInstallStation(installStation);
                Log<bool>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> DeleteInstallStation(string id)
        {
            try
            {
                Info("DeleteInstallStation");
                Info("id:" + Convert.ToString(id));
                SingleMessage<bool> result = Repository.DeleteInstallStation(id);
                Log<bool>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> UpdateInstallStation(InstallStation installStation)
        {
            try
            {
                Info("UpdateInstallStation");
                Info("installStation" + Convert.ToString(installStation));
                SingleMessage<bool> result = Repository.UpdateInstallStation(installStation);
                Log<bool>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public MultiMessage<InstallStation> GetInstallStationsByAlphabet(PagingInfo page, string clientID)
        {
            try
            {
                Info("GetInstallStationsByAlphabet");
                Info("page" + Convert.ToString(page));
                int totalRecord;
                UserInfoMessageHeader userInfo = GetUserInfo();
                var temp = Repository.GetInstallStationsByAlphabet(userInfo, page, out totalRecord, clientID);
                MultiMessage<InstallStation> result = new MultiMessage<InstallStation>() { Result = temp, TotalRecord = totalRecord, IsSuccess = true };
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallStation>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        //多条件查询
        public MultiMessage<InstallStation> GetInstallStationsFuzzy(string districtCode, string param, string name, PagingInfo page, string clientID)
        {
            try
            {
                Info("GetInstallStationsFuzzy");
                Info("districtCode:" + Convert.ToString(districtCode) + ";" + "name:" + Convert.ToString(name) + ";" + "page:" + Convert.ToString(page));
                int totalRecord;
                var temp = Repository.GetInstallStationsFuzzy(districtCode, param, name, page, out totalRecord, GetUserInfo(), clientID);
                MultiMessage<InstallStation> result = new MultiMessage<InstallStation>() { Result = temp, TotalRecord = totalRecord, IsSuccess = true };
                Log<InstallStation>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallStation>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public MultiMessage<InstallStation> GetInstallStations(string clientID)
        {
            try
            {
                Info("GetInstallStations");
                var temp = Repository.GetInstallStations(clientID);
                MultiMessage<InstallStation> result = new MultiMessage<InstallStation>() { Result = temp, IsSuccess = true };
                Log<InstallStation>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallStation>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<bool> CheckInstallStationExistByName(string name, string clientID)
        {
            try
            {
                Info("CheckInstallStationExistByName");
                Info("name:" + Convert.ToString(name));
                var temp = Repository.CheckInstallStationExistByName(name);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = true, Result = temp };
                Log<bool>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> CheckInstallDetailById(string Id)
        {
            try
            {
                Info("CheckInstallDetailById");
                Info("Id:" + Convert.ToString(Id));
                var temp = Repository.CheckInstallDetailById(Id);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = true, Result = temp };
                Log<bool>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<Boolean> BatchAddStation(List<InstallStation> installBatchList)
        {
            try
            {
                Info("BatchAddStation");
                Info("installBatchList:" + Convert.ToString(installBatchList));
                var temp = Repository.BatchAdd(installBatchList);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = true, Result = temp };
                Log<bool>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public MultiMessage<InstallStation> CheckInstallStationExist(List<InstallStation> installStationList)
        {
            try
            {
                Info("CheckInstallStationExist");
                Info("installStationList:" + Convert.ToString(installStationList));
                var temp = Repository.BatchCheckInstallStationExist(installStationList);
                Log<InstallStation>(temp);
                return temp;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallStation>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public MultiMessage<InstallStationUser> GetInstallStationUser(string installStationID, string clientID)
        {
            try
            {
                Info("GetInstallStationUser");
                //Info("page" + Convert.ToString(page));
                var temp = Repository.GetInstallStationUser(installStationID, clientID);
                MultiMessage<InstallStationUser> result = new MultiMessage<InstallStationUser>() { IsSuccess = true, Result = temp };
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallStationUser>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> SaveInstallStationUser(ObservableCollection<InstallStationUser> installStationUser)
        {
            try
            {
                Info("SaveInstallStationUser");
                Info("installStationUser" + Convert.ToString(installStationUser));
                SingleMessage<bool> result = Repository.SaveInstallStationUser(installStationUser);
                Log<bool>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public MultiMessage<InstallStation> GetInstallStationsByUser(string userID)
        {
            try
            {
                Info("GetInstallStations");
                var temp = Repository.GetInstallStationsByUser(userID);
                MultiMessage<InstallStation> result = new MultiMessage<InstallStation>(temp, temp.Count) { IsSuccess = true };
                Log<InstallStation>(result);
                return result;
            }
            catch(Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallStation>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }
    }
}
