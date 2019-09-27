using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
    /// <summary>
    /// 安全套件状态
    /// </summary>
    public enum DeviceSuiteStatusMangement
    {
        /// <summary>
        /// 工作中
        /// </summary>
        Working = 20,
        /// <summary>
        /// 测试
        /// </summary>
        Testing = 22,
        /// <summary>
        /// 异常
        /// </summary>
        Abnormal = 24,
        /// <summary>
        /// 维修
        /// </summary>
        Maintenance = 30,
        /// <summary>
        /// 报废
        /// </summary>
        Scrap = 40,
    
    }
}

