using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using Gsafety.PTMS.Share;
using Gsafety.Common.Converts;

namespace Gsafety.Common.Map
{
    public class GoogleMap : TiledMapServiceLayer
    {
        private const double cornerCoordinate = 20037508.3427892;
        private string _baseURL = "t@128"; //google地形图

        public string BaseURL
        {
            get { return _baseURL; }
            set { _baseURL = value; }
        }

        public override void Initialize()
        {
            Envelope env = Transform.GetExtent(ApplicationContext.Instance.ServerConfig.OverMapMaximumExtent);
            env.SpatialReference = new SpatialReference(102100);
            this.FullExtent = env;

            //图层的空间坐标系
            this.SpatialReference = new SpatialReference(102100);
            // 建立切片信息，每个切片大小256*256px，共16级.
            this.TileInfo = new TileInfo()
            {
                Height = 256,
                Width = 256,
                Origin = new MapPoint(-cornerCoordinate, cornerCoordinate) { SpatialReference = new ESRI.ArcGIS.Client.Geometry.SpatialReference(102100) },
                Lods = new Lod[16]
            };
            //为每级建立方案，每一级是前一级别的一半.
            double resolution = cornerCoordinate * 2 / 256;
            for (int i = 0; i < TileInfo.Lods.Length; i++)
            {
                TileInfo.Lods[i] = new Lod() { Resolution = resolution };
                resolution /= 2;
            }
            // 调用初始化函数
            base.Initialize();
        }


        public override string GetTileUrl(int level, int row, int col)
        {
            string country = ApplicationContext.Instance.ServerConfig.MapLanguage.Substring(3, 2).ToLower();
            string url = "http://mt" + (col % 4) + "." + ApplicationContext.Instance.ServerConfig.GoogleAddress + "/vt/lyrs=" + _baseURL + "&v=w2.114&hl=" + ApplicationContext.Instance.ServerConfig.MapLanguage + "&gl=" + country + "&" + "x=" + col + "&" + "y=" + row + "&" + "z=" + level + "&s=Galil";
            if (_baseURL == "s@92")
            {
                url = "http://mt" + (col % 4) + "." + ApplicationContext.Instance.ServerConfig.GoogleAddress + "/vt/lyrs=" + _baseURL + "&v=w2.114&hl=" + ApplicationContext.Instance.ServerConfig.MapLanguage + "&gl=" + country + "&" + "x=" + col + "&" + "y=" + row + "&" + "z=" + level + "&s=Galil"; //加载Google遥感图
            }
            if (_baseURL == "t@128")
            {
                url = "http://mt" + (col % 4) + "." + ApplicationContext.Instance.ServerConfig.GoogleAddress + "/vt/lyrs=" + _baseURL + ",r@169000000&v=w2.114&hl=" + ApplicationContext.Instance.ServerConfig.MapLanguage + "&gl=" + country + "&" + "x=" + col + "&" + "y=" + row + "&" + "z=" + level + "&s=Galil";//加载Google地形图
            }
            if (_baseURL == "m@161000000")
            {
                url = "http://mt" + (col % 4) + "." + ApplicationContext.Instance.ServerConfig.GoogleAddress + "/vt/lyrs=" + _baseURL + "&v=w2.114&hl=" + ApplicationContext.Instance.ServerConfig.MapLanguage + "&gl=" + country + "&" + "x=" + col + "&" + "y=" + row + "&" + "z=" + level + "&s=Galil"; //加载Google街道图
            }
            ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "GIS", "GISUrl:" + url);
            return string.Format(url);

            //调用加载初始的Google街道地图
            //string baseUrl = "http://mt2.google.cn/vt/v=w2.116&hl=zh-CN&gl=cn&x={0}&y={1}&z={2}&s=G";
            //return string.Format(baseUrl, col, row, level);
        }
    }
}

