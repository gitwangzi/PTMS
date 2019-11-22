using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Manager.Contract
{
    public enum ContentType
    {
        /// <summary>
        /// Daily GPS Data
        /// </summary>
        RealTimeGps,
        /// <summary>
        /// History GPS Data
        /// </summary>
        HistoryGps,
        /// <summary>
        /// One-Click Alarm GPS Data
        /// </summary>
        AlarmGps, 
        /// <summary>
        /// Real Time Video
        /// </summary>
        RealTimeVedio,
        /// <summary>
        /// History Video
        /// </summary>
        HistroyVedio,
        /// <summary>
        /// File Download
        /// </summary>
        FileDownload
    }
}
