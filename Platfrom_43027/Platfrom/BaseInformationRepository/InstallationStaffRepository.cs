using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.BaseInformation.Contract.Models;
using System.Data;
namespace Gsafety.PTMS.BaseInformation.Repository
{
	///<summary>
	///
	///</summary>
	public class InstallationStaffRepository 
	{

		static string FailedToSave = "Failed to Save to DB";
		/// <summary>
		/// 添加
		/// </summary>
		/// <param name="model"></param>
		public static SingleMessage<bool> InsertInstallationStaff(PTMSEntities context,InstallationStaff model)
		{
			var entity = new BSC_INSTALLATION_STAFF();
			InstallationStaffUtility.UpdateEntity(entity, model, true);

			context.BSC_INSTALLATION_STAFF.Add(entity);

			return context.Save();
		}

		/// <summary>
		/// 修改
		/// </summary>
		public static SingleMessage<bool> UpdateInstallationStaff(PTMSEntities context,InstallationStaff model)
		{
            var entity = context.BSC_INSTALLATION_STAFF.FirstOrDefault(t => t.ID == model.ID && t.VALID == (short)ValidEnum.Valid);
			if (null == entity)
			{
				return new SingleMessage<bool>(false, "");
			}

			InstallationStaffUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = EntityState.Modified;
			return context.Save();
		}

        /// <summary>
        /// 删除
        /// </summary>
        public static SingleMessage<bool> DeleteInstallationStaffByID(PTMSEntities context, string ID)
        {
            BSC_INSTALLATION_STAFF entity = context.BSC_INSTALLATION_STAFF.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.BSC_INSTALLATION_STAFF.Attach(entity);
                context.BSC_INSTALLATION_STAFF.Remove(entity);
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
        public static SingleMessage<InstallationStaff> GetInstallationStaff(PTMSEntities context, string ID)
        {
            BSC_INSTALLATION_STAFF entity = context.BSC_INSTALLATION_STAFF.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                InstallationStaff model = InstallationStaffUtility.GetModel(entity);
                return new SingleMessage<InstallationStaff>(model);
            }
            return null;
        }


		/// <summary>
		/// 获取
		/// </summary>
		public static MultiMessage<InstallationStaff> GetInstallationStaffList(PTMSEntities context,int pageIndex = 1, int pageSize = 10)
		{
			int totalCount;
			var list = context.BSC_INSTALLATION_STAFF.Page(out totalCount, pageIndex, pageSize, true).ToList();

			var items = list.Select(t => InstallationStaffUtility.GetModel(t)).ToList();

			return new MultiMessage<InstallationStaff>(items, totalCount);
		}

	}
}

