using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Data.Enum;
using System.Data;
namespace Gsafety.PTMS.Traffic.Repository
{
    ///<summary>
    ///
    ///</summary>
    public class TrafficFenceRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public static SingleMessage<bool> InsertTrafficFence(PTMSEntities context, TrafficFence model)
        {
            var entity = new TRF_FENCE();
            TrafficFenceUtility.UpdateEntity(entity, model, true);

            context.TRF_FENCE.Add(entity);

            return context.Save();
        }

        /// <summary>
        /// 修改
        /// </summary>
        public static SingleMessage<bool> UpdateTrafficFence(PTMSEntities context, TrafficFence model)
        {
            var entity = context.TRF_FENCE.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            TrafficFenceUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = EntityState.Modified;
            return context.Save();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public static SingleMessage<bool> DeleteTrafficFenceByID(PTMSEntities context, string ID)
        {
            TRF_FENCE entity = context.TRF_FENCE.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.TRF_FENCE.Attach(entity);
                context.TRF_FENCE.Remove(entity);

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
        public static SingleMessage<TrafficFence> GetTrafficFence(PTMSEntities context, string ID)
        {
            TRF_FENCE entity = context.TRF_FENCE.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                TrafficFence model = TrafficFenceUtility.GetModel(entity);
                return new SingleMessage<TrafficFence>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取
        /// </summary>
        public static MultiMessage<TrafficFence> GetTrafficFenceList(PTMSEntities context, string clientID, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            var list = context.TRF_FENCE.Where(n => n.CLIENT_ID == clientID).Page(out totalCount, pageIndex, pageSize, true).ToList();

            var items = list.Select(t => TrafficFenceUtility.GetModel(t)).ToList();

            return new MultiMessage<TrafficFence>(items, totalCount);
        }


        public static SingleMessage<bool> IsTrafficExists(PTMSEntities context, string fenceName, string clientID, string fenceID)
        {
            TRF_FENCE entity = null;
            if (string.IsNullOrEmpty(fenceID))
            {
                entity = context.TRF_FENCE.FirstOrDefault(n => n.CLIENT_ID == clientID && n.NAME == fenceName);
            }
            else
            {
                entity = context.TRF_FENCE.FirstOrDefault(n => n.CLIENT_ID == clientID && n.NAME == fenceName && n.ID != fenceID);
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

        public static MultiMessage<TrafficFence> GetTrafficFenceListByVehicleIDAndFenceName(PTMSEntities context, string clientID, string vehicleID, string fenceName)
        {
            int totalCount;
            string innerFenceName = string.Empty;
            string innerVehicleID = string.Empty;

            if (!string.IsNullOrEmpty(vehicleID))
            {
                innerVehicleID = vehicleID.ToUpper().Trim();
            }

            if (!string.IsNullOrEmpty(fenceName))
            {
                innerFenceName = fenceName.ToUpper().Trim();
            }

            if (!string.IsNullOrEmpty(innerFenceName) && !string.IsNullOrEmpty(innerVehicleID))
            {
                var temp = from f in context.TRF_FENCE
                           join v in context.TRF_FENCE_QUEUE on f.ID equals v.FENCE_ID
                           where f.CLIENT_ID == clientID && v.VEHICLE_ID.ToUpper().Contains(innerVehicleID) && f.NAME.ToUpper().Contains(innerFenceName) && !(v.OPER_TYPE == 2 && v.STATUS == (short)CommandStateEnum.Succeed)
                           select f;
                var templist = temp.ToList();

                List<TrafficFence> fences = new List<TrafficFence>();
                foreach (var item in templist)
                {
                    fences.Add(TrafficFenceUtility.GetModel(item));
                }

                totalCount = fences.Count;

                return new MultiMessage<TrafficFence>(fences, totalCount);
            }
            else if (!string.IsNullOrEmpty(innerFenceName))
            {
                var temp = from f in context.TRF_FENCE
                           where f.CLIENT_ID == clientID && f.NAME.ToUpper().Contains(innerFenceName)
                           select f;

                var templist = temp.ToList();

                List<TrafficFence> fences = new List<TrafficFence>();
                foreach (var item in templist)
                {
                    fences.Add(TrafficFenceUtility.GetModel(item));
                }

                totalCount = fences.Count;

                return new MultiMessage<TrafficFence>(fences, totalCount);
            }
            else if (!string.IsNullOrEmpty(innerVehicleID))
            {
                var temp = from f in context.TRF_FENCE
                           join v in context.TRF_FENCE_QUEUE on f.ID equals v.FENCE_ID
                           where f.CLIENT_ID == clientID && v.VEHICLE_ID.ToUpper().Contains(innerVehicleID) && !(v.OPER_TYPE == 2 && v.STATUS == (short)CommandStateEnum.Succeed)
                           select f;

                var templist = temp.ToList();

                List<TrafficFence> fences = new List<TrafficFence>();
                foreach (var item in templist)
                {
                    fences.Add(TrafficFenceUtility.GetModel(item));
                }

                totalCount = fences.Count;

                return new MultiMessage<TrafficFence>(fences, totalCount);
            }
            else
            {
                var temp = from f in context.TRF_FENCE
                           where f.CLIENT_ID == clientID
                           select f;

                var templist = temp.ToList();

                List<TrafficFence> fences = new List<TrafficFence>();
                foreach (var item in templist)
                {
                    fences.Add(TrafficFenceUtility.GetModel(item));
                }

                totalCount = fences.Count;

                return new MultiMessage<TrafficFence>(fences, totalCount);
            }


        }

        public static MultiMessage<TrafficFence> GetDeliveredTrafficFenceListByVehicleID(PTMSEntities context, string vehicleID, string clientID)
        {
            if (!string.IsNullOrEmpty(vehicleID))
            {
                vehicleID = vehicleID.ToUpper();
                var temp = from f in context.TRF_FENCE
                           join q in context.TRF_FENCE_QUEUE on f.ID equals q.FENCE_ID
                           where q.VEHICLE_ID.ToUpper().Contains(vehicleID) && q.STATUS == (short)CommandStateEnum.Succeed && q.OPER_TYPE != 2
                           select f;

                var templist = temp.ToList();

                List<TrafficFence> fences = new List<TrafficFence>();
                foreach (var item in templist)
                {
                    fences.Add(TrafficFenceUtility.GetModel(item));
                }

                return new MultiMessage<TrafficFence>(fences, fences.Count);
            }
            else
            {
                return new MultiMessage<TrafficFence>(null, 0);
            }
        }

        public static MultiMessage<TrafficFence> GetTrafficFenceListOnVehicleByVehicleID(PTMSEntities context, string vehicleID, string clientID)
        {
            if (!string.IsNullOrEmpty(vehicleID))
            {
                vehicleID = vehicleID.ToUpper();
                var temp = from f in context.TRF_FENCE
                           join q in context.TRF_FENCEQUEUE_ONVEHICLE on f.ID equals q.FENCE_ID
                           where q.VEHICLE_ID.ToUpper().Contains(vehicleID) && q.STATUS == (short)CommandStateEnum.Succeed && q.OPER_TYPE != 2
                           select f;

                var templist = temp.ToList();

                List<TrafficFence> fences = new List<TrafficFence>();
                foreach (var item in templist)
                {
                    fences.Add(TrafficFenceUtility.GetModel(item));
                }

                return new MultiMessage<TrafficFence>(fences, fences.Count);
            }
            else
            {
                return new MultiMessage<TrafficFence>(null, 0);
            }
        }

        public static SingleMessage<bool> IsFenceDelivered(PTMSEntities context, string fenceID)
        {
            TRF_FENCE_QUEUE entity = null;
            if (!string.IsNullOrEmpty(fenceID))
            {
                entity = context.TRF_FENCE_QUEUE.FirstOrDefault(n => n.FENCE_ID == fenceID);
            }
            else
            {
                return new SingleMessage<bool>(false);
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

        public static SingleMessage<bool> ObsoleteFence(PTMSEntities context, string fenceID)
        {
            TRF_FENCE entity = context.TRF_FENCE.FirstOrDefault(n => n.ID == fenceID);

            entity.VALID = 0;

            return context.Save();
        }
    }
}

