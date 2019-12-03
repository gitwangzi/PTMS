using Gsafety.Common.Logging;
using Gsafety.PTMS.Alarm.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Enum;
using Gsafety.PTMS.DBEntity;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0951a455-6e48-4f4f-9b34-abdbe248ae27      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Repository
/////    Project Description:    
/////             Class Name: VehicleAlarmRepository
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/15 9:40:01
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/15 9:40:01
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using System.Transactions;

namespace Gsafety.PTMS.Alarm.Repository
{
    public class VehicleAlarmRepository
    {
        public bool AddAlarm(PTMSEntities context, AlarmInfoEx model)
        {
            bool ret = false;
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (var scope = new TransactionScope(TransactionScopeOption.Required, optons))
            {
                try
                {
                    ALM_ALARM_RECORD entity = new ALM_ALARM_RECORD();
                    entity.ID = model.ID;
                    entity.SUITE_INFO_ID = model.SuiteInfoID;
                    entity.MDVR_CORE_SN = model.MdvrCoreId;
                    entity.VEHICLE_ID = model.VehicleId;
                    entity.SUITE_STATUS = (short)model.SuiteStatus;
                    entity.GPS_VALID = model.GpsValid;
                    entity.STATUS = 1;//not handle
                    entity.BUTTON_NUM = (short)model.ButtonNum;
                    entity.REMOVED_FLAG = 0;
                    entity.DISTRICT_CODE = model.DistrictCode;
                    entity.LONGITUDE = model.Longitude;
                    entity.LATITUDE = model.Latitude.ToString();
                    if (model.GpsTime != DateTime.MinValue)
                    {
                        entity.GPS_TIME = model.GpsTime;
                    }
                    entity.SPEED = model.Speed;
                    entity.DIRECTION = model.Direction;
                    entity.CREATE_TIME = DateTime.Now.ToUniversalTime();
                    entity.CLIENT_ID = model.ClientId;
                    entity.ADDITIONAL_INFO = model.AdditionalInfo;
                    entity.NOTE = string.Empty;
                    entity.SOURCE = model.Source;
                    entity.ALARM_UID = model.MdvrCoreId;
                    entity.USER_ID = model.User;
                    entity.KEYWORD = model.Keyword;
                    entity.ALARM_CONTENT = model.AlarmContent;


                    context.ALM_ALARM_RECORD.Add(entity);
                    LoggerManager.Logger.Info("AddAlarm [SuiteStatus] is " + model.SuiteStatus);
                    if (model.SuiteStatus == (int)DeviceSuiteStatus.Testing)
                    {
                        var result = (from a in context.MTN_INSTALLATION_AUDIT
                                      join d in context.MTN_INSTALLATION_DETAIL on a.INSTALL_ID equals d.ID
                                      where a.AUDIT_FLAG == -1
                                      && d.MDVR_CORE_SN == model.MdvrCoreId
                                      && d.CHECKSTEP < 7
                                      select a).FirstOrDefault();

                        if (result != null)
                        {
                            LoggerManager.Logger.Info("AddAlarm:[result] is not null");
                            result.ALARM_CHECK = 1;
                        }
                        else
                        {
                            LoggerManager.Logger.Error("AddAlarm:[result] is null");
                        }
                    }
                    context.SaveChanges();

                    scope.Complete();

                    ret = true;
                }
                catch (Exception ex)
                {
                    //LoggerManager.Logger.Error(ex);
                    LoggerManager.Logger.Error("********\r\n" + DateTime.Now + ":An exception occurred while  alarm information into the database。\r\n" + "Method:Gsafety.PTMS.Alarm.Repository.VehicleAlarmRepository.AddAlarm;\r\n" + ex.ToString() + "\r\n********\r\n");
                }
            }

            return ret;
        }

        public MultiMessage<Gsafety.PTMS.Common.Data.AlarmInfoEx> GetSecurityAlarms(PTMSEntities context, string carNumber, DateTime? startTime, DateTime? endTime, short? isTrueAlarm, PagingInfo pagingInfo)
        {
            MultiMessage<Gsafety.PTMS.Common.Data.AlarmInfoEx> result = null;

            var source = from a in context.ALM_ALARM_RECORD
                         join b in context.BSC_VEHICLE on a.VEHICLE_ID equals b.VEHICLE_ID
                         join d in context.BSC_DISTRICT on b.DISTRICT_CODE equals d.CODE
                         join e in context.BSC_DISTRICT on b.DISTRICT_CODE.Substring(0, 2) equals e.CODE
                         join f in context.BSC_DEV_SUITE on a.SUITE_INFO_ID equals f.SUITE_INFO_ID
                         join h in context.ALM_911_DISPOSE on a.ID equals h.ALARM_ID
                         where a.STATUS == 4 &&
                         (a.SUITE_STATUS == (short)DeviceSuiteStatus.WaitingMaintenance || a.SUITE_STATUS == (short)DeviceSuiteStatus.Running || a.SUITE_STATUS == (short)DeviceSuiteStatus.Abnormal) &&
                         (string.IsNullOrEmpty(carNumber) ? true : a.VEHICLE_ID.ToLower().Contains(carNumber.ToLower()))
                         && (startTime == null ? true : a.GPS_TIME >= startTime)
                         && (endTime == null ? true : endTime >= a.GPS_TIME)
                         && (isTrueAlarm == 1 ? h.ALARM_FLAG == 1 : true)
                         && (isTrueAlarm == 0 ? (h.ALARM_FLAG == 0 || h.ALARM_FLAG == null) : true)
                         select new Gsafety.PTMS.Common.Data.AlarmInfoEx
                         {
                             ID = a.ID,
                             AlarmTime = a.GPS_TIME.Value,
                             VehicleId = a.VEHICLE_ID,
                             Speed = a.SPEED,
                             Direction = a.DIRECTION,
                             GpsTime = a.GPS_TIME,
                             GpsValid = a.GPS_VALID,
                             Latitude = a.LATITUDE,
                             Longitude = a.LONGITUDE,
                             ButtonNum = a.BUTTON_NUM.Value,
                             MdvrCoreId = a.MDVR_CORE_SN,
                             OwnerPhone = b.CONTACT_PHONE,
                             VehicleOwner = b.OWNER,
                             SuiteID = f.SUITE_ID,
                             City = d.NAME,
                             Province = e.NAME,
                         };
            var list = source.OrderByDescending(p => p.AlarmTime).Skip((pagingInfo.PageIndex - 1) * pagingInfo.PageSize).Take(pagingInfo.PageSize).ToList();
            var totalcont = source.Count();
            result = new MultiMessage<Gsafety.PTMS.Common.Data.AlarmInfoEx>
            {
                TotalRecord = totalcont,
                Result = list
            };

            return result;
        }

