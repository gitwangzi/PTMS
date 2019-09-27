using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.BaseInformation.Contract.Models;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Installation.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Gs.PTMS.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“InstallStaffService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 InstallStaffService.svc 或 InstallStaffService.svc.cs，然后开始调试。
    public class InstallStaffService : BaseService, IInstallationStaff, IMaintainApplication
    {
        #region installstaff....
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public SingleMessage<bool> InsertInstallationStaff(InstallationStaff model)
        {
            Info("InsertInstallationStaff");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = InstallationStaffRepository.InsertInstallationStaff(context, model);
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
        /// 修改
        /// </summary>
        public SingleMessage<bool> UpdateInstallationStaff(InstallationStaff model)
        {
            Info("UpdateInstallationStaff");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = InstallationStaffRepository.UpdateInstallationStaff(context, model);
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
        /// 删除
        /// </summary>
        public SingleMessage<bool> DeleteInstallationStaffByID(string ID)
        {
            Info("DeleteInstallationStaffByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = InstallationStaffRepository.DeleteInstallationStaffByID(context, ID);
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
        /// 获取
        /// </summary>
        public SingleMessage<InstallationStaff> GetInstallationStaff(string ID)
        {
            Info("GetInstallationStaff");
            Info(ID.ToString());
            try
            {
                SingleMessage<InstallationStaff> result = null;
                using (var context = new PTMSEntities())
                {
                    result = InstallationStaffRepository.GetInstallationStaff(context, ID);
                }
                Log<InstallationStaff>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<InstallationStaff>(false, ex);
            }
        }
        /// <summary>
        /// 获取
        /// </summary>
        public MultiMessage<InstallationStaff> GetInstallationStaffList(int pageIndex, int pageSize)
        {
            Info("GetInstallationStaffList");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<InstallationStaff> result = null;
                using (var context = new PTMSEntities())
                {
                    result = InstallationStaffRepository.GetInstallationStaffList(context, pageIndex, pageSize);
                }
                Log<InstallationStaff>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallationStaff>(false, ex);
            }
        }
        
        #endregion


        #region application.....维修申请
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public SingleMessage<bool> InsertMaintainApplication(MaintainApplication model)
        {
            Info("InsertMaintainApplication");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                model.CreateTime = DateTime.UtcNow;
                using (var context = new PTMSEntities())
                {
                      result = MaintainApplicationRepository.InsertMaintainApplication(context, model);
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
        /// 修改
        /// </summary>
        public SingleMessage<bool> UpdateMaintainApplication(MaintainApplication model)
        {
            Info("UpdateMaintainApplication");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result =MaintainApplicationRepository.UpdateMaintainApplication(context, model);
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
        /// 删除
        /// </summary>
        public SingleMessage<bool> DeleteMaintainApplicationByID(string ID)
        {
            Info("DeleteMaintainApplicationByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                     result = MaintainApplicationRepository.DeleteMaintainApplicationByID(context, ID);
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
        /// 获取
        /// </summary>
        public SingleMessage<MaintainApplication> GetMaintainApplication(string clientID,string ID)
        {
            Info("GetMaintainApplication");
            Info(ID.ToString());
            try
            {
                SingleMessage<MaintainApplication> result = null;
                using (var context = new PTMSEntities())
                {
                     result = MaintainApplicationRepository.GetMaintainApplication(context, ID);
                }
                Log<MaintainApplication>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<MaintainApplication>(false, ex);
            }
        }
        /// <summary>
        /// 获取
        /// </summary>
        public MultiMessage<MaintainApplication> GetMaintainApplicationList(string clientID, int pageIndex, int pageSize)
        {
            Info("GetMaintainApplicationList");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<MaintainApplication> result = null;
                using (var context = new PTMSEntities())
                {
                     result = MaintainApplicationRepository.GetMaintainApplicationList(context,clientID, pageIndex, pageSize);
                }
                Log<MaintainApplication>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<MaintainApplication>(false, ex);
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        public MultiMessage<MaintainApplication> GetMaintainApplicationByCondition(string clientID, string name, int pageIndex, int pageSize)
        {
            Info("GetMaintainApplicationByCondition");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<MaintainApplication> result = null;
                using (var context = new PTMSEntities())
                {
                      result = MaintainApplicationRepository.GetMaintainApplicationByCondition(context,clientID,name, pageIndex, pageSize);
                }
                Log<MaintainApplication>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<MaintainApplication>(false, ex);
            }
        }

        
        #endregion


    }
}
