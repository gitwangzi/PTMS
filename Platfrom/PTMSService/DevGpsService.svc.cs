using Gsafety.PTMS.BaseInformation.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.Ant.BaseInformation.Repository;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Data;

namespace Gs.PTMS.Service
{

    public class DevGpsService : BaseService, IDevGpsService
    {
        /// <summary>
        /// 添加GPS设备
        /// </summary>
        /// <param name="model">GPS设备</param>
        public SingleMessage<bool> InsertDevGps(DevGps model)
        {
            Info("InsertDevGps");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                model.CreateTime = DateTime.UtcNow;
                using (var context = new PTMSEntities())
                {
                    result = DevGpsRepository.InsertDevGps(context, model);
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
        /// 修改GPS设备
        /// </summary>
        public SingleMessage<bool> UpdateDevGps(DevGps model)
        {
            Info("UpdateDevGps");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = DevGpsRepository.UpdateDevGps(context, model);
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
        /// 删除GPS设备
        /// </summary>
        public SingleMessage<bool> DeleteDevGpsByID(string ID)
        {
            Info("DeleteDevGpsByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = DevGpsRepository.DeleteDevGpsByID(context, ID);
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
        /// 获取GPS设备
        /// </summary>
        public SingleMessage<DevGps> GetDevGps(string ID)
        {
            Info("GetDevGps");
            Info(ID.ToString());
            try
            {
                SingleMessage<DevGps> result = null;
                using (var context = new PTMSEntities())
                {
                    result = DevGpsRepository.GetDevGps(context, ID);
                }
                Log<DevGps>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<DevGps>(false, ex);
            }
        }

        /// <summary>
        /// 获取GPS设备
        /// </summary>
        public SingleMessage<DevGps> GetDevGpsBySN(string ID)
        {
            Info("GetDevGps");
            Info(ID.ToString());
            try
            {
                SingleMessage<DevGps> result = null;
                using (var context = new PTMSEntities())
                {
                    result = DevGpsRepository.GetDevGpsBySN(context, ID);
                }
                Log<DevGps>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<DevGps>(false, ex);
            }
        }


        /// <summary>
        /// 获取GPS设备
        /// </summary>
        public MultiMessage<DevGps> GetDevGpsList(PagingInfo page, string clientID)
        {
            Info("GetDevGpsList");
            Info(page.PageIndex.ToString());
            Info(page.PageSize.ToString());
            try
            {
                MultiMessage<DevGps> result = null;
                using (var context = new PTMSEntities())
                {
                    result = DevGpsRepository.GetDevGpsList(context, page.PageIndex, page.PageSize, clientID);
                }
                Log<DevGps>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<DevGps>(false, ex);
            }
        }
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="clientID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public MultiMessage<DevGps> GetByNameDevGpsList(PagingInfo page, string clientID, string name, string vehicleID)
        {
            Info("GetByNameDevGpsList");
            Info(page.PageIndex.ToString());
            Info(page.PageSize.ToString());
            try
            {
                MultiMessage<DevGps> result = null;
                using (var context = new PTMSEntities())
                {
                    result = DevGpsRepository.GetByNameDevGpsList(context, page.PageIndex, page.PageSize, clientID, name, vehicleID);
                }
                Log<DevGps>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<DevGps>(false, ex);
            }
        }

        public SingleMessage<Boolean> BatchAddDevGps(List<DevGps> devGpsBatchList)
        {
            try
            {
                Info("BatchAddDevGps");
                Info("devGpsBatchList:" + Convert.ToString(devGpsBatchList));
                var temp = DevGpsRepository.BatchAddDevGps(devGpsBatchList);
                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = true, Result = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public MultiMessage<DevGps> CheckDevGpsExist(List<DevGps> devGpsBatchList)
        {
            try
            {
                Info("CheckDevGpsExist");
                Info("devGpsBatchList:" + Convert.ToString(devGpsBatchList));
                var temp = DevGpsRepository.CheckDevGpsExist(devGpsBatchList);
                Log<DevGps>(temp);
                return temp;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<DevGps>() { IsSuccess = false, ExceptionMessage = ex };
            }
        }

    }
}
