using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.Common.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Gsafety.PTMS.BaseInformation.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class SecuritySuiteService : BaseService, ISecuritySuiteService
    {
        private SecuritySuiteRepository Repository = new SecuritySuiteRepository();

        public SingleMessage<bool> AddSecuritySuite(DeviceSuite SecuritySuite)
        {
            try
            {
                Info("AddSecuritySuite");
                Info("SecuritySuite:" + Convert.ToString(SecuritySuite));
                var temp = Repository.AddSecuritySuite(SecuritySuite);
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

        public SingleMessage<bool> UpdateSecuritySuite(DeviceSuite SecuritySuite)
        {
            try
            {
                Info("UpdateSecuritySuite");
                Info("SecuritySuite:" + Convert.ToString(SecuritySuite));
                var temp = Repository.UpdateSecuritySuite(SecuritySuite);
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

        public SingleMessage<bool> DeleteSecuritySuite(DeviceSuite SecuritySuite)
        {
            try
            {
                Info("DeleteSecuritySuite");
                Info("SecuritySuite:" + Convert.ToString(SecuritySuite));
                var temp = Repository.DeleteSecuritySuite(SecuritySuite);
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

        public SingleMessage<DeviceSuite> GetSecuritySuiteBySuiteId(string suiteId, string mdvrid)
        {
            try
            {
                Info("GetSecuritySuiteBySuiteId");
                Info("suiteId:" + Convert.ToString(suiteId) + ";" + "mdvrid:" + Convert.ToString(mdvrid));
                var temp = Repository.GetSecuritySuiteBySuiteId(suiteId, mdvrid);
                SingleMessage<DeviceSuite> result = new SingleMessage<DeviceSuite>() { IsSuccess = true, Result = temp };
                Log<DeviceSuite>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<DeviceSuite>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public MultiMessage<DeviceSuite> GetSecuritySuitesFuzzy(string vehicleId, string suiteId, string mdvrId, string mdvrCoreId, InstallStatusType? status, PagingInfo page)
        {
            try
            {
                Info("GetSecuritySuitesFuzzy");
                Info("vehicleId:" + Convert.ToString(vehicleId) + ";" + "suiteId:" + Convert.ToString(suiteId) + ";" + "mdvrId:" + Convert.ToString(mdvrId) + ";" + "mdvrCoreId:" + Convert.ToString(mdvrCoreId) + ";" + "status:" + Convert.ToString(status) + ";" + "page:" + Convert.ToString(page));
                int totalRecord;
                var temp = Repository.GetSecuritySuitesFuzzy(vehicleId, suiteId, mdvrId, mdvrCoreId, status, page, out totalRecord, GetUserInfo());
                MultiMessage<DeviceSuite> result = new MultiMessage<DeviceSuite>() { Result = temp, TotalRecord = totalRecord };
                Log<DeviceSuite>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<DeviceSuite>() { ExceptionMessage = ex };

            }
        }

        public SingleMessage<bool> UpdateSecuritySuiteStatusByID(string Id, DeviceSuiteStatus status)
        {
            try
            {
                Info("UpdateSecuritySuiteStatusByID");
                Info("Id:" + Convert.ToString(Id) + ";" + "status:" + Convert.ToString(status));
                var temp = Repository.UpdateSecuritySuiteStatusByID(Id, status);
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

        public SingleMessage<bool> CheckSecuritySuiteExistBySuiteID(string suiteID)
        {
            try
            {
                Info("CheckSecuritySuiteExistBySuiteID");
                Info("suiteID:" + Convert.ToString(suiteID));
                var temp = Repository.CheckSecuritySuiteExistBySuiteID(suiteID);
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

        public SingleMessage<bool> CheckSecuritySuiteExistByMdvrCoreId(string mdvrCoreId)
        {
            try
            {
                Info("CheckSecuritySuiteExistByMdvrCoreId");
                Info("mdvrCoreId:" + Convert.ToString(mdvrCoreId));
                var temp = Repository.CheckSecuritySuiteExistByMdvrCoreId(mdvrCoreId);
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

        public SingleMessage<bool> CheckSecuritySuiteExistByMdvrId(string mdvrId)
        {
            try
            {
                Info("CheckSecuritySuiteExistByMdvrId");
                Info("mdvrId:" + Convert.ToString(mdvrId));
                var temp = Repository.CheckSecuritySuiteExistByMdvrId(mdvrId);
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

        public SingleMessage<bool> BatchAdd(List<DeviceSuite> suiteList)
        {
            try
            {
                Info("BatchAdd");
                Info("suiteList:" + Convert.ToString(suiteList));
                var temp = Repository.BatchAdd(suiteList);
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

        public SingleMessage<bool> CheckSecuritySuiteExist(List<DeviceSuite> deviceSuiteList)
        {
            try
            {
                Info("CheckSecuritySuiteExist");
                Info("deviceSuiteList:" + Convert.ToString(deviceSuiteList));
                var temp = Repository.CheckSecuritySuiteExist(deviceSuiteList);
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
    }
}
