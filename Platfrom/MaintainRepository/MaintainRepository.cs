/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 95857ff1-78f1-4a5d-8ca4-93f426c364f8      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Repository
/////    Project Description:    
/////             Class Name: MaintainRepository
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/28 16:17:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/28 16:17:14
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using System.Transactions;
using Gsafety.Common.Util;

namespace Gsafety.PTMS.Maintain.Repository
{
    public class MaintainRepository
    {
        /// <summary>
        /// 添加安全套件升级记录
        /// </summary>
        /// <param name="item"></param>
        /// <param name="status">  ////1：等待升级；2：开始升级；3：升级中；4：升级完成</param>
        public void AddSuiteUpgradeRecord(PTMSEntities context, UpgradeCMD item, short status)
        {
            //SUITE_UPGRADE_RECORD entity;

            //var result = (from s in context.SECURITY_SUITE_WORKING
            //              where s.MDVR_CORE_SN == item.DvId
            //              select s).FirstOrDefault();

            //if (result != null)
            //{
            //    entity = new SUITE_UPGRADE_RECORD();
            //    entity.ID = Guid.NewGuid().ToString();
            //    entity.MDVR_CORE_SN = item.DvId;
            //    entity.OPERATOR = item.Operator;
            //    entity.OPER_TIME = DateTime.Now;
            //    entity.CONTENT = item.Context;
            //    entity.CURR_VERSION = item.Version;
            //    entity.SUITE_INFO_ID = result.SUITE_INFO_ID;
            //    entity.STATUS = status;

            //    context.SUITE_UPGRADE_RECORD.Add(entity);

            //    context.SaveChanges();
            //}

        }

        /// <summary>
        /// 软件升级命令回复（拒绝升级时，修改升级结果和状态）
        /// </summary>
        /// <param name="item"></param>
        public void UpdateSuiteUpgradeReply(PTMSEntities context, UpgradeReply item)
        {

            var upgradeResult = (from s in context.TRF_UPGRADE_RECORD
                                 where s.MDVR_CORE_SN == item.MdvrCoreId
                                 && s.STATUS == 3
                                 select s).OrderByDescending(x => x.OPER_TIME).FirstOrDefault();            ////升级中

            if (upgradeResult != null)
            {
                if (item.ReplyResult == 0)
                {
                    upgradeResult.UPDATE_RESULT = "failed";      ////升级失败
                    upgradeResult.ERROR_NUMBER = item.ErrorCode;   ////TODO:失败原因
                    upgradeResult.STATUS = 4;////升级完成 
                }
                else if (item.ReplyResult == 1)
                {
                    upgradeResult.STATUS = 3;////升级中
                }
                upgradeResult.LONGITUDE = item.Longitude;
                upgradeResult.LATITUDE = item.Latitude;
                upgradeResult.SPEED = item.Speed;
                upgradeResult.DIRECTION = item.Direction;
                upgradeResult.GPS_TIME = item.GpsTime;
                upgradeResult.GPS_VALID = item.GpsValid;

                context.SaveChanges();
            }

        }

        /// <summary>
        /// 软件升级命令状态回复（拒绝升级时，修改升级结果和状态）
        /// </summary>
        /// <param name="item"></param>
        public void UpdateSuiteUpgradeStatusReply(PTMSEntities context, UpgradeStatusReply item)
        {
            var upgradeResult = (from s in context.TRF_UPGRADE_RECORD
                                 where s.MDVR_CORE_SN == item.MdvrCoreId
                                 && s.STATUS == 3
                                 select s).OrderByDescending(x => x.OPER_TIME).FirstOrDefault();            ////升级中

            if (upgradeResult != null)
            {
                upgradeResult.LONGITUDE = item.Longitude;
                upgradeResult.LATITUDE = item.Latitude;
                upgradeResult.SPEED = item.Speed;
                upgradeResult.DIRECTION = item.Direction;
                upgradeResult.GPS_TIME = item.GpsTime;
                upgradeResult.GPS_VALID = item.GpsValid;

                if (item.ReplyResult == 1)
                {
                    upgradeResult.UPDATE_RESULT = "failed";////升级失败
                    upgradeResult.ERROR_NUMBER = item.ErrorCode;
                    upgradeResult.STATUS = 3;////升级完成 
                }
                else if (item.ReplyResult == 0)
                {
                    ////不处理，等待升级结果的xml
                    //upgradeResult.UPDATE_RESULT = "failed";////升级成功
                    //upgradeResult.STATUS = 3;////升级完成 
                }
                else if (item.ReplyResult == 2)
                {
                    upgradeResult.STATUS = 3;////升级中
                }

                context.SaveChanges();
            }

        }

