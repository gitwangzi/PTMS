/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d81c6c11-359b-4e8c-8cca-7e776a93854d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Alarm
/////    Project Description:    
/////             Class Name: ECU911Treatment
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/20 10:36:39
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/20 10:36:39
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Alarm.Repository;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.Common.Logging;
using Gsafety.Common.Util;

using Gs.CitySafety.AccessInContracts;
using Gs.CitySafety.AccessInContracts.Data;
using System.Configuration;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.Analysis.Alarm
{
    public static class ARADSTreatment
    {
        private static VehicleAlarmRepository _vehicleAlarmRepository;
        private static List<ECU911Mapping> _mappingInfo;
        private static System.Timers.Timer _timerForFaile;
        private static System.Timers.Timer _timerForOverTime;
        private static string CenterCode;

        static ARADSTreatment()
        {
            _vehicleAlarmRepository = new VehicleAlarmRepository();
            _mappingInfo = _vehicleAlarmRepository.GetEcu911MappingInfo();

            _timerForFaile = new System.Timers.Timer();
            string failTimespan = ConfigurationManager.AppSettings["AlarmToARADSFaileTimespan"] ?? "3600000";
            _timerForFaile.Interval = Convert.ToDouble(failTimespan);
            _timerForFaile.Elapsed += _timer_Elapsed;
            _timerForFaile.Start();

            _timerForOverTime = new System.Timers.Timer();
            string overTimespan = ConfigurationManager.AppSettings["AlarmToARADSOverTimespan"] ?? "3600000";
            CenterCode = ConfigurationManager.AppSettings["CenterCode"];
            _timerForOverTime.Interval = Convert.ToDouble(overTimespan);
            _timerForOverTime.Elapsed += _timerForOverTime_Elapsed;
            _timerForOverTime.Start();
        }

        static async void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _timerForFaile.Stop();
                foreach (var item in _mappingInfo)
                {
                    try
                    {
                        List<AlarmInfo> alarmInfo = _vehicleAlarmRepository.GetSendToECU911FailedInfoByCode(item.DistrictCode);
                        foreach (var alarmItem in alarmInfo)
                        {
                            var result = await SendAlarmInfoToVideoAlarmAsync(alarmItem);
                            if (!result) break;
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerManager.Logger.Error(ex);
                    }
                }

                _timerForFaile.Start();
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        static async void _timerForOverTime_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _timerForOverTime.Stop();
                try
                {
                    List<AlarmInfo> alarmInfo = _vehicleAlarmRepository.UpdateSendToECU911OverTimeInfo();
                    foreach (var alarmItem in alarmInfo)
                    {
                        var result = await SendAlarmInfoToVideoAlarmAsync(alarmItem);
                        if (!result) break;
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                }
                _timerForOverTime.Start();
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }


        /// <summary>
        /// 911 forward model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static TransferAlarmInfo BuildAlarmInfo(AlarmInfo model)
        {
            if (string.IsNullOrEmpty(model.Note)) model.Note = "from ptms!";
            if (string.IsNullOrEmpty(model.Mobile)) model.Mobile = "from ptms";


            return new TransferAlarmInfo()
            {
                //Required property
                busNumber = model.VehicleId,
                orientation = model.Direction,
                mdvrCoreSn = model.MdvrCoreId,
                gpsTime = model.GpsTime.HasValue ? model.GpsTime.Value : DateTime.Now,
                assigedArea = string.IsNullOrEmpty(model.AlarmAddressCode) ? model.DistrictCode : model.AlarmAddressCode,//model.DistrictCode
                Mobile = model.Mobile,
                //AlarmOrignalTypeId=14,
                reportTime = DateTime.Now,
                vehicleType = model.VehicleType,
                longitude = (float)Convert.ToDouble(GISConvertHelper.GetLongitude(model.Longitude)),
                latitude = (float)Convert.ToDouble(GISConvertHelper.GetLatitude(model.Latitude)),
                owner = model.Owner,
                district = model.DistrictCode,
                speed = double.Parse(GISConvertHelper.GetSpeed(model.Speed, System.Globalization.CultureInfo.CurrentCulture)),
                brandModel = model.BrandModel,
                operationLincese = model.OperationLincese,
                startYear = model.StartYear,
                reportMan = model.Owner,
                reportPhone = model.Mobile,
                reportAddress = model.DistrictName,
                vehicleSn = model.VehicleSn,
                centerCode = CenterCode,
                alarmId = model.Id,
                districtName = model.DistrictName
            };
        }



        /// <summary>
        /// send alarm to 911
        /// </summary>
        /// <returns></returns>
        public async static Task<bool> SendAlarmInfoToVideoAlarmAsync(AlarmInfo model)
        {
            bool returnResult = false;
            ECU911Dispose disposeItem = null;

            try
            {
                //if can,t find AlarmAddressCode ，use model provicecode
                if (string.IsNullOrEmpty(model.AlarmAddressCode))
                {
                    model.AlarmAddressCode = model.ProvinceCode;
                }

                var mappingItem = _mappingInfo.FirstOrDefault(x => x.DistrictCode == model.AlarmAddressCode);
                if (mappingItem == null)
                {
                    LoggerManager.Logger.Error(string.Format("Can not find VEN911 code from VEN911 mapping cache,VEN911 code: : {0}！", model.AlarmAddressCode));
                    return returnResult;
                }

                disposeItem = new ECU911Dispose() { AlarmID = model.Id, ForwardTime = DateTime.Now, ForwardDest = model.AlarmAddressCode, AlarmAddress = model.AlarmAddress };


                var vehicleAlarmItem = BuildAlarmInfo(model);
                if (string.IsNullOrEmpty(vehicleAlarmItem.owner))
                {
                    vehicleAlarmItem.owner = "ptms";
                }
                else if (vehicleAlarmItem.owner.Length > 128)
                {
                    vehicleAlarmItem.owner = vehicleAlarmItem.owner.Substring(0, 120);
                }

                disposeItem.Ecu911Center = mappingItem.ECU911Name;


                string sendAlarmUrl = string.Format(mappingItem.ECU911Url);
                object obj;
                obj = WebServiceHelper.InvokeWebService(
                sendAlarmUrl,
                "Gs.VedioAppeal.WebServices",
                "TerminalWebSevice",
                "PublicTrafficAlarmService",
                new object[]
                {
                    vehicleAlarmItem.busNumber,
                    vehicleAlarmItem.orientation,
                    vehicleAlarmItem.mdvrCoreSn,
                    vehicleAlarmItem.gpsTime,
                    vehicleAlarmItem.speed,
                    vehicleAlarmItem.assigedArea,
                    vehicleAlarmItem.vehicleSn,
                    vehicleAlarmItem.brandModel,
                    vehicleAlarmItem.vehicleType,
                    vehicleAlarmItem.Mobile,
                    vehicleAlarmItem.operationLincese,
                    vehicleAlarmItem.owner,
                    vehicleAlarmItem.district,
                    vehicleAlarmItem.startYear,
                    vehicleAlarmItem.reportMan,
                    vehicleAlarmItem.reportPhone,
                    vehicleAlarmItem.reportAddress,
                    vehicleAlarmItem.gpsTime,
                    vehicleAlarmItem.longitude,
                    vehicleAlarmItem.latitude,
                    vehicleAlarmItem.centerCode,
                    vehicleAlarmItem.alarmId,
                    vehicleAlarmItem.districtName,
                    model.AlarmUid
                });

                int i = 0;
                while (!obj.ToString().Equals("1") && i < 3)
                {
                    i++;
                    LoggerManager.Logger.Error(string.Format("Send alarm message to ARADS failure .Try again :" + i));

                    obj = WebServiceHelper.InvokeWebService(
                    sendAlarmUrl,
                    "Gs.VedioAppeal.WebServices",
                    "TerminalWebSevice",
                    "PublicTrafficAlarmService",
                     new object[]
                        {
                            vehicleAlarmItem.busNumber,
                            vehicleAlarmItem.orientation,
                            vehicleAlarmItem.mdvrCoreSn,
                            vehicleAlarmItem.gpsTime,
                            vehicleAlarmItem.speed,
                            vehicleAlarmItem.assigedArea,
                            vehicleAlarmItem.vehicleSn,
                            vehicleAlarmItem.brandModel,
                            vehicleAlarmItem.vehicleType,
                            vehicleAlarmItem.Mobile,
                            vehicleAlarmItem.operationLincese,
                            vehicleAlarmItem.owner,
                            vehicleAlarmItem.district,
                            vehicleAlarmItem.startYear,
                            vehicleAlarmItem.reportMan,
                            vehicleAlarmItem.reportPhone,
                            vehicleAlarmItem.reportAddress,
                            vehicleAlarmItem.reportTime,
                            vehicleAlarmItem.longitude,
                            vehicleAlarmItem.latitude,
                            vehicleAlarmItem.centerCode,
                            vehicleAlarmItem.alarmId,
                            vehicleAlarmItem.districtName,
                            model.AlarmUid
                        });
                }

                if (obj.ToString().Equals("1"))
                {
                    //send to 911 successful
                    disposeItem.ForwardedFlag = 1;
                    returnResult = true;
                    LoggerManager.Logger.Info("Send alarm message to ARADS success！AlarmId：" + vehicleAlarmItem.alarmId);
                }
                else
                {
                    //send to 911 fail
                    disposeItem.ForwardedFlag = 2;
                    LoggerManager.Logger.Error(string.Format("Send alarm message to ARADS failure,error message : return false！"));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                //sent to 911 fail
                disposeItem.ForwardedFlag = 2;
            }
            finally
            {
                LoggerManager.Logger.Info(string.Format("Update ECU911_DISPOSE AlarmID:{0}", disposeItem.AlarmID));
                _vehicleAlarmRepository.UpdateARADSDispose(disposeItem);
            }
            return returnResult;
        }
    }
}
