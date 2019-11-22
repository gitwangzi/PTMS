using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Email.Contract.Data;
using Gsafety.PTMS.Email.Contract;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Email.Repository;
using Gsafety.PTMS.Manager.Contract.Data;
using Gsafety.Common.Logging;

namespace Gs.PTMS.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“EmailService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 EmailService.svc 或 EmailService.svc.cs，然后开始调试。
    public class EmailService : BaseService, IEmailService
    {
        private EmailOperateRepository Repository = new EmailOperateRepository();
        public SingleMessage<bool> SendEmail(EmailInfo email)
        {
            try
            {
                var result = Repository.SendEmail(email);
                return new SingleMessage<bool>() { IsSuccess = result, Result = result };
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public bool SendTest(bool x)
        {

            return true;
        }



    }
}
