using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Traffic.Repository;
using Gsafety.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.Traffic.Contract;
using Gsafety.PTMS.Traffic.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Data;

namespace Gs.PTMS.Service
{
    public class TrafficManageService : BaseService, ITrafficManageService
    {
        private TrafficRepository Repository = new TrafficRepository();
        #region Fence
        [Obsolete]
        public SingleMessage<bool> CheckFenceNameExist(string strFenceName, short nType)
        {
            try
            {
                Info("CheckFenceNameExist");
                Info("strFenceName:" + Convert.ToString(strFenceName) + ";" + "nType:" + Convert.ToString(nType));

                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = Repository.BeExistFenceName(context, strFenceName, nType);
                    SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                    Log<bool>(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        [Obsolete]
        public SingleMessage<bool> DeleteCarFenceByFenceID(string fenceID)
        {
            try
            {
                Info("DeleteCarFenceByFenceID");
                Info("fenceID:" + Convert.ToString(fenceID));

                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = Repository.DeleteVehicleFenceByFenceID(context, fenceID);
                    SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                    Log<bool>(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        [Obsolete]
        public Gsafety.PTMS.Base.Contract.Data.MultiMessage<Gsafety.PTMS.Traffic.Contract.Vehicle> GetVehicleByFence(string fenceID, short nState)
        {
            try
            {
                Info("GetVehicleByFence");
                Info("fenceID:" + Convert.ToString(fenceID) + ";" + "nState:" + Convert.ToString(nState));

                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = Repository.GetVehicleByFence(context, fenceID, nState);
                    MultiMessage<Gsafety.PTMS.Traffic.Contract.Vehicle> result = new MultiMessage<Gsafety.PTMS.Traffic.Contract.Vehicle> { Result = temp, TotalRecord = temp.Count, IsSuccess = true };
                    Log<Gsafety.PTMS.Traffic.Contract.Vehicle>(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Gsafety.PTMS.Traffic.Contract.Vehicle>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }
        [Obsolete]
        public MultiMessage<string> GetFenceIDByVehicleID(string VehicleID, short nState)
        {
            try
            {
                Info("GetFenceIDByVehicleID");
                Info("VehicleID:" + Convert.ToString(VehicleID) + ";" + "nState:" + Convert.ToString(nState));

                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = Repository.GetFenceIDByCarID(context, VehicleID, nState);
                    MultiMessage<string> result = new MultiMessage<string> { Result = temp, TotalRecord = temp.Count, IsSuccess = true };
                    Log<string>(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<string>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }
        [Obsolete]
        public SingleMessage<bool> CheckSpeedLimitNameExist(string strSpeedName)
        {
            try
            {
                Info("CheckSpeedLimitNameExist");
                Info("strSpeedName:" + Convert.ToString(strSpeedName));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.BeExistSpeedLimitName(context, strSpeedName);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        [Obsolete]
        public SingleMessage<bool> CheckSpeedLimitidNameExist(string strSpeedName, string id)
        {
            try
            {
                Info("CheckSpeedLimitidNameExist");
                Info("strSpeedName:" + Convert.ToString(strSpeedName) + ";" + "id:" + Convert.ToString(id));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.BeExistSpeedLimitidName(context, strSpeedName, id);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

     
        #endregion

        /// <summary>
        /// 添加一个电子围栏
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SingleMessage<bool> InsertTrafficFence(Gsafety.PTMS.Common.Data.TrafficFence model)
        {
            Info("InsertTrafficFence");
            Info(model.ToString());
            model.Valid = true;
            try
            {
                SingleMessage<bool> result = null;
                model.CreateTime = DateTime.UtcNow;
                using (var context = new PTMSEntities())
                {
                    result = TrafficFenceRepository.InsertTrafficFence(context, model);
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
        /// 更新电子围栏
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SingleMessage<bool> UpdateTrafficFence(Gsafety.PTMS.Common.Data.TrafficFence model)
        {
            Info("UpdateTrafficFence");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = TrafficFenceRepository.UpdateTrafficFence(context, model);
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
        /// 删除电子围栏
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public SingleMessage<bool> DeleteTrafficFenceByID(string ID)
        {
            Info("DeleteTrafficFenceByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = TrafficFenceRepository.DeleteTrafficFenceByID(context, ID);
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
        /// 根据TrafficFenceID获取TrafficeFence
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public SingleMessage<Gsafety.PTMS.Common.Data.TrafficFence> GetTrafficFence(string ID)
        {
            Info("GetTrafficFence");
            Info(ID.ToString());
            try
            {
                SingleMessage<Gsafety.PTMS.Common.Data.TrafficFence> result = null;
                using (var context = new PTMSEntities())
                {
                    result = TrafficFenceRepository.GetTrafficFence(context, ID);
                }
                Log<Gsafety.PTMS.Common.Data.TrafficFence>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<Gsafety.PTMS.Common.Data.TrafficFence>(false, ex);
            }
        }

        /// <summary>
        /// 是否有相同名称的TrafficeFence
        /// </summary>
        /// <param name="fenceName"></param>
        /// <param name="clientID"></param>
        /// <param name="fenceID"></param>
        /// <returns></returns>
        public SingleMessage<bool> IsTrafficExists(string fenceName, string clientID, string fenceID)
        {
            Info("IsTrafficExists");
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = TrafficFenceRepository.IsTrafficExists(context, fenceName, clientID, fenceID);
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
        /// 获与车绑定的下发成功过的电子围栏,状态为成功，操作不为删除
        /// </summary>
        /// <param name="vehicleID"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [Obsolete]
        public MultiMessage<Gsafety.PTMS.Common.Data.TrafficFence> GetDeliveredTrafficFenceListByVehicleID(string vehicleID, string clientID)
        {
            Info("GetDeliveredTrafficFenceListByVehicleID");
            Info(vehicleID.ToString());
            try
            {
                MultiMessage<Gsafety.PTMS.Common.Data.TrafficFence> result = null;
                using (var context = new PTMSEntities())
                {
                    result = TrafficFenceRepository.GetDeliveredTrafficFenceListByVehicleID(context, vehicleID, clientID);
                }
                Log<Gsafety.PTMS.Common.Data.TrafficFence>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Gsafety.PTMS.Common.Data.TrafficFence>(false, ex);
            }
        }

        /// <summary>
        /// 在车上正在应用的电子围栏，去除那些删除成功的电子围栏
        /// </summary>
        /// <param name="vehicleID"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public MultiMessage<Gsafety.PTMS.Common.Data.TrafficFence> GetTrafficFenceListOnVehicleByVehicleID(string vehicleID, string clientID)
        {
            Info("GetTrafficFenceListOnVehicleByVehicleID");
            Info(vehicleID.ToString());
            try
            {
                MultiMessage<Gsafety.PTMS.Common.Data.TrafficFence> result = null;
                using (var context = new PTMSEntities())
                {
                    result = TrafficFenceRepository.GetTrafficFenceListOnVehicleByVehicleID(context, vehicleID, clientID);
                }
                Log<Gsafety.PTMS.Common.Data.TrafficFence>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Gsafety.PTMS.Common.Data.TrafficFence>(false, ex);
            }
        }

        public MultiMessage<Gsafety.PTMS.Common.Data.TrafficFence> GetTrafficFenceListByVehicleIDAndFenceName(string fenceName, string vehicleID, string clientID)
        {
            Info("GetTrafficFenceListByVehicleIDAndClient");
            Info(fenceName.ToString());
            Info(vehicleID.ToString());
            Info(clientID.ToString());
            try
            {
                MultiMessage<Gsafety.PTMS.Common.Data.TrafficFence> result = null;
                using (var context = new PTMSEntities())
                {
                    result = TrafficFenceRepository.GetTrafficFenceListByVehicleIDAndFenceName(context, clientID, vehicleID, fenceName);
                }
                Log<Gsafety.PTMS.Common.Data.TrafficFence>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Gsafety.PTMS.Common.Data.TrafficFence>(false, ex);
            }
        }

        /// <summary>
        /// 围栏是否下发过
        /// </summary>
        /// <param name="fenceID"></param>
        /// <returns></returns>
        public SingleMessage<bool> IsFenceDelivered(string fenceID)
        {
            Info("IsFenceDelivered");
            Info("fenceID:" + fenceID);
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = TrafficFenceRepository.IsFenceDelivered(context, fenceID);
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

        public SingleMessage<bool> ObsoleteFence(string fenceID)
        {
            Info("ObsoleteFence");
            Info("fenceID:" + fenceID);
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = TrafficFenceRepository.ObsoleteFence(context, fenceID);
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

        public SingleMessage<bool> InsertFenceQueue(List<Gsafety.PTMS.Common.Data.FenceQueue> fencequeues)
        {
            Info("InsertFenceQueue");
            foreach (var model in fencequeues)
            {
                Info(model.ToString());
                model.CreateTime = DateTime.UtcNow;
            }

            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = FenceQueueRepository.InsertFenceQueue(context, fencequeues);
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

        public SingleMessage<bool> DeleteFenceQueueByID(string ID)
        {
            Info("DeleteFenceQueueByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = FenceQueueRepository.DeleteFenceQueueByID(context, ID);
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
        /// 获取电子围栏上绑定的车辆
        /// </summary>
        /// <param name="fenceID"></param>
        /// <param name="clientID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public MultiMessage<Gsafety.PTMS.Common.Data.FenceQueue> GetFenceQueueListByFenceID(string fenceID, string clientID, string vehicleName, int pageIndex, int pageSize)
        {
            Info("GetFenceQueueListByFenceID");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            Info(fenceID.ToString());
            Info(clientID.ToString());
            try
            {
                MultiMessage<FenceQueue> result = null;
                using (var context = new PTMSEntities())
                {
                    result = FenceQueueRepository.GetFenceQueueListByFenceID(context, fenceID, clientID, vehicleName, pageIndex, pageSize);
                }
                Log<FenceQueue>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<FenceQueue>(false, ex);
            }
        }

        /// <summary>
        /// 获取已经下发成功的或正要下发的
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="fenceID"></param>
        /// <returns></returns>
        public MultiMessage<Gsafety.PTMS.Common.Data.FenceQueue> GetAllFenceQueueListByFenceID(string clientID, string fenceID)
        {
            Info("GetFenceQueueListByFenceID");
            Info(fenceID.ToString());
            Info(clientID.ToString());
            try
            {
                MultiMessage<FenceQueue> result = null;
                using (var context = new PTMSEntities())
                {
                    result = FenceQueueRepository.GetAllFenceQueueListByFenceID(context, fenceID, clientID);
                }
                Log<FenceQueue>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<FenceQueue>(false, ex);
            }
        }

        /// <summary>
        /// 改变电子围栏队列的状态
        /// </summary>
        /// <param name="vehicles"></param>
        /// <returns></returns>
        public SingleMessage<bool> DeliverFenceQueueToVehicle(List<Gsafety.PTMS.Common.Data.FenceQueue> vehicles)
        {
            Info("DeliverFenceQueueToVehicle");
            foreach (var item in vehicles)
            {
                Info(item.ToString());
            }
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = FenceQueueRepository.DeliverFenceQueueToVehicle(context, vehicles);
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

        public SingleMessage<bool> InsertTrafficRoute(Gsafety.PTMS.Common.Data.TrafficRoute model)
        {
            Info("InsertTrafficRoute");
            Info(model.ToString());
            model.Valid = true;
            try
            {
                SingleMessage<bool> result = null;
                model.CreateTime = DateTime.UtcNow;
                using (var context = new PTMSEntities())
                {
                    result = TrafficRouteRepository.InsertTrafficRoute(context, model);
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

        public SingleMessage<bool> UpdateTrafficRoute(Gsafety.PTMS.Common.Data.TrafficRoute model)
        {
            Info("UpdateTrafficRoute");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = TrafficRouteRepository.UpdateTrafficRoute(context, model);
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

        public SingleMessage<bool> DeleteTrafficRouteByID(string ID)
        {
            Info("DeleteTrafficRouteByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = TrafficRouteRepository.DeleteTrafficRouteByID(context, ID);
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

        public SingleMessage<Gsafety.PTMS.Common.Data.TrafficRoute> GetTrafficRoute(string ID)
        {
            Info("GetTrafficRoute");
            Info(ID.ToString());
            try
            {
                SingleMessage<Gsafety.PTMS.Common.Data.TrafficRoute> result = null;
                using (var context = new PTMSEntities())
                {
                    result = TrafficRouteRepository.GetTrafficRoute(context, ID);
                }
                Log<Gsafety.PTMS.Common.Data.TrafficRoute>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<Gsafety.PTMS.Common.Data.TrafficRoute>(false, ex);
            }
        }

        public SingleMessage<bool> IsRouteExists(string routeName, string clientID, string routeID)
        {
            Info("IsRouteExists");
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = TrafficRouteRepository.IsRouteExists(context, routeName, clientID, routeID);
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

        public MultiMessage<Gsafety.PTMS.Common.Data.TrafficRoute> GetDeliveredTrafficRouteListByVehicleID(string vehicleID, string clientID)
        {
            Info("GetDeliveredTrafficRouteListByVehicleID");
            Info(vehicleID.ToString());
            try
            {
                MultiMessage<Gsafety.PTMS.Common.Data.TrafficRoute> result = null;
                using (var context = new PTMSEntities())
                {
                    result = TrafficRouteRepository.GetDeliveredTrafficRouteListByVehicleID(context, vehicleID, clientID);
                }
                Log<Gsafety.PTMS.Common.Data.TrafficRoute>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Gsafety.PTMS.Common.Data.TrafficRoute>(false, ex);
            }
        }

        public MultiMessage<Gsafety.PTMS.Common.Data.TrafficRoute> GetTrafficRouteListByVehicleIDAndRouteName(string routeName, string vehicleID, string clientID)
        {
            Info("GetTrafficRouteListByVehicleIDAndRouteName");
            Info(routeName.ToString());
            Info(vehicleID.ToString());
            Info(clientID.ToString());
            try
            {
                MultiMessage<Gsafety.PTMS.Common.Data.TrafficRoute> result = null;
                using (var context = new PTMSEntities())
                {
                    result = TrafficRouteRepository.GetTrafficRouteListByVehicleIDAndRouteName(context, clientID, vehicleID, routeName);
                }
                Log<Gsafety.PTMS.Common.Data.TrafficRoute>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Gsafety.PTMS.Common.Data.TrafficRoute>(false, ex);
            }
        }

        public SingleMessage<bool> ObsoleteTrafficeRoute(string routeID)
        {
            Info("ObsoleteTrafficeRoute");
            Info(routeID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = TrafficRouteRepository.ObsoleteTrafficeRoute(context, routeID);
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

        public SingleMessage<bool> IsRouteDelivered(string routeID)
        {
            Info("IsRouteDelivered");
            Info(routeID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = TrafficRouteRepository.IsRouteDelivered(context, routeID);
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

        public MultiMessage<Gsafety.PTMS.Common.Data.RouteQueue> GetRouteQueueByRouteID(string clientID, string routeID, string vehicleName, int pageIndex, int pageSize)
        {
            Info("GetRouteQueueByRouteID");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            Info(routeID.ToString());
            Info(clientID.ToString());
            try
            {
                MultiMessage<RouteQueue> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RouteQueueRepository.GetRouteQueueListByRouteID(context, routeID, clientID, vehicleName, pageIndex, pageSize);
                }
                Log<RouteQueue>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<RouteQueue>(false, ex);
            }
        }

        public MultiMessage<Gsafety.PTMS.Common.Data.RouteQueue> GetAllRouteQueueByRouteID(string clientID, string routeID)
        {
            Info("GetFenceQueueListByFenceID");
            Info(routeID.ToString());
            Info(clientID.ToString());
            try
            {
                MultiMessage<RouteQueue> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RouteQueueRepository.GetAllRouteQueueListByRouteID(context, routeID, clientID);
                }
                Log<RouteQueue>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<RouteQueue>(false, ex);
            }
        }

        public SingleMessage<bool> DeliverRouteQueueToVehicle(List<Gsafety.PTMS.Common.Data.RouteQueue> vehicles)
        {
            Info("DeliverRouteQueueToVehicle");
            foreach (var item in vehicles)
            {
                Info(item.ToString());
            }
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RouteQueueRepository.DeliverRouteQueueToVehicle(context, vehicles);
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


        public MultiMessage<RouteQueue> GetTrafficRouteListOnVehicleByVehicleID(string vehicleID, string clientID)
        {
            Info("GetTrafficRouteListOnVehicleByVehicleID");
            Info(vehicleID.ToString());
            try
            {
                MultiMessage<Gsafety.PTMS.Common.Data.RouteQueue> result = null;
                using (var context = new PTMSEntities())
                {
                    result = TrafficRouteRepository.GetTrafficRouteListOnVehicleByVehicleID(context, vehicleID, clientID);
                }
                Log<Gsafety.PTMS.Common.Data.RouteQueue>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Gsafety.PTMS.Common.Data.RouteQueue>(false, ex);
            }
        }


        public SingleMessage<bool> InsertRouteQueue(List<RouteQueue> routequeues)
        {
            Info("InsertRouteQueue");
            foreach (var model in routequeues)
            {
                Info(model.ToString());
                model.CreateTime = DateTime.UtcNow;
            }

            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RouteQueueRepository.InsertFenceQueue(context, routequeues);
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


        public SingleMessage<bool> DeleteRouteQueueByID(string ID)
        {
            Info("DeleteRouteQueueByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RouteQueueRepository.DeleteRouteQueueByID(context, ID);
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
