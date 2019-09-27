using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using System.ServiceModel;
using Gsafety.PTMS.PublicService.Contract;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Base.Contract.Data;
namespace GSafety.PTMS.PublicService.Repository
{
    ///<summary>
    ///丢失登记
    ///</summary>
    public class LostRegistryRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加丢失登记
        /// </summary>
        /// <param name="model">丢失登记</param>
        public static SingleMessage<bool> InsertLostRegistry(PTMSEntities context, LostRegistry model)
        {
            var entity = new RUN_LOST_REGISTRY();
            LostRegistryUtility.UpdateEntity(entity, model, true);

            context.RUN_LOST_REGISTRY.Add(entity);

            return context.Save();
        }

        /// <summary>
        /// 修改丢失登记
        /// </summary>
        public static SingleMessage<bool> UpdateLostRegistry(PTMSEntities context, LostRegistry model)
        {
            var entity = context.RUN_LOST_REGISTRY.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            LostRegistryUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = System.Data.EntityState.Modified;
            return context.Save();
        }

        /// <summary>
        /// 删除丢失登记
        /// </summary>
        public static SingleMessage<bool> DeleteLostRegistryByID(PTMSEntities context, string ID)
        {
            RUN_LOST_REGISTRY entity = context.RUN_LOST_REGISTRY.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.RUN_LOST_REGISTRY.Attach(entity);
                context.RUN_LOST_REGISTRY.Remove(entity);
                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        /// <summary>
        /// 获取丢失登记
        /// </summary>
        public static SingleMessage<LostRegistry> GetLostRegistry(PTMSEntities context, string ID)
        {
            RUN_LOST_REGISTRY entity = context.RUN_LOST_REGISTRY.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                LostRegistry model = LostRegistryUtility.GetModel(entity);
                return new SingleMessage<LostRegistry>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取丢失登记
        /// </summary>
        public static MultiMessage<LostRegistry> GetLostRegistryList(PTMSEntities context, int pageIndex, int pageSize,string clientID)
        {
            int totalCount;

            MultiMessage<LostRegistry> result = new MultiMessage<LostRegistry>();

            var sour = from v in context.RUN_LOST_REGISTRY
                       where v.CLIENT_ID == clientID
                       select v;
            var items = sour.Page(out totalCount, pageIndex, pageSize, true, n => n.CREATE_TIME, false)
                 .Select(t => LostRegistryUtility.GetModel(t))
                 .ToList();

            return new MultiMessage<LostRegistry>(items, totalCount);
   
        }

        /// <summary>
        /// 获取丢失登记
        /// </summary>
        public static MultiMessage<LostRegistry> GetLostRegistryByConditionList(PTMSEntities context, int pageIndex, int pageSize, string clientID, string LostName, string Keyword, string LostIDCard)
        {
            int totalCount;
           
            MultiMessage<LostRegistry> result = new MultiMessage<LostRegistry>();

            var sour = from v in context.RUN_LOST_REGISTRY
                       where v.CLIENT_ID == clientID && ((LostIDCard == null || LostIDCard == "") ? true : v.LOST_IDCARD.Contains(LostIDCard))
                       && ((Keyword == null || Keyword == "") ? true : v.KEYWORD.Contains(Keyword))
                       && ((LostName == null || LostName == "") ? true : v.LOST_NAME.Contains(LostName))
                       select v;
            var items = sour.Page(out totalCount, pageIndex, pageSize, true, n => n.CREATE_TIME, false)
                .ToList()
                .Select(t => LostRegistryUtility.GetModel(t))
                .ToList();

            return new MultiMessage<LostRegistry>(items, totalCount);
   

        }


    }
}

