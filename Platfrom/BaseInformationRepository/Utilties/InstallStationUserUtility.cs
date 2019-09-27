using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Ant.BaseInformation.Repository.Utilties
{
    public class InstallStationUserUtility
    {
        public static BSC_SETUPSTATION_USER UpdateEntity(BSC_SETUPSTATION_USER entity, InstallStationUser model)
        {

            entity.ID = model.ID;
            entity.STATION_ID = model.InstallStationID;
            entity.USER_ID = model.UserID;           
            entity.CREATE_TIME = model.CreateTime;
            return entity;
        }

        public static InstallStationUser GetModel(BSC_SETUPSTATION_USER entity)
        {
            InstallStationUser model = new InstallStationUser();
            model.ID = entity.ID;
            model.InstallStationID = entity.STATION_ID;
            model.UserID = entity.USER_ID;
            model.CreateTime = entity.CREATE_TIME;         
            return model;
        }

    }
}
