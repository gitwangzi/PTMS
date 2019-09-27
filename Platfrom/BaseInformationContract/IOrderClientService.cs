using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Models;
using Gsafety.PTMS.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInformation.Contract
{
    [ServiceContract]
    public interface IOrderClientService
    {
        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="model">客户</param>
        [OperationContract]
        SingleMessage<bool> InsertOrderClient(OrderClientEx model, LogManager log);

        /// <summary>
        /// 修改客户
        /// </summary>
        /// <param name="model">客户</param>
        [OperationContract]
        SingleMessage<bool> UpdateOrderClient(OrderClientEx model, LogManager log);

        /// <summary>
        /// 获取客户
        /// </summary>
        /// <returns>获取客户列表及附加信息</returns>
        [OperationContract]
        MultiMessage<OrderClientEx> GetOrderClientExList(OrderClientManagerQueryModel obj);

        /// <summary>
        /// 重置密码
        /// 设置为默认密码
        /// </summary>
        /// <param name="orderClientID">客户ID</param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<bool> ResetPassword(string orderClientID, string newPassword, LogManager log);

        /// <summary>
        /// 开通或暂停使用
        /// </summary>
        /// <param name="orderClientID">客户ID</param>
        /// <param name="enable">True表示开通，FALSE表示暂停</param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<bool> SetOrderClientStatus(string orderClientID, bool enable, LogManager log);

        /// <summary>
        /// 获取客户
        /// </summary>
        /// <returns>获取客户</returns>
        [OperationContract]
        SingleMessage<OrderClient> GetOrderClient(string ID);
    }
}
