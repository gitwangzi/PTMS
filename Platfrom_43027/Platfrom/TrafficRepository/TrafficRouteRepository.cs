using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using System.Data;
namespace Gsafety.PTMS.Traffic.Repository
{
    ///<summary>
    ///
    ///</summary>
    public class TrafficRouteRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public static SingleMessage<bool> InsertTrafficRoute(PTMSEntities context, TrafficRoute model)
        {
            var entity = new TRF_ROUTE();
            TrafficRouteUtility.UpdateEntity(entity, model, true);

            context.TRF_ROUTE.Add(entity);

            return context.Save();
        }

        /// <summary>
        /// 修改
        /// </summary>
        public static SingleMessage<bool> UpdateTrafficRoute(PTMSEntities context, TrafficRoute model)
        {
            var entity = context.TRF_ROUTE.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            TrafficRouteUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = EntityState.Modified;
            return context.Save();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public static SingleMessage<bool> DeleteTrafficRouteByID(PTMSEntities context, string ID)
        {
            TRF_ROUTE entity = context.TRF_ROUTE.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.TRF_ROUTE.Remove(entity);
                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        public static SingleMessage<TrafficRoute> GetTrafficRoute(PTMSEntities context, string ID)
        {
            TRF_ROUTE entity = context.TRF_ROUTE.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                TrafficRoute model = TrafficRouteUtility.GetModel(entity);
                return new SingleMessage<TrafficRoute>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取
        /// </summary>
        public static MultiMessage<TrafficRoute> GetTrafficRouteList(PTMSEntities context, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            var list = context.TRF_ROUTE.Page(out totalCount, pageIndex, pageSize, true, t => t.CREATE_TIME).ToList();

            var items = list.Select(t => TrafficRouteUtility.GetModel(t)).ToList();

            return new MultiMessage<TrafficRoute>(items, totalCount);
        }


        public static SingleMessage<bool> IsRouteExists(PTMSEntities context, string routeName, string clientID, string routeID)
        {
            TRF_ROUTE entity = null;
            if (string.IsNullOrEmpty(routeID))
            {
                entity = context.TRF_ROUTE.FirstOrDefault(n => n.CLIENT_ID == clientID && n.NAME == routeName);
            }
            else
            {
                entity = context.TRF_ROUTE.FirstOrDefault(n => n.CLIENT_ID == clientID && n.NAME == routeName && n.ID != routeID);
            }
            if (entity != null)
            {
                return new SingleMessage<bool>(true);
            }
            else
            {
                return new SingleMessage<bool>(false);
            }
        }

        public static MultiMessage<TrafficRoute> GetDeliveredTrafficRouteListByVehicleID(PTMSEntities context, string vehicleID, string clientID)
        {
            if (!string.IsNullOrEmpty(vehicleID))
            {
                vehicleID = vehicleID.ToUpper();
                var temp = from f in context.TRF_ROUTE
                           join q in context.TRF_ROUTE_QUEUE on f.ID equals q.ROUTE_ID
                           where q.VEHICLE_ID.ToUpper().Contains(vehicleID) && q.STATUS == (short)CommandStateEnum.Succeed && q.OPER_TYPE != 2
                           select f;

                var templist = temp.ToList();

                List<TrafficRoute> fences = new List<TrafficRoute>();
                foreach (var item in templist)
                {
                    fences.Add(TrafficRouteUtility.GetModel(item));
                }

                return new MultiMessage<TrafficRoute>(fences, fences.Count);
            }
            else
            {
                return new MultiMessage<TrafficRoute>(null, 0);
            }
        }

        public static MultiMessage<TrafficRoute> GetTrafficRouteListByVehicleIDAndRouteName(PTMSEntities context, string clientID, string vehicleID, string routeName)
        {
            int totalCount;
            string innerRouteName = string.Empty;
            string innerVehicleID = string.Empty;

            if (!string.IsNullOrEmpty(vehicleID))
            {
                innerVehicleID = vehicleID.ToUpper().Trim();
            }

            if (!string.IsNullOrEmpty(routeName))
            {
                innerRouteName = routeName.ToUpper().Trim();
            }

            if (!string.IsNullOrEmpty(innerRouteName) && !string.IsNullOrEmpty(innerVehicleID))
            {
                var temp = from f in context.TRF_ROUTE
                           join v in context.TRF_ROUTE_QUEUE on f.ID equals v.ROUTE_ID
                           where f.CLIENT_ID == clientID && v.VEHICLE_ID.ToUpper().Contains(innerVehicleID) && f.NAME.ToUpper().Contains(innerRouteName) && !(v.OPER_TYPE == 2 && v.STATUS == (short)CommandStateEnum.Succeed)
                           select f;
                var templist = temp.ToList();

                List<TrafficRoute> routes = new List<TrafficRoute>();
                foreach (var item in templist)
                {
                    routes.Add(TrafficRouteUtility.GetModel(item));
                }

                totalCount = routes.Count;

                return new MultiMessage<TrafficRoute>(routes, totalCount);
            }
            else if (!string.IsNullOrEmpty(innerRouteName))
            {
                var temp = from f in context.TRF_ROUTE
                           where f.CLIENT_ID == clientID && f.NAME.ToUpper().Contains(innerRouteName)
                           select f;

                var templist = temp.ToList();

                List<TrafficRoute> routes = new List<TrafficRoute>();
                foreach (var item in templist)
                {
                    routes.Add(TrafficRouteUtility.GetModel(item));
                }

                totalCount = routes.Count;

                return new MultiMessage<TrafficRoute>(routes, totalCount);
            }
            else if (!string.IsNullOrEmpty(innerVehicleID))
            {
                var temp = from f in context.TRF_ROUTE
                           join v in context.TRF_ROUTE_QUEUE on f.ID equals v.ROUTE_ID
                           where f.CLIENT_ID == clientID && v.VEHICLE_ID.ToUpper().Contains(innerVehicleID) && !(v.OPER_TYPE == 2 && v.STATUS == (short)CommandStateEnum.Succeed)
                           select f;

                var templist = temp.ToList();

                List<TrafficRoute> routes = new List<TrafficRoute>();
                foreach (var item in templist)
                {
                    routes.Add(TrafficRouteUtility.GetModel(item));
                }

                totalCount = routes.Count;

                return new MultiMessage<TrafficRoute>(routes, totalCount);
            }
            else
            {
                var temp = from f in context.TRF_ROUTE
                           where f.CLIENT_ID == clientID
                           select f;

                var templist = temp.ToList();

                List<TrafficRoute> routes = new List<TrafficRoute>();
                foreach (var item in templist)
                {
                    routes.Add(TrafficRouteUtility.GetModel(item));
                }

                totalCount = routes.Count;

                return new MultiMessage<TrafficRoute>(routes, totalCount);
            }
        }

        public static SingleMessage<bool> ObsoleteTrafficeRoute(PTMSEntities context, string routeID)
        {
            TRF_ROUTE entity = context.TRF_ROUTE.FirstOrDefault(t => t.ID == routeID);
            if (entity != null)
            {
                entity.VALID = 0;
                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        public static SingleMessage<bool> IsRouteDelivered(PTMSEntities context, string routeID)
        {
            TRF_ROUTE_QUEUE entity = context.TRF_ROUTE_QUEUE.FirstOrDefault(t => t.ROUTE_ID == routeID);
            if (entity != null)
            {
                return new SingleMessage<bool>(true);
            }
            else
            {
                return new SingleMessage<bool>(false);
            }
        }



        public static MultiMessage<RouteQueue> GetTrafficRouteListOnVehicleByVehicleID(PTMSEntities context, string vehicleID, string clientID)
        {
            if (!string.IsNullOrEmpty(vehicleID))
            {
                vehicleID = vehicleID.ToUpper();
                var temp = from f in context.TRF_ROUTE_QUEUE
                           join q in context.TRF_ROUTEQUEUE_ONVEHCILE on f.ID equals q.ROUTE_ID
                           where q.VEHICLE_ID.ToUpper().Contains(vehicleID) && q.STATUS == (short)CommandStateEnum.Succeed && q.OPER_TYPE != 2
                           select f;

                var templist = temp.ToList();

                List<RouteQueue> routes = new List<RouteQueue>();
                foreach (var item in templist)
                {
                    routes.Add(RouteQueueUtility.GetModel(item));
                }

                return new MultiMessage<RouteQueue>(routes, routes.Count);
            }
            else
            {
                return new MultiMessage<RouteQueue>(null, 0);
            }
        }
    }
}

