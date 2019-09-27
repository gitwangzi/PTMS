using Gsafety.PTMS.Manager.Contract.LogManager;

namespace Gs.PTMS.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“LogTest”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 LogTest.svc 或 LogTest.svc.cs，然后开始调试。
    public class LogTest : ILogTest
    {

        public string ShowLogName()
        {
            return "LogNAME";
        }


        public string ShowLogName2()
        {
            return "LogNAME2";
        }
    }
}
