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
using Newtonsoft.Json;


namespace LogisticsWebApi.Models
{
    public class MapTransfer
    {
       // 


        public static string GetDataAsync(string url)
        {
            var result = string.Empty;
            try
            {

                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                var httpwebrequest = (HttpWebRequest)WebRequest.Create(url);
                httpwebrequest.ContentType = "application/json";
                httpwebrequest.Method = "Get";


                var httpResponse = (HttpWebResponse)httpwebrequest.GetResponse();
                using (var sr = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    return result;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return null;

            }
        }

        public static GeometriesItem GeographicToWebMercator_ArgGis(string lon, string lat)
        {

            string GeoUrl = System.Configuration.ConfigurationManager.AppSettings["GisTransformServiceUrl"];

            if (GeoUrl != string.Empty)
            {

                if (!GeoUrl.EndsWith("/"))
                {
                    GeoUrl += "/";
                }
            }
            string url = GeoUrl + "?inSR=4326&outSR=3857&transformation=&transformForward=true&vertical=false&f=json&geometries=" + lon + "," + lat;

            string PoiList = GetDataAsync(url);

            if (!string.IsNullOrEmpty(PoiList))
            {

                ArgGisTransformRoot data = JsonConvert.DeserializeObject<ArgGisTransformRoot>(PoiList);

                if (data != null && data.geometries.Count>0)
                {
                    return data.geometries[0];
                }
               
            }
            return null;


        }

        public static GeometriesItem WebMercatorToGeographic_ArgGis(string lon, string lat)
        {

            string GeoUrl = System.Configuration.ConfigurationManager.AppSettings["GisTransformServiceUrl"];

            if (GeoUrl != string.Empty)
            {

                if (!GeoUrl.EndsWith("/"))
                {
                    GeoUrl += "/";
                }
            }
            string url = GeoUrl + "?inSR=3857&outSR=4326&transformation=&transformForward=true&vertical=false&f=json&geometries=" + lon + "," + lat;

            string PoiList = GetDataAsync(url);

            if (!string.IsNullOrEmpty(PoiList))
            {

                ArgGisTransformRoot data = JsonConvert.DeserializeObject<ArgGisTransformRoot>(PoiList);

                if (data != null && data.geometries.Count > 0)
                {
                    return data.geometries[0];
                }

            }
            return null;


        }

        public static MapPoint GeographicToWebMercator(MapPoint point)
        {
            var flag = 2 * Math.PI * 6378137 / 2.0;
            var mx = point.X * flag / 180.0;
            var my = Math.Log(Math.Tan((90 + point.Y) * Math.PI / 360.0)) / (Math.PI / 180.0);

            my = my * flag / 180.0;

            //const int radius = 6378137;
            //const double minorRadius = 6356752.314245179;

            //const double d = Math.PI / 180;
            //const double r = radius;
            //var y = point.Y * d;
            //const double tmp = minorRadius / r;
            //double e = Math.Sqrt(1 - tmp * tmp),
            //    con = e * Math.Sin(y);

            //var ts = Math.Tan(Math.PI / 4 - y / 2) / Math.Pow((1 - con) / (1 + con), e / 2);
            //y = -r * Math.Log(Math.Max(ts, 1E-10));

            //var xValue = point.X * d * r;
            //var yValue = y;


            return new MapPoint(mx, my);

        }

        public static MapPoint WebMercatorToGeographic(MapPoint point)
        {


            var flag = 2 * Math.PI * 6378137 / 2.0;

            var x = point.X / flag * 180;
            var y = point.Y / flag * 180;
            y = 180 / Math.PI * (2 * Math.Atan(Math.Exp(y * Math.PI / 180)) - Math.PI / 2);

            var longitude = x;
            var latitude = y;



            return new MapPoint(x,y);

        }
    }
}