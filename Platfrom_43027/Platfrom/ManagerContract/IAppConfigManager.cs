using Gsafety.PTMS.Manager.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.Base.Contract.Data;

namespace Gsafety.PTMS.Manager.Contract
{
    /// <summary>
    /// AppConfig Management
    /// </summary>
    [ServiceContract]
    public interface IAppConfigManager
    {
        /// <summary>
        /// Get All Config
        /// </summary>
        /// <returns>Tree Of Config</returns>
        [OperationContract]
        List<ConfigTree> GetAllSections();
        /// <summary>
        /// Get Config By Section
        /// </summary>
        /// <param name="name">Section Name</param>
        /// <returns>Collection Of Config</returns>
        [OperationContract]
        Dictionary<string,string> GetSectionByName(string name);
        /// <summary>
        /// Add Config Tree
        /// </summary>
        /// <param name="tree">Config Tree</param>
        /// <returns>Operation Result</returns>
        [OperationContract]
        SingleMessage<Boolean> AddSection(ConfigTree tree);
        /// <summary>
        /// Update Config Tree
        /// </summary>
        /// <param name="tree">Config Tree</param>
        /// <returns>Operation Result</returns>
        [OperationContract]
        SingleMessage<Boolean> UpdateSection(ConfigTree tree);
        /// <summary>
        /// Delete Config Tree
        /// </summary>
        /// <param name="tree">Config Tree</param>
        /// <returns>Operation Result </returns>
        [OperationContract]
        SingleMessage<Boolean> DeleteSection(ConfigTree tree);
        /// <summary>
        /// Delete Config By ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>Operation Result</returns>
        [OperationContract]
        SingleMessage<Boolean> DeleteSectionById(string id);
        /// <summary>
        /// Check Name is Valid
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Config Name</param>
        /// <param name="parentName">Parent Name</param>
        /// <returns>Operation Result</returns>
        [OperationContract]
        SingleMessage<Boolean> IsValidName(string id, string name, string parentName);
        /// <summary>
        /// Get All Config
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<AppConfig> GetappConfigInfo(string desc);
        /// <summary>
        /// Update Config
        /// </summary>
        /// <param name="configkey">Config Key</param>
        /// <param name="configvalue">Config Value</param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> UpdateConfig(string configkey,string configvalue);
        /// <summary>
        /// Get the Configuration Of Email
        /// </summary>
        /// <returns></returns>
        [OperationContract]
         MultiMessage<AppConfig> GetappEmailInfo();
        /// <summary>
        /// Get Config By Type
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<AppConfig> GetConfigInfoBytype(string type,string desc);
        /// <summary>
        /// Update All Config
        /// </summary>
        /// <param name="Dic"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> UpdateAllSection(Dictionary<string, string> Dic);
        /// <summary>
        /// Get Config By Name
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<AppConfig> GetConfigInfoBySectionName(string Name);

        /// <summary>
        /// Get SetInfo By Description
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<AppConfig> GetSetInfoBydesc(string desc, string type);
    }
}
