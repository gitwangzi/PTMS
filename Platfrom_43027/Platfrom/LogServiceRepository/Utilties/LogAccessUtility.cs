using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using LogServiceContract.Data;

namespace LogServiceRepository.Utilties
{
    public class LogAccessUtility
    {

        public static LOG_ACCESS UpdateEntity(LOG_ACCESS entity, LogAccess model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
                entity.CLIENT_ID = model.ClientID;
            }
            entity.CLIENT_ID = model.ClientID;
            entity.LOGIN_TIME = model.LoginTime;
            entity.LOGOUT_TIME = model.LogoutTime;
            entity.SESSION_ID = model.SessionID;
            entity.USER_ID = model.UserID;
            entity.LOGIN_USER = model.LoginUser;
            entity.USER_TYPE = (short)model.UserType;
            return entity;
        }

        public static LogAccess GetModel(LOG_ACCESS entity)
        {
            LogAccess model = new LogAccess();
            model.ID = entity.ID;
            model.ClientID = entity.CLIENT_ID;
            if (entity.LOGIN_TIME.HasValue)
            {
                model.LoginTime = entity.LOGIN_TIME.Value;
            }
            if (entity.LOGOUT_TIME.HasValue)
            {
                model.LogoutTime = entity.LOGOUT_TIME.Value;
            }
            model.LoginUser = entity.LOGIN_USER;
            if (entity.USER_TYPE.HasValue)
            {
                model.UserType = entity.USER_TYPE.Value;
            }
            model.SessionID = entity.SESSION_ID;
            model.UserID = entity.USER_ID;
            return model;
        }
    }
}
