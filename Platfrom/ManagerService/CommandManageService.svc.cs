using Gsafety.Common.Logging;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Manager.Contract;
using Gsafety.PTMS.Manager.Contract.Data.CommandManage;
using Gsafety.PTMS.Manager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Gsafety.PTMS.Manager.Service
{
    public class CommandManageService : BaseService, ICommandManageService
    {
        CommandManageRepository dbHelper = new CommandManageRepository();
        #region GPS
        public SingleMessage<bool> CheckGpsExist(string strGpsName)
        {
            try
            {
                Info("CheckGpsExist");
                Info("strGpsName:" + Convert.ToString(strGpsName));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.BeExistGpsName(context, strGpsName);
                }


                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        public SingleMessage<bool> CheckGpsidExist(string strGpsName, string id)
        {
            try
            {
                Info("CheckGpsidExist");
                Info("strGpsName:" + Convert.ToString(strGpsName) + ";" + "id:" + Convert.ToString(id));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.BeExistGpsNameid(context, strGpsName, id);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        public MultiMessage<GpsSettingInfo> GpsSetting(string gps_RuleName, short? gps_UploadType, PagingInfo pageInfo)
        {
            try
            {
                Info("GpsSetting");
                Info("gps_RuleName:" + Convert.ToString(gps_RuleName) + ";" + "gps_UploadType:" + Convert.ToString(gps_UploadType) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                var temp = new MultiMessage<GpsSettingInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GpsSetting(context, gps_RuleName, gps_UploadType, pageInfo);
                }

                MultiMessage<GpsSettingInfo> result = new MultiMessage<GpsSettingInfo> { IsSuccess = true, Result = temp.Result, TotalRecord = temp.TotalRecord };
                Log<GpsSettingInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<GpsSettingInfo>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        public SingleMessage<bool> GpsSettingAdd(GpsSettingInfo GpsSettAdd)
        {
            try
            {
                Info("GpsSettingAdd");
                Info("GpsSettAdd:" + Convert.ToString(GpsSettAdd));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GpsSettingAdd(context, GpsSettAdd);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<bool> ModifyGpsSettings(GpsSettingInfo GpsSettModify)
        {
            try
            {
                Info("ModifyGpsSettings");
                Info("GpsSettModify:" + Convert.ToString(GpsSettModify));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.ModifyGpsSettings(context, GpsSettModify);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<bool> DeleteGpsSetting(string ruleName)
        {
            try
            {
                Info("DeleteGpsSetting");
                Info("ruleName:" + Convert.ToString(ruleName));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.DeleteGpsSetting(context, ruleName);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }

        }
        public SingleMessage<GpsSettingInfo> GetDefaultGpsSetting()
        {
            try
            {
                Info("GetDefaultGpsSetting");
                var temp = new GpsSettingInfo();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GetDefalutGpsRule(context);
                }

                SingleMessage<GpsSettingInfo> result = new SingleMessage<GpsSettingInfo> { Result = temp, IsSuccess = true };
                Log<GpsSettingInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<GpsSettingInfo> { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        #endregion

        #region Alarm

        public SingleMessage<bool> CheckAlarmExist(string strAlarmName)
        {
            try
            {
                Info("CheckAlarmExist");
                Info("strAlarmName:" + Convert.ToString(strAlarmName));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.BeExistAlarmName(context, strAlarmName);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        public SingleMessage<bool> CheckAlarmidExist(string strAlarmName, string id)
        {
            try
            {
                Info("CheckAlarmidExist");
                Info("strAlarmName:" + Convert.ToString(strAlarmName) + ";" + "id:" + Convert.ToString(id));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.BeExistAlarmidName(context, strAlarmName, id);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        public MultiMessage<AlarmSettingRules> AlarmSetting(string ruleName, PagingInfo pageInfo)
        {
            try
            {
                Info("AlarmSetting");
                Info("ruleName:" + Convert.ToString(ruleName) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                var temp = new MultiMessage<AlarmSettingRules>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.AlarmSetting(context, ruleName, pageInfo);
                }


                MultiMessage<AlarmSettingRules> result = new MultiMessage<AlarmSettingRules> { Result = temp.Result, IsSuccess = true, TotalRecord = temp.TotalRecord };
                Log<AlarmSettingRules>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<AlarmSettingRules> { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        public SingleMessage<bool> AlarmSettingAdd(AlarmSettingRules alarmAdd)
        {
            try
            {
                Info("AlarmSettingAdd");
                Info("alarmAdd:" + Convert.ToString(alarmAdd));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.AlarmSettingAdd(context, alarmAdd);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }

        }

        public SingleMessage<bool> ModifyAlarmSettings(AlarmSettingRules alarmModify)
        {
            try
            {
                Info("ModifyAlarmSettings");
                Info("alarmModify:" + Convert.ToString(alarmModify));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.ModifyAlarmSettings(context, alarmModify);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }

        }

        public SingleMessage<bool> DeleteAlarmSetting(string ruleName)
        {
            try
            {
                Info("DeleteAlarmSetting");
                Info("ruleName:" + Convert.ToString(ruleName));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.DeleteAlarmSetting(context, ruleName);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }

        }

        public SingleMessage<AlarmSettingRules> GetDefaultAlarmSetting()
        {
            try
            {
                Info("GetDefaultAlarmSetting");
                var temp = new AlarmSettingRules();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GetDefalutAlarmRule(context);
                }

                SingleMessage<AlarmSettingRules> result = new SingleMessage<AlarmSettingRules> { Result = temp, IsSuccess = true };
                Log<AlarmSettingRules>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<AlarmSettingRules> { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        #endregion

        #region Temperature Setting
        public SingleMessage<bool> AddTemperatureRule(TemperatureRuleInfo info)
        {
            try
            {
                Info("AddTemperatureRule");
                Info("info:" + Convert.ToString(info));
                bool temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.AddTemperatureRule(context, info);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<bool> IsExistTemperatureRule(string ruleName)
        {
            try
            {
                Info("IsExistTemperatureRule");
                Info("ruleName:" + Convert.ToString(ruleName));
                bool temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.IsExistTemperatureRule(context, ruleName);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        public SingleMessage<bool> IsExistTemperatureRuleForUpdate(string ruleName, string id)
        {
            try
            {
                Info("IsExistTemperatureRuleForUpdate");
                Info("ruleName:" + Convert.ToString(ruleName) + ";" + "id:" + Convert.ToString(id));
                bool temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.IsExistTemperatureRule(context, ruleName, id);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        public MultiMessage<TemperatureRuleInfo> GetAllTemperatureSettingInfo(string userName, string ruleName, PagingInfo pageInfo)
        {
            try
            {
                Info("GetAllTemperatureSettingInfo");
                Info("userName:" + Convert.ToString(userName) + ";" + "ruleName:" + Convert.ToString(ruleName) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<TemperatureRuleInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GetAllTemperatureSettingInfo(context, userName, ruleName, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<TemperatureRuleInfo> result = new MultiMessage<TemperatureRuleInfo> { Result = temp, TotalRecord = totalCount };
                Log<TemperatureRuleInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<TemperatureRuleInfo>() { Result = null, TotalRecord = 0, ExceptionMessage = ex };
            }
        }
        public SingleMessage<bool> CheckSettingUsing(CheckType type, string ruleId)
        {
            try
            {
                Info("CheckSettingUsing");
                Info("type:" + Convert.ToString(type) + ";" + "ruleId:" + Convert.ToString(ruleId));
                bool temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.CheckSettingUsing(context, type, ruleId);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;

            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }
        }


        public SingleMessage<bool> DeleteTemperatureRule(string ruleId)
        {
            try
            {
                Info("DeleteTemperatureRule");
                Info("ruleId:" + Convert.ToString(ruleId));
                bool temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.DeleteTemperatureRule(context, ruleId);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        public SingleMessage<bool> UpdateTemperatureRule(TemperatureRuleInfo info)
        {
            try
            {
                Info("UpdateTemperatureRule");
                Info("info:" + Convert.ToString(info));
                bool temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.UpdateTemperatureRule(context, info);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<TemperatureRuleInfo> GetDefaultTemperatureSetting()
        {
            try
            {
                Info("GetDefaultTemperatureSetting");
                var temp = new TemperatureRuleInfo();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GetDefalutTemperatureRule(context);
                }

                SingleMessage<TemperatureRuleInfo> result = new SingleMessage<TemperatureRuleInfo> { Result = temp, IsSuccess = true };
                Log<TemperatureRuleInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<TemperatureRuleInfo> { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        #endregion

        #region AbnormalDoorRule
        public SingleMessage<bool> DeleteAbnormalDoorRule(string ruleId)
        {
            try
            {
                Info("DeleteAbnormalDoorRule");
                Info("ruleId:" + Convert.ToString(ruleId));
                bool temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.DeleteAbnormalDoorRule(context, ruleId);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<bool> AddAbnormalDoorRule(AbnormalDoorRuleInfo info)
        {
            try
            {
                Info("AddAbnormalDoorRule");
                Info("info:" + Convert.ToString(info));
                bool temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.AddAbnormalDoorRule(context, info);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<bool> IsExistAbnormalDoorRule(string ruleName)
        {
            try
            {
                Info("IsExistAbnormalDoorRule");
                Info("ruleName:" + Convert.ToString(ruleName));
                bool temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.IsExistAbnormalDoorRule(context, ruleName);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        public SingleMessage<bool> IsExistAbnormalDoorRuleForUpdate(string ruleName, string id)
        {
            try
            {
                Info("IsExistAbnormalDoorRuleForUpdate");
                Info("ruleName:" + Convert.ToString(ruleName) + ";" + "id:" + Convert.ToString(id));
                bool temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.IsExistAbnormalDoorRule(context, ruleName, id);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        public MultiMessage<AbnormalDoorRuleInfo> GetAllAbnormalDoorRuleInfo(string userName, string ruleName, PagingInfo pageInfo)
        {
            try
            {
                Info("GetAllAbnormalDoorRuleInfo");
                Info("userName:" + Convert.ToString(userName) + ";" + "ruleName:" + Convert.ToString(ruleName) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                int totalCount = 0;
                var temp = new List<AbnormalDoorRuleInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GetAllAbnormalDoorRuleInfo(context, userName, ruleName, pageInfo.PageSize, pageInfo.PageIndex, out totalCount);
                }

                MultiMessage<AbnormalDoorRuleInfo> result = new MultiMessage<AbnormalDoorRuleInfo> { Result = temp, TotalRecord = totalCount };
                Log<AbnormalDoorRuleInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<AbnormalDoorRuleInfo>() { Result = null, TotalRecord = 0, ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> CheckAbnormalDoorRuleUsing(string ruleId)
        {
            try
            {
                Info("CheckAbnormalDoorRuleUsing");
                Info("ruleId:" + Convert.ToString(ruleId));
                bool temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.CheckAbnormalDoorRuleUsing(context, ruleId);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { IsSuccess = true, Result = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        public SingleMessage<bool> UpdateExistAbnormalDoorRule(AbnormalDoorRuleInfo info)
        {
            try
            {
                Info("UpdateExistAbnormalDoorRule");
                Info("info:" + Convert.ToString(info));
                bool temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.UpdateAbnormalDoorRule(context, info);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        public SingleMessage<AbnormalDoorRuleInfo> GetDefaultAbnormalDoorRule()
        {
            try
            {
                Info("GetDefaultAbnormalDoorRule");
                var temp = new AbnormalDoorRuleInfo();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GetDefalutAbnormalDoorRule(context);
                }

                SingleMessage<AbnormalDoorRuleInfo> result = new SingleMessage<AbnormalDoorRuleInfo> { Result = temp, IsSuccess = true };
                Log<AbnormalDoorRuleInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<AbnormalDoorRuleInfo> { ExceptionMessage = ex, IsSuccess = false };
            }
        }


        #endregion

        #region ConfigInfo

        public MultiMessage<AlarmSettingRules> GetDefaultAlarmInfo()
        {
            try
            {
                Info("GetDefaultAlarmInfo");
                var temp = new MultiMessage<AlarmSettingRules>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GetDefaultAlarmInfo(context);
                }

                MultiMessage<AlarmSettingRules> result = new MultiMessage<AlarmSettingRules> { Result = temp.Result, IsSuccess = true };
                Log<AlarmSettingRules>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<AlarmSettingRules> { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public MultiMessage<GpsSettingInfo> GetDefaultGpsInfo()
        {
            try
            {
                Info("GetDefaultGpsInfo");
                var temp = new MultiMessage<GpsSettingInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GetDefaultGpsInfo(context);
                }

                MultiMessage<GpsSettingInfo> result = new MultiMessage<GpsSettingInfo> { Result = temp.Result, IsSuccess = true };
                Log<GpsSettingInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<GpsSettingInfo> { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public MultiMessage<AbnormalDoorRuleInfo> GetDefaultAbnormalDoorInfo()
        {
            try
            {
                Info("GetDefaultAbnormalDoorInfo");
                var temp = new MultiMessage<AbnormalDoorRuleInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GetDefaultAbnormalDoorInfo(context);
                }

                MultiMessage<AbnormalDoorRuleInfo> result = new MultiMessage<AbnormalDoorRuleInfo> { Result = temp.Result, IsSuccess = true };
                Log<AbnormalDoorRuleInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<AbnormalDoorRuleInfo> { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public MultiMessage<TemperatureRuleInfo> GetDefaultTemperatureInfo()
        {
            try
            {
                Info("GetDefaultTemperatureInfo");
                var temp = new MultiMessage<TemperatureRuleInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GetDefaultTemperatureInfo(context);
                }

                MultiMessage<TemperatureRuleInfo> result = new MultiMessage<TemperatureRuleInfo> { Result = temp.Result, IsSuccess = true };
                Log<TemperatureRuleInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<TemperatureRuleInfo> { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public MultiMessage<ConfigInfo> ConfigInfo(string vehicleID, string mdvr_ID, PagingInfo pageInfo)
        {
            try
            {
                Info("ConfigInfo");
                Info("vehicleID:" + Convert.ToString(vehicleID) + ";" + "mdvr_ID:" + Convert.ToString(mdvr_ID) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                var temp = new MultiMessage<ConfigInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.ConfigInfo(context, vehicleID, mdvr_ID, pageInfo);
                }

                MultiMessage<ConfigInfo> result = new MultiMessage<ConfigInfo> { Result = temp.Result, IsSuccess = true, TotalRecord = temp.TotalRecord };
                Log<ConfigInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<ConfigInfo> { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public MultiMessage<ConfigDetail> ConfigInfoDetail(string Gps_ruleName, string Temperature_ruleName, string Alarm_ruleName, string AbnormalDoor_ruleName)
        {
            try
            {
                Info("ConfigInfoDetail");
                Info("Gps_ruleName:" + Convert.ToString(Gps_ruleName) + ";" + "Temperature_ruleName:" + Convert.ToString(Temperature_ruleName) + ";" + "Alarm_ruleName:" + Convert.ToString(Alarm_ruleName) + ";" + "AbnormalDoor_ruleName:" + Convert.ToString(AbnormalDoor_ruleName));
                var temp = new List<ConfigDetail>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.ConfigInfoDetail(context, Gps_ruleName, Temperature_ruleName, Alarm_ruleName, AbnormalDoor_ruleName);
                }

                MultiMessage<ConfigDetail> result = new MultiMessage<ConfigDetail> { Result = temp, IsSuccess = true };
                Log<ConfigDetail>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<ConfigDetail> { ExceptionMessage = ex, IsSuccess = false };
            }
        }
        public MultiMessage<GpsSettingInfo> ConfigGps(string Gps_ruleName)
        {
            try
            {
                Info("ConfigGps");
                Info("Gps_ruleName:" + Convert.ToString(Gps_ruleName));
                var temp = new MultiMessage<GpsSettingInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.ConfigGps(context, Gps_ruleName);
                }

                MultiMessage<GpsSettingInfo> result = new MultiMessage<GpsSettingInfo> { Result = temp.Result, IsSuccess = true };
                Log<GpsSettingInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<GpsSettingInfo> { ExceptionMessage = ex, IsSuccess = false };
            }
        }


        public MultiMessage<AbnormalDoorRuleInfo> ConfigAbnormalDoor(string abnormalDoor)
        {
            try
            {
                Info("ConfigAbnormalDoor");
                Info("abnormalDoor:" + Convert.ToString(abnormalDoor));
                var temp = new MultiMessage<AbnormalDoorRuleInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.ConfigAbnormalDoor(context, abnormalDoor);
                }

                MultiMessage<AbnormalDoorRuleInfo> result = new MultiMessage<AbnormalDoorRuleInfo> { Result = temp.Result, IsSuccess = true };
                Log<AbnormalDoorRuleInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<AbnormalDoorRuleInfo> { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public MultiMessage<AlarmSettingRules> ConfigAlarmSettings(string alarmRuleName)
        {
            try
            {
                Info("ConfigAlarmSettings");
                Info("alarmRuleName:" + Convert.ToString(alarmRuleName));
                var temp = new MultiMessage<AlarmSettingRules>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.ConfigAlarmSettings(context, alarmRuleName);
                }

                MultiMessage<AlarmSettingRules> result = new MultiMessage<AlarmSettingRules> { Result = temp.Result, IsSuccess = true };
                Log<AlarmSettingRules>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<AlarmSettingRules> { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public MultiMessage<TemperatureRuleInfo> ConfigTemperatureSettings(string temperatureRuleName)
        {
            try
            {
                Info("ConfigTemperatureSettings");
                Info("temperatureRuleName:" + Convert.ToString(temperatureRuleName));
                var temp = new MultiMessage<TemperatureRuleInfo>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.ConfigTemperatureSettings(context, temperatureRuleName);
                }

                MultiMessage<TemperatureRuleInfo> result = new MultiMessage<TemperatureRuleInfo> { Result = temp.Result, IsSuccess = true };
                Log<TemperatureRuleInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<TemperatureRuleInfo> { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        #endregion

        #region CurrentSettingRule
        public MultiMessage<CurrentSettingRuleInfo> GetAllCurrentSettingRuleInfo(string userName, string vehicle, PagingInfo pageInfo)
        {
            try
            {
                Info("GetAllCurrentSettingRuleInfo");
                Info("userName:" + Convert.ToString(userName) + ";" + "vehicle:" + Convert.ToString(vehicle) + ";" + "pageInfo:" + Convert.ToString(pageInfo));

                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = dbHelper.GetAllCurrentSettingRuleInfo(context, userName, vehicle, pageInfo);
                    MultiMessage<CurrentSettingRuleInfo> result = new MultiMessage<CurrentSettingRuleInfo> { Result = temp.Result, IsSuccess = true, TotalRecord = temp.TotalRecord };
                    Log<CurrentSettingRuleInfo>(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<CurrentSettingRuleInfo> { Result = null, IsSuccess = false };
            }
        }
        #endregion

        #region RuleSettingLog
        public MultiMessage<CurrentSettingRuleInfo> GetAllRuleSettingLogInfo(string userName, string vehicle, PagingInfo pageInfo)
        {
            try
            {
                Info("GetAllRuleSettingLogInfo");
                Info("userName:" + Convert.ToString(userName) + ";" + "vehicle:" + Convert.ToString(vehicle) + ";" + "pageInfo:" + Convert.ToString(pageInfo));

                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = dbHelper.GetAllRuleSettingLogInfo(context, userName, vehicle, pageInfo);
                    MultiMessage<CurrentSettingRuleInfo> result = new MultiMessage<CurrentSettingRuleInfo> { Result = temp.Result, IsSuccess = true, TotalRecord = temp.TotalRecord };
                    Log<CurrentSettingRuleInfo>(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<CurrentSettingRuleInfo> { Result = null, IsSuccess = false };
            }
        }


        #endregion

        public SingleMessage<VehicleRuleRelation> GetAllVehicleRuleRelation(string ruleid, RuleType type)
        {
            try
            {
                Info("GetAllVehicleRuleRelation");
                Info("ruleid:" + Convert.ToString(ruleid) + ";" + "type:" + Convert.ToString(type));
                SingleMessage<VehicleRuleRelation> result = new SingleMessage<VehicleRuleRelation>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = dbHelper.GetVehicleRuleRelation(context, ruleid, type);
                }

                Log<VehicleRuleRelation>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<VehicleRuleRelation> { Result = null, IsSuccess = false };
            }
        }
    }
}
