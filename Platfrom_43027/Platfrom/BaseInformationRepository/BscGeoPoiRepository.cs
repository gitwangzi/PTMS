using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using System.Data;
namespace Gsafety.PTMS.BaseInformation.Repository
{
    ///<summary>
    ///POI
    ///</summary>
    public class BscGeoPoiRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加POI
        /// </summary>
        /// <param name="model">POI</param>
        public static SingleMessage<bool> InsertBscGeoPoi(PTMSEntities context, BscGeoPoi model)
        {
            var entity = new BSC_GEO_POI();
            BscGeoPoiUtility.UpdateEntity(entity, model, true);

            context.BSC_GEO_POI.Add(entity);

            return context.Save();
        }

        /// <summary>
        /// 修改POI
        /// </summary>
        public static SingleMessage<bool> UpdateBscGeoPoi(PTMSEntities context, BscGeoPoi model)
        {
            var entity = context.BSC_GEO_POI.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            BscGeoPoiUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = EntityState.Modified;
            return context.Save();
        }

        /// <summary>
        /// 删除POI
        /// </summary>
        public static SingleMessage<bool> DeleteBscGeoPoiByID(PTMSEntities context, decimal ID)
        {
            BSC_GEO_POI entity = context.BSC_GEO_POI.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.BSC_GEO_POI.Attach(entity);
                context.BSC_GEO_POI.Remove(entity);
                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        /// <summary>
        /// 获取POI
        /// </summary>
        public static SingleMessage<BscGeoPoi> GetBscGeoPoi(PTMSEntities context, decimal ID)
        {
            BSC_GEO_POI entity = context.BSC_GEO_POI.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                BscGeoPoi model = BscGeoPoiUtility.GetModel(entity);
                return new SingleMessage<BscGeoPoi>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取POI
        /// </summary>
        public static MultiMessage<BscGeoPoi> GetBscGeoPoiList(PTMSEntities context, int pageIndex, int pageSize, string searchContent, decimal property)
        {
            //int totalCount = 0;

            List<BSC_GEO_POI> list = new List<BSC_GEO_POI>();

            if (property < 0)
            {
                list = context.BSC_GEO_POI.Where(n => n.NAME.ToLower().Contains(searchContent)).ToList();
            }
            else
            {
                list = context.BSC_GEO_POI.Where(n => n.NAME.ToLower().Contains(searchContent) && n.PROPERTY == property).ToList();
            }

            var items = list.Select(t => BscGeoPoiUtility.GetModel(t));
            return new MultiMessage<BscGeoPoi>(items.ToList(), items.ToList().Count());
        }

    }
}

