using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Gs.PTMS.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“BscDevSuiteService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 BscDevSuiteService.svc 或 BscDevSuiteService.svc.cs，然后开始调试。
    public class BscDevSuiteService : BaseService, IBscDevSuiteService, IBscDevSuitePartService
    {
        /// <summary>
        /// 添加安全套件
        /// </summary>
        /// <param name="model">安全套件</param>
        public SingleMessage<bool> InsertBscDevSuite(DevSuite model)
        {
            Info("InsertBscDevSuite");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    model.CreateTime = DateTime.Now.ToUniversalTime();
                    result = BscDevSuiteRepository.InsertBscDevSuite(context, model);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        /// <summary>
        /// 修改安全套件
        /// </summary>
        public SingleMessage<bool> UpdateBscDevSuite(DevSuite model)
        {
            Info("UpdateBscDevSuite");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = BscDevSuiteRepository.UpdateBscDevSuite(context, model);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        /// <summary>
        /// 删除安全套件
        /// </summary>
        public SingleMessage<bool> DeleteBscDevSuiteByID(string SuiteInfoID)
        {
            Info("DeleteBscDevSuiteByID");
            Info(SuiteInfoID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = BscDevSuiteRepository.DeleteBscDevSuiteByID(context, SuiteInfoID);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        /// <summary>
        /// 获取安全套件
        /// </summary>
        public SingleMessage<DevSuite> GetBscDevSuite(string SuiteInfoID)
        {
            Info("GetBscDevSuite");
            Info(SuiteInfoID.ToString());
            try
            {
                SingleMessage<DevSuite> result = null;
                using (var context = new PTMSEntities())
                {
                    result = BscDevSuiteRepository.GetBscDevSuite(context, SuiteInfoID);
                }
                Log<DevSuite>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<DevSuite>(false, ex);
            }
        }

        /// <summary>
        /// 获取安全套件
        /// </summary>
        public SingleMessage<DevSuite> GetDevSuiteBySuiteID(string suiteID)
        {
            Info("GetBscDevSuite");
            Info(suiteID.ToString());
            try
            {
                SingleMessage<DevSuite> result = null;
                using (var context = new PTMSEntities())
                {
                    result = BscDevSuiteRepository.GetDevSuiteBySuiteID(context, suiteID);
                }
                Log<DevSuite>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<DevSuite>(false, ex);
            }
        }
        /// <summary>
        /// 获取安全套件
        /// </summary>
        public MultiMessage<DevSuite> GetBscDevSuiteList(string clientID, InstallStatusType? installStatus, string vehicleSn, string suitID, string mdvrCoreSn, string mdvrSn, string mdvrSim, int pageIndex, int pageSize)
        {
            Info("GetBscDevSuiteList");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<DevSuite> result = null;
                using (var context = new PTMSEntities())
                {
                    result = BscDevSuiteRepository.GetBscDevSuiteList(context, clientID, installStatus, vehicleSn, suitID, mdvrCoreSn, mdvrSn, mdvrSim, pageIndex, pageSize);
                }
                Log<DevSuite>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<DevSuite>(false, ex);
            }
        }

        /// <summary>
        /// 根据车牌号查询安全套件号
        /// </summary>
        /// <param name="vehicleSN"></param>
        /// <returns></returns>
        public SingleMessage<string> GetBscDevSuiteIDByVehicleSN(string vehicleSN)
        {
            Info("GetBscDevSuiteIDByVehicleSN");
            Info(vehicleSN);
            try
            {
                SingleMessage<string> result = null;
                using (var context = new PTMSEntities())
                {
                    result = BscDevSuiteRepository.GetBscDevSuiteIDByVehicleSN(context, vehicleSN);
                }
                Log<string>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<string>(false, ex);
            }
        }

        //*********************************************************************


        public SingleMessage<bool> InsertBscDevSuitePart(DevSuitePart model)
        {
            Info("InsertBscDevSuitePart");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    model.CreateTime = DateTime.Now.ToUniversalTime();
                    result = BscDevSuiteRepository.InsertBscDevSuitePart(context, model);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public SingleMessage<bool> UpdateBscDevSuitePart(DevSuitePart model)
        {
            Info("UpdateBscDevSuitePart");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = BscDevSuiteRepository.UpdateBscDevSuitePart(context, model);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public SingleMessage<bool> DeleteBscDevSuitePartByID(string SuiteInfoID)
        {
            Info("DeleteBscDevSuitePartByID");
            Info(SuiteInfoID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = BscDevSuiteRepository.DeleteBscDevSuitePartByID(context, SuiteInfoID);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        /// <summary>
        /// 获取安全套件摄像头列表
        /// </summary>
        public MultiMessage<DevSuitePart> GetCameraListBySuiteInfoID(string suitInfoID)
        {
            Info("GetCameraListBySuiteInfoID");
            try
            {
                MultiMessage<DevSuitePart> result = null;
                using (var context = new PTMSEntities())
                {
                    result = BscDevSuiteRepository.GetCameraListBySuiteInfoID(context, suitInfoID);
                }
                Log<DevSuitePart>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<DevSuitePart>(false, ex);
            }
        }

        public SingleMessage<bool> BatchAdd(List<DevSuite> suiteList)
        {
            try
            {
                Info("BatchAdd");
                Info("suiteList:" + Convert.ToString(suiteList));
                var temp = BscDevSuiteRepository.BatchAdd(suiteList);
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

        public MultiMessage<DevSuite> CheckSecuritySuiteExist(List<DevSuite> deviceSuiteList)
        {
            try
            {
                Info("CheckSecuritySuiteExist");
                Info("deviceSuiteList:" + Convert.ToString(deviceSuiteList));
                var temp = BscDevSuiteRepository.CheckSecuritySuiteExist(deviceSuiteList);
                Log<DevSuite>(temp);
                return temp;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<DevSuite>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public MultiMessage<DevSuite> GetBscDevSuiteExportList(string clientID, InstallStatusType? installStatus, string vehicleSn, string suitID, string mdvrCoreSn, string mdvrSn, string mdvrSim, int pageIndex, int pageSize)
        {
            Info("GetBscDevSuiteExportList");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<DevSuite> result = null;
                using (var context = new PTMSEntities())
                {
                    result = BscDevSuiteRepository.GetBscDevSuiteExportList(context, clientID, installStatus, vehicleSn, suitID, mdvrCoreSn, mdvrSn, mdvrSim, pageIndex, pageSize);
                }
                Log<DevSuite>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<DevSuite>(false, ex);
            }
        }
    }
}
