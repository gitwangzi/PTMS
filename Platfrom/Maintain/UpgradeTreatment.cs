/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5b2a1f97-e4d5-4546-9a7b-1a5e5087e73e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.MonitorTreatment
/////    Project Description:    
/////             Class Name: UpgradeTreatment
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/28 15:09:00
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/28 15:09:00
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.Common.Util;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.Maintain.Repository;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.Analysis.Maintain
{
    public static class UpgradeTreatment
    {
        private static MaintainRepository _maintainRepository;

        static UpgradeTreatment()
        {
            _maintainRepository = new MaintainRepository();
        }

        public static byte[] UpgradeCmd(byte[] bytes)
        {
            byte[] msg = null;

            var model = ConvertHelper.BytesToObject(bytes) as UpgradeCMD;
            LoggerManager.Logger.Info(string.Format("get UpgradeCmd model:{0}", ConvertHelper.ConvertModelToJson(model)));


            using (PTMSEntities context = new PTMSEntities())
            {
                _maintainRepository.AddSuiteUpgradeRecord(context,model, 3);
            }
	        	
            string strCmd = model.ToString();
            LoggerManager.Logger.Info(string.Format("get UpgradeCmd data, string:{0}", model.ToString()));
            msg = System.Text.UTF8Encoding.UTF8.GetBytes(strCmd);
            return msg;
        }

        public static byte[] UpgradeStatusCMD(byte[] bytes)
        {
            byte[] msg = null;

            var model = ConvertHelper.BytesToObject(bytes) as UpgradeStatusCMD;
            LoggerManager.Logger.Info(string.Format("get UpgradeCmd model:{0}", ConvertHelper.ConvertModelToJson(model)));

            string strCmd = model.ToString();
            LoggerManager.Logger.Info(string.Format("get UpgradeCmd data, string:{0}", model.ToString()));
            msg = System.Text.UTF8Encoding.UTF8.GetBytes(strCmd);
            return msg;
        }

        public static void UpgradeReply(byte[] bytes)
        {
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info(string.Format("get UpgradeReply data, string:{0}", str));
            UpgradeReply model = new UpgradeReply(str);
            if (model != null)
            {

                using (PTMSEntities context = new PTMSEntities())
                {
                    _maintainRepository.UpdateSuiteUpgradeReply(context,model);
                }
	        	
            }
            else
            {
                LoggerManager.Logger.Warn(string.Format("Converted UpgradeReply to entity is empty,string:{0}", str));
            }
        }

        public static void UpgradeStatusReply(byte[] bytes)
        {
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info(string.Format("get UpgradeReply data, string:{0}", str));
            UpgradeStatusReply model = new UpgradeStatusReply(str);
            if (model != null)
            {

                using (PTMSEntities context = new PTMSEntities())
                {
                    _maintainRepository.UpdateSuiteUpgradeStatusReply(context,model);
                }
	        	
            }
            else
            {
                LoggerManager.Logger.Warn(string.Format("Converted UpgradeStatusReply to entity is empty,string:{0}", str));
            }
        }
    }
}
