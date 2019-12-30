using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticsWebApi.Models
{
    public class ArgGisTransformRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GeometriesItem> geometries { get; set; }

    }

    //如果好用，请收藏地址，帮忙分享。
    public class GeometriesItem
    {
        /// <summary>
        /// 
        /// </summary>
        public double x { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double y { get; set; }
    }

}
