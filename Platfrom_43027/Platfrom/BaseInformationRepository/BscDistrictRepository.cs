using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
namespace Gsafety.PTMS.BaseInformation.Repository
{
    ///<summary>
    ///行政区域
    ///</summary>
    public class BscDistrictRepository : Gsafety.PTMS.BaseInfo.BaseRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加行政区域
        /// </summary>
        /// <param name="model">行政区域</param>
        public static SingleMessage<bool> InsertBscDistrict(PTMSEntities context, BscDistrict model)
        {
            var entity = new BSC_DISTRICT();
            BscDistrictUtility.UpdateEntity(entity, model, true);

            context.BSC_DISTRICT.Add(entity);

            if (context.SaveChanges() > 0)
                return new SingleMessage<bool>(true);
            else
                return new SingleMessage<bool>(false, FailedToSave);
        }

        /// <summary>
        /// 修改行政区域
        /// </summary>
        public static SingleMessage<bool> UpdateBscDistrict(PTMSEntities context, BscDistrict model)
        {
            var entity = context.BSC_DISTRICT.FirstOrDefault(t => t.CODE == model.Code);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            BscDistrictUtility.UpdateEntity(entity, model, false);

            if (context.SaveChanges() > 0)
                return new SingleMessage<bool>(true);
            else
                return new SingleMessage<bool>(false, FailedToSave);
        }

        /// <summary>
        /// 删除行政区域
        /// </summary>
        public static SingleMessage<bool> DeleteBscDistrictByID(PTMSEntities context, string Code)
        {
            BSC_DISTRICT entity = context.BSC_DISTRICT.FirstOrDefault(t => t.CODE == Code);
            if (entity != null)
            {
                context.BSC_DISTRICT.Attach(entity);
                context.BSC_DISTRICT.Remove(entity);
                if (context.SaveChanges() > 0)
                    return new SingleMessage<bool>(true);
                else
                    return new SingleMessage<bool>(false, FailedToSave);
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        /// <summary>
        /// 获取行政区域
        /// </summary>
        public static SingleMessage<BscDistrict> GetBscDistrict(PTMSEntities context, string Code)
        {
            BSC_DISTRICT entity = context.BSC_DISTRICT.SingleOrDefault(n => n.CODE == Code);
            if (entity != null)
            {
                BscDistrict model = BscDistrictUtility.GetModel(entity);
                return new SingleMessage<BscDistrict>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取行政区域
        /// </summary>
        public static MultiMessage<BscDistrict> GetBscDistrictList(PTMSEntities context, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            var list = context.BSC_DISTRICT.Page(out totalCount, pageIndex, pageSize, true).ToList();

            var items = list.Select(t => BscDistrictUtility.GetModel(t)).ToList();

            return new MultiMessage<BscDistrict>(items, totalCount);
        }

    }
}