        /// <summary>
        /// 软件升级结果记录
        /// </summary>
        /// <param name="item"></param>
        public void UpdateSuiteUpgradeResult(PTMSEntities context, UpgradeResult item)
        {
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadUncommitted;

            var scope = new TransactionScope(TransactionScopeOption.Required, optons);


            try
            {
                var upgradeResult = (from s in context.TRF_UPGRADE_RECORD
                                     where s.MDVR_CORE_SN == item.MdvrCoreId
                                     && s.STATUS == 3
                                     select s).OrderByDescending(x => x.OPER_TIME).FirstOrDefault();            ////升级中

                if (upgradeResult != null)
                {
                    upgradeResult.LONGITUDE = item.Longitude;
                    upgradeResult.LATITUDE = item.Latitude;
                    upgradeResult.SPEED = item.Speed;
                    upgradeResult.DIRECTION = item.Direction;
                    upgradeResult.GPS_TIME = item.GpsTime;
                    upgradeResult.GPS_VALID = item.GpsValid;

                    upgradeResult.UPDATE_RESULT = item.UpdateResult;
                    upgradeResult.CURR_VERSION = item.CurFiremareVer;
                    upgradeResult.LAST_VERSION = item.LasTFiremareVer;
                    upgradeResult.LAST_UPDATE_TIME = item.LastFiremareUpdateTime;
                    upgradeResult.UPDATE_START_TIME = item.UpdateRebootTime;
                    upgradeResult.UPDATE_END_TIME = item.UpdateEndTime;
                    upgradeResult.ERROR_NUMBER = item.ErrorNumber;

                    upgradeResult.STATUS = 4;////升级完成
                    upgradeResult.CONTENT = item.Context;

                    var workingResult = (from s in context.BSC_DEV_SUITE
                                         where s.MDVR_CORE_SN == item.MdvrCoreId
                                         && s.STATUS == (int)DeviceSuiteStatus.Working
                                         select s).FirstOrDefault();

                    if (workingResult != null)
                    {
                        workingResult.SOFTWARE_VERSION = item.CurFiremareVer;
                    }
                }
                context.SaveChanges();

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

        }

        /// <summary>
        /// 添加运维硬件信息
        /// </summary>
        public void AddSuiteRuningStatus(PTMSEntities context, SuiteRunintStatusCMD model)
        {
            //SUITE_RUNNING_DETAIL entity = new SUITE_RUNNING_DETAIL()
            //{
            //    ID = model.SuiteRunintStatusID,
            //    MDVR_CORE_SN = model.DvId,
            //    CONTENT = model.Context,
            //    VEHICLE_ID = model.VehicleId,
            //    CREATE_TIME = DateTime.Now
            //};

            //context.SUITE_RUNNING_DETAIL.Add(entity);

            //context.SaveChanges();

        }

        /// <summary>
        /// 修改运维基础信息
        /// </summary>
        public void UpdateSuiteRuningStatus(PTMSEntities context, BasicInfo model)
        {
            //if (!string.IsNullOrEmpty(model.Id))
            //{
            //    var result = (from s in context.SUITE_RUNNING_DETAIL
            //                  where s.ID == model.Id
            //                  select s).FirstOrDefault();

            //    if (result != null)
            //    {
            //        ////修改
            //        result.VEHICLE_ID = model.VehicleId;
            //        result.CONTENT = result.CONTENT + model.Context;
            //    }

            //    context.SaveChanges();

            //}
        }

        /// <summary>
        /// 修改运维环境信息
        /// </summary>
        public void UpdateSuiteRuningStatus(PTMSEntities context, Enviroment model)
        {
            if (!string.IsNullOrEmpty(model.Id))
            {
                //var result = (from s in context.SUITE_RUNNING_DETAIL
                //              where s.ID == model.Id
                //              select s).FirstOrDefault();

                //if (result != null)
                //{
                //    ////修改
                //    result.VOLTAGE = model.Voltage;
                //    result.VOLTAGE_FLAG = model.VoltageFlag;
                //    result.TEMPERATURE_IN = model.TemperatureIn;
                //    result.TEMPERATURE_IN_FLAG = model.TemperatureInFlag;
                //    result.TEMPERATURE_OUT = model.TemperatureOut;
                //    result.TEMPERATURE_OUT_FLAG = model.TemperatureOutFlag;
                //    result.ACC_STATUS = model.AccStatus;
                //    result.BATTERY_STATUS = model.BatteryStatus;
                //    result.CONTENT = result.CONTENT + model.Context;
                //}

                //context.SaveChanges();

            }
        }

        /// <summary>
        /// 修改运维硬件信息
        /// </summary>
        public void UpdateSuiteRuningStatus(PTMSEntities context, Hardware model)
        {
            if (!string.IsNullOrEmpty(model.Id))
            {
                //var result = (from s in context.SUITE_RUNNING_DETAIL
                //              where s.ID == model.Id
                //              select s).FirstOrDefault();

                //if (result != null)
                //{
                //    ////修改
                //    result.RECSD_STATUS = model.RecsdStatus;
                //    result.RECSD_WR_ERROR = model.RecsdWrError;
                //    result.RECSD_FULL = model.RecsdFull;
                //    result.GPS_VALID = model.GpsValid;
                //    result.GPS_ANTENNA = model.GpsAntenna;
                //    result.GPS_POORCNT = model.GpsPoorcnt;
                //    result.CAMERA1_STATUS = model.Camera1Status;
                //    result.CAMERA2_STATUS = model.Camera2Status;
                //    result.CAMERA1_RECSTAT = model.Camera1Recstat;
                //    result.CAMERA2_RECSTAT = model.Camera2Recstat;
                //    result.RECORD_STATUS = model.RecordStatus;
                //    result.CONTENT = result.CONTENT + model.Context;
                //}
                //context.SaveChanges();

            }
        }

        /// <summary>
        /// 根据软件版本号检查套件是否需要升级
        /// </summary>
        /// <param name="softwareVersion"></param>
        public void ChechSoftwareVersion(PTMSEntities context)
        {

            string sql = @" BEGIN
                                DELETE FROM SUITE_UPGRADE_RECORD WHERE STATUS=1 OR STATUS=2;
                                INSERT INTO SUITE_UPGRADE_RECORD(ID,SUITE_INFO_ID,MDVR_CORE_SN,OPERATOR,OPER_TIME,CURR_VERSION,STATUS)
                                SELECT SYS_GUID()||'0000' AS ID ,SUITE_INFO_ID,MDVR_CORE_SN,'SOFTWAREUPGRADESERVICE' AS OPERATOR,SYSDATE AS OPER_TIME, VENDOR_VERSION AS CURR_VERSION,1 AS STATUS FROM
                                (
                                SELECT SUITE_ID,SUITE_INFO_ID,MDVR_CORE_SN,SOFTWARE_VERSION,T1.VENDOR_VERSION  FROM SECURITY_SUITE_INFO LEFT JOIN 
                                (
                                SELECT VENDOR_VERSION,CASE  WHEN VENDOR='WKP' THEN '0203' WHEN VENDOR='RM' THEN '0202' WHEN VENDOR='SC' THEN '0204' END AS FACTORYCODE  FROM SUITE_SOFTWARE_VERSION
                                WHERE UNIFIED_VERSION IN (SELECT MAX(UNIFIED_VERSION) FROM SUITE_SOFTWARE_VERSION)
                                ) T1 ON SUBSTR(SUITE_ID,1,4)=T1.FACTORYCODE
                                ) T_VERSION 
                                WHERE SOFTWARE_VERSION<>VENDOR_VERSION 
                                AND SUITE_ID||VENDOR_VERSION NOT IN (SELECT SUITE_INFO_ID||CURR_VERSION FROM SUITE_UPGRADE_RECORD WHERE CURR_VERSION IN (SELECT VENDOR_VERSION FROM SUITE_SOFTWARE_VERSION
                                WHERE UNIFIED_VERSION IN (SELECT MAX(UNIFIED_VERSION) FROM SUITE_SOFTWARE_VERSION)  ) );
                                COMMIT;
                                END;";
            context.Database.ExecuteSqlCommand(sql);

        }

        /// <summary>
        /// 获取待升级的安全套件信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        //public List<SUITE_UPGRADE_RECORD> GetWaitingUpgradeSuiteInfo(PTMSEntities context)
        //{
        //    var softwareVersionInfo = (from x in context.SUITE_SOFTWARE_VERSION
        //                               where x.UNIFIED_VERSION == context.SUITE_SOFTWARE_VERSION.Max(y => y.UNIFIED_VERSION)
        //                               select x).ToList();

        //    if (softwareVersionInfo != null)
        //    {
        //        string wkpVersion = string.Empty;
        //        string scVersion = string.Empty;
        //        string rmVersion = string.Empty;

        //        var wkpVersionInfo = softwareVersionInfo.Where(x => x.VENDOR == "WKP").FirstOrDefault();
        //        if (wkpVersionInfo != null)
        //        {
        //            wkpVersion = wkpVersionInfo.VENDOR_VERSION;
        //        }

        //        var rmVersionInfo = softwareVersionInfo.Where(x => x.VENDOR == "RM").FirstOrDefault();
        //        if (rmVersionInfo != null)
        //        {
        //            rmVersion = rmVersionInfo.VENDOR_VERSION;
        //        }

        //        var scVersionInfo = softwareVersionInfo.Where(x => x.VENDOR == "SC").FirstOrDefault();
        //        if (scVersionInfo != null)
        //        {
        //            scVersion = scVersionInfo.VENDOR_VERSION;
        //        }

        //        var result = (from x in context.DEV_SUITE
        //                      where x.STATUS == (int)DeviceSuiteStatus.Working
        //                      && ((string.IsNullOrEmpty(wkpVersion) ? true : (x.SUITE_ID.Substring(0, 4) == "0203" && x.SOFTWARE_VERSION != wkpVersion))
        //                      || (string.IsNullOrEmpty(rmVersion) ? true : (x.SUITE_ID.Substring(0, 4) == "0202" && x.SOFTWARE_VERSION != rmVersion))
        //                      || (string.IsNullOrEmpty(scVersion) ? true : (x.SUITE_ID.Substring(0, 4) == "0204" && x.SOFTWARE_VERSION != scVersion)))
        //                      select new
        //                      {
        //                          MDVR_CORE_SN = x.MDVR_CORE_SN,
        //                          SUITE_ID = x.SUITE_ID
        //                      }).ToList()
        //                        .Select(x => new SUITE_UPGRADE_RECORD
        //                        {

        //                            ID = Guid.NewGuid().ToString(),
        //                            MDVR_CORE_SN = x.MDVR_CORE_SN,
        //                            OPERATOR = "SoftwareUpgradeService",
        //                            OPER_TIME = DateTime.Now,
        //                            CURR_VERSION = GetVendorVersionBySuiteId(x.SUITE_ID, softwareVersionInfo),
        //                            STATUS = 1 ////等待升级
        //                        });

        //        return result.ToList();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// 根据安全套件号，判断厂家设备该升级的版本号
        /// </summary>
        /// <param name="suiteId"></param>
        /// <param name="softwareVersionInfo"></param>
        /// <returns></returns>
        //public string GetVendorVersionBySuiteId(string suiteId, List<SUITE_SOFTWARE_VERSION> softwareVersionInfo)
        //{
        //    if (!string.IsNullOrEmpty(suiteId))
        //    {
        //        if (suiteId.Substring(0, 4) == "0203")
        //        {
        //            return softwareVersionInfo.Where(s => s.VENDOR == "WKP").FirstOrDefault().VENDOR_VERSION;
        //        }
        //        else if (suiteId.Substring(0, 4) == "0202")
        //        {
        //            return softwareVersionInfo.Where(s => s.VENDOR == "RM").FirstOrDefault().VENDOR_VERSION;
        //        }
        //        else if (suiteId.Substring(0, 4) == "0204")
        //        {
        //            return softwareVersionInfo.Where(s => s.VENDOR == "SC").FirstOrDefault().VENDOR_VERSION;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// 批量添加安全套件升级记录(开始升级)
        /// </summary>
        /// <param name="item"></param>
        /// <param name="status">  ////1：等待升级；2：开始升级；3：升级中；4：升级完成</param>
        //public void BatchAddSuiteUpgradeRecord(List<SUITE_UPGRADE_RECORD> SuiteUpgradeRecordInfo, PTMSEntities context)
        //{
        //    foreach (var item in SuiteUpgradeRecordInfo)
        //    {
        //        context.SUITE_UPGRADE_RECORD.Add(item);
        //    }
        //    context.SaveChanges();
        //}

        /// <summary>
        /// 批量添加安全套件升级记录(开始升级)
        /// </summary>
        /// <param name="item"></param>
        /// <param name="status">  ////1：等待升级；2：开始升级；3：升级中；4：升级完成</param>
        //public void BatchDeleteSuiteUpgradeRecord(List<SUITE_UPGRADE_RECORD> SuiteUpgradeRecordInfo, PTMSEntities context)
        //{
        //    foreach (var item in SuiteUpgradeRecordInfo)
        //    {
        //        ////如果有等待升级，开始升级的记录，则删除，重新添加升级记录
        //        var result = from x in context.SUITE_UPGRADE_RECORD
        //                     where (x.MDVR_CORE_SN == item.MDVR_CORE_SN)
        //                     && (x.STATUS == 1 || x.STATUS == 2)
        //                     select x;

        //        if (result != null)
        //        {
        //            foreach (var x in result)
        //            {
        //                context.SUITE_UPGRADE_RECORD.Remove(x);
        //            }
        //        }
        //    }
        //    context.SaveChanges();
        //}

        /// <summary>
        /// 获取需要升级的记录
        /// </summary>
        /// <returns></returns>
        public List<UpgradeCMD> GetSuiteUpgradeRecord(PTMSEntities context, int count)
        {
            //var result = (from x in context.SUITE_UPGRADE_RECORD
            //              join y in context.SUITE_SOFTWARE_VERSION on x.CURR_VERSION equals y.VENDOR_VERSION
            //              where x.STATUS == 1
            //              select new
            //             {
            //                 SuiteUpgradeRecordId = x.ID,
            //                 DvId = x.MDVR_CORE_SN,
            //                 FileName = y.FILE_NAME,
            //                 FileSize = y.FILE_SIZE
            //             }).ToList()
            //             .Select(x => new UpgradeCMD
            //             {
            //                 SuiteUpgradeRecordId = x.SuiteUpgradeRecordId,
            //                 DvId = x.DvId,
            //                 FileName = x.FileName,
            //                 DataPacketCount = (int)x.FileSize,
            //                 FTPAddress = ConfigurationHelper.FTPWlanServerName,
            //                 MD5Code = "2222",
            //                 MsgId = "1",
            //                 Password = ConfigurationHelper.FTPPassword,
            //                 SendTime = DateTime.Now,
            //                 Port = ConfigurationHelper.FTPPort.ToString(),
            //                 UserName = ConfigurationHelper.FTPUserName,
            //                 UUId = Guid.NewGuid().ToString()
            //             });

            ////事务
            var scope = new TransactionScope(TransactionScopeOption.Required);


            try
            {
                //var returmInfo = result.OrderBy(x => x.DvId).Skip(1).Take(count).ToList();
                //var listId = returmInfo.Select(x => x.SuiteUpgradeRecordId);
                //BatchUpdateUpgradeStatusToStartUpgrade(listId, context);
                //context.SaveChanges();
                //scope.Complete();
                //return returmInfo;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                scope.Dispose();
            }
            return null;
        }

        /// <summary>
        /// 批量更新套件升级记录状态为开始升级
        /// </summary>
        public void BatchUpdateUpgradeStatusToStartUpgrade(IEnumerable<string> listId, PTMSEntities context)
        {
            foreach (var id in listId)
            {
                //var result = (from x in context.SUITE_UPGRADE_RECORD
                //              where x.ID == id
                //              select x).FirstOrDefault();
                //if (result != null)
                //{
                //    result.STATUS = 2;
                //}
            }
        }

        /// <summary>
        /// 批量更新套件升级记录状态为升级中
        /// </summary>
        public void UpdateUpgradeStatusToInProgress(PTMSEntities context, string suiteUpgradeRecordId)
        {
            //var result = (from x in context.SUITE_UPGRADE_RECORD
            //              where x.ID == suiteUpgradeRecordId
            //              select x).FirstOrDefault();
            //if (result != null)
            //{
            //    result.STATUS = 3;
            //    result.OPER_TIME = DateTime.Now;
            //}
            context.SaveChanges();

        }
    }
}
