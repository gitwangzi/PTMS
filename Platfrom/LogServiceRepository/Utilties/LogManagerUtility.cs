using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using LogServiceContract.Data;

namespace LogServiceRepository.Utilties
{
    public class LogManagerUtility
    {

        public static LOG_MANAGER UpdateEntity(LOG_MANAGER entity, LogManager model, bool isAdd)
        {
            // LOG_MANAGER entity = new LOG_MANAGER();
            if (isAdd)
            {
                entity.ID = model.ID;
            }
            entity.CLIENT_ID = model.ClientID;
            entity.CLIENT_NAME = model.ClientName;
            entity.CONTENT = model.Content;
            entity.MANAGER_ID = model.ManagerID;
            entity.MANAGER = model.Manager;
            entity.CREATE_TIME = model.CreateTime;
            return entity;
        }

        public static LogManager GetModel(LOG_MANAGER entity)
        {
            LogManager model = new LogManager();
            model.ID = entity.ID;
            model.Content = entity.CONTENT;
            model.ManagerID = entity.MANAGER_ID;
            model.Manager = entity.MANAGER;
            model.CreateTime = entity.CREATE_TIME.Value;
            model.ClientName = entity.CLIENT_NAME;
            model.ClientID = entity.CLIENT_ID;
            if (entity.CREATE_TIME.HasValue)
            {
                model.CreateTime = entity.CREATE_TIME.Value;
            }
            return model;
        }

    }
}

