using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.BaseInformation.Contract.Data;
namespace Gsafety.PTMS.BaseInformation.Repository
{
	///<summary>
	///车位位置
	///</summary>
	public class RunVehicleLocationRepository 
	{

		static string FailedToSave = "Failed to Save to DB";
		/// <summary>
		/// 添加车位位置
		/// </summary>
		/// <param name="model">车位位置</param>
		public static SingleMessage<bool> InsertRunVehicleLocation(PTMSEntities context,RunVehicleLocation model)
		{
            var entity = new RUN_VEHICLE_LOCATION();
			RunVehicleLocationUtility.UpdateEntity(entity, model, true);

            context.RUN_VEHICLE_LOCATION.Add(entity);

			return context.Save();
		}

		/// <summary>
		/// 修改车位位置
		/// </summary>
		public static SingleMessage<bool> UpdateRunVehicleLocation(PTMSEntities context,RunVehicleLocation model)
		{
            var entity = context.RUN_VEHICLE_LOCATION.FirstOrDefault(t => t.ID == model.ID);
			if (null == entity)
			{
				return new SingleMessage<bool>(false, "");
			}

			RunVehicleLocationUtility.UpdateEntity(entity, model, false);

			return context.Save();
		}

		/// <summary>
		/// 删除车位位置
		/// </summary>
		public static SingleMessage<bool> DeleteRunVehicleLocationByID(PTMSEntities context,string ID)
		{
            RUN_VEHICLE_LOCATION entity = context.RUN_VEHICLE_LOCATION.FirstOrDefault(t => t.ID == ID);
			if (entity != null)
			{
                context.RUN_VEHICLE_LOCATION.Attach(entity);
                context.RUN_VEHICLE_LOCATION.Remove(entity);
				return context.Save();
			}
			else
			{
				return new SingleMessage<bool>(false, "");
			}
		}

		/// <summary>
		/// 获取车位位置
		/// </summary>
		public static SingleMessage<RunVehicleLocation> GetRunVehicleLocation(PTMSEntities context,string ID)
		{
            RUN_VEHICLE_LOCATION entity = context.RUN_VEHICLE_LOCATION.SingleOrDefault(n => n.ID == ID);
			if (entity != null)
			{
				RunVehicleLocation model = RunVehicleLocationUtility.GetModel(entity);
				return new SingleMessage<RunVehicleLocation>(model);
			}
			return null;
		}

		/// <summary>
		/// 获取车位位置
		/// </summary>
		public static MultiMessage<RunVehicleLocation> GetRunVehicleLocationList(PTMSEntities context,int pageIndex = 1, int pageSize = 10)
		{
			int totalCount;
			var list = context.RUN_VEHICLE_LOCATION.Page(out totalCount, pageIndex, pageSize, true).ToList();

			var items = list.Select(t => RunVehicleLocationUtility.GetModel(t)).ToList();

            return new MultiMessage<RunVehicleLocation>(items, totalCount);
		}

	}
}

