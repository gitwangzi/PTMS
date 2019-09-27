using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Traffic.Repository;
using Gsafety.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.Traffic.Contract;
using Gsafety.PTMS.Traffic.Contract.Data;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.Traffic.Service
{
    public class TrafficManageService : BaseService, ITrafficManageService
    {
        private TrafficRepository Repository = new TrafficRepository();
        #region Fence
        public MultiMessage<Fence> GetFenceByNameKeyAndVehcleID(string fenceName, string strVehcleID, short nState)
        {
            try
            {
                Info("GetFenceByNameKeyAndVehcleID");
                Info("fenceName:" + Convert.ToString(fenceName) + ";" + "strVehcleID:" + Convert.ToString(strVehcleID) + ";" + "nState:" + Convert.ToString(nState));
                UserInfoMessageHeader userinfo = GetUserInfo();
                MultiMessage<Fence> result = new MultiMessage<Fence>();
                result.IsSuccess = true;

                if (userinfo.Group == "SecurityManager" || userinfo.Group == "SecurityAdmin")
                {

                    using (PTMSEntities context = new PTMSEntities())
                    {
                        var temp = Repository.GetDistFenceByUserInfo(context, userinfo, fenceName, strVehcleID, nState);
                        result.Result = temp;
                        result.TotalRecord = temp.Count;
                    }

                }
                Log<Fence>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Fence>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }

        public MultiMessage<Fence> GetAllLFence()
        {
            try
            {
                Info("GetAllLFence");
                UserInfoMessageHeader userinfo = GetUserInfo();

                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = Repository.GetCompanyFenceByUserInfo(context, userinfo);
                    MultiMessage<Fence> result = new MultiMessage<Fence> { Result = temp, TotalRecord = temp.Count, IsSuccess = true };
                    Log<Fence>(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Fence>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }

        public SingleMessage<bool> AddCarFenceList(List<CarFence> listcarFence)
        {
            try
            {
                Info("AddCarFenceList");
                Info("listcarFence:" + Convert.ToString(listcarFence));

                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = Repository.AddVehicleFence(context, listcarFence);
                    SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                    Log<bool>(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<bool> AddCarFenceListForSelectModel(SelectInfoModel selectModel)
        {
            try
            {
                Info("selectModel:" + Convert.ToString(selectModel));
                if (selectModel.Type == SettingType.Vehicle)
                {
                    //foreach (var item in selectModel.)
                    //{

                    //}
                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public SingleMessage<bool> DeleteCarFenceList(List<string> listCarFence)
        {
            try
            {
                Info("DeleteCarFenceList");
                Info("listCarFence:" + Convert.ToString(listCarFence));
                var temp = Repository.DeleteVehicleFenceList(listCarFence);
                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public MultiMessage<decimal> GetValidFenceIDByCarID(string VehicleID, short nType)
        {
            try
            {
                Info("GetValidFenceIDByCarID");
                Info("VehicleID:" + Convert.ToString(VehicleID) + ";" + "nType:" + Convert.ToString(nType));

                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = Repository.GetValidFenceIDByCarID(context, VehicleID, nType);
                    MultiMessage<decimal> result = new MultiMessage<decimal> { Result = temp, TotalRecord = temp.Count, IsSuccess = true };
                    Log<decimal>(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<decimal>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }

        public SingleMessage<bool> CheckFenceNameExist(string strFenceName, short nType)
        {
            try
            {
                Info("CheckFenceNameExist");
                Info("strFenceName:" + Convert.ToString(strFenceName) + ";" + "nType:" + Convert.ToString(nType));

                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = Repository.BeExistFenceName(context, strFenceName, nType);
                    SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                    Log<bool>(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<bool> UpdateFenceVehicleState(string strID, short nState)
        {
            try
            {
                Info("UpdateFenceVehicleState");
                Info("strID:" + Convert.ToString(strID) + ";" + "nState:" + Convert.ToString(nState));
                var temp = Repository.UpdateVehicleState(strID, nState);
                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public MultiMessage<CarFence> GetVehicleFenceByFence(string fenceID, short nState)
        {
            try
            {
                Info("GetVehicleFenceByFence");
                Info("fenceID:" + Convert.ToString(fenceID) + ";" + "nState:" + Convert.ToString(nState));
                UserInfoMessageHeader userinfo = GetUserInfo();
                MultiMessage<CarFence> result = new MultiMessage<CarFence>() { TotalRecord = 0, IsSuccess = true };
                if (userinfo.Group == "SecurityManager" || userinfo.Group == "SecurityAdmin")
                {

                    using (PTMSEntities context = new PTMSEntities())
                    {
                        var temp = Repository.GetVehicleFenceByFence(context, fenceID, nState);
                        result.Result = temp;
                        result.TotalRecord = temp.Count;
                    }

                }

                Log<CarFence>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<CarFence>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }
        public MultiMessage<VehicleFenceFailedReason> GetVehicleFenceFailedReason(string vehicleID, string fenceID, short status)
        {
            try
            {
                Info("GetVehicleFenceFailedReason");
                Info("fenceID:" + Convert.ToString(fenceID) + ";");
                UserInfoMessageHeader userinfo = GetUserInfo();
                MultiMessage<VehicleFenceFailedReason> result = new MultiMessage<VehicleFenceFailedReason>() { IsSuccess = true };
                if (userinfo.Group == "SecurityManager" || userinfo.Group == "SecurityAdmin")
                {

                    using (PTMSEntities context = new PTMSEntities())
                    {
                        var temp = Repository.GetVehicleFenceFailedReason(context, vehicleID, fenceID, status);
                        result.Result = temp;
                    }

                }

                Log<VehicleFenceFailedReason>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<VehicleFenceFailedReason>() { ExceptionMessage = ex, IsSuccess = false };
            }

        }
        public Gsafety.PTMS.Base.Contract.Data.SingleMessage<bool> DeleteCarFenceByID(string ID)
        {
            try
            {
                Info("DeleteCarFenceByID");
                Info("ID:" + Convert.ToString(ID));

                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = Repository.DeleteVehicleFence(context, ID);
                    SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                    Log<bool>(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<bool> DeleteCarFenceByFenceID(string fenceID)
        {
            try
            {
                Info("DeleteCarFenceByFenceID");
                Info("fenceID:" + Convert.ToString(fenceID));

                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = Repository.DeleteVehicleFenceByFenceID(context, fenceID);
                    SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                    Log<bool>(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public Gsafety.PTMS.Base.Contract.Data.SingleMessage<bool> AddCarFence(CarFence carFence)
        {
            try
            {
                Info("AddCarFence");
                Info("carFence:" + Convert.ToString(carFence));

                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = Repository.AddVehicleFence(context, carFence);
                    SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                    Log<bool>(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public Gsafety.PTMS.Base.Contract.Data.MultiMessage<Vehicle> GetVehicleByFence(string fenceID, short nState)
        {
            try
            {
                Info("GetVehicleByFence");
                Info("fenceID:" + Convert.ToString(fenceID) + ";" + "nState:" + Convert.ToString(nState));

                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = Repository.GetVehicleByFence(context, fenceID, nState);
                    MultiMessage<Vehicle> result = new MultiMessage<Vehicle> { Result = temp, TotalRecord = temp.Count, IsSuccess = true };
                    Log<Vehicle>(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Vehicle>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }

        public MultiMessage<string> GetFenceIDByVehicleID(string VehicleID, short nState)
        {
            try
            {
                Info("GetFenceIDByVehicleID");
                Info("VehicleID:" + Convert.ToString(VehicleID) + ";" + "nState:" + Convert.ToString(nState));

                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = Repository.GetFenceIDByCarID(context, VehicleID, nState);
                    MultiMessage<string> result = new MultiMessage<string> { Result = temp, TotalRecord = temp.Count, IsSuccess = true };
                    Log<string>(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<string>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }

        public SingleMessage<bool> UpdateFenceVehicleListState(List<CarFence> listmodel, short nState)
        {
            try
            {
                Info("UpdateFenceVehicleListState");
                Info("listmodel:" + Convert.ToString(listmodel) + ";" + "nState:" + Convert.ToString(nState));

                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = Repository.UpdateVehicleState(context, listmodel, nState);
                    SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                    Log<bool>(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        #endregion
        #region LimitSpeed

        public SingleMessage<bool> CheckSpeedLimitNameExist(string strSpeedName)
        {
            try
            {
                Info("CheckSpeedLimitNameExist");
                Info("strSpeedName:" + Convert.ToString(strSpeedName));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.BeExistSpeedLimitName(context, strSpeedName);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<bool> CheckSpeedLimitidNameExist(string strSpeedName, string id)
        {
            try
            {
                Info("CheckSpeedLimitidNameExist");
                Info("strSpeedName:" + Convert.ToString(strSpeedName) + ";" + "id:" + Convert.ToString(id));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.BeExistSpeedLimitidName(context, strSpeedName, id);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public MultiMessage<SpeedLimit> GetSpeedLimitByNameKeyAndVeicleID(string Name, string VehicleID, short nStatus, PagingInfo pageInfo)
        {
            try
            {
                Info("GetSpeedLimitByNameKeyAndVeicleID");
                Info("Name:" + Convert.ToString(Name) + ";" + "VehicleID:" + Convert.ToString(VehicleID) + ";" + "nStatus:" + Convert.ToString(nStatus) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                UserInfoMessageHeader userinfo = GetUserInfo();

                var temp = new MultiMessage<SpeedLimit>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetDistSpeedLimitByNameKeyAndVeicleIDAndUserInfo(context, userinfo, Name, VehicleID, nStatus, pageInfo);
                }

                MultiMessage<SpeedLimit> result = new MultiMessage<SpeedLimit> { Result = temp.Result, TotalRecord = temp.TotalRecord, IsSuccess = true };
                Log<SpeedLimit>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<SpeedLimit>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }

        public SingleMessage<bool> DeleteVehicleToRule(string ruleID)
        {
            try
            {
                Info("DeleteVehicleToRule");
                Info("ruleID:" + Convert.ToString(ruleID));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.DeleteSpeedToVehilce(context, ruleID);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public MultiMessage<SpeedLimit> GetSpeedLimitByNameKey(string Name, short nStatus, PagingInfo pageInfo)
        {
            try
            {
                Info("GetSpeedLimitByNameKey");
                Info("Name:" + Convert.ToString(Name) + ";" + "nStatus:" + Convert.ToString(nStatus) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                UserInfoMessageHeader userinfo = GetUserInfo();

                var temp = new MultiMessage<SpeedLimit>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetDistSpeedLimitByNameKey(context, userinfo, Name, nStatus, pageInfo);
                }

                MultiMessage<SpeedLimit> result = new MultiMessage<SpeedLimit> { Result = temp.Result, TotalRecord = temp.TotalRecord, IsSuccess = true };
                Log<SpeedLimit>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<SpeedLimit>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }

        public SingleMessage<bool> AddSpeedLimit(SpeedLimit model)
        {
            try
            {
                Info("AddSpeedLimit");
                Info("model:" + Convert.ToString(model));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.AddSpeedLimit(context, model);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<bool> DeleteSpeedLimit(string strSpeedID)
        {
            try
            {
                Info("DeleteSpeedLimit");
                Info("strSpeedID:" + Convert.ToString(strSpeedID));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.DeleteSpeedLimit(context, strSpeedID);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public MultiMessage<SpeedStatusFailed> SpeedToVehicleFialState(string vehicleID, string ruleName, PagingInfo pageInfo)
        {
            try
            {
                Info("SpeedToVehicleFialState");
                Info("vehicleID:" + Convert.ToString(vehicleID) + ";" + "ruleName:" + Convert.ToString(ruleName) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                var temp = new MultiMessage<SpeedStatusFailed>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.SpeedToVehicleFialState(context, vehicleID, ruleName, pageInfo);
                }

                MultiMessage<SpeedStatusFailed> result = new MultiMessage<SpeedStatusFailed> { Result = temp.Result, TotalRecord = temp.TotalRecord, IsSuccess = true };
                Log<SpeedStatusFailed>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<SpeedStatusFailed>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<bool> UpdateVehicleSpeedStateBySpeedIDAndCarNum(SpeedLimit speedInfo)
        {
            try
            {
                Info("UpdateVehicleSpeedStateBySpeedIDAndCarNum");
                Info("speedInfo:" + Convert.ToString(speedInfo));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.UpdateVehicleSpeedState(context, speedInfo);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<bool> DeleteVehicleSpeedList(List<CarSpeed> listModel)
        {
            try
            {
                Info("DeleteVehicleSpeedList");
                Info("listModel:" + Convert.ToString(listModel));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.DeleteVehicleSpeedList(context, listModel);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public MultiMessage<Vehicle> GetVehicleBySpeedId(string speedId)
        {
            try
            {
                Info("GetVehicleBySpeedId");
                Info("speedId:" + Convert.ToString(speedId));
                var temp = new List<Vehicle>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetVehicleBySpeedID(context, speedId);
                }

                MultiMessage<Vehicle> result = new MultiMessage<Vehicle> { Result = temp, TotalRecord = temp.Count, IsSuccess = true };
                Log<Vehicle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Vehicle>() { ExceptionMessage = ex, TotalRecord = 0, IsSuccess = false };
            }
        }

        public SingleMessage<bool> DeleteCarSpeedByCarSpeed(string carNumber, string speedId)
        {
            try
            {
                Info("DeleteCarSpeedByCarSpeed");
                Info("carNumber:" + Convert.ToString(carNumber) + ";" + "speedId:" + Convert.ToString(speedId));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.DeleteVehicleSpeed(context, carNumber, speedId);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<bool> UpdateSpeedLimite(SpeedLimit updateSpeedLimit)
        {
            try
            {
                Info("UpdateSpeedLimite");
                Info("updateSpeedLimit:" + Convert.ToString(updateSpeedLimit));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.UpdateSpeedLimite(context, updateSpeedLimit);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        #endregion
    }
}
