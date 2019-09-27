using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Manager.Contract.Data;

namespace Gsafety.PTMS.Manager.Contract
{
    [ServiceContract]
    public interface IGetSuiteInfoService
    {
        [OperationContract]
        MultiMessage<SuiteInfoLog> GetSuitInfo(string suiteID, DateTime startTime, DateTime endTime, PagingInfo pageInfo);
    }
}
