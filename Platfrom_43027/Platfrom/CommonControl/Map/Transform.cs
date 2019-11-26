using ESRI.ArcGIS.Client.Geometry;
using Gsafety.Common.Converts;
using Gsafety.PTMS.Share;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Common
{
    public class Transform
    {
        public static MapPoint GeographicToWebMercator(MapPoint point)
        {
            switch (ApplicationContext.Instance.ServerConfig.BaseMapType)
            {
                case Gsafety.PTMS.Enums.BaseMapTypeEnum.ArcGisMap:
                    return ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(point);
                case Gsafety.PTMS.Enums.BaseMapTypeEnum.BingMap:
                //return ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(point);
                case Gsafety.PTMS.Enums.BaseMapTypeEnum.GoogleMap:
                    if (ApplicationContext.Instance.ServerConfig.Bias)
                    {
                        MapPoint ptgoogle = wgsToGcj(point.Y, point.X);
                        return ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(ptgoogle);
                    }
                    else
                    {
                        return ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(point);
                    }
                case Gsafety.PTMS.Enums.BaseMapTypeEnum.BaiduMap:
                    if (ApplicationContext.Instance.ServerConfig.Bias)
                    {
                        MapPoint pt = wgsToGcj(point.Y, point.X);
                        pt = gcjToBd(pt.Y, pt.X);
                        return ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(pt);
                    }
                    else
                    {
                        return ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(point);
                    }
                case PTMS.Enums.BaseMapTypeEnum.TsMap:
                    if (ApplicationContext.Instance.ServerConfig.Bias)
                    {
                        if (ApplicationContext.Instance.ServerConfig.BiasType.ToLower() == "basic")
                        {
                            MapPoint ptgoogle = wgsToGcj(point.Y, point.X);
                            return ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(ptgoogle);
                        }
                        else if (ApplicationContext.Instance.ServerConfig.BiasType.ToLower() == "extra")
                        {
                            MapPoint pt = wgsToGcj(point.Y, point.X);
                            pt = gcjToBd(pt.Y, pt.X);
                            return ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(pt);
                        }
                        else if (ApplicationContext.Instance.ServerConfig.BiasType.ToLower() == "custom")
                        {
                            MapPoint projectpoint = ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(point);
                            return new MapPoint(projectpoint.X + ApplicationContext.Instance.ServerConfig.BiasX, projectpoint.Y + ApplicationContext.Instance.ServerConfig.BiasY);
                        }
                        else
                        {
                            return ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(point);
                        }
                    }
                    else
                    {
                        return ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(point);
                    }
                    break;
                default:
                    return ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(point);
            }
        }

        public static MapPoint WebMercatorToGeographic(MapPoint point)
        {
            switch (ApplicationContext.Instance.ServerConfig.BaseMapType)
            {
                case Gsafety.PTMS.Enums.BaseMapTypeEnum.ArcGisMap:
                    return ESRI.ArcGIS.Client.Bing.Transform.WebMercatorToGeographic(point);
                case Gsafety.PTMS.Enums.BaseMapTypeEnum.BingMap:
                //return ESRI.ArcGIS.Client.Bing.Transform.WebMercatorToGeographic(point);
                case Gsafety.PTMS.Enums.BaseMapTypeEnum.GoogleMap:
                    if (ApplicationContext.Instance.ServerConfig.Bias)
                    {
                        MapPoint ptgoogle = ESRI.ArcGIS.Client.Bing.Transform.WebMercatorToGeographic(point);

                        return gcjToWgs(ptgoogle.Y, ptgoogle.X);
                    }
                    else
                    {
                        return ESRI.ArcGIS.Client.Bing.Transform.WebMercatorToGeographic(point);
                    }
                case Gsafety.PTMS.Enums.BaseMapTypeEnum.BaiduMap:
                    if (ApplicationContext.Instance.ServerConfig.Bias)
                    {
                        MapPoint ptbaidu = ESRI.ArcGIS.Client.Bing.Transform.WebMercatorToGeographic(point);
                        MapPoint pt = bdToGcj(ptbaidu.Y, ptbaidu.X);
                        pt = gcjToWgs(pt.Y, pt.X);
                        return pt;
                    }
                    else
                    {
                        return ESRI.ArcGIS.Client.Bing.Transform.WebMercatorToGeographic(point);
                    }
                case  PTMS.Enums.BaseMapTypeEnum.TsMap:
                    if (ApplicationContext.Instance.ServerConfig.Bias)
                    {
                        if (ApplicationContext.Instance.ServerConfig.BiasType.ToLower() == "basic")
                        {
                            MapPoint ptgoogle = ESRI.ArcGIS.Client.Bing.Transform.WebMercatorToGeographic(point);

                            return gcjToWgs(ptgoogle.Y, ptgoogle.X);
                        }
                        else if (ApplicationContext.Instance.ServerConfig.BiasType.ToLower() == "extra")
                        {
                            MapPoint ptbaidu = ESRI.ArcGIS.Client.Bing.Transform.WebMercatorToGeographic(point);
                            MapPoint pt = bdToGcj(ptbaidu.Y, ptbaidu.X);
                            pt = gcjToWgs(pt.Y, pt.X);
                            return pt;
                        }
                        else if (ApplicationContext.Instance.ServerConfig.BiasType.ToLower() == "custom")
                        {
                            MapPoint projectpoint = new MapPoint(point.X - ApplicationContext.Instance.ServerConfig.BiasX, point.Y - ApplicationContext.Instance.ServerConfig.BiasY);
                            return ESRI.ArcGIS.Client.Bing.Transform.WebMercatorToGeographic(projectpoint);
                        }
                        else
                        {
                            return ESRI.ArcGIS.Client.Bing.Transform.WebMercatorToGeographic(point);
                        }
                    }
                    else
                    {
                        return ESRI.ArcGIS.Client.Bing.Transform.WebMercatorToGeographic(point);
                    }
                default:
                    break;
            }

            return null;
        }

        private static MapPoint gcjToWgs(double Y, double X)
        {
            MapPoint pl = wgsToGcj(Y, X);
            double offsetLat = pl.Y - Y;
            double offsetLng = pl.X - X;

            return new MapPoint(X - offsetLng, Y - offsetLat);
        }

        static double a = 6378245.0;
        static double ee = 0.00669342162296594323;
        private static MapPoint wgsToGcj(double Y, double X)
        {
            double mgLat = 0;
            double mgLon = 0;
            double dLat = transformLat(X - 105.0, Y - 35.0);
            double dLon = transformLon(X - 105.0, Y - 35.0);
            double radLat = Y / 180.0 * Math.PI;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * Math.PI);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * Math.PI);
            mgLat = Y + dLat;
            mgLon = X + dLon;

            MapPoint pl = new MapPoint(mgLon, mgLat);
            return pl;
        }

        private static double transformLat(double x, double y)
        {
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y
                    + 0.2 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * Math.PI) + 20.0 * Math.Sin(2.0 * x * Math.PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * Math.PI) + 40.0 * Math.Sin(y / 3.0 * Math.PI)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * Math.PI) + 320 * Math.Sin(y * Math.PI / 30.0)) * 2.0 / 3.0;
            return ret;
        }

        private static double transformLon(double x, double y)
        {
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1
                    * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * Math.PI) + 20.0 * Math.Sin(2.0 * x * Math.PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * Math.PI) + 40.0 * Math.Sin(x / 3.0 * Math.PI)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * Math.PI) + 300.0 * Math.Sin(x / 30.0
                    * Math.PI)) * 2.0 / 3.0;
            return ret;
        }
        static double x_pi = Math.PI * 3000.0 / 180.0;

        public static MapPoint bdToGcj(double Y, double X)
        {
            double x = X - 0.0065, y = Y - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
            double gg_lon = z * Math.Cos(theta);
            double gg_lat = z * Math.Sin(theta);
            return new MapPoint(gg_lon, gg_lat);
        }

        public static MapPoint gcjToBd(double Y, double X)
        {
            double bd_lat;
            double bd_lon;
            double x = X, y = Y;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * x_pi);
            bd_lon = z * Math.Cos(theta) + 0.0065;
            bd_lat = z * Math.Sin(theta) + 0.006;

            return new MapPoint(bd_lon, bd_lat);
        }

        public static Envelope GetExtent(string str)
        {
            if ((str != null) && (str != ""))
            {
                string[] temp = str.Split(";".ToCharArray());
                if (temp.Length < 4) return null;
                DisplayLonConvert loncon = new DisplayLonConvert();
                DisplayLatConvert latcon = new DisplayLatConvert();
                double xmin = double.Parse(loncon.ConvertBack(temp[0], null, null, null).ToString());
                double ymin = double.Parse(latcon.ConvertBack(temp[1], null, null, null).ToString());
                double xmax = double.Parse(loncon.ConvertBack(temp[2], null, null, null).ToString());
                double ymax = double.Parse(latcon.ConvertBack(temp[3], null, null, null).ToString());

                //3857地图
                ESRI.ArcGIS.Client.Geometry.MapPoint pt1 = GetProjCoord(xmin, ymin);
                ESRI.ArcGIS.Client.Geometry.MapPoint pt2 = GetProjCoord(xmax, ymax);

                //ESRI.ArcGIS.Client.Geometry.MapPoint pt1 = new ESRI.ArcGIS.Client.Geometry.MapPoint(xmin, ymin);
                //ESRI.ArcGIS.Client.Geometry.MapPoint pt2 = new ESRI.ArcGIS.Client.Geometry.MapPoint(xmax, ymax);
                return new Envelope(pt1, pt2);
            }

            return null;
        }

        public static Envelope GetBaiduExtent(string str)
        {
            if ((str != null) && (str != ""))
            {
                string[] temp = str.Split(";".ToCharArray());
                if (temp.Length < 4) return null;
                DisplayLonConvert loncon = new DisplayLonConvert();
                DisplayLatConvert latcon = new DisplayLatConvert();
                double xmin = double.Parse(loncon.ConvertBack(temp[0], null, null, null).ToString());
                double ymin = double.Parse(latcon.ConvertBack(temp[1], null, null, null).ToString());
                double xmax = double.Parse(loncon.ConvertBack(temp[2], null, null, null).ToString());
                double ymax = double.Parse(latcon.ConvertBack(temp[3], null, null, null).ToString());

                MapPoint ptmin = wgsToGcj(ymin, xmin);
                ptmin = ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(ptmin);

                MapPoint ptmax = wgsToGcj(ymax, xmax);
                ptmax = ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(ptmax);

                return new Envelope(ptmin, ptmax);
            }

            return null;
        }

        public static MapPoint GetProjCoord(double xmin, double ymin)
        {
            ESRI.ArcGIS.Client.Geometry.MapPoint pt = new ESRI.ArcGIS.Client.Geometry.MapPoint(xmin, ymin);
            return GeographicToWebMercator(pt);
        }
    }
}
