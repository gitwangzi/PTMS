/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4c52e1a0-bdd8-4829-a6a6-178354829a37      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Alarm
/////    Project Description:    
/////             Class Name: AlarmTreatment
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/22 15:02:43
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/22 15:02:43
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.PTMS.Alarm.Repository;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.Analysis.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using System.Reflection;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Data;

namespace Gsafety.PTMS.Analysis.Alarm
{
    public static class AlarmTreatment
    {
        private static VehicleAlarmRepository _alarmRepository;
        private static SecuritySuiteRepository _suiteInfoRepository;

        static AlarmTreatment()
        {
            _alarmRepository = new VehicleAlarmRepository();
            _suiteInfoRepository = new SecuritySuiteRepository();
        }

        /// <summary>
        /// the initial alarm and transfer to video alarm
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ReturnInfo InitAlarmInfoAndTransfer(PTMSEntities context, byte[] bytes)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            string ruleKey = null;

            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info(string.Format("Get alarm info:{0}", str));

            //adapter model
            Gsafety.PTMS.Message.Contract.Data.AlarmInfo AlarmModel = new Gsafety.PTMS.Message.Contract.Data.AlarmInfo(str);
            if (AlarmModel != null)
            {
                //fill model 
                if (FillAlarmModel(AlarmModel, out ruleKey))
                {
                    //添加到 报警记录
                    LoggerManager.Logger.Info("InitAlarmInfoAndTransfer:Before Add Alarm info to database!");
                    if (_alarmRepository.AddAlarm(AlarmModel))
                    {
                        LoggerManager.Logger.Info("InitAlarmInfoAndTransfer:After Add Alarm info to database!");
                        returnInfo.RuleKey = ruleKey;
                        //convert the model to binary
                        returnInfo.Message = ConvertHelper.ObjectToBytes(AlarmModel);
                        ////test status can,t send to 911
                        if (AlarmModel.SuiteStatus != (int)DeviceSuiteStatus.Testing)
                        {
                            //    ////send to 911
                            //    //Task.Factory.StartNew(() => {  });
                            //    ECU911Treatment.SendAlarmInfoToECU911Async(AlarmModel);
                            //rule route
                            //returnInfo.RuleKey = ruleKey;
                            //convert the model to binary
                            //returnInfo.Message = ConvertHelper.ObjectToBytes(AlarmModel);
                            bool bTransferResult = ForwardingAlarmToVideoAlarm(context, AlarmModel);
                            _alarmRepository.SystemAutoTransferAlarmEnd(AlarmModel.Id, bTransferResult);
                        }
                        else
                        {
                            LoggerManager.Logger.Info("InitAlarmInfoAndTransfer:[test status can,t send to 911]");
                        }
                    }
                }
            }
            else
            {
                LoggerManager.Logger.Warn(string.Format("Converted alarm to entity is null,string:{0}", str));
            }
            return returnInfo;
        }


        /// <summary>
        /// 转发到视频接系统
        /// </summary>
        /// <param name="bytes"></param>
        public static bool ForwardingAlarmToVideoAlarm(PTMSEntities context, Gsafety.PTMS.Message.Contract.Data.AlarmInfo info)
        {
            try
            {
                LoggerManager.Logger.Info(string.Format("Get Alarm Info From Database MDVR_Core_SN:{0},Alarm_Time:{1}", info.MdvrCoreId, info.AlarmTime));
                Gsafety.PTMS.Message.Contract.Data.AlarmInfo realAlarmInfo = _alarmRepository.GetRealAlarmInfo(context, info.Id);
                ///GIS inferance No debugging,so temporarliy comment,do not delete!from jiangj
                //////Gis get the alarm area
                //FillAlarmAddress(realAlarmInfo);
                //////Gis get the alarm zone number
                //FillAlarmProv(realAlarmInfo);

                //ForwardDest will affect the interface to obtain police intelligence information，because police intelligence will be associated with this field
                //GetAlarm911Treatments(string ID)
                LoggerManager.Logger.Info(string.Format("Add Record to DisposeRecord MDVR_Core_SN:{0},Alarm_Time:{1}", info.MdvrCoreId, info.AlarmTime));
                _alarmRepository.AddECU911Dispose(context, new ECU911Dispose() { AlarmID = realAlarmInfo.Id, AlarmAddress = realAlarmInfo.AlarmAddress, ForwardDest = realAlarmInfo.DistrictCode });
                LoggerManager.Logger.Info(string.Format("Forwarding Alarm To ECU-911,MDVR_Core_SN:{0},Alarm_Time:{1}", info.MdvrCoreId, info.AlarmTime));
                return ARADSTreatment.SendAlarmInfoToVideoAlarmAsync(realAlarmInfo).Result;

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("ForwardingAlarmToECU911", ex);
                return false;
            }
        }

