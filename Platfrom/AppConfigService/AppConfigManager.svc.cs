using Gsafety.Ant.AppConfig.Contract;
using Gsafety.Ant.AppConfig.Contract.Data;
using Gsafety.Ant.AppConfig.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Gsafety.Ant.AppConfig.Service
{
   
    public class AppConfigManager : IAppConfigManager,IDisposable
    {
        private DbOperatorHelper _helper = new DbOperatorHelper();

        public List<AppConfigModel> GetAllSections()
        {
            return _helper.GetAppConfigs();
        }

        public Dictionary<string, string> GetSectionByName(string name)
        {
            var result = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(name))
            {
                var cata = _helper.GetAppConfigs(name).FirstOrDefault();
                if (cata != null)
                {
                    result = cata.Children.ToDictionary(x => x.Value.SECTION_NAME, y => y.Value.SECTION_VALUE);
                }
            }
            return result;
        }

        public void  AddSection(AppConfigModel section)
        {
            _helper.AddOrUpdateModel(section);
          
        }

        public void UpdateSection(AppConfigModel section)
        {
            _helper.AddOrUpdateModel(section);           
        }

        public void DeleteSection(AppConfigModel section)
        {
            DeleteSectionById(section.Value.ID);
        }

        public void DeleteSectionById(string id)
        {
            _helper.DeleteModel(id);
            
        }

        public void Dispose()
        {
            _helper.Dispose();
        }


        public bool IsValidName(string id, string name, string parentName)
        {
            return !_helper.IsExistsSection(id,name, parentName);
        }
    }
}