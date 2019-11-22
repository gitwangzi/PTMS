using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Leader.Contract;
using Gsafety.PTMS.Leader.Contract.Data;
//using Gsafety.PTMS.Integration.Repository;
using Gsafety.PTMS.DBEntity;


namespace Gsafety.PTMS.Integration.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“LeaderTerminalService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 LeaderTerminalService.svc 或 LeaderTerminalService.svc.cs，然后开始调试。
    public class LeaderTerminalService : BaseService, ILeaderTerminalService
    {
        //private LeaderTerminalRepository ltr = null;
        //#region Implementation of ILeaderTerminalService

        //public LeaderTerminalService()
        //{
        //    try
        //    {
        //        Info("LeaderTerminalService");
        //        ltr = new LeaderTerminalRepository();
        //    }
        //    catch (Exception ex)
        //    {
        //        Error(ex);

        //    }
        //}

        //public List<GPSModel> getGPSList()
        //{
        //    try
        //    {
        //        Info("getGPSList");

        //        using (PTMSEntities context = new PTMSEntities())
        //        {
        //            return LeaderTerminalRepository.getGPSList(context);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Error(ex);
        //        return null;
        //    }
        //}

        //public int getMdvrCount()
        //{
        //    try
        //    {
        //        Info("getMdvrCount");

        //        using (PTMSEntities context = new PTMSEntities())
        //        {
        //            return LeaderTerminalRepository.getMdvrCount(context);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Error(ex);
        //        return -1;
        //    }
        //}

        //#endregion
        public List<GPSModel> getGPSList()
        {
            throw new NotImplementedException();
        }

        public int getMdvrCount()
        {
            throw new NotImplementedException();
        }
    }
}
