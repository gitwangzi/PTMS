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
    [Serializable]
    public class PolygonsRegion
    {

        [DataMember]
        public string UID { get; set; }//平台ID

        //[DataMember]
        public int SerialNo = SerialNoHelper.Create();//流水号

        [DataMember]
        public int RegionID { get; set; }//区域ID

        [DataMember]
        public List<int> RegionProperty { get; set; }//区域属性

        [DataMember]
        public string StartTime { get; set; }//起始时间

        [DataMember]
        public string EndTime { get; set; }//结束时间

        [DataMember]
        public int MaxSpeed { get; set; }//最高速度

        [DataMember]
        public int OverSpeedDuration { get; set; }//超速持续时间

        [DataMember]
        public int PointCount { get; set; }//总顶点数

        //[DataMember]
        private List<Point> pointsList;//区域内容

        [DataMember]
        public List<Point> PointsList
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
        public int RegionCount { get; set; }//总顶点数

        ////[DataMember]
        //private List<int> regionList;//区域内容

        //[DataMember]
        //public List<int> RegionList
        //{
        //    get { return regionList; }
        //    set
        //    {
        //        regionList = value;
        //        if (regionList != null && regionList.Count > 0)
        //            RegionCount = regionList.Count;
        //    }
        //}
    }
}
