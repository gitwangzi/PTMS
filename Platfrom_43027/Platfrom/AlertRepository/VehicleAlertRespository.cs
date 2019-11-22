using Gsafety.Common.Logging;
using Gsafety.PTMS.Alert.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b0e1aa0a-2c10-46c6-90e7-fd7c8b293d70      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Repository
/////    Project Description:    
/////             Class Name: VehicleAlertRespository
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/9 10:59:15
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/9 10:59:15
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Gsafety.PTMS.Alert.Repository
{
    public class VehicleAlertRespository : Gsafety.PTMS.BaseInfo.BaseRepository
    {

        public MultiMessage<VehicleAlertEx> GetSecurityVehicleAlertEx(PTMSEntities _context, string carNumber, DateTime? startTime, DateTime? endTime, int alertType, PagingInfo pagingInfo)
        {
            var source = from alert in _context.ALT_BUSINESS_ALERT
                         join vehcle in _context.BSC_VEHICLE on alert.VEHICLE_ID equals vehcle.VEHICLE_ID
                         join alerthandle in _context.ALT_BUSINESS_ALERT_HANDLE on alert.ID equals alerthandle.BUSINESS_ALERT_ID
                         join city in _context.BSC_DISTRICT on alert.DISTRICT_CODE equals city.CODE
                         join province in _context.BSC_DISTRICT on alert.DISTRICT_CODE.Substring(0, 2) equals province.CODE
                         where (alertType == 0 ? true : alert.ALERT_TYPE == alertType) &&
                         alert.STATUS == 4 && (alert.SUITE_STATUS == (short)DeviceSuiteStatus.Running || alert.SUITE_STATUS == (short)DeviceSuiteStatus.WaitingMaintenance || alert.SUITE_STATUS == (short)DeviceSuiteStatus.Abnormal) &&
                         (string.IsNullOrEmpty(carNumber) ? true : alert.VEHICLE_ID.ToLower().Contains(carNumber.ToLower())) &&
                         (startTime == null ? true : alert.ALERT_TIME > startTime) &&
                         (endTime == null ? true : alert.ALERT_TIME < endTime)
                         select new VehicleAlertEx
                         {
                             Id = alert.ID,
                             VehicleId = alert.VEHICLE_ID,
                             AlertType = alert.ALERT_TYPE,
                             AlertTime = alert.ALERT_TIME,
                             Longitude = alert.LONGITUDE,
                             Latitude = alert.LATITUDE,
                             Content = alerthandle.CONTENT,
                             DisposeStaff = alerthandle.HANDLE_USER,
                             DisposeTime = alerthandle.HANDLE_TIME,
                             AlertId = alerthandle.ID,
                             MdvrCoreId = alert.MDVR_CORE_SN,
                             Direction = alert.DIRECTION,
                             Status = alert.STATUS.Value,
                             GpsTime = alert.GPS_TIME,
                             GpsValid = alert.GPS_VALID,
                             BUSINESS_ALERT_ID = alerthandle.BUSINESS_ALERT_ID,
                             Speed = alert.SPEED,
                             City = city.NAME,
                             Province = province.NAME,
                         };


            var totalcount = source.Count();
            var resultlist = new List<VehicleAlertEx>();
            if (pagingInfo == null)
            {
                resultlist = source.OrderByDescending(c => c.AlertTime).ToList();
            }
            else
            {
                resultlist = source.OrderByDescending(c => c.AlertTime).Skip(pagingInfo.PageSize * (pagingInfo.PageIndex - 1)).Take(pagingInfo.PageSize).ToList();
            }

            var result = new MultiMessage<VehicleAlertEx>
            {
                Result = resultlist,
                TotalRecord = totalcount
            };
            return result;

        }

        /// <summary>
        /// Get detailed information on the vehicle alarm
        /// </summary>
        /// <param name="vehicleDoorAlertId"></param>
        /// <returns></returns>
        public SingleMessage<VehicleAlertDetail> GetVehicleAlertDetail(PTMSEntities _context, string vehicleDoorAlertId)
        {
            try
            {

                var result = new SingleMessage<VehicleAlertDetail>
                {

                    Result = (from item in _context.ALT_BUSINESS_ALERT
                              join vehicle in _context.BSC_VEHICLE on item.VEHICLE_ID equals vehicle.VEHICLE_ID
                              join city in _context.BSC_DISTRICT on item.DISTRICT_CODE equals city.CODE
                              join province in _context.BSC_DISTRICT on item.DISTRICT_CODE.Substring(0, 2) equals province.CODE
                              join suiteInfo in _context.BSC_DEV_SUITE on item.SUITE_INFO_ID equals suiteInfo.SUITE_INFO_ID
                              where item.ID == vehicleDoorAlertId
                              select new VehicleAlertDetail
                              {
                                  Id = item.ID,
                                  VehicleId = item.VEHICLE_ID,
                                  SuiteId = suiteInfo.SUITE_ID,
                                  AlertType = item.ALERT_TYPE,
                                  AlertTime = item.ALERT_TIME,
                                  Speed = item.SPEED,
                                  Longitude = item.LONGITUDE,
                                  Owner_Phone = vehicle.CONTACT_PHONE,
                                  VehicleOwner = vehicle.OWNER,
                                  GpsValid = item.GPS_VALID,
                                  Latitude = item.LATITUDE,
                                  Status = item.STATUS.Value,
                                  Direction = item.DIRECTION,
                                  MdvrCoreId = item.MDVR_CORE_SN,
                                  CityName = city.NAME,
                                  ProvinceName = province.NAME,
                              }).FirstOrDefault()
                };
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// Add alarm processing records
        /// </summary>
        /// <param name="alertTreatment"></param>
        /// <returns></returns>
        public SingleMessage<bool> AddVechileAlertTreatment(PTMSEntities content, VehicleAlertTreatment alertTreatment)
        {
            var scope = new TransactionScope((TransactionScopeOption.Required));

            try
            {
                var result = (from i in content.ALT_BUSINESS_ALERT
                              where i.MDVR_CORE_SN == alertTreatment.MDVRID
                               && i.ALERT_TIME == alertTreatment.AlertTime
                              select i).ToList();
                foreach (var item in result)
                {
                    if (item != null)
                    {
                        item.STATUS = 4;
                        ALT_BUSINESS_ALERT_HANDLE handle = new ALT_BUSINESS_ALERT_HANDLE();
                        handle.ID = Guid.NewGuid().ToString();
                        handle.HANDLE_USER = alertTreatment.DisposeStaff;
                        handle.HANDLE_TIME = DateTime.Now;
                        handle.CONTENT = alertTreatment.Content;
                        handle.BUSINESS_ALERT_ID = item.ID;
                        content.ALT_BUSINESS_ALERT_HANDLE.Add(handle);
                    }
                }

                content.SaveChanges();

                scope.Complete();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                scope.Dispose();
            }

            return new SingleMessage<bool> { Result = true };
        }

        //private Expression<Func<ALERT_FENCE_VIEW, bool>> GetfencealertExpression(string vehicleID, string fenceID, short fenceType, DateTime? startTime, DateTime? endTime)
        //{
        //    Expression<Func<ALERT_FENCE_VIEW, bool>> Exp = null;
        //    if (!string.IsNullOrEmpty(vehicleID))
        //    {
        //        Exp = (item => item.VEHICLE_ID.Trim().ToLower().Contains(vehicleID.Trim().ToLower()));
        //    }

        //    if (!string.IsNullOrEmpty(fenceID))
        //    {
        //        if (Exp == null)
        //            Exp = (item => item.VEHICLE_ID.Equals(fenceID));
        //        else
        //            Exp = Exp.And(item => item.VEHICLE_ID.Equals(fenceID));
        //    }
        //    if (fenceType != null && fenceType != -1)
        //    {
        //        if (Exp == null)
        //            Exp = (item => fenceType.Equals((short)item.ALERT_TYPE));
        //        else
        //            Exp = Exp.And(item => fenceType.Equals((short)item.ALERT_TYPE));
        //    }
        //    if (startTime != null)
        //    {
        //        if (Exp == null)
        //            Exp = (item => item.ALERT_TIME >= startTime);
        //        else
        //            Exp = Exp.And(item => item.ALERT_TIME >= startTime);
        //    }
        //    if (endTime != null)
        //    {
        //        if (Exp == null)
        //            Exp = (item => item.ALERT_TIME <= endTime);
        //        else
        //            Exp = Exp.And(item => item.ALERT_TIME <= endTime);
        //    }
        //    return Exp;
        //}

        public MultiMessage<VehicleFenceAlert> GetVehicleFenceAlert(PTMSEntities _context, string vehicleID, string fenceID, short fenceType, DateTime? startTime, DateTime? endTime)
        {
            try
            {

                //Expression<Func<ALERT_FENCE_VIEW, bool>> Exp = GetfencealertExpression(vehicleID, fenceID, fenceType, startTime, endTime);
                //var rlt = new MultiMessage<VehicleFenceAlert>();
                //var result = from fence in _context.ALERT_FENCE_VIEW.Where(Exp)
                //             where fence.ALERT_TYPE == 11 || fence.ALERT_TYPE == 12
                //             select new VehicleFenceAlert
                //             {
                //                 Name = fence.NAME,
                //                 VehicleId = fence.VEHICLE_ID,
                //                 InFenceTime = fence.ALERT_TYPE == 11 ? fence.ALERT_TIME : null,
                //                 OutFenceTime = fence.ALERT_TYPE == 12 ? fence.ALERT_TIME : null,
                //                 alertType = (short)fence.ALERT_TYPE,
                //             };
                //if (result.ToList().Count != 0)
                //{
                //    var totalcount = result.ToList().Count();
                //    rlt.Result = result.ToList();
                //    rlt.TotalRecord = totalcount;

                //}
                //return rlt;
                return null;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return null;
            }

        }


        public MultiMessage<BusinessAlertEx> GetUnHandleAlertByClient(PTMSEntities context, string clientID, List<string> orgnizations)
        {
            try
            {
                List<int> type = new List<int>();
                type.Add(0);
                type.Add(1);
                type.Add(2);
                type.Add(3);
                var temp = from b in context.ALT_BUSINESS_ALERT
                           join v in context.BSC_VEHICLE on b.VEHICLE_ID equals v.VEHICLE_ID
                           join r in context.USR_ORGANIZATION.Where(o => o.VALID == (short)ValidEnum.Valid) on v.ORGNIZATION_ID equals r.ID
                           join s in context.BSC_DEV_SUITE on b.SUITE_INFO_ID equals s.SUITE_INFO_ID
                           //join e in context.ALT_BUSINESS_ALERT_HANDLE on b.ID equals e.BUSINESS_ALERT_ID
                           where b.STATUS == 1 && b.CLIENT_ID == clientID && orgnizations.Contains(v.ORGNIZATION_ID) && type.Contains(b.ALERT_TYPE.Value)
                           && b.SUITE_STATUS != (short)DeviceSuiteStatus.Testing
                           orderby b.ALERT_TIME descending
                           select new BusinessAlertEx
                           {
                               AlertTime = b.ALERT_TIME,
                               AlertType = b.ALERT_TYPE,
                               City = b.DISTRICT_CODE,
                               Direction = b.DIRECTION,
                               GpsTime = b.GPS_TIME,
                               GpsValid = b.GPS_VALID,
                               Id = b.ID,
                               Latitude = b.LATITUDE,
                               Longitude = b.LONGITUDE,
                               MdvrCoreId = b.MDVR_CORE_SN,
                               Speed = b.SPEED,
                               VehicleId = b.VEHICLE_ID,
                               OwnerPhone = v.CONTACT_PHONE,
                               VehicleOwner = v.OWNER,
                               SuiteID = s.SUITE_ID,
                               DistrictCode = b.DISTRICT_CODE,
                               OrganizationId = r.ID,
                               OrganizationName = r.NAME,
                               VehicleType = v.VEHICLE_TYPE,
                               //Note = e.CONTENT,
                           };

                var result = temp.Take(200).ToList();

                return new MultiMessage<BusinessAlertEx>(result, result.Count);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return null;
            }
        }

        public SingleMessage<AlertHandleResult> InsertBusinessAlertHandle(PTMSEntities context, BusinessAlertHandle model)
        {
            var businessalert = context.ALT_BUSINESS_ALERT.FirstOrDefault(n => n.ID == model.BusinessAlertID);
            if (businessalert != null)
            {
                businessalert.STATUS = 4;
            }

            var entity = new ALT_BUSINESS_ALERT_HANDLE();
            BusinessAlertHandleUtility.UpdateEntity(entity, model, true);

            context.ALT_BUSINESS_ALERT_HANDLE.Add(entity);

            if (context.SaveChanges() > 0)
            {
                SingleMessage<AlertHandleResult> result = new SingleMessage<AlertHandleResult>();
                result.Result = new AlertHandleResult();
                result.Result.AlertID = model.BusinessAlertID;
                result.Result.Content = model.Content;
                result.Result.HandleTime = model.HandleTime;

                return result;
            }
            else
            {
                return new SingleMessage<AlertHandleResult>(false, "FailedToSave");
            }
        }

        public SingleMessage<BusinessAlertHandle> GetBusinessAlertHandle(PTMSEntities context, string alertID)
        {
            var entity = from bh in context.ALT_BUSINESS_ALERT_HANDLE
                         where bh.BUSINESS_ALERT_ID == alertID
                         select new BusinessAlertHandle
                         {
                             BusinessAlertID = bh.BUSINESS_ALERT_ID,
                             Content = bh.CONTENT,
                             HandleTime = bh.HANDLE_TIME.Value,
                             HandleUser = bh.HANDLE_USER,
                             ID = bh.ID
                         };
            BusinessAlertHandle model = entity.FirstOrDefault();

            if (model != null)
            {
                return new SingleMessage<BusinessAlertHandle>(model);
            }
            else
            {
                return new SingleMessage<BusinessAlertHandle>(false, "");
            }
        }

        public SingleMessage<BusinessAlertEx> GetVehicleAlertDisposeInfoResp(PTMSEntities _context, string id, string vehicleId, string clientId)
        {
            var entity = from t in _context.ALT_BUSINESS_ALERT
                         join d in _context.ALT_BUSINESS_ALERT_HANDLE on t.ID equals d.BUSINESS_ALERT_ID
                         where t.VEHICLE_ID == vehicleId && t.CLIENT_ID == clientId && t.ID == id
                         select new BusinessAlertEx
                         {
                             Id = t.ID,
                             SuiteID = t.SUITE_INFO_ID,
                             Note = d.CONTENT,
                             HandlePerson = d.HANDLE_USER,
                             HandleTime = d.HANDLE_TIME.Value,
                         };
            var model = entity.FirstOrDefault();
            return new SingleMessage<BusinessAlertEx>(model);
            //if (model != null)
            //{
            //    return new SingleMessage<BusinessAlertEx>(model);
            //}
            //else
            //{
            //    return new SingleMessage<BusinessAlertEx>(false, "");
            //}
        }

        public MultiMessage<BusinessAlertEx> GetAllBusinessAlert(PTMSEntities _context, string carNumber, DateTime? startTime, DateTime? endTime, PagingInfo pagingInfo, List<string> orgnizations, int alerttype)
        {
            if (alerttype == null)
            {
                alerttype = 4;
            }
            List<int> type = new List<int>();
            if (alerttype > 3 || alerttype < 0)
            {
                type.Add(0);
                type.Add(1);
                type.Add(2);
                type.Add(3);
            }
            else
            {

                type.Add(alerttype);

            }
           

            var source = from alert in _context.ALT_BUSINESS_ALERT
                         join vehcle in _context.BSC_VEHICLE on alert.VEHICLE_ID equals vehcle.VEHICLE_ID
                         join or in _context.USR_ORGANIZATION.Where(o => o.VALID == (short)ValidEnum.Valid) on vehcle.ORGNIZATION_ID equals or.ID
                         join city in _context.BSC_DISTRICT on alert.DISTRICT_CODE equals city.CODE
                         join suite in _context.BSC_DEV_SUITE on alert.MDVR_CORE_SN equals suite.MDVR_CORE_SN
                         join province in _context.BSC_DISTRICT on alert.DISTRICT_CODE.Substring(0, 2) equals province.CODE
                         where (alert.SUITE_STATUS == (short)DeviceSuiteStatus.Running || alert.SUITE_STATUS == (short)DeviceSuiteStatus.WaitingMaintenance || alert.SUITE_STATUS == (short)DeviceSuiteStatus.Abnormal) &&
                         (string.IsNullOrEmpty(carNumber) ? true : alert.VEHICLE_ID.ToLower().Contains(carNumber.ToLower())) &&
                         (startTime == null ? true : alert.ALERT_TIME > startTime) &&
                         (endTime == null ? true : alert.ALERT_TIME < endTime) && orgnizations.Contains(vehcle.ORGNIZATION_ID)
                         && type.Contains(alert.ALERT_TYPE.Value)
                         select new BusinessAlertEx
                         {
                             Id = alert.ID,
                             VehicleId = alert.VEHICLE_ID,
                             AlertType = alert.ALERT_TYPE,
                             AlertTime = alert.ALERT_TIME,
                             Longitude = alert.LONGITUDE,
                             Latitude = alert.LATITUDE,
                             //Content = alerthandle.CONTENT,
                             //DisposeStaff = alerthandle.HANDLE_USER,
                             //DisposeTime = alerthandle.HANDLE_TIME,
                             //AlertId = alerthandle.ID,
                             MdvrCoreId = alert.MDVR_CORE_SN,
                             Direction = alert.DIRECTION,
                             Status = alert.STATUS.Value,
                             GpsTime = alert.GPS_TIME,
                             GpsValid = alert.GPS_VALID,
                             //BUSINESS_ALERT_ID = alerthandle.BUSINESS_ALERT_ID,
                             Speed = alert.SPEED,
                             City = city.NAME,
                             Province = province.NAME,
                             VehicleOwner = vehcle.OWNER,
                             SuiteID = suite.SUITE_ID,
                             OwnerPhone = vehcle.CONTACT_PHONE,
                             OrganizationId = or.ID,
                             OrganizationName = or.NAME,
                         };


            var totalcount = source.Count();
            var resultlist = new List<BusinessAlertEx>();
            if (pagingInfo == null)
            {
                resultlist = source.OrderByDescending(c => c.AlertTime).ToList();
            }
            else
            {
                resultlist = source.OrderByDescending(c => c.AlertTime).Skip(pagingInfo.PageSize * (pagingInfo.PageIndex - 1)).Take(pagingInfo.PageSize).ToList();
            }

            var result = new MultiMessage<BusinessAlertEx>
            {
                Result = resultlist,
                TotalRecord = totalcount
            };
            return result;
        }




        public static MultiMessage<BusinessAlertEx> GetAlert(PTMSEntities _context, string carNumber, int pageindex, int pagesize, DateTime starttime, DateTime endtime, int? businesstype)
        {
            var source = from alert in _context.ALT_BUSINESS_ALERT
                         join vehcle in _context.BSC_VEHICLE on alert.VEHICLE_ID equals vehcle.VEHICLE_ID
                         join city in _context.BSC_DISTRICT on alert.DISTRICT_CODE equals city.CODE
                         join suite in _context.BSC_DEV_SUITE on alert.MDVR_CORE_SN equals suite.MDVR_CORE_SN
                         join province in _context.BSC_DISTRICT on alert.DISTRICT_CODE.Substring(0, 2) equals province.CODE
                         where (alert.SUITE_STATUS == (short)DeviceSuiteStatus.Running || alert.SUITE_STATUS == (short)DeviceSuiteStatus.WaitingMaintenance || alert.SUITE_STATUS == (short)DeviceSuiteStatus.Abnormal) && alert.ALERT_TIME > starttime && alert.ALERT_TIME < endtime && (businesstype == null ? alert.ALERT_TYPE < 4 : alert.ALERT_TYPE == businesstype)

                         select new BusinessAlertEx
                         {
                             Id = alert.ID,
                             VehicleId = alert.VEHICLE_ID,
                             AlertType = alert.ALERT_TYPE,
                             AlertTime = alert.ALERT_TIME,
                             Longitude = alert.LONGITUDE,
                             Latitude = alert.LATITUDE,
                             //Content = alerthandle.CONTENT,
                             //DisposeStaff = alerthandle.HANDLE_USER,
                             //DisposeTime = alerthandle.HANDLE_TIME,
                             //AlertId = alerthandle.ID,
                             MdvrCoreId = alert.MDVR_CORE_SN,
                             Direction = alert.DIRECTION,
                             Status = alert.STATUS.Value,
                             GpsTime = alert.GPS_TIME,
                             GpsValid = alert.GPS_VALID,
                             //BUSINESS_ALERT_ID = alerthandle.BUSINESS_ALERT_ID,
                             Speed = alert.SPEED,
                             City = city.NAME,
                             Province = province.NAME,
                             VehicleOwner = vehcle.OWNER,
                             SuiteID = suite.SUITE_ID,
                             OwnerPhone = vehcle.CONTACT_PHONE
                         };


            var totalcount = source.Count();
            var resultlist = new List<BusinessAlertEx>();

            resultlist = source.OrderByDescending(c => c.AlertTime).Skip(pagesize * (pageindex - 1)).Take(pagesize).ToList();

            var result = new MultiMessage<BusinessAlertEx>
            {
                Result = resultlist,
                TotalRecord = totalcount
            };
            return result;
        }

    }
}
