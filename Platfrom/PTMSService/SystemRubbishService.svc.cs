using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.SystemRubbish.Contract;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.SystemRubbishRepository.Repository;
using Gsafety.PTMS.Manager.Contract.Data;

namespace Gs.PTMS.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“SystemRubbishService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 SystemRubbishService.svc 或 SystemRubbishService.svc.cs，然后开始调试。
    public class SystemRubbishService : BaseService, ISystemRubbishService
    {
        private SystemRubbishRepository systemRubbishRepository = new SystemRubbishRepository();
        public MultiMessage<Vehicle> GetAbandonVehicleList(string clientID,string SearchVehicleId, string SearchOwner, string SearchVehicleType, string vehicletypeid)
        {
            Info("GetAbandonVehicleList");
            try
            {
                MultiMessage<Vehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = SystemRubbishRepository.GetAbandonVehicleList(context,clientID,SearchVehicleId, SearchOwner, vehicletypeid);
                }
                Log<Vehicle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Vehicle>(false, ex);
            }
        }

        public SingleMessage<bool> RecoverVehicle(Vehicle model)
        {
            Info("RecoverVehicle");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = systemRubbishRepository.RecoverVehicle(context, model);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        /// <summary>
        /// 获取删除的安全套件列表
        /// </summary>
        public MultiMessage<DevSuite> GetAbandonDevSuiteList(string clientID, string suitID, string mdvrCoreSn, string mdvrSn, string mdvrSim)
        {
            Info("GetAbandonDevSuiteList");
            try
            {
                MultiMessage<DevSuite> result = null;
                using (var context = new PTMSEntities())
                {
                    result = SystemRubbishRepository.GetAbandonDevSuiteList(context, clientID, suitID, mdvrCoreSn, mdvrSn, mdvrSim);
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


        public SingleMessage<bool> RecoverDevSuite(DevSuite model)
        {
            Info("RecoverDevSuite");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = SystemRubbishRepository.RecoverDevSuite(context, model);
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


        public MultiMessage<GUser> GetAbandonGUserList(string clientID, string qureyUserName)
        {
            try
            {
                Info("GetAbandonGUserList");
                MultiMessage<GUser> result = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = SystemRubbishRepository.GetAbandonGUserList(context, clientID, qureyUserName);
                }

                Log<GUser>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<GUser>(false, ex);
            }
        }

        public SingleMessage<bool> RecoverGUser(GUser model)
        {
            Info("RecoverGUser");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = SystemRubbishRepository.RecoverGUser(context, model);
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

    }
}
