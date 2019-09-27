using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data.Enum
{
    /// <summary>
    /// traffic,command,message
    /// </summary>
    public enum CommandStateEnum
    {
        /// <summary>
        /// 未发送
        /// </summary>
        UnDelivered = 0,
        /// <summary>
        /// 等待下发
        /// </summary>
        WaitForDeliver = 1,
        /// <summary>
        /// 发送中
        /// </summary>
        Delivering = 2,
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 3,
        /// <summary>
        /// 失败
        /// </summary>
        Failed = 4,
    }
}
