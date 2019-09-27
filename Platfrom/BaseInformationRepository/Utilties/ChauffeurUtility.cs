using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;

namespace Gsafety.Ant.BaseInformation.Repository.Utilties
{
    public class ChauffeurUtility
    {
        public static BSC_CHAUFFEUR UpdateEntity(BSC_CHAUFFEUR entity, Chauffeur model, bool isAdd)
        {
            //entity = new BSC_SETUP_STATION();
            if (isAdd)
            {

                entity.ID = model.ID;
                entity.CLIENT_ID = model.ClientID;

            }
            entity.CLIENT_ID = model.ClientID;
            entity.ICARD_ID = model.ICardID;
            entity.NAME = model.Name;  
            entity.DRIVER_LICENSE = model.DriverLicense;
            entity.PHONE = model.Phone;
            entity.EMAIL = model.Email;
            entity.CELLPHONE = model.CellPhone;   
            entity.ADDRESS = model.Address;
            entity.NOTE = model.Note;
            entity.CREATOR = model.Creator;
            entity.CREATE_TIME = model.CreateTime;
            if (model.Valid == 1)
                entity.VALID = 1;
            else
                entity.VALID = 0;
            return entity;
        }

        public static Chauffeur GetModel(BSC_CHAUFFEUR entity)
        {
            Chauffeur model = new Chauffeur();
            model.ID = entity.ID;
            model.ClientID = entity.CLIENT_ID;
            model.ICardID = entity.ICARD_ID;
            model.Name = entity.NAME;
            model.DriverLicense=entity.DRIVER_LICENSE;
            model.Phone=entity.PHONE;
            model.Email=entity.EMAIL;
            model.CellPhone=entity.CELLPHONE;
            model.Address=entity.ADDRESS;
            model.Email = entity.EMAIL;
            model.Note = entity.NOTE;
            model.Creator=entity.CREATOR;
            model.CreateTime = (entity.CREATE_TIME).ToLocalTime();
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
