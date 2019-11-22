using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Gsafety.PTMS.BaseInformation.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“InstallStaffService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 InstallStaffService.svc 或 InstallStaffService.svc.cs，然后开始调试。
     [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class InstallStaffService : IInstallStaffService
    {

        private InstallStaffRepository Repository = new InstallStaffRepository();
        public void DoWork()
        {
        }

        ///// <summary>
        ///// 查询所有安装/维修人员基本信息
        ///// </summary>
        ///// <param name="type">人员类型</param>
        ///// <returns>安装维修人员基本信息</returns>
        //public MultiMessage<InstallStaffBasicInfo> GetInstallStaffByType(MaintenanceStaffType type)
        //{
        //    try
        //    {
        //        var result = Repository.GetInstallStaffByType(type);
        //        return new MultiMessage<InstallStaffBasicInfo>() { Result = result };
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.Logger.Error(ex);
        //        return new MultiMessage<InstallStaffBasicInfo>() { ExceptionMessage = ex };
        //    }
        //}
    }
}
