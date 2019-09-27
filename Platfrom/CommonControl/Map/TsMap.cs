using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using System;
using System.Text;
namespace Gsafety.Common.Map
{
    public class TsMap : TiledMapServiceLayer
    {
        public string Url;
        public string Layers;
        public string TileMatrixSet;
        public string MatrixSet;
        public int Wkid = 4326;
        private MapPoint WebOrigin = new MapPoint(-20037508.342787, 20037508.342787, new SpatialReference(3857));
        private MapPoint WorldOrigin = new MapPoint(-180.0, 90.0, new SpatialReference(4326));
        public TsMap()
        {
        }
        public TsMap(string url, string layers, string MartixSet, string tileMatrixSet, int wkid)
        {
            this.Url = url;
            this.Layers = layers;
            this.MatrixSet = MartixSet;
            this.TileMatrixSet = tileMatrixSet;
            this.Wkid = wkid;
        }
        public override void Initialize()
        {
            base.SpatialReference = new SpatialReference(this.Wkid);
            base.TileInfo = new TileInfo
            {
                Width = 256,
                Height = 256,
                SpatialReference = base.SpatialReference,
                Lods = new Lod[21]
            };
            double num = 0.0;
            if (this.Wkid == 3857)
            {
                num = 156543.03392800014;
                base.TileInfo.Origin = this.WebOrigin;
                this.FullExtent = new Envelope(-20037508.342787, -20037508.342787, 20037508.342787, 20037508.342787)
                {
                    SpatialReference = base.SpatialReference
                };
            }
            else
            {
                if (this.Wkid == 4326)
                {
                    num = 0.703125;
                    base.TileInfo.Origin = this.WorldOrigin;
                    this.FullExtent = new Envelope(-180.0, -90.0, 180.0, 90.0)
                    {
                        SpatialReference = base.SpatialReference
                    };
                }
            }
            for (int i = 0; i < 21; i++)
            {
                base.TileInfo.Lods[i] = new Lod
                {
                    Resolution = num
                };
                num /= 2.0;
            }
            base.Initialize();
        }
        public override string GetTileUrl(int level, int row, int col)
        {
            StringBuilder stringBuilder = new StringBuilder(this.Url);
            stringBuilder.Append("?service=WMTS&request=GetTile&version=1.0.0");
            stringBuilder.AppendFormat("&layer={0}", this.Layers);
            stringBuilder.Append("&style=_null");
            stringBuilder.Append("&format=image/png");
            stringBuilder.AppendFormat("&tilematrixset={0}", this.MatrixSet);
            stringBuilder.AppendFormat("&TileMatrix={0}{1}", this.TileMatrixSet, level);
            stringBuilder.AppendFormat("&tileRow={0}", row);
            stringBuilder.AppendFormat("&tileCol={0}", col);
            return stringBuilder.ToString();
        }
    }
}
