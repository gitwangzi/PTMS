using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
namespace Gsafety.PTMS.BaseInformation.Service
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
                bool temp = Repository.AddInstallStation(installStation);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
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
                bool temp = Repository.DeleteInstallStation(id);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
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
                var temp = Repository.UpdateInstallStation(installStation);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public MultiMessage<InstallStation> GetInstallStationsByAlphabet(PagingInfo page)
        {
            try
            {
                Info("GetInstallStationsByAlphabet");
                Info("page" + Convert.ToString(page));
                int totalRecord;
                UserInfoMessageHeader userInfo = GetUserInfo();
                var temp = Repository.GetInstallStationsByAlphabet(userInfo, page, out totalRecord);
                MultiMessage<InstallStation> result = new MultiMessage<InstallStation>() { Result = temp, TotalRecord = totalRecord };
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallStation>() { ExceptionMessage = ex };
            }
        }

        public MultiMessage<InstallStation> GetInstallStationsFuzzy(string districtCode, string name, PagingInfo page)
        {
            try
            {
                Info("GetInstallStationsFuzzy");
                Info("districtCode:" + Convert.ToString(districtCode) + ";" + "name:" + Convert.ToString(name) + ";" + "page:" + Convert.ToString(page));
                int totalRecord;
                var temp = Repository.GetInstallStationsFuzzy(districtCode, name, page, out totalRecord, GetUserInfo());
                MultiMessage<InstallStation> result = new MultiMessage<InstallStation>() { Result = temp, TotalRecord = totalRecord };
                Log<InstallStation>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallStation>() { ExceptionMessage = ex };
            }
        }

        public MultiMessage<InstallStation> GetInstallStations()
        {
            try
            {
                Info("GetInstallStations");
                var temp = Repository.GetInstallStations();
                MultiMessage<InstallStation> result = new MultiMessage<InstallStation>() { Result = temp };
                Log<InstallStation>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallStation>() { ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> CheckInstallStationExistByName(string name)
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
            catch (Exception ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> CheckInstallStationExist(List<InstallStation> installStationList)
        {
            try
            {
                Info("CheckInstallStationExist");
                Info("installStationList:" + Convert.ToString(installStationList));
                var temp = Repository.BatchCheckInstallStationExist(installStationList);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = true, Result = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }


        public MultiMessage<InstallStation> GetInstallStationsByAlphabet(PagingInfo page, string clientID)
        {
            throw new NotImplementedException();
        }

        public MultiMessage<InstallStation> GetInstallStationsFuzzy(string districtCode, string name, PagingInfo page, string clientID)
        {
            throw new NotImplementedException();
        }

        public MultiMessage<InstallStation> GetInstallStations(string clientID)
        {
            throw new NotImplementedException();
        }

        public SingleMessage<bool> CheckInstallStationExistByName(string name, string clientID)
        {
            throw new NotImplementedException();
        }
    }
}
