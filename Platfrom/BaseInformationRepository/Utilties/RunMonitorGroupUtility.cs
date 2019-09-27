using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Monitor.Contract.Data;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.BaseInformation.Repository
{
    public class RunMonitorGroupUtility
    {

        public static RUN_MONITOR_GROUP UpdateEntity(RUN_MONITOR_GROUP entity, RunMonitorGroup model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
            }
            entity.AD_USER = model.AdUser;
            entity.GROUP_NAME = model.GroupName;
            entity.GROUP_TYPE = (short)model.GroupType;
            entity.IS_DEFAULT = (short)model.IsDefault;
            entity.GROUP_INDEX = (short)model.GroupIndex;
            entity.NOTE = model.Note;
            return entity;
        }

        public static RunMonitorGroup GetModel(RUN_MONITOR_GROUP entity)
        {
            RunMonitorGroup model = new RunMonitorGroup();
            model.ID = entity.ID;
            model.AdUser = entity.AD_USER;
            model.GroupName = entity.GROUP_NAME;
            model.GroupType = entity.GROUP_TYPE;
            model.IsDefault = entity.IS_DEFAULT;
            model.GroupIndex = (decimal)entity.GROUP_INDEX;
            model.Note = entity.NOTE;
            return model;
        }

    }
}

