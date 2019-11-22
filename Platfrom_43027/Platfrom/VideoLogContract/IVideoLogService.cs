using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.VideoLog.Contract
{
    [ServiceContract]
    public interface IVideoLogService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "AddVideoLog", Method = "POST")]
        int AddVideoLog(PTMSEntities context, AddVideoLogArgs args);

        [OperationContract]
        [WebInvoke(UriTemplate = "VerifyUser", Method = "POST")]
        VerifyUserResult VerifyUser(VerifyUserArgs args);
    }
}
