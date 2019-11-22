/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e2ddca29-5f28-4a3d-aba4-ad34f95bd93c      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Repository
/////    Project Description:    
/////             Class Name: DeviceAlertRespository
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/15 11:45:56
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/24 15:00:00
/////            Modified by: guoh
/////   Modified Description: Add a device class the alarm information written to the database
/////======================================================================
/////          Modified Time: 2013/09/02 15:00:00
/////            Modified by: guoh
/////   Modified Description: Security suite alert lifted notify relevant database operations
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Gsafety.PTMS.Alert.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.Common.Logging;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Alert.Contract;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Alert.Repository.Utility;
using System.Data;

namespace Gsafety.PTMS.Alert.Repository
{
    public class DeviceAlertRespository : Gsafety.PTMS.BaseInfo.BaseRepository
    {
        public DeviceAlertRespository()
        {
        }

        public void AddDeviceAlert(PTMSEntities context, DeviceAlertEx model)
        {
            try
            {
                ALT_DEVICE_ALERT entity = new ALT_DEVICE_ALERT()
                {
                    ID = model.Id,
                    MDVR_CORE_SN = model.MdvrCoreId,
                    SUITE_INFO_ID = model.SuiteInfoId,
                    VEHICLE_ID = model.VehicleId,
                    SUITE_STATUS = model.SuiteStatus,
                    ALERT_TYPE = model.AlertType,
                    ALERT_TIME = model.GpsTime,
                    LONGITUDE = model.Longitude,
                    LATITUDE = model.Latitude,
                    SPEED = model.Speed,
                    DIRECTION = model.Direction,
                    DISTRICT_CODE = model.DistrictCode,
                    GPS_VALID = model.GpsValid,
                    CREATE_TIME = DateTime.Now.ToUniversalTime(),
                    CLIENT_ID = model.ClientId,
                    ADDITIONAL_INFO = model.AdditionalInfo,
                };

                if (model.GpsTime != DateTime.MinValue)
                {
                    entity.GPS_TIME = model.GpsTime;
                }

                context.ALT_DEVICE_ALERT.Add(entity);
                context.SaveChanges();

                ModifySecuritySuiteStatus(context, model.MdvrCoreId, DeviceSuiteStatus.Abnormal, (Gsafety.PTMS.Common.Data.DeviceAlertType)(model.AlertType));
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("********\r\n" + DateTime.Now + ":An exception occurred while device alarm information into the database。\r\n" + "Method:Gsafety.Ant.Alert.Repository.DeviceAlertRespository.AddDeviceAlert;\r\n" + ex.ToString() + "\r\n********\r\n");
            }
        }

