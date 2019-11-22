using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Manager.Contract.Data.CommandManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Manager.Contract
{
    [ServiceContract]
    public interface ILocationReportRuleService
    {
        /// <summary>
        /// 添加LocationReportRule设备
        /// </summary>
        /// <param name="model">LocationReportRule设备</param>
        [OperationContract]
        SingleMessage<bool> InsertLocationReportRule(LocationReportRule model);

        /// <summary>
        /// 修改GPS设备
        /// </summary>
        /// <param name="model">LocationReportRule设备</param>
        [OperationContract]
        SingleMessage<bool> UpdateLocationReportRule(LocationReportRule model);

        /// <summary>
        /// 删除LocationReportRule设备
        /// </summary>
        /// <param name="model">LocationReportRule设备</param>
        [OperationContract]
        SingleMessage<bool> DeleteLocationReportRuleByID(string ID);

        [OperationContract]
        MultiMessage<LocationReportRule> GetByNameLocationReportRuleList(PagingInfo page, string clientID, string name);
    }
}
