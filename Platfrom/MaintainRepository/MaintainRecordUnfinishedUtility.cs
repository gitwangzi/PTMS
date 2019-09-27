using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.Maintain.Repository
{
    public class MaintainRecordUnfinishedUtility
    {
        public static MaintainRecordUnfinished GetModel(MNT_MAINTAINRECORD_UNFINISHED entity)
        {
            MaintainRecordUnfinished model = new MaintainRecordUnfinished();
            model.ID = entity.ID;
            model.VehcileID = entity.VEHCILE_ID;
            model.Applicant = entity.APPLICANT;
            model.Contact = entity.CONTACT;
            model.ApplicationID = entity.APPLICATIONID;
            model.ApplicationStatus = entity.MAINTAINAPPLICATIONSTATUS;
            model.Problem = entity.PROBLEM;
            model.ScheduleDate = entity.SCHEDULE_DATE;
            model.SetupStation = entity.SETUP_STATION;
            model.Worker = entity.WORKER;
            if (entity.START_TIME.HasValue)
                model.StartTime = entity.START_TIME.Value;
            if (entity.END_TIME.HasValue)
                model.EndTime = entity.END_TIME.Value;
            model.Note = entity.NOTE;
            if (entity.STATUS.HasValue)
                model.Status = entity.STATUS.Value;
            model.CreateTime = entity.CREATE_TIME;
            return model;
        }

    }
}

