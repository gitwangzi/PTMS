/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 529ad430-b1b0-4cf8-aaa4-79345e4ebfbd      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-GUOH
/////                 Author: TEST(guoh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Alert
/////    Project Description:    
/////             Class Name: AlertTreatment
/////          Class Version: v1.0.0.0
/////            Create Time: 8/24/2013 1:52:56 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/24/2013 1:52:56 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Alarm.Repository;
using Gsafety.PTMS.Alert.Repository;
using Gsafety.PTMS.Analysis.Cache;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.PTMS.Traffic.Repository;
using Gsafety.PTMS.DBEntity;
using System.Collections;
using Gsafety.PTMS.Common.Data;

namespace Gsafety.PTMS.Analysis.Alert
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
            msgID = GetMsgId(ba);
            if (ba != null)
            {
                BusinessAlertEx model = new BusinessAlertEx();
                
                model.MdvrCoreId = ba.GpsInfo.UID;
                if (FillAlertModel(model, out ruleKey))
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
                model.MdvrCoreId = da.GpsInfo.UID;
                if (FillAlertModel(model, out ruleKey))
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

        private static bool FillAlertModel(object alertModel, out string ruleKey)
        {
            if (alertModel is BusinessAlertEx)
            {
                BusinessAlertEx current = alertModel as BusinessAlertEx;
                var suiteInfo = MdvrIdkeySuiteCache.GetValue(current.MdvrCoreId);
                if (suiteInfo != null)
                {
                    current.SuiteInfoID = suiteInfo.SuiteInfoID;
                    current.SuiteID = suiteInfo.SuiteId;
                    current.DistrictCode = suiteInfo.DistrictCode;
                    current.SuiteStatus = Convert.ToInt16(suiteInfo.Status);
                    current.VehicleId = suiteInfo.VehicleId;
                    current.Province = suiteInfo.ProvinceName;
                    current.City = suiteInfo.CityName;
                    current.ClientId = suiteInfo.ClientId;
                   
                    ruleKey = string.Format(".{0}.{1}.{2}", suiteInfo.ProvinceCode, suiteInfo.CityCode, suiteInfo.OrgnizationId);
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
                var suiteInfo = MdvrIdkeySuiteCache.GetValue(current.MdvrCoreId);
                if (suiteInfo != null)
                {
                    current.SuiteInfoId = suiteInfo.SuiteInfoID;
                    current.SuiteId = suiteInfo.SuiteId;
                    current.DistrictCode = suiteInfo.DistrictCode;
                    current.SuiteStatus = Convert.ToInt16(suiteInfo.Status);
                    current.VehicleId = suiteInfo.VehicleId;
                    current.Province = suiteInfo.ProvinceName;
                    current.City = suiteInfo.CityName;
                    current.ClientId = suiteInfo.ClientId;
                    ruleKey = string.Format(".{0}.{1}.{2}", suiteInfo.ProvinceCode, suiteInfo.CityCode, suiteInfo.OrgnizationId);
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
                type = ((Gsafety.PTMS.Common.Data.DeviceAlertType)(item.AlertType)).ToString();
            }

            return hashAlertRouteKey[type].ToString();
        }

        #region Method

        #endregion

    }
}
