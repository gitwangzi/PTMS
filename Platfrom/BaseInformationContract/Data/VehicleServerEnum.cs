using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
    public enum VehicleServerEnum
    {
        /// <summary>
        /// 商用
        /// </summary>
        Business = 1,
        /// <summary>
        /// 公共
        /// </summary>
        Public = 2,
        /// <summary>
        /// 私有
        /// </summary>
        Private = 3,
        /// <summary>
        /// 未知
        /// </summary>
        None = 99
    }
}
