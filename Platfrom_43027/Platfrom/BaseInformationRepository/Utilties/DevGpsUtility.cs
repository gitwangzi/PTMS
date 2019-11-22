using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.Ant.BaseInformation.Repository.Utilties
{
    public class DevGpsUtility
    {

        public static BSC_DEV_GPS UpdateEntity(BSC_DEV_GPS entity, DevGps model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
            }
            entity.CLIENT_ID = model.ClientId;
            entity.GPS_SN = model.GpsSn;
            entity.STATUS = (short)model.Status;
            entity.DISTRICT_CODE = model.DistrictCode;
            entity.GPS_SIM = model.GpsSim;
            entity.CREATOR = model.Creator;
            entity.CREATE_TIME = model.CreateTime;
            entity.GPS_UID = model.GpsUid;
            if (model.Valid == 1)
                entity.VALID = 1;
            else
                entity.VALID = 0;
            return entity;
        }

        public static DevGps GetModel(BSC_DEV_GPS entity)
        {
            DevGps model = new DevGps();
            model.ID = entity.ID;
            model.ClientId = entity.CLIENT_ID;
            model.GpsSn = entity.GPS_SN;
            model.Status = entity.STATUS;
            model.DistrictCode = entity.DISTRICT_CODE;
            model.GpsSim = entity.GPS_SIM;
            model.Creator = entity.CREATOR;
            model.CreateTime = entity.CREATE_TIME;
            model.GpsUid = entity.GPS_UID;

            if (entity.VALID == 1)
            {
                model.Valid = 1;
            }
            else
            {
                model.Valid = 0;
            }
            return model;
        }

    }
}
