using Gsafety.PTMS.DBEntity;
using LogServiceContract.Data;

namespace LogServiceRepository.Utilties
{
    public class LogDataUtility
    {

        public static LOG_DATA UpdateEntity(LOG_DATA entity, LogData model, bool isAdd)
        {
            // LOG_DATA entity = new LOG_DATA();
            if (isAdd)
            {
                entity.ID = model.ID;
                entity.CLIENT_ID = model.ClientID;
            }
            entity.CLIENT_ID = model.ClientID;
            entity.USER_NAME = model.UserName;
            entity.MSG_ID = model.MsgID;
            entity.USER_TYPE = model.UserType;
            entity.CONTENTTYPE = (short)model.Contenttype;
            entity.USER_DEPT = model.UserDept;
            entity.MDVR_CORE_SN = model.MdvrCoreSn;
            entity.ACCESS_TIME = model.AccessTime;
            entity.VEHICLE_ID = model.VehicleId;
            entity.CHANNEL = model.Channel;
            entity.START_TIME = model.StartTime;
            entity.END_TIME = model.EndTime;
            entity.FILE_NAME = model.FileName;
            entity.EXTENED1 = model.Extened1;
            entity.EXTENED2 = model.Extened2;
            entity.EXTENED3 = model.Extened3;
            entity.CREATE_TIME = model.CreateTime;
            return entity;
        }

        public static LogData GetModel(LOG_DATA entity)
        {
            LogData model = new LogData();
            model.ID = entity.ID;
            model.ClientID = entity.CLIENT_ID;
            model.UserName = entity.USER_NAME;
            model.MsgID = entity.MSG_ID;
            model.UserType = entity.USER_TYPE;
            model.Contenttype = entity.CONTENTTYPE;
            model.UserDept = entity.USER_DEPT;
            model.MdvrCoreSn = entity.MDVR_CORE_SN;
            //model.AccessTime = entity.ACCESS_TIME.Value;
            if (entity.ACCESS_TIME.HasValue)
            {
                model.AccessTime = entity.ACCESS_TIME.Value;
            }
            model.VehicleId = entity.VEHICLE_ID;
            model.Channel = entity.CHANNEL;
            //model.StartTime = entity.STARTTIME;
            if (entity.START_TIME.HasValue)
            {
                model.StartTime = entity.START_TIME.Value;
            }
            //model.EndTime = entity.ENDTIME;
            if (entity.END_TIME.HasValue)
            {
                model.EndTime = entity.END_TIME.Value;
            }
            model.FileName = entity.FILE_NAME;
            model.Extened1 = entity.EXTENED1;
            model.Extened2 = entity.EXTENED2;
            model.Extened3 = entity.EXTENED3;
            //model.CreateTime = entity.CREATETIME;
            if (entity.CREATE_TIME.HasValue)
            {
                model.CreateTime = entity.CREATE_TIME.Value;
            }
            return model;
        }

    }
}

