using Gsafety.PTMS.Manager.Contract;
using System;

namespace Gs.PTMS.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    public class LogService : ILoginLogService
    {
        public void DoWork()
        {
        }

        public Gsafety.PTMS.Base.Contract.Data.SingleMessage<string> AddLoginLog(string userName, short type, string organization)
        {
            throw new NotImplementedException();
        }

        public Gsafety.PTMS.Base.Contract.Data.SingleMessage<Gsafety.PTMS.Manager.Contract.Data.LoginLogInfo> AddLogoutLog(string userName, string id)
        {
            throw new NotImplementedException();
        }

        public Gsafety.PTMS.Base.Contract.Data.MultiMessage<Gsafety.PTMS.Manager.Contract.Data.LoginLogInfo> GetLoginLog(string userName, DateTime startTime, DateTime endTime, Gsafety.PTMS.Base.Contract.Data.PagingInfo pageInfo)
        {
            throw new NotImplementedException();
        }

        public Gsafety.PTMS.Base.Contract.Data.MultiMessage<Gsafety.PTMS.Manager.Contract.Data.LoginLogInfo> GetUserOnline(string userName, DateTime startTime, DateTime endTime, Gsafety.PTMS.Base.Contract.Data.PagingInfo pageInfo)
        {
            throw new NotImplementedException();
        }

        public Gsafety.PTMS.Base.Contract.Data.SingleMessage<bool> ClearLoginLog(DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public string GetLogTest()
        {
            return "this is 日志";
        }
    }
}
