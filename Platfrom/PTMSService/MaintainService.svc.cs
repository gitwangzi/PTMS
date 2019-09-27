using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Maintain.Contract;
using Gsafety.PTMS.Maintain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Gs.PTMS.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“MaintainService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 MaintainService.svc 或 MaintainService.svc.cs，然后开始调试。
    public class MaintainService : BaseService, IMaintainServiceContract
    {
        /// <summary>
        /// 添加维修申请
        /// </summary>
        /// <param name="model">维修申请</param>
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
        /// 修改维修申请
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
                    result = MaintainApplicationRepository.UpdateMaintainApplication(context, model);
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
        /// 删除维修申请
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
        /// 获取维修申请
        /// </summary>
        public SingleMessage<MaintainApplication> GetMaintainApplication(string ID)
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
        /// 获取维修申请
        /// </summary>
        public MultiMessage<MaintainApplication> GetMaintainApplicationList(string clientID, int state, string applicant, string vehicleID, string application, int pageIndex, int pageSize)
        {
            Info("GetMaintainApplicationList");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<MaintainApplication> result = null;
                using (var context = new PTMSEntities())
                {
                    result = MaintainApplicationRepository.GetMaintainApplicationList(context, clientID,state, applicant, vehicleID, application, pageIndex, pageSize);
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

        public MultiMessage<InstallStation> GetInstallStationList(string clientID)
        {
            Info("GetInstallStationList");

            try
            {
                MultiMessage<InstallStation> result = null;
                using (var context = new PTMSEntities())
                {
                    result = MaintainApplicationRepository.GetInstallStationList(context, clientID);
                }
                Log<InstallStation>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<InstallStation>(false, ex);
            }
        }


        //**************************************************Record*****************************************************************
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public SingleMessage<bool> InsertMaintainRecord(MaintainRecord model)
        {
            Info("InsertMaintainRecord");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                model.CreateTime = DateTime.UtcNow;
                using (var context = new PTMSEntities())
                {
                    result = MaintainRecordRepository.InsertMaintainRecord(context, model);
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
        public SingleMessage<bool> UpdateMaintainRecord(MaintainRecord model)
        {
            Info("UpdateMaintainRecord");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = MaintainRecordRepository.UpdateMaintainRecord(context, model);
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
        public SingleMessage<bool> DeleteMaintainRecordByID(string ID)
        {
            Info("DeleteMaintainRecordByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = MaintainRecordRepository.DeleteMaintainRecordByID(context, ID);
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
        public SingleMessage<MaintainRecord> GetMaintainRecord(string ID)
        {
            Info("GetMaintainRecord");
            Info(ID.ToString());
            try
            {
                SingleMessage<MaintainRecord> result = null;
                using (var context = new PTMSEntities())
                {
                    result = MaintainRecordRepository.GetMaintainRecord(context, ID);
                }
                Log<MaintainRecord>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<MaintainRecord>(false, ex);
            }
        }
        /// <summary>
        /// 获取
        /// </summary>
        public MultiMessage<MaintainRecord> GetMaintainRecordList(string clientID, string worker, DateTime? beginTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            Info("GetMaintainRecordList");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<MaintainRecord> result = null;
                using (var context = new PTMSEntities())
                {
                    result = MaintainRecordRepository.GetMaintainRecordList(context,clientID,worker,beginTime,endTime, pageIndex, pageSize);
                }
                Log<MaintainRecord>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<MaintainRecord>(false, ex);
            }
        }
        //*****************************************************************************************************

        /// <summary>
        /// 获取
        /// </summary>
        public MultiMessage<MaintainRecordUnfinished> GetMaintainRecordUnfinishedList(string clientID, string vehcileID, string contact, string worker, int pageIndex, int pageSize)
        {
            Info("GetMaintainRecordUnfinishedList");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<MaintainRecordUnfinished> result = null;
                using (var context = new PTMSEntities())
                {
                    result = MaintainRecordUnfinishedRepository.GetMaintainRecordUnfinishedList(context,clientID, vehcileID, contact, worker, pageIndex, pageSize);
                }
                Log<MaintainRecordUnfinished>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<MaintainRecordUnfinished>(false, ex);
            }
        }
        



    }
}
