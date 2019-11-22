using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Alarm.Repository;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Enum;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Message.Contract.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.AnalysisLib.Utility
{
    public static class AlarmTreatment
    {
        private static VehicleAlarmRepository _alarmRepository;

        static AlarmTreatment()
        {
            _alarmRepository = new VehicleAlarmRepository();
        }

        public static ReturnInfo AlarmInfo(PTMSEntities context, byte[] bytes)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            string ruleKey = null;
            //byte[] -> json
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info(string.Format("AlarmInfo:{0}", str));
            //json -> entity
            Gsafety.PTMS.Common.Data.AlarmInfo alarminfo = JsonHelper.FromJsonString<Gsafety.PTMS.Common.Data.AlarmInfo>(str);

            if (alarminfo != null)
            {
                AlarmInfoEx model = new AlarmInfoEx();
                model.ID = Guid.NewGuid().ToString();
                model.AlarmGuid = alarminfo.GpsInfo.UID;
                model.MdvrCoreId = alarminfo.GpsInfo.UID;
                model.Latitude = alarminfo.GpsInfo.Latitude;
                model.Longitude = alarminfo.GpsInfo.Longitude;
                model.Speed = alarminfo.GpsInfo.Speed;
                model.Direction = alarminfo.GpsInfo.Direction;
                string datestring = alarminfo.GpsInfo.GpsTime;
                model.GpsValid = alarminfo.GpsInfo.Valid;
                model.AdditionalInfo = alarminfo.AdditionalInfo;

                try
                {
                    DateTime time = DateTime.Parse(datestring);
                    model.GpsTime = time;
                    model.AlarmTime = model.GpsTime;
                }
                catch (Exception)
                {

                }

                model.Source = (short)AlarmSourceEnum.Suite;
                LoggerManager.Logger.Info(string.Format("Before FillAlarmModel"));
                if (FillAlarmModel(context, model, out ruleKey))
                {
                    LoggerManager.Logger.Info(string.Format("Before Add RegisterInfo to database!UID:{0}", str));
                    //check && storge && response 
                    _alarmRepository.AddAlarm(context, model);
                    LoggerManager.Logger.Info(string.Format("After Add RegisterInfo to database!UID:{0}", str));
                    returnInfo.RuleKey = ruleKey;
                    returnInfo.Message = ConvertHelper.ObjectToBytes(model);

                    if (!CacheDataManager.ClientModes.ContainsKey(model.ClientId))
                    {
                        var clientorder = context.BSC_ORDER_CLIENT.FirstOrDefault(n => n.ID == model.ClientId);
                        if (clientorder != null)
                        {
                            lock (CacheDataManager.ClientModes)
                            {
                                CacheDataManager.ClientModes.Add(model.ClientId, clientorder.TANSFER_MODE);
                            }
                        }

                    }

                    if (CacheDataManager.ClientModes.ContainsKey(model.ClientId))
                    {
                        int transfermode = CacheDataManager.ClientModes[model.ClientId];
                        if (transfermode == (int)TansferModeEnum.DirectTransfer)
                        {
                            TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, AlarmRoute.DirectTransferKey + model.VehicleId, returnInfo.Message);
                        }
                    }

                }
            }
            else
            {
                LoggerManager.Logger.Warn(string.Format("Converted register to entity is null,string:{0}", str));
            }
            return returnInfo;
        }

        private static bool FillAlarmModel(PTMSEntities context, AlarmInfoEx alarmModel, out string ruleKey)
        {
            if (CacheDataManager.Suites.ContainsKey(alarmModel.MdvrCoreId))
            {
                SuiteCache suite = CacheDataManager.Suites[alarmModel.MdvrCoreId];

                alarmModel.VehicleId = suite.VEHICLE_ID;
                alarmModel.SuiteInfoID = suite.SUITE_INFO_ID;

                LoggerManager.Logger.Info("Fill Vehicle Info");
                //string districtcode = GetDistrictCodeByGis(alarmModel.Latitude, alarmModel.Longitude);
                //if (districtcode != string.Empty)
                //{
                //    alarmModel.DistrictCode = districtcode;
                //}
                //else
                //{
                    alarmModel.DistrictCode = suite.DISTRICT_CODE;
                //}

                if (alarmModel.DistrictCode.Length == 2)
                {
                    alarmModel.Province = CacheDataManager.Districts[suite.DISTRICT_CODE];
                }
                else if (alarmModel.DistrictCode.Length == 5)
                {
                    alarmModel.City = CacheDataManager.Districts[suite.DISTRICT_CODE];
                    string provicecode = alarmModel.DistrictCode.Substring(0, 2);
                    alarmModel.Province = CacheDataManager.Districts[provicecode];
                }

                alarmModel.VehicleOwner = suite.OWNER;
                alarmModel.OwnerPhone = suite.CONTACT_PHONE;


                LoggerManager.Logger.Info("Fill Suite Info");
                alarmModel.SuiteID = suite.SUITE_ID;
                alarmModel.SuiteStatus = Convert.ToInt16(suite.Status);
                alarmModel.VehicleType = suite.VehicleType;
                alarmModel.VehicleSn = suite.VehicleSn;

                alarmModel.ClientId = suite.CLIENT_ID;
                ruleKey = string.Empty;

                LoggerManager.Logger.Info("Fill Organization Info");
                List<string> organizations = new List<string>();
                organizations.Add(suite.ORGNIZATION_ID);

                alarmModel.Organizations = organizations;

                ruleKey = AlarmRoute.AlarmInfoKey + suite.VEHICLE_ID;
                if (!CacheDataManager.ClientModes.ContainsKey(alarmModel.ClientId))
                {
                    var clientorder = context.BSC_ORDER_CLIENT.FirstOrDefault(n => n.ID == alarmModel.ClientId);
                    if (clientorder != null)
                    {
                        lock (CacheDataManager.ClientModes)
                        {
                            CacheDataManager.ClientModes.Add(alarmModel.ClientId, clientorder.TANSFER_MODE);
                        }
                    }

                }

                if (CacheDataManager.ClientModes.ContainsKey(alarmModel.ClientId))
                {
                    int transfermode = CacheDataManager.ClientModes[alarmModel.ClientId];
                    if (transfermode != (int)TansferModeEnum.DirectTransfer)
                    {
                        LoggerManager.Logger.Info("Assign User To Deal With Alarm");
                        List<RUN_USER_ONLINE> users = new List<RUN_USER_ONLINE>();
                        lock (CacheDataManager.Users)
                        {
                            if (CacheDataManager.OrganizationUser.ContainsKey(suite.ORGNIZATION_ID))
                            {
                                users = CacheDataManager.OrganizationUser[suite.ORGNIZATION_ID].Values.ToList();
                            }
                        }

                        RUN_USER_ONLINE user = null;
                        lock (CacheDataManager.AlarmTime)
                        {
                            user = users.Where(n => !CacheDataManager.AlarmTime.ContainsKey(n.USER_ID)).FirstOrDefault();
                            string userid = string.Empty;
                            if (user != null)
                            {
                                userid = user.USER_ID;
                                CacheDataManager.AlarmTime.Add(user.USER_ID, DateTime.Now);
                            }
                            else
                            {
                                List<string> userids = users.Select(n => n.USER_ID).ToList();
                                var result = from a in CacheDataManager.AlarmTime
                                             where userids.Contains(a.Key)
                                             orderby a.Value
                                             select a.Key;

                                userid = result.FirstOrDefault();

                                if (userid != null)
                                {
                                    CacheDataManager.AlarmTime[userid] = DateTime.Now;
                                }
                            }

                            alarmModel.User = userid;

#if DEBUG
                            var temp = context.USR_GUSER.FirstOrDefault(t => t.ID == userid);
                            if (temp != null)
                            {
                                System.Diagnostics.Debug.WriteLine("分配警情给：" + temp.ACCOUNT + "---" + temp.USER_NAME);
                            }
#endif
                        }
                    }
                }

                return true;
            }
            else
            {
                LoggerManager.Logger.Warn(string.Format("Fill alert data from cache when the entity is emply,MdvrCoreSN:{0}", alarmModel.MdvrCoreId));
                ruleKey = null;
                return false;
            }
        }

        public static ReturnInfo MobileAlarmInfo(PTMSEntities context, byte[] bytes)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            string ruleKey = null;
            //byte[] -> json
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info(string.Format("AlarmInfo:{0}", str));
            //json -> entity
            Gsafety.PTMS.Common.Data.MobileAlarmInfo alarminfo = JsonHelper.FromJsonString<Gsafety.PTMS.Common.Data.MobileAlarmInfo>(str);

            if (alarminfo != null)
            {
                string[] fields = alarminfo.UID.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                AlarmInfoEx model = new AlarmInfoEx();
                model.ID = Guid.NewGuid().ToString();
                model.AlarmGuid = fields[0];
                model.MdvrCoreId = model.AlarmGuid;
                model.Latitude = alarminfo.GpsInfo.Latitude;
                model.Longitude = alarminfo.GpsInfo.Longitude;
                model.Speed = alarminfo.GpsInfo.Speed;
                model.Direction = alarminfo.GpsInfo.Direction;
                string datestring = alarminfo.GpsInfo.GpsTime;
                model.GpsValid = alarminfo.GpsInfo.Valid;
                model.AdditionalInfo = alarminfo.AdditionalInfo;
                model.Keyword = alarminfo.Keyword;
                model.AlarmContent = alarminfo.AlarmContent;
                try
                {
                    DateTime time = DateTime.Parse(datestring);
                    model.GpsTime = time;
                    model.AlarmTime = model.GpsTime;
                }
                catch (Exception)
                {

                }

                model.Source = (short)AlarmSourceEnum.Mobile;
                LoggerManager.Logger.Info(string.Format("Before FillAlarmModel"));
                if (FillMobileAlarmModel(context, model, out ruleKey))
                {
                    LoggerManager.Logger.Info(string.Format("Before Add RegisterInfo to database!UID:{0}", str));
                    //check && storge && response 
                    _alarmRepository.AddAlarm(context, model);
                    LoggerManager.Logger.Info(string.Format("After Add RegisterInfo to database!UID:{0}", str));
                    returnInfo.RuleKey = ruleKey;
                    returnInfo.Message = ConvertHelper.ObjectToBytes(model);

                    if (!CacheDataManager.ClientModes.ContainsKey(model.ClientId))
                    {
                        var clientorder = context.BSC_ORDER_CLIENT.FirstOrDefault(n => n.ID == model.ClientId);
                        if (clientorder != null)
                        {
                            lock (CacheDataManager.ClientModes)
                            {
                                CacheDataManager.ClientModes.Add(model.ClientId, clientorder.TANSFER_MODE);
                            }
                        }
                    }

                    if (CacheDataManager.ClientModes.ContainsKey(model.ClientId))
                    {
                        int transfermode = CacheDataManager.ClientModes[model.ClientId];
                        if (transfermode == (int)TansferModeEnum.DirectTransfer)
                        {
                            TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, AlarmRoute.DirectTransferKey + model.VehicleId, returnInfo.Message);
                        }
                    }
                }
            }
            else
            {
                LoggerManager.Logger.Warn(string.Format("Converted register to entity is null,string:{0}", str));
            }
            return returnInfo;
        }

        private static bool FillMobileAlarmModel(PTMSEntities context, AlarmInfoEx alarmModel, out string ruleKey)
        {
            if (!CacheDataManager.Mobiles.ContainsKey(alarmModel.MdvrCoreId))
            {
                lock (CacheDataManager.Mobiles)
                {
                    CacheDataManager.Mobiles.Clear();
                    foreach (var item in context.RUN_MOBILE_WORKING.ToList())
                    {
                        CacheDataManager.Mobiles.Add(item.MOBILE_NUMBER, item);
                    }
                }
            }
            if (CacheDataManager.Mobiles.ContainsKey(alarmModel.MdvrCoreId))
            {
                RUN_MOBILE_WORKING mobile = CacheDataManager.Mobiles[alarmModel.MdvrCoreId];

                alarmModel.AlarmMobile = mobile.MOBILE_NUMBER;
                alarmModel.VehicleId = mobile.VEHICLE_ID;
                alarmModel.SuiteInfoID = mobile.MOBILE_NUMBER;
                BSC_VEHICLE vehicle = context.BSC_VEHICLE.FirstOrDefault(n => n.VEHICLE_ID == mobile.VEHICLE_ID);
                if (vehicle != null)
                {
                    LoggerManager.Logger.Info("Fill Vehicle Info");
                    //string districtcode = GetDistrictCodeByGis(alarmModel.Latitude, alarmModel.Longitude);
                    //if (districtcode != string.Empty)
                    //{
                    //    alarmModel.DistrictCode = districtcode;
                    //}
                    //else
                    //{
                        alarmModel.DistrictCode = vehicle.DISTRICT_CODE;
                    //}

                    if (alarmModel.DistrictCode.Length == 2)
                    {
                        alarmModel.Province = CacheDataManager.Districts[alarmModel.DistrictCode];
                    }
                    else if (alarmModel.DistrictCode.Length == 5)
                    {
                        alarmModel.City = CacheDataManager.Districts[alarmModel.DistrictCode];
                        string provicecode = alarmModel.DistrictCode.Substring(0, 2);
                        alarmModel.Province = CacheDataManager.Districts[provicecode];
                    }

                    alarmModel.VehicleOwner = vehicle.OWNER;
                    alarmModel.OwnerPhone = vehicle.CONTACT_PHONE;
                    alarmModel.VehicleSn = vehicle.VEHICLE_SN;

                    var type=context.BSC_VEHICLE_TYPE.FirstOrDefault(n => n.ID == vehicle.VEHICLE_TYPE);
                    if (type != null)
                    {
                        alarmModel.VehicleType = type.NAME;
                    }
                }

                LoggerManager.Logger.Info("Fill Suite Info");
                alarmModel.SuiteStatus = Convert.ToInt16(DeviceSuiteStatus.Running);

                alarmModel.ClientId = mobile.CLIENT_ID;
                ruleKey = string.Empty;

                LoggerManager.Logger.Info("Fill Organization Info");
                List<string> organizations = new List<string>();
                organizations.Add(vehicle.ORGNIZATION_ID);

                alarmModel.Organizations = organizations;

                ruleKey = AlarmRoute.AlarmInfoKey + vehicle.VEHICLE_ID;

                LoggerManager.Logger.Info("Assign User To Deal With Alarm");
                List<RUN_USER_ONLINE> users = new List<RUN_USER_ONLINE>();
                lock (CacheDataManager.Users)
                {
                    if (CacheDataManager.OrganizationUser.ContainsKey(vehicle.ORGNIZATION_ID))
                    {
                        users = CacheDataManager.OrganizationUser[vehicle.ORGNIZATION_ID].Values.ToList();
                    }
                }

                RUN_USER_ONLINE user = null;
                lock (CacheDataManager.AlarmTime)
                {
                    user = users.Where(n => !CacheDataManager.AlarmTime.ContainsKey(n.USER_ID)).FirstOrDefault();
                    string userid = string.Empty;
                    if (user != null)
                    {
                        userid = user.USER_ID;
                        CacheDataManager.AlarmTime.Add(user.USER_ID, DateTime.Now);
                    }
                    else
                    {
                        List<string> userids = users.Select(n => n.USER_ID).ToList();
                        var result = from a in CacheDataManager.AlarmTime
                                     where userids.Contains(a.Key)
                                     orderby a.Value
                                     select a.Key;

                        userid = result.FirstOrDefault();

                        if (userid != null)
                        {
                            CacheDataManager.AlarmTime[userid] = DateTime.Now;
                        }

                    }

                    alarmModel.User = userid;
                }

                return true;
            }
            else
            {
                LoggerManager.Logger.Warn(string.Format("Fill alert data from cache when the entity is emply,MdvrCoreSN:{0}", alarmModel.MdvrCoreId));
                ruleKey = null;
                return false;
            }
        }

        //public static string GetDistrictCodeByGis(string latitude, string longitude)
        //{
        //    string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
        //    using (OracleConnection oracleConnection = new OracleConnection(ConnectionString))
        //    {
        //        try
        //        {
        //            string sql = "select CODE from CFG_GIS_SPATIAL m where SDO_WITHIN_DISTANCE(m.SPATIAL_AREA,MDSYS.SDO_GEOMETRY(2001,null,MDSYS.SDO_POINT_TYPE(" + longitude + "," + latitude + ",0),null,null),'distance=0')='TRUE' order by m.CODE desc";
        //            using (OracleCommand cmd = new OracleCommand(sql, oracleConnection))
        //            {
        //                oracleConnection.Open();
        //                OracleDataReader reader = cmd.ExecuteReader();
        //                try
        //                {
        //                    if (reader.Read())
        //                    {
        //                        return reader.GetString(0);
        //                    }
        //                    else
        //                    {
        //                        return string.Empty;
        //                    }
        //                }
        //                finally
        //                {
        //                    reader.Close();
        //                }
        //            }
        //        }
        //        finally
        //        {
        //            oracleConnection.Close();
        //        }
        //    }
        //}
    }
}
