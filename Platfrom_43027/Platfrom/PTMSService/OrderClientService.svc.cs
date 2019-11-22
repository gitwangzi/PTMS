using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Models;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using LogServiceRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Transactions;

namespace Gs.PTMS.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“CloudAccountService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 CloudAccountService.svc 或 CloudAccountService.svc.cs，然后开始调试。
    public class OrderClientService : BaseService, IOrderClientService
    {
        /// <summary>
        /// 创建一个新的云帐户
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public SingleMessage<bool> InsertOrderClient(OrderClientEx model, LogManager log)
        {
            Info("InsertOrderClient");
            Info(model);
            try
            {
                SingleMessage<bool> result;
                using (var context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);
                    try
                    {
                        result = OrderClientRepository.InsertOrderClient(context, model);
                        if (result.IsSuccess)
                        {
                            log.CreateTime = DateTime.UtcNow;
                            result = LogManagerRepository.InsertLogManager(context, log);
                        }
                        if (result.IsSuccess)
                        {
                            scope.Complete();
                        }
                    }
                    finally
                    {
                        scope.Dispose();
                    }
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

        public SingleMessage<bool> UpdateOrderClient(OrderClientEx model, LogManager log)
        {
            Info("UpdateOrderClient");
            Info(model);
            try
            {
                SingleMessage<bool> result;
                using (var context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);
                    try
                    {
                        result = OrderClientRepository.UpdateOrderClient(context, model);
                        if (result.IsSuccess)
                        {
                            log.CreateTime = DateTime.UtcNow;
                            result = LogManagerRepository.InsertLogManager(context, log);
                        }
                        if (result.IsSuccess)
                        {
                            scope.Complete();
                        }
                    }
                    finally
                    {
                        scope.Dispose();
                    }
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

        public MultiMessage<OrderClientEx> GetOrderClientExList(OrderClientManagerQueryModel obj)
        {
            Info("GetOrderClientExList");
            Info(obj);

            try
            {
                MultiMessage<OrderClientEx> result;
                using (var context = new PTMSEntities())
                {
                    result = OrderClientRepository.GetOrderClientExList(context, obj);
                }
                Log<OrderClientEx>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<OrderClientEx>(false, ex);
            }
        }

        public SingleMessage<bool> ResetPassword(string orderClientID, string newPassword, LogManager log)
        {
            Info("ResetPassword");
            Info(orderClientID);
            try
            {
                SingleMessage<bool> result;
                using (var context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);
                    try
                    {
                        result = OrderClientRepository.ResetPassword(context, orderClientID, newPassword);
                        if (result.IsSuccess)
                        {
                            log.CreateTime = DateTime.UtcNow;
                            result = LogManagerRepository.InsertLogManager(context, log);
                        }
                        if (result.IsSuccess)
                        {
                            scope.Complete();
                        }
                    }
                    finally
                    {
                        scope.Dispose();
                    }
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

        public SingleMessage<bool> SetOrderClientStatus(string orderClientID, bool enable, LogManager log)
        {
            Info("SetOrderClientStatus");
            Info(orderClientID);
            Info(enable);
            try
            {
                SingleMessage<bool> result;
                using (var context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);
                    try
                    {
                        result = OrderClientRepository.SetOrderClientStatus(context, orderClientID, enable);
                        if (result.IsSuccess)
                        {
                            log.CreateTime = DateTime.UtcNow;
                            result = LogManagerRepository.InsertLogManager(context, log);
                        }
                        if (result.IsSuccess)
                        {
                            scope.Complete();
                        }
                    }
                    finally
                    {
                        scope.Dispose();
                    }
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

        public SingleMessage<OrderClient> GetOrderClient(string ID)
        {
            Info("GetOrderClient");
            Info(ID.ToString());
            try
            {
                SingleMessage<OrderClient> result = null;
                using (var context = new PTMSEntities())
                {
                    result = OrderClientRepository.GetOrderClient(context, ID);
                }
                Log<OrderClient>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<OrderClient>(false, ex);
            }
        }
    }
}
