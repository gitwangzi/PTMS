using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Data;
namespace Gsafety.PTMS.Maintain.Repository
{
    ///<summary>
    ///
    ///</summary>
    public class MaintainRecordUnfinishedRepository
    {

        /// <summary>
        /// 获取
        /// </summary>
        public static MultiMessage<MaintainRecordUnfinished> GetMaintainRecordUnfinishedList(PTMSEntities context, string clientID,
            string vehcileID, string contact, string worker, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            MultiMessage<MaintainRecordUnfinished> result = new MultiMessage<MaintainRecordUnfinished>();
            var sour = from v in context.MNT_MAINTAINRECORD_UNFINISHED
                       join m in context.MTN_MAINTAIN_APPLICATION on v.APPLICATIONID equals m.ID
                       where ((v.MAINTAINAPPLICATIONSTATUS == 2) || (v.MAINTAINAPPLICATIONSTATUS == 3)) &&
                       (string.IsNullOrEmpty(vehcileID) ? true : v.VEHCILE_ID.Contains(vehcileID)) &&
                (string.IsNullOrEmpty(contact) ? true : v.CONTACT.Contains(contact)) &&
                (string.IsNullOrEmpty(worker) ? true : v.WORKER.Contains(worker)) && m.CLIENT_ID == clientID
                       orderby m.CREATE_TIME descending
                       select new MaintainRecordUnfinished()
                       {
                           ID = v.ID,
                           VehcileID = v.VEHCILE_ID,
                           Applicant = v.APPLICANT,
                           Contact = v.CONTACT,
                           ApplicationID = v.APPLICATIONID,
                           ApplicationStatus = v.MAINTAINAPPLICATIONSTATUS,
                           Problem = v.PROBLEM,
                           ScheduleDate = v.SCHEDULE_DATE,
                           SetupStation = v.SETUP_STATION,
                           Worker = m.WORKER,
                           CreateTime = v.CREATE_TIME,
                           Status = v.STATUS.HasValue ? v.STATUS.Value : -1,
                           StartTime = v.START_TIME.HasValue ? v.START_TIME.Value : new System.Nullable<DateTime>(),
                           EndTime = v.END_TIME.HasValue ? v.END_TIME.Value : new System.Nullable<DateTime>()

                       };
            var items = sour.Page(out totalCount, pageIndex, pageSize, true, n => n.CreateTime, false)
                .ToList();

            return new MultiMessage<MaintainRecordUnfinished>(items, totalCount);

        }


    }
}

