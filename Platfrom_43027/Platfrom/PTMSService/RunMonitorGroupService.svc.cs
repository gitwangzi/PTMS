using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Monitor.Contract;
using Gsafety.PTMS.Monitor.Contract.Data;
using Gsafety.PTMS.Monitor.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace Gs.PTMS.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“RunMonitorGroupService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 RunMonitorGroupService.svc 或 RunMonitorGroupService.svc.cs，然后开始调试。
    public class RunMonitorGroupService : BaseService, IRunMonitorGroupService
    {
        public SingleMessage<bool> ChangeRunMonitorGroup(ObservableCollection<RunMonitorGroup> groupModel, string userID)
        {
            Info("ChangeRunMonitorGroup");
            Info(groupModel.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RunMonitorGroupRepository.ChangeRunMonitorGroup(context, groupModel, userID);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        public SingleMessage<bool> DeleteRunMonitorGroupByID(string ID)
        {
            Info("DeleteRunMonitorGroupByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RunMonitorGroupRepository.DeleteRunMonitorGroupByID(context, ID);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        /// <summary>
        /// 获取分组
        /// </summary>
        public MultiMessage<RunMonitorGroup> GetRunMonitorGroupList(int pageIndex, int pageSize, string userID)
        {
            Info("GetRunMonitorGroupList");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<RunMonitorGroup> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RunMonitorGroupRepository.GetRunMonitorGroupList(context, userID, pageIndex, pageSize);
                }
                Log<RunMonitorGroup>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<RunMonitorGroup>(false, ex);
            }
        }

        //================================


        public SingleMessage<bool> ChangeRunMonitorGroupVehicle(string carNo, string groupId)
        {
            Info("ChangeRunMonitorGroupVehicle");
            Info(carNo);
            Info(groupId);

            try
            {
                SingleMessage<bool> result = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = RunMonitorGroupRepository.ChangeRunMonitorGroupVehicle(context, carNo, groupId);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        /// <summary>
        /// 删除分组车辆
        /// </summary>
        public SingleMessage<bool> DeleteRunMonitorGroupVehicleByID(string vehicleID, string userID)
        {
            Info("DeleteRunMonitorGroupVehicleByID");
            Info(vehicleID.ToString());
            Info(userID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RunMonitorGroupRepository.DeleteRunMonitorGroupVehicleByID(context, vehicleID, userID);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        /// <summary>
        /// 获取分组车辆
        /// </summary>
        public MultiMessage<RunMonitorGroupVehicle> GetRunMonitorGroupVehicleList(int pageIndex, int pageSize, string userID)
        {
            Info("GetRunMonitorGroupVehicleList");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<RunMonitorGroupVehicle> result = null;
                using (var context = new PTMSEntities())
                {
                    result = RunMonitorGroupRepository.GetRunMonitorGroupVehicleList(context, userID, pageIndex, pageSize);
                }
                Log<RunMonitorGroupVehicle>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<RunMonitorGroupVehicle>(false, ex);
            }
        }
    }
}