        /// <summary>
        /// fill model,if the msg can,t find in the local cache ,the function 
        /// in this function will find in database and then insert into the local cache,
        /// if can,t find in the local cache and database will return false and logger warn 
        /// </summary>
        /// <param name="AlarmModel"></param>
        private static bool FillAlarmModel(Gsafety.PTMS.Message.Contract.Data.AlarmInfo AlarmModel, out string ruleKey)
        {
            var suiteInfo = MdvrIdkeySuiteCache.GetValue(AlarmModel.MdvrCoreId);
            if (suiteInfo != null)
            {
                AlarmModel.SuiteID = suiteInfo.SuiteId;
                AlarmModel.SuiteStatus = suiteInfo.Status;
                AlarmModel.SuiteInfoID = suiteInfo.SuiteInfoID;
                AlarmModel.VehicleId = suiteInfo.VehicleId;
                AlarmModel.DistrictCode = suiteInfo.DistrictCode;
                AlarmModel.ProvinceName = suiteInfo.ProvinceName;
                AlarmModel.ProvinceCode = suiteInfo.ProvinceCode;
                AlarmModel.CityName = suiteInfo.CityName;

                AlarmModel.VehicleType = suiteInfo.VehicleType;
                AlarmModel.VehicleSn = suiteInfo.VehicleSn;
                AlarmModel.BrandModel = suiteInfo.BrandModel;
                AlarmModel.Mobile = suiteInfo.Mobile;
                AlarmModel.OperationLincese = suiteInfo.OperationLincese;
                AlarmModel.Owner = suiteInfo.Owner;
                AlarmModel.StartYear = suiteInfo.StartYear;
                //the rule route key
                ruleKey = string.Format(".{0}.{1}", suiteInfo.ProvinceCode, suiteInfo.CityCode);
                return true;
            }
            else
            {
                LoggerManager.Logger.Warn(string.Format("Fill alarm data from cache when the entity is emply,MdvrCoreSN:{0}", AlarmModel.MdvrCoreId));
                ruleKey = null;
                return false;
            }
        }

        /// <summary>
        /// get the alarm address from gis interface
        /// </summary>
        /// <param name="model"></param>
        [Obsolete]
        private static void FillAlarmAddress(Gsafety.PTMS.Message.Contract.Data.AlarmInfo model)
        {
            var webClient = new System.Net.WebClient();
            try
            {
                string addressUrl = string.Format(ConfigHelper.ArcGISUrl + "LOC_GetAddress/GeocodeServer/reverseGeocode?location={0}%2C{1}&distance=100000&outSR=&f=pjson", GISConvertHelper.GetLongitude(model.Longitude), GISConvertHelper.GetLatitude(model.Latitude));
                webClient.Proxy = null;
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers["Content-Type"] = "application/json";
                string result = webClient.DownloadString(new System.Uri(addressUrl));

                var arcGisResult = ConvertHelper.ConvertJsonToModel<ArcGisResult>(result);
                if (arcGisResult.address != null)
                {
                    model.AlarmAddress = arcGisResult.address.State;
                    LoggerManager.Logger.Info(string.Format("Get alarm address for arcGis ,address:{0}", arcGisResult.address.State));
                }
                else
                {
                    LoggerManager.Logger.Warn(string.Format("Get alarm address for arcGis is empty! Longitude:{0},Latitude:{1}", GISConvertHelper.GetLongitude(model.Longitude), GISConvertHelper.GetLatitude(model.Latitude)));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex + "Longitude:" + GISConvertHelper.GetLongitude(model.Longitude) + "Latitude:" + GISConvertHelper.GetLatitude(model.Latitude));
            }
            finally
            {
                webClient.Dispose();
            }
        }

        /// <summary>
        /// get the alarm distance code form gis interface
        /// </summary>
        /// <param name="model"></param>
        [Obsolete]
        private static void FillAlarmProv(Gsafety.PTMS.Message.Contract.Data.AlarmInfo model)
        {
            var webClient = new System.Net.WebClient();
            try
            {
                string url = string.Format(ConfigHelper.ArcGISUrl + "LOC_GetProv/GeocodeServer/reverseGeocode?location={0}%2C{1}&distance=100000&outSR=&f=pjson", GISConvertHelper.GetLongitude(model.Longitude), GISConvertHelper.GetLatitude(model.Latitude));
                webClient.Proxy = null;
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers["Content-Type"] = "application/json";
                string result = webClient.DownloadString(new System.Uri(url));

                var arcGisResult = ConvertHelper.ConvertJsonToModel<ArcGisResult>(result);
                if (arcGisResult.address != null)
                {
                    model.AlarmAddressCode = arcGisResult.address.State;
                    LoggerManager.Logger.Info(string.Format("Get alarm prov for arcGis ,prov:{0}", arcGisResult.address.State));
                }
                else
                {
                    LoggerManager.Logger.Warn(string.Format("Get alarm prov for arcGis is empty! Longitude:{0},Latitude:{1}", GISConvertHelper.GetLongitude(model.Longitude), GISConvertHelper.GetLatitude(model.Latitude)));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex + " Longitude:" + GISConvertHelper.GetLongitude(model.Longitude) + "Latitude:" + GISConvertHelper.GetLatitude(model.Latitude));
            }
            finally
            {
                webClient.Dispose();
            }
        }
    }
}