        public MultiMessage<Gsafety.PTMS.Alarm.Contract.Data.AlarmInfo> GetUnHandledAllAlarms(PTMSEntities context, UserInfoMessageHeader userinfo, PagingInfo pagingInfo)
        {
            //ConditonMaker<ALARM_UNHANDLED_VIEW> cdm = new ConditonMaker<ALARM_UNHANDLED_VIEW>();
            //List<CondtionItem> conditons = GetCommonCondition(userinfo);
            //BasePropertyConditonGroup conditonGroup = new BasePropertyConditonGroup(Gsafety.PTMS.BaseInfo.Conditions.MakerContions.Enums.LogicSymbol.OR, conditons);
            //MultiMessage<Gsafety.PTMS.Message.Contract.Data.AlarmInfo> resultinfo = null;

            //var source = from item in context.ALARM_UNHANDLED_VIEW.Where(cdm.MakeCondtions(conditonGroup))
            //             select new Gsafety.PTMS.Message.Contract.Data.AlarmInfo
            //             {
            //                 Id = item.ID,
            //                 VehicleId = item.VEHICLE_ID,
            //                 MdvrCoreId = item.MDVR_CORE_SN,
            //                 ProvinceName = item.PROVINCE,
            //                 CityName = item.CITY,
            //                 ButtonNum = item.BUTTON_NUM.Value,
            //                 //BusinessType = item.VEHICLE_TYPE.Value,
            //                 //Direction = item.DIRECTION,
            //                 //AlarmGuid = item.ALARM_UID,
            //                 //Latitude = item.LATITUDE,
            //                 //Longitude = item.LONGITUDE,
            //                 //GpsTime = item.GPS_TIME,
            //                 //GpsValid = item.GPS_VALID,
            //                 //Speed = item.SPEED,
            //                 //VehicleType = item.VEHICLE_TYPE.Value,
            //                 //Phone = item.OWNER_PHONE,
            //                 //VehicleOwner = item.OWNER,
            //                 SuiteID = item.SUITE_ID,
            //                 AlarmStatus = item.STATUS.Value,
            //                 //AlarmTime = item.GPS_TIME,

            //             };
            //var dataSource = new List<Gsafety.PTMS.Message.Contract.Data.AlarmInfo>();
            //if (pagingInfo.PageSize == -1)
            //{
            //    dataSource = source.OrderByDescending(x => x.AlarmTime).ToList();
            //}
            //else
            //{
            //    dataSource = source.OrderByDescending(x => x.AlarmTime).Skip((pagingInfo.PageIndex - 1) * pagingInfo.PageSize).Take(pagingInfo.PageSize).ToList();
            //}

            //resultinfo = new MultiMessage<Gsafety.PTMS.Message.Contract.Data.AlarmInfo>()
            //{
            //    Result = dataSource,
            //    TotalRecord = dataSource.Count,
            //};
            //return resultinfo;
            return null;
        }

        public SingleMessage<bool> IfAlarmDetail(PTMSEntities context, string vehicleAlarmId)
        {
            SingleMessage<bool> result = null;

            var source = (from item in context.ALM_ALARM_DISPOSE
                          where item.ALARM_ID == vehicleAlarmId
                          select item).OrderByDescending(c => c.DISPOSE_TIME).FirstOrDefault();

            if (source == null)
            {
                result = new SingleMessage<bool> { Result = false };
            }
            else
            {
                result = new SingleMessage<bool> { Result = true };
            }
            return result;

        }

        public SingleMessage<bool> AddAlarmNote(PTMSEntities context, string ID,string clientid,string note)
        {
           

            try
            {
                ALM_ALARM_NOTE data = new ALM_ALARM_NOTE();
                data.ID = Guid.NewGuid().ToString();
                data.CLIENT_ID = clientid;
                data.NOTE = note;
                data.VALID = 1;
                context.ALM_ALARM_NOTE.Add(data);
                context.SaveChanges();              
            }
            catch (Exception)
            {
                throw;
            }
          
            return new SingleMessage<bool> { Result = true };
        }

        public SingleMessage<bool> UpdateAlarmNote(PTMSEntities context, string ID, string note)
        {


            try
            {

                var source = (from item in context.ALM_ALARM_NOTE
                              where item.ID == ID
                              select item).FirstOrDefault();

                if (source != null)
                {
                 
                    source.NOTE = note;                   
                    context.SaveChanges();
                }              

               
            }
            catch (Exception)
            {
                throw;
            }

            return new SingleMessage<bool> { Result = true };
        }

        public SingleMessage<bool> DeleteAlarmNote(PTMSEntities context, string ID)
        {


            try
            {
                var source = (from item in context.ALM_ALARM_NOTE
                              where item.ID == ID
                              select item).FirstOrDefault();

                if (source != null)
                {
                    source.VALID = 0;                   
                    context.SaveChanges();
                }              

            }
            catch (Exception)
            {
                throw;
            }

            return new SingleMessage<bool> { Result = true };
        }

        public List<GPS> GetAlarmGPSTrack(PTMSEntities context, string vehicleId, DateTime startTime, DateTime endTime)
        {

            var result = from a in context.RUN_VEHICLE_LOCATION
                         .Where(o => o.GPS_TIME >= startTime
                             && o.GPS_TIME <= endTime
                             && o.VEHICLE_ID == vehicleId)
                         select new GPS
                         {
                             Longitude = a.LONGITUDE,
                             Latitude = a.LATITUDE,
                             Speed = a.SPEED,
                             Direction = a.DIRECTION,
                             //MdvrCoreId = a.MDVR_CORE_SN,
                             Valid = a.GPS_VALID,
                             GpsTime = a.GPS_TIME,
                             VehicleId = a.VEHICLE_ID
                         };
            var Result = result.OrderBy(i => i.GpsTime).Take(2000).ToList();
            return Result;

        }

        public List<AlarmTreatment> GetAlarmTreatment(PTMSEntities context, string alarmID)
        {

            var result = from a in context.ALM_911_DISPOSE
                         .Where(o => o.ALARM_ID == alarmID)
                         select new AlarmTreatment
                         {
                             DisposeStaff = a.DISPOSE_STAFF,
                             DisposeTime = a.DISPOSE_TIME.Value,
                             Content = a.CONTENT,
                             IsTrueAlarm = a.ALARM_FLAG
                         };
            return result.ToList();

        }

