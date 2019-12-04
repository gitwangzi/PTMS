using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using LogServiceRepository.Utilties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogServiceRepository
{
    public class LogErrorRepository
    {
        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public static SingleMessage<bool> InsertLogError( LogError model)
        {
            try
            {

                using (PTMSEntities context = new PTMSEntities())
                {

                    var entity = new LOG_ERROR();
                    LogErrorUtility.UpdateEntity(entity, model, true);

                    context.LOG_ERROR.Add(entity);

                    if (context.SaveChanges() > 0)
                        return new SingleMessage<bool>(true);
                    else
                        return new SingleMessage<bool>(false, "InsertLogAccess:" + FailedToSave);
                }
            }
            catch (Exception ex)
            {

                return new SingleMessage<bool>(false, "InsertLogAccess:" + FailedToSave);
            }
        }

        public static MultiMessage<LogError> GetLogErrorList(PTMSEntities context, int pageIndex, int pageSize, string reason, DateTime? beginTime, DateTime? endTime)
        {
            MultiMessage<LogError> result = new MultiMessage<LogError>();
            reason = reason.ToUpper();
            var sour = from v in context.LOG_ERROR
                       where ((reason == null || reason == "") ? true : v.ERROR_REASON.ToUpper().Contains(reason))
                       && (beginTime == null ? true : (v.CREATE_TIME >= beginTime)) && (endTime == null ? true : (v.CREATE_TIME <= endTime))
                       select v;

            List<LOG_ERROR> entitylist = null;
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
                result.Result.Add(LogErrorUtility.GetModel(item));
            }

            return result;

        }


    }
}
