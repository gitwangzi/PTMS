/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5f6f9891-b9f9-45b0-8c5e-e497a1eff91b      
/////             clrversion: 4.0.30319.34011
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: CommandManagement
/////    Project Description:    
/////             Class Name: MessageProcessing
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/3/7 05:16:40
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/3/7 05:16:40
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.CommandManagement.Management;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.CommandManagement
{
    public class MessageProcessing
    {
        #region On offline

        /// <summary>
        /// Online
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: MonitorRoute.OriginalOnlineKey)]
        public void ProcessOnline(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessOnline");
                if (string.IsNullOrEmpty(key))
                    return;
                string[] keys = key.Split('.');
                if (keys.Length < 3)
                    return;
                SuiteStatusInfoManage.ChangeOnlineFlag(keys[2], true);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// offline A1
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: MonitorRoute.OriginalOfflineA1Key)]
        public void ProcessOfflineA1(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessOfflineA1");
                //Parsing,filling,offline message conversion
                if (string.IsNullOrEmpty(key))
                    return;
                string[] keys = key.Split('.');
                if (keys.Length < 3)
                    return;
                SuiteStatusInfoManage.ChangeOnlineFlag(keys[2], false);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// offline A2
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: MonitorRoute.OriginalOfflineA2Key)]
        public void ProcessOfflineA2(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessOfflineA2");
                if (string.IsNullOrEmpty(key))
                    return;
                string[] keys = key.Split('.');
                if (keys.Length < 3)
                    return;
                SuiteStatusInfoManage.ChangeOnlineFlag(keys[2], false);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        #endregion

        #region  equipment installation and maintenance
        /// <summary>
        /// equipment installation is complete,the cached package information
        /// </summary>
        [Business(typeName: UserMessageRoute.ComplateSuiteInstallKey)]
        public void ProcessDeviceInstall(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessDeviceInstall");
                var model = ConvertHelper.BytesToObject(bytes) as DeviceInstall;
                if (model != null)
                {
                    SuiteStatusInfoManage.AddSuiteInfo(model.MdvrCoreId);

                    LoggerManager.Logger.Info(string.Format("The new equipment is installed,MDVR_Core_SN:{0}", model.MdvrCoreId));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// device switches to maintenance to remove the package from the cache
        /// </summary>
        [Business(typeName: UserMessageRoute.SuiteMaintainKey)]
        public void ProcessDeviceMaintain(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessDeviceMaintain");
                var model = ConvertHelper.BytesToObject(bytes) as DeviceMaintain;
                if (model != null)
                {
                    SuiteStatusInfoManage.DeleteSuiteInfo(model.MdvrCoreId);
                    LoggerManager.Logger.Info(string.Format("Device into maintenance state,MDVR_Core_SN:{0}!", model.MdvrCoreId));
                }

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        #endregion

        #region set speed command

        /// <summary>
        /// set speed command 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: MonitorRoute.OriginalSettingOverSpeedCMDKey)]
        public void ProcessSettingOverSpeedCMD(PTMSEntities context, byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessSettingOverSpeedCMD");
                OverSpeedCommandManager.CommandSend(context, bytes, key);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        #endregion

        #region electronic fence

        /// <summary>
        /// Information issused electronic fence,if the device is not online,but the information is not delivered in the local cache
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: MonitorRoute.OriginalFenceKey)]
        public void ProcessFence(PTMSEntities context, byte[] bytes, string key)
        {
            try
            {
                ElectricFenceCommand.CommandSend(context, bytes, key);
                LoggerManager.Logger.Info("ProcessFence");
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        ///// <summary>
        ///// electronic fence reply
        ///// </summary>
        ///// <param name="bytes"></param>
        ///// <param name="key"></param>
        //[Business(typeName: ReplyRoute.OriginalFenceyDeviceReplyKey)]
        //public void ProcessFenceyReply(byte[] bytes, string key)
        //{
        //    try
        //    {
        //        FenceCommandManager.CommandReply(bytes, key,TrafficReplySource.Device);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.Logger.Error(ex);
        //    }
        //}

        /// <summary>
        /// business electronic fence reply
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [Business(typeName: ReplyRoute.OriginalFenceyBusinessReplyKey)]
        public void ProcessFenceyBusinessReply(PTMSEntities context, byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessFenceyBusinessReply");
                FenceCommandManager.CommandReply(context, bytes, key, TrafficReplySource.MoitorAlert);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        #endregion

        #region C30 Command

        /// <summary>
        [Business(typeName: SettingRoute.ElectricFenceObjectKey)]
        public void ProcessEletricFenceSendup(PTMSEntities context, byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessEletricFenceSendup");
                LoggerManager.Logger.Info("Get Information:" + SettingRoute.ElectricFenceObjectKey);
                ElectricFenceCommand.CommandSend(context, bytes, key);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        [Business(typeName: ReplyRoute.OriginalFenceyDeviceReplyKey)]
        public void ProcessFenceyReply(PTMSEntities context, byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessFenceyReply");
                ElectricFenceCommand.CommandReply(context, bytes, key);
                // FenceCommandManager.CommandReply(bytes, key, TrafficReplySource.Device);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        [Business(typeName: SettingRoute.OverSpeedObjectKey)]
        public void ProcessOverSpeedSendup(PTMSEntities context, byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("Get Information:" + SettingRoute.OverSpeedObjectKey);
                OverSpeedCommand.CommandSend(context, bytes, key);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        [Business(typeName: SettingRoute.SendInfomationKey)]
        public void InformationSend(PTMSEntities context, byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("Get Information:" + SettingRoute.SendInfomationKey);
                C57CommandManager.CommandSend(context, bytes, key);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }


        [Business(typeName: ReplyRoute.OriginalSettingOverSpeedReplyKey)]
        public void ProcessSettingOverSpeedReply(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("ProcessSettingOverSpeedReply");
                OverSpeedCommand.CommandReply(bytes, key);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        #endregion

        #region not delete
        ///// 设置 GPS上传规则
        ///// </summary>
        ///// <param name="bytes"></param>
        ///// <param name="key"></param>
        //[Business(typeName: UserMessageRoute.OriginalLocationMonitorKey)]
        //public void ProcessMDVRGPSMonitor(byte[] bytes, string key)
        //{
        //    try
        //    {
        //        C30CommandManager.CommandSend(bytes, key);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.Logger.Error(ex);
        //    }
        //}

        ///// <summary>
        ///// Daily monitoring of the GPS information request MDVR
        ///// </summary>
        ///// <param name="bytes"></param>
        ///// <param name="key"></param>
        //[Business(typeName: UserMessageRoute.CancelLocationMonitorKey)]
        //public void ProcessCancelMDVRGPSMonitor(byte[] bytes, string key)
        //{
        //    try
        //    {
        //        C30CommandManager.CanccenMonitor(bytes, key);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.Logger.Error(ex);
        //    }
        //}

        ///// <summary>
        ///// Daily monitoring of the GPS information request MDVR Reply
        ///// </summary>
        ///// <param name="bytes"></param>
        ///// <param name="key"></param>
        //[Business(typeName: ReplyRoute.HandleGPSMonitorReplyKey)]
        //public void ProcessMDVRGPSMonitorReplay(byte[] bytes, string key)
        //{
        //    try
        //    {
        //        string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
        //        string[] array = str.Substring(0, str.LastIndexOf("#")).Split(',');
        //        if (array[2].Contains(":"))
        //        {
        //            GpsSendUpCommand.CommandReply(bytes, key);
        //        }
        //        else
        //        {
        //            C30CommandManager.CommandReply(bytes, key);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.Logger.Error(ex);
        //    }
        //}

        //[Business(typeName: ReplyRoute.HandleTemperatureReplyKey)]
        //public void ProcessTemperatureReplay(byte[] bytes, string key)
        //{
        //    try
        //    {
        //        TemperatureCommand.CommandReply(bytes, key);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.Logger.Error(ex);
        //    }
        //}

        //[Business(typeName: ReplyRoute.HandleOneKeyAlarmReplyKey)]
        //public void ProcessOneKeyAlarmReplay(byte[] bytes, string key)
        //{
        //    try
        //    {
        //        OneKeyAlarmCommand.CommandReply(bytes, key);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.Logger.Error(ex);
        //    }
        //}

        //[Business(typeName: ReplyRoute.HandleDelayAlarmReplyKey)]
        //public void ProcessDelayAlarmReplay(byte[] bytes, string key)
        //{
        //    try
        //    {
        //        OneKeyAlarmCommand.CommandC78Reply(bytes, key);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.Logger.Error(ex);
        //    }
        //}

        //[Business(typeName: ReplyRoute.HandleAbnormalDoorReplyKey)]
        //public void ProcessAbnormalDoorReplay(byte[] bytes, string key)
        //{
        //    try
        //    {
        //        AbnormalDoorCommand.CommandReply(bytes, key);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.Logger.Error(ex);
        //    }
        //}

        //[Business(typeName:SettingRoute.GpsSettingObjectKey)]
        //public void ProcessGPSSendup(byte[] bytes, string key)
        //{
        //    try
        //    {
        //        LoggerManager.Logger.Info("Get Information:" + SettingRoute.GpsSettingObjectKey);
        //        GpsSendUpCommand.CommandSend(bytes, key);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.Logger.Error(ex);
        //    }
        //}

        //[Business(typeName: SettingRoute.TemperatureSettingObjectKey)]
        //public void ProcessTemperatureSendup(byte[] bytes, string key)
        //{
        //    try
        //    {
        //        LoggerManager.Logger.Info("Get Information:" + SettingRoute.TemperatureSettingObjectKey);
        //        TemperatureCommand.CommandSend(bytes, key);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.Logger.Error(ex);
        //    }
        //}

        //[Business(typeName: SettingRoute.OneKeyAlarmSettingObjectKey)]
        //public void ProcessOneKeyAlarmSendup(byte[] bytes, string key)
        //{
        //    try
        //    {
        //        LoggerManager.Logger.Info("Get Information:" + SettingRoute.OneKeyAlarmSettingObjectKey);
        //        OneKeyAlarmCommand.CommandSend(bytes, key);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.Logger.Error(ex);
        //    }
        //}
        #endregion
    }
}
