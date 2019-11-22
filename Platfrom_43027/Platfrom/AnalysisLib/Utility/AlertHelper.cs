using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.PTMS.Alert.Repository;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Message.Contract.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.AnalysisLib.Utility
{
    /// <summary>
    /// Alarm processing
    /// </summary>
    public class AlertTreatment
    {
        #region property

        /// <summary>
        /// device alarm data is written to the database operations
        /// </summary>
        private static DeviceAlertRespository deviceAlertRespository;

        /// <summary>
        /// business alarm data written to the database operations
        /// </summary>
        private static BusinessAlertRespository businessAlertRespository;

        #endregion

        public static Hashtable hashAlertRouteKey;

        static AlertTreatment()
        {
            deviceAlertRespository = new DeviceAlertRespository();
            businessAlertRespository = new BusinessAlertRespository();
            hashAlertRouteKey = RouteHelper.GetAlertRouteInfo();
        }

        public static ReturnInfo BusinessAlert(PTMSEntities context, byte[] bytes, out string msgID)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            string ruleKey = null;
            //byte[] -> json
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info(string.Format("Get BusinessAlert info:{0}", str));
            //json -> entity
            BusinessAlert ba = JsonHelper.FromJsonString<BusinessAlert>(str);

            if (ba != null && ba.AlertType < 4)
            {
                msgID = GetMsgId(ba);
                BusinessAlertEx model = new BusinessAlertEx();
                model.Id = Guid.NewGuid().ToString();
                model.MdvrCoreId = ba.GpsInfo.UID;
                model.GpsValid = ba.GpsInfo.Valid;
                try
                {
                    DateTime time = DateTime.Parse(ba.GpsInfo.GpsTime);
                    model.GpsTime = time;
                }
                catch (Exception)
                {

                }

                if (model.GpsTime != DateTime.MinValue)
                {
                    model.AlertTime = model.GpsTime;
                }
                else
                {
                    model.AlertTime = DateTime.Now.ToUniversalTime();
                }

                model.MdvrCoreId = ba.GpsInfo.UID;
                model.Longitude = ba.GpsInfo.Longitude;
                model.Latitude = ba.GpsInfo.Latitude;
                model.Speed = ba.GpsInfo.Speed;
                model.Direction = ba.GpsInfo.Direction;
                model.AdditionalInfo = ba.AdditionalInfo;
                model.AlertType = (short)ba.AlertType;

                if (FillAlertModel(context, model, out ruleKey))
                {
                    LoggerManager.Logger.Info(string.Format("Before Add BusinessAlert to database!UID:{0}", str));
                    businessAlertRespository.AddBusinessAlert(context, model);
                    LoggerManager.Logger.Info(string.Format("After Add BusinessAlert to database!UID:{0}", str));
                    returnInfo.RuleKey = ruleKey;
                    returnInfo.Message = ConvertHelper.ObjectToBytes(model);
                }
            }
            else
            {
                msgID = string.Empty;
                LoggerManager.Logger.Warn(string.Format("Converted BusinessAlert to entity is null,string:{0}", str));
            }
            return returnInfo;
        }

        public static ReturnInfo DeviceAlert(PTMSEntities context, byte[] bytes, out string msgID)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            string ruleKey = null;
            //byte[] -> json
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info(string.Format("Get DeviceAlert info:{0}", str));
            //json -> entity
            DeviceAlert da = JsonHelper.FromJsonString<DeviceAlert>(str);
            msgID = GetMsgId(da);

            if (da != null)
            {
                DeviceAlertEx model = new DeviceAlertEx();
                model.Id = Guid.NewGuid().ToString();
                try
                {
                    DateTime time = DateTime.Parse(da.GpsInfo.GpsTime);
                    model.GpsTime = time;
                }
                catch (Exception)
                {

                }

                if (model.GpsTime != DateTime.MinValue)
                {
                    model.AlertTime = model.GpsTime;
                }
                else
                {
                    model.AlertTime = DateTime.Now.ToUniversalTime();
                }

                model.MdvrCoreId = da.GpsInfo.UID;
                model.Longitude = da.GpsInfo.Longitude;
                model.Latitude = da.GpsInfo.Latitude;
                model.Speed = da.GpsInfo.Speed;
                model.Direction = da.GpsInfo.Direction;
                model.AdditionalInfo = da.AdditionalInfo;
                model.AlertType = (short)da.AlertType;
                model.GpsValid = da.GpsInfo.Valid;
                if (FillAlertModel(context, model, out ruleKey))
                {
                    LoggerManager.Logger.Info(string.Format("Before Add DeviceAlert to database!UID:{0}", str));
                    deviceAlertRespository.AddDeviceAlert(context, model);
                    LoggerManager.Logger.Info(string.Format("After Add DeviceAlert to database!UID:{0}", str));
                    returnInfo.RuleKey = ruleKey;
                    returnInfo.Message = ConvertHelper.ObjectToBytes(model);
                }
            }
            else
            {
                LoggerManager.Logger.Warn(string.Format("Converted DeviceAlert to entity is null,string:{0}", str));
            }
            return returnInfo;
        }

        private static bool FillAlertModel(PTMSEntities context, object alertModel, out string ruleKey)
        {
            if (alertModel is BusinessAlertEx)
            {
                BusinessAlertEx current = alertModel as BusinessAlertEx;
                if (CacheDataManager.Suites.ContainsKey(current.MdvrCoreId))
                {
                    SuiteCache suite = CacheDataManager.Suites[current.MdvrCoreId];
                    current.SuiteInfoID = suite.SUITE_INFO_ID;
                    //current.SuiteID = suiteInfo.SuiteId;

                    current.SuiteStatus = Convert.ToInt16(suite.Status);
                    current.VehicleId = suite.VEHICLE_ID;
                    current.SuiteID = suite.SUITE_ID;

                    if (suite.DISTRICT_CODE.Length == 2)
                    {
                        current.Province = CacheDataManager.Districts[suite.DISTRICT_CODE];
                    }
                    else if (suite.DISTRICT_CODE.Length == 5)
                    {
                        current.City = CacheDataManager.Districts[suite.DISTRICT_CODE];
                        string provicecode = suite.DISTRICT_CODE.Substring(0, 2);
                        current.Province = CacheDataManager.Districts[provicecode];
                    }

                    List<string> organizations = new List<string>();
                    organizations.Add(suite.ORGNIZATION_ID);
                    current.OrganizationId = suite.ORGNIZATION_ID;
                    current.DistrictCode = suite.DISTRICT_CODE;
                    current.Organizations = organizations;


                    current.ClientId = suite.CLIENT_ID;
                    ruleKey = AlertRoute.HandleBusinessAlertKey + current.VehicleId;
                    return true;
                }
                else
                {
                    LoggerManager.Logger.Warn(string.Format("Fill business alert data from cache when the entity is emply,MdvrCoreSN:{0}", current.MdvrCoreId));
                    ruleKey = null;
                    return false;
                }
            }
            else
            {
                DeviceAlertEx current = alertModel as DeviceAlertEx;

                if (CacheDataManager.Suites.ContainsKey(current.MdvrCoreId))
                {
                    SuiteCache suite = CacheDataManager.Suites[current.MdvrCoreId];
                    current.SuiteInfoId = suite.SUITE_INFO_ID;
                    current.SuiteId = suite.SUITE_ID;

                    current.SuiteStatus = Convert.ToInt16(suite.Status);
                    current.VehicleId = suite.VEHICLE_ID;

                    if (suite.DISTRICT_CODE.Length == 2)
                    {
                        current.Province = CacheDataManager.Districts[suite.DISTRICT_CODE];
                    }
                    else if (suite.DISTRICT_CODE.Length == 5)
                    {
                        current.City = CacheDataManager.Districts[suite.DISTRICT_CODE];
                        string provicecode = suite.DISTRICT_CODE.Substring(0, 2);
                        current.Province = CacheDataManager.Districts[provicecode];
                    }

                    current.DistrictCode = suite.DISTRICT_CODE;

                    List<string> organizations = new List<string>();
                    organizations.Add(suite.ORGNIZATION_ID);
                    current.Organizations = organizations;

                    current.ClientId = suite.CLIENT_ID;
                    ruleKey = AlertRoute.HandleDeviceAlertKey + current.VehicleId;
                    return true;
                }
                else
                {
                    LoggerManager.Logger.Warn(string.Format("Fill device alert data from cache when the entity is emply,MdvrCoreSN:{0}", current.MdvrCoreId));
                    ruleKey = null;
                    return false;
                }
            }
        }

        private static string GetMsgId(object model)
        {
            string type = string.Empty;

            if (model is BusinessAlert)
            {
                var item = model as BusinessAlert;
                type = ((BussinessAlertType)item.AlertType).ToString();
            }
            else
            {
                var item = model as DeviceAlert;
                type = ((DeviceAlertType)(item.AlertType)).ToString();
            }

            return hashAlertRouteKey[type].ToString();
        }

        #region Method

        #endregion
    }
}
