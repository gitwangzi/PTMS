using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using LogServiceContract.Data;
using LogServiceRepository.Utilties;
using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
namespace LogServiceRepository
{
    ///<summary>
    ///访问日志
    ///</summary>
    public class LogAccessRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加访问日志
        /// </summary>
        /// <param name="model">访问日志</param>
        public static SingleMessage<bool> InsertLogAccess(PTMSEntities context, LogAccess model)
        {
            var entity = new LOG_ACCESS();
            LogAccessUtility.UpdateEntity(entity, model, true);

            context.LOG_ACCESS.Add(entity);

            if (context.SaveChanges() > 0)
                return new SingleMessage<bool>(true);
            else
                return new SingleMessage<bool>(false, "InsertLogAccess:" + FailedToSave);
        }

        /// <summary>
        /// 删除访问日志
        /// </summary>
        public static SingleMessage<bool> DeleteLogAccessByID(PTMSEntities context, string ID)
        {
            LOG_ACCESS entity = context.LOG_ACCESS.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.LOG_ACCESS.Attach(entity);
                context.LOG_ACCESS.Remove(entity);
                if (context.SaveChanges() > 0)
                    return new SingleMessage<bool>(true);
                else
                    return new SingleMessage<bool>(false, "DeleteLogAccessByID:" + FailedToSave);
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        /// <summary>
        /// 获取访问日志
        /// </summary>
        public static MultiMessage<LogAccess> GetLogAccessList(PTMSEntities context, int pageIndex, int pageSize, string ClientID)
        {
            int totalCount;
            MultiMessage<LogAccess> result = new MultiMessage<LogAccess>();

            var sour = from v in context.LOG_ACCESS
                       where v.CLIENT_ID == ClientID
                       select v;
            var items = sour.Page(out totalCount, pageIndex, pageSize, true, n => n.LOGOUT_TIME, false)
                         .Select(t => LogAccessUtility.GetModel(t))
                         .ToList();

            return new MultiMessage<LogAccess>(items, totalCount);

        }

        /// <summary>
        /// 获取访问日志
        /// </summary>
        public static MultiMessage<LogAccess> GetLogAccessListByCondition(PTMSEntities context, int pageIndex, int pageSize, string ClientID, string loginUser, DateTime? beginTime, DateTime? endTime)
        {

            MultiMessage<LogAccess> result = new MultiMessage<LogAccess>();

            var sour = from v in context.LOG_ACCESS
                       where v.CLIENT_ID == ClientID && ((loginUser == null || loginUser == "") ? true : v.LOGIN_USER.StartsWith(loginUser))
                       && (beginTime == null ? true : (v.LOGIN_TIME >= beginTime && v.LOGIN_TIME < endTime))
                       select v;
            List<LOG_ACCESS> entitylist = null;
            if (pageIndex > 0)
            {
                result.TotalRecord = sour.Count();
                entitylist = sour.OrderBy(t => t.LOGOUT_TIME)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize)
                    .ToList();
            }
            else
            {
                result.TotalRecord = sour.Count();
                entitylist = sour.OrderBy(t => t.LOGOUT_TIME).ToList();
            }

            foreach (var item in entitylist)
            {
                result.Result.Add(LogAccessUtility.GetModel(item));
            }

            return result;
        }



        public static List<LogAccess> GetLoginLog(PTMSEntities context, string clientID, string userName, DateTime? startTime, DateTime? endTime, int pageSize, int pageIndex, out int totalCount)
        {
            List<LogAccess> result = new List<LogAccess>();

            var sour = from v in context.LOG_ACCESS
                       join u in context.USR_GUSER on v.USER_ID equals u.ID
                       join r in context.USR_ROLE on u.ROLE_ID equals r.ID
                       where v.CLIENT_ID == clientID && (string.IsNullOrEmpty(userName)? true : v.LOGIN_USER.ToUpper().Contains(userName.ToUpper()))
                       && (startTime.HasValue ? v.LOGIN_TIME >= startTime : true)
  && (endTime.HasValue ? v.LOGIN_TIME <= endTime : true)
                       select new LogAccess()
                       {
                           ClientID = v.CLIENT_ID,
                           ID = v.ID,
                           LoginTime = v.LOGIN_TIME.Value,
                           LoginUser = v.LOGIN_USER,
                           LogoutTime = v.LOGOUT_TIME,
                           ShowRoleName = r.NAME,
                           UserID = v.USER_ID,
                       };

            var entitylist = sour.Page(out totalCount, pageIndex, pageSize, true, n => n.LoginTime, false).ToList();

            return entitylist;
        }
    }
}
