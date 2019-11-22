using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.BaseInfo;
using Gsafety.Common.Util;
using Gsafety.Common.Logging;

namespace Gsafety.PTMS.BaseInformation.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class VehicleService : BaseService, IVehicleService
    {
        private VehicleRepository Repository = new VehicleRepository();
        public VehicleService()
        {
        }

        public SingleMessage<bool> AddVehicle(Vehicle vehicle)
        {
            try
            {
                Info("AddVehicle");
                Info("vehicle:" + Convert.ToString(vehicle));
                var temp = Repository.AddVehicle(vehicle);
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

        public SingleMessage<bool> DeleteVehicle(string vehicleId)
        {
            try
            {
                Info("DeleteVehicle");
                Info("vehicleId:" + Convert.ToString(vehicleId));
                var temp = Repository.DeleteVehicle(vehicleId);
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

        public SingleMessage<bool> UpdateVehicle(Vehicle vehicle)
        {
            try
            {
                Info("UpdateVehicle");
                Info("vehicle:" + Convert.ToString(vehicle));
                var temp = Repository.UpdateVehicle(vehicle);
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

        public SingleMessage<VehicleCheckResult> CheckInstallVehicle(string vehicleId)
        {
            try
            {
                Info("CheckInstallVehicle");
                Info("vehicleId:" + Convert.ToString(vehicleId));
                var temp = Repository.CheckInstallVehicle(vehicleId);
                SingleMessage<VehicleCheckResult> result = new SingleMessage<VehicleCheckResult>() { Result = temp, IsSuccess = true };
                Log<VehicleCheckResult>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<VehicleCheckResult>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public MultiMessage<Vehicle> GetVehiclesFuzzy(string vehicleId, string districtCode, VehicleType? type, string owner, InstallStatusType? status, PagingInfo page)
        {
            try
            {
                Info("GetVehiclesFuzzy");
                Info("vehicleId:" + Convert.ToString(vehicleId) + ";" + "districtCode:" + Convert.ToString(districtCode) + ";" + "type:" + Convert.ToString(type) + ";" + "owner:" + Convert.ToString(owner) + ";" + "status:" + Convert.ToString(status) + ";" + "page:" + Convert.ToString(page));
                int totalRecord;
                var temp = Repository.GetVehiclesFuzzy(vehicleId, districtCode, type, owner, status, page, out totalRecord, GetUserInfo());
                MultiMessage<Vehicle> result = new MultiMessage<Vehicle>() { Result = temp, TotalRecord = totalRecord };
                Log<Vehicle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Vehicle>() { ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> UpdateVehicleStatusByVehicleId(string vehicleId, int status, string note)
        {
            try
            {
                Info("UpdateVehicleStatusByVehicleId");
                Info("vehicleId:" + Convert.ToString(vehicleId) + ";" + "status:" + Convert.ToString(status) + ";" + "note:" + Convert.ToString(note));
                var temp = Repository.UpdateVehicleStatusByVehicleId(vehicleId, status, note);
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

        public SingleMessage<bool> CheckVehicleExistByVehicleId(string vehicleId)
        {
            try
            {
                Info("CheckVehicleExistByVehicleId");
                Info("vehicleId:" + Convert.ToString(vehicleId));
                var temp = Repository.CheckVehicleExistByVehicleId(vehicleId);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = true, Result = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> CheckVehicleExistByVehicleSn(string vehicleSn)
        {
            try
            {
                Info("CheckVehicleExistByVehicleSn");
                Info("vehicleSn:" + Convert.ToString(vehicleSn));
                var temp = Repository.CheckVehicleExistByVehicleSn(vehicleSn);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = true, Result = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> CheckVehicleExist(List<Vehicle> vehicleList)
        {
            try
            {
                Info("CheckVehicleExist");
                Info("vehicleList:" + Convert.ToString(vehicleList));
                var temp = Repository.CheckVehicleExist(vehicleList);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = true, Result = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> BatchAdd(List<Vehicle> vehicleList)
        {
            try
            {
                Info("BatchAdd");
                Info("vehicleList:" + Convert.ToString(vehicleList));
                var temp = Repository.BatchAdd(vehicleList);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = true, Result = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<Vehicle> GetInstalSecuritVehicleByMDVRID(string mdvrID)
        {
            try
            {
                Info("GetInstalSecuritVehicleByMDVRID");
                Info("mdvrID:" + Convert.ToString(mdvrID));
                var temp = Repository.GetInstalSecuritVehicleByMDVRID(mdvrID);
                SingleMessage<Vehicle> result = new SingleMessage<Vehicle>() { IsSuccess = true, Result = temp };
                Log<Vehicle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<Vehicle>() { IsSuccess = false, ExceptionMessage = ex };
            }

        }

        /// <summary>
        /// find the vehicl which had through rule install mdvr
        /// </summary>
        /// <returns></returns>
        public byte[] GetInstalSecuritVehiclesByAuthority()
        {
            try
            {
                Info("GetInstalSecuritVehiclesByAuthority");
                UserInfoMessageHeader userInfo = GetUserInfo();
                var result = Repository.GetInstalSecuritVehiclesByAuthority(userInfo.Province, userInfo.City, userInfo.Group, userInfo.Region, userInfo.Level);

                return CompressedSerializer.Compress<MultiMessage<Vehicle>>(new MultiMessage<Vehicle>() { Result = result });
            }
            catch (Exception ex)
            {
                Error(ex);
                return CompressedSerializer.Compress<MultiMessage<Vehicle>>(new MultiMessage<Vehicle>() { ExceptionMessage = ex });
            }
        }
    }
}
