using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.Traffic.Contract.Data;
using Gsafety.PTMS.Traffic.Repository;
using Gsafety.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.MonitorAlert
{
    class MonitorAlertPlan
    {
       
        public MonitorAlertPlan()
        {

        }       

        /// <summary>
        /// 判断是否在计划点范围内
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public bool IsPointInPlan(string x1, string y1, string x2, string y2, double radius)
        {
            try
            {
                //采用地球大圆的算法
                SimplePoint ptGPS = GridCellCoord.GPS2GridCoord(x1, y1);
                double lon1 = ptGPS.X / 100;
                double lat1 = ptGPS.Y / 100;
                double lon2 = double.Parse(x2);
                double lat2 = double.Parse(y2);

                double EarthRadius = 6378.137;
                lon1 = lon1 / 180 * Math.PI;
                lon2 = lon2 / 180 * Math.PI;
                lat1 = lat1 / 180 * Math.PI;
                lat2 = lat2 / 180 * Math.PI;
                double x = Math.Sqrt(Math.Pow((Math.Sin((lat1 - lat2) / 2)), 2) +
                 Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin((lon1 - lon2) / 2), 2));
                if ((2 * Math.Asin(x) * EarthRadius * 1000) <= radius)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                return false;
            }
        }

       
    }
    

}
