using Gsafety.PTMS.BaseInformation.Contract.Models;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;

namespace Gsafety.PTMS.BaseInformation.Repository
{
    public class InstallationStaffUtility
    {

        public static BSC_INSTALLATION_STAFF UpdateEntity(BSC_INSTALLATION_STAFF entity, InstallationStaff model, bool isAdd)
        {
            if (isAdd)
            {


            }
            entity.ID = model.ID;
            entity.NAME = model.Name;
            entity.ICARD_ID = model.IcardID;
            entity.GRADE = (short)model.Grade;
            entity.STAFF_TYPE = (short)model.StaffType;
            entity.PHONE = model.Phone;
            entity.EMAIL = model.Email;
            entity.ADDRESS = model.Address;
            entity.STATION_ID = model.StationID;
            entity.NOTE = model.Note;
            if (model.Valid)
                entity.VALID = 1;
            else
                entity.VALID = 0;
            return entity;
        }

        public static InstallationStaff GetModel(BSC_INSTALLATION_STAFF entity)
        {
            InstallationStaff model = new InstallationStaff();
            model.ID = entity.ID;
            model.Name = entity.NAME;
            model.IcardID = entity.ICARD_ID;
            model.Grade = entity.GRADE.Value;
            model.StaffType = entity.STAFF_TYPE.Value;
            model.Phone = entity.PHONE;
            model.Email = entity.EMAIL;
            model.Address = entity.ADDRESS;
            model.StationID = entity.STATION_ID;
            model.Note = entity.NOTE;
            if (entity.VALID == 0)
            {
                model.Valid = false;
            }
            else
            {
                model.Valid = true;
            }
            return model;
        }

    }
}

