using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Data;

namespace Gsafety.PTMS.Manager.Repository
{
    ///<summary>
    ///
    ///</summary>
    public class UserOnlineRepository
    {

        static string FailedToSave = "Failed to Save to DB";

        /// <summary>
        /// 获取
        /// </summary>
        public static MultiMessage<UserOnline> GetUserOnlineList(PTMSEntities context, string clientID, string userName, int pageIndex = 1, int pageSize = 10)
        {

            int totalCount;
            if (string.IsNullOrEmpty(userName))
            {
                var list = from u in context.RUN_USER_ONLINE
                           join g in context.USR_GUSER on u.USER_ID equals g.ID
                           join r in context.USR_ROLE on g.ROLE_ID equals r.ID
                           where u.CLIENT_ID == clientID
                           orderby u.ONLINE_TIME descending
                           select new UserOnline
                           {
                               ClientID = u.CLIENT_ID,
                               OnlineTime = u.ONLINE_TIME,
                               UserID = u.USER_ID,
                               UserName = g.USER_NAME,
                               RoleName = r.NAME
                           };

                var items = list.Page(out totalCount, pageIndex, pageSize, true).ToList();

                return new MultiMessage<UserOnline>(items, totalCount);
            }
            else
            {
                string username = userName.ToUpper();
                var list = from u in context.RUN_USER_ONLINE
                           join g in context.USR_GUSER on u.USER_ID equals g.ID
                           join r in context.USR_ROLE on g.ROLE_ID equals r.ID
                           where u.CLIENT_ID == clientID && g.USER_NAME.ToUpper().Contains(username)
                           orderby u.ONLINE_TIME descending
                           select new UserOnline
                           {
                               ClientID = u.CLIENT_ID,
                               OnlineTime = u.ONLINE_TIME,
                               UserID = u.USER_ID,
                               UserName = g.USER_NAME,
                               RoleName = r.NAME
                           };

                var items = list.Page(out totalCount, pageIndex, pageSize, true).ToList();

                return new MultiMessage<UserOnline>(items, totalCount);
            }
        }

    }
}

