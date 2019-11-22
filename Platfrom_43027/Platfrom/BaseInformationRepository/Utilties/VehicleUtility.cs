using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Ant.BaseInformation.Repository.Utilties
{
    public class VehicleUtility
    {
        public static BSC_VEHICLE UpdateEntity(BSC_VEHICLE entity, Vehicle model, bool isAdd)
        {
            if (isAdd)
            {

                entity.VEHICLE_ID = model.VehicleId;
                entity.CLIENT_ID = model.ClientId;

            }
            entity.CLIENT_ID = model.ClientId;
            entity.ORGNIZATION_ID = model.OrgnizationId;
            entity.VEHICLE_ID = model.VehicleId;
            entity.VEHICLE_SN = model.VehicleSn;
            entity.ENGINE_ID = model.EngineId;
            entity.BRAND_MODEL = model.BrandModel;
            entity.DISTRICT_CODE = model.DistrictCode;
            entity.OPERATION_LICENSE = model.OperationLicense;
            entity.VEHICLE_STATUS = (short)model.VehicleStatus;
            entity.OWNER = model.Owner;
            entity.CONTACT = model.Contact;
            entity.CONTACT_ADDRESS = model.ContactAddress;
            entity.CONTACT_EMAIL = model.ContactEmail;
            entity.CONTACT_PHONE = model.ContactPhone;
            entity.REGION = model.Region;
            entity.START_YEAR = model.StartYear;
            entity.SERVICE_TYPE = (short)model.ServiceType;
            entity.NOTE = model.Note;
            entity.CREATOR = model.Creator;
            entity.CREATE_TIME = model.CreateTime;
            if (model.Valid == 1)
                entity.VALID = 1;
            else
                entity.VALID = 0;
            return entity;
        }

        public static Vehicle GetModel(BSC_VEHICLE entity)
        {
            Vehicle model = new Vehicle();
            model.ClientId = entity.CLIENT_ID;
            model.OrgnizationId = entity.ORGNIZATION_ID;
            model.VehicleId = entity.VEHICLE_ID;
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
            model.CreateTime = (DateTime)entity.CREATE_TIME;
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

        public static VVehicle GetVVehicleModel(BSC_VEHICLE entity)
        {
            VVehicle vv = new VVehicle();
            Vehicle model = new Vehicle();
            model.ClientId = entity.CLIENT_ID;
            model.OrgnizationId = entity.ORGNIZATION_ID;
            model.VehicleId = entity.VEHICLE_ID;
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
            model.CreateTime = (DateTime)entity.CREATE_TIME;
            if (entity.VALID == 1)
            {
                model.Valid = 1;
            }
            else
            {
                model.Valid = 0;
            }

            vv.Vehicles = model;
            return vv;
        }
    }
}
