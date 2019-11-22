using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.PublicService.Contract;
using GSafety.PTMS.PublicService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Gs.PTMS.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“PublicService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 PublicService.svc 或 PublicService.svc.cs，然后开始调试。
    public class PublicService : BaseService, ILostRegistry, IFoundRegistry, IRunMdvrMessage, IRunAppMessage, IRunMdvrmessageVehicle, IAppMessageVehicle
    {
        #region MyRegion
        /// <summary>
        /// 添加拾到物
        /// </summary>
        /// <param name="model">拾到物</param>
        public SingleMessage<bool> InsertFoundRegistry(FoundRegistry model)
        {
            Info("InsertFoundRegistry");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                model.CreateTime = DateTime.UtcNow;
                using (var context = new PTMSEntities())
                {
                    result = FoundRegistryRepository.InsertFoundRegistry(context, model);
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
        /// 修改拾到物
        /// </summary>
        public SingleMessage<bool> UpdateFoundRegistry(FoundRegistry model)
        {
            Info("UpdateFoundRegistry");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = FoundRegistryRepository.UpdateFoundRegistry(context, model);
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
        /// 删除拾到物
        /// </summary>
        public SingleMessage<bool> DeleteFoundRegistryByID(string ID)
        {
            Info("DeleteFoundRegistryByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = FoundRegistryRepository.DeleteFoundRegistryByID(context, ID);
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
        /// 获取拾到物
        /// </summary>
        public SingleMessage<FoundRegistry> GetFoundRegistry(string ID)
        {
            Info("GetFoundRegistry");
            Info(ID.ToString());
            try
            {
                SingleMessage<FoundRegistry> result = null;
                using (var context = new PTMSEntities())
                {
                    result = FoundRegistryRepository.GetFoundRegistry(context, ID);
                }
                Log<FoundRegistry>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<FoundRegistry>(false, ex);
            }
        }
        /// <summary>
        /// 获取拾到物
        /// </summary>
        public MultiMessage<FoundRegistry> GetFoundRegistryList(PagingInfo page, string clientID)
        {
            Info("GetFoundRegistryList");
            Info(page.PageIndex.ToString());
            Info(page.PageSize.ToString());
            try
            {
                MultiMessage<FoundRegistry> result = null;
                using (var context = new PTMSEntities())
                {
                    result = FoundRegistryRepository.GetFoundRegistryList(context, page.PageIndex, page.PageSize, clientID);
                }
                Log<FoundRegistry>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<FoundRegistry>(false, ex);
            }
        }


        /// <summary>
        /// 获取拾到物
        /// </summary>
        public MultiMessage<FoundRegistry> GetFoundRegistryByConditionList(PagingInfo page, string clientID, string Founder, string Keyword, string LostName)
        {
            Info("GetFoundRegistryByConditionList");
            Info(page.PageIndex.ToString());
            Info(page.PageSize.ToString());
            try
            {
                MultiMessage<FoundRegistry> result = null;
                using (var context = new PTMSEntities())
                {
                    result = FoundRegistryRepository.GetFoundRegistryByConditionList(context, page.PageIndex, page.PageSize, clientID, Founder, Keyword, LostName);
                }
                Log<FoundRegistry>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<FoundRegistry>(false, ex);
            }
        }


        /***************************************************************************************************/
        /// <summary>
        /// 添加丢失登记
        /// </summary>
        /// <param name="model">丢失登记</param>
        public SingleMessage<bool> InsertLostRegistry(LostRegistry model)
        {
            Info("InsertLostRegistry");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                model.CreateTime = DateTime.UtcNow;
                using (var context = new PTMSEntities())
                {
                    result = LostRegistryRepository.InsertLostRegistry(context, model);
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
        /// 修改丢失登记
        /// </summary>
        public SingleMessage<bool> UpdateLostRegistry(LostRegistry model)
        {
            Info("UpdateLostRegistry");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LostRegistryRepository.UpdateLostRegistry(context, model);
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
        /// 删除丢失登记
        /// </summary>
        public SingleMessage<bool> DeleteLostRegistryByID(string ID)
        {
            Info("DeleteLostRegistryByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LostRegistryRepository.DeleteLostRegistryByID(context, ID);
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
        /// 获取丢失登记
        /// </summary>
        public SingleMessage<LostRegistry> GetLostRegistry(string ID)
        {
            Info("GetLostRegistry");
            Info(ID.ToString());
            try
            {
                SingleMessage<LostRegistry> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LostRegistryRepository.GetLostRegistry(context, ID);
                }
                Log<LostRegistry>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<LostRegistry>(false, ex);
            }
        }
        /// <summary>
        /// 获取丢失登记
        /// </summary>
        public MultiMessage<LostRegistry> GetLostRegistryList(PagingInfo page, string clientID)
        {
            Info("GetLostRegistryList");
            Info(page.PageIndex.ToString());
            Info(page.PageSize.ToString());
            try
            {
                MultiMessage<LostRegistry> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LostRegistryRepository.GetLostRegistryList(context, page.PageIndex, page.PageSize, clientID);
                }
                Log<LostRegistry>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<LostRegistry>(false, ex);
            }
        }


        /// <summary>
        /// 获取丢失登记
        /// </summary>
        public MultiMessage<LostRegistry> GetLostRegistryByConditionList(PagingInfo page, string clientID, string LostIDCard, string Keyword, string LostName)
        {
            Info("GetFoundRegistryByConditionList");
            Info(page.PageIndex.ToString());
            Info(page.PageSize.ToString());
            try
            {
                MultiMessage<LostRegistry> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LostRegistryRepository.GetLostRegistryByConditionList(context, page.PageIndex, page.PageSize, clientID, LostIDCard, Keyword, LostName);
                }
                Log<LostRegistry>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<LostRegistry>(false, ex);
            }
        }

        #endregion


        #region 消息管理服务

        /// <summary>
        /// 添加LED消息
        /// </summary>
        /// <param name="model">LED消息</param>
        public SingleMessage<bool> InsertRunMdvrMessage(RunMdvrMessage model)
        {
            Info("InsertRunMdvrMessage");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                model.CreateTime = DateTime.UtcNow;
                using (var context = new PTMSEntities())
                {
                    result = RunMdvrMessageRepository.InsertRunMdvrMessage(context, model);
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
        /// 修改LED消息
        /// </summary>
        public SingleMessage<bool> UpdateRunMdvrMessage(RunMdvrMessage model)
        {
            Info("UpdateRunMdvrMessage");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RunMdvrMessageRepository.UpdateRunMdvrMessage(context, model);
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
        /// 删除LED消息
        /// </summary>
        public SingleMessage<bool> DeleteRunMdvrMessageByID(string ID)
        {
            Info("DeleteRunMdvrMessageByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RunMdvrMessageRepository.DeleteRunMdvrMessageByID(context, ID);
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
        /// 获取LED消息
        /// </summary>
        public SingleMessage<RunMdvrMessage> GetRunMdvrMessage(string ID)
        {
            Info("GetRunMdvrMessage");
            Info(ID.ToString());
            try
            {
                SingleMessage<RunMdvrMessage> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RunMdvrMessageRepository.GetRunMdvrMessage(context, ID);
                }
                Log<RunMdvrMessage>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<RunMdvrMessage>(false, ex);
            }
        }
        /// <summary>
        /// 获取LED消息
        /// </summary>
        public MultiMessage<RunMdvrMessage> GetRunMdvrMessageList(string clientID, string title, int type, string name, int pageIndex, int pageSize)
        {
            Info("GetRunMdvrMessageList");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<RunMdvrMessage> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RunMdvrMessageRepository.GetRunMdvrMessageList(context, clientID,title,type, name, pageIndex, pageSize);
                }
                Log<RunMdvrMessage>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<RunMdvrMessage>(false, ex);
            }
        }

        /// <summary>
        /// 添加消息车辆
        /// </summary>
        /// <param name="model">消息车辆</param>
        public SingleMessage<bool> InsertRunMdvrmessageVehicle(List<MdvrMessageVehicle> model)
        {
            Info("InsertRunMdvrmessageVehicle");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                foreach (var item in model)
                {
                    item.CreateTime = DateTime.UtcNow;
                }
                using (var context = new PTMSEntities())
                {
                    result = RunMdvrmessageVehicleRepository.InsertRunMdvrmessageVehicle(context, model);
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
        /// 删除消息车辆
        /// </summary>
        public SingleMessage<bool> DeleteRunMdvrmessageVehicleByID(string ID)
        {
            Info("DeleteRunMdvrmessageVehicleByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RunMdvrmessageVehicleRepository.DeleteRunMdvrmessageVehicleByID(context, ID);
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
        /// 获取消息车辆
        /// </summary>
        public MultiMessage<MdvrMessageVehicle> GetRunMdvrmessageVehicleList(string messageID, int pageIndex, int pageSize)
        {
            Info("GetRunMdvrmessageVehicleList");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<MdvrMessageVehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RunMdvrmessageVehicleRepository.GetRunMdvrmessageVehicleList(context, messageID, pageIndex, pageSize);
                }
                Log<MdvrMessageVehicle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<MdvrMessageVehicle>(false, ex);
            }
        }

        public SingleMessage<bool> DeliverRunMdvrmessageToVehicle(List<MdvrMessageVehicle> vehicles)
        {
            Info("DeliverSpeedLimitToVehicle");
            foreach (var item in vehicles)
            {
                Info(item.ToString());
            }
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RunMdvrmessageVehicleRepository.DeliverRunMdvrmessageToVehicle(context, vehicles);
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


        public MultiMessage<MdvrMessageVehicle> GetAllRunMdvrmessageVehicleListBySpeedID(string clientID, string messageId)
        {
            Info("GetVehicleSpeedList");
            Info(clientID.ToString());
            Info(messageId.ToString());
            try
            {
                MultiMessage<MdvrMessageVehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RunMdvrmessageVehicleRepository.GetAllRunMdvrmessageVehicleListBySpeedID(context, clientID, messageId);
                }
                Log<MdvrMessageVehicle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<MdvrMessageVehicle>(false, ex);
            }
        }

        /// <summary>
        /// 添加App消息
        /// </summary>
        /// <param name="model">App消息</param>
        public SingleMessage<bool> InsertRunAppMessage(RunAppMessage model)
        {
            Info("InsertRunAppMessage");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                model.CreateTime = DateTime.UtcNow;
                using (var context = new PTMSEntities())
                {
                    result = RunAppMessageRepository.InsertRunAppMessage(context, model);
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
        /// 修改App消息
        /// </summary>
        public SingleMessage<bool> UpdateRunAppMessage(RunAppMessage model)
        {
            Info("UpdateRunAppMessage");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RunAppMessageRepository.UpdateRunAppMessage(context, model);
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
        /// 删除App消息
        /// </summary>
        public SingleMessage<bool> DeleteRunAppMessageByID(string ID)
        {
            Info("DeleteRunAppMessageByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RunAppMessageRepository.DeleteRunAppMessageByID(context, ID);
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
        /// 获取App消息
        /// </summary>
        public SingleMessage<RunAppMessage> GetRunAppMessage(string ID)
        {
            Info("GetRunAppMessage");
            Info(ID.ToString());
            try
            {
                SingleMessage<RunAppMessage> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RunAppMessageRepository.GetRunAppMessage(context, ID);
                }
                Log<RunAppMessage>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<RunAppMessage>(false, ex);
            }
        }
        /// <summary>
        /// 获取App消息
        /// </summary>
        public MultiMessage<RunAppMessage> GetRunAppMessageList(string clientID, string title, int pageIndex, int pageSize)
        {
            Info("GetRunAppMessageList");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<RunAppMessage> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RunAppMessageRepository.GetRunAppMessageList(context, clientID, title, pageIndex, pageSize);
                }
                Log<RunAppMessage>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<RunAppMessage>(false, ex);
            }
        }

        /// <summary>
        /// 发送App消息
        /// </summary>
        /// <param name="model">App消息</param>
        public SingleMessage<bool> SendRunAppMessage(RunAppMessage model, string vehicleId)
        {
            Info("InsertRunAppMessage");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                model.CreateTime = DateTime.UtcNow;
                using (var context = new PTMSEntities())
                {
                    result = RunAppMessageRepository.SendRunAppMessage(context, model, vehicleId);
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


        #endregion

        //*************************************************************************************************************
        public SingleMessage<bool> InsertAppMessageVehicle(List<AppMessageVehicle> models, RunAppMessage message)
        {
            Info("InsertAppMessageVehicle");
            Info(models.ToString());
            try
            {
                SingleMessage<bool> result = null;
                foreach (var item in models)
                {
                    item.CreateTime = DateTime.UtcNow;
                }
                using (var context = new PTMSEntities())
                {
                    result = AppMessageVehicleRepository.InsertAppMessageVehicle(context, models, message);
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

        public SingleMessage<bool> DeleteAppMessageVehicleByID(string ID)
        {
            Info("DeleteAppMessageVehicleByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = AppMessageVehicleRepository.DeleteAppMessageVehicleByID(context, ID);
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

        public MultiMessage<AppMessageVehicle> GetAppMessageVehicleListByAppID(string clientID, string appID, int pageIndex, int pageSize)
        {
            Info("GetAppMessageVehicleListByAppID");
            Info(appID.ToString());
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<AppMessageVehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = AppMessageVehicleRepository.GetAppMessageVehicleListByAppID(context, appID, pageIndex, pageSize);
                }
                Log<AppMessageVehicle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<AppMessageVehicle>(false, ex);
            }
        }

        public MultiMessage<AppMessageVehicle> GetAllAppMessageVehicleListByAppID(string clientID, string appID)
        {
            Info("GetAllAppMessageVehicleListByAppID");
            Info(appID.ToString());
            try
            {
                MultiMessage<AppMessageVehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = AppMessageVehicleRepository.GetAllAppMessageVehicleListByAppID(context, appID);
                }
                Log<AppMessageVehicle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<AppMessageVehicle>(false, ex);
            }
        }

        public SingleMessage<bool> DeliverAppMessageToVehicle(List<AppMessageVehicle> vehicles)
        {
            Info("DeliverAppMessageToVehicle");
            foreach (var item in vehicles)
            {
                Info(item.ToString());
            }
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = AppMessageVehicleRepository.DeliverAppMessageToVehicle(context, vehicles);
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
