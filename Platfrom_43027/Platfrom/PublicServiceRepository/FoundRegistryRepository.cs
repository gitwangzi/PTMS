using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using System.ServiceModel;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.PublicService.Contract;
using Gsafety.PTMS.Common.Data;
using System.Data;
namespace GSafety.PTMS.PublicService.Repository
{
    ///<summary>
    ///拾到物
    ///</summary>
    public class FoundRegistryRepository
    {

        #region FoundRegistrypingtai....
        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加拾到物
        /// </summary>
        /// <param name="model">拾到物</param>
        public static SingleMessage<bool> InsertFoundRegistry(PTMSEntities context, FoundRegistry model)
        {
            var entity = new RUN_FOUND_REGISTRY();
            FoundRegistryUtility.UpdateEntity(entity, model, true);

            context.RUN_FOUND_REGISTRY.Add(entity);

            return context.Save();
        }

        /// <summary>
        /// 修改拾到物
        /// </summary>
        public static SingleMessage<bool> UpdateFoundRegistry(PTMSEntities context, FoundRegistry model)
        {
            var entity = context.RUN_FOUND_REGISTRY.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            FoundRegistryUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = EntityState.Modified;
            return context.Save();
        }

        /// <summary>
        /// 删除拾到物
        /// </summary>
        public static SingleMessage<bool> DeleteFoundRegistryByID(PTMSEntities context, string ID)
        {
            RUN_FOUND_REGISTRY entity = context.RUN_FOUND_REGISTRY.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.RUN_FOUND_REGISTRY.Attach(entity);
                context.RUN_FOUND_REGISTRY.Remove(entity);
                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        /// <summary>
        /// 获取拾到物
        /// </summary>
        public static SingleMessage<FoundRegistry> GetFoundRegistry(PTMSEntities context, string ID)
        {
            RUN_FOUND_REGISTRY entity = context.RUN_FOUND_REGISTRY.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                FoundRegistry model = FoundRegistryUtility.GetModel(entity);
                return new SingleMessage<FoundRegistry>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取拾到物
        /// </summary>
        public static MultiMessage<FoundRegistry> GetFoundRegistryList(PTMSEntities context, int pageIndex, int pageSize, string clientID)
        {
            int totalCount;
            var sour = from v in context.RUN_FOUND_REGISTRY
                       where v.CLIENT_ID == clientID
                       select v;
            var items = sour.Page(out totalCount, pageIndex, pageSize, true)
                 .ToList().Select(t => FoundRegistryUtility.GetModel(t))
                 .OrderByDescending(s => s.CreateTime).ToList();

            return new MultiMessage<FoundRegistry>(items, totalCount);
        }


        public static MultiMessage<FoundRegistry> GetFoundRegistryByConditionList(PTMSEntities context, int pageIndex, int pageSize, string clientID, string Founder, string Keyword, string LostName)
        {

            int totalCount;

            var sour = from v in context.RUN_FOUND_REGISTRY
                       where v.CLIENT_ID == clientID && ((Founder == null || Founder == "") ? true : v.FOUNDER.Contains(Founder))
                       && ((Keyword == null || Keyword == "") ? true : v.KEYWORD.Contains(Keyword))
                       && ((LostName == null || LostName == "") ? true : v.LOST_NAME.Contains(LostName))
                       select v;
            var items = sour.Page(out totalCount, pageIndex, pageSize, true)
                .ToList().Select(t => FoundRegistryUtility.GetModel(t))
                .OrderByDescending(s => s.CreateTime).ToList();
            return new MultiMessage<FoundRegistry>(items, totalCount);

        }

        #endregion

        #region mobile...
        public static MultiMessage<FoundRegistry> GetFoundRegistryByMobile(PTMSEntities context, string sim, string pageIndex, string pageSize, DateTime starttime, DateTime endtime, int? type)
        {
            int pageIndextemp = Convert.ToInt32(pageIndex);
            int pageSizetemp = Convert.ToInt32(pageSize);
            int totalCount;
            var sour = from fr in context.RUN_FOUND_REGISTRY
                       where fr.FOUND_PHONE == sim && fr.FOUND_TIME > starttime && fr.FOUND_TIME < endtime && (type == null ? fr.STATUS < 2 : fr.STATUS == type)
                       select fr;

            var items = sour.Page(out totalCount, pageIndextemp, pageSizetemp, true)
                 .ToList().Select(t => FoundRegistryUtility.GetModel(t))
                 .OrderByDescending(s => s.FoundTime).ToList();

            return new MultiMessage<FoundRegistry>(items, totalCount);
        }


        public static MultiMessage<MaintainApplication> GetApplicationByMobile(PTMSEntities context, string sim, string pageIndex, string pageSize)
        {
            int pageIndextemp = Convert.ToInt32(pageIndex);
            int pageSizetemp = Convert.ToInt32(pageSize);

            int totalCount;
            var sour = from fr in context.MTN_MAINTAIN_APPLICATION
                       where fr.WORKER_PHONE == sim
                       select fr;

            var items = sour.Page(out totalCount, pageIndextemp, pageSizetemp, true)
                 .ToList().Select(t => MaintainApplicationUtility.GetModel(t))
                 .OrderByDescending(s => s.CreateTime).ToList();

            return new MultiMessage<MaintainApplication>(items, totalCount);
        }

        public static MultiMessage<MaintainApplication> GetApplicationByVehicle(PTMSEntities context, string vehicle, string pageIndex, string pageSize, DateTime starttime, DateTime endtime, int? type)
        {
            int pageIndextemp = Convert.ToInt32(pageIndex);
            int pageSizetemp = Convert.ToInt32(pageSize);
            int totalCount;

            var sour = from fr in context.MTN_MAINTAIN_APPLICATION
                       where fr.CREATE_TIME > starttime && fr.CREATE_TIME < endtime && (type == null ? fr.STATUS < 5 : fr.STATUS == type)
                       select fr;
            var items = sour.Page(out totalCount, pageIndextemp, pageSizetemp, true, n => n.CREATE_TIME, false).ToList();

            List<MaintainApplication> results = new List<MaintainApplication>();
            List<string> stations = new List<string>();
            foreach (var item in items)
            {
                MaintainApplication ma = MaintainApplicationUtility.GetModel(item);
                if (!string.IsNullOrEmpty(ma.SetupStation) && !stations.Contains(ma.SetupStation))
                {
                    stations.Add(ma.SetupStation);
                }
                results.Add(ma);
            }

            var temp = context.BSC_SETUP_STATION.Where(n => stations.Contains(n.ID)).ToList();
            foreach (var item in results)
            {
                if (!string.IsNullOrEmpty(item.SetupStation))
                {
                    item.SetupStation = temp.FirstOrDefault(n => n.ID == item.SetupStation).NAME;
                }
            }


            return new MultiMessage<MaintainApplication>(results, results.Count);
        }

        public static SingleMessage<bool> InsertAddApplication(PTMSEntities context, MaintainApplication model)
        {
            var entity = new MTN_MAINTAIN_APPLICATION();
            MaintainApplicationUtility.UpdateEntity(entity, model, true);

            context.MTN_MAINTAIN_APPLICATION.Add(entity);

            return context.Save();
        }


        public static SingleMessage<FoundRegistry> GetFoundRegistryByID(PTMSEntities context, string id)
        {
            var result = from fr in context.RUN_FOUND_REGISTRY
                         where fr.ID == id
                         select new FoundRegistry
                         {
                             ClientID = fr.CLIENT_ID,
                             Founder = fr.FOUNDER,
                             FounderIDCard = fr.FOUNDER_ID_CARD,
                             FoundPhone = fr.FOUND_PHONE,
                             FoundTime = fr.FOUND_TIME,
                             Content = fr.CONTENT,
                             Keyword = fr.KEYWORD,
                             Address = fr.ADDRESS,
                             Status = fr.STATUS,
                             CreateTime = fr.CREATE_TIME.Value,
                             ClaimTime = fr.CLAIM_TIME.HasValue ? fr.CLAIM_TIME.Value : new Nullable<DateTime>(),
                             VehicleID = fr.VEHICLE_ID,
                             LostName = fr.LOST_NAME,
                             LostPhone = fr.LOST_PHONE
                         };

            FoundRegistry model = result.FirstOrDefault();

            if (model != null)
            {
                return new SingleMessage<FoundRegistry>(model);
            }
            else
            {
                return new SingleMessage<FoundRegistry>(false, "NotFound");
            }
        }

        public static SingleMessage<MaintainApplication> GetMaintainApplicationByID(PTMSEntities context, string id)
        {
            var result = from fr in context.MTN_MAINTAIN_APPLICATION
                         where fr.ID == id
                         select new MaintainApplication
                         {
                             ID = fr.ID,
                             ClientID = fr.CLIENT_ID,
                             Applicant = fr.APPLICANT,
                             Contact = fr.CONTACT,
                             Problem = fr.PROBLEM,
                             SetupStation=fr.SETUP_STATION,
                             Status = fr.STATUS,
                             CreateTime = fr.CREATE_TIME,
                             VehicleID = fr.VEHCILE_ID,
                             ScheduleDate = fr.SCHEDULE_DATE.Value,
                             Worker = fr.WORKER,
                             WorkerPhone = fr.WORKER_PHONE
                         };

            MaintainApplication model = result.FirstOrDefault();

            if (model != null)
            {
                if (model.SetupStation != null)
                {
                    var station = context.BSC_SETUP_STATION.FirstOrDefault(n => n.ID == model.SetupStation);
                    if (station != null)
                    {
                        model.SetupStation = station.NAME;
                    }
                }
                return new SingleMessage<MaintainApplication>(model);
            }
            else
            {
                return new SingleMessage<MaintainApplication>(false, "NotFound");
            }
        }

        public static SingleMessage<bool> UpdateFoundRegistry(PTMSEntities context, MaintainApplication model)
        {
            var entity = context.MTN_MAINTAIN_APPLICATION.FirstOrDefault(n => n.ID == model.ID);
            if (entity != null)
            {
                MaintainApplicationUtility.UpdateEntity(entity, model, true);

                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "NotFound");
            }

        }

        #endregion
    }
}

