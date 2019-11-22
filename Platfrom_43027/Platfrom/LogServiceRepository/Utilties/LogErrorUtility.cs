using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogServiceRepository.Utilties
{
    public class LogErrorUtility
    {
        public static LOG_ERROR UpdateEntity(LOG_ERROR entity, LogError model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
            }
            entity.CREATE_TIME = model.CreateTime;
            entity.ERROR_REASON = model.ErrorReason;
            return entity;
        }

        public static LogError GetModel(LOG_ERROR entity)
        {
            LogError model = new LogError();
            model.ID = entity.ID;
            if (entity.CREATE_TIME.HasValue)
            {
                model.CreateTime = entity.CREATE_TIME.Value;
            }
            model.ErrorReason = entity.ERROR_REASON;
            
            return model;
        }
    }
}
