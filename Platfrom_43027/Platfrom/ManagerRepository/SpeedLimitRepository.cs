using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Base.Contract.Data;
using System.Linq.Expressions;
using Gsafety.PTMS.Common.Data.Enum;
namespace Gsafety.PTMS.Manager.Repository
{
    ///<summary>
    ///超速表
    ///</summary>
    public class SpeedLimitRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加超速表
        /// </summary>
        /// <param name="model">超速表</param>
        public static SingleMessage<bool> InsertSpeedLimit(PTMSEntities context, SpeedLimit model)
        {
            var entity = new TRF_COMMAND_PARAM();
            var hasOne = context.TRF_COMMAND_PARAM.Any(p => p.NAME == model.Name && p.VALID == 1 && p.CLIENT_ID == model.ClientID && p.TYPE == (short)CommandParaEnum.Speed);
            if (hasOne)
                return new SingleMessage<bool>(false, "SameNameExist");
            SpeedLimitUtility.UpdateEntity(entity, model, true);

            context.TRF_COMMAND_PARAM.Add(entity);

            return context.Save();
        }

        /// <summary>
        /// 修改超速表
        /// </summary>
        public static SingleMessage<bool> UpdateSpeedLimit(PTMSEntities context, SpeedLimit model)
        {
            var hasOne = context.TRF_COMMAND_PARAM.Any(t => t.NAME == model.Name && t.ID != model.ID && t.VALID == (short)ValidEnum.Valid && t.TYPE == (short)CommandParaEnum.Speed && t.CLIENT_ID == model.ClientID);
            if (hasOne)
            {
                return new SingleMessage<bool>(false, "SameNameExist");
            }
            var entity = context.TRF_COMMAND_PARAM.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            SpeedLimitUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = System.Data.EntityState.Modified;
            return context.Save();
        }

        /// <summary>
        /// 删除超速表
        /// </summary>
        public static SingleMessage<bool> DeleteSpeedLimitByID(PTMSEntities context, string ID)
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
        /// 获取超速表
        /// </summary>
        public static SingleMessage<SpeedLimit> GetSpeedLimit(PTMSEntities context, string ID)
        {
            TRF_COMMAND_PARAM entity = context.TRF_COMMAND_PARAM.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                SpeedLimit model = SpeedLimitUtility.GetModel(entity);
                return new SingleMessage<SpeedLimit>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取超速表
        /// </summary>
        public static MultiMessage<SpeedLimit> GetSpeedLimitList(PTMSEntities context, string ruleName, int pageIndex = 1, int pageSize = 10)
        {
            Expression<Func<TRF_COMMAND_PARAM, bool>> condition = speed => speed.TYPE == (short)CommandParaEnum.Speed;

            if (string.IsNullOrWhiteSpace(ruleName) == false)
            {
                condition.And(speed => speed.NAME == ruleName);
            }

            int totalCount;
            var list = context.TRF_COMMAND_PARAM.Where(condition).Page(out totalCount, pageIndex, pageSize, true).ToList();

            var items = list.Select(t => SpeedLimitUtility.GetModel(t)).ToList();

            List<SpeedLimit> items2 = new List<SpeedLimit>();
            foreach (var temp in items)
            {
                var result = context.TRF_COMMAND_VEHICLE.Where(v => v.COMMAND_PARAM_ID == temp.ID);
                if (result.Count() > 0)
                {
                    temp.IsVisible = false;
                }
                else
                {
                    temp.IsVisible = true;
                }
                items2.Add(temp);
            }



            return new MultiMessage<SpeedLimit>(items2, totalCount);
        }


        public static MultiMessage<SpeedLimit> GetSpeedLimitListByName(PTMSEntities context, string clientID, string name, int pageIndex, int pageSize)
        {
            int totalCount = 0;
            List<TRF_COMMAND_PARAM> list = null;
            if (string.IsNullOrEmpty(name))
            {
                list = context.TRF_COMMAND_PARAM.Where(n => n.CLIENT_ID == clientID && n.TYPE == (short)CommandParaEnum.Speed).Page(out totalCount, pageIndex, pageSize, true, n => n.CREATE_TIME, false).ToList();

            }
            else
            {
                list = context.TRF_COMMAND_PARAM.Where(n => n.CLIENT_ID == clientID && n.TYPE == (short)CommandParaEnum.Speed && n.NAME.Contains(name)).Page(out totalCount, pageIndex, pageSize, true, n => n.CREATE_TIME, false).ToList();
            }

            var items = list.Select(t => SpeedLimitUtility.GetModel(t)).ToList();
            List<SpeedLimit> items2 = new List<SpeedLimit>();
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

            return new MultiMessage<SpeedLimit>(items2, totalCount);
        }

        public static MultiMessage<VehicleSpeedLimit> GetSpeedLimitList()
        {
            try
            {
                using (var context = new PTMSEntities())
                {
                    var temp = from f in context.TRF_COMMAND_PARAM
                               join q in context.TRF_COMMAND_VEHICLE on f.ID equals q.COMMAND_PARAM_ID
                               where f.TYPE == (int)CommandParaEnum.Speed && f.VALID == 1 && q.STATUS == (int)CommandStateEnum.Succeed
                               select new VehicleSpeedLimit 
                               { 
                                   VehicleID=q.VEHICLE_ID,
                                   UID = q.MDVR_CORE_SN,
                                   MaxSpeed = f.MAX_SPEED.Value,
                                   Duration = f.DURATION.Value
                               
                               };
 
                    var templist = temp.ToList();


                    return new MultiMessage<VehicleSpeedLimit>(templist, templist.Count);
                }
            }
            catch (Exception ex)
            { 
                return new MultiMessage<VehicleSpeedLimit>(null, 0);
            }

        }
    }
}

