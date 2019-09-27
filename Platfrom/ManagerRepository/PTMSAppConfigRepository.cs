using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Manager.Contract.Data;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7da6ac54-85e8-41f4-bda2-791046a85226      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Repository
/////    Project Description:    
/////             Class Name: PTMSAppConfigRepository
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/5 10:42:17
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/5 10:42:17
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
namespace Gsafety.PTMS.Manager.Repository
{
    /// <summary>
    /// Config Value
    /// </summary>
    public class PTMSAppConfigRepository
    {
        /// <summary>
        /// Get Config 
        /// </summary>
        /// <returns></returns>
        public List<AppConfig> GetappConfigInfolist(PTMSEntities context, string desc, string name)
        {
            var result = from i in context.CFG_APP_CONFIG
                         where i.SECTION_TYPE == name && (string.IsNullOrEmpty(desc) ? true : i.SECTION_DESC.Contains(desc))
                         select new AppConfig
                         {
                             ID = i.ID,
                             SECTION_DESC = i.SECTION_DESC,
                             SECTION_NAME = i.SECTION_NAME,
                             SECTION_TYPE = i.SECTION_TYPE,
                             SECTION_VALUE = i.SECTION_VALUE,
                             SECTION_UNIT = i.SECTION_UNIT
                         };
            return result.ToList();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public AppConfig GetappConfigInfo(PTMSEntities context, string Name)
        {

            var result = from i in context.CFG_APP_CONFIG
                         where i.SECTION_TYPE == "BaseInfo" && i.SECTION_NAME == Name
                         select new AppConfig
                         {
                             ID = i.ID,
                             SECTION_DESC = i.SECTION_DESC,
                             //SECTION_LEVEL = i.SECTION_LEVEL,
                             SECTION_NAME = i.SECTION_NAME,
                             SECTION_TYPE = i.SECTION_TYPE,
                             SECTION_VALUE = i.SECTION_VALUE,
                         };
            return result.SingleOrDefault();

        }
        /// <summary>
        /// get email config
        /// </summary>
        /// <returns></returns>
        public List<AppConfig> GetappEmailInfo(PTMSEntities context)
        {
            var result = from i in context.CFG_APP_CONFIG
                         where i.SECTION_TYPE == "Email"
                         select new AppConfig
                         {
                             ID = i.ID,
                             SECTION_DESC = i.SECTION_DESC,
                             //SECTION_LEVEL = i.SECTION_LEVEL,
                             SECTION_NAME = i.SECTION_NAME,
                             SECTION_TYPE = i.SECTION_TYPE,
                             SECTION_VALUE = i.SECTION_VALUE,

                         };
            return result.ToList();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public AppConfig GetSetInfoBydesc(PTMSEntities context, string desc, string type)
        {
            var result = from i in context.CFG_APP_CONFIG
                         where i.SECTION_TYPE == type && i.SECTION_DESC == desc
                         select new AppConfig
                         {
                             ID = i.ID,
                             SECTION_DESC = i.SECTION_DESC,
                             //SECTION_LEVEL = i.SECTION_LEVEL,
                             SECTION_NAME = i.SECTION_NAME,
                             SECTION_TYPE = i.SECTION_TYPE,
                             SECTION_VALUE = i.SECTION_VALUE,

                         };
            return result.FirstOrDefault();

        }
        /// <summary>
        /// get config by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<AppConfig> GetConfigInfoBytype(PTMSEntities context, string type, string desc)
        {
            var result = from i in context.CFG_APP_CONFIG
                         where i.SECTION_TYPE == type && (desc == "All" ? true : i.SECTION_NAME == desc) && i.SECTION_NAME != "AbnormalDoor"
                         select new AppConfig
                         {
                             ID = i.ID,
                             SECTION_DESC = i.SECTION_DESC,
                             //SECTION_LEVEL = i.SECTION_LEVEL,
                             SECTION_NAME = i.SECTION_NAME,
                             SECTION_TYPE = i.SECTION_TYPE,
                             SECTION_VALUE = i.SECTION_VALUE,

                         };
            return result.OrderBy(o => o.SECTION_NAME).ToList();

        }
        /// <summary>
        /// update config
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Boolean UpdateConfig(PTMSEntities context, string key, string value)
        {
            var entity = (from s in context.CFG_APP_CONFIG
                          where s.SECTION_NAME == key
                          select s).FirstOrDefault();
            if (entity == null)
            {
                return false;
            }

            entity.SECTION_VALUE = value;

            int i = context.SaveChanges();

            if (i >= 0)
            {
                return true;
            }
            return false;

        }

        public Boolean UpdateConfig(string key, string value, PTMSEntities context)
        {
            var entity = (from s in context.CFG_APP_CONFIG
                          where s.SECTION_NAME == key
                          select s).FirstOrDefault();
            if (entity == null)
            {
                return false;
            }

            entity.SECTION_VALUE = value;

            int i = context.SaveChanges();

            if (i >= 0)
            {
                return true;
            }
            return false;

        }

        public Boolean UpdateAllSection(PTMSEntities context, Dictionary<string, string> Dic)
        {
            bool issscu = true;
            var scope = new TransactionScope((TransactionScopeOption.Required));

            try
            {
                foreach (var item in Dic)
                {
                    issscu = UpdateConfig(context, item.Key, item.Value);
                    if (!issscu)
                    {
                        return false;
                    }
                }
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

            return true;

        }
    }
}
