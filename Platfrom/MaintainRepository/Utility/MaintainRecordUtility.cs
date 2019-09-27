using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.Maintain.Repository
{
    public class MaintainRecordUtility
    {

        public static MTN_MAINTAIN_RECORD UpdateEntity(MTN_MAINTAIN_RECORD entity, MaintainRecord model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
            }
            entity.APPLICATION_ID = model.ApplicationID;
            entity.WORKER = model.Worker;
            entity.CREATE_TIME = model.CreateTime;
            entity.STATUS = (short)model.Status;
            entity.START_TIME = model.StartTime;
            entity.END_TIME = model.EndTime;
            entity.NOTE = model.Note;
            return entity;
        }

        public static MaintainRecord GetModel(MTN_MAINTAIN_RECORD entity)
        {
            MaintainRecord model = new MaintainRecord();
            model.ID = entity.ID;
            model.ApplicationID = entity.APPLICATION_ID;
            model.Worker = entity.WORKER;
            model.CreateTime = entity.CREATE_TIME;
            model.Status = entity.STATUS;
            model.StartTime = entity.START_TIME;
            model.EndTime = entity.END_TIME;
            model.Note = entity.NOTE;
            return model;
        }

    }
}

