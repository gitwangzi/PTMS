using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using LogServiceContract.Data;
using LogServiceRepository.Utilties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogServiceRepository
{
    ///<summary>
    ///外部访问日志
    ///</summary>
    public class LogManagerRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加外部访问日志
        /// </summary>
        /// <param name="model">外部访问日志</param>
        public static SingleMessage<bool> InsertLogManager(PTMSEntities context, LogManager model)
        {
            var entity = new LOG_MANAGER();
            LogManagerUtility.UpdateEntity(entity, model, true);

            context.LOG_MANAGER.Add(entity);

            if (context.SaveChanges() > 0)
                return new SingleMessage<bool>(true);
            else
                return new SingleMessage<bool>(false, FailedToSave);
        }

        /// <summary>
        /// 修改外部访问日志
        /// </summary>
        public static SingleMessage<bool> UpdateLogManager(PTMSEntities context, LogManager model)
        {
            var entity = context.LOG_MANAGER.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            LogManagerUtility.UpdateEntity(entity, model, false);

            if (context.SaveChanges() > 0)
                return new SingleMessage<bool>(true);
            else
                return new SingleMessage<bool>(false, FailedToSave);
        }

        /// <summary>
        /// 删除外部访问日志
        /// </summary>
        public static SingleMessage<bool> DeleteLogManagerByID(PTMSEntities context, string ID)
        {
            LOG_MANAGER entity = context.LOG_MANAGER.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.LOG_MANAGER.Attach(entity);
                context.LOG_MANAGER.Remove(entity);
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
        /// 获取外部访问日志
        /// </summary>
        public static MultiMessage<LogManager> GetLogManagerList(PTMSEntities context, int pageIndex, int pageSize)
        {
            MultiMessage<LogManager> result = new MultiMessage<LogManager>();

            var sour = from v in context.LOG_MANAGER
                       select v;
            List<LOG_MANAGER> entitylist = null;
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
                result.Result.Add(LogManagerUtility.GetModel(item));
            }

            return result;
        }

        /// <summary>
        /// 获取外部访问日志
        /// </summary>
        public static MultiMessage<LogManager> GetLogManagerListByCondition(PTMSEntities context, int pageIndex, int pageSize, string manager, DateTime? beginTime, DateTime? endTime)
        {
            MultiMessage<LogManager> result = new MultiMessage<LogManager>();
            manager = manager.ToUpper();
            var sour = from v in context.LOG_MANAGER
                       where ((manager == null || manager == "") ? true : v.MANAGER.ToUpper().Contains(manager))
                       && (beginTime == null ? true : (v.CREATE_TIME >= beginTime)) && (endTime == null ? true : (v.CREATE_TIME <= endTime))
                       select v;
            int count = 0;
            List<LOG_MANAGER> entitylist = sour.Page(out count, pageIndex, pageSize, true, (n) => n.CLIENT_NAME, false).ToList();


            foreach (var item in entitylist)
            {
                result.Result.Add(LogManagerUtility.GetModel(item));
            }

            result.TotalRecord = count;
            return result;

        }

    }
}

