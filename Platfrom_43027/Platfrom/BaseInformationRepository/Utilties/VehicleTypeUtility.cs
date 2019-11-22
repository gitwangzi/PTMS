using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.BaseInformation.Repository
{
    public class VehicleTypeUtility
    {

        public static BSC_VEHICLE_TYPE UpdateEntity(VehicleType model, BSC_VEHICLE_TYPE entity, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
                entity.CLIENT_ID = model.ClientID;

            }
            entity.NAME = model.Name;
            entity.DESCRIPTION = model.Description;
            entity.CREATE_TIME = model.CreateTime;
            entity.DESCRIPTION = model.Description;
            if (model.Image != null)
            {
                entity.ICON = model.Image;
            }

            //int length = entity.ICON.Length;
            entity.CREATOR = model.Creator;
            if (model.Valid == 1)
                entity.VALID = 1;
            else
                entity.VALID = 0;
            return entity;
        }

        public static VehicleType GetModel(BSC_VEHICLE_TYPE entity)
        {
            VehicleType model = new VehicleType();
            model.ID = entity.ID;
            model.ClientID = entity.CLIENT_ID;
            model.Name = entity.NAME;
            model.CreateTime = ((DateTime)entity.CREATE_TIME).ToLocalTime();
            model.Description = entity.DESCRIPTION;
            model.Creator = entity.CREATOR;
            model.Image = entity.ICON;
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

        public static VehicleTypeColor GetColorModel(BSC_VEHICLE_SPEEDCOLOR entity)
        {
            VehicleTypeColor model = new VehicleTypeColor();
            model.ID = entity.ID;
            model.TypeID = entity.TYPE_ID;
            model.MaxSpeed = entity.MAXSPEED.Value;
            model.MinSpeed = entity.MINSPEED.Value; 
            model.Color = entity.COLOR;
          
            return model;
        }

    }
}

