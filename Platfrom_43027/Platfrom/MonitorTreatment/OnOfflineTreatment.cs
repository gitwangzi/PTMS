/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c0c9440b-aa8b-481a-8501-e25a67428315      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.MonitorTreatment
/////    Project Description:    
/////             Class Name: OnlineTreatment
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/24 13:36:44
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/24 13:36:44
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
using Gsafety.PTMS.Monitor.Repository;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Data;

namespace Gsafety.PTMS.Analysis.MonitorTreatment
{
    public static class OnOfflineTreatment
    {
        public static MonitorRepository _onitorRepository;
        private static SecuritySuiteRepository _suiteInfoRepository;

        static OnOfflineTreatment()
        {
            _suiteInfoRepository = new SecuritySuiteRepository();
            _onitorRepository = new MonitorRepository();
        }

        public static ReturnInfo Authenticate(PTMSEntities context, byte[] bytes)
        {
            ReturnInfo returnInfo = new ReturnInfo();

            //byte[] -> json
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info(string.Format("Get Authenticate info:{0}", str));
            //json -> entity
            Authenticate authenticate = JsonHelper.FromJsonString<Authenticate>(str);

            AuthenticateResponse response = new AuthenticateResponse();

            if (authenticate != null)
            {

                LoggerManager.Logger.Info(string.Format("Before Check Authenticate by database!UID:{0}", str));
                response = _onitorRepository.Authenticate(context, authenticate);
                LoggerManager.Logger.Info(string.Format("After Check Authenticate by database!UID:{0}", str));

                if (response != null)
                {
                    //entity -> json
                    string s = JsonHelper.ToJsonString(response);
                    LoggerManager.Logger.Info(string.Format("RegisterResponse !UID:{0}", s));
                    //json -> byte[]
                    var msg = System.Text.UTF8Encoding.UTF8.GetBytes(s);

                    if (msg != null && msg.Length > 0)
                    {
                        returnInfo.Message = msg;
                    }
                }
            }
            else
            {
                LoggerManager.Logger.Warn(string.Format("Converted Authenticate info to entity is null,string:{0}", str));
            }
            return returnInfo;
        }

        public static ReturnInfo GPSAuthenticate(PTMSEntities context, byte[] bytes)
        {
            ReturnInfo returnInfo = new ReturnInfo();

            //byte[] -> json
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info(string.Format("Get GPSAuthenticate info:{0}", str));
            //json -> entity
            Authenticate authenticate = JsonHelper.FromJsonString<Authenticate>(str);

            AuthenticateResponse response = new AuthenticateResponse();

            if (authenticate != null)
            {

                LoggerManager.Logger.Info(string.Format("Before Check GPSAuthenticate by database!UID:{0}", str));
                response = _onitorRepository.GPSAuthenticate(context, authenticate);
                LoggerManager.Logger.Info(string.Format("After Check GPSAuthenticate by database!UID:{0}", str));

                if (response != null)
                {
                    //entity -> json
                    string s = JsonHelper.ToJsonString(response);
                    LoggerManager.Logger.Info(string.Format("RegisterResponse !UID:{0}", s));
                    //json -> byte[]
                    var msg = System.Text.UTF8Encoding.UTF8.GetBytes(s);

                    if (msg != null && msg.Length > 0)
                    {
                        returnInfo.Message = msg;
                    }
                }
            }
            else
            {
                LoggerManager.Logger.Warn(string.Format("Converted Authenticate info to entity is null,string:{0}", str));
            }
            return returnInfo;
        }

        //public static ReturnInfo OnOffline(PTMSEntities context, byte[] bytes, out string mdvrid, out bool isonline)
        //{
        //    ReturnInfo returnInfo = new ReturnInfo();
        //    string ruleKey = null;
        //    //byte[] -> json
        //    string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
        //    LoggerManager.Logger.Info(string.Format("Get OnOffline info:{0}", str));
        //    //json -> entity
        //    OnOffLineData tempmodel = JsonHelper.FromJsonString<OnOffLineData>(str);


        //    if (tempmodel != null)
        //    {
        //        OnOfflineEx model = new OnOfflineEx();
        //        model.MdvrCoreId = tempmodel.UID;
        //        model.IsOnline = Convert.ToInt32(tempmodel.IsOnline);
        //        string[] fileds = tempmodel.OnOffLineTime.Split("-".ToCharArray());
        //        model.OnOffLineTime = DateTime.Parse(fileds[0] + "-" + fileds[1] + "-" + fileds[2] + " " + fileds[3] + ":" + fileds[4] + ":" + fileds[5]);

        //        mdvrid = tempmodel.UID;
        //        isonline = model.IsOnline == 1 ? true : false;
        //        model.MdvrCoreId = tempmodel.UID;
        //        if (FillOnOfflineModel(model, out ruleKey))
        //        {
        //            LoggerManager.Logger.Info(string.Format("Before Add OnOffline to database!UID:{0}", str));
        //            //check && storge && response 
        //            _onitorRepository.AddSuiteOnOffline(context, model);
        //            LoggerManager.Logger.Info(string.Format("After Add OnOffline to database!UID:{0}", str));
        //            returnInfo.RuleKey = ruleKey;
        //            returnInfo.Message = ConvertHelper.ObjectToBytes(model);
        //        }
        //    }
        //    else
        //    {
        //        mdvrid = string.Empty;
        //        isonline = false;
        //        LoggerManager.Logger.Warn(string.Format("Converted register to entity is null,string:{0}", str));
        //    }
        //    return returnInfo;
        //}

