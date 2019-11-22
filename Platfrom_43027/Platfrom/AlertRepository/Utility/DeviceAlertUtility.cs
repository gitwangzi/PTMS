using Gsafety.PTMS.Alert.Contract.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Alert.Repository.Utility
{
    public class DeviceAlertUtility
    {
        public static ALT_DEVICE_ALERT UpdateEntity(ALT_DEVICE_ALERT entity, DeviceAlert model, bool isAdd)
        {
            if (isAdd)
            {

                entity.ID = model.Id;

            }
            entity.CLIENT_ID = model.ClientID;
            entity.MDVR_CORE_SN = model.MdvrCoreId;
            entity.SUITE_INFO_ID = model.SuiteInfoId;
            entity.VEHICLE_ID = model.VehicleId;
            entity.SUITE_STATUS = model.SuiteStatus;
            entity.ALERT_TYPE = model.AlertType;
            entity.ALERT_TIME = model.AlertTime;
            entity.GPS_VALID = model.GpsValid;
            entity.STATUS = model.Status;
            entity.CHECK_ID = model.CheckID;
            entity.HANDLE_ID = model.HandleId;
            entity.DISTRICT_CODE = model.DistrictCode;
            entity.LONGITUDE = model.Longitude;
            entity.LATITUDE = model.Latitude;
            entity.GPS_TIME = model.GpsTime;
            entity.SPEED = model.Speed;
            entity.DIRECTION = model.Direction;
            entity.ADDITIONAL_INFO = model.AdditionalInfo;
            entity.CREATE_TIME = model.CreateTime;
            return entity;
        }

        public static DeviceAlert GetModel(ALT_DEVICE_ALERT entity)
        {
            DeviceAlert model = new DeviceAlert();
            model.Id = entity.ID;
            model.ClientID = entity.CLIENT_ID;
            model.MdvrCoreId = entity.MDVR_CORE_SN;
            model.SuiteInfoId = entity.SUITE_INFO_ID;
            model.VehicleId = entity.VEHICLE_ID;
            model.SuiteStatus = entity.SUITE_STATUS;
            model.AlertType = entity.ALERT_TYPE;
            model.AlertTime = entity.ALERT_TIME;
            model.GpsValid = entity.GPS_VALID;
            model.Status = entity.STATUS;
            model.CheckID = entity.CHECK_ID;
            model.HandleId = entity.HANDLE_ID;
            model.DistrictCode = entity.DISTRICT_CODE;
            model.Longitude = entity.LONGITUDE;
            model.Latitude = entity.LATITUDE;
            model.GpsTime = entity.GPS_TIME;
            model.Speed = entity.SPEED;
            model.Direction = entity.DIRECTION;
            model.AdditionalInfo = entity.ADDITIONAL_INFO;
            model.CreateTime = entity.CREATE_TIME;
            return model;
        }
    }
}
