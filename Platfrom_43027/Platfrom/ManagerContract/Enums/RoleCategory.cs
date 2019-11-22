
namespace Gsafety.PTMS.Manager.Contract
{
    /// <summary>
    /// 用户类型枚举
    /// </summary>
    public enum RoleCategory : short
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        SuperPower = 0,
        /// <summary>
        /// 定单客户
        /// </summary>
        ClientAdmin = 1,
        /// <summary>
        /// 系统管理员
        /// </summary>
        SecurityAdmin = 2,
        /// <summary>
        /// 监督管理员
        /// </summary>
        SecurityMonitor = 3,
        /// <summary>
        /// 运维管理员
        /// </summary>
        MaintainAdmin = 4,
        /// <summary>
        /// 运维人员
        /// </summary>
        MaintainMonitor = 5,
        /// <summary>
        /// 其他
        /// </summary>
        Other = 6
    }
}
