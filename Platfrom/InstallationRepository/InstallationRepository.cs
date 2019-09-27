using Gs.PTMS.Common.Data.Enum;
using Gsafety.Common.Util;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Installation.Contract;
using Gsafety.PTMS.Installation.Contract.Data;
using Gsafety.PTMS.Manager.Repository;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ea0ee472-a85c-438b-9046-a9a21e4a82h1      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-XUHJ
/////                 Author: TEST(xuhj)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Repository
/////    Project Description:    
/////             Class Name: InstallationRepository
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/22 16:42:00
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/22 16:42:00
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;


namespace Gsafety.PTMS.Installation.Repository
{
    public class InstallationRepository : Gsafety.PTMS.BaseInfo.BaseRepository
    {
        public InstallationRepository()
        {

        }

        public RegisterResponse Register(PTMSEntities context, Register registerInfo)
        {
            RegisterResponse result = new RegisterResponse();
            result.UID = registerInfo.UID;
            result.SerialNo = registerInfo.SerialNo;
            result.RegisterNo = registerInfo.UID;
            result.SIM = registerInfo.SIM;

            ////检查UID和MDVR_CORE_SN
            //if ((registerInfo.ManufactureId + registerInfo.TerminalId) != registerInfo.UID)
            //{
            //    result.ResultType = (int)RegisterResultType.NoMdvr;
            //    return result;
            //}







            //var register = context.MTN_MDVR_REGISTER.FirstOrDefault(x => x.MDVR_CORE_SN == registerInfo.UID || x.VEHICLE_ID == registerInfo.VehicleId);
            //if (register != null)
            //{
            //    //车辆套件已注册也通过
            //    if ((register.VEHICLE_ID == registerInfo.VehicleId) && (register.MDVR_CORE_SN == registerInfo.UID))
            //    {
            //        result.ResultType = (int)RegisterResultType.Success;
            //        result.VehicleId = registerInfo.VehicleId;
            //        return result;
            //    }
            //    else
            //    {
            //        if (register.VEHICLE_ID == registerInfo.VehicleId)
            //        {
            //            result.ResultType = (int)RegisterResultType.VehicleRegistered;
            //            return result;
            //        }
            //        else
            //        {
            //            result.ResultType = (int)RegisterResultType.MdvrRegistered;
            //            return result;
            //        }
            //    }
            //}
            //else
            //{
            //    MTN_MDVR_REGISTER item = new MTN_MDVR_REGISTER();
            //    item.ID = Guid.NewGuid().ToString();
            //    item.SUITE_INFO_ID = suite.SUITE_INFO_ID;
            //    item.MDVR_CORE_SN = registerInfo.UID;
            //    item.VEHICLE_ID = registerInfo.VehicleId;
            //    item.REGISTER_TIME = DateTime.Now;
            //    item.REGISTER_CODE = registerInfo.UID;
            //    item.CLIENT_ID = vehicle.CLIENT_ID;

            //    context.MTN_MDVR_REGISTER.Add(item);
            //    context.SaveChanges();
            //    result.ResultType = (int)RegisterResultType.Success;
            //    return result;
            //}
            RUN_SUITE_WORKING entity = context.RUN_SUITE_WORKING.FirstOrDefault(n => n.MDVR_CORE_SN == result.UID && n.VEHICLE_ID == registerInfo.VehicleId);
            if (entity != null)
            {
                result.ResultType = (int)RegisterResultType.Success;
                result.VehicleId = entity.VEHICLE_ID;
            }
            else
            {
                result.ResultType = (int)RegisterResultType.NoVehicle;
            }

            return result;
        }

        public UnRegisterResponse DelRegisterInfo(PTMSEntities context, UnRegister registerInfo)
        {
            UnRegisterResponse response = new UnRegisterResponse();
            response.UID = registerInfo.UID;
            response.SerialNo = registerInfo.SerialNo;
            response.RegisterNo = registerInfo.UID;
            response.SIM = registerInfo.SIM;
            response.IsPassed = (int)IsPassed.Yes;

            var result = context.MTN_MDVR_REGISTER.FirstOrDefault(x => x.REGISTER_CODE == registerInfo.RegisterNo);
            if (result != null)
            {
                context.MTN_MDVR_REGISTER.Remove(result);
                if (context.SaveChanges() <= 0)
                {
                    response.IsPassed = (int)IsPassed.No;
                }
            }
            return response;
        }

