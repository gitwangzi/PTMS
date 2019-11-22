using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Message.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract
{
    public interface IMessageCallBackExt
    {
        [OperationContract(IsOneWay = true)]
        [ServiceKnownType(typeof(HeartbeatInfo))]
        [ServiceKnownType(typeof(UserModel))]
        [ServiceKnownType(typeof(BaseInfo))]
        [ServiceKnownType(typeof(Gsafety.PTMS.Common.Data.GPS))]
        [ServiceKnownType(typeof(OnOfflineEx))]
        [ServiceKnownType(typeof(AlarmInfoEx))]
        [ServiceKnownType(typeof(DeviceAlertEx))]
        [ServiceKnownType(typeof(BusinessAlertEx))]
        [ServiceKnownType(typeof(Gsafety.PTMS.Common.Data.CompleteAlarm))]
        [ServiceKnownType(typeof(Gsafety.PTMS.Common.Data.CompleteAlert))]
        [ServiceKnownType(typeof(Gsafety.PTMS.Common.Data.QueryServerFileListMessageResponse))]
        [ServiceKnownType(typeof(TakePictureMessageResponse))]
        [ServiceKnownType(typeof(Vehicle))]
        [ServiceKnownType(typeof(AuthenticationInfo))]
        void MessageCallBack(object message);
    }
}
