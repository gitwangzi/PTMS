using EmailContract;
using EmailContract.Data;
using EmailRepository;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.Common.Logging;
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Gsafety.PTMS.BaseInfo;

namespace EmailService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class EmailService : BaseService, IEmailService
    {
        private EmailOperateRepository Repository = new EmailOperateRepository();
        public EmailService()
        {

        }

        public SingleMessage<bool> SendEmail(Email email)
        {
            try
            {
                Info("SendEmail");
                Info("email:" + Convert.ToString(email));
                var temp = Repository.SendEmail(email);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = temp, Result = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }
    }
}
