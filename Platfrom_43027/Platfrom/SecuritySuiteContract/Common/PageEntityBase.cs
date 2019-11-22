using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.SecuritySuite.Contract
{
    public class PageEntityBase
    {
        /// <summary>
        /// 所有记录
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页数目
        /// </summary>
        public int PageSize { get; set; }
    }
}
