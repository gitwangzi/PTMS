using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
namespace LogServiceRepository
{
    ///<summary>
    ///
    ///</summary>
    public class LogOperateRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public static SingleMessage<bool> InsertLogOperate(PTMSEntities context, LogOperate model)
        {
            var entity = new LOG_OPERATE();
            LogOperateUtility.UpdateEntity(entity, model, true);

            context.LOG_OPERATE.Add(entity);

            return context.Save();
        }

        /// <summary>
        /// 修改
        /// </summary>
        public static SingleMessage<bool> UpdateLogOperate(PTMSEntities context, LogOperate model)
        {
            var entity = context.LOG_OPERATE.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            LogOperateUtility.UpdateEntity(entity, model, false);

            return context.Save();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public static SingleMessage<bool> DeleteLogOperateByID(PTMSEntities context, string ID)
        {
            LOG_OPERATE entity = context.LOG_OPERATE.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.LOG_OPERATE.Attach(entity);
                context.LOG_OPERATE.Remove(entity);
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
        public static SingleMessage<LogOperate> GetLogOperate(PTMSEntities context, string ID)
        {
            LOG_OPERATE entity = context.LOG_OPERATE.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                LogOperate model = LogOperateUtility.GetModel(entity);
                return new SingleMessage<LogOperate>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取
        /// </summary>
        public static MultiMessage<LogOperate> GetLogOperateList(PTMSEntities context, string userName, string clientID, DateTime? startTime, DateTime? endTime, int pageIndex = 1, int pageSize = 10)
        {
            //int totalCount;

            //IQueryable<LOG_OPERATE> list;
            //if(!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userName.Trim()))
            //{
            //    list = from l in context.LOG_OPERATE
            //           where l.CLIENT_ID == clientID && (startTime.HasValue ? l.OPERATE_TIME > startTime.Value : true) && (endTime.HasValue ? l.OPERATE_TIME < endTime : true) && (l.OPERATOR_NAME.Contains(userName))
            //           select l;
            //}
            //else
            //{
            //    list = from l in context.LOG_OPERATE
            //           where l.CLIENT_ID == clientID && (startTime.HasValue ? l.OPERATE_TIME > startTime.Value : true) && (endTime.HasValue ? l.OPERATE_TIME < endTime : true)
            //           select l;

            //}
            //var items = list.Page(out totalCount, pageIndex, pageSize, true, n => n.OPERATE_TIME, false).ToList();

            //var temp = items.Select(n => LogOperateUtility.GetModel(n)).ToList();
            //foreach(var ite in temp)
            //{
            //    var res2 = from g in context.USR_GUSER
            //               join r in context.USR_ROLE on g.ROLE_ID equals r.ID
            //               where g.USER_NAME.Trim() == ite.OperatorName.Trim()
            //               select r;
            //    if(res2.FirstOrDefault() != null)
            //        ite.ShowRoleName = res2.FirstOrDefault().NAME;
            //}
            //return new MultiMessage<LogOperate>(temp, totalCount);

            int totalCount;
            IQueryable<LogOperate> list;
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userName.Trim()))
            {
                list = from l in context.LOG_OPERATE
                       join u in context.USR_GUSER on l.OPERATOR_ID equals u.ID
                       join r in context.USR_ROLE on u.ROLE_ID equals r.ID
                       where l.CLIENT_ID == clientID && (startTime.HasValue ? l.OPERATE_TIME > startTime.Value : true) && (endTime.HasValue ? l.OPERATE_TIME < endTime : true) && (u.ACCOUNT.Contains(userName)) && (u.VALID == (short)ValidEnum.Valid) && (r.VALID == (short)ValidEnum.Valid)
                       select new LogOperate()
                       {
                           ShowRoleName = r.NAME,
                           OperatorName = l.OPERATOR_NAME,
                           OperateTime = l.OPERATE_TIME.Value,
                           OperateType = l.OPERATE_TYPE.HasValue ? l.OPERATE_TYPE.Value : (short)0,
                           LoginName = u.ACCOUNT,
                           OperateContent=l.OPERATE_CONTENT
                       };
            }
            else
            {
                list = from l in context.LOG_OPERATE
                       join u in context.USR_GUSER on l.OPERATOR_ID equals u.ID
                       join r in context.USR_ROLE on u.ROLE_ID equals r.ID
                       where l.CLIENT_ID == clientID && (startTime.HasValue ? l.OPERATE_TIME > startTime.Value : true) && (endTime.HasValue ? l.OPERATE_TIME < endTime : true) && (u.VALID == (short)ValidEnum.Valid) && (r.VALID == (short)ValidEnum.Valid)
                       select new LogOperate()
                       {
                           ShowRoleName = r.NAME,
                           OperatorName = l.OPERATOR_NAME,
                           OperateTime = l.OPERATE_TIME.Value,
                           OperateType = l.OPERATE_TYPE.HasValue ? l.OPERATE_TYPE.Value : (short)0,
                           LoginName = u.ACCOUNT,
                           OperateContent = l.OPERATE_CONTENT
                       };

            }
            var items = list.Page(out totalCount, pageIndex, pageSize, true, n => n.OperateTime, false).ToList();
            return new MultiMessage<LogOperate>(items, totalCount);
        }


        public static bool ClearOperateLog(PTMSEntities context, string clientID, LogOperate log)
        {
            bool suc = false;
            try
            {
                List<LOG_OPERATE> list = context.LOG_OPERATE.Where(n => n.CLIENT_ID == clientID).ToList();
                foreach (var item in list)
                {
                    context.LOG_OPERATE.Remove(item);
                }

                LOG_OPERATE data = new LOG_OPERATE();
                data.CLIENT_ID = clientID;
                data.OPERATE_CONTENT = log.OperateContent;
                data.OPERATE_TIME = log.OperateTime;
                data.OPERATE_TYPE = (short)OperateTypeEnum.LogManagement;
                data.OPERATOR_ID = log.OperatorID;
                data.OPERATOR_NAME = log.OperatorName;
                data.ID = log.ID;
                context.LOG_OPERATE.Add(data);

                return context.SaveChanges() > 0;
            }
            catch
            {
                suc = false;
            }

            return suc;
        }
    }
}

