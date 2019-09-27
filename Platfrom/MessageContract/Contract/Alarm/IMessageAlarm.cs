using Gsafety.PTMS.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract
{
    [ServiceContract]
    public interface IMessageAlarm
    {
        [OperationContract]
        void CompleteAlarm(CompleteAlarm alarm);

        [OperationContract]
        void TransferAlarm(AlarmInfoEx alarm);
    }
}
