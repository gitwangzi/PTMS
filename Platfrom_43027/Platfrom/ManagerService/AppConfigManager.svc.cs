using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Manager.Contract;
using Gsafety.PTMS.Manager.Contract.Data;
using Gsafety.PTMS.Manager.Repository;
using Gsafety.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.Manager.Service
{
    public class AppConfigManager : BaseService, IAppConfigManager
    {
        private DbOperatorHelper _helper = new DbOperatorHelper();
        private PTMSAppConfigRepository apprepository = new PTMSAppConfigRepository();
        public List<ConfigTree> GetAllSections()
        {

            using (PTMSEntities context = new PTMSEntities())
            {
                return _helper.GetAppConfigs(context);
            }

        }

        public Dictionary<string, string> GetSectionByName(string name)
        {
            var result = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(name))
            {
                var cata = new ConfigTree();
                using (PTMSEntities context = new PTMSEntities())
                {
                    cata = _helper.GetAppConfigs(context, name).FirstOrDefault();
                }

                if (cata != null)
                {
                    result = cata.Children.ToDictionary(x => x.Value.SectionName, y => y.Value.SectionValue);
                }
            }
            return result;
        }
        public SingleMessage<Boolean> UpdateAllSection(Dictionary<string, string> Dic)
        {
            try
            {
                Info("UpdateAllSection");
                Info("Dic:" + Convert.ToString(Dic));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = apprepository.UpdateAllSection(context, Dic);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                return new SingleMessage<bool>() { Result = false, IsSuccess = false, ExceptionMessage = ex };
            }
        }
        public SingleMessage<Boolean> AddSection(ConfigTree section)
        {
            try
            {
                Info("AddSection");
                Info("section:" + Convert.ToString(section));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = _helper.AddOrUpdateModel(section, context);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { Result = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<Boolean> UpdateSection(ConfigTree section)
        {
            try
            {
                Info("UpdateSection");
                Info("section:" + Convert.ToString(section));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = _helper.AddOrUpdateModel(section, context);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { Result = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<Boolean> DeleteSection(ConfigTree section)
        {
            try
            {
                Info("DeleteSection");
                Info("section:" + Convert.ToString(section));

                SingleMessage<bool> result = DeleteSectionById(section.Value.Id);
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { Result = false, ExceptionMessage = ex };
            }
        }
        public SingleMessage<Boolean> DeleteSectionById(string id)
        {
            try
            {
                Info("DeleteSectionById");
                Info("id:" + Convert.ToString(id));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = _helper.DeleteModel(id, context);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { Result = false, ExceptionMessage = ex };
            }
        }

        public SingleMessage<Boolean> IsValidName(string id, string name, string parentName)
        {
            try
            {
                Info("IsValidName");
                Info("id:" + Convert.ToString(id) + ";" + "name:" + Convert.ToString(name) + ";" + "parentName:" + Convert.ToString(parentName));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = !_helper.IsExistsSection(context, id, name, parentName);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { IsSuccess = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }
        public MultiMessage<AppConfig> GetappConfigInfo(string desc)
        {
            try
            {
                Info("GetappConfigInfo");
                Info("desc:" + Convert.ToString(desc));
                var temp = new List<AppConfig>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = apprepository.GetappConfigInfolist(context, desc, "BaseInfo");
                }


                MultiMessage<AppConfig> result = new MultiMessage<AppConfig> { Result = temp };
                Log<AppConfig>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<AppConfig>() { Result = null, ExceptionMessage = ex };
            }
        }
        public MultiMessage<AppConfig> GetappEmailInfo()
        {
            try
            {
                Info("GetappEmailInfo");
                var temp = new List<AppConfig>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = apprepository.GetappEmailInfo(context);
                }


                MultiMessage<AppConfig> result = new MultiMessage<AppConfig> { Result = temp };
                Log<AppConfig>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<AppConfig>() { Result = null, ExceptionMessage = ex };
            }
        }
        public SingleMessage<Boolean> UpdateConfig(string configkey, string configvalue)
        {
            try
            {
                Info("UpdateConfig");
                Info("configkey:" + Convert.ToString(configkey) + ";" + "configvalue:" + Convert.ToString(configvalue));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = apprepository.UpdateConfig(context, configkey, configvalue);
                }


                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { Result = false, ExceptionMessage = ex };
            }
        }
        /// <summary>
        /// Get Config Info By type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public MultiMessage<AppConfig> GetConfigInfoBytype(string type, string desc)
        {
            try
            {
                Info("GetConfigInfoBytype");
                Info("type:" + Convert.ToString(type) + ";" + "desc:" + Convert.ToString(desc));
                var temp = new List<AppConfig>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = apprepository.GetConfigInfoBytype(context, type, desc);
                }


                MultiMessage<AppConfig> result = new MultiMessage<AppConfig> { Result = temp, IsSuccess = true };
                Log<AppConfig>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<AppConfig>() { Result = null, IsSuccess = false, ExceptionMessage = ex };
            }
        }
        public SingleMessage<AppConfig> GetConfigInfoBySectionName(string Name)
        {
            try
            {
                Info("GetConfigInfoBySectionName");
                Info("Name:" + Convert.ToString(Name));
                var temp = new AppConfig();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = apprepository.GetappConfigInfo(context, Name);
                }


                SingleMessage<AppConfig> result = new SingleMessage<AppConfig> { Result = temp, IsSuccess = true };
                Log<AppConfig>(result);
                return result;
            }
            catch (Exception ex)
            {
                return new SingleMessage<AppConfig>() { Result = null, IsSuccess = false, ExceptionMessage = ex };
            }
        }
        public SingleMessage<AppConfig> GetSetInfoBydesc(string desc, string type)
        {
            try
            {
                Info("GetSetInfoBydesc");
                Info("desc:" + Convert.ToString(desc) + ";" + "type:" + Convert.ToString(type));
                var temp = new AppConfig();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = apprepository.GetSetInfoBydesc(context, desc, type);
                }


                SingleMessage<AppConfig> result = new SingleMessage<AppConfig> { Result = temp, IsSuccess = true };
                Log<AppConfig>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<AppConfig>() { Result = null, IsSuccess = false, ExceptionMessage = ex };

            }
        }
    }
}
