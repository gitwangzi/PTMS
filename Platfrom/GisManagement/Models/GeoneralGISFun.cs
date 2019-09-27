/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 33a076b0-3934-44b8-a8c7-874a6ee922d6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGZG
/////                 Author: TEST(zhangzg)
/////======================================================================
/////           Project Name: GisManagement.Models
/////    Project Description:    
/////             Class Name: GeoneralGISFun
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/24 9:51:24
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/24 9:51:24
/////            Modified by:
/////   Modified Description: 
/////======================================================================
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
using ESRI.ArcGIS.Client.Geometry;

namespace GisManagement.Models
{
    public static class GeoneralGISFun
    {
        /// <summary>
        /// Get the line buffer
        /// </summary>
        /// <param name="line"></param>
        /// <param name="Dis"></param>
        /// <returns></returns>
        public static ESRI.ArcGIS.Client.Geometry.Geometry GetLineBuffer(ESRI.ArcGIS.Client.Geometry.Polyline line, double Dis)
        {
            ESRI.ArcGIS.Client.Geometry.PointCollection pts = line.Paths[0] as ESRI.ArcGIS.Client.Geometry.PointCollection;
            ESRI.ArcGIS.Client.Geometry.PointCollection pts1 = GetPXLine(pts, Dis);
            ESRI.ArcGIS.Client.Geometry.PointCollection pts2 = GetPXLine(pts, -Dis);

            ESRI.ArcGIS.Client.Geometry.PointCollection newpts = new ESRI.ArcGIS.Client.Geometry.PointCollection();
            for (int i = 0; i <= pts1.Count - 1; i++)
            {
                MapPoint pt = new MapPoint();
                pt.X = pts1[i].X;
                pt.Y = pts1[i].Y;

                newpts.Add(pt);
            }

            for (int i = pts2.Count - 1; i >= 0; i--)
            {
                MapPoint pt = new MapPoint();
                pt.X = pts2[i].X;
                pt.Y = pts2[i].Y;

                newpts.Add(pt);
            }
            ESRI.ArcGIS.Client.Geometry.Polygon polygon = new ESRI.ArcGIS.Client.Geometry.Polygon();
            polygon.Rings.Add(newpts);

            return polygon as ESRI.ArcGIS.Client.Geometry.Geometry;
        }

        /// <summary>
        /// Get parallel lines
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="Dis"></param>
        /// <returns></returns>
        private static ESRI.ArcGIS.Client.Geometry.PointCollection GetPXLine(ESRI.ArcGIS.Client.Geometry.PointCollection pts, double Dis)
        {

            ESRI.ArcGIS.Client.Geometry.PointCollection newpts = new ESRI.ArcGIS.Client.Geometry.PointCollection();

            double angle = Math.Atan2(pts[1].Y - pts[0].Y, pts[1].X - pts[0].X);
            if (angle < 0) angle = 2 * Math.PI + angle;

            ESRI.ArcGIS.Client.Geometry.MapPoint pt = new MapPoint();
            pt.X = pts[0].X - Dis * Math.Sin(angle);
            pt.Y = pts[0].Y + Dis * Math.Cos(angle);
            newpts.Add(pt);

            for (int i = 1; i <= pts.Count - 2; i++)
            {
                pt = new MapPoint();
                pt = NextPoint(pts[i - 1], pts[i], pts[i + 1], Dis);
                newpts.Add(pt);
            }

            angle = Math.Atan2(pts[pts.Count - 1].Y - pts[pts.Count - 2].Y, pts[pts.Count - 1].X - pts[pts.Count - 2].X);
            if (angle < 0) angle = 2 * Math.PI + angle;

            pt = new MapPoint();
            pt.X = pts[pts.Count - 1].X - Dis * Math.Sin(angle);
            pt.Y = pts[pts.Count - 1].Y + Dis * Math.Cos(angle);
            newpts.Add(pt);

            return newpts;
        }

        /// <summary>
        /// The next point to get parallel lines
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="pt3"></param>
        /// <param name="Dis"></param>
        /// <returns></returns>
        private static MapPoint NextPoint(MapPoint pt1, MapPoint pt2, MapPoint pt3, double Dis)
        {
            double Eparam = 0.00001;
            double K1 = 0;
            double K2 = 0;
            double angle1 = Math.Atan2(pt2.Y - pt1.Y, pt2.X - pt1.X);
            double angle2 = Math.Atan2(pt3.Y - pt2.Y, pt3.X - pt2.X);
            if (angle1 < 0) angle1 = 2 * Math.PI + angle1;
            if (angle2 < 0) angle2 = 2 * Math.PI + angle2;

            MapPoint pt = new MapPoint();
            double y1 = pt1.Y + Dis * Math.Cos(angle1);
            double x1 = pt1.X - Dis * Math.Sin(angle1);
            double x2 = pt3.X - Dis * Math.Sin(angle2);
            double y2 = pt3.Y + Dis * Math.Cos(angle2);

            if ((-Eparam < pt3.X - pt2.X) && (pt3.X - pt2.X < Eparam) && ((-Eparam < pt2.X - pt1.X) && (pt2.X - pt1.X < Eparam)))
            {
                pt.X = x1;
                pt.Y = pt2.Y;
            }
            else if ((-Eparam < pt3.X - pt2.X) && (pt3.X - pt2.X < Eparam))
            {
                K1 = (pt2.Y - pt1.Y) / (pt2.X - pt1.X);
                pt.X = x2;
                pt.Y = K1 * x2 + y1 - K1 * x1;
            }
            else if ((-Eparam < pt2.X - pt1.X) && (pt2.X - pt1.X < Eparam))
            {
                K2 = (pt3.Y - pt2.Y) / (pt3.X - pt2.X);
                pt.X = x1;
                pt.Y = K2 * x1 + y2 - K2 * x2;
            }
            else
            {
                K1 = (pt2.Y - pt1.Y) / (pt2.X - pt1.X);
                K2 = (pt3.Y - pt2.Y) / (pt3.X - pt2.X);

                if (K1 == K2)
                {
                    pt.X = pt2.X - Dis * Math.Sin(angle1);
                    pt.Y = pt2.Y + Dis * Math.Cos(angle1);
                }
                else
                {
                    pt.X = (K1 * x1 - K2 * x2 + y2 - y1) / (K1 - K2);
                    pt.Y = (K1 * K2 * (x1 - x2) + K1 * y2 - K2 * y1) / (K1 - K2);
                }
            }
            return pt;
        }

