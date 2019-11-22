using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Ant.Base.Contract.Data
{
    public enum Traffic_FenceType
    {
        /// <summary>
        /// 无
        /// </summary>
        NoType = 0x00,
        /// <summary>
        /// 监控点
        /// </summary>
        ControlPoint = 0x01,
        /// <summary>
        /// 区域围栏
        /// </summary>
        PolygonFence = 0x02
    }
}
