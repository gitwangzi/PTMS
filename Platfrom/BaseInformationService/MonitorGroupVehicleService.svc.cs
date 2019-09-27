using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.BaseInfo;

namespace Gsafety.PTMS.BaseInformation.Service
{
    public class MonitorGroupVehicleService : BaseService, IMonitorGroupVehicleService
    {
        private MonitorGroupVehicleRepository Repository = new MonitorGroupVehicleRepository();

        public MultiMessage<MonitorGroupVehicle> GetAllMonitorGroupsVehicle(string userid)
        {
            try
            {
                Info("GetAllMonitorGroupsVehicle");

                var temp = Repository.GetAllMonitorGroupsVehicle(userid);
                MultiMessage<MonitorGroupVehicle> result = new MultiMessage<MonitorGroupVehicle>() { Result = temp };
                Log<MonitorGroupVehicle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<MonitorGroupVehicle>() { ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> AddMonitorGroupsVehicle(string Group_ID, string Vehicle_ID, short? TRACED_FLAG, int? VEHICLE_INDEX)
        {
            try
            {
                Info("AddMonitorGroupsVehicle");
                Info("Group_ID:" + Convert.ToString(Group_ID) + ";" + "Vehicle_ID:" + Convert.ToString(Vehicle_ID) + ";" + "TRACED_FLAG:" + Convert.ToString(TRACED_FLAG) + ";" + "VEHICLE_INDEX:" + Convert.ToString(VEHICLE_INDEX));
                var temp = Repository.AddMonitorGroupsVehicle(Group_ID, Vehicle_ID, TRACED_FLAG, VEHICLE_INDEX);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = temp };
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> RemoveMonitorGroupsVehicle(string monitorGroup_ID, string Vehicle_ID)
        {
            try
            {
                Info("RemoveMonitorGroupsVehicle");
                Info("monitorGroup_ID:" + Convert.ToString(monitorGroup_ID) + ";" + "Vehicle_ID:" + Convert.ToString(Vehicle_ID));
                var temp = Repository.RemoveMonitorGroupsVehicle(monitorGroup_ID, Vehicle_ID);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> MoveMonitorGroupsVehicle(string OldGroup_ID, string NewGroup_ID, string Vehicle_ID, short? TRACED_FLAG, int? VEHICLE_INDEX)
        {
            try
            {
                Info("MoveMonitorGroupsVehicle");
                Info("OldGroup_ID:" + Convert.ToString(OldGroup_ID) + ";" + "NewGroup_ID:" + Convert.ToString(NewGroup_ID) + ";" + "Vehicle_ID:" + Convert.ToString(Vehicle_ID) + ";" + "TRACED_FLAG:" + Convert.ToString(TRACED_FLAG) + ";" + "VEHICLE_INDEX:" + Convert.ToString(VEHICLE_INDEX));
                var temp = Repository.MoveMonitorGroupsVehicle(OldGroup_ID, NewGroup_ID, Vehicle_ID, TRACED_FLAG, VEHICLE_INDEX);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> SetMonitorGroupsVehicleState(string monitorGroup_ID, string Vehicle_ID, bool IsTrace)
        {
            try
            {
                Info("SetMonitorGroupsVehicleState");
                Info("monitorGroup_ID:" + Convert.ToString(monitorGroup_ID) + ";" + "Vehicle_ID:" + Convert.ToString(Vehicle_ID) + ";" + "IsTrace:" + Convert.ToString(IsTrace));
                var temp = Repository.SetMonitorGroupsVehicleState(monitorGroup_ID, Vehicle_ID, IsTrace);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }


        public SingleMessage<bool> AddMonitorGroupsVehicle(string Group_ID, string Vehicle_ID, short? TRACED_FLAG, int? VEHICLE_INDEX, string clientID)
        {
            throw new NotImplementedException();
        }
    }
}
