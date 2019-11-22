using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using System.Collections.Generic;
using System.Linq;

namespace Gsafety.PTMS.Manager.Repository.UserManage
{
    public class LocationReportRuleRepository
    {
        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加LocationReportRule设备
        /// </summary>
        /// <param name="model">LocationReportRule设备</param>
        public static SingleMessage<bool> InsertLocationReportRule(PTMSEntities context, LocationReportRule model)
        {
            var entity = new TRF_COMMAND_PARAM();
            var hasOne = context.TRF_COMMAND_PARAM.FirstOrDefault(p => p.NAME == model.Name && p.CLIENT_ID == model.ClientID && p.VALID == (short)ValidEnum.Valid && p.TYPE == (short)CommandParaEnum.ReportStrategy);
            if (hasOne != null)
                return new SingleMessage<bool>(false, "SystemManager_Name_Duplicate");
            entity = LocationReportRuleUtility.UpdateEntity(entity, model, true);
            entity.VALID = 1;
            context.TRF_COMMAND_PARAM.Add(entity);
            return context.Save();
        }

        /// <summary>
        /// 修改LocationReportRule设备
        /// </summary>
        public static SingleMessage<bool> UpdateLocationReportRule(PTMSEntities context, LocationReportRule model)
        {
            var entity = context.TRF_COMMAND_PARAM.FirstOrDefault(t => t.NAME == model.Name && t.ID != model.ID && t.VALID == (short)ValidEnum.Valid && t.TYPE == (short)CommandParaEnum.ReportStrategy && t.CLIENT_ID == model.ClientID);
            if (entity != null)
            {
                return new SingleMessage<bool>(false, "SameNameExist");
            }
            var entityUpdateTarget = context.TRF_COMMAND_PARAM.FirstOrDefault(t => t.ID == model.ID && t.VALID == (short)ValidEnum.Valid && t.TYPE == (short)CommandParaEnum.ReportStrategy && t.CLIENT_ID == model.ClientID);
            if (entityUpdateTarget == entity)
            {
                return new SingleMessage<bool>(false, "NotExist");
            }

            LocationReportRuleUtility.UpdateEntity(entityUpdateTarget, model, false);
            context.Entry(entityUpdateTarget).State = System.Data.EntityState.Modified;
            return context.Save();
        }

        /// <summary>
        /// 删除LocationReportRule设备
        /// </summary>
        public static SingleMessage<bool> DeleteLocationReportRuleByID(PTMSEntities context, string ID)
        {
            bool referenced = context.TRF_COMMAND_VEHICLE.Any(n => n.COMMAND_PARAM_ID == ID);
            if (referenced)
            {
                return new SingleMessage<bool>(false, "Referenced");
            }
            TRF_COMMAND_PARAM entity = context.TRF_COMMAND_PARAM.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.TRF_COMMAND_PARAM.Remove(entity);

                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        /// <summary>
        /// 获取LocationReportRule设备
        /// </summary>
        public static SingleMessage<LocationReportRule> GetLocationReportRule(PTMSEntities context, string ID)
        {
            TRF_COMMAND_PARAM entity = context.TRF_COMMAND_PARAM.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                LocationReportRule model = LocationReportRuleUtility.GetModel(entity);
                return new SingleMessage<LocationReportRule>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取LocationReportRule设备
        /// </summary>
        public static MultiMessage<LocationReportRule> GetLocationReportRuleList(PTMSEntities context, int pageIndex, int pageSize, string ClientID)
        {

            MultiMessage<LocationReportRule> result = new MultiMessage<LocationReportRule>();

            var sour = from v in context.TRF_COMMAND_PARAM
                       where v.VALID == 1 && v.CLIENT_ID == ClientID && v.TYPE == (short)CommandParaEnum.ReportStrategy
                       orderby v.CREATE_TIME descending
                       select v;
            List<TRF_COMMAND_PARAM> entitylist = null;
            if (pageIndex > 0)
            {
                result.TotalRecord = sour.Count();
                entitylist = sour.OrderBy(t => t.CREATE_TIME)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize)
                    .ToList();
            }
            else
            {
                result.TotalRecord = sour.Count();
                entitylist = sour.OrderBy(t => t.CREATE_TIME).ToList();
            }

            foreach (var item in entitylist)
            {
                result.Result.Add(LocationReportRuleUtility.GetModel(item));
            }

            return result;
        }


        public static MultiMessage<LocationReportRule> GetByNameLocationReportRuleList(PTMSEntities context, int pageIndex, int pageSize, string ClientID, string name)
        {
            int totalCount = 0;
            List<TRF_COMMAND_PARAM> list = null;
            if (string.IsNullOrEmpty(name))
            {
                list = context.TRF_COMMAND_PARAM.Where(n => n.CLIENT_ID == ClientID && n.TYPE == (short)CommandParaEnum.ReportStrategy && n.VALID == (short)ValidEnum.Valid).Page(out totalCount, pageIndex, pageSize, true, n => n.CREATE_TIME, false).ToList();

            }
            else
            {
                list = context.TRF_COMMAND_PARAM.Where(n => n.CLIENT_ID == ClientID && n.TYPE == (short)CommandParaEnum.ReportStrategy && n.NAME.Contains(name) && n.VALID == (short)ValidEnum.Valid).Page(out totalCount, pageIndex, pageSize, true, n => n.CREATE_TIME, false).ToList();
            }

            var items = list.Select(t => LocationReportRuleUtility.GetModel(t)).ToList();
            List<LocationReportRule> items2 = new List<LocationReportRule>();
            List<string> ids = list.Select(n => n.ID).ToList();
            List<string> referenceids = context.TRF_COMMAND_VEHICLE.Where(v => ids.Contains(v.COMMAND_PARAM_ID)).Select(n => n.COMMAND_PARAM_ID).ToList();
            foreach (var temp in items)
            {
                if (referenceids.Contains(temp.ID))
                {
                    temp.IsVisible = false;
                }
                else
                {
                    temp.IsVisible = true;
                }
                items2.Add(temp);
            }

            return new MultiMessage<LocationReportRule>(items2, totalCount);

        }
    }
}
