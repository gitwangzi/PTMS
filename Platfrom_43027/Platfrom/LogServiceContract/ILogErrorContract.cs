using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LogServiceContract
{
    [ServiceContract]
    public interface IPTMSLogError
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        //[OperationContract]
        //SingleMessage<bool> InsertErrorLog(LogError models);

        [OperationContract]
        MultiMessage<LogError> GetErrorLog(string clientID, string content, DateTime? startTime, DateTime? endTime, PagingInfo pageInfo);
    }
}
