using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using System.Collections.Generic;
using System.Linq;
namespace Gsafety.PTMS.Manager.Repository
{
    ///<summary>
    ///心跳规则
    ///</summary>
    public class HeartbeatRuleRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加心跳规则
        /// </summary>
        /// <param name="model">心跳规则</param>
        public static SingleMessage<bool> InsertHeartbeatRule(PTMSEntities context, HeartbeatRule model)
        {
            var entity = new TRF_COMMAND_PARAM();
            var hasOne = context.TRF_COMMAND_PARAM.Any(p => p.NAME == model.Name && p.CLIENT_ID == model.ClientID && p.VALID == 1 && p.TYPE == (short)CommandParaEnum.HeartBeat);
            if (hasOne)
                return new SingleMessage<bool>(false, "SameNameExist");
            HeartbeatRuleUtility.UpdateEntity(entity, model, true);

            context.TRF_COMMAND_PARAM.Add(entity);

            return context.Save();
        }

        /// <summary>
        /// 修改心跳规则
        /// </summary>
        public static SingleMessage<bool> UpdateHeartbeatRule(PTMSEntities context, HeartbeatRule model)
        {
            var hasOne = context.TRF_COMMAND_PARAM.Any(t => t.NAME == model.Name && t.ID != model.ID && t.VALID == (short)ValidEnum.Valid && t.TYPE == (short)CommandParaEnum.HeartBeat && t.CLIENT_ID == model.ClientID);
            if (hasOne)
            {
                return new SingleMessage<bool>(false, "SameNameExist");
            }
            var entityUpdateTarget = context.TRF_COMMAND_PARAM.FirstOrDefault(t => t.ID == model.ID && t.VALID == (short)ValidEnum.Valid);
            if (entityUpdateTarget == null)
            {
                return new SingleMessage<bool>(false, "NotExist");
            }

            HeartbeatRuleUtility.UpdateEntity(entityUpdateTarget, model, false);
            context.Entry(entityUpdateTarget).State = System.Data.EntityState.Modified;
            return context.Save();
        }

        /// <summary>
        /// 删除心跳规则
        /// </summary>
        public static SingleMessage<bool> DeleteHeartbeatRuleByID(PTMSEntities context, string ID)
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
        /// 获取心跳规则
        /// </summary>
        public static SingleMessage<HeartbeatRule> GetHeartbeatRule(PTMSEntities context, string ID)
        {
            TRF_COMMAND_PARAM entity = context.TRF_COMMAND_PARAM.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                HeartbeatRule model = HeartbeatRuleUtility.GetModel(entity);
                return new SingleMessage<HeartbeatRule>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取心跳规则
        /// </summary>
        public static MultiMessage<HeartbeatRule> GetHeartbeatRuleList(PTMSEntities context, string clientid, string name, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount = 0;
            List<TRF_COMMAND_PARAM> list = null;
            if (string.IsNullOrEmpty(name))
            {
                list = context.TRF_COMMAND_PARAM.Where(n => n.CLIENT_ID == clientid && n.TYPE == (short)CommandParaEnum.HeartBeat).Page(out totalCount, pageIndex, pageSize, true, n => n.CREATE_TIME, false).ToList();

            }
            else
            {
                list = context.TRF_COMMAND_PARAM.Where(n => n.CLIENT_ID == clientid && n.TYPE == (short)CommandParaEnum.HeartBeat && n.NAME.Contains(name)).Page(out totalCount, pageIndex, pageSize, true, n => n.CREATE_TIME, false).ToList();
            }

            var items = list.Select(t => HeartbeatRuleUtility.GetModel(t)).ToList();
            List<HeartbeatRule> items2 = new List<HeartbeatRule>();

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

            return new MultiMessage<HeartbeatRule>(items2, totalCount);
        }

    }
}

