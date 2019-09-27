using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.BaseInformation.Repository
{
    public class BscVehicleUtility
    {
        public static BSC_VEHICLE UpdateEntity(BSC_VEHICLE _entity, Vehicle model, bool isAdd)
        {
            if (isAdd)
            {
                _entity.VEHICLE_ID = model.VehicleId;
            }
            _entity.CLIENT_ID = model.ClientId;
            _entity.ORGNIZATION_ID = model.OrgnizationId;
            _entity.VEHICLE_SN = model.VehicleSn;
            _entity.ENGINE_ID = model.EngineId;
            _entity.BRAND_MODEL = model.BrandModel;
            _entity.DISTRICT_CODE = model.DistrictCode;
            _entity.OPERATION_LICENSE = model.OperationLicense;
            _entity.VEHICLE_STATUS = 1;//model.VehicleStatus;
            _entity.OWNER = model.Owner;
            _entity.CONTACT = model.Contact;
            _entity.CONTACT_ADDRESS = model.ContactAddress;
            _entity.CONTACT_EMAIL = model.ContactEmail;
            _entity.CONTACT_PHONE = model.ContactPhone;
            _entity.REGION = model.Region;
            _entity.START_YEAR = model.StartYear;
            _entity.SERVICE_TYPE = (short?)model.ServiceType;
            _entity.NOTE = model.Note;
            _entity.CREATOR = model.Creator;
            _entity.CREATE_TIME = model.CreateTime;
            _entity.VEHICLE_TYPE = model.VehicleType.ID;
            if (model.Valid == 1)
                _entity.VALID = 1;
            else
                _entity.VALID = 0;
            return _entity;
        }

        public static Vehicle GetModel(BSC_VEHICLE entity)
        {
            Vehicle model = new Vehicle();
            model.VehicleId = entity.VEHICLE_ID;
            model.ClientId = entity.CLIENT_ID;
            model.OrgnizationId = entity.ORGNIZATION_ID;
            model.VehicleSn = entity.VEHICLE_SN;
            model.EngineId = entity.ENGINE_ID;
            model.BrandModel = entity.BRAND_MODEL;
            model.DistrictCode = entity.DISTRICT_CODE;
            model.OperationLicense = entity.OPERATION_LICENSE;
            model.VehicleStatus = (VehicleConditionType)entity.VEHICLE_STATUS;
            model.Owner = entity.OWNER;
            model.Contact = entity.CONTACT;
            model.ContactAddress = entity.CONTACT_ADDRESS;
            model.ContactEmail = entity.CONTACT_EMAIL;
            model.ContactPhone = entity.CONTACT_PHONE;
            model.Region = entity.REGION;
            model.StartYear = entity.START_YEAR;
            model.ServiceType = (VehicleSeviceType)entity.SERVICE_TYPE;
            model.Note = entity.NOTE;
            model.Creator = entity.CREATOR;
            model.CreateTime = entity.CREATE_TIME == null ? DateTime.UtcNow : (DateTime)entity.CREATE_TIME;
            model.VehicleType = new VehicleType();
            model.VehicleType.ID = entity.VEHICLE_TYPE ?? "Empty";
            if (entity.VALID == 0)
            {
                model.Valid = 0;
            }
            else
            {
                model.Valid = 1;
            }
            return model;
        }

    }
}

