using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Data;
using System.Linq;
namespace Gsafety.PTMS.Maintain.Repository
{
    ///<summary>
    ///维修申请
    ///</summary>
    public class MaintainApplicationRepository
    {

        static string FailedToSave = "FailedtoSavetoDB";
        /// <summary>
        /// 添加维修申请
        /// </summary>
        /// <param name="model">维修申请</param>
        public static SingleMessage<bool> InsertMaintainApplication(PTMSEntities context, MaintainApplication model)
        {
            var entity = new MTN_MAINTAIN_APPLICATION();
            MaintainApplicationUtility.UpdateEntity(entity, model, true);

            context.MTN_MAINTAIN_APPLICATION.Add(entity);

            return context.Save();
        }

        /// <summary>
        /// 修改维修申请
        /// </summary>
        public static SingleMessage<bool> UpdateMaintainApplication(PTMSEntities context, MaintainApplication model)
        {
            try
            {
                var entity = context.MTN_MAINTAIN_APPLICATION.FirstOrDefault(t => t.ID == model.ID);
                if(null == entity)
                {
                    return new SingleMessage<bool>(false, "");
                }

                MaintainApplicationUtility.UpdateEntity(entity, model, false);
                context.Entry(entity).State = EntityState.Modified;
                return context.Save();
            }
            catch(Exception)
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.SaveDBFailed);
            }

        }

        /// <summary>
        /// 删除维修申请
        /// </summary>
        public static SingleMessage<bool> DeleteMaintainApplicationByID(PTMSEntities context, string ID)
        {
            MTN_MAINTAIN_APPLICATION entity = context.MTN_MAINTAIN_APPLICATION.FirstOrDefault(t => t.ID == ID);
            if(entity != null)
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
        public static SingleMessage<MaintainApplication> GetMaintainApplication(PTMSEntities context, string ID)
        {
            MTN_MAINTAIN_APPLICATION entity = context.MTN_MAINTAIN_APPLICATION.SingleOrDefault(n => n.ID == ID);
            if(entity != null)
            {
                MaintainApplication model = MaintainApplicationUtility.GetModel(entity);
                return new SingleMessage<MaintainApplication>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取维修申请
        /// </summary>
        public static MultiMessage<MaintainApplication> GetMaintainApplicationList(PTMSEntities context, string clientID,int state, string applicant, string vehicleID, string application, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;

            var result = from c in context.MTN_MAINTAIN_APPLICATION
                         where c.CLIENT_ID==clientID&&c.STATUS==state && (string.IsNullOrEmpty(applicant) ? true : c.APPLICANT.Contains(applicant)) &&
                         (string.IsNullOrEmpty(vehicleID) ? true : c.VEHCILE_ID.Contains(vehicleID)) &&
                         (string.IsNullOrEmpty(application) ? true : c.CONTACT.Contains(application))
                         
                         select c;

            var items = result.Page(out totalCount, pageIndex, pageSize, true,t=>t.CREATE_TIME,false)
                .ToList()
                .Select(t => MaintainApplicationUtility.GetModel(t)).ToList();

            return new MultiMessage<MaintainApplication>(items, totalCount);
        }


        public static MultiMessage<InstallStation> GetInstallStationList(PTMSEntities context, string clientID)
        {

            var list = context.BSC_SETUP_STATION.ToList();
            int totalCount = list.Count;
            var items = list.Select(t => InstallStationUtility.GetModel(t)).Where(c => c.ClientID == clientID && c.Valid == 1).ToList();

            return new MultiMessage<InstallStation>(items, totalCount);
        }
    }
}

