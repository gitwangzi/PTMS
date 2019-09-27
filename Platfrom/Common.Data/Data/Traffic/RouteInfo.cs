using Gsafety.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class RouteInfo
    {

        [DataMember]
        public string UID { get; set; }//平台ID

        //[DataMember]
        public int SerialNo = SerialNoHelper.Create();//流水号

        [DataMember]
        public int RouteId { get; set; }//路线Id

        [DataMember]
        public List<int> RouteAttribute { get; set; }//路线属性

        [DataMember]
        public string StartTime { get; set; } //开始时间

        [DataMember]
        public string EndTime { get; set; } //结束时间

        //[DataMember]
        public int PointCount { get; set; } //总拐点数

        private List<TurningPoint> pointsList;

        [DataMember]
        public List<TurningPoint> PointsList //拐点项
        {
            get { return pointsList; }
            set
            {
                pointsList = value;
                if (pointsList != null && pointsList.Count > 0)
                    PointCount = pointsList.Count;
            }
        }

        [DataMember]
        public int RouteCount { get; set; }//总顶点数

        //[DataMember]
        private List<int> routeList;//区域内容

        [DataMember]
        public List<int> RouteList
        {
            get { return routeList; }
            set
            {
                routeList = value;
                if (routeList != null && routeList.Count > 0)
                    RouteCount = routeList.Count;
            }
        }

    }
}
