using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ESRI.ArcGIS.Client.Geometry;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;


namespace LogisticsWebApi.Models
{
    public class MapTransfer
    {
        public static MapPoint GeographicToWebMercator(MapPoint point)
        {

            return ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(point);
            
        }

        public static MapPoint WebMercatorToGeographic(MapPoint point)
        {

            return ESRI.ArcGIS.Client.Bing.Transform.WebMercatorToGeographic(point);

        }
    }
}