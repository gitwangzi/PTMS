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
    public class FenceQueueRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public static SingleMessage<bool> InsertFenceQueue(PTMSEntities context, List<Gsafety.PTMS.Common.Data.FenceQueue> fencequeues)
        {
            List<string> fenceids = fencequeues.Select(n => n.FenceID).Distinct().ToList();
            List<string> temp = context.TRF_FENCEQUEUE_TOVEHICLE.Where(n => !(n.OPER_TYPE == 2 && n.STATUS == (short)CommandStateEnum.Succeed)).Where(n => fenceids.Contains(n.FENCE_ID)).Select(n => n.VEHICLE_ID).ToList();
            List<string> vehicles = fencequeues.Select(n => n.VehicleID).Distinct().ToList();
            List<RUN_SUITE_WORKING> workingsuites = context.RUN_SUITE_WORKING.Where(n => vehicles.Contains(n.VEHICLE_ID)).ToList();

            bool shouldsave = false;
            foreach (var item in fencequeues)
            {
                if (!temp.Contains(item.VehicleID))
                {
                    var entity = new TRF_FENCE_QUEUE();
                    RUN_SUITE_WORKING workingsuite = workingsuites.FirstOrDefault(n => n.VEHICLE_ID == item.VehicleID);
                    if (workingsuite != null)
                    {
                        item.MdvrCoreSn = workingsuite.MDVR_CORE_SN;
                        FenceQueueUtility.UpdateEntity(entity, item, true);
                        context.TRF_FENCE_QUEUE.Add(entity);
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

        /// <summary>
        /// 修改
        /// </summary>
        public static SingleMessage<bool> UpdateFenceQueue(PTMSEntities context, FenceQueue model)
        {
            var entity = context.TRF_FENCE_QUEUE.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            FenceQueueUtility.UpdateEntity(entity, model, false);

            return context.Save();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public static SingleMessage<bool> DeleteFenceQueueByID(PTMSEntities context, string ID)
        {
            TRF_FENCE_QUEUE entity = context.TRF_FENCE_QUEUE.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                //if (entity.OPER_TYPE == 0 && (entity.STATUS == (short)CommandStateEnum.UnDelivered || entity.STATUS == (short)CommandStateEnum.WaitForDeliver || entity.STATUS == (short)CommandStateEnum.Failed))
                //{
                //    context.TRF_FENCE_QUEUE.Remove(entity);
                //}
                //else if (entity.OPER_TYPE == 0 && entity.STATUS == (short)CommandStateEnum.Succeed)
                //{
                //    entity.OPER_TYPE = 2;
                //    entity.STATUS = (short)CommandStateEnum.WaitForDeliver;
                //}
                //return context.Save();

                if (entity.STATUS == (short)CommandStateEnum.UnDelivered || entity.STATUS == (short)CommandStateEnum.Failed || entity.STATUS == (short)CommandStateEnum.Succeed || entity.STATUS == (short)CommandStateEnum.WaitForDeliver)
                {
                    context.TRF_FENCE_QUEUE.Attach(entity);
                    context.TRF_FENCE_QUEUE.Remove(entity);
                    return context.Save();
                }
                else
                {
                    return new SingleMessage<bool>(false, "OperatorServiceError");
                }
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        public static SingleMessage<FenceQueue> GetFenceQueue(PTMSEntities context, string ID)
        {
            TRF_FENCE_QUEUE entity = context.TRF_FENCE_QUEUE.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                FenceQueue model = FenceQueueUtility.GetModel(entity);
                return new SingleMessage<FenceQueue>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取
        /// </summary>
        public static MultiMessage<FenceQueue> GetFenceQueueList(PTMSEntities context, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            var list = context.TRF_FENCE_QUEUE.Page(out totalCount, pageIndex, pageSize, true).ToList();

            var items = list.Select(t => FenceQueueUtility.GetModel(t)).ToList();

            return new MultiMessage<FenceQueue>(items, totalCount);
        }


        public static MultiMessage<FenceQueue> GetFenceQueueListByFenceID(PTMSEntities context, string fenceID, string clientID, string vehicleName, int pageIndex, int pageSize)
        {
            int totalCount;
            List<TRF_FENCE_QUEUE> temp = null;
            if (string.IsNullOrEmpty(vehicleName))
            {
                var list = from fq in context.TRF_FENCE_QUEUE
                           join v in context.TRF_FENCEQUEUE_TOVEHICLE on fq.ID equals v.ID
                           where v.FENCE_ID == fenceID && !(fq.OPER_TYPE == 2 && fq.STATUS == (short)(CommandStateEnum.Succeed))
                           select fq;

                temp = list.Page(out totalCount, pageIndex, pageSize, true).ToList();
            }
            else
            {
                string vehicle = vehicleName.ToUpper();
                var list = from fq in context.TRF_FENCE_QUEUE
                           join v in context.TRF_FENCEQUEUE_TOVEHICLE on fq.ID equals v.ID
                           where v.FENCE_ID == fenceID && v.VEHICLE_ID.ToUpper().Contains(vehicle)
                           select fq;
                temp = list.Page(out totalCount, pageIndex, pageSize, true).ToList();
            }
            var items = temp.Select(t => FenceQueueUtility.GetModel(t)).ToList();

            return new MultiMessage<FenceQueue>(items, totalCount);
        }

        public static MultiMessage<FenceQueue> GetAllFenceQueueListByFenceID(PTMSEntities context, string fenceID, string clientID)
        {
            var list = from fq in context.TRF_FENCE_QUEUE
                       join v in context.TRF_FENCEQUEUE_TOVEHICLE on fq.ID equals v.ID
                       where v.FENCE_ID == fenceID && !(fq.OPER_TYPE == 2 && fq.STATUS == (short)(CommandStateEnum.Succeed))
                       select fq;

            List<TRF_FENCE_QUEUE> temp = list.ToList();
            List<FenceQueue> items = new List<FenceQueue>();
            foreach (var item in temp)
            {
                items.Add(FenceQueueUtility.GetModel(item));
            }

            return new MultiMessage<FenceQueue>(items, items.Count);
        }

        public static SingleMessage<bool> DeliverFenceQueueToVehicle(PTMSEntities context, List<FenceQueue> vehicles)
        {
            List<string> ids = vehicles.Select(n => n.ID).ToList();

            var tempqueues = from hv in context.TRF_FENCE_QUEUE
                             where ids.Contains(hv.ID) && hv.STATUS == (short)CommandStateEnum.UnDelivered
                             select hv;

            if (tempqueues.Count() == 0)
            {
                return new SingleMessage<bool>(true);
            }

            var temp = from hv in context.TRF_FENCE_QUEUE
                       group hv by hv.VEHICLE_ID into grp
                       select new
                       {
                           VehicleID = grp.Key,
                           Value = grp.Max(n => n.REGION_ID.HasValue ? n.REGION_ID.Value : 0)
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

    }
}

