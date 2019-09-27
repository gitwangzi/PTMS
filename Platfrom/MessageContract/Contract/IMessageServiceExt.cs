using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract
{
    [ServiceContract(CallbackContract = typeof(IMessageCallBackExt))]
    public interface IMessageServiceExt : IMessageAlarm, IMessageAlart, IMessageCommand, IMessageGPS, IMessageInstall, IMessageManagement, IMessageMonitor, IMessageUser
    {
        
    }
}
