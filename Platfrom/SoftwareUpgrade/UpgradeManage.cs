/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 8cde1c2c-ba64-4c0e-b656-eb8661266189      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SoftwareUpgrade
/////    Project Description:    
/////             Class Name: UpgradeManage
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/6 17:27:25
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/6 17:27:25
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Analysis.Maintain;
using Gsafety.PTMS.Maintain.Repository;
using Gsafety.Common.Logging;
using Gsafety.MQ;
using Gsafety.Common.Util;
using Gsafety.PTMS.Message.Contract.Data;
using System.Xml.Linq;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.SoftwareUpgrade
{
    public class UpgradeManage
    {
        private static MaintainRepository _maintainRepository;

        static UpgradeManage()
        {
            _maintainRepository = new MaintainRepository();
        }

        public static void ProcessUpgrade(byte[] bytes, string key)
        {
            try
            {
                var model = ConvertHelper.BytesToObject(bytes) as UpgradeCMD;
                LoggerManager.Logger.Info(string.Format("get UpgradeCmd model:{0}", ConvertHelper.ConvertModelToJson(model)));


                using (PTMSEntities context = new PTMSEntities())
                {
                    _maintainRepository.AddSuiteUpgradeRecord(context, model, 3);
                }

                string strCmd = model.ToString();
                LoggerManager.Logger.Info(string.Format("get UpgradeCmd data, string:{0}", model.ToString()));
                var msg = System.Text.UTF8Encoding.UTF8.GetBytes(strCmd);
                if (msg != null && msg.Length > 0)
                {
                    ProcessMessage.PublishMessage(Constdefine.MDVREXCHANGE, key.Replace(MonitorRoute.OriginalUpgradeCMDKey, MonitorRoute.HandleUpgradeKey), msg);
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        public static void ProcessUpgradeReply(byte[] bytes)
        {
            try
            {
                string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                LoggerManager.Logger.Info(string.Format("get UpgradeReply data, string:{0}", str));
                UpgradeReply model = new UpgradeReply(str);
                if (model != null)
                {

                    using (PTMSEntities context = new PTMSEntities())
                    {
                        _maintainRepository.UpdateSuiteUpgradeReply(context, model);
                    }

                }
                else
                {
                    LoggerManager.Logger.Warn(string.Format("Converted UpgradeReply to entity is empty,string:{0}", str));
                }
                UpgradingCache.Remove(model.MdvrCoreId);
                BatchSendUpgradeCMD();
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        public static void UpgradeStatusCMD(byte[] bytes, string key)
        {
            try
            {
                var model = ConvertHelper.BytesToObject(bytes) as UpgradeCMD;
                LoggerManager.Logger.Info(string.Format("get UpgradeCmd model:{0}", ConvertHelper.ConvertModelToJson(model)));


                using (PTMSEntities context = new PTMSEntities())
                {
                    _maintainRepository.AddSuiteUpgradeRecord(context, model, 3);
                }

                string strCmd = model.ToString();
                LoggerManager.Logger.Info(string.Format("get UpgradeCmd data, string:{0}", model.ToString()));
                var msg = System.Text.UTF8Encoding.UTF8.GetBytes(strCmd);
                if (msg != null && msg.Length > 0)
                {
                    ProcessMessage.PublishMessage(Constdefine.MDVREXCHANGE, key.Replace(MonitorRoute.OriginalUpgradeStatusKey, MonitorRoute.HandleUpgradeStatusKey), msg);
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        public static void UpgradeStatusReply(byte[] bytes)
        {
            try
            {
                string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                LoggerManager.Logger.Info(string.Format("get UpgradeReply data, string:{0}", str));
                UpgradeStatusReply model = new UpgradeStatusReply(str);
                if (model != null)
                {

                    using (PTMSEntities context = new PTMSEntities())
                    {
                        _maintainRepository.UpdateSuiteUpgradeStatusReply(context, model);
                    }

                }
                else
                {
                    LoggerManager.Logger.Warn(string.Format("Converted UpgradeStatusReply to entity is empty,string:{0}", str));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        public static void StartBatchSoftwareUpgrade(byte[] bytes)
        {
            try
            {
                var model = ConvertHelper.BytesToObject(bytes) as UpgradeNotify;
                LoggerManager.Logger.Info(string.Format("get UpgradeNotify data, NotifyTime:{0}", model.NotifyTime));

                using (PTMSEntities context = new PTMSEntities())
                {
                    _maintainRepository.ChechSoftwareVersion(context);
                }

                BatchSendUpgradeCMD();
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        public static void BatchSendUpgradeCMD()
        {
            int count = ConfigHelper.MaxUpgradeCount - UpgradingCache.Count();
            if (count > 0)
            {
                var upgradeInfo = new List<UpgradeCMD>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    upgradeInfo = _maintainRepository.GetSuiteUpgradeRecord(context, count);
                }

                if (upgradeInfo.Count > 0)
                {

                    byte[] msg;
                    foreach (var item in upgradeInfo)
                    {
                        msg = System.Text.UTF8Encoding.UTF8.GetBytes(item.ToString());
                        ProcessMessage.PublishMessage(Constdefine.MDVREXCHANGE, MonitorRoute.HandleUpgradeKey + item.DvId, msg);
                        UpgradingCache.Add(item);

                        using (PTMSEntities context = new PTMSEntities())
                        {
                            _maintainRepository.UpdateUpgradeStatusToInProgress(context, item.SuiteUpgradeRecordId);
                        }

                    }
                }
                else
                {
                    LoggerManager.Logger.Info("No need to upgrade equipment!");
                }
            }
        }
    }
}
