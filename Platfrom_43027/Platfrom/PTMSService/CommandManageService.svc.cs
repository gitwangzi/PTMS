using Gsafety.Common.Logging;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Manager.Contract;
using Gsafety.PTMS.Manager.Contract.Data;
using Gsafety.PTMS.Manager.Contract.Data.CommandManage;
using Gsafety.PTMS.Manager.Repository;
using Gsafety.PTMS.Manager.Repository.UserManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Gs.PTMS.Service
{
    public class CommandManageService : BaseService, ICommandManageService
    {
        /// <summary>
        /// 添加心跳规则
        /// </summary>
        /// <param name="model">心跳规则</param>
        public SingleMessage<bool> InsertHeartbeatRule(HeartbeatRule model)
        {
            Info("InsertHeartbeatRule");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    model.CreateTime = DateTime.UtcNow;
                    result = HeartbeatRuleRepository.InsertHeartbeatRule(context, model);
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
        /// 修改心跳规则
        /// </summary>
        public SingleMessage<bool> UpdateHeartbeatRule(HeartbeatRule model)
        {
            Info("UpdateHeartbeatRule");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = HeartbeatRuleRepository.UpdateHeartbeatRule(context, model);
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
        /// 删除心跳规则
        /// </summary>
        public SingleMessage<bool> DeleteHeartbeatRuleByID(string ID)
        {
            Info("DeleteHeartbeatRuleByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = HeartbeatRuleRepository.DeleteHeartbeatRuleByID(context, ID);
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
        /// 获取心跳规则
        /// </summary>
        public MultiMessage<HeartbeatRule> GetHeartbeatRuleList(int pageIndex, int pageSize, string clientid, string name)
        {
            Info("GetHeartbeatRuleList");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<HeartbeatRule> result = null;
                using (var context = new PTMSEntities())
                {
                    result = HeartbeatRuleRepository.GetHeartbeatRuleList(context, clientid, name, pageIndex, pageSize);
                }
                Log<HeartbeatRule>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<HeartbeatRule>(false, ex);
            }
        }

        /// <summary>
        /// 添加心跳规则车辆
        /// </summary>
        /// <param name="model">心跳规则车辆</param>
        public SingleMessage<bool> InsertHeartbeatVehicle(List<HeartbeatVehicle> models)
        {
            Info("InsertHeartbeatVehicle");
            foreach (var item in models)
            {
                Info(item.ToString());
            }

            try
            {
                SingleMessage<bool> result = null;
                foreach (var item in models)
                {
                    item.CreateTime = DateTime.UtcNow;
                }
                using (var context = new PTMSEntities())
                {
                    result = HeartbeatVehicleRepository.InsertHeartbeatVehicle(context, models);
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
        /// 删除心跳规则车辆
        /// </summary>
        public SingleMessage<bool> DeleteHeartbeatVehicleByID(string ID)
        {
            Info("DeleteHeartbeatVehicleByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = HeartbeatVehicleRepository.DeleteHeartbeatVehicleByID(context, ID);
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
        /// 获取心跳规则车辆
        /// </summary>
        public MultiMessage<HeartbeatVehicle> GetHeartbeatVehicleListByHeartBeatID(string clientID, string heartBeatRuleID, int pageIndex, int pageSize)
        {
            Info("GetHeartbeatVehicleList");
            Info(clientID.ToString());
            Info(heartBeatRuleID.ToString());
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<HeartbeatVehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = HeartbeatVehicleRepository.GetHeartbeatVehicleList(context, clientID, heartBeatRuleID, pageIndex, pageSize);
                }
                Log<HeartbeatVehicle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<HeartbeatVehicle>(false, ex);
            }
        }

        public MultiMessage<HeartbeatVehicle> GetAllHeartbeatVehicleListByHeartBeatID(string clientID, string heartBeatRuleID)
        {
            Info("GetHeartbeatVehicleList");
            Info(clientID.ToString());
            Info(heartBeatRuleID.ToString());
            try
            {
                MultiMessage<HeartbeatVehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = HeartbeatVehicleRepository.GetAllHeartbeatVehicleListByHeartBeatID(context, clientID, heartBeatRuleID);
                }
                Log<HeartbeatVehicle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<HeartbeatVehicle>(false, ex);
            }
        }

        public SingleMessage<bool> DeliverHeartBeatRuleToVehicle(List<HeartbeatVehicle> vehicles)
        {
            Info("DeliverHeartBeatRuleToVehicle");
            foreach (var item in vehicles)
            {
                Info(item.ToString());
            }
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = HeartbeatVehicleRepository.DeliverHeartBeatRuleToVehicle(context, vehicles);
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


        #region LocationReportRule.....

        /// <summary>
        /// 添加LocationReportRule设备
        /// </summary>
        /// <param name="model">LocationReportRule设备</param>
        public SingleMessage<bool> InsertLocationReportRule(LocationReportRule model)
        {
            Info("InsertLocationReportRule");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                model.CreateTime = DateTime.UtcNow;
                using (var context = new PTMSEntities())
                {
                    result = LocationReportRuleRepository.InsertLocationReportRule(context, model);
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
        /// 修改LocationReportRule设备
        /// </summary>
        public SingleMessage<bool> UpdateLocationReportRule(LocationReportRule model)
        {
            Info("UpdateLocationReportRule");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LocationReportRuleRepository.UpdateLocationReportRule(context, model);
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
        /// 删除LocationReportRule设备
        /// </summary>
        public SingleMessage<bool> DeleteLocationReportRuleByID(string ID)
        {
            Info("DeleteLocationReportRuleByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LocationReportRuleRepository.DeleteLocationReportRuleByID(context, ID);
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
        /// 条件查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="clientID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public MultiMessage<LocationReportRule> GetByNameLocationReportRuleList(PagingInfo page, string clientID, string Name)
        {
            Info("GetByNameLocationReportRuleList");
            Info(page.PageIndex.ToString());
            Info(page.PageSize.ToString());
            try
            {
                MultiMessage<LocationReportRule> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LocationReportRuleRepository.GetByNameLocationReportRuleList(context, page.PageIndex, page.PageSize, clientID, Name);
                }
                Log<LocationReportRule>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<LocationReportRule>(false, ex);
            }
        }

        /// <summary>
        /// 添加汇报策略车辆
        /// </summary>
        /// <param name="model">汇报策略车辆</param>
        public SingleMessage<bool> InsertLocationReportVehicle(List<LocationReportVehicle> models)
        {
            Info("InsertLocationReportVehicle");
            foreach (var model in models)
            {
                Info(model.ToString());
            }

            try
            {
                SingleMessage<bool> result = null;
                foreach (var item in models)
                {
                    item.CreateTime = DateTime.UtcNow;
                }
                using (var context = new PTMSEntities())
                {
                    result = LocationReportVehicleRepository.InsertLocationReportVehicle(context, models);
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
        /// 删除汇报策略车辆
        /// </summary>
        public SingleMessage<bool> DeleteLocationReportVehicleByID(string ID)
        {
            Info("DeleteLocationReportVehicleByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LocationReportVehicleRepository.DeleteLocationReportVehicleByID(context, ID);
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
        /// 获取汇报策略车辆
        /// </summary>
        public MultiMessage<LocationReportVehicle> GetLocationReportVehicleListByLocationReportID(string clientID, string locationReportID, int pageIndex, int pageSize)
        {
            Info("GetLocationReportVehicleList");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            Info(clientID.ToString());
            Info(locationReportID.ToString());
            try
            {
                MultiMessage<LocationReportVehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LocationReportVehicleRepository.GetLocationReportVehicleListByLocationReportID(context, clientID, locationReportID, pageIndex, pageSize);
                }
                Log<LocationReportVehicle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<LocationReportVehicle>(false, ex);
            }
        }

        public MultiMessage<LocationReportVehicle> GetAllLocationReportVehicleListByLocationReportID(string clientID, string locationReportID)
        {
            Info("GetHeartbeatVehicleList");
            Info(clientID.ToString());
            Info(locationReportID.ToString());
            try
            {
                MultiMessage<LocationReportVehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LocationReportVehicleRepository.GetAllLocationReportVehicleListByLocationReportID(context, clientID, locationReportID);
                }
                Log<LocationReportVehicle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<LocationReportVehicle>(false, ex);
            }
        }

        public SingleMessage<bool> DeliverLocationReportRuleToVehicle(List<LocationReportVehicle> vehicles)
        {
            Info("LocationReportVehicle");
            foreach (var item in vehicles)
            {
                Info(item.ToString());
            }
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LocationReportVehicleRepository.DeliverLocationReportRuleToVehicle(context, vehicles);
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

        #region VideoRule.....
        /// <summary>
        /// 添加视频设置
        /// </summary>
        /// <param name="model">视频设置</param>
        public SingleMessage<bool> InsertVideoRule(VideoRule model)
        {
            Info("InsertVideoRule");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VideoRuleRepository.InsertVideoRule(context, model);
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
        /// 修改视频设置
        /// </summary>
        public SingleMessage<bool> UpdateVideoRule(VideoRule model)
        {
            Info("UpdateVideoRule");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VideoRuleRepository.UpdateVideoRule(context, model);
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
        /// 删除视频设置
        /// </summary>
        public SingleMessage<bool> DeleteVideoRuleByID(string ID)
        {
            Info("DeleteVideoRuleByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VideoRuleRepository.DeleteVideoRuleByID(context, ID);
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


        public MultiMessage<VideoRule> GetVideoRuleListByName(PagingInfo page, string clientID, string name)
        {
            Info("GetVideoRuleList");
            Info(page.PageIndex.ToString());
            Info(page.PageSize.ToString());
            try
            {
                MultiMessage<VideoRule> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VideoRuleRepository.GetVideoRuleList(context, page.PageIndex, page.PageSize, clientID, name);
                }
                Log<VideoRule>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<VideoRule>(false, ex);
            }
        }

        public SingleMessage<bool> InsertVideoRuleVehicle(List<VideoRuleVehicle> models)
        {
            Info("InsertVideoRuleVehicle");
            foreach (var model in models)
            {
                Info(model.ToString());
            }

            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VideoRuleVehicleRepository.InsertVideoRuleVehicle(context, models);
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

        public SingleMessage<bool> DeleteVideoRuleVehicleByID(string ID)
        {
            Info("DeleteVideoRuleVehicleByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VideoRuleVehicleRepository.DeleteVideoRuleVehicleByID(context, ID);
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

        public MultiMessage<VideoRuleVehicle> GetVideoRuleVehicleListByVideoRuleID(string clientID, string videoRuleID, int pageIndex, int pageSize)
        {
            Info("GetVideoRuleVehicleList");
            Info(clientID.ToString());
            Info(videoRuleID.ToString());
            Info(pageIndex.ToString());
            Info(pageSize.ToString());

            try
            {
                MultiMessage<VideoRuleVehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VideoRuleVehicleRepository.GetVideoRuleVehicleListByVideoRuleID(context, clientID, videoRuleID, pageIndex, pageSize);
                }
                Log<VideoRuleVehicle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<VideoRuleVehicle>(false, ex);
            }
        }

        public MultiMessage<VideoRuleVehicle> GetAllVideoRuleVehicleListByVideoRuleID(string clientID, string videoRuleID)
        {
            Info("GetVideoRuleVehicleList");
            Info(clientID.ToString());
            Info(videoRuleID.ToString());

            try
            {
                MultiMessage<VideoRuleVehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VideoRuleVehicleRepository.GetAllVideoRuleVehicleListByVideoRuleID(context, clientID, videoRuleID);
                }
                Log<VideoRuleVehicle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<VideoRuleVehicle>(false, ex);
            }
        }

        public SingleMessage<bool> DeliverVideoRuleToVehicle(List<VideoRuleVehicle> vehicles)
        {
            Info("DeliverVideoRuleToVehicle");
            foreach (var item in vehicles)
            {
                Info(item.ToString());
            }
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VideoRuleVehicleRepository.DeliverVideoRuleToVehicle(context, vehicles);
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

        /// <summary>
        /// 添加超速表
        /// </summary>
        /// <param name="model">超速表</param>
        public SingleMessage<bool> InsertSpeedLimit(SpeedLimit model)
        {
            Info("InsertSpeedLimit");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                model.CreateTime = DateTime.UtcNow;
                using (var context = new PTMSEntities())
                {
                    result = SpeedLimitRepository.InsertSpeedLimit(context, model);
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
        /// 修改超速表
        /// </summary>
        public SingleMessage<bool> UpdateSpeedLimit(SpeedLimit model)
        {
            Info("UpdateSpeedLimit");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = SpeedLimitRepository.UpdateSpeedLimit(context, model);
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
        /// 删除超速表
        /// </summary>
        public SingleMessage<bool> DeleteSpeedLimitByID(string ID)
        {
            Info("DeleteSpeedLimitByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = SpeedLimitRepository.DeleteSpeedLimitByID(context, ID);
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


        public MultiMessage<Gsafety.PTMS.Common.Data.SpeedLimit> GetSpeedLimitListByName(string clientID, string name, int pageIndex, int pageSize)
        {
            Info("GetSpeedLimitListByName");
            Info(clientID.ToString());
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<SpeedLimit> result = null;
                using (var context = new PTMSEntities())
                {
                    result = SpeedLimitRepository.GetSpeedLimitListByName(context, clientID, name, pageIndex, pageSize);
                }
                Log<SpeedLimit>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<SpeedLimit>(false, ex);
            }
        }

        public SingleMessage<bool> InsertVehicleSpeed(List<Gsafety.PTMS.Common.Data.VehicleSpeed> models)
        {
            Info("InsertVehicleSpeed");
            foreach (var model in models)
            {
                Info(model.ToString());
            }

            try
            {
                SingleMessage<bool> result = null;
                foreach (var item in models)
                {
                    item.CreateTime = DateTime.UtcNow;
                }
                using (var context = new PTMSEntities())
                {
                    result = VehicleSpeedRepository.InsertVehicleSpeed(context, models);
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

        public SingleMessage<bool> DeleteVehicleSpeedByID(string ID)
        {
            Info("DeleteVehicleSpeedByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VehicleSpeedRepository.DeleteVehicleSpeedByID(context, ID);
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

        public MultiMessage<Gsafety.PTMS.Common.Data.VehicleSpeed> GetAllVehicleSpeedListBySpeedID(string clientID, string speedID)
        {
            Info("GetVehicleSpeedList");
            Info(clientID.ToString());
            Info(speedID.ToString());
            try
            {
                MultiMessage<VehicleSpeed> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VehicleSpeedRepository.GetAllVehicleSpeedListBySpeedID(context, clientID, speedID);
                }
                Log<VehicleSpeed>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<VehicleSpeed>(false, ex);
            }
        }

        public MultiMessage<Gsafety.PTMS.Common.Data.VehicleSpeed> GetVehicleSpeedListBySpeedID(string clientID, string speedID, string vehicleName, int pageIndex, int pageSize)
        {
            Info("GetVehicleSpeedList");
            Info(clientID.ToString());
            Info(speedID.ToString());
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<VehicleSpeed> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VehicleSpeedRepository.GetVehicleSpeedListBySpeedID(context, clientID, speedID, vehicleName, pageIndex, pageSize);
                }
                Log<VehicleSpeed>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<VehicleSpeed>(false, ex);
            }
        }

        public SingleMessage<bool> DeliverSpeedLimitToVehicle(List<VehicleSpeed> vehicles)
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
                    result = VehicleSpeedRepository.DeliverSpeedLimitToVehicle(context, vehicles);
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
