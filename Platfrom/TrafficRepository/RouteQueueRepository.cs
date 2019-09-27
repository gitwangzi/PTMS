using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
namespace Gsafety.PTMS.Traffic.Repository
{
    ///<summary>
    ///
    ///</summary>
    public class RouteQueueRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public static SingleMessage<bool> InsertRouteQueue(PTMSEntities context, RouteQueue model)
        {
            var entity = new TRF_ROUTE_QUEUE();
            RouteQueueUtility.UpdateEntity(entity, model, true);

            context.TRF_ROUTE_QUEUE.Add(entity);

            return context.Save();
        }

        /// <summary>
        /// 修改
        /// </summary>
        public static SingleMessage<bool> UpdateRouteQueue(PTMSEntities context, RouteQueue model)
        {
            var entity = context.TRF_ROUTE_QUEUE.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            RouteQueueUtility.UpdateEntity(entity, model, false);

            return context.Save();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public static SingleMessage<bool> DeleteRouteQueueByID(PTMSEntities context, string ID)
        {

            TRF_ROUTE_QUEUE entity = context.TRF_ROUTE_QUEUE.FirstOrDefault(t => t.ID == ID);

            if (entity != null)
            {
                if (entity.STATUS == (short)CommandStateEnum.UnDelivered || entity.STATUS == (short)CommandStateEnum.Failed || entity.STATUS == (short)CommandStateEnum.Succeed || entity.STATUS == (short)CommandStateEnum.WaitForDeliver)
                {
                    context.TRF_ROUTE_QUEUE.Attach(entity);
                    context.TRF_ROUTE_QUEUE.Remove(entity);
                    return context.Save();
                }
                else
                {
                    return new SingleMessage<bool>(false, "OperatorServiceError");
                }
            }
            else
            {
                return new SingleMessage<bool>(false, "OperatorServiceError");
            }
            //TRF_ROUTE_QUEUE entity = context.TRF_ROUTE_QUEUE.FirstOrDefault(t => t.ID == ID);
            //if (entity != null)
            //{
            //    if (entity.OPER_TYPE == 0 && (entity.STATUS == (short)CommandStateEnum.WaitForDeliver || entity.STATUS == (short)CommandStateEnum.UnDelivered || entity.STATUS == (short)CommandStateEnum.Failed))
            //    {
            //        context.TRF_ROUTE_QUEUE.Remove(entity);
            //    }
            //    else if (entity.OPER_TYPE == 0 && entity.STATUS == (short)CommandStateEnum.Succeed)
            //    {
            //        entity.OPER_TYPE = 2;
            //        entity.STATUS = (short)CommandStateEnum.WaitForDeliver;
            //    }
            //    return context.Save();
            //}
            //else
            //{
            //    return new SingleMessage<bool>(false, "");
            //}
        }

        /// <summary>
        /// 获取
        /// </summary>
        public static SingleMessage<RouteQueue> GetRouteQueue(PTMSEntities context, string ID)
        {
            TRF_ROUTE_QUEUE entity = context.TRF_ROUTE_QUEUE.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                RouteQueue model = RouteQueueUtility.GetModel(entity);
                return new SingleMessage<RouteQueue>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取
        /// </summary>
        public static MultiMessage<RouteQueue> GetRouteQueueList(PTMSEntities context, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            var list = context.TRF_ROUTE_QUEUE.Page(out totalCount, pageIndex, pageSize, true).ToList();

            var items = list.Select(t => RouteQueueUtility.GetModel(t)).ToList();

            return new MultiMessage<RouteQueue>(items, totalCount);
        }


        public static SingleMessage<bool> DeliverRouteQueueToVehicle(PTMSEntities context, List<RouteQueue> vehicles)
        {
            List<string> ids = vehicles.Select(n => n.ID).ToList();

            var tempqueues = from hv in context.TRF_ROUTE_QUEUE
                             where ids.Contains(hv.ID) && hv.STATUS == (short)CommandStateEnum.UnDelivered
                             select hv;

            if (tempqueues.Count() == 0)
            {
                return new SingleMessage<bool>(true);
            }

            var temp = from hv in context.TRF_ROUTE_QUEUE
                       group hv by hv.VEHICLE_ID into grp
                       select new
                       {
                           VehicleID = grp.Key,
                           Value = grp.Max(n => n.REGION_ID.Value)
                       };

            var lists = temp.ToList();            

            foreach (var item in tempqueues)
            {
                item.STATUS = (short)CommandStateEnum.WaitForDeliver;

                foreach (var tempvalue in lists)
                {
                    if (item.VEHICLE_ID == tempvalue.VehicleID)
                    {
                        item.REGION_ID = (short)(tempvalue.Value + 1);
                    }
                }
            }

            return context.Save();
        }

        public static MultiMessage<RouteQueue> GetAllRouteQueueListByRouteID(PTMSEntities context, string routeID, string clientID)
        {
            var list = from fq in context.TRF_ROUTE_QUEUE
                       join v in context.TRF_ROUTEQUEUE_TOVEHICLE on fq.ID equals v.ID
                       where v.ROUTE_ID == routeID && !(fq.OPER_TYPE == 2 && fq.STATUS == (short)(CommandStateEnum.Succeed))
                       select fq;

            var templist = list.ToList();
            List<RouteQueue> items = new List<RouteQueue>();
            foreach (var item in templist)
            {
                items.Add(RouteQueueUtility.GetModel(item));
            }

            return new MultiMessage<RouteQueue>(items, items.Count);
        }

        public static MultiMessage<RouteQueue> GetRouteQueueListByRouteID(PTMSEntities context, string routeID, string clientID, string vehicleName, int pageIndex, int pageSize)
        {
            int totalCount;
            List<TRF_ROUTE_QUEUE> temp = null;
            if (string.IsNullOrEmpty(vehicleName))
            {
                var list = from rq in context.TRF_ROUTE_QUEUE
                           join v in context.TRF_ROUTEQUEUE_TOVEHICLE on rq.ID equals v.ID
                           where v.ROUTE_ID == routeID && !(rq.OPER_TYPE == 2 && rq.STATUS == (short)(CommandStateEnum.Succeed))
                           select rq;

                temp = list.Page(out totalCount, pageIndex, pageSize, true).ToList();
            }
            else
            {
                string vehicle = vehicleName.ToUpper();
                var list = from rq in context.TRF_ROUTE_QUEUE
                           join v in context.TRF_ROUTEQUEUE_TOVEHICLE on rq.ID equals v.ID
                           where v.ROUTE_ID == routeID && v.VEHICLE_ID.ToUpper().Contains(vehicle)
                           select rq;
                temp = list.Page(out totalCount, pageIndex, pageSize, true).ToList();
            }
            var items = temp.Select(t => RouteQueueUtility.GetModel(t)).ToList();

            return new MultiMessage<RouteQueue>(items, totalCount);
        }

        public static SingleMessage<bool> InsertFenceQueue(PTMSEntities context, List<RouteQueue> routequeues)
        {
            List<string> fenceids = routequeues.Select(n => n.RouteID).Distinct().ToList();
            List<string> temp = context.TRF_ROUTEQUEUE_TOVEHICLE.Where(n => !(n.OPER_TYPE == 2 && n.STATUS == (short)CommandStateEnum.Succeed)).Where(n => fenceids.Contains(n.ROUTE_ID)).Select(n => n.VEHICLE_ID).ToList();
            List<string> vehicles = routequeues.Select(n => n.VehicleID).Distinct().ToList();
            List<RUN_SUITE_WORKING> workingsuites = context.RUN_SUITE_WORKING.Where(n => vehicles.Contains(n.VEHICLE_ID)).ToList();
            bool shouldsave = false;
            foreach (var item in routequeues)
            {
                if (!temp.Contains(item.VehicleID))
                {
                    RUN_SUITE_WORKING workingsuite = workingsuites.FirstOrDefault(n => n.VEHICLE_ID == item.VehicleID);
                    if (workingsuite != null)
                    {
                        var entity = new TRF_ROUTE_QUEUE();
                        item.MdvrCoreSn = workingsuite.MDVR_CORE_SN;
                        RouteQueueUtility.UpdateEntity(entity, item, true);

                        context.TRF_ROUTE_QUEUE.Add(entity);
                        shouldsave = true;
                    }
                }
            }
            if (!shouldsave)
            {
                return new SingleMessage<bool>(true);
            }
            return context.Save();
        }
    }
}

