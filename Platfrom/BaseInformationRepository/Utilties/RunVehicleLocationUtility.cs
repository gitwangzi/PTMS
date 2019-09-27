using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.BaseInformation.Repository
{
    public class RunVehicleLocationUtility
    {

        public static RUN_VEHICLE_LOCATION UpdateEntity(RUN_VEHICLE_LOCATION entity, RunVehicleLocation model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
            }
            entity.CLIENT_ID = model.ClientId;
            entity.SOURCE = (short)model.Source;
            entity.VEHICLE_ID = model.GpsValid;
            entity.LATITUDE = model.Latitude;
            entity.LONGITUDE = model.Longitude;
            entity.SPEED = model.Speed;
            entity.DIRECTION = model.Direction;
            entity.GPS_TIME = model.GpsTime;
            entity.DISTRICT_CODE = model.DistrictCode;
            entity.ALARM_FLAG = (short)model.AlarmFlag;
            entity.VEHICLE_ID = model.VehicleId;
            entity.STATUS_FLAG = (short)model.StatusFlag;
            return entity;
        }

        public static RunVehicleLocation GetModel(RUN_VEHICLE_LOCATION entity)
        {
            RunVehicleLocation model = new RunVehicleLocation();
            model.ID = entity.ID;
            model.ClientId = entity.CLIENT_ID;
            model.Source = entity.SOURCE;
            model.GpsValid = entity.GPS_VALID;
            model.Latitude = entity.LATITUDE;
            model.Longitude = entity.LONGITUDE;
            model.Speed = entity.SPEED;
            model.Direction = entity.DIRECTION;
            model.GpsTime = entity.GPS_TIME;
            model.DistrictCode = entity.DISTRICT_CODE;
            model.AlarmFlag = entity.ALARM_FLAG;
            model.VehicleId = entity.VEHICLE_ID;
            model.StatusFlag = entity.STATUS_FLAG;
            return model;
        }

    }
}