        public void ModifySecuritySuiteStatus(PTMSEntities context, string mdvrCoreSN, DeviceSuiteStatus status, Gsafety.PTMS.Common.Data.DeviceAlertType alertType)
        {
            try
            {
                var result = (from s in context.RUN_SUITE_WORKING where s.MDVR_CORE_SN == mdvrCoreSN select s).FirstOrDefault();
                if (result != null && (result.STATUS == (short)DeviceSuiteStatus.Running || result.STATUS == (short)DeviceSuiteStatus.Abnormal))
                {
                    //The reason this caused the exception
                    string alertCause = Convert.ToString((short)alertType);
                    string abnormalCause = result.ABNORMAL_CAUSE;

                    if (string.IsNullOrEmpty(abnormalCause))
                    {
                        result.ABNORMAL_CAUSE = alertCause;
                    }
                    else
                    {
                        if (!result.ABNORMAL_CAUSE.Contains(string.Format("{0}, ", alertCause)) && !result.ABNORMAL_CAUSE.Contains(alertCause))
                            result.ABNORMAL_CAUSE = string.Format("{0}, {1}", alertCause, abnormalCause);
                    }

                    if (result.STATUS == (short)DeviceSuiteStatus.Running)
                    {
                        result.STATUS = (short)status;
                        result.FAULT_TIME = System.DateTime.Now.ToUniversalTime();
                    }

                    result.SWITCH_TIME = System.DateTime.Now.ToUniversalTime();
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("********\r\n" + DateTime.Now + ":An exception occurred while modifying the state security suite。\r\n" + "Method:Gsafety.PTMS.Alert.Repository.DeviceAlertRespository.ModifySecuritySuiteStatus;\r\n" + ex.ToString() + "\r\n********\r\n");
            }
        }







        public SingleMessage<Gsafety.PTMS.Alert.Contract.Data.DeviceAlert> AddDeviceAlert(PTMSEntities _context, Gsafety.PTMS.Alert.Contract.Data.DeviceAlert alert)
        {

            //There is no vehicle
            RUN_SUITE_WORKING record = _context.RUN_SUITE_WORKING.FirstOrDefault(v => v.VEHICLE_ID.Equals(alert.VehicleId));
            if (record == null)
            {
                return new SingleMessage<Gsafety.PTMS.Alert.Contract.Data.DeviceAlert> { IsSuccess = false, Result = null, ErrorMsg = ErrorMessage.ValidateNotRight.ToString() };
            }
            //Vehicles already in the abnormal state
            if (record.STATUS == (short)DeviceSuiteStatus.Abnormal)
            {
                return new SingleMessage<Gsafety.PTMS.Alert.Contract.Data.DeviceAlert> { IsSuccess = false, Result = null, ErrorMsg = ErrorMessage.SUITE_VehicleAbnormal.ToString() };
            }
            //The vehicle is not in the running state
            if (record.STATUS != (short)DeviceSuiteStatus.Running)
            {
                return new SingleMessage<Gsafety.PTMS.Alert.Contract.Data.DeviceAlert> { IsSuccess = false, Result = null, ErrorMsg = ErrorMessage.SUITE_VehicleNotRunning.ToString() };
            }

            if (String.IsNullOrEmpty(alert.SuiteInfoId))
            {
                alert.SuiteInfoId = _context.RUN_SUITE_WORKING.SingleOrDefault(s => s.VEHICLE_ID == alert.VehicleId).SUITE_INFO_ID;
                alert.MdvrCoreId = _context.RUN_SUITE_WORKING.SingleOrDefault(s => s.VEHICLE_ID == alert.VehicleId).MDVR_CORE_SN;
            }

            SingleMessage<Gsafety.PTMS.Alert.Contract.Data.DeviceAlert> result;
            var scope = new TransactionScope(TransactionScopeOption.Required);

            try
            {
                ALT_DEVICE_ALERT deviceAlert = new ALT_DEVICE_ALERT()
                {
                    ID = Guid.NewGuid().ToString(),
                    MDVR_CORE_SN = alert.MdvrCoreId,
                    SUITE_INFO_ID = alert.SuiteInfoId,
                    VEHICLE_ID = alert.VehicleId,
                    SUITE_STATUS = alert.SuiteStatus,
                    ALERT_TIME = alert.AlertTime,
                    //CMD = alert.Cmd,
                    LONGITUDE = alert.Longitude,
                    LATITUDE = alert.Latitude,
                    GPS_TIME = alert.GpsTime,
                    SPEED = alert.Speed,
                    DIRECTION = alert.Direction,
                    GPS_VALID = alert.GpsValid,
                    CREATE_TIME = DateTime.Now,
                    //TAG_VALUE = alert.TagValue
                };

                _context.ALT_DEVICE_ALERT.Add(deviceAlert);

                RUN_SUITE_WORKING status = _context.RUN_SUITE_WORKING.FirstOrDefault(d => d.MDVR_CORE_SN == alert.MdvrCoreId);
                status.STATUS = (short)DeviceSuiteStatus.Abnormal;

                int i = _context.SaveChanges();
                alert.Id = deviceAlert.ID;
                result = new SingleMessage<Gsafety.PTMS.Alert.Contract.Data.DeviceAlert>()
                {
                    IsSuccess = i > 0 ? true : false,
                    Result = alert
                };
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
            return result;

        }

        #region Device class alarm written to the database 2013-08-24 added by guoh

        /// <summary>
        /// The camera no signal alarm information written to the database
        /// </summary>
        /// <param name="alertModel">Camera no signal alarm entity</param>
        public void AddCameraNoSignalAlert(PTMSEntities context, CameraNoSignalAlert alertModel)
        {
            try
            {
                LoggerManager.Logger.Info("Before Add Cameral No Signal Alert to Database");

                //Filling device class the alarm data table
                ALT_DEVICE_ALERT entity = new ALT_DEVICE_ALERT();
                FillDeviceAlert(alertModel, entity);

                //TODO:Complement other alarm information, such as no signal the camera channel number.
                string tag = "ChannelId:" + alertModel.ChannelId;
                //entity.TAG_VALUE = tag;

                //Written to the database, and save
                context.ALT_DEVICE_ALERT.Add(entity);
                context.SaveChanges();

                //TODO：Modified state security suite (SECURITY_SUITE_WORKING. STATUS) for ABNORMAL (DeviceSuiteStatus. ABNORMAL), ABNORMAL reasons and add equipment
                LoggerManager.Logger.Info("Before Update Working Suite State to Database");
                ModifySecuritySuiteStatus(context, alertModel.MdvrCoreSN, DeviceSuiteStatus.Abnormal, Gsafety.PTMS.Base.Contract.Data.DeviceAlertType.VedioNoSignal);

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("********\r\n" + DateTime.Now + ":The camera no signal abnormal alarm information written to the database。\r\n" + "Method:Gsafety.PTMS.Alert.Repository.DeviceAlertRespository.AddCameraNoSignalAlert;\r\n" + ex.ToString() + "\r\n********\r\n");
            }
        }

        /// <summary>
        /// write the camera occlusion alarm information into the database
        /// </summary>
        /// <param name="alertModel">Camera occlusion alarm entities</param>
        public void AddCameraOcclusionAlert(PTMSEntities context, CameraOcclusionAlert alertModel)
        {
            try
            {

                //Filling device class alarm data table
                ALT_DEVICE_ALERT entity = new ALT_DEVICE_ALERT();
                FillDeviceAlert(alertModel, entity);

                //TODO:Complement other alarm information, such as obscured camera channel number.
                string tag = "ChannelId:" + alertModel.ChannelId;
                //entity.TAG_VALUE = tag;

                //Written to the database, and save
                context.ALT_DEVICE_ALERT.Add(entity);
                context.SaveChanges();

                //TODO：Modify the security package status (SECURITY_SUITE_WORKING.STATUS) abnormal (DeviceSuiteStatus.ABNORMAL), and add the reason for the exception equipment
                LoggerManager.Logger.Info("Before Update Working Suite State to Database");
                ModifySecuritySuiteStatus(context, alertModel.MdvrCoreSN, DeviceSuiteStatus.Abnormal, Gsafety.PTMS.Base.Contract.Data.DeviceAlertType.VedioShelter);

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("********\r\n" + DateTime.Now + ":An exception occurred while the camera occlusion alarm information into the database。\r\n" + "Method:Gsafety.PTMS.Alert.Repository.DeviceAlertRespository.AddCameraOcclusionAlert;\r\n" + ex.ToString() + "\r\n********\r\n");
            }
        }

        /// <summary>
        /// The temperature alarm information into the database
        /// </summary>
        /// <param name="alertModel">Temperature alarm entities</param>
        public void AddTemperatureAlert(PTMSEntities context, TemperatureAlert alertModel)
        {
            try
            {
                LoggerManager.Logger.Info("Before Add Temperature Alert to Database");

                //Filling device class alarm data table
                ALT_DEVICE_ALERT entity = new ALT_DEVICE_ALERT();
                FillDeviceAlert(alertModel, entity);

                //TODO:Complement other warning information, such as temperature type, the current temperature, low temperature, high temperature, etc.
                string tag = "TemperatureType:" + alertModel.TemperatureType + ";CurrentTemperature:" + alertModel.CurrentTemperature + ";MinTemperature:" + alertModel.MinTemperature + ";MaxTemperature:" + alertModel.MaxTemperature;
                //entity.TAG_VALUE = tag;

                //Written to the database and save
                context.ALT_DEVICE_ALERT.Add(entity);
                context.SaveChanges();

                //TODO：Modify the security package status (SECURITY_SUITE_WORKING.STATUS) abnormal (DeviceSuiteStatus.ABNORMAL), and add the reason for the exception equipment
                LoggerManager.Logger.Info("Before Update Working Suite State to Database");
                ModifySecuritySuiteStatus(context, alertModel.MdvrCoreSN, DeviceSuiteStatus.Abnormal, Gsafety.PTMS.Base.Contract.Data.DeviceAlertType.OverTemperature);

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("********\r\n" + DateTime.Now + ":An exception occurred when the temperature alarm information into the database。\r\n" + "Method:Gsafety.PTMS.Alert.Repository.DeviceAlertRespository.AddTemperatureAlert;\r\n" + ex.ToString() + "\r\n********\r\n");
            }
        }

        /// <summary>
        /// The MDVR memory card error warning information into the database
        /// </summary>
        /// <param name="alertModel">MDVR memory card error warning entity</param>
        public void AddMdvrMemoryCardErrorAlert(PTMSEntities context, MdvrMemoryCardErrorAlert alertModel)
        {
            try
            {
                //Filling device class alarm data table
                ALT_DEVICE_ALERT entity = new ALT_DEVICE_ALERT();
                FillDeviceAlert(alertModel, entity);

                //TODO:Complement other alarm information, such as hard disk number, error codes, error messages, etc.
                string tag = "HardDiskId:" + alertModel.HardDiskId + ";ErrorCode:" + alertModel.ErrorCode + ";ErrorMessage:" + alertModel.ErrorMessage;
                //entity.TAG_VALUE = tag;

                //Written to the database and save
                context.ALT_DEVICE_ALERT.Add(entity);
                context.SaveChanges();

                //TODO：Modify the security Kit Status (SECURITY_SUITE_WORKING.STATUS) abnormal (DeviceSuiteStatus.ABNORMAL), and add the reason for the exception equipment
                ModifySecuritySuiteStatus(context, alertModel.MdvrCoreSN, DeviceSuiteStatus.Abnormal, Gsafety.PTMS.Base.Contract.Data.DeviceAlertType.SdFault);

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("********\r\n" + DateTime.Now + ":An exception occurred while the MDVR memory card error warning information into the database。\r\n" + "Method:Gsafety.PTMS.Alert.Repository.DeviceAlertRespository.AddMdvrMemoryCardErrorAlert;\r\n" + ex.ToString() + "\r\n********\r\n");
            }
        }

        /// <summary>
        /// The GPS receiver fault alarm information into the database
        /// </summary>
        /// <param name="alertModel">GPS receiver fault alarm entities</param>
        public void AddGpsReceiverFaultAlert(PTMSEntities context, GpsReceiverFaultAlert alertModel)
        {
            try
            {
                LoggerManager.Logger.Info("Before Add Gps Receiver Fault Alert to Database");

                //Filling device class alarm data table
                ALT_DEVICE_ALERT entity = new ALT_DEVICE_ALERT();
                FillDeviceAlert(alertModel, entity);

                //TODO:Complement other alarms

                //Written to the database and save
                context.ALT_DEVICE_ALERT.Add(entity);
                context.SaveChanges();

                //TODO：Modify the security Kit Status (SECURITY_SUITE_WORKING.STATUS) abnormal (DeviceSuiteStatus.ABNORMAL), and add the reason for the exception equipment
                LoggerManager.Logger.Info("Before update Working Suite State to Database");
                ModifySecuritySuiteStatus(context, alertModel.MdvrCoreSN, DeviceSuiteStatus.Abnormal, Gsafety.PTMS.Base.Contract.Data.DeviceAlertType.GpsFault);

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("********\r\n" + DateTime.Now + ":An exception occurred while the GPS receiver fault alarm information into the database.\r\n" + "Method:Gsafety.PTMS.Alert.Repository.DeviceAlertRespository.AddGpsReceiverFaultAlert;\r\n" + ex.ToString() + "\r\n********\r\n");
            }
        }

        /// <summary>
        /// The abnormal voltage alarm information into the database
        /// </summary>
        /// <param name="alertModel">Abnormal voltage alarm entities</param>
        public void AddVoltageAbnormalAlert(PTMSEntities context, VoltageAbnormalAlert alertModel)
        {
            try
            {
                LoggerManager.Logger.Info("Before Add VoltageAbnormalAlert to Database");

                //Filling device class alarm data table
                ALT_DEVICE_ALERT entity = new ALT_DEVICE_ALERT();
                FillDeviceAlert(alertModel, entity);

                //TODO:Complement other alarm information, such as current voltage, low voltage, maximum voltage.
                string tag = "CurrentVoltage:" + alertModel.CurrentVoltage + ";MinVoltage:" + alertModel.MinVoltage + ";MaxVoltage:" + alertModel.MaxVoltage;
                //entity.TAG_VALUE = tag;

                //Written to the database and save
                context.ALT_DEVICE_ALERT.Add(entity);
                context.SaveChanges();

                //TODO：Modify the security Kit Status (SECURITY_SUITE_WORKING.STATUS) abnormal (DeviceSuiteStatus.ABNORMAL), and add the reason for the exception equipment
                LoggerManager.Logger.Info("Before update Working Suite State to Database");
                ModifySecuritySuiteStatus(context, alertModel.MdvrCoreSN, DeviceSuiteStatus.Abnormal, Gsafety.PTMS.Base.Contract.Data.DeviceAlertType.AbnormalValtage);

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("********\r\n" + DateTime.Now + ":The exception occurs when abnormal voltage alarm information into the database。\r\n" + "Method:Gsafety.PTMS.Alert.Repository.DeviceAlertRespository.AddVoltageAbnormalAlert;\r\n" + ex.ToString() + "\r\n********\r\n");
            }
        }

        /// <summary>
        /// Turn the ignition alarm information into the database
        /// </summary>
        /// <param name="alertModel">Ignition alarm entities</param>
        public void AddFireAlert(PTMSEntities context, FireAlert alertModel)
        {
            try
            {

                //Filling device class alarm data table
                ALT_DEVICE_ALERT entity = new ALT_DEVICE_ALERT();
                FillDeviceAlert(alertModel, entity);

                //TODO:Complement other alarm information, such as ignition status, the local time zone.
                string tag = "FireType:" + alertModel.FireType + ";TimeZone:" + alertModel.TimeZone;
                //entity.TAG_VALUE = tag;

                //Written to the database and save
                context.ALT_DEVICE_ALERT.Add(entity);
                context.SaveChanges();

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("********\r\n" + DateTime.Now + ":An exception occurred while the ignition alarm information into the database。\r\n" + "Method:Gsafety.PTMS.Alert.Repository.DeviceAlertRespository.AddFireAlert;\r\n" + ex.ToString() + "\r\n********\r\n");
            }
        }

        /// <summary>
        /// Filling device class the alarm data table entity
        /// </summary>
        /// <param name="alertBaseModel">Alarm entity base class</param>
        /// <param name="entity">Alarm device class data sheet entities</param>
        private static void FillDeviceAlert(AlertBaseModel alertBaseModel, ALT_DEVICE_ALERT entity)
        {
            entity.ID = alertBaseModel.ID;
            entity.MDVR_CORE_SN = alertBaseModel.MdvrCoreSN;
            entity.SUITE_INFO_ID = alertBaseModel.SuitInfoID;
            entity.VEHICLE_ID = alertBaseModel.VehicleID;
            entity.SUITE_STATUS = alertBaseModel.SuiteStatus;
            entity.ALERT_TYPE = alertBaseModel.AlertType;
            entity.ALERT_TIME = alertBaseModel.AlertTime;
            //entity.CMD = alertBaseModel.Cmd;
            entity.LONGITUDE = alertBaseModel.Longitude;
            entity.LATITUDE = alertBaseModel.Latitude;
            entity.GPS_TIME = alertBaseModel.GpsTime;
            entity.SPEED = alertBaseModel.Speed.ToString();
            entity.DIRECTION = alertBaseModel.Direction;
            entity.GPS_VALID = alertBaseModel.GpsValid;
            entity.CREATE_TIME = alertBaseModel.CreateTime;
            entity.STATUS = alertBaseModel.Status;
            entity.DISTRICT_CODE = alertBaseModel.DistrictCode;
        }

        /// <summary>
        /// Modify the specified number of chips MDVR state security suite
        /// </summary>
        /// <param name="mdvrCoreSN">mdvr Core SN</param>
        /// <param name="status">status</param>
        /// <param name="alertType">alert Type</param>
        public void ModifySecuritySuiteStatus(PTMSEntities context, string mdvrCoreSN, DeviceSuiteStatus status, Gsafety.PTMS.Base.Contract.Data.DeviceAlertType alertType)
        {
            try
            {
                //context.Configuration.ValidateOnSaveEnabled = false; 
                //Query specifies MDVR chip number information security suite 
                var result = (from s in context.RUN_SUITE_WORKING where s.MDVR_CORE_SN == mdvrCoreSN select s).FirstOrDefault();
                if (result != null && (result.STATUS == (short)DeviceSuiteStatus.Running || result.STATUS == (short)DeviceSuiteStatus.Abnormal))
                {
                    //The reason this caused the exception
                    string alertCause = Convert.ToString((short)alertType);
                    string abnormalCause = result.ABNORMAL_CAUSE;

                    if (string.IsNullOrEmpty(abnormalCause))
                    {
                        result.ABNORMAL_CAUSE = alertCause;
                    }
                    else
                    {
                        ////////////The median value of the current alarm types are consistent（2 digits ） update dzl
                        if (!result.ABNORMAL_CAUSE.Contains(string.Format("{0}, ", alertCause)) && !result.ABNORMAL_CAUSE.Contains(alertCause))
                            result.ABNORMAL_CAUSE = string.Format("{0}, {1}", alertCause, abnormalCause);

                        ////////////If the alarm type value-digit inconsistent   update dzl
                        //if (!abnormalCause.Equals(alertCause) && !abnormalCause.Contains(string.Format(", {0}, ", alertCause)) 
                        //    && !abnormalCause.Substring(0, abnormalCause.IndexOf(", ") + 1).Equals(alertCause)
                        //   && !abnormalCause.Substring(abnormalCause.LastIndexOf(", " + 2)).Equals(alertCause))
                        //{
                        //    result.ABNORMAL_CAUSE = string.Format("{0}, {1}", alertCause, abnormalCause);
                        //}
                    }

                    if (result.STATUS == (short)DeviceSuiteStatus.Running)
                    {
                        result.STATUS = (short)status;
                        result.FAULT_TIME = System.DateTime.Now;
                    }

                    result.SWITCH_TIME = System.DateTime.Now;
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("********\r\n" + DateTime.Now + ":An exception occurred while modifying the state security suite。\r\n" + "Method:Gsafety.PTMS.Alert.Repository.DeviceAlertRespository.ModifySecuritySuiteStatus;\r\n" + ex.ToString() + "\r\n********\r\n");
            }
        }

        #endregion

        #region relevant database operations of Security Suite released notify  2013-09-02 added by guoh
        /// <summary>
        /// relevant database operations of Security Suite released notify 
        /// Change the database security suite SECURITY_SUITE_WORKING tables state and abnormal reasons.
        /// When abnormal reason is empty, modify the security suite for the normal operation of the state;Otherwise, the state security suite do not modify.
        /// </summary>
        /// <param name="model">Security Suite alarm notification released entity</param>
        public void RemoveDeviceSuiteAlertNotify(PTMSEntities context, RemoveDeviceSuiteAlertNotify model)
        {
            //TODO: Remove the cause of the abnormal corresponding alarm device; When the device is empty abnormal reason, modify the security package status (SECURITY_SUITE_WORKING.STATUS) for normal operation status (DeviceSuiteStatus.Running)
            try
            {
                LoggerManager.Logger.Info("Before Update Working Seucrity State to Database");
                //Get need to lift the alarm type
                string removeDeviceAlertTypeString = model.removeDeviceAlertType;
                if (removeDeviceAlertTypeString != null && removeDeviceAlertTypeString.Length > 0)
                {
                    //The need to lift the alarm type array
                    List<string> removeAlertTypes = removeDeviceAlertTypeString.Split(',').ToList<string>();
                    //MDVR chip no.
                    string mdvrCoreSN = model.MdvrCoreSN;
                    //Operation Database

                    //Query specifies MDVR chip number information security suite
                    var result = (from s in context.RUN_SUITE_WORKING where s.MDVR_CORE_SN == mdvrCoreSN select s).FirstOrDefault();
                    if (result != null)
                    {
                        //Get the current content security suite Abnormal
                        string abnormalCause = result.ABNORMAL_CAUSE;
                        bool isChange = false;
                        if (!string.IsNullOrEmpty(abnormalCause))
                        {
                            foreach (var item in removeAlertTypes)
                            {
                                if (abnormalCause.Equals(item))
                                {
                                    abnormalCause = string.Empty;
                                    isChange = true;
                                    break;
                                }

                                #region The median value of the current alarm types are the same (for the two), can only be judged on it so  update dzl
                                if (abnormalCause.Contains(string.Format("{0}, ", item)))
                                {
                                    abnormalCause = abnormalCause.Replace(string.Format("{0}, ", item), string.Empty);
                                    isChange = true;
                                }

                                if (abnormalCause.Contains(string.Format(", {0}", item)))
                                {
                                    abnormalCause = abnormalCause.Replace(string.Format("{0}, ", item), string.Empty);
                                    isChange = true;
                                }
                                #endregion

                                #region If the alarm type is inconsistent with the following median value judgments must be judged using the following method update dzl

                                //if (abnormalCause.Contains(string.Format(", {0}, ", item)))
                                //    abnormalCause = abnormalCause.Replace(string.Format(", {0}, ", item), ", ");
                                //if (abnormalCause.Substring(0, abnormalCause.IndexOf(", ") + 1).Equals(item))
                                //    abnormalCause = abnormalCause.Substring(abnormalCause.IndexOf(", ") + 2);
                                //if (abnormalCause.Substring(abnormalCause.LastIndexOf(", " + 2)).Equals(item))
                                //    abnormalCause = abnormalCause.Substring(0, abnormalCause.LastIndexOf(", ") + 1);

                                #endregion
                            }
                        }

                        if (isChange)
                        {
                            if (string.IsNullOrEmpty(abnormalCause))
                            {
                                result.STATUS = (short)DeviceSuiteStatus.Running;
                                result.FAULT_TIME = null;
                            }
                            result.ABNORMAL_CAUSE = abnormalCause;
                            result.SWITCH_TIME = System.DateTime.Now;
                            context.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("********\r\n" + DateTime.Now + ":An exception occurred while lifting security suite alarm notification。\r\n" + "Method:Gsafety.PTMS.Alert.Repository.DeviceAlertRespository.RemoveDeviceSuiteAlert;\r\n" + ex.ToString() + "\r\n********\r\n");
            }
        }
        #endregion

        public List<DeviceAlertEx> GetDeviceAlertEx1(PTMSEntities _context, string CarNumber, string sutieId, List<decimal?> alertType, DateTime? startTime, DateTime? endTime, PagingInfo pagingInfo, out int totalCount)
        {
            if (alertType == null)
            {
                alertType = new List<decimal?>();
            }
            var result = (from alert in _context.ALT_DEVICE_ALERT
                          join y in _context.BSC_DEV_SUITE on alert.SUITE_INFO_ID equals y.SUITE_INFO_ID
                          join city in _context.BSC_DISTRICT on alert.DISTRICT_CODE equals city.CODE
                          where (string.IsNullOrEmpty(sutieId) ? true : alert.MDVR_CORE_SN.Contains(sutieId)) &&
                                (alertType.Count() == 0 ? true : alertType.Contains(alert.ALERT_TYPE)) &&
                                (string.IsNullOrEmpty(CarNumber) ? true : (alert.VEHICLE_ID.ToLower().Contains(CarNumber.ToLower()))) &&
                                (startTime == null ? true : (alert.ALERT_TIME >= startTime)) &&
                                (endTime == null ? true : (alert.ALERT_TIME <= endTime))
                          select new DeviceAlertEx
                          {
                              Id = alert.ID,
                              VehicleId = alert.VEHICLE_ID,
                              AlertType = alert.ALERT_TYPE,
                              AlertTime = alert.ALERT_TIME,
                              Longitude = alert.LONGITUDE,
                              Latitude = alert.LATITUDE,
                              MdvrCoreId = alert.MDVR_CORE_SN,
                              Direction = alert.DIRECTION,
                              SuiteStatus = alert.STATUS.Value,
                              GpsTime = alert.GPS_TIME,
                              GpsValid = alert.GPS_VALID,
                              Speed = alert.SPEED,
                              Province = city.NAME

                          });

            return result.Page(out totalCount, pagingInfo.PageIndex, pagingInfo.PageSize, true, t => t.AlertTime, false).ToList();
        }

        /// <summary>
        /// Offline storage devices (24 hours off, 48 hours off, 72 hours off) alarm
        /// </summary>
        /// <param name="offlineAlert"></param>
        public void AddOfflineAler(PTMSEntities context, OfflineAlert offlineAlert)
        {
            try
            {

                //Filling device class alarm data table
                ALT_DEVICE_ALERT entity = new ALT_DEVICE_ALERT();
                entity.ID = Guid.NewGuid().ToString();
                entity.MDVR_CORE_SN = offlineAlert.MdvrCoreId;
                entity.SUITE_INFO_ID = offlineAlert.SuiteInfoId;
                entity.VEHICLE_ID = offlineAlert.VehicleId;
                entity.SUITE_STATUS = offlineAlert.SuiteStatus;
                entity.ALERT_TYPE = offlineAlert.AlertType;
                entity.ALERT_TIME = offlineAlert.AlertTime;
                entity.GPS_VALID = "V";
                entity.GPS_TIME = offlineAlert.AlertTime;
                entity.CREATE_TIME = DateTime.Now;
                entity.DISTRICT_CODE = offlineAlert.DistrictCode;

                //Written to the database and save
                context.ALT_DEVICE_ALERT.Add(entity);
                context.SaveChanges();

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("********\r\n" + DateTime.Now + ":An exception occurred while writing to the database offline timeout information。\r\n" + "Method:Gsafety.PTMS.Alert.Repository.DeviceAlertRespository.AddFireAlert;\r\n" + ex.ToString() + "\r\n********\r\n");
            }
        }

        /**************************************************************************************************************/

        public static SingleMessage<bool> UpdateDeviceAlert(PTMSEntities context, Gsafety.PTMS.Alert.Contract.Data.DeviceAlert model)
        {
            var entity = context.ALT_DEVICE_ALERT.FirstOrDefault(t => t.ID == model.Id);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            DeviceAlertUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = EntityState.Modified;
            return context.Save();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public static SingleMessage<bool> DeleteDeviceAlertByID(PTMSEntities context, string ID)
        {
            ALT_DEVICE_ALERT entity = context.ALT_DEVICE_ALERT.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.ALT_DEVICE_ALERT.Attach(entity);
                context.ALT_DEVICE_ALERT.Remove(entity);
                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        public static MultiMessage<Gsafety.PTMS.Alert.Contract.Data.DeviceAlert> GetDeviceAlertList(PTMSEntities context, string clientID, string vehicleID, int? alertType, DateTime? StartTime, DateTime? EndTime, List<string> stationids, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            var result = from d in context.ALT_DEVICE_ALERT
                         join r in context.MTN_INSTALLATION_DETAIL
                         on d.VEHICLE_ID equals r.VEHICLE_ID
                         where d.CLIENT_ID == clientID
                               && (string.IsNullOrEmpty(vehicleID) ? true : d.VEHICLE_ID.ToUpper().Contains(vehicleID))
                               && d.ALERT_TIME > StartTime.Value
                               && d.ALERT_TIME < EndTime.Value
                               && stationids.Contains(r.STATION_ID)
                         select d;

            if (alertType.HasValue)
            {
                result = result.Where(n => n.ALERT_TYPE == alertType.Value);
            }

            var list = result.Page(out totalCount, pageIndex, pageSize, true, t => t.ALERT_TIME).ToList();

            var items = list.Select(t => DeviceAlertUtility.GetModel(t)).ToList();
            return new MultiMessage<Gsafety.PTMS.Alert.Contract.Data.DeviceAlert>(items, totalCount);
        }


    }
}
