using Gsafety.PTMS.Email.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Email.Contract
{
    [ServiceContract]
    public interface IEmailService
    {
        [OperationContract]
        SingleMessage<Boolean> SendEmail(EmailInfo email);
        [OperationContract]
        bool SendTest(bool x);
    }
}
