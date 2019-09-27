using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Monitor.Contract.Data;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.BaseInformation.Repository
{
    public class RunMonitorGroupVehicleUtility
    {

        public static RUN_MONITOR_GROUP_VEHICLE UpdateEntity(RUN_MONITOR_GROUP_VEHICLE entity, RunMonitorGroupVehicle model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = Guid.NewGuid().ToString() ;
            }
            else
            {
                entity.ID = model.ID;
            }
            entity.GROUP_ID = model.GroupId;
            entity.VEHICLE_ID = model.VehicleId;
            entity.TRACED_FLAG = (short)model.TracedFlag;
            entity.VEHICLE_INDEX = (int)model.VehicleIndex;
            return entity;
        }

        public static RunMonitorGroupVehicle GetModel(RUN_MONITOR_GROUP_VEHICLE entity)
        {
            RunMonitorGroupVehicle model = new RunMonitorGroupVehicle();
            model.ID = entity.ID;
            model.GroupId = entity.GROUP_ID;
            model.VehicleId = entity.VEHICLE_ID;
            model.TracedFlag = (decimal)entity.TRACED_FLAG;
            model.VehicleIndex = (decimal)entity.VEHICLE_INDEX;
            return model;
        }

    }
}