        public Alarm911Treatment GetAlarm911Treatments(string ID)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                //perhaps need left join,if
                //the data in ecu911_dispose don't have forward_dest
                //will can't find the prop for exmple :
                //the clinet will can't cathc time
                var result = (from i in context.ALM_911_DISPOSE
                              join x in context.DISTRICT_LEVEL_VIEW on i.FORWARD_DEST equals x.CODE
                              where i.ALARM_ID == ID
                              select new Alarm911Treatment
                              {
                                  AlarmId = i.ALARM_ID,
                                  DisposeStaff = i.APPREAL_STAFF,
                                  DisposeTime = i.DISPOSE_TIME,
                                  Content = i.DISPOSE_CONTENT,
                                  Ecu911Center = i.TRANSFER_CENTER,
                                  ALARM_FLAG = i.ALARM_FLAG,
                                  ALARM_ADDRESS = i.ALARM_ADDRESS,
                                  FORWARD_DEST = x.PROVINCE,
                                  FORWARD_TIME = i.FORWARD_TIME.Value,
                                  FORWARDED_FLAG = i.FORWARDED_FLAG.Value,
                                  INCIDENT_ID = i.INCIDENT_ID,
                              }).FirstOrDefault();
                return result;
            }
        }

        /// <summary>
        /// 根据转警是否成功 对警情状态库进行更新
        /// </summary>
        /// <param name="alarmID"></param>
        /// <param name="bTransferSuccessful"></param>
        public void SystemAutoTransferAlarmEnd(string alarmID, bool bTransferSuccessful)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                var scope = new TransactionScope();

                try
                {
                    var result = (from a in context.ALM_ALARM_RECORD.Where(o => o.ID == alarmID)
                                  select a).FirstOrDefault();
                    if (result == null) return;
                    if (result.STATUS == 4)
                    {
                        return;
                    }
                    if (bTransferSuccessful)
                    {
                        //成功，改为处置中
                        result.STATUS = 3;
                    }
                    else
                    {
                        //失败，未处置
                        result.STATUS = 1;
                    }
                    //添加处警信息
                    ALM_ALARM_DISPOSE entity = new ALM_ALARM_DISPOSE();
                    entity.ID = Guid.NewGuid().ToString();
                    entity.ALARM_ID = alarmID;
                    entity.CONTENT = "System Auto Transfer!";
                    entity.DISPOSE_TIME = DateTime.Now;
                    entity.DISPOSE_STAFF = "PTMS";
                    entity.ALARM_FLAG = 0;//默认为 假警情，等视频接警处理完成之后再根据结果更改此值
                    context.ALM_ALARM_DISPOSE.Add(entity);


                    //更改 911的转警状态
                    var ecuresult = (from a in context.ALM_911_DISPOSE.Where(o => o.ALARM_ID == alarmID)
                                     select a).FirstOrDefault();
                    if (ecuresult == null) return;
                    if (bTransferSuccessful)
                    {
                        //转发成功
                        ecuresult.FORWARDED_FLAG = 1;
                    }
                    else
                    {
                        //转发失败
                        ecuresult.FORWARDED_FLAG = 2;
                    }

                    context.SaveChanges();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    //EndTrueAlarm will insert into ecu911_dispose,perhaps had a exception to throw ,
                    //so catch this exception don,t .not handled
                    LoggerManager.Logger.Error("Error:EndTrueAlarm Insert Into ECU911_DISPOSE;Exception:" + ex.ToString() + "; ---PROP---AlarmId:" + alarmID);
                }
                finally
                {
                    scope.Dispose();
                }
            }
        }
        /// <summary>
        ///insert into ALARM_RECORD 
        ///and if this alarm message come from the MDVR test the alarm,
        ///this function will through the alarm(update the alarm validate msg to through),
        ///if this alarm message,s GpsValid.Equals("A"),
        ///will through the gps(update the the gps validate msg to through)
        /// </summary>
        /// <param name="model"></param>
        public bool AddAlarm(Gsafety.PTMS.Message.Contract.Data.AlarmInfo model)
        {
            bool ret = false;
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (var scope = new TransactionScope(TransactionScopeOption.Required, optons))
            {
                try
                {
                    using (PTMSEntities context = new PTMSEntities())
                    {
                        ALM_ALARM_RECORD entity = new ALM_ALARM_RECORD();
                        entity.ID = model.Id;
                        entity.ALARM_UID = model.AlarmUid;
                        entity.VEHICLE_ID = model.VehicleId;
                        entity.MDVR_CORE_SN = model.MdvrCoreId;
                        entity.DISTRICT_CODE = model.DistrictCode;
                        entity.SUITE_INFO_ID = model.SuiteInfoID;
                        entity.LONGITUDE = model.Longitude;
                        entity.LATITUDE = model.Latitude;
                        entity.DIRECTION = model.Direction;
                        entity.SPEED = model.Speed;
                        entity.SUITE_STATUS = (short)model.SuiteStatus;
                        entity.REMOVED_FLAG = 0;
                        entity.STATUS = 1;//not handle
                        entity.GPS_VALID = model.GpsValid;
                        entity.GPS_TIME = model.GpsTime;
                        entity.CREATE_TIME = DateTime.Now;
                        entity.BUTTON_NUM = (short)model.ButtonNum;

                        context.ALM_ALARM_RECORD.Add(entity);
                        LoggerManager.Logger.Info("AddAlarm [SuiteStatus] is " + model.SuiteStatus);
                        if (model.SuiteStatus == (int)DeviceSuiteStatus.Testing)
                        {
                            var result = (from a in context.MTN_INSTALLATION_AUDIT
                                          join d in context.MTN_INSTALLATION_DETAIL on a.INSTALL_ID equals d.ID
                                          where a.AUDIT_FLAG == -1
                                          && d.MDVR_CORE_SN == model.MdvrCoreId
                                          && d.CHECKSTEP < 7
                                          select a).FirstOrDefault();

                            if (result != null)
                            {
                                LoggerManager.Logger.Info("AddAlarm:[result] is not null");
                                result.ALARM_CHECK = 1;
                                result.ALARM_ID = model.Id;
                                ////V	Error	    not available，often happen by the shortage of satellite 
                                ////A	Available	Available
                                ////N	None	    the device have not gps module
                                if (model.GpsValid.Equals("A"))
                                {
                                    result.GPS_CHECK = 1;
                                }
                                else
                                {
                                    result.GPS_CHECK = 0;
                                }
                            }
                            else
                            {
                                LoggerManager.Logger.Error("AddAlarm:[result] is null");
                            }
                        }

                        context.SaveChanges();

                        scope.Complete();

                        ret = true;
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                }
            }

            return ret;
        }

        public void AddECU911Dispose(PTMSEntities context, Gsafety.PTMS.Message.Contract.Data.ECU911Dispose model)
        {
            try
            {

                //var result = (from e in context.ECU_DISPOSE
                //              where e.ALARM_ID == model.AlarmID
                //              select e).FirstOrDefault();
                //if (result != null)
                //    return;
                //ECU_DISPOSE item = new ECU_DISPOSE()
                //{
                //    ALARM_ID = model.AlarmID,
                //    ALARM_ADDRESS = model.AlarmAddress,
                //    INSERT_TIME = DateTime.Now
                //};
                //context.ECU_DISPOSE.Add(item);
                //context.SaveChanges();

            }
            catch (Exception ex)
            {
                //EndTrueAlarm will insert into ecu911_dispose,perhaps had a exception to throw ,
                //so catch this exception don,t .not handled
                LoggerManager.Logger.Error("Error:AddECU911Dispose Come From Analysis Server; Insert Into Ecu011_dispose;Exception:" + ex.ToString() + "; ---PROP---AlarmId:" + model.AlarmID);
            }
        }

        public void UpdateARADSDispose(Gsafety.PTMS.Message.Contract.Data.ECU911Dispose model)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                //    var result = (from e in context.ECU_DISPOSE
                //                  where e.ALARM_ID == model.AlarmID
                //                  select e).FirstOrDefault();
                //    if (result != null)
                //    {
                //        result.FORWARDED_FLAG = model.ForwardedFlag;
                //        result.FORWARD_TIME = model.ForwardTime;
                //        result.INCIDENT_ID = model.IncidentId;
                //        result.ALARM_ADDRESS = model.AlarmAddress;
                //        result.FORWARD_DEST = model.ForwardDest;
                //        result.ECU911_CENTER = model.Ecu911Center;
                //        context.SaveChanges();
                //    }
            }
        }

        public List<Gsafety.PTMS.Message.Contract.Data.AlarmInfo> GetSendToECU911FailedInfoByCode(string code)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                //var result = (from a in context.ALARM_RECORD
                //              join e in context.ECU_DISPOSE on a.ID equals e.ALARM_ID
                //              join v in context.VEHICLE on a.VEHICLE_ID equals v.VEHICLE_ID
                //              where e.FORWARDED_FLAG == 2 && v.VALID == 1
                //              && e.FORWARD_DEST == code
                //              select new Gsafety.PTMS.Message.Contract.Data.AlarmInfo
                //              {
                //                  Id = a.ID,
                //                  VehicleId = a.VEHICLE_ID,
                //                  MdvrCoreId = a.MDVR_CORE_SN,
                //                  DistrictCode = a.DISTRICT_CODE,
                //                  SuiteInfoID = a.SUITE_INFO_ID,
                //                  Longitude = a.LONGITUDE,
                //                  Latitude = a.LATITUDE,
                //                  Direction = a.DIRECTION,
                //                  Speed = a.SPEED,
                //                  SuiteStatus = a.SUITE_STATUS,
                //                  GpsValid = a.GPS_VALID,
                //                  GpsTime = a.GPS_TIME,
                //                  AlarmAddressCode = e.FORWARD_DEST,
                //                  VehicleSn = v.VEHICLE_SN,
                //                  VehicleType = (short)v.VEHICLE_TYPE,
                //                  BrandModel = v.BRAND_MODEL,
                //                  Mobile = v.OWNER_PHONE,
                //                  OperationLincese = v.OPERATION_LICENSE,
                //                  Owner = v.OWNER,
                //                  StartYear = v.START_YEAR
                //              }).ToList();
                //return result;
                return null;
            }
        }

        public List<Gsafety.PTMS.Message.Contract.Data.ECU911Mapping> GetEcu911MappingInfo()
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                //var result = from e in context.ECU911_MAPPING
                //             select new Gsafety.PTMS.Message.Contract.Data.ECU911Mapping
                //             {
                //                 ID = e.ID,
                //                 DistrictCode = e.DISTRICT_CODE,
                //                 ECU911Url = e.ECU911_URL,
                //                 Password = e.PASSWORD,
                //                 UserName = e.USER_NAME,
                //                 ECU911Name = e.ECU911_NAME
                //             };
                //return result.ToList();
            }
            return null;
        }

        /// <summary>
        /// get the realAlarmInfo from ALARM_RECORD(Analysis)
        /// </summary>
        /// <param name="alarmId"></param>
        /// <returns></returns>
        public Gsafety.PTMS.Message.Contract.Data.AlarmInfo GetRealAlarmInfo(PTMSEntities context, string alarmId)
        {

            //var result = (from a in context.ALARM_RECORD
            //              join v in context.VEHICLE on a.VEHICLE_ID equals v.VEHICLE_ID
            //              join d in context.DISTRICT on a.DISTRICT_CODE equals d.CODE
            //              where a.ID.Equals(alarmId)
            //              select new Gsafety.PTMS.Message.Contract.Data.AlarmInfo
            //              {
            //                  Id = a.ID,
            //                  VehicleId = a.VEHICLE_ID,
            //                  MdvrCoreId = a.MDVR_CORE_SN,
            //                  DistrictCode = a.DISTRICT_CODE,
            //                  SuiteInfoID = a.SUITE_INFO_ID,
            //                  AlarmUid = a.ALARM_UID,
            //                  Longitude = a.LONGITUDE,
            //                  Latitude = a.LATITUDE,
            //                  Direction = a.DIRECTION,
            //                  Speed = a.SPEED,
            //                  SuiteStatus = a.SUITE_STATUS,
            //                  GpsValid = a.GPS_VALID,
            //                  GpsTime = a.GPS_TIME,

            //                  VehicleSn = v.VEHICLE_SN,
            //                  VehicleType = (short)v.VEHICLE_TYPE,
            //                  BrandModel = v.BRAND_MODEL,
            //                  Mobile = v.OWNER_PHONE,
            //                  OperationLincese = v.OPERATION_LICENSE,
            //                  Owner = v.OWNER,
            //                  StartYear = v.START_YEAR,
            //                  ProvinceCode = v.DISTRICT_CODE,
            //                  DistrictName = d.NAME
            //              }).FirstOrDefault();
            //return result;
            return null;
        }

        public List<Gsafety.PTMS.Message.Contract.Data.AlarmInfo> UpdateSendToECU911OverTimeInfo()
        {
            //using (PTMSEntities context = new PTMSEntities())
            //{
            //    var result = (from a in context.ALARM_RECORD
            //                  join e in context.ECU_DISPOSE on a.ID equals e.ALARM_ID
            //                  join v in context.VEHICLE on a.VEHICLE_ID equals v.VEHICLE_ID
            //                  where e.FORWARDED_FLAG == 3 && v.VALID == 1 && e.INSERT_TIME < DateTime.Now
            //                  select new Gsafety.PTMS.Message.Contract.Data.AlarmInfo
            //                  {
            //                      Id = a.ID,
            //                      VehicleId = a.VEHICLE_ID,
            //                      MdvrCoreId = a.MDVR_CORE_SN,
            //                      DistrictCode = a.DISTRICT_CODE,
            //                      SuiteInfoID = a.SUITE_INFO_ID,
            //                      Longitude = a.LONGITUDE,
            //                      Latitude = a.LATITUDE,
            //                      Direction = a.DIRECTION,
            //                      Speed = a.SPEED,
            //                      SuiteStatus = a.SUITE_STATUS,
            //                      GpsValid = a.GPS_VALID,
            //                      GpsTime = a.GPS_TIME,
            //                      AlarmAddressCode = v.DISTRICT_CODE,
            //                      VehicleSn = v.VEHICLE_SN,
            //                      VehicleType = (short)v.VEHICLE_TYPE,
            //                      BrandModel = v.BRAND_MODEL,
            //                      Mobile = v.OWNER_PHONE,
            //                      OperationLincese = v.OPERATION_LICENSE,
            //                      Owner = v.OWNER,
            //                      StartYear = v.START_YEAR
            //                  });
            //    foreach (Gsafety.PTMS.Message.Contract.Data.AlarmInfo model in result.ToList())
            //    {
            //        var resultDispose = (from e in context.ECU_DISPOSE
            //                             where e.ALARM_ID == model.Id
            //                             select e).FirstOrDefault();
            //        if (result != null)
            //        {
            //            resultDispose.FORWARDED_FLAG = 2;
            //        }
            //    }
            //    context.SaveChanges();
            //    return result.ToList();
            //}
            return null;
        }


        public MultiMessage<AlarmInfoEx> GetUnHandledAllAlarms(PTMSEntities context, string clientid, List<string> Organizations)
        {
            try
            {
                var result = (from a in context.ALM_ALARM_RECORD
                              join v in context.BSC_VEHICLE on a.VEHICLE_ID equals v.VEHICLE_ID
                              join t in context.BSC_VEHICLE_TYPE on v.VEHICLE_TYPE equals t.ID
                              where a.STATUS == 1 && Organizations.Contains(v.ORGNIZATION_ID) && a.SUITE_STATUS != (short)DeviceSuiteStatus.Testing
                              orderby a.GPS_TIME descending
                              select new AlarmInfoEx
                              {
                                  ID = a.ID,
                                  VehicleId = a.VEHICLE_ID,
                                  MdvrCoreId = a.MDVR_CORE_SN,
                                  DistrictCode = v.DISTRICT_CODE,
                                  ButtonNum = a.BUTTON_NUM.Value,
                                  Direction = a.DIRECTION,
                                  AlarmGuid = a.ALARM_UID,
                                  Latitude = a.LATITUDE,
                                  Longitude = a.LONGITUDE,
                                  GpsTime = a.GPS_TIME,
                                  GpsValid = a.GPS_VALID,
                                  Speed = a.SPEED,
                                  OwnerPhone = v.CONTACT_PHONE,
                                  VehicleOwner = v.OWNER,
                                  SuiteInfoID = a.SUITE_INFO_ID,
                                  AlarmStatus = a.STATUS.Value,
                                  AlarmTime = a.GPS_TIME.Value,
                                  Source = a.SOURCE.Value,
                                  //SuiteID = a.SUITE_INFO_ID,
                                  User = a.USER_ID,
                                  AlarmContent = a.ALARM_CONTENT,
                                  Keyword = a.KEYWORD,
                                  ClientId = clientid,
                                  VehicleSn = v.VEHICLE_SN,
                                  VehicleType = t.NAME
                              });


                List<AlarmInfoEx> list = result.Take(200).ToList();

                List<string> suiteids = new List<string>();
                foreach (var item in list)
                {
                    suiteids.Add(item.SuiteInfoID);
                }

                List<BSC_DEV_SUITE> suites = context.BSC_DEV_SUITE.Where(n => suiteids.Contains(n.SUITE_INFO_ID)).ToList();

                foreach (var item in list)
                {
                    BSC_DEV_SUITE suite = suites.FirstOrDefault(n => n.SUITE_INFO_ID == item.SuiteInfoID);
                    if (suite != null)
                    {
                        item.SuiteInfoID = suite.SUITE_INFO_ID;
                        item.SuiteID = suite.SUITE_ID;
                    }
                    else
                    {
                        item.AlarmMobile = item.SuiteInfoID;
                    }
                }

                List<string> alarmids = new List<string>();
                foreach (var item in list)
                {
                    alarmids.Add(item.ID);
                }

                var appealresult = from d in context.ALM_ALARM_DISPOSE
                                   where alarmids.Contains(d.ALARM_ID)
                                   select d.ALARM_ID;

                var appeallist = appealresult.ToList();

                var disposeresult = from d in context.ALM_911_DISPOSE
                                    where alarmids.Contains(d.ALARM_ID)
                                    select d.ALARM_ID;

                var disposelist = disposeresult.ToList();

                foreach (var item in list)
                {
                    if (appeallist.Contains(item.ID))
                    {
                        item.AppealStatus = 4;
                    }

                    if (disposelist.Contains(item.ID))
                    {
                        item.TransferStatus = 4;
                    }
                }


                return new MultiMessage<AlarmInfoEx>(list, list.Count);
            }
            catch (Exception ex)
            {
                return new MultiMessage<AlarmInfoEx>(false, ex);
            }
        }

        public SingleMessage<AlarmHandleResult> HandleAlarm(PTMSEntities context, string alarmid, string user, bool alarmresult, string note, DateTime time, bool istransfer, int transfermode, int incidentlevel, string incidentaddress, string incidenttype)
        {
            try
            {
                SingleMessage<AlarmHandleResult> result = new SingleMessage<AlarmHandleResult>();
                ALM_ALARM_RECORD record = context.ALM_ALARM_RECORD.FirstOrDefault(n => n.ID == alarmid);
                if (record != null)
                {
                    record.STATUS = 2;
                }

                ALM_ALARM_DISPOSE entity = context.ALM_ALARM_DISPOSE.FirstOrDefault(n => n.ALARM_ID == alarmid);
                if (entity == null)
                {
                    entity = new ALM_ALARM_DISPOSE();
                    if (alarmresult)
                    {
                        entity.ALARM_FLAG = 1;
                    }
                    else
                    {
                        entity.ALARM_FLAG = 0;
                    }

                    entity.ALARM_ID = alarmid;
                    entity.CONTENT = note;
                    entity.DISPOSE_STAFF = user;
                    entity.DISPOSE_TIME = time;
                    entity.ID = Guid.NewGuid().ToString();

                    context.ALM_ALARM_DISPOSE.Add(entity);
                }
                else
                {
                    if (alarmresult)
                    {
                        entity.ALARM_FLAG = 1;
                    }
                    else
                    {
                        entity.ALARM_FLAG = 0;
                    }

                    entity.CONTENT = note;
                    entity.DISPOSE_STAFF = user;
                    entity.DISPOSE_TIME = time;
                }

                if (istransfer)
                {
                    ALM_911_DISPOSE transferentity = context.ALM_911_DISPOSE.FirstOrDefault(n => n.ALARM_ID == alarmid);
                    if (transferentity == null)
                    {
                        transferentity = new ALM_911_DISPOSE();
                        transferentity.ALARM_ID = alarmid;
                        transferentity.IS_TRANSFER = 1;
                        transferentity.TRANSFER_MODE = (short)transfermode;
                        transferentity.ID = Guid.NewGuid().ToString();
                        transferentity.CREATE_TIME = DateTime.Now.ToUniversalTime();
                        transferentity.ALARM_ADDRESS = incidentaddress;
                        transferentity.INCIDENT_LEVEL = incidentlevel;
                        transferentity.INCIDENT_TYPE = incidenttype;
                        context.ALM_911_DISPOSE.Add(transferentity);
                    }
                    else
                    {
                        transferentity.ALARM_ID = alarmid;
                        transferentity.IS_TRANSFER = 1;
                        transferentity.TRANSFER_MODE = (short)transfermode;
                        transferentity.ID = Guid.NewGuid().ToString();
                    }
                }

                if (context.SaveChanges() > 0)
                {
                    result.IsSuccess = true;
                    result.Result = new AlarmHandleResult();
                    result.Result.AlarmFlag = alarmresult;
                    result.Result.AlarmID = alarmid;
                    result.Result.HandleTime = time;
                    result.Result.IsTransfer = istransfer;
                    result.Result.IncidentAddress = incidentaddress;
                    result.Result.IncidentLevel = incidentlevel.ToString();
                    result.Result.IncidentType = incidenttype;
                    result.Result.Note = note;
                }
                else
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "FailedToSave";
                }

                return result;
            }
            catch (Exception ex)
            {
                return new SingleMessage<AlarmHandleResult>(false, ex);
            }
        }

        public SingleMessage<ApealDispose> GetApealDisposeByAlarmID(PTMSEntities context, string alarmID)
        {
            ALM_ALARM_DISPOSE entity = context.ALM_ALARM_DISPOSE.SingleOrDefault(n => n.ALARM_ID == alarmID);
            if (entity != null)
            {
                ApealDispose model = ApealDisposeUtility.GetModel(entity);
                return new SingleMessage<ApealDispose>(model);
            }
            return null;
        }

        public SingleMessage<TransferDispose> GetTransferDisposeByAlarmID(PTMSEntities context, string AlarmID)
        {
            ALM_911_DISPOSE entity = context.ALM_911_DISPOSE.SingleOrDefault(n => n.ALARM_ID == AlarmID);
            if (entity != null)
            {
                TransferDispose model = TransferDisposeUtility.GetModel(entity);
                return new SingleMessage<TransferDispose>(model);
            }
            else
            {
                return new SingleMessage<TransferDispose>(false, "NotFound");
            }
        }

        public MultiMessage<AlarmInfoEx> GetAllAlarms(PTMSEntities context, string carNumber, DateTime? startTime, DateTime? endTime, PagingInfo pagingInfo, List<string> organizations)
        {
            MultiMessage<Gsafety.PTMS.Common.Data.AlarmInfoEx> result = null;

            var source = from a in context.ALM_ALARM_RECORD
                         join b in context.BSC_VEHICLE on a.VEHICLE_ID equals b.VEHICLE_ID
                         join d in context.BSC_DISTRICT on b.DISTRICT_CODE equals d.CODE
                         join e in context.BSC_DISTRICT on b.DISTRICT_CODE.Substring(0, 2) equals e.CODE
                         where (a.SUITE_STATUS == (short)DeviceSuiteStatus.WaitingMaintenance || a.SUITE_STATUS == (short)DeviceSuiteStatus.Running || a.SUITE_STATUS == (short)DeviceSuiteStatus.Abnormal) &&
                         (string.IsNullOrEmpty(carNumber) ? true : a.VEHICLE_ID.ToLower().Contains(carNumber.ToLower()))
                         && (startTime.HasValue ? a.GPS_TIME >= startTime : true)
                         && (endTime.HasValue ? endTime >= a.GPS_TIME : true) && organizations.Contains(b.ORGNIZATION_ID)
                         select new Gsafety.PTMS.Common.Data.AlarmInfoEx
                         {
                             ID = a.ID,
                             AlarmTime = a.GPS_TIME.Value,
                             VehicleId = a.VEHICLE_ID,
                             Speed = a.SPEED,
                             Direction = a.DIRECTION,
                             GpsTime = a.GPS_TIME,
                             GpsValid = a.GPS_VALID,
                             Latitude = a.LATITUDE,
                             Longitude = a.LONGITUDE,
                             ButtonNum = a.BUTTON_NUM.HasValue ? a.BUTTON_NUM.Value : -1,
                             MdvrCoreId = a.MDVR_CORE_SN,
                             OwnerPhone = b.CONTACT_PHONE,
                             VehicleOwner = b.OWNER,
                             City = d.NAME,
                             Province = e.NAME,
                             AlarmStatus = a.STATUS.Value,
                             SuiteInfoID = a.SUITE_INFO_ID,
                             Source = a.SOURCE.Value,
                             Keyword = a.KEYWORD,
                             AlarmContent = a.ALARM_CONTENT
                         };
            var list = source.OrderByDescending(p => p.AlarmTime).Skip((pagingInfo.PageIndex - 1) * pagingInfo.PageSize).Take(pagingInfo.PageSize).ToList();

            List<string> suiteids = new List<string>();
            foreach (var item in list)
            {
                suiteids.Add(item.SuiteInfoID);
            }

            List<BSC_DEV_SUITE> suites = context.BSC_DEV_SUITE.Where(n => suiteids.Contains(n.SUITE_INFO_ID)).ToList();

            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.SuiteInfoID))
                {
                    continue;
                }

                BSC_DEV_SUITE suite = suites.FirstOrDefault(n => n.SUITE_INFO_ID == item.SuiteInfoID);
                if (suite != null)
                {
                    item.SuiteInfoID = suite.SUITE_INFO_ID;
                    item.SuiteID = suite.SUITE_ID;
                }
                else
                {
                    item.AlarmMobile = item.SuiteInfoID;
                }
            }
            List<string> alarmids = list.Select(n => n.ID).ToList();
            List<string> appealids = context.ALM_ALARM_DISPOSE.Where(n => alarmids.Contains(n.ALARM_ID)).Select(n => n.ALARM_ID).ToList();
            List<string> transferids = context.ALM_911_DISPOSE.Where(n => alarmids.Contains(n.ALARM_ID)).Select(n => n.ALARM_ID).ToList();

            var totalcont = source.Count();
            foreach (var item in list)
            {
                if (appealids.Contains(item.ID))
                {
                    item.AppealStatus = 4;
                }

                if (transferids.Contains(item.ID))
                {
                    item.TransferStatus = 4;
                }
            }
            result = new MultiMessage<Gsafety.PTMS.Common.Data.AlarmInfoEx>
            {
                TotalRecord = totalcont,
                Result = list
            };

            return result;
        }

        public MultiMessage<AlarmNoteInfo> GetAllAlarmNote(PTMSEntities context, string clientid)
        {
            MultiMessage<Gsafety.PTMS.Common.Data.AlarmNoteInfo> result = null;

            var source = from a in context.ALM_ALARM_NOTE
                         where a.CLIENT_ID == clientid && a.VALID==1

                         select new Gsafety.PTMS.Common.Data.AlarmNoteInfo
                         {
                             ID = a.ID,
                             Note = a.NOTE
                            
                         };
            var list = source.ToList();
            var totalcont = source.Count();
            result = new MultiMessage<Gsafety.PTMS.Common.Data.AlarmNoteInfo>
            {
                TotalRecord = totalcont,
                Result = list
            };

            return result;
           
        }


        public MultiMessage<AlarmEmailInfo> GetAllAlarmEmail(PTMSEntities context, string clientid)
        {
            MultiMessage<Gsafety.PTMS.Common.Data.AlarmEmailInfo> result = null;

            var source = from a in context.ALM_ALARM_MAIL
                         where a.CLIENT_ID == clientid && a.VALID == 1 && a.TYPE == 0

                         select new Gsafety.PTMS.Common.Data.AlarmEmailInfo
                         {
                             ID = a.ID,
                             Mail = a.MAIL,
                             Name = a.NAME,
                             Level = a.LEVEL.Value,
                             EmailType = a.TYPE.Value
                         };
            var list = source.ToList();
            var totalcont = source.Count();
            result = new MultiMessage<Gsafety.PTMS.Common.Data.AlarmEmailInfo>
            {
                TotalRecord = totalcont,
                Result = list
            };

            return result;

        }

        public SingleMessage<bool> AddAlarmEmail(PTMSEntities context, AlarmEmailInfo email)
        {


            try
            {
                ALM_ALARM_MAIL data = new ALM_ALARM_MAIL();
                data.ID = email.ID;
                data.CLIENT_ID = email.ClientId;
                data.MAIL = email.Mail;
                data.VALID = 1;
                data.LEVEL = email.Level;
                data.NAME = email.Name;
                data.TYPE = 0;
                context.ALM_ALARM_MAIL.Add(data);
                context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return new SingleMessage<bool> { Result = true };
        }

        public SingleMessage<bool> UpdateAlarmEmail(PTMSEntities context, AlarmEmailInfo email)
        {


            try
            {

                var source = (from item in context.ALM_ALARM_MAIL
                              where item.ID == email.ID
                              select item).FirstOrDefault();

                if (source != null)
                {

                    source.MAIL = email.Mail;
                    source.LEVEL = email.Level;
                    source.NAME = email.Name;
                    context.SaveChanges();
                }


            }
            catch (Exception)
            {
                throw;
            }

            return new SingleMessage<bool> { Result = true };
        }

        public SingleMessage<bool> DeleteAlarmEmail(PTMSEntities context, string ID)
        {


            try
            {
                var source = (from item in context.ALM_ALARM_MAIL
                              where item.ID == ID
                              select item).FirstOrDefault();

                if (source != null)
                {
                    source.VALID = 0;
                    context.SaveChanges();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return new SingleMessage<bool> { Result = true };
        }

        public static MultiMessage<AlarmInfoEx> GetAlarm(PTMSEntities context, string carNumber, int pageindex, int pagesize, DateTime starttime, DateTime endtime)
        {
            MultiMessage<Gsafety.PTMS.Common.Data.AlarmInfoEx> result = null;

            var source = from a in context.ALM_ALARM_RECORD
                         join b in context.BSC_VEHICLE on a.VEHICLE_ID equals b.VEHICLE_ID
                         join s in context.BSC_DEV_SUITE on a.MDVR_CORE_SN equals s.MDVR_CORE_SN
                         join d in context.BSC_DISTRICT on b.DISTRICT_CODE equals d.CODE
                         join e in context.BSC_DISTRICT on b.DISTRICT_CODE.Substring(0, 2) equals e.CODE
                         join f in context.BSC_DEV_SUITE on a.SUITE_INFO_ID equals f.SUITE_INFO_ID
                         where (a.SUITE_STATUS == (short)DeviceSuiteStatus.WaitingMaintenance || a.SUITE_STATUS == (short)DeviceSuiteStatus.Running || a.SUITE_STATUS == (short)DeviceSuiteStatus.Abnormal) && a.VEHICLE_ID.ToLower().Contains(carNumber.ToLower()) && a.CREATE_TIME > starttime && a.CREATE_TIME < endtime
                         select new Gsafety.PTMS.Common.Data.AlarmInfoEx
                         {
                             ID = a.ID,
                             AlarmTime = a.GPS_TIME.Value,
                             VehicleId = a.VEHICLE_ID,
                             Speed = a.SPEED,
                             Direction = a.DIRECTION,
                             GpsTime = a.GPS_TIME,
                             GpsValid = a.GPS_VALID,
                             Latitude = a.LATITUDE,
                             Longitude = a.LONGITUDE,
                             ButtonNum = a.BUTTON_NUM.Value,
                             MdvrCoreId = a.MDVR_CORE_SN,
                             OwnerPhone = b.CONTACT_PHONE,
                             VehicleOwner = b.OWNER,
                             City = d.NAME,
                             Province = e.NAME,
                             AlarmStatus = a.STATUS.Value,
                             SuiteID = s.SUITE_ID,
                             Keyword = a.KEYWORD,
                             AlarmContent = a.ALARM_CONTENT
                         };
            var list = source.OrderByDescending(p => p.AlarmTime).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();

            List<string> alarmids = list.Select(n => n.ID).ToList();
            List<string> appealids = context.ALM_ALARM_DISPOSE.Where(n => alarmids.Contains(n.ALARM_ID)).Select(n => n.ALARM_ID).ToList();
            List<string> transferids = context.ALM_911_DISPOSE.Where(n => alarmids.Contains(n.ALARM_ID)).Select(n => n.ALARM_ID).ToList();

            var totalcont = source.Count();
            foreach (var item in list)
            {
                if (appealids.Contains(item.ID))
                {
                    item.AppealStatus = 4;
                }

                if (transferids.Contains(item.ID))
                {
                    item.TransferStatus = 4;
                }
            }
            result = new MultiMessage<Gsafety.PTMS.Common.Data.AlarmInfoEx>
            {
                TotalRecord = totalcont,
                Result = list
            };

            return result;
        }

        /// <summary>
        /// 安装流程中检查报警
        /// </summary>
        /// <param name="installationDetailID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public SingleMessage<bool> GetAlarmCheck(PTMSEntities context, string installationDetailID, DateTime date)
        {
            var detail = context.MTN_INSTALLATION_DETAIL.FirstOrDefault(n => n.ID == installationDetailID);
            if (detail == null)
            {
                return new SingleMessage<bool>(false, "MTN_INSTALLATION_DETAIL Not Exist");
            }

            var exist = context.ALM_ALARM_RECORD.Where(t => t.MDVR_CORE_SN == detail.MDVR_CORE_SN && t.CREATE_TIME >= detail.CREATE_TIME.Value && t.CREATE_TIME >= date).Any();

            if (exist)
            {
                var audit = context.MTN_INSTALLATION_AUDIT.Where(t => t.INSTALL_ID == detail.ID).FirstOrDefault();
                audit.ALARM_CHECK = 1;

                if (audit.VIDEO_CHECK == 1 && audit.GPS_CHECK == 1 && audit.ALARM_CHECK == 1)
                {
                    audit.AUDIT_FLAG = 1;
                }

                context.SaveChanges();
            }

            return new SingleMessage<bool>(exist);
        }

        public SingleMessage<AlarmInfoEx> InsertManualAlarm(PTMSEntities context, ManualAlarmInfo alarminfo)
        {
            try
            {
                SingleMessage<AlarmInfoEx> result = new SingleMessage<AlarmInfoEx>();
                ALM_ALARM_RECORD record = new ALM_ALARM_RECORD();
                record.CLIENT_ID = alarminfo.ClientID;
                record.CREATE_TIME = DateTime.Now.ToUniversalTime();

                RUN_VEHICLE_LOCATION location = context.RUN_VEHICLE_LOCATION.OrderByDescending(n => n.GPS_TIME).FirstOrDefault(n => n.VEHICLE_ID == alarminfo.VehicleID && n.GPS_VALID == "A");
                if (location != null)
                {
                    record.DIRECTION = location.DIRECTION;
                    record.GPS_VALID = location.GPS_VALID;
                    record.LATITUDE = location.LATITUDE;
                    record.LONGITUDE = location.LONGITUDE;
                    record.SPEED = location.SPEED;
                }

                BSC_VEHICLE vehicle = context.BSC_VEHICLE.FirstOrDefault(n => n.VEHICLE_ID == alarminfo.VehicleID);
                if (vehicle != null)
                {
                    record.DISTRICT_CODE = vehicle.DISTRICT_CODE;
                }

                record.GPS_TIME = alarminfo.GPSTime;
                record.ID = alarminfo.ID;
                record.SOURCE = (short)AlarmSourceEnum.Manual;
                record.USER_ID = alarminfo.UserID;
                record.VEHICLE_ID = alarminfo.VehicleID;
                record.SUITE_STATUS = (short)DeviceSuiteStatus.Running;
                record.STATUS = 2;

                string suiteID = "";
                var suite = context.RUN_SUITE_WORKING.FirstOrDefault(t => t.VEHICLE_ID == alarminfo.VehicleID);
                if (suite != null)
                {
                    record.SUITE_INFO_ID = suite.SUITE_INFO_ID;
                    record.MDVR_CORE_SN = suite.MDVR_CORE_SN;
                    suiteID = suite.MDVR_CORE_SN;
                }

                context.ALM_ALARM_RECORD.Add(record);
                ALM_ALARM_DISPOSE disposeentity = new ALM_ALARM_DISPOSE();
                disposeentity.ALARM_FLAG = 1;
                disposeentity.ALARM_ID = alarminfo.ID;
                disposeentity.CONTENT = alarminfo.Note;
                disposeentity.DISPOSE_STAFF = alarminfo.Account;
                disposeentity.DISPOSE_TIME = alarminfo.GPSTime;
                disposeentity.ID = Guid.NewGuid().ToString();

                context.ALM_ALARM_DISPOSE.Add(disposeentity);


                if (alarminfo.IsTransfer)
                {
                    ALM_911_DISPOSE transferentity = new ALM_911_DISPOSE();
                    transferentity.ALARM_ID = alarminfo.ID;
                    transferentity.IS_TRANSFER = 1;
                    transferentity.TRANSFER_MODE = (short)alarminfo.TransferMode;
                    transferentity.ID = Guid.NewGuid().ToString();
                    transferentity.CREATE_TIME = DateTime.Now.ToUniversalTime();
                    transferentity.ALARM_ADDRESS = alarminfo.IncidentAddress;
                    transferentity.INCIDENT_LEVEL = alarminfo.IncidentLevel;
                    transferentity.INCIDENT_TYPE = alarminfo.IncidentType;
                    context.ALM_911_DISPOSE.Add(transferentity);

                }

                if (context.SaveChanges() > 0)
                {
                    result.IsSuccess = true;
                    result.Result = new AlarmInfoEx();
                    result.Result.ID = record.ID;
                    result.Result.AlarmStatus = 4;
                    result.Result.AlarmTime = record.GPS_TIME;
                    result.Result.AppealStatus = 4;
                    result.Result.BrandModel = vehicle.BRAND_MODEL;
                    result.Result.ClientId = record.CLIENT_ID;
                    result.Result.Direction = record.DIRECTION;
                    if (alarminfo.IsTransfer)
                    {
                        result.Result.TransferStatus = 4;
                        result.Result.DisposalStatus = 0;
                    }

                    result.Result.DistrictCode = vehicle.DISTRICT_CODE;
                    result.Result.GpsTime = alarminfo.GPSTime;
                    result.Result.GpsValid = record.GPS_VALID;
                    result.Result.Latitude = record.LATITUDE;
                    result.Result.Longitude = record.LONGITUDE;
                    result.Result.Source = (short)record.SOURCE;
                    result.Result.Speed = record.SPEED;
                    result.Result.User = record.USER_ID;
                    result.Result.VehicleId = record.VEHICLE_ID;
                    result.Result.SuiteInfoID = record.SUITE_INFO_ID;
                    result.Result.MdvrCoreId = record.MDVR_CORE_SN;
                    result.Result.SuiteID = suiteID;
                    result.Result.IncidentAddress = alarminfo.IncidentAddress;
                    result.Result.IncidentLevel = alarminfo.IncidentLevel.ToString();
                    result.Result.AlarmContent = alarminfo.Note;
                    result.Result.IncidentType = alarminfo.IncidentType;

                    result.IsSuccess = true;

                    return result;
                }
                else
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "FailedToSave";
                }

                return result;
            }
            catch (Exception ex)
            {
                return new SingleMessage<AlarmInfoEx>(false, ex);
            }
        }
    }
}
