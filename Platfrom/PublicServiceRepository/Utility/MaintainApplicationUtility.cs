using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace GSafety.PTMS.PublicService.Repository
{
    public class MaintainApplicationUtility
    {

        public static MTN_MAINTAIN_APPLICATION UpdateEntity(MTN_MAINTAIN_APPLICATION entity, MaintainApplication model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
            }
            entity.CLIENT_ID = model.ClientID;
            entity.APPLICANT = model.Applicant;
            entity.SETUP_STATION = model.SetupStation;
            entity.STATUS = (short)model.Status;
            entity.NOTE = model.Note;
            entity.WORKER = model.Worker;
            entity.WORKER_PHONE = model.WorkerPhone;
            entity.SCHEDULE_DATE = model.ScheduleDate;
            entity.CREATOR = model.Creator;
            entity.CREATE_TIME = model.CreateTime;
            entity.PROBLEM = model.Problem;
            entity.CONTACT = model.Contact;
            entity.VEHCILE_ID = model.VehicleID;
            return entity;
        }

        public static MaintainApplication GetModel(MTN_MAINTAIN_APPLICATION entity)
        {
            MaintainApplication model = new MaintainApplication();
            model.ID = entity.ID;
            model.ClientID = entity.CLIENT_ID;
            model.Applicant = entity.APPLICANT;
            model.SetupStation = entity.SETUP_STATION;
            model.Status = entity.STATUS;
            model.Note = entity.NOTE;
            model.Worker = entity.WORKER;
            model.WorkerPhone = entity.WORKER_PHONE;
            if (entity.SCHEDULE_DATE.HasValue)
                model.ScheduleDate = entity.SCHEDULE_DATE.Value;
            model.Creator = entity.CREATOR;
            model.CreateTime = entity.CREATE_TIME;
            model.Problem = entity.PROBLEM;
            model.Contact = entity.CONTACT;
            model.VehicleID = entity.VEHCILE_ID;
            return model;
        }

    }
}

