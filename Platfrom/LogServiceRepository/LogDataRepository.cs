using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;
using LogServiceContract.Data;
using LogServiceRepository.Utilties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LogServiceRepository
{
    ///<summary>
    ///视频日志
    ///</summary>
    public class LogDataRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加视频日志
        /// </summary>
        /// <param name="model">视频日志</param>
        public static SingleMessage<bool> InsertLogData(PTMSEntities context, LogData model)
        {
            var entity = new LOG_DATA();
            LogDataUtility.UpdateEntity(entity, model, true);

            context.LOG_DATA.Add(entity);

            if (context.SaveChanges() > 0)
                return new SingleMessage<bool>(true);
            else
                return new SingleMessage<bool>(false, "InsertLogData:" + FailedToSave);
        }

        /// <summary>
        /// 修改视频日志
        /// </summary>
        public static SingleMessage<bool> UpdateLogData(PTMSEntities context, LogData model)
        {
            var entity = context.LOG_DATA.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            LogDataUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = EntityState.Modified;
            if (context.SaveChanges() > 0)
                return new SingleMessage<bool>(true);
            else
                return new SingleMessage<bool>(false, "UpdateLogData:" + FailedToSave);
        }

        /// <summary>
        /// 删除视频日志
        /// </summary>
        public static SingleMessage<bool> DeleteLogDataByID(PTMSEntities context, string ID)
        {
            LOG_DATA entity = context.LOG_DATA.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.LOG_DATA.Attach(entity);
                context.LOG_DATA.Remove(entity);
                if (context.SaveChanges() > 0)
                    return new SingleMessage<bool>(true);
                else
                    return new SingleMessage<bool>(false, "DeleteLogDataByID:" + FailedToSave);
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        /// <summary>
        /// 获取视频日志
        /// </summary>
        public static MultiMessage<LogData> GetLogDataList(PTMSEntities context, int pageIndex, int pageSize, string ClientID)
        {
            MultiMessage<LogData> result = new MultiMessage<LogData>();

            var sour = from v in context.LOG_DATA
                       where v.CLIENT_ID == ClientID
                       select v;

            List<LOG_DATA> entitylist = null;
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
                var res2 = from g in context.USR_GUSER
                           join r in context.USR_ROLE on g.ROLE_ID equals r.ID
                           where g.USER_NAME == item.USER_NAME
                           select r;
                if (res2.FirstOrDefault() != null)
                    item.USER_TYPE = res2.FirstOrDefault().NAME;
                result.Result.Add(LogDataUtility.GetModel(item));
            }
            return result;
        }

        /// <summary>
        /// 获取视频日志
        /// </summary>
        public static MultiMessage<LogData> GetLogDataListByCondition(PTMSEntities context, int pageIndex, int pageSize, string ClientID, string loginUser, DateTime? beginTime, DateTime? endTime)
        {
            MultiMessage<LogData> result = new MultiMessage<LogData>();

            var sour = from v in context.LOG_DATA
                       where v.CLIENT_ID == ClientID && ((loginUser == null || loginUser == "") ? true : v.USER_NAME.Contains(loginUser))
                       && (beginTime == null ? true : (v.START_TIME >= beginTime && v.END_TIME < endTime))
                       select v;
            List<LOG_DATA> entitylist = null;
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
                result.Result.Add(LogDataUtility.GetModel(item));
            }
            return result;
        }

    }
}

