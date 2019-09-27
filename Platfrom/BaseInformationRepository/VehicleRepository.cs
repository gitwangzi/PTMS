using Gsafety.Ant.BaseInformation.Repository.Utilties;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 50cd3052-a584-4b7f-a7a6-48d51c12e5ed      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Repository
/////    Project Description:    
/////             Class Name: VehicleRepository
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/20 16:36:42
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/20 16:36:42
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInfo.Conditions;
using Gsafety.PTMS.BaseInfo.Conditions.QueryFiler;
using Gsafety.PTMS.BaseInfo.Conditons.QueryFiler;
using Gsafety.PTMS.BaseInfo.MakerContions.Items;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.Common.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Gsafety.PTMS.BaseInformation.Repository
{
    public class VehicleRepository : Gsafety.PTMS.BaseInfo.BaseRepository
    {
        public VehicleRepository()
        {
        }

        /// <summary>
        /// 删除车辆
        /// </summary>
        /// <param name="context"></param>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public static SingleMessage<bool> DeleteVehicle(PTMSEntities context, string vehicleId)
        {
            try
            {
                var entity = context.BSC_VEHICLE.FirstOrDefault(v => v.VEHICLE_ID == vehicleId && v.VALID == 1);
                if (entity != null)
                {
                    bool reference = context.MTN_INSTALLATION_DETAIL.Any(n => n.VEHICLE_ID == vehicleId);
                    if (reference)
                    {
                        return new SingleMessage<bool>(false, "InstallWithSuite");
                    }
                    else
                    {
                        reference = context.MTN_GPS_INSTALLATION_DETAIL.Any(n => n.VEHICLE_ID == vehicleId);
                        if (reference)
                        {
                            return new SingleMessage<bool>(false, "InstallWithGPS");
                        }
                        else
                        {
                            reference = context.BSC_VEHICLE_CHAUFFEUR.Any(n => n.VEHICLE_ID == vehicleId);
                            if (reference)
                            {
                                return new SingleMessage<bool>(false, "BindingWithCHAUFFEUR");
                            }
                            else
                            {
                                reference = context.RUN_MOBILE_WORKING.Any(n => n.VEHICLE_ID == vehicleId);
                                if (reference)
                                {
                                    return new SingleMessage<bool>(false, "InstallWithMobile");
                                }
                            }
                        }
                    }
                    entity.VALID = 0;
                    if (context.SaveChanges() > 0)
                    {
                        return new SingleMessage<bool>(true);
                    }
                    else
                    {
                        return new SingleMessage<bool>(false, "FailedToSave");
                    }
                }
                else
                {
                    return new SingleMessage<bool>(false, "NotExist");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// 插入车辆信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public static SingleMessage<bool> InsertVehicle(PTMSEntities context, Vehicle vehicle)
        {
            bool exist = context.BSC_VEHICLE.Where(x => x.VEHICLE_ID == vehicle.VehicleId && x.VALID == 1).Any();
            if (exist)
            {
                return new SingleMessage<bool>(false, "VehicleIDDuplicate");
            }
            exist = context.BSC_VEHICLE.Where(x => x.VEHICLE_SN == vehicle.VehicleSn && x.VALID == 1 && x.CLIENT_ID == vehicle.ClientId).Any();
            if (exist)
            {
                return new SingleMessage<bool>(false, "VehicleSnDuplicate");
            }
            exist = context.BSC_VEHICLE.Where(x => x.ENGINE_ID == vehicle.EngineId && x.VALID == 1 && x.CLIENT_ID == vehicle.ClientId).Any();
            if (exist)
            {
                return new SingleMessage<bool>(false, "EngineIDDuplicate");
            }
            BSC_VEHICLE item = new BSC_VEHICLE();
            item = BscVehicleUtility.UpdateEntity(item, vehicle, true);
            context.BSC_VEHICLE.Add(item);
            if (context.SaveChanges() > 0)
            {
                return new SingleMessage<bool>(true);
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }


        public static SingleMessage<bool> UpdateVehicle(PTMSEntities context, Vehicle vehicle)
        {

            try
            {
                //bool reference = context.MTN_INSTALLATION_DETAIL.Any(n => n.VEHICLE_ID == vehicle.VehicleId);
                //if (reference)
                //{
                //    return new SingleMessage<bool>(false, "InstallWithSuite");
                //}
                //else
                //{
                //    reference = context.MTN_GPS_INSTALLATION_DETAIL.Any(n => n.VEHICLE_ID == vehicle.VehicleId);
                //    if (reference)
                //    {
                //        return new SingleMessage<bool>(false, "InstallWithGPS");
                //    }
                //    else
                //    {
                //        reference = context.BSC_VEHICLE_CHAUFFEUR.Any(n => n.VEHICLE_ID == vehicle.VehicleId);
                //        if (reference)
                //        {
                //            return new SingleMessage<bool>(false, "BindingWithCHAUFFEUR");
                //        }
                //        else
                //        {
                //            reference = context.RUN_MOBILE_WORKING.Any(n => n.VEHICLE_ID == vehicle.VehicleId);
                //            if (reference)
                //            {
                //                return new SingleMessage<bool>(false, "InstallWithMobile");
                //            }
                //        }
                //    }
                //}
                bool exist = context.BSC_VEHICLE.Where(x => x.VEHICLE_SN == vehicle.VehicleSn && x.VALID == 1 && x.VEHICLE_ID != vehicle.VehicleId && x.CLIENT_ID == vehicle.ClientId).Any();
                if (exist)
                {
                    return new SingleMessage<bool>(false, "VehicleSnDuplicate");
                }
                exist = context.BSC_VEHICLE.Where(x => x.ENGINE_ID == vehicle.EngineId && x.VALID == 1 && x.VEHICLE_ID != vehicle.VehicleId && x.CLIENT_ID == vehicle.ClientId).Any();
                if (exist)
                {
                    return new SingleMessage<bool>(false, "EngineIDDuplicate");
                }

                BSC_VEHICLE item = new BSC_VEHICLE();
                item = BscVehicleUtility.UpdateEntity(item, vehicle, true);
                context.BSC_VEHICLE.Attach(item);
                context.Entry(item).State = EntityState.Modified;
                if (context.SaveChanges() > 0)
                {
                    return new SingleMessage<bool>(true);
                }
                else
                {
                    return new SingleMessage<bool>(false, "");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateVehicleStatusByVehicleId(string vehicleId, int status, string note)
        {
            if (status < 0 || status > 1)
            {
                return false;
            }

            using (PTMSEntities context = new PTMSEntities())
            {
                var result = (from x in context.BSC_VEHICLE
                              where x.VEHICLE_ID == vehicleId && x.VALID == 1
                              select x).FirstOrDefault();
                if (result != null)
                {
                    result.VEHICLE_STATUS = (short)status;
                    result.NOTE = note;
                    context.Entry(result).State = EntityState.Modified;
                    int i = context.SaveChanges();
                    if (i > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public Vehicle GetInstalSecuritVehicleByMDVRID(string MDVR_ID)
        {
            //using (PTMSEntities _context = new PTMSEntities())
            //{
            //    var result = (from x in _context.SECURITY_SUITE_WORKING
            //                  join a in _context.VEHICLE on x.VEHICLE_ID.Trim() equals a.VEHICLE_ID
            //                  join y in _context.DISTRICT on a.DISTRICT_CODE equals y.CODE
            //                  join z in _context.DISTRICT on y.CODE.Substring(0, 2) equals z.CODE

            //                  join s in _context.DEV_SUITE on x.MDVR_CORE_SN.Trim() equals s.MDVR_CORE_SN.Trim() into ss
            //                  from s in ss.DefaultIfEmpty()
            //                  where a.VALID == 1 && (x.MDVR_CORE_SN.Trim() == MDVR_ID)
            //                  select new Vehicle
            //                  {
            //                      ServerType = a.SERVICE_TYPE == null ? VehicleSeviceType.Comercial : (VehicleSeviceType)a.SERVICE_TYPE,
            //                      Type = a.VEHICLE_TYPE == null ? VehicleType.Bus : (VehicleType)a.VEHICLE_TYPE,
            //                      Region = a.REGION,

            //                      OperatingLicense = a.OPERATION_LICENSE,
            //                      Owner = a.OWNER,
            //                      OwnerId = a.OWNER_ID,
            //                      OwnerPhone = a.OWNER_PHONE,
            //                      OwnerAddress = a.OWNER_ADDRESS,
            //                      BrandModel = a.BRAND_MODEL,
            //                      VehicleId = a.VEHICLE_ID,
            //                      VehicleSn = a.VEHICLE_SN,
            //                      EngineId = a.ENGINE_ID,
            //                      StartYear = a.START_YEAR,
            //                      Status = a.VEHICLE_STATUS == null ? VehicleConditionType.Available : (VehicleConditionType)a.VEHICLE_STATUS,
            //                      CityCode = y.CODE,
            //                      CityName = y.NAME,
            //                      ProvinceCode = z.CODE,
            //                      ProvinceName = z.NAME,
            //                      Note = a.NOTE,

            //                      MDVR_SN = s.MDVR_CORE_SN,
            //                      IsOnLine = x.ONLINE_FLAG,
            //                  }).FirstOrDefault();
            //    return result;
            //}
            return null;
        }

        public VehicleCheckResultExt CheckInstallVehicleForSuite(string vehicleId, string clientID)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                var vehicle = context.BSC_VEHICLE.Where(x => x.VEHICLE_ID == vehicleId && x.VALID == 1 && x.CLIENT_ID == clientID).FirstOrDefault();
                if (vehicle != null)
                {
                    var vehicletype = (from vt in context.BSC_VEHICLE_TYPE where vt.ID == vehicle.VEHICLE_TYPE select vt).FirstOrDefault();
                    var org = (from o in context.USR_ORGANIZATION where o.ID == vehicle.ORGNIZATION_ID select o).FirstOrDefault();
                    var working = (from x in context.BSC_VEHICLE
                                   join y in context.RUN_SUITE_WORKING on x.VEHICLE_ID equals y.VEHICLE_ID
                                   where x.VEHICLE_ID == vehicleId && x.VALID == 1
                                   select x).FirstOrDefault();

                    if (working == null)
                    {
                        var detail = context.MTN_INSTALLATION_DETAIL.Where(x => x.VEHICLE_ID == vehicleId && x.VALID == 1 && x.CHECKSTEP < 7).FirstOrDefault();
                        if (detail == null)
                        {
                            return new VehicleCheckResultExt() { Result = 1, InvalidCode = 1, Type = vehicletype == null ? "" : vehicletype.NAME, ContactPhone = vehicle.CONTACT_PHONE, EngineId = vehicle.ENGINE_ID, OperationLicense = vehicle.OPERATION_LICENSE, Owner = vehicle.OWNER, VehicleSn = vehicle.VEHICLE_SN, OrgnizationName = org.NAME, OrganizationID = vehicle.ORGNIZATION_ID };
                        }
                    }
                    return new VehicleCheckResultExt() { Result = 1, InvalidCode = 2, Type = vehicletype == null ? "" : vehicletype.NAME, ContactPhone = vehicle.CONTACT_PHONE, EngineId = vehicle.ENGINE_ID, OperationLicense = vehicle.OPERATION_LICENSE, Owner = vehicle.OWNER, VehicleSn = vehicle.VEHICLE_SN, OrgnizationName = org.NAME, OrganizationID = vehicle.ORGNIZATION_ID };
                }
                else
                {
                    return new VehicleCheckResultExt() { Result = 0, Type = string.Empty };
                }
            }
        }

        public VehicleCheckResultExt CheckInstallVehicleForGPS(string vehicleId, string clientID)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                var vehicle = context.BSC_VEHICLE.Where(x => x.VEHICLE_ID == vehicleId && x.VALID == 1 && x.CLIENT_ID == clientID).FirstOrDefault();
                if (vehicle != null)
                {
                    var vehicletype = (from vt in context.BSC_VEHICLE_TYPE where vt.ID == vehicle.VEHICLE_TYPE select vt).FirstOrDefault();
                    var org = (from o in context.USR_ORGANIZATION where o.ID == vehicle.ORGNIZATION_ID select o).FirstOrDefault();
                    var working = (from x in context.BSC_VEHICLE
                                   join y in context.RUN_GPS_WORKING on x.VEHICLE_ID equals y.VEHICLE_ID
                                   where x.VEHICLE_ID == vehicleId && x.VALID == 1
                                   select x).FirstOrDefault();

                    if (working == null)
                    {
                        var detail = context.MTN_GPS_INSTALLATION_DETAIL.Where(x => x.VEHICLE_ID == vehicleId && x.VALID == 1 && x.CHECKSTEP < 7).FirstOrDefault();
                        if (detail == null)
                        {
                            return new VehicleCheckResultExt() { Result = 1, InvalidCode = 1, Type = vehicletype == null ? "" : vehicletype.NAME, ContactPhone = vehicle.CONTACT_PHONE, EngineId = vehicle.ENGINE_ID, OperationLicense = vehicle.OPERATION_LICENSE, Owner = vehicle.OWNER, VehicleSn = vehicle.VEHICLE_SN, OrgnizationName = org.NAME, OrganizationID = vehicle.ORGNIZATION_ID };
                        }
                    }
                    return new VehicleCheckResultExt() { Result = 1, InvalidCode = 2, Type = vehicletype == null ? "" : vehicletype.NAME, ContactPhone = vehicle.CONTACT_PHONE, EngineId = vehicle.ENGINE_ID, OperationLicense = vehicle.OPERATION_LICENSE, Owner = vehicle.OWNER, VehicleSn = vehicle.VEHICLE_SN, OrgnizationName = org.NAME, OrganizationID = vehicle.ORGNIZATION_ID };
                }
                else
                {
                    return new VehicleCheckResultExt() { Result = 0, Type = string.Empty };
                }
            }
        }

        /// <summary>
        /// 获取车辆信息
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <param name="districtCode"></param>
        /// <param name="type"></param>
        /// <param name="owner"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="totalRecord"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        //public List<Vehicle> GetVehiclesFuzzy(string vehicleId, string districtCode, VehicleTypeEnum? type, string owner, InstallStatusType? status, PagingInfo page, out int totalRecord, UserInfoMessageHeader header)
        //{
        //using (PTMSEntities _context = new PTMSEntities())
        //{
        //    var result = (from vehicle in _context.VEHICLE_VIEW
        //                  //join detail in _context.INSTALLATION_DETAIL.Where(item => item.VALID == 1) on vehicle.VEHICLE_ID equals detail.VEHICLE_ID
        //                  //join setupStation in _context.SETUP_STATION.Where(item => item.VALID == 1) on detail.STATION_ID equals setupStation.ID
        //                  where (string.IsNullOrEmpty(vehicleId) ? true : vehicle.VEHICLE_ID.ToLower().Contains(vehicleId.ToLower())) &&
        //                      //(!string.IsNullOrEmpty(vehicle.STATION_ID))&&
        //                        (string.IsNullOrEmpty(owner) ? true : vehicle.OWNER.Contains(owner)) &&
        //                      //  ((type != null && type == VehicleType.Taxi) ? (vehicle.VEHICLE_TYPE == (short)VehicleType.Taxi) : true) &&
        //                        ((type != null && type == VehicleTypeEnum.Bus) ? (vehicle.VEHICLE_TYPE == VehicleTypeEnum.Bus.ToString()) : true) &&
        //                      //((type != null && type == VehicleType.Flota) ? (vehicle.VEHICLE_TYPE == (short)VehicleType.Flota) : true) &&
        //                        ((status != null && status == InstallStatusType.Installed) ? (vehicle.CHECKSTEP == 7) && !string.IsNullOrEmpty(vehicle.STATION_ID) : true) &&
        //                        ((status != null && status == InstallStatusType.Installing) ? (vehicle.CHECKSTEP > 0 && vehicle.CHECKSTEP < 7) && !string.IsNullOrEmpty(vehicle.STATION_ID) : true) &&
        //                        ((status != null && status == InstallStatusType.UnInstall) ? (vehicle.CHECKSTEP == 0 || vehicle.CHECKSTEP == null) : true)
        //                  select new Vehicle
        //                  {
        //                      VehicleId = vehicle.VEHICLE_ID,
        //                      VehicleSn = vehicle.VEHICLE_SN,
        //                      BrandModel = vehicle.BRAND_MODEL,
        //                      EngineId = vehicle.ENGINE_ID,
        //                      Note = vehicle.NOTE,
        //                      OperationLicense = vehicle.OPERATION_LICENSE,
        //                      Owner = vehicle.OWNER,
        //                      ContactAddress = vehicle.OWNER_ADDRESS,
        //                      Creator = vehicle.OWNER_ID,
        //                      ContactPhone = vehicle.OWNER_PHONE,
        //                      ContactEmail = vehicle.OWNER_EMAIL,
        //                      StartYear = vehicle.START_YEAR,
        //                      //Type = vehicle.VEHICLE_TYPE == null ? VehicleTypeEnum.Bus: vehicle.VEHICLE_TYPE,
        //                      ServiceType = vehicle.SERVICE_TYPE == null ? VehicleSeviceType.Comercial : (VehicleSeviceType)vehicle.SERVICE_TYPE,
        //                      VehicleStatus = vehicle.VEHICLE_STATUS == null ? VehicleConditionType.Available : (VehicleConditionType)vehicle.VEHICLE_STATUS,
        //                      CityCode = vehicle.DISTRICT_CODE,
        //                      CityName = vehicle.CITY,
        //                      ProvinceCode = vehicle.CODE.Substring(0, 2),
        //                      ProvinceName = vehicle.PROVINCE,
        //                      Region = vehicle.REGION,
        //                      DeleteFlag = vehicle.DELETEFLAG == null ? true : false,
        //                      UpdateFlag = true,
        //                      CheckStep = vehicle.CHECKSTEP,
        //                  });
        //    if (page == null || page.PageIndex == -1)
        //    {
        //        totalRecord = result.Count();
        //        List<Vehicle> Result = result.ToList();
        //        Result.ForEach(x =>
        //         {
        //             if (x.CheckStep == 7)
        //             {
        //                 x.InstallStatus = InstallStatusType.Installed;
        //                 x.UpdateFlag = false;
        //             }
        //             else if (x.CheckStep > 0)
        //             {
        //                 x.InstallStatus = InstallStatusType.Installing;
        //                 x.UpdateFlag = false;
        //             }
        //             else
        //             {
        //                 x.InstallStatus = InstallStatusType.UnInstall;
        //             }
        //         });
        //        return Result;
        //    }
        //    else
        //    {
        //        totalRecord = result.Count();
        //        List<Vehicle> Result = result.OrderBy(x => x.VehicleId).Skip(page.PageSize * (page.PageIndex - 1)).Take(page.PageSize).ToList();
        //        Result.ForEach(x =>
        //        {
        //            if (x.CheckStep == 7)
        //            {
        //                x.InstallStatus = InstallStatusType.Installed;
        //                x.UpdateFlag = false;
        //            }
        //            else if (x.CheckStep > 0)
        //            {
        //                x.InstallStatus = InstallStatusType.Installing;
        //                x.UpdateFlag = false;
        //            }
        //            else
        //            {
        //                x.InstallStatus = InstallStatusType.UnInstall;
        //            }
        //        });
        //        return Result;
        //    }
        //}
        //    totalRecord = 0;
        //    return null;
        //}

        public List<Vehicle> GetVehiclesFuzzyEx(string vehicleId, string districtCode, InstallStatusType? installStatus, PagingInfo page, out int totalRecord, UserInfoMessageHeader header)
        {
            //using (PTMSEntities _context = new PTMSEntities())
            //{
            //    Expression<Func<DISTRICT_LEVEL_VIEW, bool>> Exp = base.GetDistrictExpression(header);
            //    var result = (from vehicle in _context.VEHICLE_VIEW
            //                  join district in _context.DISTRICT_LEVEL_VIEW.Where(Exp) on vehicle.DISTRICT_CODE equals district.CODE
            //                  where (string.IsNullOrEmpty(vehicleId) ? true : vehicle.VEHICLE_ID.ToLower().Contains(vehicleId.ToLower())) &&
            //                        (string.IsNullOrEmpty(districtCode) ? true : district.CODE.StartsWith(districtCode)) &&
            //                        ((installStatus != null && installStatus == InstallStatusType.Installed) ? (vehicle.CHECKSTEP == 7) : true) &&
            //                        ((installStatus != null && installStatus == InstallStatusType.Installing) ? (vehicle.CHECKSTEP > 0 && vehicle.CHECKSTEP < 7) : true) &&
            //                        ((installStatus != null && installStatus == InstallStatusType.UnInstall) ? (vehicle.CHECKSTEP == 0 || vehicle.CHECKSTEP == null) : true)
            //                  select new Vehicle
            //                  {
            //                      VehicleId = vehicle.VEHICLE_ID,
            //                      VehicleSn = vehicle.VEHICLE_SN,
            //                      BrandModel = vehicle.BRAND_MODEL,
            //                      EngineId = vehicle.ENGINE_ID,
            //                      Note = vehicle.NOTE,
            //                      OperatingLicense = vehicle.OPERATION_LICENSE,
            //                      Owner = vehicle.OWNER,
            //                      OwnerAddress = vehicle.OWNER_ADDRESS,
            //                      OwnerId = vehicle.OWNER_ID,
            //                      OwnerPhone = vehicle.OWNER_PHONE,
            //                      OwnerEmail = vehicle.OWNER_EMAIL,
            //                      StartYear = vehicle.START_YEAR,
            //                      Type = vehicle.VEHICLE_TYPE == null ? VehicleType.Bus : (VehicleType)vehicle.VEHICLE_TYPE,
            //                      ServerType = vehicle.SERVICE_TYPE == null ? VehicleSeviceType.Comercial : (VehicleSeviceType)vehicle.SERVICE_TYPE,
            //                      Status = vehicle.VEHICLE_STATUS == null ? VehicleConditionType.Available : (VehicleConditionType)vehicle.VEHICLE_STATUS,


            //                      CityCode = vehicle.DISTRICT_CODE,
            //                      CityName = district.CITY,
            //                      ProvinceCode = district.CODE.Substring(0, 2),
            //                      ProvinceName = district.PROVINCE,
            //                      Region = vehicle.REGION,
            //                      DeleteFlag = vehicle.DELETEFLAG == null ? true : false,
            //                      CheckStep = vehicle.CHECKSTEP,
            //                  });
            //    if (page == null || page.PageIndex == -1)
            //    {
            //        totalRecord = result.Count();
            //        return result.ToList();
            //    }
            //    else
            //    {
            //        //totalRecord = result.Count();
            //        //return result.OrderBy(x => x.VehicleId).Skip(page.PageSize * (page.PageIndex - 1)).Take(page.PageSize).ToList();
            //        totalRecord = result.Count();
            //        List<Vehicle> Result = result.OrderBy(x => x.VehicleId).Skip(page.PageSize * (page.PageIndex - 1)).Take(page.PageSize).ToList();
            //        Result.ForEach(x =>
            //        {
            //            if (x.CheckStep == 7)
            //            {
            //                x.InstallStatus = InstallStatusType.Installed;
            //            }
            //            else if (x.CheckStep > 0)
            //            {
            //                x.InstallStatus = InstallStatusType.Installing;
            //            }
            //            else
            //            {
            //                x.InstallStatus = InstallStatusType.UnInstall;
            //            }
            //        });
            //        return Result;
            //    }
            //}
            totalRecord = 0;
            return null;
        }

        public bool CheckVehicleExistByVehicleId(string vehicleId)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                var result = (from x in context.BSC_VEHICLE
                              where x.VEHICLE_ID == vehicleId && x.VALID == 1
                              select x).ToList();
                if (result.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool CheckVehicleExistByVehicleSn(string vehicleSn)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                var result = (from x in context.BSC_VEHICLE
                              where x.VEHICLE_SN == vehicleSn && x.VALID == 1
                              select x).ToList();
                if (result.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public MultiMessage<Vehicle> CheckVehicleExist(List<Vehicle> vehicleList)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                //Batch Check
                Func<Vehicle, bool> filter = tv => context.BSC_VEHICLE.Local.Any(v => v.VALID == 1 && (v.VEHICLE_SN == tv.VehicleSn || v.VEHICLE_ID == tv.VehicleId || v.ENGINE_ID == tv.EngineId))
                     || context.BSC_VEHICLE.Any(v => v.VALID == 1 && (v.VEHICLE_SN == tv.VehicleSn || v.VEHICLE_ID == tv.VehicleId || v.ENGINE_ID == tv.EngineId));

                var list = vehicleList.Where(filter).ToList();
                return new MultiMessage<Vehicle>(list, list.Count);
            }
        }

        public bool BatchAdd(List<Vehicle> vehicleList)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                var idList = vehicleList.Select(t => t.VehicleId).ToList();
                var snList = vehicleList.Select(t => t.VehicleSn).ToList();
                var result = context.BSC_VEHICLE.Where(x => x.VALID == 0 && (idList.Contains(x.VEHICLE_ID) || snList.Contains(x.VEHICLE_SN))).ToList();
                foreach (var delVehicle in result)
                {
                    context.BSC_VEHICLE.Remove(delVehicle);
                }

                for (int i = 0; i < vehicleList.Count; i++)
                {
                    var item = vehicleList[i];

                    //Batch Check
                    if (context.BSC_VEHICLE.Local.Any(x => x.VEHICLE_ID == item.VehicleId && x.VALID == 1))
                    {
                        //errorIndex = i;
                        //break;
                        continue;
                    }
                    if (context.BSC_VEHICLE.Any(x => x.VEHICLE_ID == item.VehicleId && x.VALID == 1))
                    {
                        //errorIndex = i;
                        //break;
                        continue;
                    }

                    //Batch Delete 
                    var result1 = context.BSC_VEHICLE.Where(x => x.VEHICLE_ID == item.VehicleId && x.VALID == 0).FirstOrDefault();
                    if (result1 != null)
                    {
                        context.BSC_VEHICLE.Remove(result1);
                    }

                    context.BSC_VEHICLE.Add(new BSC_VEHICLE
                    {
                        VEHICLE_ID = item.VehicleId,
                        SERVICE_TYPE = (short)item.ServiceType,
                        VEHICLE_SN = item.VehicleSn,
                        DISTRICT_CODE = item.CityCode,
                        BRAND_MODEL = item.BrandModel,
                        VEHICLE_TYPE = item.VehicleType.ID,
                        VEHICLE_STATUS = (short)VehicleConditionType.Available,
                        REGION = item.Region,
                        OPERATION_LICENSE = item.OperationLicense,
                        ENGINE_ID = item.EngineId,
                        OWNER = item.Owner,
                        CONTACT = item.Contact,
                        CONTACT_PHONE = item.ContactPhone,
                        CONTACT_EMAIL = item.ContactEmail,
                        CONTACT_ADDRESS = item.ContactAddress,
                        START_YEAR = item.StartYear,
                        NOTE = item.Note,
                        CLIENT_ID = item.ClientId,
                        ORGNIZATION_ID = item.OrgnizationId,
                        CREATOR = item.Creator,
                        CREATE_TIME = DateTime.UtcNow,
                        VALID = 1,
                    });
                }
                context.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// find the vehicle which pass ruledata（had already install mdvr）
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="nullable"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public List<Vehicle> GetInstalSecuritVehiclesByAuthority(string province, string city, string userType, string regions, int level, int i = 0)
        {
            using (PTMSEntities _context = new PTMSEntities())
            {
                ConditonMaker<DISTRICT_LEVEL_VIEW> cdmDistrict = new ConditonMaker<DISTRICT_LEVEL_VIEW>();
                ConditonMaker<BSC_VEHICLE> cdmCompany = new ConditonMaker<BSC_VEHICLE>();
                List<CondtionItem> conditons = new List<CondtionItem>();
                List<CondtionItem> conditonsCompany = new List<CondtionItem>();
                if (!string.IsNullOrEmpty(regions))
                {
                    if (regions.Trim().Equals("*"))
                    {
                        if (level == 1)
                        {
                            conditons.Add(new StringPropertyConditon("CODE", Gsafety.PTMS.BaseInfo.MakerContions.Enums.OperateEnum.StartsWith, province));
                        }
                        else if (level == 2)
                        {
                            conditons.Add(new StringPropertyConditon("CODE", Gsafety.PTMS.BaseInfo.MakerContions.Enums.OperateEnum.StartsWith, city));
                        }
                        else
                        {
                            conditons.Add(new ConstantPropertyConditon(true));
                        }
                    }
                    else
                    {
                        string[] separator = { ", " };
                        string[] regionList = regions.Trim().Split(separator, StringSplitOptions.None);

                        foreach (string regionItem in regionList)
                        {
                            if (level != -1)
                            {
                                conditons.Add(new StringPropertyConditon("CODE", Gsafety.PTMS.BaseInfo.MakerContions.Enums.OperateEnum.StartsWith, regionItem.Trim()));
                            }
                            else
                            {
                                conditonsCompany.Add(new StringPropertyConditon("COMPANY_ID", Gsafety.PTMS.BaseInfo.MakerContions.Enums.OperateEnum.equal, regionItem.Trim()));
                            }
                        }

                    }
                }
                else
                {
                    conditons.Add(new ConstantPropertyConditon(false));
                }
                if (level == -1)
                {
                    conditons.Add(new ConstantPropertyConditon(true));
                }
                else
                {
                    conditonsCompany.Add(new ConstantPropertyConditon(true));
                }
                BasePropertyConditonGroup conditonGroup = new BasePropertyConditonGroup(Gsafety.PTMS.BaseInfo.Conditions.MakerContions.Enums.LogicSymbol.OR, conditons);
                BasePropertyConditonGroup conditonGroupCom = new BasePropertyConditonGroup(Gsafety.PTMS.BaseInfo.Conditions.MakerContions.Enums.LogicSymbol.OR, conditonsCompany);
                var result = (from x in _context.RUN_SUITE_WORKING.OrderBy(p => p.MDVR_CORE_SN).Skip(i * 1000).Take(1000)
                              join a in _context.BSC_VEHICLE.Where(cdmCompany.MakeCondtions(conditonGroupCom)) on x.VEHICLE_ID equals a.VEHICLE_ID
                              join z in _context.DISTRICT_LEVEL_VIEW.Where(cdmDistrict.MakeCondtions(conditonGroup)) on a.DISTRICT_CODE equals z.CODE
                              join s in _context.BSC_DEV_SUITE on x.SUITE_INFO_ID equals s.SUITE_INFO_ID //into ss from s in ss.DefaultIfEmpty()
                              where a.VALID == 1 && (x.STATUS == 24 || x.STATUS == 23)
                              select new Vehicle
                              {
                                  ServiceType = a.SERVICE_TYPE == null ? VehicleSeviceType.Comercial : (VehicleSeviceType)a.SERVICE_TYPE,
                                  VehicleType = new VehicleType() { ID = a.VEHICLE_TYPE == null ? "" : a.VEHICLE_TYPE },
                                  Region = a.REGION,
                                  OperationLicense = a.OPERATION_LICENSE,
                                  Owner = a.OWNER,
                                  Contact = a.OWNER,
                                  ContactPhone = a.CONTACT_PHONE,
                                  ContactAddress = a.CONTACT_ADDRESS,
                                  BrandModel = a.BRAND_MODEL,
                                  VehicleId = a.VEHICLE_ID,
                                  VehicleSn = a.VEHICLE_SN,
                                  EngineId = a.ENGINE_ID,
                                  StartYear = a.START_YEAR,
                                  VehicleStatus = a.VEHICLE_STATUS == null ? VehicleConditionType.Available : (VehicleConditionType)a.VEHICLE_STATUS,
                                  CityCode = z.CODE,
                                  CityName = z.CITY,
                                  ProvinceCode = z.CODE,
                                  ProvinceName = z.PROVINCE,
                                  Note = a.NOTE,
                                  MDVR_SN = s.MDVR_CORE_SN,
                                  //IsOnLine = x.ONLINE_FLAG,
                              });
                return result.ToList();
            }
        }

        /// <summary>
        /// 查询车辆类型
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<BSC_VEHICLE_TYPE> GetVehicleType(string userId)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                var id = from a in context.USR_ORGANIZATION_USER
                         where a.USER_ID == userId
                         select a.ID;
                bool f = id.Any();

                var items = from ou in context.USR_ORGANIZATION_USER
                            join o in context.USR_ORGANIZATION on ou.ORGANIZATION_ID equals o.ID
                            join vt in context.BSC_VEHICLE_TYPE on o.CLIENT_ID equals vt.CLIENT_ID
                            where ou.USER_ID == userId
                            select vt;

                return items.ToList();
            }
        }


        #region zhangxw 160708

        /// <summary>
        /// 获取车辆
        /// </summary>
        public static SingleMessage<Vehicle> GetBscVehicle(PTMSEntities context, string VehicleId)
        {
            BSC_VEHICLE entity = context.BSC_VEHICLE.SingleOrDefault(n => n.VEHICLE_ID == VehicleId);
            if (entity != null)
            {
                Vehicle model = BscVehicleUtility.GetModel(entity);

                return new SingleMessage<Vehicle>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取车辆列表
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static MultiMessage<Vehicle> GetBscVehicleList(PTMSEntities context, int pageIndex, int pageSize, string SearchVehicleId, string SearchOwner, string SearchVehicleType, string orgId, string vehicletypeid)
        {
            try
            {
                int totalCount;
                Expression<Func<BSC_VEHICLE, bool>> filter = f => f.ORGNIZATION_ID == orgId && f.VALID == 1;
                if (!string.IsNullOrEmpty(SearchVehicleId))
                {
                    filter = filter.And(f => f.VEHICLE_ID.Contains(SearchVehicleId));
                }
                if (!string.IsNullOrEmpty(SearchOwner))
                {
                    filter = filter.And(f => f.OWNER.Contains(SearchOwner));
                }
                if (!string.IsNullOrEmpty(vehicletypeid))
                {
                    filter = filter.And(v => v.VEHICLE_TYPE == vehicletypeid);
                }
                var source = context.BSC_VEHICLE.Where(filter);
                var list = source.Page(out totalCount, pageIndex, pageSize, true, n => n.CREATE_TIME, false).ToList();
                var items = from c in list
                            join t in context.BSC_VEHICLE_TYPE on c.VEHICLE_TYPE equals t.ID
                            select new Vehicle()
                            {
                                VehicleId = c.VEHICLE_ID,
                                ClientId = c.CLIENT_ID,
                                OrgnizationId = c.ORGNIZATION_ID,
                                VehicleSn = c.VEHICLE_SN,
                                EngineId = c.ENGINE_ID,
                                BrandModel = c.BRAND_MODEL,
                                DistrictCode = c.DISTRICT_CODE,
                                OperationLicense = c.OPERATION_LICENSE,
                                VehicleStatus = (VehicleConditionType)c.VEHICLE_STATUS,
                                Owner = c.OWNER,
                                Contact = c.CONTACT,
                                ContactAddress = c.CONTACT_ADDRESS,
                                ContactEmail = c.CONTACT_EMAIL,
                                ContactPhone = c.CONTACT_PHONE,
                                Region = c.REGION,
                                StartYear = c.START_YEAR,
                                ServiceType = (VehicleSeviceType)c.SERVICE_TYPE,
                                Note = c.NOTE,
                                Creator = c.CREATOR,
                                CreateTime = c.CREATE_TIME == null ? DateTime.UtcNow : (DateTime)c.CREATE_TIME,
                                VehicleType = new VehicleType() { ID = c.VEHICLE_TYPE },
                                VehicleTypeDescribe = t.NAME,
                                VehicleTypeImage = t.ICON,
                                Valid = c.VALID.Value,
                            };

                List<Vehicle> vehicles = items.ToList();
                List<string> vehicleids = vehicles.Select(n => n.VehicleId).ToList();

                List<MTN_INSTALLATION_DETAIL> suitedetails = context.MTN_INSTALLATION_DETAIL.Where(n => vehicleids.Contains(n.VEHICLE_ID)).ToList();
                foreach (var item in vehicles)
                {
                    if (suitedetails.Any(n => n.VEHICLE_ID == item.VehicleId))
                    {
                        item.IsBinding = true;
                        vehicleids.Remove(item.VehicleId);
                    }
                }
                List<MTN_GPS_INSTALLATION_DETAIL> gpsdetails = context.MTN_GPS_INSTALLATION_DETAIL.Where(n => vehicleids.Contains(n.VEHICLE_ID)).ToList();
                foreach (var item in vehicles)
                {
                    if (gpsdetails.Any(n => n.VEHICLE_ID == item.VehicleId))
                    {
                        item.IsBinding = true;
                        vehicleids.Remove(item.VehicleId);
                    }
                }
                List<BSC_VEHICLE_CHAUFFEUR> chaufferdetails = context.BSC_VEHICLE_CHAUFFEUR.Where(n => vehicleids.Contains(n.VEHICLE_ID)).ToList();
                foreach (var item in vehicles)
                {
                    if (chaufferdetails.Any(n => n.VEHICLE_ID == item.VehicleId))
                    {
                        item.IsBinding = true;
                        vehicleids.Remove(item.VehicleId);
                    }
                }
                return new MultiMessage<Vehicle>(vehicles, totalCount);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion


        /// <summary>
        /// 车辆类型
        /// </summary>
        /// <returns></returns>
        public static MultiMessage<VehicleType> GetVehicleTypeList(PTMSEntities context, string clientID)
        {
            int totalCount = 1;
            var list = context.BSC_VEHICLE_TYPE.ToList();
            var items = list.Select(s => VehicleTypeUtility.GetModel(s)).Where(v => v.Valid == 1 && v.ClientID == clientID);
            return new MultiMessage<VehicleType>(items.ToList(), totalCount);
        }

        public static List<Vehicle> GetChauffeurVehicle(PTMSEntities context, string clientID)
        {
            List<Vehicle> result = new List<Vehicle>();
            if (string.IsNullOrWhiteSpace(clientID))
            {
                return new List<Vehicle>();
            }

            var temp = from u in context.BSC_VEHICLE
                       where u.VALID == 1 && u.CLIENT_ID == clientID
                       select u;

            foreach (var item in temp)
            {
                result.Add(VehicleUtility.GetModel(item));
            }
            return result;
        }


        /// <summary>
        /// 获取驾驶员车辆
        /// </summary>
        /// <param name="context"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public static MultiMessage<VVehicle> GetChauffeurVehiclePageList(PTMSEntities context, PagingInfo page, string clientID, string vehicleNum)
        {
            MultiMessage<VVehicle> result = new MultiMessage<VVehicle>();
            if (string.IsNullOrWhiteSpace(clientID))
            {
                return new MultiMessage<VVehicle>();
            }

            var entitylist = new List<BSC_VEHICLE>();
            var temp = new List<BSC_VEHICLE>();
            if (vehicleNum != string.Empty)
            {
                temp = (from u in context.BSC_VEHICLE.Where(t => t.VEHICLE_ID.ToUpper().Contains(vehicleNum.ToUpper()))
                        where u.VALID == 1 && u.CLIENT_ID == clientID
                        select u).ToList();
            }
            else
            {
                temp = (from u in context.BSC_VEHICLE
                        where u.VALID == 1 && u.CLIENT_ID == clientID
                        select u).ToList();
            }

            if (page.PageIndex > 0)
            {
                result.TotalRecord = temp.Count();
                entitylist = temp.OrderByDescending(t => t.CREATE_TIME)
                    .Skip(page.PageSize * (page.PageIndex - 1))
                    .Take(page.PageSize)
                    .ToList();
            }
            else
            {
                result.TotalRecord = temp.Count();
                entitylist = temp.OrderBy(t => t.CREATE_TIME).ToList();
            }
            foreach (var item in entitylist)
            {
                result.Result.Add(VehicleUtility.GetVVehicleModel(item));
            }

            result.IsSuccess = true;
            return result;
        }

        public MultiMessage<Vehicle> GetInstallVehiclesByAuthority(PTMSEntities context, List<string> organizations)
        {
            //var vehicles = from org in context.USR_ORGANIZATION_USER.Where(o => o.USER_ID == userID)
            //               join veh in context.BSC_VEHICLE.Where(v => v.VALID == (short)ValidEnum.Valid) on org.ORGANIZATION_ID equals veh.ORGNIZATION_ID
            //               let mdvr = context.RUN_SUITE_WORKING.FirstOrDefault(mdvrWorking => mdvrWorking.VEHICLE_ID == veh.VEHICLE_ID)
            //               let gps = (from installVeh in context.RUN_GPS_WORKING.Where(gpsWorking => gpsWorking.VEHICLE_ID == veh.VEHICLE_ID)
            //                          select installVeh).FirstOrDefault()
            //               let mobile = (from installVeh in context.RUN_MOBILE_WORKING.Where(mobileWorking => mobileWorking.VEHICLE_ID == veh.VEHICLE_ID)
            //                             select installVeh).FirstOrDefault()
            //               where mdvr != null || gps != null || mobile != null
            //               select new Vehicle()
            //               {
            //                   OrgnizationId = org.ORGANIZATION_ID,
            //                   IsOnLine = working.ONLINE_FLAG,
            //                   VehicleId = veh.VEHICLE_ID,
            //                   InstallStatus = InstallStatusType.Installed,
            //                   ServiceType = veh.SERVICE_TYPE == null ? VehicleSeviceType.Comercial : (VehicleSeviceType)veh.SERVICE_TYPE,
            //                   VehicleStatus = veh.VEHICLE_STATUS == null ? VehicleConditionType.Available : (VehicleConditionType)veh.VEHICLE_STATUS,
            //               };


            //var vehicles = from org in context.USR_ORGANIZATION_USER.Where(o => o.USER_ID == userID)
            //               join veh in context.BSC_VEHICLE.Where(v => v.VALID == (short)ValidEnum.Valid) on org.ORGANIZATION_ID equals veh.ORGNIZATION_ID
            //               join installVeh in context.MTN_INSTALLATION_DETAIL.Where(install => install.CHECKSTEP == 3 && install.VALID == (short)ValidEnum.Valid) on veh.VEHICLE_ID equals installVeh.VEHICLE_ID
            //               join working in context.RUN_SUITE_WORKING on installVeh.SUITE_INFO_ID equals working.SUITE_INFO_ID

            //               select new Vehicle()
            //               {
            //                   OrgnizationId = org.ORGANIZATION_ID,
            //                   IsOnLine = working.ONLINE_FLAG,
            //                   VehicleId = veh.VEHICLE_ID,
            //                   InstallStatus = InstallStatusType.Installed,
            //                   ServiceType = veh.SERVICE_TYPE == null ? VehicleSeviceType.Comercial : (VehicleSeviceType)veh.SERVICE_TYPE,
            //                   VehicleStatus = veh.VEHICLE_STATUS == null ? VehicleConditionType.Available : (VehicleConditionType)veh.VEHICLE_STATUS,
            //               };

            List<Vehicle> vehicles = null;

            var mdvrvehicles = from veh in context.BSC_VEHICLE.Where(v => v.VALID == (short)ValidEnum.Valid)
                               join orgation in context.USR_ORGANIZATION on veh.ORGNIZATION_ID equals orgation.ID
                               join vt in context.BSC_VEHICLE_TYPE.Where(t => t.VALID == (short)ValidEnum.Valid) on veh.VEHICLE_TYPE equals vt.ID
                               join mdvr in context.RUN_SUITE_WORKING on veh.VEHICLE_ID equals mdvr.VEHICLE_ID
                               where ((mdvr != null && (mdvr.STATUS == (short)DeviceSuiteStatus.Running) || mdvr.STATUS == (short)DeviceSuiteStatus.Abnormal) && organizations.Contains(orgation.ID))
                               select new Vehicle()
                               {
                                   OrgnizationId = veh.ORGNIZATION_ID,
                                   OrgnizationName = orgation.NAME,
                                   VehicleId = veh.VEHICLE_ID,
                                   MDVROnline = mdvr.ONLINE_FLAG,
                                   ServiceType = veh.SERVICE_TYPE == null ? VehicleSeviceType.Comercial : (VehicleSeviceType)veh.SERVICE_TYPE,
                                   VehicleType = new VehicleType() { ID = veh.VEHICLE_TYPE == null ? string.Empty : veh.VEHICLE_TYPE },
                                   Region = veh.REGION,
                                   OperationLicense = veh.OPERATION_LICENSE,
                                   Owner = veh.OWNER,
                                   Contact = veh.OWNER,
                                   ContactPhone = veh.CONTACT_PHONE,
                                   ContactAddress = veh.CONTACT_ADDRESS,
                                   BrandModel = veh.BRAND_MODEL,
                                   VehicleSn = veh.VEHICLE_SN,
                                   EngineId = veh.ENGINE_ID,
                                   StartYear = veh.START_YEAR,
                                   VehicleStatus = veh.VEHICLE_STATUS == null ? VehicleConditionType.Available : (VehicleConditionType)veh.VEHICLE_STATUS,
                                   Note = veh.NOTE,
                                   VehicleTypeDescribe = vt.NAME,
                                   VehicleTypeImage = vt.ICON,
                                   MDVR_SN = mdvr.MDVR_CORE_SN,
                                   DistrictCode = veh.DISTRICT_CODE,
                               };

            vehicles = mdvrvehicles.ToList();

            var gpsvehicles = from veh in context.BSC_VEHICLE.Where(v => v.VALID == (short)ValidEnum.Valid)
                              join orgation in context.USR_ORGANIZATION on veh.ORGNIZATION_ID equals orgation.ID
                              join vt in context.BSC_VEHICLE_TYPE.Where(t => t.VALID == (short)ValidEnum.Valid) on veh.VEHICLE_TYPE equals vt.ID
                              join gps in context.RUN_GPS_WORKING on veh.VEHICLE_ID equals gps.VEHICLE_ID
                              where gps.STATUS == (short)DeviceSuiteStatus.Running && organizations.Contains(orgation.ID)
                              select new Vehicle()
                              {
                                  OrgnizationId = veh.ORGNIZATION_ID,
                                  OrgnizationName = orgation.NAME,
                                  VehicleId = veh.VEHICLE_ID,
                                  GPSOnline = gps.ONLINE_FLAG,
                                  ServiceType = veh.SERVICE_TYPE == null ? VehicleSeviceType.Comercial : (VehicleSeviceType)veh.SERVICE_TYPE,
                                  VehicleType = new VehicleType() { ID = veh.VEHICLE_TYPE == null ? string.Empty : veh.VEHICLE_TYPE },
                                  Region = veh.REGION,
                                  OperationLicense = veh.OPERATION_LICENSE,
                                  Owner = veh.OWNER,
                                  Contact = veh.OWNER,
                                  ContactPhone = veh.CONTACT_PHONE,
                                  ContactAddress = veh.CONTACT_ADDRESS,
                                  BrandModel = veh.BRAND_MODEL,
                                  VehicleSn = veh.VEHICLE_SN,
                                  EngineId = veh.ENGINE_ID,
                                  StartYear = veh.START_YEAR,
                                  VehicleStatus = veh.VEHICLE_STATUS == null ? VehicleConditionType.Available : (VehicleConditionType)veh.VEHICLE_STATUS,
                                  Note = veh.NOTE,
                                  VehicleTypeDescribe = vt.NAME,
                                  VehicleTypeImage = vt.ICON,
                                  GPS_SN = gps.GPS_SN,
                                  DistrictCode = veh.DISTRICT_CODE,
                              };


            foreach (var item in gpsvehicles)
            {
                var vehicle = vehicles.FirstOrDefault(n => n.VehicleId == item.VehicleId);
                if (vehicle == null)
                {
                    vehicles.Add(item);
                }
                else
                {
                    vehicle.GPSOnline = item.GPSOnline;
                    vehicle.GPS_SN = item.GPS_SN;
                }
            }

            var mobilevehicles = from veh in context.BSC_VEHICLE.Where(v => v.VALID == (short)ValidEnum.Valid)
                                 join orgation in context.USR_ORGANIZATION on veh.ORGNIZATION_ID equals orgation.ID
                                 join vt in context.BSC_VEHICLE_TYPE.Where(t => t.VALID == (short)ValidEnum.Valid) on veh.VEHICLE_TYPE equals vt.ID
                                 join mobile in context.RUN_MOBILE_WORKING on veh.VEHICLE_ID equals mobile.VEHICLE_ID
                                 where organizations.Contains(orgation.ID)
                                 select new Vehicle()
                                 {
                                     OrgnizationId = veh.ORGNIZATION_ID,
                                     OrgnizationName = orgation.NAME,
                                     VehicleId = veh.VEHICLE_ID,
                                     MobileOnline = mobile.ONLINE_FLAG,
                                     ServiceType = veh.SERVICE_TYPE == null ? VehicleSeviceType.Comercial : (VehicleSeviceType)veh.SERVICE_TYPE,
                                     VehicleType = new VehicleType() { ID = veh.VEHICLE_TYPE == null ? string.Empty : veh.VEHICLE_TYPE },
                                     Region = veh.REGION,
                                     OperationLicense = veh.OPERATION_LICENSE,
                                     Owner = veh.OWNER,
                                     Contact = veh.OWNER,
                                     ContactPhone = veh.CONTACT_PHONE,
                                     ContactAddress = veh.CONTACT_ADDRESS,
                                     BrandModel = veh.BRAND_MODEL,
                                     VehicleSn = veh.VEHICLE_SN,
                                     EngineId = veh.ENGINE_ID,
                                     StartYear = veh.START_YEAR,
                                     VehicleStatus = veh.VEHICLE_STATUS == null ? VehicleConditionType.Available : (VehicleConditionType)veh.VEHICLE_STATUS,
                                     Note = veh.NOTE,
                                     VehicleTypeDescribe = vt.NAME,
                                     VehicleTypeImage = vt.ICON,
                                     Mobile_SN = mobile.MOBILE_NUMBER,
                                     DistrictCode = veh.DISTRICT_CODE,
                                 };

            foreach (var item in mobilevehicles)
            {
                var vehicle = vehicles.FirstOrDefault(n => n.VehicleId == item.VehicleId);
                if (vehicle == null)
                {
                    //vehicles.Add(item);
                }
                else
                {
                    vehicle.MobileOnline = item.MobileOnline;
                    vehicle.Mobile_SN = item.Mobile_SN;
                }
            }

            return new MultiMessage<Vehicle>(vehicles, vehicles.Count);



            //BasePropertyConditonGroup conditonGroup = new BasePropertyConditonGroup(Gsafety.PTMS.BaseInfo.Conditions.MakerContions.Enums.LogicSymbol.OR, conditons);
            //BasePropertyConditonGroup conditonGroupCom = new BasePropertyConditonGroup(Gsafety.PTMS.BaseInfo.Conditions.MakerContions.Enums.LogicSymbol.OR, conditonsCompany);
            //var result = (from x in _context.RUN_SUITE_WORKING.OrderBy(p => p.MDVR_CORE_SN).Skip(i * 1000).Take(1000)
            //              join a in _context.BSC_VEHICLE.Where(cdmCompany.MakeCondtions(conditonGroupCom)) on x.VEHICLE_ID equals a.VEHICLE_ID
            //              join z in _context.DISTRICT_LEVEL_VIEW.Where(cdmDistrict.MakeCondtions(conditonGroup)) on a.DISTRICT_CODE equals z.CODE
            //              join s in _context.BSC_DEV_SUITE on x.SUITE_INFO_ID equals s.SUITE_INFO_ID //into ss from s in ss.DefaultIfEmpty()
            //              where a.VALID == 1 && (x.STATUS == 24 || x.STATUS == 23)
            //              select new Vehicle
            //              {
            //                  ServiceType = a.SERVICE_TYPE == null ? VehicleSeviceType.Comercial : (VehicleSeviceType)a.SERVICE_TYPE,
            //                  VehicleType = new VehicleType() { ID = a.VEHICLE_TYPE == null ? "" : a.VEHICLE_TYPE },
            //                  Region = a.REGION,
            //                  OperationLicense = a.OPERATION_LICENSE,
            //                  Owner = a.OWNER,
            //                  Contact = a.OWNER,
            //                  ContactPhone = a.CONTACT_PHONE,
            //                  ContactAddress = a.CONTACT_ADDRESS,
            //                  BrandModel = a.BRAND_MODEL,
            //                  VehicleId = a.VEHICLE_ID,
            //                  VehicleSn = a.VEHICLE_SN,
            //                  EngineId = a.ENGINE_ID,
            //                  StartYear = a.START_YEAR,
            //                  VehicleStatus = a.VEHICLE_STATUS == null ? VehicleConditionType.Available : (VehicleConditionType)a.VEHICLE_STATUS,
            //                  CityCode = z.CODE,
            //                  CityName = z.CITY,
            //                  ProvinceCode = z.CODE,
            //                  ProvinceName = z.PROVINCE,
            //                  Note = a.NOTE,
            //                  MDVR_SN = s.MDVR_CORE_SN,
            //                  IsOnLine = x.ONLINE_FLAG,
            //              });
            //return result.ToList();
        }

        public SingleMessage<Vehicle> GetInstallVehicle(PTMSEntities context, string organization, string vehicleid)
        {
            Vehicle vehicle = null;

            var mdvrvehicles = from veh in context.BSC_VEHICLE.Where(v => v.VALID == (short)ValidEnum.Valid && v.VEHICLE_ID == vehicleid)
                               join orgation in context.USR_ORGANIZATION on veh.ORGNIZATION_ID equals orgation.ID
                               join vt in context.BSC_VEHICLE_TYPE.Where(t => t.VALID == (short)ValidEnum.Valid) on veh.VEHICLE_TYPE equals vt.ID
                               join mdvr in context.RUN_SUITE_WORKING on veh.VEHICLE_ID equals mdvr.VEHICLE_ID
                               where ((mdvr != null && (mdvr.STATUS == (short)DeviceSuiteStatus.Running) || mdvr.STATUS == (short)DeviceSuiteStatus.Abnormal) && organization == orgation.ID)
                               select new Vehicle()
                               {
                                   OrgnizationId = veh.ORGNIZATION_ID,
                                   OrgnizationName = orgation.NAME,
                                   VehicleId = veh.VEHICLE_ID,
                                   MDVROnline = mdvr.ONLINE_FLAG,
                                   ServiceType = veh.SERVICE_TYPE == null ? VehicleSeviceType.Comercial : (VehicleSeviceType)veh.SERVICE_TYPE,
                                   VehicleType = new VehicleType() { ID = veh.VEHICLE_TYPE == null ? string.Empty : veh.VEHICLE_TYPE },
                                   Region = veh.REGION,
                                   OperationLicense = veh.OPERATION_LICENSE,
                                   Owner = veh.OWNER,
                                   Contact = veh.OWNER,
                                   ContactPhone = veh.CONTACT_PHONE,
                                   ContactAddress = veh.CONTACT_ADDRESS,
                                   BrandModel = veh.BRAND_MODEL,
                                   VehicleSn = veh.VEHICLE_SN,
                                   EngineId = veh.ENGINE_ID,
                                   StartYear = veh.START_YEAR,
                                   VehicleStatus = veh.VEHICLE_STATUS == null ? VehicleConditionType.Available : (VehicleConditionType)veh.VEHICLE_STATUS,
                                   Note = veh.NOTE,
                                   VehicleTypeDescribe = vt.NAME,
                                   VehicleTypeImage = vt.ICON,
                                   MDVR_SN = mdvr.MDVR_CORE_SN,
                                   DistrictCode = veh.DISTRICT_CODE,
                               };

            var mdvrvehicle = mdvrvehicles.FirstOrDefault();

            var gpsvehicles = from veh in context.BSC_VEHICLE.Where(v => v.VALID == (short)ValidEnum.Valid && v.VEHICLE_ID == vehicleid)
                              join orgation in context.USR_ORGANIZATION on veh.ORGNIZATION_ID equals orgation.ID
                              join vt in context.BSC_VEHICLE_TYPE.Where(t => t.VALID == (short)ValidEnum.Valid) on veh.VEHICLE_TYPE equals vt.ID
                              join gps in context.RUN_GPS_WORKING on veh.VEHICLE_ID equals gps.VEHICLE_ID
                              where gps.STATUS == (short)DeviceSuiteStatus.Running && organization == orgation.ID
                              select new Vehicle()
                              {
                                  OrgnizationId = veh.ORGNIZATION_ID,
                                  OrgnizationName = orgation.NAME,
                                  VehicleId = veh.VEHICLE_ID,
                                  GPSOnline = gps.ONLINE_FLAG,
                                  ServiceType = veh.SERVICE_TYPE == null ? VehicleSeviceType.Comercial : (VehicleSeviceType)veh.SERVICE_TYPE,
                                  VehicleType = new VehicleType() { ID = veh.VEHICLE_TYPE == null ? string.Empty : veh.VEHICLE_TYPE },
                                  Region = veh.REGION,
                                  OperationLicense = veh.OPERATION_LICENSE,
                                  Owner = veh.OWNER,
                                  Contact = veh.OWNER,
                                  ContactPhone = veh.CONTACT_PHONE,
                                  ContactAddress = veh.CONTACT_ADDRESS,
                                  BrandModel = veh.BRAND_MODEL,
                                  VehicleSn = veh.VEHICLE_SN,
                                  EngineId = veh.ENGINE_ID,
                                  StartYear = veh.START_YEAR,
                                  VehicleStatus = veh.VEHICLE_STATUS == null ? VehicleConditionType.Available : (VehicleConditionType)veh.VEHICLE_STATUS,
                                  Note = veh.NOTE,
                                  VehicleTypeDescribe = vt.NAME,
                                  VehicleTypeImage = vt.ICON,
                                  GPS_SN = gps.GPS_SN,
                                  DistrictCode = veh.DISTRICT_CODE,
                              };

            var gpsvehicle = gpsvehicles.FirstOrDefault();


            var mobilevehicles = from veh in context.BSC_VEHICLE.Where(v => v.VALID == (short)ValidEnum.Valid && v.VEHICLE_ID == vehicleid)
                                 join orgation in context.USR_ORGANIZATION on veh.ORGNIZATION_ID equals orgation.ID
                                 join vt in context.BSC_VEHICLE_TYPE.Where(t => t.VALID == (short)ValidEnum.Valid) on veh.VEHICLE_TYPE equals vt.ID
                                 join mobile in context.RUN_MOBILE_WORKING on veh.VEHICLE_ID equals mobile.VEHICLE_ID
                                 where organization == orgation.ID
                                 select new Vehicle()
                                 {
                                     OrgnizationId = veh.ORGNIZATION_ID,
                                     OrgnizationName = orgation.NAME,
                                     VehicleId = veh.VEHICLE_ID,
                                     MobileOnline = mobile.ONLINE_FLAG,
                                     ServiceType = veh.SERVICE_TYPE == null ? VehicleSeviceType.Comercial : (VehicleSeviceType)veh.SERVICE_TYPE,
                                     VehicleType = new VehicleType() { ID = veh.VEHICLE_TYPE == null ? string.Empty : veh.VEHICLE_TYPE },
                                     Region = veh.REGION,
                                     OperationLicense = veh.OPERATION_LICENSE,
                                     Owner = veh.OWNER,
                                     Contact = veh.OWNER,
                                     ContactPhone = veh.CONTACT_PHONE,
                                     ContactAddress = veh.CONTACT_ADDRESS,
                                     BrandModel = veh.BRAND_MODEL,
                                     VehicleSn = veh.VEHICLE_SN,
                                     EngineId = veh.ENGINE_ID,
                                     StartYear = veh.START_YEAR,
                                     VehicleStatus = veh.VEHICLE_STATUS == null ? VehicleConditionType.Available : (VehicleConditionType)veh.VEHICLE_STATUS,
                                     Note = veh.NOTE,
                                     VehicleTypeDescribe = vt.NAME,
                                     VehicleTypeImage = vt.ICON,
                                     Mobile_SN = mobile.MOBILE_NUMBER,
                                     DistrictCode = veh.DISTRICT_CODE,
                                 };

            var mobilevehicle = mobilevehicles.FirstOrDefault();

            if (mdvrvehicle != null)
            {
                vehicle = mdvrvehicle;
                if (gpsvehicle != null)
                {
                    vehicle.GPSOnline = gpsvehicle.GPSOnline;
                    vehicle.GPS_SN = gpsvehicle.GPS_SN;
                }

                if (mobilevehicle != null)
                {
                    vehicle.MobileOnline = mobilevehicle.MobileOnline;
                    vehicle.Mobile_SN = mobilevehicle.Mobile_SN;
                }
            }
            else if (gpsvehicle != null)
            {
                vehicle = gpsvehicle;

                if (mobilevehicle != null)
                {
                    vehicle.MobileOnline = mobilevehicle.MobileOnline;
                    vehicle.Mobile_SN = mobilevehicle.Mobile_SN;
                }
            }
            else if (mobilevehicle != null)
            {
                vehicle = mobilevehicle;
            }

            return new SingleMessage<Vehicle>(vehicle);
        }
    }
}
