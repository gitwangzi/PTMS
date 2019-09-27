using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LogServiceContract;
using LogServiceContract.Data;
using Gsafety.PTMS.DBEntity;
using LogServiceRepository;
using Gsafety.PTMS.Common.Data;

namespace Gs.PTMS.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class LogService : BaseService, ILogData, ILogManager
    {
        /********************LogData*******************/
        #region logData
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Obsolete]
        public SingleMessage<bool> InsertLogData(LogData model)
        {
            Info("InsertLogAccess");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                model.CreateTime = DateTime.UtcNow;
                using (var context = new PTMSEntities())
                {
                    result = LogDataRepository.InsertLogData(context, model);
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
        [Obsolete]
        public SingleMessage<bool> UpdateLogData(LogData model)
        {
            Info("UpdateLogData");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LogDataRepository.UpdateLogData(context, model);
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
        [Obsolete]
        public SingleMessage<bool> DeleteLogDataByID(string ID)
        {
            Info("DeleteLogDataByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LogDataRepository.DeleteLogDataByID(context, ID);
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

        [Obsolete]
        public MultiMessage<LogData> GetLogDataList(PagingInfo page, string clientID)
        {
            Info("GetLogDataList");
            Info(page.PageIndex.ToString());
            Info(page.PageSize.ToString());
            try
            {
                MultiMessage<LogData> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LogDataRepository.GetLogDataList(context, page.PageIndex, page.PageSize, clientID);
                }
                Log<LogData>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<LogData>(false, ex);
            }
        }
        [Obsolete]
        public MultiMessage<LogData> GetLogDataListByCondition(PagingInfo page, string clientID, string logName, DateTime? beginTime, DateTime? endTime)
        {
            Info("GetLogDataListByCondition");
            Info(page.PageIndex.ToString());
            Info(page.PageSize.ToString());
            try
            {
                MultiMessage<LogData> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LogDataRepository.GetLogDataListByCondition(context, page.PageIndex, page.PageSize, clientID, logName, beginTime, endTime);
                }
                Log<LogData>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<LogData>(false, ex);
            }
        }

        #endregion
        /********************LogManager*******************/


        public SingleMessage<bool> InsertLogManager(LogManager model)
        {
            Info("InsertLogManager");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                model.CreateTime = DateTime.UtcNow;
                using (var context = new PTMSEntities())
                {
                    result = LogManagerRepository.InsertLogManager(context, model);
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

        public SingleMessage<bool> DeleteLogManagerByID(string ID)
        {
            Info("UpdateLogManager");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LogManagerRepository.DeleteLogManagerByID(context, ID);
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

        public MultiMessage<LogManager> GetLogManagerList(PagingInfo page)
        {
            Info("GetLogManagerList");
            Info(page.PageIndex.ToString());
            Info(page.PageSize.ToString());
            try
            {
                MultiMessage<LogManager> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LogManagerRepository.GetLogManagerList(context, page.PageIndex, page.PageSize);
                }
                Log<LogManager>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<LogManager>(false, ex);
            }
        }

        public MultiMessage<LogManager> GetLogManagerListByCondition(PagingInfo page, string manager, DateTime? beginTime, DateTime? endTime)
        {
            Info("GetLogManagerListByCondition");
            Info(page.PageIndex.ToString());
            Info(page.PageSize.ToString());
            try
            {
                MultiMessage<LogManager> result = null;
                using (var context = new PTMSEntities())
                {
                    result = LogManagerRepository.GetLogManagerListByCondition(context, page.PageIndex, page.PageSize, manager, beginTime, endTime);
                }
                Log<LogManager>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<LogManager>(false, ex);
            }
        }

    }
}
