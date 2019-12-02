namespace LogisticsWebApi.Models
{
    using System;

    public class Location
    {
        /// <summary>
        /// 经度，(-180,180]
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 纬度，[-90,90]，南极-90，赤道0，北极90
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        public string Speed { get; set; }
        /// <summary>
        /// 方向，[0,360)，正北为0度，正东90度，正南180度，正西270度
        /// </summary>
        public string Direction { get; set; }
        /// <summary>
        /// 时间，0时区
        /// </summary>
        public string GPSTime { get; set; }
    }
}
