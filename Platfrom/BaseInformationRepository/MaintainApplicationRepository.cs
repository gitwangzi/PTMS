using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Data;
using GSafety.PTMS.PublicService.Repository;
using System.Data;
namespace Gsafety.PTMS.BaseInformation.Repository
{
	///<summary>
	///维修申请
	///</summary>
	public class MaintainApplicationRepository 
	{

		static string FailedToSave = "Failed to Save to DB";
		/// <summary>
		/// 添加维修申请
		/// </summary>
		/// <param name="model">维修申请</param>
		public static SingleMessage<bool> InsertMaintainApplication(PTMSEntities context,MaintainApplication model)
		{
            var entity = new MTN_MAINTAIN_APPLICATION();
            MaintainApplicationUtility.UpdateEntity(entity, model, true);

            context.MTN_MAINTAIN_APPLICATION.Add(entity);

			return context.Save();
		}

		/// <summary>
		/// 修改维修申请
		/// </summary>
		public static SingleMessage<bool> UpdateMaintainApplication(PTMSEntities context,MaintainApplication model)
		{
            var entity = context.MTN_MAINTAIN_APPLICATION.FirstOrDefault(t => t.ID == model.ID);
			if (null == entity)
			{
				return new SingleMessage<bool>(false, "");
			}

			MaintainApplicationUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = EntityState.Modified;
			return context.Save();
		}

		/// <summary>
		/// 删除维修申请
		/// </summary>
		public static SingleMessage<bool> DeleteMaintainApplicationByID(PTMSEntities context,string ID)
		{
            MTN_MAINTAIN_APPLICATION entity = context.MTN_MAINTAIN_APPLICATION.FirstOrDefault(t => t.ID == ID);
			if (entity != null)
			{
                context.MTN_MAINTAIN_APPLICATION.Attach(entity);
                context.MTN_MAINTAIN_APPLICATION.Remove(entity);
				return context.Save();
			}
			else
			{
				return new SingleMessage<bool>(false, "");
			}
		}

		/// <summary>
		/// 获取维修申请
		/// </summary>
		public static SingleMessage<MaintainApplication> GetMaintainApplication(PTMSEntities context,string ID)
		{
            MTN_MAINTAIN_APPLICATION entity = context.MTN_MAINTAIN_APPLICATION.SingleOrDefault(n => n.ID == ID);
			if (entity != null)
			{
				MaintainApplication model = MaintainApplicationUtility.GetModel(entity);
				return new SingleMessage<MaintainApplication>(model);
			}
			return null;
		}

		/// <summary>
		/// 获取维修申请
		/// </summary>
        public static MultiMessage<MaintainApplication> GetMaintainApplicationList(PTMSEntities context, string clientID, int pageIndex = 1, int pageSize = 10)
		{
			int totalCount;
            var list = context.MTN_MAINTAIN_APPLICATION.Page(out totalCount, pageIndex, pageSize, true).ToList();

			var items = list.Select(t => MaintainApplicationUtility.GetModel(t)).ToList();

            return new MultiMessage<MaintainApplication>(items, totalCount);
		}

        /// <summary>
        /// 获取维修申请
        /// </summary>
        public static MultiMessage<MaintainApplication> GetMaintainApplicationByCondition(PTMSEntities context, string clientID,string name, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            var list = context.MTN_MAINTAIN_APPLICATION.Page(out totalCount, pageIndex, pageSize, true).ToList();

            var items = list.Select(t => MaintainApplicationUtility.GetModel(t)).ToList();

            return new MultiMessage<MaintainApplication>(items, totalCount);
        }

	}
}