        /// <summary>
        /// Test equipment currently installed license plate number to upload
        /// </summary>
        /// <param name="installationId">Equipment installation unique number</param>
        /// <returns>Calibration results</returns>
        public string GetInstallingSuiteVehicleIdCheckSuiteCar(PTMSEntities context, string mdvrId)
        {
            PTMSAppConfigRepository AppConfig = new PTMSAppConfigRepository();
            int validtime;
            if (AppConfig.GetappConfigInfo(context, "InstallAvailableTime") != null)
            {
                validtime = int.Parse(AppConfig.GetappConfigInfo(context, "InstallAvailableTime").SECTION_VALUE);
            }
            else
            {
                validtime = 1;
            }

            DateTime dt = DateTime.Now.AddHours(-validtime);

            var result = (from x in context.RUN_SUITE_ONLINE_RECORD

                          where x.MDVR_CORE_SN == mdvrId
                          && x.GPS_TIME > dt
                          && x.VEHICLE_ID != null
                          select x).OrderByDescending(c => c.GPS_TIME).FirstOrDefault();
            if (result != null)
            {
                return result.VEHICLE_ID;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Installation Record Results
        /// </summary>
        /// <param name="installationId">Installation Record Id</param>
        /// <returns></returns>
        public InstallInfoResult GetInstallationResult(PTMSEntities context, string installationId)
        {
            InstallInfoResult model = new InstallInfoResult();
            InstallationInfo Installation = new InstallationInfo();
            InstallationAudit Audit = new InstallationAudit();

            var item = context.MTN_INSTALLATION_DETAIL.SingleOrDefault(d => d.ID == installationId);
            if (item != null)
            {
                Installation.Id = item.ID;
                Installation.VehicleID = item.VEHICLE_ID;
                //if (item.VEHICLE_ID != null)
                //    if (context.VEHICLE.SingleOrDefault(d => d.VEHICLE_ID == item.VEHICLE_ID) != null)
                //        Installation.VehicleType = context.VEHICLE.SingleOrDefault(d => d.VEHICLE_ID == item.VEHICLE_ID).VEHICLE_TYPE;

                Installation.DeviceCoreId = item.MDVR_CORE_SN;
                if (item.SUITE_INFO_ID != null)
                    if (context.BSC_DEV_SUITE.SingleOrDefault(d => d.SUITE_INFO_ID == item.SUITE_INFO_ID) != null)
                        Installation.DeviceSN = context.BSC_DEV_SUITE.SingleOrDefault(d => d.SUITE_INFO_ID == item.SUITE_INFO_ID).SUITE_ID;
                Installation.InstallationStationId = item.STATION_ID;


                if (item.STATION_ID != null)
                    if (context.BSC_SETUP_STATION.SingleOrDefault(d => d.ID == item.STATION_ID) != null)
                        Installation.InstallationStationName = context.BSC_SETUP_STATION.Single(d => d.ID == item.STATION_ID).NAME;
                Installation.InstallationStaff = item.INSTALL_STAFF;
                Installation.RecordStaff = item.RECORD_STAFF;
                if (item.CREATE_TIME != null)
                    Installation.CreateTime = DateTime.Parse(item.CREATE_TIME.ToString());
                if (item.FINISH_TIME != null)
                    Installation.FinishTime = DateTime.Parse(item.FINISH_TIME.ToString());
                Installation.Note = item.NOTE;
                Installation.CheckStep = item.CHECKSTEP;
            }


            var item1 = context.MTN_INSTALLATION_AUDIT.SingleOrDefault(d => d.INSTALL_ID == installationId);
            if (item1 != null)
            {
                Audit.Id = item1.INSTALL_ID;
                if (item1.ALARM_ID != null)
                    Audit.AlarmId = item1.ALARM_ID;
                Audit.AlarmCheck = item1.ALARM_CHECK;
                Audit.GpsCheck = item1.GPS_CHECK;
                Audit.VideoCheck = item1.VIDEO_CHECK;
                Audit.IsSuccess = item1.AUDIT_FLAG;
                Audit.Approver = item1.APPROVER;
                Audit.ApproverTime = item1.APPROVE_TIME;
                Audit.Content = item1.CONTENT;
            }

            model.Installation = Installation;
            model.Audit = Audit;

            return model;

        }

        /// <summary>
        /// For installation information based on the primary key ID
        /// </summary>
        /// <param name="installationId">Install the primary key Id</param>
        /// <returns>Installation Information</returns>
        public InstallationInfo GetInstallationDetail(PTMSEntities context, string installationId)
        {

            var item = context.MTN_INSTALLATION_DETAIL.SingleOrDefault(d => d.ID == installationId);
            if (item != null)
            {
                InstallationInfo Installation = new InstallationInfo();
                Installation.Id = item.ID;
                Installation.VehicleID = item.VEHICLE_ID;
                //if (!string.IsNullOrEmpty(item.VEHICLE_ID))
                //    if (context.VEHICLE.SingleOrDefault(d => d.VEHICLE_ID == item.VEHICLE_ID) != null)
                //        Installation.VehicleType = context.VEHICLE.SingleOrDefault(d => d.VEHICLE_ID == item.VEHICLE_ID).VEHICLE_TYPE;
                Installation.DeviceCoreId = item.MDVR_CORE_SN;
                Installation.DeviceKey = item.SUITE_INFO_ID;
                if (!string.IsNullOrEmpty(item.SUITE_INFO_ID))
                    if (context.BSC_DEV_SUITE.SingleOrDefault(d => d.SUITE_INFO_ID == item.SUITE_INFO_ID) != null)
                        Installation.DeviceSN = context.BSC_DEV_SUITE.SingleOrDefault(d => d.SUITE_INFO_ID == item.SUITE_INFO_ID).SUITE_ID;
                Installation.InstallationStationId = item.STATION_ID;
                if (!string.IsNullOrEmpty(item.STATION_ID))
                    if (context.BSC_SETUP_STATION.SingleOrDefault(d => d.ID == item.STATION_ID) != null)
                        Installation.InstallationStationName = context.BSC_SETUP_STATION.SingleOrDefault(d => d.ID == item.STATION_ID).NAME;
                Installation.InstallationStaff = item.INSTALL_STAFF;
                Installation.RecordStaff = item.RECORD_STAFF;
                if (item.CREATE_TIME != null)
                    Installation.CreateTime = DateTime.Parse(item.CREATE_TIME.ToString());
                if (item.FINISH_TIME != null)
                    Installation.FinishTime = DateTime.Parse(item.FINISH_TIME.ToString());
                Installation.Note = item.NOTE;
                Installation.CheckStep = item.CHECKSTEP;

                var v = context.BSC_VEHICLE.FirstOrDefault(n => n.VEHICLE_ID == Installation.VehicleID && n.VALID == 1);
                if (v != null)
                {
                    Installation.Organization = v.ORGNIZATION_ID;
                }

                return Installation;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Add a new installation record
        /// </summary>
        /// <param name="installation">Installation Record</param>
        /// <returns>True indicates success, False indicates failure</returns>
        public bool AddInstallation(PTMSEntities context, InstallationInfo installationInfo)
        {
            MTN_INSTALLATION_DETAIL item = new MTN_INSTALLATION_DETAIL();
            if (installationInfo.Id != null) item.ID = installationInfo.Id;
            if (installationInfo.VehicleID != null) item.VEHICLE_ID = installationInfo.VehicleID;
            if (installationInfo.DeviceKey != null) item.SUITE_INFO_ID = installationInfo.DeviceKey;
            if (installationInfo.DeviceCoreId != null) item.MDVR_CORE_SN = installationInfo.DeviceCoreId;
            if (installationInfo.InstallationStationId != null) item.STATION_ID = installationInfo.InstallationStationId;
            if (installationInfo.InstallationStaff != null) item.INSTALL_STAFF = installationInfo.InstallationStaff;
            if (installationInfo.RecordStaff != null) item.RECORD_STAFF = installationInfo.RecordStaff;
            if (installationInfo.Note != null) item.NOTE = installationInfo.Note;
            if (installationInfo.CheckStep != null) item.CHECKSTEP = short.Parse(installationInfo.CheckStep.ToString());
            if (installationInfo.FinishTime != null) item.FINISH_TIME = installationInfo.FinishTime;
            if (installationInfo.CreateTime != null) item.CREATE_TIME = installationInfo.CreateTime;
            item.VALID = 1;

            context.MTN_INSTALLATION_DETAIL.Add(item);
            if (context.SaveChanges() > 0)
                return true;
            else
                return false;

        }

        /// <summary>
        /// Update Installation Record
        /// </summary>
        /// <param name="installation">Installation Record</param>
        /// <returns>True indicates success, False indicates failure</returns>
        public bool UpdateInstallation(PTMSEntities context, InstallationInfo installation)
        {
            MTN_INSTALLATION_DETAIL item = context.MTN_INSTALLATION_DETAIL.SingleOrDefault(d => d.ID == installation.Id);
            if (item.CHECKSTEP == installation.CheckStep)
            {
                return true;
            }
            if (!string.IsNullOrEmpty(installation.Id)) item.ID = installation.Id;
            if (!string.IsNullOrEmpty(installation.VehicleID)) item.VEHICLE_ID = installation.VehicleID;
            if (!string.IsNullOrEmpty(installation.DeviceKey)) item.SUITE_INFO_ID = installation.DeviceKey;
            if (!string.IsNullOrEmpty(installation.DeviceCoreId)) item.MDVR_CORE_SN = installation.DeviceCoreId;
            if (!string.IsNullOrEmpty(installation.InstallationStationId)) item.STATION_ID = installation.InstallationStationId;
            if (!string.IsNullOrEmpty(installation.InstallationStaff)) item.INSTALL_STAFF = installation.InstallationStaff;
            if (!string.IsNullOrEmpty(installation.RecordStaff)) item.RECORD_STAFF = installation.RecordStaff;
            if (!string.IsNullOrEmpty(installation.Note)) item.NOTE = installation.Note;
            if (installation.CheckStep != null) item.CHECKSTEP = short.Parse(installation.CheckStep.ToString());
            if (installation.FinishTime != null) item.FINISH_TIME = installation.FinishTime;
            if (installation.CreateTime != null) item.CREATE_TIME = installation.CreateTime;
            if (context.SaveChanges() > 0)
                return true;
            else
                return false;

        }

        /// <summary>
        /// Delete the corresponding data in accordance with the installation Install the primary key Id
        /// </summary>
        /// <param name="installationId">Install the primary key Id</param>
        /// <returns>true indicates deleted successfully, false representation delete failed</returns>
        public bool DeleteInstallation(PTMSEntities context, string installationId)
        {

            MTN_INSTALLATION_DETAIL item = context.MTN_INSTALLATION_DETAIL.SingleOrDefault(d => d.ID == installationId);
            if (item != null)
                context.MTN_INSTALLATION_DETAIL.Remove(item);
            if (context.SaveChanges() > 0)
                return true;
            else
                return false;

        }

        /// <summary>
        /// Add a new installation audit records
        /// </summary>
        /// <param name="audit">Id primary key</param>
        /// <returns>True indicates success, False indicates failure</returns>
        public bool AddInstallationAudit(PTMSEntities context, InstallationAudit audit)
        {
            MTN_INSTALLATION_AUDIT item = new MTN_INSTALLATION_AUDIT();
            if (audit.Id != null) item.INSTALL_ID = audit.Id;
            //if (audit.SelfInspectId != null) item.INSPECT_ID = audit.SelfInspectId;
            //if (audit.SelfInspectCheck != null) item.INSPECT_CHECK = short.Parse(audit.SelfInspectCheck.ToString());
            if (audit.GpsCheck != null) item.GPS_CHECK = short.Parse(audit.GpsCheck.ToString());
            if (audit.AlarmCheck != null) item.ALARM_CHECK = short.Parse(audit.AlarmCheck.ToString());
            if (audit.AlarmId != null) item.ALARM_ID = audit.AlarmId.ToString();
            //if (audit.VideoCheck != null) item.VIDEO_CHECK = short.Parse(audit.VideoCheck.ToString());
            //if (audit.VideoQualityCheck != null) item.VIDEO_QUALITY_CHECK = audit.VideoQualityCheck;
            if (audit.IsSuccess != null) item.AUDIT_FLAG = short.Parse(audit.IsSuccess.ToString());
            if (audit.Approver != null) item.APPROVER = audit.Approver;
            if (audit.ApproverTime != null) item.APPROVE_TIME = DateTime.Parse(audit.ApproverTime.ToString());
            if (audit.Content != null) item.CONTENT = audit.Content;
            //if (audit.VideoFileId != null) item.VIDEO_FILE = audit.VideoFileId;
            //if (audit.VideoFileSize != null) item.VIDEO_FILE_SIZE = audit.VideoFileSize;
            item.AUDIT_FLAG = -1;

            context.MTN_INSTALLATION_AUDIT.Add(item);
            if (context.SaveChanges() > 0)
                return true;
            else
                return false;

        }

        /// <summary>
        /// Delete the installation audit records based on Id primary key
        /// </summary>
        /// <param name="auditId">Id primary key installation audit records</param>
        /// <returns>True indicates success, False indicates failure</returns>
        public bool DeleteInstallationAudit(PTMSEntities context, string auditId)
        {

            MTN_INSTALLATION_AUDIT item = context.MTN_INSTALLATION_AUDIT.SingleOrDefault(d => d.INSTALL_ID == auditId);
            if (item != null)
                context.MTN_INSTALLATION_AUDIT.Remove(item);
            if (context.SaveChanges() > 0)
                return true;
            else
                return false;

        }

        /// <summary>
        /// Get a mount point Unfinished Installation Record
        /// </summary>
        /// <param name="installStationId">Installation point numbers, which are all empty</param>
        /// <returns></returns>
        public List<InstallationInfo> GetInstallationInProgress(PTMSEntities context, string installStationId)
        {

            List<InstallationInfo> InstallationInfo = new List<InstallationInfo>();
            var result = context.MTN_INSTALLATION_DETAIL.Where(d => d.STATION_ID == installStationId && d.CHECKSTEP < 7).ToList();
            InstallationInfo = ConvertToInstallationInfo(result, context);
            return InstallationInfo;

        }


        /// <summary>
        /// Get unfinished Installation Record (fuzzy query)
        /// </summary>
        /// <param name="carNumber">License plate number</param>
        /// <param name="suiteId">Security Suite No.</param>
        /// <param name="installer">Installer</param>
        /// <param name="installStationId">Mounting pointsid</param>
        /// <returns></returns>
        public List<InstallationInfo> GetInstallationInProgressEx(PTMSEntities context, string clientID, string carNumber, string suiteId, string installer, List<string> installStationIds, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo, out int totalCount)
        {
            if (!string.IsNullOrEmpty(suiteId))
            {
                var result = (from x in context.MTN_INSTALLATION_DETAIL.Where(item => item.VALID == 1 && item.CHECKSTEP < 7)
                              join y in context.BSC_VEHICLE.Where(item => item.VALID == 1) on x.VEHICLE_ID equals y.VEHICLE_ID
                              join z in context.BSC_SETUP_STATION.Where(item => item.VALID == 1) on x.STATION_ID equals z.ID
                              join e in context.BSC_DEV_SUITE on x.SUITE_INFO_ID equals e.SUITE_INFO_ID
                              join vehicleType in context.BSC_VEHICLE_TYPE on y.VEHICLE_TYPE equals vehicleType.ID
                              where y.CLIENT_ID == clientID && (string.IsNullOrEmpty(carNumber) ? true : x.VEHICLE_ID.ToLower().Contains(carNumber.ToLower())) &&
                                    e.SUITE_ID.ToUpper().Contains(suiteId.ToUpper()) &&
                                    (string.IsNullOrEmpty(installer) ? true : x.INSTALL_STAFF.Contains(installer)) &&
                                    installStationIds.Contains(x.STATION_ID) &&
                                    (beginDate == null ? true : x.CREATE_TIME >= beginDate) &&
                                    (endDate == null ? true : x.CREATE_TIME <= endDate)
                                    && e.STATUS != (short)DeviceSuiteStatus.History
                              select new InstallationInfo
                              {
                                  Id = x.ID,
                                  VehicleID = x.VEHICLE_ID,
                                  DeviceCoreId = x.MDVR_CORE_SN,
                                  DeviceKey = x.SUITE_INFO_ID,
                                  DeviceSN = e.SUITE_ID,
                                  InstallationStationId = x.STATION_ID,
                                  InstallationStationName = z.NAME,
                                  InstallationStaff = x.INSTALL_STAFF,
                                  RecordStaff = x.RECORD_STAFF,
                                  CreateTime = x.CREATE_TIME,
                                  FinishTime = x.FINISH_TIME,
                                  Note = x.NOTE,
                                  CheckStep = x.CHECKSTEP,
                                  VehicleTypeName = vehicleType.NAME
                              });



                if (pageInfo.PageIndex == -1)
                {
                    pageInfo.PageIndex = 0;
                }

                totalCount = result.Count();
                return result.OrderByDescending(x => x.CreateTime).Skip(pageInfo.PageSize * pageInfo.PageIndex).Take(pageInfo.PageSize).ToList();

            }
            else
            {
                var result = from x in context.MTN_INSTALLATION_DETAIL.Where(item => item.VALID == 1 && item.CHECKSTEP < 7)
                             join y in context.BSC_VEHICLE.Where(item => item.VALID == 1) on x.VEHICLE_ID equals y.VEHICLE_ID
                             join vehicleType in context.BSC_VEHICLE_TYPE on y.VEHICLE_TYPE equals vehicleType.ID
                             join z in context.BSC_SETUP_STATION.Where(item => item.VALID == 1) on x.STATION_ID equals z.ID
                             //from id in installStationIds
                             where (string.IsNullOrEmpty(carNumber) ? true : x.VEHICLE_ID.ToLower().Contains(carNumber.ToLower())) &&
                                   (string.IsNullOrEmpty(installer) ? true : x.INSTALL_STAFF.Contains(installer)) &&
                                 installStationIds.Contains(x.STATION_ID) &&
                                   (beginDate == null ? true : x.CREATE_TIME >= beginDate) &&
                                   (endDate == null ? true : x.CREATE_TIME <= endDate)
                             let suite = context.BSC_DEV_SUITE.FirstOrDefault(d => d.SUITE_INFO_ID == x.SUITE_INFO_ID)
                             select new InstallationInfo
                             {
                                 Id = x.ID,
                                 VehicleID = x.VEHICLE_ID,
                                 VehicleTypeName = vehicleType.NAME,
                                 DeviceCoreId = x.MDVR_CORE_SN,
                                 DeviceKey = x.SUITE_INFO_ID,

                                 DeviceSN = suite != null ? suite.SUITE_ID : null,
                                 InstallationStationId = x.STATION_ID,
                                 InstallationStationName = z.NAME,
                                 InstallationStaff = x.INSTALL_STAFF,
                                 RecordStaff = x.RECORD_STAFF,
                                 CreateTime = x.CREATE_TIME,
                                 FinishTime = x.FINISH_TIME,
                                 Note = x.NOTE,
                                 CheckStep = x.CHECKSTEP
                             };

                result = from a in result
                         from id in installStationIds
                         where id == a.InstallationStationId
                         select a;

                if (pageInfo.PageIndex == -1)
                {
                    pageInfo.PageIndex = 0;
                }

                totalCount = result.Count();
                return result.OrderByDescending(x => x.CreateTime).Skip(pageInfo.PageSize * pageInfo.PageIndex).Take(pageInfo.PageSize).ToList();
            }
        }

        /// <summary>
        /// Get unfinished Installation Record (fuzzy query)
        /// </summary>
        /// <param name="carNumber">License plate number</param>
        /// <param name="suiteId">Security Suite No.</param>
        /// <param name="installer">Installer</param>
        /// <param name="installStationId">Mounting pointsid</param>
        /// <returns></returns>
        public List<InstallationInfo> GetGPSInstallationInProgressEx(PTMSEntities context, string carNumber, string gpsid, string installer, List<string> installStationIds, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo, out int totalCount)
        {
            if (!string.IsNullOrEmpty(gpsid))
            {
                var result = (from x in context.MTN_GPS_INSTALLATION_DETAIL.Where(item => item.VALID == 1 && item.CHECKSTEP < 4)
                              join y in context.BSC_VEHICLE.Where(item => item.VALID == 1) on x.VEHICLE_ID equals y.VEHICLE_ID
                              join vehicleType in context.BSC_VEHICLE_TYPE on y.VEHICLE_TYPE equals vehicleType.ID
                              join z in context.BSC_SETUP_STATION.Where(item => item.VALID == 1) on x.STATION_ID equals z.ID
                              where (string.IsNullOrEmpty(carNumber) ? true : x.VEHICLE_ID.ToLower().Contains(carNumber.ToLower())) &&
                                    (string.IsNullOrEmpty(installer) ? true : x.INSTALL_STAFF.Contains(installer)) &&
                                     installStationIds.Contains(x.STATION_ID) &&
                                    (beginDate == null ? true : x.CREATE_TIME >= beginDate) &&
                                    (endDate == null ? true : x.CREATE_TIME <= endDate)
                              let gps = context.BSC_DEV_GPS.FirstOrDefault(d => d.ID == x.GPS_ID)
                              select new InstallationInfo
                              {
                                  Id = x.ID,
                                  VehicleID = x.VEHICLE_ID,
                                  VehicleTypeName = vehicleType.NAME,
                                  DeviceKey = gps != null ? gps.ID : null,
                                  DeviceSN = gps != null ? gps.GPS_SN : null,
                                  DeviceCoreId = gps != null ? gps.GPS_UID : null,
                                  InstallationStationId = x.STATION_ID,
                                  InstallationStationName = z.NAME,
                                  InstallationStaff = x.INSTALL_STAFF,
                                  RecordStaff = x.RECORD_STAFF,
                                  CreateTime = x.CREATE_TIME,
                                  FinishTime = x.FINISH_TIME,
                                  Note = x.NOTE,
                                  CheckStep = x.CHECKSTEP
                              });

                result = result.Where(n => n.DeviceSN.ToUpper().Contains(gpsid.ToUpper()));

                if (pageInfo.PageIndex == -1)
                {
                    pageInfo.PageIndex = 0;
                }

                totalCount = result.Count();
                return result.OrderByDescending(x => x.CreateTime).Skip(pageInfo.PageSize * pageInfo.PageIndex).Take(pageInfo.PageSize).ToList();

            }
            else
            {
                var result = (from x in context.MTN_GPS_INSTALLATION_DETAIL.Where(item => item.VALID == 1 && item.CHECKSTEP < 4)
                              join y in context.BSC_VEHICLE.Where(item => item.VALID == 1) on x.VEHICLE_ID equals y.VEHICLE_ID
                              join z in context.BSC_SETUP_STATION.Where(item => item.VALID == 1) on x.STATION_ID equals z.ID
                              join vehicleType in context.BSC_VEHICLE_TYPE on y.VEHICLE_TYPE equals vehicleType.ID
                              //from id in installStationIds
                              where (string.IsNullOrEmpty(carNumber) ? true : x.VEHICLE_ID.ToLower().Contains(carNumber.ToLower())) &&
                                    (string.IsNullOrEmpty(installer) ? true : x.INSTALL_STAFF.Contains(installer)) &&
                                  installStationIds.Contains(x.STATION_ID) &&
                                    (beginDate == null ? true : x.CREATE_TIME >= beginDate) &&
                                    (endDate == null ? true : x.CREATE_TIME <= endDate)
                              let gps = context.BSC_DEV_GPS.FirstOrDefault(d => d.ID == x.GPS_ID)
                              select new InstallationInfo
                              {
                                  Id = x.ID,
                                  VehicleID = x.VEHICLE_ID,
                                  VehicleTypeName = vehicleType.NAME,
                                  DeviceKey = gps != null ? gps.ID : null,
                                  DeviceSN = gps != null ? gps.GPS_SN : null,
                                  DeviceCoreId = gps != null ? gps.GPS_UID : null,
                                  InstallationStationId = x.STATION_ID,
                                  InstallationStationName = z.NAME,
                                  InstallationStaff = x.INSTALL_STAFF,
                                  RecordStaff = x.RECORD_STAFF,
                                  CreateTime = x.CREATE_TIME,
                                  FinishTime = x.FINISH_TIME,
                                  Note = x.NOTE,
                                  CheckStep = x.CHECKSTEP
                              });

                result = from a in result
                         from id in installStationIds
                         where id == a.InstallationStationId
                         select a;

                if (pageInfo.PageIndex == -1)
                {
                    pageInfo.PageIndex = 0;
                }

                totalCount = result.Count();
                return result.OrderByDescending(x => x.CreateTime).Skip(pageInfo.PageSize * pageInfo.PageIndex).Take(pageInfo.PageSize).ToList();
            }
        }


        /// <summary>
        /// Get unfinished Installation Record, fuzzy query, paging query
        /// </summary>
        /// <param name="carNumber">License plate number</param>
        /// <param name="suiteId">Security Suite No.</param>
        /// <param name="installStationId">Mounting points</param>
        /// <param name="beginDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="pageInfo">Paging Information</param>
        /// <returns></returns>
        public MultiMessage<InstallationInfo> GetInstallationInProgressFuzzy(PTMSEntities context, UserInfoMessageHeader userInfo, string carNumber, string suiteId, string installStationId,
            DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo)
        {
            if (endDate.HasValue)
            {
                endDate = ((DateTime)endDate).AddDays(1).Date;
            }

            if (!string.IsNullOrEmpty(suiteId))
            {
                var source = (from x in context.MTN_INSTALLATION_DETAIL.Where(item => item.VALID == 1)
                              join y in context.BSC_VEHICLE.Where(item => item.VALID == 1) on x.VEHICLE_ID equals y.VEHICLE_ID
                              join z in context.BSC_SETUP_STATION.Where(item => item.VALID == 1) on x.STATION_ID equals z.ID
                              join d in context.DISTRICT_LEVEL_VIEW.Where(base.MakeDistrictCondtions<DISTRICT_LEVEL_VIEW>(userInfo)) on z.DISTRICT_CODE equals d.CODE
                              join e in context.BSC_DEV_SUITE.Where(item => item.STATUS != (short)DeviceSuiteStatus.History) on x.SUITE_INFO_ID equals e.SUITE_INFO_ID
                              where (string.IsNullOrEmpty(carNumber) ? true : x.VEHICLE_ID.ToLower().Contains(carNumber.ToLower())) &&
                                    (string.IsNullOrEmpty(suiteId) ? true : e.SUITE_ID.Contains(suiteId)) &&
                                    (string.IsNullOrEmpty(installStationId) ? true : x.STATION_ID.Contains(installStationId)) &&
                                    (beginDate == null ? true : x.CREATE_TIME >= beginDate) &&
                                    (endDate == null ? true : x.CREATE_TIME < endDate) &&
                                    (x.CHECKSTEP < 7)
                              select new InstallationInfo
                              {
                                  Id = x.ID,
                                  VehicleID = x.VEHICLE_ID,
                                  //VehicleType = y.VEHICLE_TYPE,
                                  DeviceCoreId = x.MDVR_CORE_SN,
                                  DeviceSN = e.SUITE_ID,
                                  InstallationStationId = x.STATION_ID,
                                  InstallationStationName = z.NAME,
                                  InstallationStaff = x.INSTALL_STAFF,
                                  RecordStaff = x.RECORD_STAFF,
                                  CreateTime = x.CREATE_TIME,
                                  FinishTime = x.FINISH_TIME,
                                  Note = x.NOTE,
                                  CheckStep = x.CHECKSTEP,

                              }).ToList();
                MultiMessage<InstallationInfo> result = new MultiMessage<InstallationInfo>();
                result.TotalRecord = source.Count();

                if (pageInfo == null || pageInfo.PageIndex == -1)
                {
                    result.Result = source.ToList();
                }
                else
                {
                    result.Result = source.OrderBy(x => x.Id).Skip(pageInfo.PageSize * (pageInfo.PageIndex - 1)).Take(pageInfo.PageSize).ToList();
                }

                return result;
            }
            else
            {
                var source = (from x in context.MTN_INSTALLATION_DETAIL
                              join y in context.BSC_VEHICLE on x.VEHICLE_ID equals y.VEHICLE_ID
                              join z in context.BSC_SETUP_STATION on x.STATION_ID equals z.ID
                              join d in context.DISTRICT_LEVEL_VIEW.Where(base.MakeDistrictCondtions<DISTRICT_LEVEL_VIEW>(userInfo)) on z.DISTRICT_CODE equals d.CODE
                              where (string.IsNullOrEmpty(carNumber) ? true : x.VEHICLE_ID.ToLower().Contains(carNumber.ToLower())) &&
                                    (string.IsNullOrEmpty(installStationId) ? true : x.STATION_ID.Contains(installStationId)) &&
                                    (beginDate == null ? true : x.CREATE_TIME >= beginDate) &&
                                    (endDate == null ? true : x.CREATE_TIME < endDate) &&
                                    (x.CHECKSTEP < 7)
                              select new InstallationInfo
                              {
                                  Id = x.ID,
                                  VehicleID = x.VEHICLE_ID,
                                  //VehicleType = y.VEHICLE_TYPE,
                                  DeviceCoreId = x.MDVR_CORE_SN,
                                  DeviceSN = context.BSC_DEV_SUITE.Where(item => item.STATUS != (short)DeviceSuiteStatus.History).FirstOrDefault(s => s.SUITE_INFO_ID.Equals(x.SUITE_INFO_ID)).SUITE_ID,
                                  InstallationStationId = x.STATION_ID,
                                  InstallationStationName = z.NAME,
                                  InstallationStaff = x.INSTALL_STAFF,
                                  RecordStaff = x.RECORD_STAFF,
                                  CreateTime = x.CREATE_TIME,
                                  FinishTime = x.FINISH_TIME,
                                  Note = x.NOTE,
                                  CheckStep = x.CHECKSTEP,
                              }).ToList();
                MultiMessage<InstallationInfo> result = new MultiMessage<InstallationInfo>();
                result.TotalRecord = source.Count();

                if (pageInfo == null || pageInfo.PageIndex == -1)
                {
                    result.Result = source.ToList();
                }
                else
                {
                    result.Result = source.OrderBy(x => x.Id).Skip(pageInfo.PageSize * (pageInfo.PageIndex - 1)).Take(pageInfo.PageSize).ToList();
                }

                return result;
            }

        }

        /// <summary>
        /// Get completed Installation Record
        /// </summary>
        /// <param name="installStationId">Mounting points</param>
        /// <param name="beginDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <returns></returns>
        public List<InstallationInfo> GetInstallationFinished(PTMSEntities context, string installStationId, DateTime beginDate, DateTime endDate)
        {

            List<InstallationInfo> InstallationInfo = new List<InstallationInfo>();
            var result = context.MTN_INSTALLATION_DETAIL.Where(d => d.STATION_ID == installStationId && d.FINISH_TIME >= beginDate && d.FINISH_TIME <= endDate && d.CHECKSTEP == 7).ToList();
            InstallationInfo = ConvertToInstallationInfo(result, context);
            return InstallationInfo;

        }

        /// <summary>
        /// Get completed Installation Record, fuzzy query
        /// </summary>
        /// <param name="carNumber">License plate number</param>
        /// <param name="suiteId">Security Suite No.</param>
        /// <param name="installer">Installer</param>
        /// <param name="installStationId">Mounting points</param>
        /// <param name="beginDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <returns></returns>
        public List<InstallationInfo> GetInstallationFinishedEx(PTMSEntities context, string carNumber, string suiteId, string installer, string installStationId, DateTime beginDate, DateTime endDate)
        {

            List<InstallationInfo> InstallationInfo = new List<InstallationInfo>();
            var result = context.MTN_INSTALLATION_DETAIL.Where(d => d.CHECKSTEP == 7).ToList();
            if (!string.IsNullOrEmpty(carNumber))
                result = result.Where(d => d.VEHICLE_ID.Contains(carNumber)).ToList();
            if (!string.IsNullOrEmpty(suiteId))
                result = result.Where(d => d.SUITE_INFO_ID.Contains(suiteId)).ToList();
            if (!string.IsNullOrEmpty(installer))
                result = result.Where(d => d.INSTALL_STAFF.Contains(installer)).ToList();
            if (!string.IsNullOrEmpty(installStationId))
                result = result.Where(d => d.STATION_ID.Contains(installStationId)).ToList();
            if (beginDate != null)
                result = result.Where(d => d.FINISH_TIME >= beginDate).ToList();
            if (endDate != null)
                result = result.Where(d => d.FINISH_TIME <= endDate).ToList();
            InstallationInfo = ConvertToInstallationInfo(result, context);
            return InstallationInfo;
        }


        /// <summary>
        /// Get completed Installation Record, fuzzy query, paging query
        /// </summary>
        /// <param name="carNumber">License plate number</param>
        /// <param name="suiteId">Security Suite No.</param>
        /// <param name="installStationId">Mounting points</param>
        /// <param name="beginDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="pageInfo">Paging Information</param>
        /// <returns></returns>
        public MultiMessage<InstallationInfo> GetInstallationFinishedFuzzy(PTMSEntities context, UserInfoMessageHeader userInfo, string carNumber, string suiteId, string installStationId, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo)
        {
            MultiMessage<InstallationInfo> infoList = null;

            endDate = ((DateTime)endDate).AddDays(1).Date;

            var result = (from detail in context.MTN_INSTALLATION_DETAIL.Where(item => item.VALID == 1)
                          join suite in context.BSC_DEV_SUITE.Where(item => item.STATUS == (short)DeviceSuiteStatus.Working) on detail.SUITE_INFO_ID equals suite.SUITE_INFO_ID
                          join vehicle in context.BSC_VEHICLE.Where(item => item.VALID == 1) on detail.VEHICLE_ID equals vehicle.VEHICLE_ID
                          join setupStation in context.BSC_SETUP_STATION.Where(item => item.VALID == 1) on detail.STATION_ID equals setupStation.ID
                          join d in context.DISTRICT_LEVEL_VIEW.Where(base.MakeDistrictCondtions<DISTRICT_LEVEL_VIEW>(userInfo)) on setupStation.DISTRICT_CODE equals d.CODE
                          where (string.IsNullOrEmpty(carNumber.Trim()) ? true : detail.VEHICLE_ID.ToLower().Contains(carNumber.Trim().ToLower())) &&
                          (string.IsNullOrEmpty(suiteId.Trim()) ? true : suite.SUITE_ID.Contains(suiteId.Trim())) &&
                          (string.IsNullOrEmpty(installStationId.Trim()) ? true : detail.STATION_ID.Equals(installStationId.Trim())) &&
                          detail.FINISH_TIME >= beginDate &&
                          detail.FINISH_TIME < endDate &&
                          detail.CHECKSTEP >= 7 &&
                          detail.VALID == 1 &&
                          vehicle.VALID == 1 &&
                          setupStation.VALID == 1
                          select new InstallationInfo
                          {
                              Id = detail.ID,
                              VehicleID = detail.VEHICLE_ID,
                              //VehicleType = vehicle.VEHICLE_TYPE,
                              DeviceSN = suite.SUITE_ID,
                              InstallationStationName = setupStation.NAME,
                              InstallationStaff = detail.INSTALL_STAFF,
                              RecordStaff = detail.RECORD_STAFF,
                              FinishTime = detail.FINISH_TIME,
                              CreateTime = detail.CREATE_TIME,

                          });

            infoList = new MultiMessage<InstallationInfo>
            {
                Result = result.OrderBy(i => i.VehicleID).Skip(pageInfo.PageSize * (pageInfo.PageIndex - 1)).Take(pageInfo.PageSize).ToList(),
                TotalRecord = result.OrderBy(i => i.VehicleID).Count()

            };

            return infoList;
        }


        /// <summary>
        /// Get completed Installation Record, fuzzy query, paging query
        /// </summary>
        /// <param name="carNumber">License plate number</param>
        /// <param name="suiteId">Security Suite No.</param>
        /// <param name="installer">Installer</param>
        /// <param name="installStationId">Mounting points</param>
        /// <param name="beginDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="pageSize">The number of entries per page</param>
        /// <param name="pageIndex">Pages</param>
        /// <param name="totalCount">The total number of entries</param>
        /// <returns></returns>
        ///     
        public List<InstallationInfo> GetInstallationFinishedEx1(PTMSEntities context, string clientID, string carNumber, string suiteId, string installer, List<string> installStationId, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo, out int totalCount)
        {

            var result = (from x in context.MTN_INSTALLATION_DETAIL
                          join y in context.BSC_VEHICLE on x.VEHICLE_ID equals y.VEHICLE_ID
                          join z in context.BSC_SETUP_STATION on x.STATION_ID equals z.ID
                          join e in context.BSC_DEV_SUITE on x.SUITE_INFO_ID equals e.SUITE_INFO_ID
                          join vehicleType in context.BSC_VEHICLE_TYPE on y.VEHICLE_TYPE equals vehicleType.ID
                          where y.CLIENT_ID == clientID && (string.IsNullOrEmpty(carNumber) ? true : x.VEHICLE_ID.ToLower().Contains(carNumber.ToLower())) &&
                                (string.IsNullOrEmpty(suiteId) ? true : e.SUITE_ID.Contains(suiteId)) &&
                                (string.IsNullOrEmpty(installer) ? true : x.INSTALL_STAFF.Contains(installer)) &&
                                 installStationId.Contains(x.STATION_ID) &&
                                (beginDate == null ? true : x.FINISH_TIME >= beginDate) &&
                                (endDate == null ? true : x.FINISH_TIME < endDate) &&
                                (x.CHECKSTEP >= 7) && y.VALID == 1 && z.VALID == 1
                          select new InstallationInfo
                          {
                              Id = x.ID,
                              VehicleID = x.VEHICLE_ID,
                              VehicleTypeName = vehicleType.NAME,
                              DeviceCoreId = x.MDVR_CORE_SN,
                              DeviceKey = x.SUITE_INFO_ID,
                              DeviceSN = e.SUITE_ID,
                              InstallationStationId = x.STATION_ID,
                              InstallationStationName = z.NAME,
                              InstallationStaff = x.INSTALL_STAFF,
                              RecordStaff = x.RECORD_STAFF,
                              CreateTime = x.CREATE_TIME,
                              FinishTime = x.FINISH_TIME,
                              Note = x.NOTE,
                              CheckStep = x.CHECKSTEP
                          });
            if (pageInfo.PageIndex == -1)
            {
                pageInfo.PageIndex = 0;
            }

            totalCount = result.Count();
            return result.OrderBy(x => x.Id).Skip(pageInfo.PageSize * pageInfo.PageIndex).Take(pageInfo.PageSize).ToList();

        }

        public List<InstallationAuditCollect> GetInstallationAudit(PTMSEntities context, string carNumber, string suiteId, string installer, string installStationId, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo, bool onlyWaitCheck, out int totalCount)
        {
            short? auditflag;
            if (onlyWaitCheck)
            {
                auditflag = 0;
            }
            else
            {
                auditflag = 1;
            }

            var result = (from x in context.MTN_INSTALLATION_DETAIL
                          join y in context.MTN_INSTALLATION_AUDIT on x.ID equals y.INSTALL_ID
                          join z in context.BSC_DEV_SUITE on x.SUITE_INFO_ID equals z.SUITE_INFO_ID
                          where (string.IsNullOrEmpty(carNumber) ? true : x.VEHICLE_ID.Contains(carNumber)) &&
                                (string.IsNullOrEmpty(suiteId) ? true : x.SUITE_INFO_ID.Contains(suiteId)) &&
                                (string.IsNullOrEmpty(installer) ? true : x.INSTALL_STAFF.Contains(installer)) &&
                                (string.IsNullOrEmpty(installStationId) ? true : x.STATION_ID.Contains(installStationId)) &&
                                (beginDate == null ? true : x.CREATE_TIME >= beginDate) &&
                                (endDate == null ? true : x.CREATE_TIME <= endDate) &&
                                (auditflag == 1 ? true : y.AUDIT_FLAG == auditflag) &&
                                x.VALID == 1
                          select new InstallationAuditCollect
                          {
                              VehicleID = x.VEHICLE_ID,
                              SuiteID = z.SUITE_ID,
                              //SelfInspectCheck = y.INSPECT_CHECK,
                              AlarmCheck = y.ALARM_CHECK,
                              GpsCheck = y.GPS_CHECK,
                              //VideoCheck = y.VIDEO_CHECK,
                              IsSuccess = y.AUDIT_FLAG,
                              Approver = y.APPROVER,
                              ApproverTime = y.APPROVE_TIME,
                              Content = y.CONTENT
                          }).ToList();
            if (pageInfo == null || pageInfo.PageIndex == -1)
            {
                totalCount = 1;
                return null;
            }
            else
            {
                totalCount = result.Count();
                return result.OrderByDescending(x => x.ApproverTime).Skip(pageInfo.PageSize * pageInfo.PageIndex).Take(pageInfo.PageSize).ToList();
            }

        }


        public List<InstallationInfo> ConvertToInstallationInfo(List<MTN_INSTALLATION_DETAIL> result, PTMSEntities context)
        {
            List<InstallationInfo> list = new List<InstallationInfo>();
            foreach (var item in result)
            {
                InstallationInfo info = new InstallationInfo();
                info.Id = item.ID;
                info.VehicleID = item.VEHICLE_ID;
                //if (item.VEHICLE_ID != null)
                //    info.VehicleType = context.VEHICLE.SingleOrDefault(d => d.VEHICLE_ID == item.VEHICLE_ID).VEHICLE_TYPE;
                info.DeviceCoreId = item.MDVR_CORE_SN;
                info.DeviceSN = item.SUITE_INFO_ID;
                info.InstallationStationId = item.STATION_ID;
                if (item.STATION_ID != null)
                    info.InstallationStationName = context.BSC_SETUP_STATION.Single(d => d.ID == item.STATION_ID).NAME;
                info.InstallationStaff = item.INSTALL_STAFF;
                info.RecordStaff = item.RECORD_STAFF;
                if (item.CREATE_TIME != null)
                    info.CreateTime = DateTime.Parse(item.CREATE_TIME.ToString());
                if (item.FINISH_TIME != null)
                    info.FinishTime = DateTime.Parse(item.FINISH_TIME.ToString());
                info.Note = item.NOTE;
                if (item.CHECKSTEP != null)
                    info.CheckStep = Convert.ToInt32(item.CHECKSTEP);
                list.Add(info);
            }
            return list;
        }


        public bool CheckMediaFile(PTMSEntities context, string InstallId, string size)
        {
            PTMSAppConfigRepository AppConfig = new PTMSAppConfigRepository();
            int deploySize;
            if (AppConfig.GetappConfigInfo(context, "InstallVideoMinFileSize") != null)
            {
                deploySize = int.Parse(AppConfig.GetappConfigInfo(context, "InstallVideoMinFileSize").SECTION_VALUE);
            }
            else
            {
                deploySize = 100000;
            }

            if (int.Parse(size) >= deploySize)
            {
                MTN_INSTALLATION_AUDIT item = context.MTN_INSTALLATION_AUDIT.SingleOrDefault(d => d.INSTALL_ID == InstallId);
                if (item != null)
                {
                    //item.VIDEO_CHECK = 1;
                }
                if (context.SaveChanges() > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                MTN_INSTALLATION_AUDIT item = context.MTN_INSTALLATION_AUDIT.SingleOrDefault(d => d.INSTALL_ID == InstallId);
                if (item != null)
                {
                    //item.VIDEO_CHECK = 0;
                }
                context.SaveChanges();
                return false;
            }

        }

        public SingleMessage<string> SubmitForStep1(PTMSEntities context, Step1Package step)
        {
            SingleMessage<string> result = new SingleMessage<string>();
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadUncommitted;
            var scope = new TransactionScope(TransactionScopeOption.Required, optons);


            try
            {
                BSC_ORDER_CLIENT client = context.BSC_ORDER_CLIENT.FirstOrDefault(n => n.ID == step.ClientID);
                if (client != null)
                {
                    int suitecount = context.RUN_SUITE_WORKING.Count(n => n.CLIENT_ID == step.ClientID);
                    int gpscount = context.RUN_GPS_WORKING.Count(n => n.CLIENT_ID == step.ClientID);

                    if (suitecount + gpscount < client.DEVICE_COUNT)
                    {

                        var vechile = (from x in context.BSC_VEHICLE
                                       where x.VEHICLE_ID == step.VehicleId && x.VALID == 1
                                       select x).FirstOrDefault();
                        if (vechile != null)
                        {
                            //update vehicle status
                            vechile.VEHICLE_STATUS = 1;
                            vechile.NOTE = step.Note;

                            //add install detail
                            MTN_INSTALLATION_DETAIL item = new MTN_INSTALLATION_DETAIL();
                            if (step.ID != null) item.ID = step.ID;
                            if (step.VehicleId != null) item.VEHICLE_ID = step.VehicleId;
                            if (step.InstallationStationId != null) item.STATION_ID = step.InstallationStationId;
                            if (step.InstallationStaff != null) item.INSTALL_STAFF = step.InstallationStaff;
                            if (step.RecordStaff != null) item.RECORD_STAFF = step.RecordStaff;
                            item.CHECKSTEP = short.Parse(step.Step.ToString());
                            if (step.CreateTime != null) item.CREATE_TIME = step.CreateTime;
                            item.VALID = 1;
                            context.MTN_INSTALLATION_DETAIL.Add(item);

                            if (context.SaveChanges() > 0)
                            {
                                result.IsSuccess = true;
                            }
                            else
                            {
                                result.IsSuccess = false;
                                result.Result = "SaveFailed";
                            }

                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Result = "NoFoundVehicle";
                        }


                        scope.Complete();
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Result = "MaxDeviceCount";
                    }
                }
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

        public SingleMessage<string> SubmitForStep2(PTMSEntities context, Step2Package step2)
        {
            SingleMessage<string> result = new SingleMessage<string>();
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadUncommitted;
            var scope = new TransactionScope(TransactionScopeOption.Required, optons);

            try
            {
                //add working suite
                RUN_SUITE_WORKING checkitem = context.RUN_SUITE_WORKING.FirstOrDefault(d => d.MDVR_CORE_SN == step2.MDVR_CORE_SN);
                //update installdetail
                MTN_INSTALLATION_DETAIL installdetail = context.MTN_INSTALLATION_DETAIL.SingleOrDefault(d => d.ID == step2.InstallID);

                BSC_DEV_SUITE suite = context.BSC_DEV_SUITE.FirstOrDefault(n => n.SUITE_INFO_ID == step2.SuiteId);
                if (checkitem == null && installdetail != null && suite != null)
                {
                    RUN_SUITE_WORKING item = new RUN_SUITE_WORKING();
                    if (step2.SuiteId != null) item.SUITE_INFO_ID = step2.SuiteId;
                    if (step2.VehicleId != null) item.VEHICLE_ID = step2.VehicleId;
                    if (step2.MDVR_CORE_SN != null) item.MDVR_CORE_SN = step2.MDVR_CORE_SN;
                    if (step2.SuiteStatus != null) item.STATUS = (short)DeviceSuiteStatus.Testing;
                    if (step2.SuiteSwitchTime != null) item.SWITCH_TIME = step2.SuiteSwitchTime;
                    if (step2.OnlineFlag != null) item.ONLINE_FLAG = (short?)step2.OnlineFlag;
                    if (step2.AbnormalCause != null) item.ABNORMAL_CAUSE = step2.AbnormalCause;
                    item.CLIENT_ID = step2.ClientID;
                    item.ORGANIZATION_ID = step2.Organization;
                    context.RUN_SUITE_WORKING.Add(item);

                    if (!string.IsNullOrEmpty(step2.VehicleId)) installdetail.VEHICLE_ID = step2.VehicleId;
                    if (!string.IsNullOrEmpty(step2.SuiteKey)) installdetail.SUITE_INFO_ID = step2.SuiteId;
                    if (!string.IsNullOrEmpty(step2.MDVR_CORE_SN)) installdetail.MDVR_CORE_SN = step2.MDVR_CORE_SN;
                    installdetail.CHECKSTEP = 2;

                    //add audit
                    MTN_INSTALLATION_AUDIT audit = new MTN_INSTALLATION_AUDIT();
                    audit.INSTALL_ID = step2.InstallID;
                    //if (step2.SelfInspectCheck != null)
                    //    audit.INSPECT_CHECK = step2.SelfInspectCheck;
                    if (step2.GpsCheck != null)
                        audit.GPS_CHECK = step2.GpsCheck;
                    if (step2.AlarmCheck != null)
                        audit.ALARM_CHECK = step2.AlarmCheck;
                    //if (step2.VideoCheck != null)
                    //    audit.VIDEO_CHECK = step2.VideoCheck;
                    if (step2.IsSuccess != null)
                        audit.AUDIT_FLAG = step2.IsSuccess;
                    context.MTN_INSTALLATION_AUDIT.Add(audit);

                    //update status
                    suite.STATUS = (short)DeviceSuiteStatus.Working;


                    if (context.SaveChanges() > 0)
                    {
                        result.IsSuccess = true;
                        scope.Complete();
                    }
                    else
                    {
                        result.IsSuccess = true;
                        result.Result = "SaveFailed";
                    }
                }
                else if (checkitem == null)
                {
                    result.IsSuccess = false;
                    result.Result = "ID_INSTALL_SuiteAlreadyInstalled";
                }
                else if (installdetail == null)
                {
                    result.IsSuccess = false;
                    result.Result = "InstallDetailMissing";
                }
                else if (suite == null)
                {
                    result.IsSuccess = false;
                    result.Result = "SuiteMissing";
                }

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

        public SingleMessage<string> SubmitForStep6(PTMSEntities context, Step6Package step)
        {
            SingleMessage<string> result = new SingleMessage<string>();
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadUncommitted;
            var scope = new TransactionScope(TransactionScopeOption.Required, optons);


            try
            {

                MTN_INSTALLATION_DETAIL item = context.MTN_INSTALLATION_DETAIL.SingleOrDefault(d => d.ID == step.Id);
                item.FINISH_TIME = step.FinishTime;
                item.CHECKSTEP = step.CheckStep;

                RUN_SUITE_WORKING suite = context.RUN_SUITE_WORKING.Single(d => d.MDVR_CORE_SN == step.MdvrCoreId);
                suite.STATUS = step.Status;

                if (context.SaveChanges() > 0)
                {
                    result.IsSuccess = true;
                    scope.Complete();
                }
                else
                {
                    result.IsSuccess = true;
                    result.Result = "SaveFailed";
                }

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

        public SingleMessage<string> SubmitForMaintenance(PTMSEntities context, MaintenancePackage package)
        {
            SingleMessage<string> result = new SingleMessage<string>();
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadCommitted;
            var scope = new TransactionScope(TransactionScopeOption.Required, optons);

            try
            {
                //add working suite
                RUN_SUITE_WORKING workingsuite = context.RUN_SUITE_WORKING.FirstOrDefault(d => d.SUITE_INFO_ID == package.SuiteKey);
                if (workingsuite != null)
                {
                    context.RUN_SUITE_WORKING.Remove(workingsuite);
                }
                //delete installaudit
                MTN_INSTALLATION_AUDIT audit = context.MTN_INSTALLATION_AUDIT.SingleOrDefault(d => d.INSTALL_ID == package.InstallID);
                if (audit != null)
                {
                    context.MTN_INSTALLATION_AUDIT.Remove(audit);
                }

                //update installdetail
                MTN_INSTALLATION_DETAIL installdetail = context.MTN_INSTALLATION_DETAIL.SingleOrDefault(d => d.ID == package.InstallID);

                if (installdetail != null)
                {
                    context.MTN_INSTALLATION_DETAIL.Remove(installdetail);
                }

                BSC_DEV_SUITE suite = context.BSC_DEV_SUITE.FirstOrDefault(n => n.SUITE_INFO_ID == package.SuiteKey);

                if (suite != null)
                {
                    //update suite status
                    suite.STATUS = package.DeviceStatus;
                }

                if (context.SaveChanges() > 0)
                {
                    result.IsSuccess = true;

                    scope.Complete();
                }
                else
                {
                    result.IsSuccess = true;
                    result.Result = "SaveFailed";
                }

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

        public SingleMessage<string> SubmitForDelete(PTMSEntities context, DeletePackage package)
        {
            SingleMessage<string> result = new SingleMessage<string>();
            TransactionOptions optons = new TransactionOptions();
            var scope = new TransactionScope(TransactionScopeOption.Required, optons);

            try
            {
                //add working suite
                RUN_SUITE_WORKING workingsuite = context.RUN_SUITE_WORKING.FirstOrDefault(d => d.SUITE_INFO_ID == package.SuiteKey);
                if (workingsuite != null)
                {
                    context.RUN_SUITE_WORKING.Remove(workingsuite);
                }
                //delete installaudit
                MTN_INSTALLATION_AUDIT audit = context.MTN_INSTALLATION_AUDIT.SingleOrDefault(d => d.INSTALL_ID == package.InstallID);
                if (audit != null)
                {
                    context.MTN_INSTALLATION_AUDIT.Remove(audit);

                    var partAudits = context.MTN_PART_AUDIT.Where(t => t.INSTALL_AUDIT_ID == audit.INSTALL_ID).ToList();
                    foreach (var item in partAudits)
                    {
                        context.MTN_PART_AUDIT.Remove(item);
                    }
                }

                //update installdetail
                MTN_INSTALLATION_DETAIL installdetail = context.MTN_INSTALLATION_DETAIL.SingleOrDefault(d => d.ID == package.InstallID);

                if (installdetail != null)
                {
                    context.MTN_INSTALLATION_DETAIL.Remove(installdetail);
                }

                BSC_DEV_SUITE suite = context.BSC_DEV_SUITE.FirstOrDefault(n => n.SUITE_INFO_ID == package.SuiteKey);

                if (suite != null)
                {
                    //update suite status
                    suite.STATUS = (int)DeviceSuiteStatus.Initial;
                }


                if (context.SaveChanges() > 0)
                {
                    result.IsSuccess = true;
                    scope.Complete();
                }
                else
                {
                    result.IsSuccess = true;
                    result.Result = "SaveFailed";
                }
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

        public MultiMessage<CameraInfo> GetCameraInfoByMdvrID(PTMSEntities context, string mdvrID)
        {
            var detail = context.MTN_INSTALLATION_DETAIL
                .Where(t => t.MDVR_CORE_SN == mdvrID).OrderByDescending(t => t.CREATE_TIME).FirstOrDefault();

            if (detail != null)
            {
                var partAudits = context.MTN_PART_AUDIT.Where(t => t.INSTALL_AUDIT_ID == detail.ID).ToList();
                var result = partAudits.Select(t => new CameraInfo()
                  {
                      ChannelID = t.CHANNEL_ID.ToString(),
                      InstallLocation = (CameraInstallLocationEnum)t.INSTALL_LOCATION,
                      SuitPartID = t.PART_ID,
                  }).ToList();

                return new MultiMessage<CameraInfo>(result, result.Count);
            }

            return new MultiMessage<CameraInfo>(new List<CameraInfo>(), 0);

            //var list = new List<CameraInfo>();
            //list.Add(new CameraInfo()
            //{
            //    ChannelID = "0",
            //    InstallLocation = CameraInstallLocationEnum.InnerLeftDriver
            //});
            //list.Add(new CameraInfo()
            //{
            //    ChannelID = "1",
            //    InstallLocation = CameraInstallLocationEnum.InnerRightDriver
            //});
            //list.Add(new CameraInfo()
            //{
            //    ChannelID = "2",
            //    InstallLocation = CameraInstallLocationEnum.InnerBehind
            //});
            //list.Add(new CameraInfo()
            //{
            //    ChannelID = "3",
            //    InstallLocation = CameraInstallLocationEnum.InnerCenter
            //});

            //return new MultiMessage<CameraInfo>(list, list.Count());
        }

        private void UpdateCameraInstallInfo(PTMSEntities context, string installAuditID, List<CameraInfo> cameraInfos)
        {
            foreach (var item in cameraInfos)
            {
                var entity = new MTN_PART_AUDIT()
                {
                    ID = Guid.NewGuid().ToString(),
                    INSTALL_AUDIT_ID = installAuditID,
                    CHANNEL_ID = short.Parse(item.ChannelID),
                    INSTALL_LOCATION = (short)item.InstallLocation,
                    PART_ID = item.SuitPartID,
                    RESULT = -1,
                    VIDEO_FILE = " ",
                    VIDEO_FILE_SIZE = -1
                };

                context.MTN_PART_AUDIT.Add(entity);
            }
        }

        public SingleMessage<string> SubmitForStep4(PTMSEntities context, string installDetailID, List<CameraInfo> cameraInfos)
        {
            SingleMessage<string> result = new SingleMessage<string>();
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadUncommitted;
            var scope = new TransactionScope(TransactionScopeOption.Required, optons);

            try
            {
                var installDetail = context.MTN_INSTALLATION_DETAIL.FirstOrDefault(t => t.ID == installDetailID);
                installDetail.CHECKSTEP = 4;

                //var audit = context.MTN_INSTALLATION_AUDIT.FirstOrDefault(t => t.INSTALL_ID == installDetailID);
                UpdateCameraInstallInfo(context, installDetailID, cameraInfos);

                if (context.SaveChanges() > 0)
                {
                    result.IsSuccess = true;
                    scope.Complete();
                }
                else
                {
                    result.IsSuccess = true;
                    result.Result = "SaveFailed";
                }

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

        public SingleMessage<string> SubmitGPSForStep1(PTMSEntities context, Step1Package step)
        {
            SingleMessage<string> result = new SingleMessage<string>();
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadUncommitted;
            var scope = new TransactionScope(TransactionScopeOption.Required, optons);

            try
            {
                BSC_ORDER_CLIENT client = context.BSC_ORDER_CLIENT.FirstOrDefault(n => n.ID == step.ClientID);
                if (client != null)
                {
                    int suitecount = context.RUN_SUITE_WORKING.Count(n => n.CLIENT_ID == step.ClientID);
                    int gpscount = context.RUN_GPS_WORKING.Count(n => n.CLIENT_ID == step.ClientID);

                    if (suitecount + gpscount < client.DEVICE_COUNT)
                    {
                        var vechile = (from x in context.BSC_VEHICLE
                                       where x.VEHICLE_ID == step.VehicleId && x.VALID == 1
                                       select x).FirstOrDefault();
                        if (vechile != null)
                        {
                            //update vehicle status
                            vechile.VEHICLE_STATUS = 1;
                            vechile.NOTE = step.Note;

                            //add install detail
                            MTN_GPS_INSTALLATION_DETAIL item = new MTN_GPS_INSTALLATION_DETAIL();
                            if (step.ID != null) item.ID = step.ID;
                            if (step.VehicleId != null) item.VEHICLE_ID = step.VehicleId;
                            if (step.InstallationStationId != null) item.STATION_ID = step.InstallationStationId;
                            if (step.InstallationStaff != null) item.INSTALL_STAFF = step.InstallationStaff;
                            if (step.RecordStaff != null) item.RECORD_STAFF = step.RecordStaff;
                            item.CHECKSTEP = short.Parse(step.Step.ToString());
                            if (step.CreateTime != null) item.CREATE_TIME = step.CreateTime;
                            item.VALID = 1;
                            context.MTN_GPS_INSTALLATION_DETAIL.Add(item);

                            if (context.SaveChanges() > 0)
                            {
                                result.IsSuccess = true;
                            }
                            else
                            {
                                result.IsSuccess = false;
                                result.Result = "SaveFailed";
                            }
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Result = "NoFoundVehicle";
                        }

                        scope.Complete();
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Result = "MaxDeviceCount";
                    }
                }
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

        public SingleMessage<SetAlarmPara> GetAlarmParaCommandResult(PTMSEntities context, string installationDetailID)
        {
            var time = ConvertHelper.DateTimeNow().AddMinutes(-1);
            var entity = context.MTN_VIDEO_UPLOAD_COMMAND.Where(t => t.INSTALLATION_DETAIL_ID == installationDetailID && t.CREATE_TIME > time)
                .OrderByDescending(t => t.CREATE_TIME).FirstOrDefault();
            if (entity == null)
            {
                return new SingleMessage<SetAlarmPara>(null);
            }

            var command = new SetAlarmPara()
            {
                MDVRID = entity.MDVR_CORE_SN,
                CommandID = entity.ID,
                SuccessFlag = (CommandStateEnum)entity.STATUS,
                InstallationDetailID = entity.INSTALLATION_DETAIL_ID,
            };

            return new SingleMessage<SetAlarmPara>(command);
        }
        public SingleMessage<string> SubmitGPSForStep2(PTMSEntities context, Step2Package step2)
        {
            SingleMessage<string> result = new SingleMessage<string>();
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadUncommitted;
            var scope = new TransactionScope(TransactionScopeOption.Required, optons);

            try
            {
                //add working suite
                RUN_GPS_WORKING checkitem = context.RUN_GPS_WORKING.FirstOrDefault(d => d.GPS_ID == step2.MDVR_CORE_SN);
                //update installdetail
                MTN_GPS_INSTALLATION_DETAIL installdetail = context.MTN_GPS_INSTALLATION_DETAIL.SingleOrDefault(d => d.ID == step2.InstallID);

                BSC_DEV_GPS gps = context.BSC_DEV_GPS.FirstOrDefault(n => n.ID == step2.SuiteKey);
                if (checkitem == null && installdetail != null && gps != null)
                {
                    RUN_GPS_WORKING item = new RUN_GPS_WORKING();
                    if (step2.SuiteId != null) item.GPS_ID = step2.SuiteKey;
                    if (step2.VehicleId != null) item.VEHICLE_ID = step2.VehicleId;
                    if (step2.MDVR_CORE_SN != null) item.GPS_SN = step2.MDVR_CORE_SN;
                    if (step2.SuiteStatus != null) item.STATUS = (short)DeviceSuiteStatus.Testing;
                    if (step2.SuiteSwitchTime != null) item.SWITCH_TIME = step2.SuiteSwitchTime;
                    if (step2.OnlineFlag != null) item.ONLINE_FLAG = (short?)step2.OnlineFlag;
                    if (step2.AbnormalCause != null) item.ABNORMAL_CAUSE = step2.AbnormalCause;
                    item.CLIENT_ID = step2.ClientID;
                    item.ORGANIZATION_ID = step2.Organization;
                    context.RUN_GPS_WORKING.Add(item);

                    if (!string.IsNullOrEmpty(step2.VehicleId)) installdetail.VEHICLE_ID = step2.VehicleId;
                    if (!string.IsNullOrEmpty(step2.SuiteKey)) installdetail.GPS_ID = step2.SuiteKey;
                    if (!string.IsNullOrEmpty(step2.MDVR_CORE_SN)) installdetail.GPS_SN = step2.MDVR_CORE_SN;
                    installdetail.CHECKSTEP = 2;

                    //update status
                    gps.STATUS = (short)DeviceSuiteStatus.Working;


                    if (context.SaveChanges() > 0)
                    {
                        result.IsSuccess = true;
                        scope.Complete();
                    }
                    else
                    {
                        result.IsSuccess = true;
                        result.Result = "Save Failed";
                    }
                }
                else if (checkitem == null)
                {
                    result.IsSuccess = false;
                    result.Result = "ID_INSTALL_SuiteAlreadyInstalled";
                }
                else if (installdetail == null)
                {
                    result.IsSuccess = false;
                    result.Result = "InstallDetailMissing";
                }
                else if (gps == null)
                {
                    result.IsSuccess = false;
                    result.Result = "GPSMissing";
                }

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

        public InstallationInfo GetGPSInstallationDetail(PTMSEntities context, string installationId)
        {
            var item = context.MTN_GPS_INSTALLATION_DETAIL.SingleOrDefault(d => d.ID == installationId);
            if (item != null)
            {
                InstallationInfo Installation = new InstallationInfo();
                Installation.Id = item.ID;
                Installation.VehicleID = item.VEHICLE_ID;
                //if (!string.IsNullOrEmpty(item.VEHICLE_ID))
                //    if (context.VEHICLE.SingleOrDefault(d => d.VEHICLE_ID == item.VEHICLE_ID) != null)
                //        Installation.VehicleType = context.VEHICLE.SingleOrDefault(d => d.VEHICLE_ID == item.VEHICLE_ID).VEHICLE_TYPE;
                Installation.DeviceCoreId = item.GPS_SN;
                Installation.DeviceKey = item.GPS_ID;
                if (!string.IsNullOrEmpty(item.GPS_ID))
                    if (context.BSC_DEV_GPS.SingleOrDefault(d => d.ID == item.GPS_ID) != null)
                        Installation.DeviceSN = context.BSC_DEV_GPS.SingleOrDefault(d => d.ID == item.GPS_ID).GPS_SN;
                Installation.InstallationStationId = item.STATION_ID;
                if (!string.IsNullOrEmpty(item.STATION_ID))
                    if (context.BSC_SETUP_STATION.SingleOrDefault(d => d.ID == item.STATION_ID) != null)
                        Installation.InstallationStationName = context.BSC_SETUP_STATION.SingleOrDefault(d => d.ID == item.STATION_ID).NAME;
                Installation.InstallationStaff = item.INSTALL_STAFF;
                Installation.RecordStaff = item.RECORD_STAFF;
                if (item.CREATE_TIME != null)
                    Installation.CreateTime = DateTime.Parse(item.CREATE_TIME.ToString());
                if (item.FINISH_TIME != null)
                    Installation.FinishTime = DateTime.Parse(item.FINISH_TIME.ToString());
                Installation.Note = item.NOTE;
                Installation.CheckStep = item.CHECKSTEP;

                var v = context.BSC_VEHICLE.FirstOrDefault(n => n.VEHICLE_ID == Installation.VehicleID && n.VALID == 1);
                if (v != null)
                {
                    Installation.Organization = v.ORGNIZATION_ID;
                }

                return Installation;
            }
            else
            {
                return null;
            }
        }



        public SingleMessage<string> SubmitGPSForStep3(PTMSEntities context, string installid)
        {
            var item = context.MTN_GPS_INSTALLATION_DETAIL.SingleOrDefault(d => d.ID == installid);
            if (item != null)
            {
                if (item.CHECKSTEP == 3)
                {
                    return new SingleMessage<string>(string.Empty);
                }
                item.CHECKSTEP = 3;

                context.Save();
            }

            return new SingleMessage<string>(string.Empty);
        }

        public SingleMessage<string> SubmitGPSForStep4(PTMSEntities context, string installid)
        {
            var item = context.MTN_GPS_INSTALLATION_DETAIL.SingleOrDefault(d => d.ID == installid);
            if (item != null)
            {
                item.CHECKSTEP = 4;
                item.FINISH_TIME = DateTime.Now.ToUniversalTime();

                RUN_GPS_WORKING gps = context.RUN_GPS_WORKING.Single(d => d.GPS_ID == item.GPS_ID);
                gps.STATUS = (short)DeviceSuiteStatus.Running;

                context.Save();
            }

            return new SingleMessage<string>(string.Empty);
        }

        public List<InstallationInfo> GetGPSInstallationFinished(PTMSEntities context, string carNumber, string gpsid, string installer, List<string> installStationId, DateTime? beginDate, DateTime? endDate, PagingInfo pageInfo, out int totalCount)
        {
            var result = (from x in context.MTN_GPS_INSTALLATION_DETAIL
                          join y in context.BSC_VEHICLE on x.VEHICLE_ID equals y.VEHICLE_ID
                          join vechicleType in context.BSC_VEHICLE_TYPE on y.VEHICLE_TYPE equals vechicleType.ID
                          join z in context.BSC_SETUP_STATION on x.STATION_ID equals z.ID
                          join e in context.BSC_DEV_GPS on x.GPS_ID equals e.ID
                          where (string.IsNullOrEmpty(carNumber) ? true : x.VEHICLE_ID.ToLower().Contains(carNumber.ToLower())) &&
                                (string.IsNullOrEmpty(gpsid) ? true : e.GPS_SN.Contains(gpsid)) &&
                                (string.IsNullOrEmpty(installer) ? true : x.INSTALL_STAFF.Contains(installer)) &&
                                installStationId.Contains(x.STATION_ID) &&
                                (beginDate == null ? true : x.FINISH_TIME >= beginDate) &&
                                (endDate == null ? true : x.FINISH_TIME < endDate) &&
                                (x.CHECKSTEP >= 4) && y.VALID == 1 && z.VALID == 1
                          select new InstallationInfo
                          {
                              Id = x.ID,
                              VehicleID = x.VEHICLE_ID,
                              VehicleTypeName = vechicleType.NAME,
                              DeviceCoreId = e.GPS_UID,
                              DeviceKey = e.ID,
                              DeviceSN = e.GPS_SN,
                              InstallationStationId = x.STATION_ID,
                              InstallationStationName = z.NAME,
                              InstallationStaff = x.INSTALL_STAFF,
                              RecordStaff = x.RECORD_STAFF,
                              CreateTime = x.CREATE_TIME,
                              FinishTime = x.FINISH_TIME,
                              Note = x.NOTE,
                              CheckStep = x.CHECKSTEP
                          });
            if (pageInfo.PageIndex == -1)
            {
                pageInfo.PageIndex = 0;
            }

            totalCount = result.Count();
            return result.OrderBy(x => x.Id).Skip(pageInfo.PageSize * pageInfo.PageIndex).Take(pageInfo.PageSize).ToList();
        }

        public MultiMessage<InstallStatisticsView> GetInstallStatisticsViewList(PTMSEntities context, string clientID, string organizationID, List<string> stations, string vehicleType, DateTime startTime, DateTime endTime)
        {
            if (string.IsNullOrEmpty(organizationID))
            {
                var list = from v in context.INSTALL_STATISTICS_VIEW
                           where v.CLIENTID == clientID && stations.Contains(v.STATION) && (string.IsNullOrEmpty(vehicleType) ? true : v.VEHICLETYPEID == vehicleType) && v.STARTTIME > startTime && v.STARTTIME < endTime
                           group v by new { STATIONNAME = v.STATIONNAME, OrganizationName = v.ORGANIZATIONNAME, VEHICLETYPE = v.VEHICLETYPE } into g
                           select new InstallStatisticsView()
                           {
                               OrganizationName = g.Key.OrganizationName,
                               StationName = g.Key.STATIONNAME,
                               VehicleType = g.Key.VEHICLETYPE,
                               Count = (short)g.Sum(n => n.SUMCOUNT)
                           };
                var items = list.ToList();
                return new MultiMessage<InstallStatisticsView>(items, items.Count);
            }
            else
            {
                List<USR_ORGANIZATION> orgs = context.USR_ORGANIZATION.Where(n => n.CLIENT_ID == clientID).ToList();

                List<string> ids = new List<string>();
                List<string> allids = new List<string>();
                allids.Add(organizationID);
                ids.Add(organizationID);
                while (ids.Count != 0)
                {
                    var temp = orgs.Where(n => ids.Contains(n.PARENT_ID)).ToList();
                    ids.Clear();
                    foreach (var item in temp)
                    {
                        ids.Add(item.ID);
                        allids.Add(item.ID);
                    }
                }

                var list = from v in context.INSTALL_STATISTICS_VIEW
                           where v.CLIENTID == clientID && allids.Contains(v.ORGNIZATION_ID) && stations.Contains(v.STATION) && (string.IsNullOrEmpty(vehicleType) ? true : v.VEHICLETYPEID == vehicleType) && v.STARTTIME > startTime && v.STARTTIME < endTime
                           group v by new { STATIONNAME = v.STATIONNAME, OrganizationName = v.ORGANIZATIONNAME, VEHICLETYPE = v.VEHICLETYPE } into g
                           select new InstallStatisticsView()
                           {
                               OrganizationName = g.Key.OrganizationName,
                               StationName = g.Key.STATIONNAME,
                               VehicleType = g.Key.VEHICLETYPE,
                               Count = (short)g.Sum(n => n.SUMCOUNT)
                           };
                var items = list.ToList();
                return new MultiMessage<InstallStatisticsView>(items, items.Count);
            }
        }

        public MultiMessage<DeviceAlertStatistics> GetDeviceAlertStatisticsViewList(PTMSEntities context, string clientID, string organizationID, string vehicleID, DateTime startTime, DateTime endTime, List<string> stations, int pageSize, int pageIndex)
        {
            if (string.IsNullOrEmpty(organizationID))
            {
                var list = from da in context.ALT_DEVICE_ALERT
                           join v in context.BSC_VEHICLE on da.VEHICLE_ID equals v.VEHICLE_ID
                           join detail in context.MTN_INSTALLATION_DETAIL on da.VEHICLE_ID equals detail.VEHICLE_ID
                           where v.CLIENT_ID == clientID && (string.IsNullOrEmpty(vehicleID) ? true : v.VEHICLE_ID.ToUpper().Contains(vehicleID.ToUpper())) && da.ALERT_TIME > startTime && da.ALERT_TIME < endTime && stations.Contains(detail.STATION_ID)
                           orderby da.VEHICLE_ID, da.ALERT_TYPE
                           group da by new { VEHICLE_ID = v.VEHICLE_ID, ALERT_TYPE = da.ALERT_TYPE } into g
                           join v1 in context.BSC_VEHICLE on g.Key.VEHICLE_ID equals v1.VEHICLE_ID
                           join o in context.USR_ORGANIZATION on v1.ORGNIZATION_ID equals o.ID
                           select new DeviceAlertStatistics()
                           {
                               VehicleID = g.Key.VEHICLE_ID,
                               OrganizatioName = o.NAME,
                               AlertType = (short)g.Key.ALERT_TYPE,
                               Count = (int)g.Count()
                           };
                int count = 0;
                var items = list.Page(out count, pageIndex, pageSize, true, n => n.VehicleID, true).ToList();
                return new MultiMessage<DeviceAlertStatistics>(items, count);
            }
            else
            {
                List<USR_ORGANIZATION> orgs = context.USR_ORGANIZATION.Where(n => n.CLIENT_ID == clientID).ToList();

                List<string> ids = new List<string>();
                List<string> allids = new List<string>();
                allids.Add(organizationID);
                ids.Add(organizationID);
                while (ids.Count != 0)
                {
                    var temp = orgs.Where(n => ids.Contains(n.PARENT_ID)).ToList();
                    ids.Clear();
                    foreach (var item in temp)
                    {
                        ids.Add(item.ID);
                        allids.Add(item.ID);
                    }
                }

                var list = from da in context.ALT_DEVICE_ALERT
                           join v in context.BSC_VEHICLE on da.VEHICLE_ID equals v.VEHICLE_ID
                           join detail in context.MTN_INSTALLATION_DETAIL on da.VEHICLE_ID equals detail.VEHICLE_ID
                           where v.CLIENT_ID == clientID && allids.Contains(v.ORGNIZATION_ID) && (string.IsNullOrEmpty(vehicleID) ? true : v.VEHICLE_ID.ToUpper().Contains(vehicleID.ToUpper())) && da.ALERT_TIME > startTime && da.ALERT_TIME < endTime && stations.Contains(detail.STATION_ID)
                           orderby da.VEHICLE_ID, da.ALERT_TYPE
                           group da by new { VEHICLE_ID = v.VEHICLE_ID, ALERT_TYPE = da.ALERT_TYPE } into g
                           join v1 in context.BSC_VEHICLE on g.Key.VEHICLE_ID equals v1.VEHICLE_ID
                           join o in context.USR_ORGANIZATION on v1.ORGNIZATION_ID equals o.ID
                           select new DeviceAlertStatistics()
                           {
                               VehicleID = g.Key.VEHICLE_ID,
                               OrganizatioName = o.NAME,
                               AlertType = (short)g.Key.ALERT_TYPE,
                               Count = (int)g.Count()
                           };
                int count = 0;
                var items = list.Page(out count, pageIndex, pageSize, true, n => n.VehicleID, true).ToList();
                return new MultiMessage<DeviceAlertStatistics>(items, count);
            }
        }
    }
}