        public static bool IsCrossSelf(ESRI.ArcGIS.Client.Geometry.Geometry geo)
        {
            if (geo == null) return false;
            ESRI.ArcGIS.Client.Geometry.PointCollection pts= (geo as ESRI.ArcGIS.Client.Geometry.Polygon).Rings[0];
            if (pts.Count<=3) return false;

            for (int i = 0; i <= pts.Count -1-1; i++)
            {
                for (int j=0;j<=pts.Count-1-1;j++)
                {
                    if (Math.Abs(i - j) > 1) 
                    {
                        if (((i == 0) && (j == pts.Count - 2)) || ((j == 0) && (i == pts.Count - 2))) continue;
                        if (GetIntersection(pts[i], pts[i + 1], pts[j], pts[j + 1])) return true;
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// Determine whether the intersection of two lines
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private static bool GetIntersection(MapPoint a, MapPoint b, MapPoint c, MapPoint d)
        {
            //Determine abnormalities
            if (Math.Abs(b.X - a.Y) + Math.Abs(b.X - a.X) + Math.Abs(d.Y - c.Y) + Math.Abs(d.X - c.X) == 0)
            {
                return false;
            }

            //a, b is a point
            if (Math.Abs(b.Y - a.Y) + Math.Abs(b.X - a.X) == 0)
            {
                return false;
            }
            //c、d is a point
            if (Math.Abs(d.Y - c.Y) + Math.Abs(d.X - c.X) == 0)
            {
                return false;
            }

            //Parallel
            if ((b.Y - a.Y) * (c.X - d.X) - (b.X - a.X) * (c.Y - d.Y) == 0)
            {
                return false;
            }

            MapPoint intersection = new MapPoint();
            intersection.X = ((b.X - a.X) * (c.X - d.X) * (c.Y - a.Y) - c.X * (b.X - a.X) * (c.Y - d.Y) + a.X * (b.Y - a.Y) * (c.X - d.X)) / ((b.Y - a.Y) * (c.X - d.X) - (b.X - a.X) * (c.Y - d.Y));
            intersection.Y = ((b.Y - a.Y) * (c.Y - d.Y) * (c.X - a.X) - c.Y * (b.Y - a.Y) * (c.X - d.X) + a.Y * (b.X - a.X) * (c.Y - d.Y)) / ((b.X - a.X) * (c.Y - d.Y) - (b.Y - a.Y) * (c.X - d.X));


            if ((intersection.X - a.X) * (intersection.X - b.X) <= 0 && (intersection.X - c.X) * (intersection.X - d.X) <= 0 && (intersection.Y - a.Y) * (intersection.Y - b.Y) <= 0 && (intersection.Y - c.Y) * (intersection.Y - d.Y) <= 0)
            {//Intersect
                return true;
            }
            else
            {//intersect But does not  the line segment
                return false;
            }
        }
        /// <summary>
        /// Determine whether straight face
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static bool PolygonIsLine(ESRI.ArcGIS.Client.Geometry.Polygon polygon)
        {
            if (polygon == null || polygon.Rings.Count == 0)
                return false;
            ESRI.ArcGIS.Client.Geometry.PointCollection ptCollect = polygon.Rings[0];
            if (ptCollect == null)
                return false;
            if (ptCollect.Count <= 2)
                return true;
            double dtanAngel = Math.Atan2(ptCollect[1].Y - ptCollect[0].Y, ptCollect[1].X - ptCollect[0].X);
            int i = 2;
            while (i < ptCollect.Count)
            {
                if ((ptCollect[i].Y - ptCollect[0].Y) != 0 && (ptCollect[i].X - ptCollect[0].X) != 0)
                {
                    double dtanAngelTemp = Math.Atan2(ptCollect[i].Y - ptCollect[0].Y, ptCollect[i].X - ptCollect[0].X);

                    if (dtanAngel != dtanAngelTemp)
                        return false;
                }
                i++;
            }
            return true;
        }
    }
}