        //private static bool FillOnOfflineModel(OnOfflineEx model, out string ruleKey)
        //{
        //    var suiteInfo = MdvrIdkeySuiteCache.GetValue(model.MdvrCoreId);
        //    if (suiteInfo != null)
        //    {
        //        model.VehicleId = suiteInfo.VehicleId;
        //        model.SuiteInfoID = suiteInfo.SuiteInfoID;
        //        model.ClientId = suiteInfo.ClientId;
        //        ruleKey = string.Format(".{0}.{1}.{2}", suiteInfo.ProvinceCode, suiteInfo.CityCode, suiteInfo.OrgnizationId);
        //        return true;
        //    }
        //    else
        //    {
        //        LoggerManager.Logger.Warn(string.Format("Fill OnOffline data from cache when the entity is emply,MdvrCoreSN:{0}", model.UID));
        //        ruleKey = null;
        //        return false;
        //    }
        //}

        public static ReturnInfo QueryParaResponse(PTMSEntities context, byte[] bytes)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            string ruleKey = null;
            //byte[] -> json
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info(string.Format("Get QueryParaResponse info:{0}", str));
            //json -> entity
            QueryParaResponseEx model = JsonHelper.FromJsonString<QueryParaResponseEx>(str);

            if (model != null)
            {
                //对参数进行处理
                //if (FillParamModel(model, out ruleKey))
                {
                    LoggerManager.Logger.Info(string.Format("Before Add RegisterInfo to database!UID:{0}", str));
                    //check && storge && response 
                    //_alarmRepository.AddAlarm(context, model);
                    LoggerManager.Logger.Info(string.Format("After Add RegisterInfo to database!UID:{0}", str));
                    returnInfo.RuleKey = ruleKey;
                    returnInfo.Message = ConvertHelper.ObjectToBytes(model);
                }
            }
            else
            {
                LoggerManager.Logger.Warn(string.Format("Converted QueryParaResponse to entity is null,string:{0}", str));
            }
            return returnInfo;
        }

        public static ReturnInfo Online(PTMSEntities context, byte[] bytes)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            string ruleKey = null;

            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info(string.Format("get online data:{0}", str));

            //var model = new OnOffline(str);
            //if (model != null)
            //{
            //    if (FillOnOfflineModel(model, out ruleKey))
            //    {
            //        LoggerManager.Logger.Info(string.Format("Add Online Record and Update Working Suite by MDVR:{0}", model.MdvrCoreId));
            //        _onitorRepository.AddOnline(context,model);
            //        returnInfo.RuleKey = ruleKey;
            //        returnInfo.Message = ConvertHelper.ObjectToBytes(model);
            //    }
            //}
            //else
            //{
            //    LoggerManager.Logger.Warn(string.Format("Converted online to entity is empty,string:{0}", str));
            //}

            return returnInfo;
        }

        public static ReturnInfo OfflineA1(PTMSEntities context, byte[] bytes)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            string ruleKey = null;

            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info(string.Format("get offlinea1 data:{0}", str));

            //var model = new OnOffline(str);
            //if (model != null)
            //{
            //    if (FillOnOfflineModel(model, out ruleKey))
            //    {
            //        LoggerManager.Logger.Info(string.Format("Add Online Record and Update Working Suite by MDVR:{0}", model.MdvrCoreId));
            //        _onitorRepository.AddOnline(context,model);
            //        returnInfo.RuleKey = ruleKey;
            //        returnInfo.Message = ConvertHelper.ObjectToBytes(model);
            //    }
            //}
            //else
            //{
            //    LoggerManager.Logger.Warn(string.Format("Converted offlinea1 to entity is empty,string:{0}", str));
            //}

            return returnInfo;
        }

        public static ReturnInfo OfflineA2(PTMSEntities context, byte[] bytes)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            string ruleKey = null;

            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info(string.Format("get offlinea2 data:{0}", str));

            //var model = new OnOffline(str);
            //if (model != null)
            //{
            //    if (FillOnOfflineModel(model, out ruleKey))
            //    {
            //        LoggerManager.Logger.Info(string.Format("Add Online Record and Update Working Suite by MDVR:{0}", model.MdvrCoreId));
            //        _onitorRepository.AddOnline(context,model);
            //        returnInfo.RuleKey = ruleKey;
            //        returnInfo.Message = ConvertHelper.ObjectToBytes(model);
            //    }
            //}
            //else
            //{
            //    LoggerManager.Logger.Warn(string.Format("Converted offlineA2 to entity is empty,string:{0}", str));
            //}

            return returnInfo;
        }

        //private static bool FillOnOfflineModel(OnOffline model, out string ruleKey)
        //{
        //    var suiteInfo = MdvrIdkeySuiteCache.GetValue(model.MdvrCoreId);
        //    if (suiteInfo != null)
        //    {
        //        //model.SuiteInfoId = suiteInfo.SuiteInfoID;
        //        ruleKey = string.Format(".{0}.{1}", suiteInfo.ProvinceCode, suiteInfo.CityCode);
        //        return true;
        //    }
        //    else
        //    {
        //        LoggerManager.Logger.Warn(string.Format("Fill OnOffline data from cache when the entity is emply,MdvrCoreSN:{0}", model.MdvrCoreId));
        //        ruleKey = null;
        //        return false;
        //    }
        //}
    }
}
