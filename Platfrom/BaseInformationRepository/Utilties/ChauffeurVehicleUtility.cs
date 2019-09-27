using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Ant.BaseInformation.Repository.Utilties
{
    public class ChauffeurVehicleUtility
    {
        public static BSC_VEHICLE_CHAUFFEUR UpdateEntity(BSC_VEHICLE_CHAUFFEUR entity, ChauffeurVehicle model)
        {

            entity.ID = model.ID;
            entity.CHAUFFEUR_ID = model.ChauffeurID;
            entity.VEHICLE_ID = model.VehicleID;
            entity.CREATE_TIME = model.CreateTime;
            entity.ACTIVATE = (short)model.Activate;
            return entity;
        }

        public static ChauffeurVehicle GetModel(BSC_VEHICLE_CHAUFFEUR entity)
        {
            ChauffeurVehicle model = new ChauffeurVehicle();
            model.ID = entity.ID;
            model.ChauffeurID = entity.CHAUFFEUR_ID;
            model.VehicleID = entity.VEHICLE_ID;
            model.CreateTime = entity.CREATE_TIME;
            model.Activate = entity.ACTIVATE;
            return model;
        }
    }
}
