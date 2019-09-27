using Gsafety.Common.Util;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Models;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Gs.PTMS.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class VehicleService : BaseService, IVehicleService, IVehicleType
    {
        private VehicleRepository Repository = new VehicleRepository();

        public SingleMessage<bool> AddVehicle(Vehicle vehicle)
        {
            Info("AddVehicle");
            Info(vehicle.ToString());

            try
            {
                SingleMessage<bool> result = null;
                vehicle.CreateTime = DateTime.UtcNow;
                using (var context = new PTMSEntities())
                {
                    result = VehicleRepository.InsertVehicle(context, vehicle);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { IsSuccess = false, ExceptionMessage = ex };
            }

        }

        public SingleMessage<bool> DeleteVehicle(Vehicle model)
        {
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VehicleRepository.DeleteVehicle(context, model.VehicleId);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { IsSuccess = false, ExceptionMessage = ex };
            }

        }

        public SingleMessage<bool> UpdateVehicle(Vehicle vehicle)
        {
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VehicleRepository.UpdateVehicle(context, vehicle);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<VehicleCheckResultExt> CheckInstallVehicleForSuite(string vehicleId, string clientID)
        {
            try
            {
                Info("CheckInstallVehicle");
                Info("vehicleId:" + Convert.ToString(vehicleId));
                var temp = Repository.CheckInstallVehicleForSuite(vehicleId, clientID);
                SingleMessage<VehicleCheckResultExt> result = new SingleMessage<VehicleCheckResultExt>() { Result = temp, IsSuccess = true };
                Log<VehicleCheckResultExt>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<VehicleCheckResultExt>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<VehicleCheckResultExt> CheckInstallVehicleForGPS(string vehicleId, string clientID)
        {
            try
            {
                Info("CheckInstallVehicle");
                Info("vehicleId:" + Convert.ToString(vehicleId));
                var temp = Repository.CheckInstallVehicleForGPS(vehicleId, clientID);
                SingleMessage<VehicleCheckResultExt> result = new SingleMessage<VehicleCheckResultExt>() { Result = temp, IsSuccess = true };
                Log<VehicleCheckResultExt>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<VehicleCheckResultExt>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        //public MultiMessage<Vehicle> GetVehiclesFuzzy(string vehicleId, string districtCode, VehicleTypeEnum? type, string owner, InstallStatusType? status, PagingInfo page)
        //{
        //    try
        //    {
        //        Info("GetVehiclesFuzzy");
        //        Info("vehicleId:" + Convert.ToString(vehicleId) + ";" + "districtCode:" + Convert.ToString(districtCode) + ";" + "type:" + Convert.ToString(type) + ";" + "owner:" + Convert.ToString(owner) + ";" + "status:" + Convert.ToString(status) + ";" + "page:" + Convert.ToString(page));
        //        int totalRecord;
        //        var temp = Repository.GetVehiclesFuzzy(vehicleId, districtCode, type, owner, status, page, out totalRecord, GetUserInfo());
        //        MultiMessage<Vehicle> result = new MultiMessage<Vehicle>() { Result = temp, TotalRecord = totalRecord };
        //        Log<Vehicle>(result);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Error(ex);
        //        return new MultiMessage<Vehicle>() { ExceptionMessage = ex };
        //    }
        //}

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

        public MultiMessage<Vehicle> CheckVehicleExist(List<Vehicle> vehicleList)
        {
            try
            {
                Info("CheckVehicleExist");
                Info("vehicleList:" + Convert.ToString(vehicleList));
                var temp = Repository.CheckVehicleExist(vehicleList);
                Log<Vehicle>(temp);
                return temp;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Vehicle>(false, ex);
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
                var result = Repository.GetInstalSecuritVehiclesByAuthority(userInfo.Province, userInfo.City, userInfo.Group, userInfo.Region, userInfo.Level, 0);

                return CompressedSerializer.Compress<MultiMessage<Vehicle>>(new MultiMessage<Vehicle>() { Result = result });
            }
            catch (Exception ex)
            {
                Error(ex);
                return CompressedSerializer.Compress<MultiMessage<Vehicle>>(new MultiMessage<Vehicle>() { ExceptionMessage = ex });
            }
        }


        public SingleMessage<bool> InsertVehicleTypeColor( List<VehicleTypeColor> color)
        {
            Info("InsertVehicleType");
          
            try
            {
                SingleMessage<bool> result = null;
               
                using (var context = new PTMSEntities())
                {
                    result = VehicleTypeRepository.InsertVehicleTypeColor(context, color);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public SingleMessage<bool> UpdateVehicleTypeColor(VehicleTypeColor model)
        {
            Info("InsertVehicleType");
          
            try
            {
                SingleMessage<bool> result = null;
               
                using (var context = new PTMSEntities())
                {
                    result = VehicleTypeRepository.UpdateVehicleTypeColor(context, model);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }
        public SingleMessage<bool> DeleteVehicleTypeColor(string  id)
        {
            Info("DeleteVehicleTypeColor");
         
            try
            {
                SingleMessage<bool> result = null;
             
                using (var context = new PTMSEntities())
                {
                    result = VehicleTypeRepository.DeleteVehicleTypeColor(context, id);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public MultiMessage<VehicleTypeColor> GetVehicleTypeColorList(string typeid)
        {
            Info("GetVehicleTypeColorList");
           
            try
            {
                MultiMessage<VehicleTypeColor> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VehicleTypeRepository.GetVehicleTypeColorList(context, typeid);
                }
                Log<VehicleTypeColor>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<VehicleTypeColor>(false, ex);
            }
        }

        public MultiMessage<VehicleTypeColor> GetAllVehicleTypeColorList(string clientID)
        {
            Info("GetAllVehicleTypeColorList");

            try
            {
                MultiMessage<VehicleTypeColor> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VehicleTypeRepository.GetAllVehicleTypeColorList(context, clientID);
                }
                Log<VehicleTypeColor>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<VehicleTypeColor>(false, ex);
            }
        }

        /** 车辆类型 */
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public SingleMessage<bool> InsertVehicleType(VehicleType model)
        {
            Info("InsertVehicleType");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                model.CreateTime = DateTime.UtcNow;
                using (var context = new PTMSEntities())
                {
                    result = VehicleTypeRepository.InsertVehicleType(context, model);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        public SingleMessage<bool> UpdateVehicleType(VehicleType model)
        {
            Info("UpdateVehicleType");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VehicleTypeRepository.UpdateVehicleType(context, model);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }


        /// <summary>
        /// 删除
        /// </summary>
        public SingleMessage<bool> DeleteVehicleTypeByID(string ID)
        {
            Info("DeleteVehicleTypeByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VehicleTypeRepository.DeleteVehicleTypeByID(context, ID);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }


        public MultiMessage<VehicleType> GetByNameVehicleTypeList(int pageIndex, int pageSize, string clientID, string name)
        {
            Info("GetByNameVehicleTypeList");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<VehicleType> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VehicleTypeRepository.GetByNameVehicleTypeList(context, pageIndex, pageSize, clientID, name);
                }
                Log<VehicleType>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<VehicleType>(false, ex);
            }
        }

        #region zhangxw 160708

        /// <summary>
        /// 获取车辆
        /// </summary>
        public SingleMessage<Vehicle> GetBscVehicle(string VehicleId)
        {
            Info("GetBscVehicle");
            Info(VehicleId.ToString());
            try
            {
                SingleMessage<Vehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VehicleRepository.GetBscVehicle(context, VehicleId);
                }
                Log<Vehicle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<Vehicle>(false, ex);
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="SearchVehicleId"></param>
        /// <param name="SearchOwner"></param>
        /// <param name="SearchVehicleType"></param>
        /// <returns></returns>
        public MultiMessage<Vehicle> GetBscVehicleList(int pageIndex, int pageSize, string SearchVehicleId, string SearchOwner, string SearchVehicleType, string orgId, string vehicletypeid)
        {
            Info("GetBscVehicleList");
            try
            {
                MultiMessage<Vehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VehicleRepository.GetBscVehicleList(context, pageIndex, pageSize, SearchVehicleId, SearchOwner, SearchVehicleType, orgId, vehicletypeid);
                }
                Log<Vehicle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Vehicle>(false, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MultiMessage<VehicleType> GetVehicleTypeList(string clientID)
        {
            MultiMessage<VehicleType> result = null;
            using (var context = new PTMSEntities())
            {
                result = VehicleRepository.GetVehicleTypeList(context, clientID);
            }
            Log<VehicleType>(result);
            return result;
        }

        public MultiMessage<Vehicle> GetChauffeurVehicle(string clientID)
        {
            Info("GetChauffeurVehicle");
            Info("clientID:" + clientID);
            try
            {
                MultiMessage<Vehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    var temp = VehicleRepository.GetChauffeurVehicle(context, clientID);
                    result = new MultiMessage<Vehicle>() { Result = temp, IsSuccess = true, TotalRecord = temp.Count };
                }
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Vehicle>(false, ex);
            }
        }

        public MultiMessage<VVehicle> GetChauffeurVehiclePageList(PagingInfo page, string clientID, string vehicleNum)
        {
            Info("GetChauffeurVehicle");
            Info("clientID:" + clientID);
            try
            {
                MultiMessage<VVehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = VehicleRepository.GetChauffeurVehiclePageList(context, page, clientID, vehicleNum);
                }
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<VVehicle>(false, ex);
            }
        }

        public MultiMessage<Vehicle> GetInstallVehiclesByAuthority(List<string> organizations)
        {
            try
            {
                Info("GetInstallVehiclesByAuthority");

                MultiMessage<Vehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = Repository.GetInstallVehiclesByAuthority(context, organizations);
                }

                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Vehicle>(false, ex);
            }
        }

        public SingleMessage<Vehicle> GetInstallVehicle(string organization, string vehicleid)
        {
            try
            {
                Info("GetInstallVehicle");

                SingleMessage<Vehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = Repository.GetInstallVehicle(context, organization, vehicleid);
                }

                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<Vehicle>(false, ex);
            }
        }
    }
}
