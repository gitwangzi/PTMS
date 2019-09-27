using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.Common.Logging;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 29aaedf7-e191-4802-94e6-0c44711af931      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGZG
/////                 Author: TEST(zhangzg)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MonitorAlert
/////    Project Description:    
/////             Class Name: GeometryBuffer
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/8 13:22:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/8 13:22:41
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.MonitorAlert
{
    /// <summary>
    /// 简单点
    /// </summary>
    public class SimplePoint
    {
        public double X
        { get; set; }
        public double Y
        { get; set; }
    }
    /// <summary>
    /// 简单线
    /// </summary>
    public class SimplePolyline
    {
        private List<SimplePoint> _pts;
        public SimplePolyline()
        {
            _pts = new List<SimplePoint>();
        }
        public List<SimplePoint> Points
        {
            get
            {
                return _pts;
            }
        }

        /// <summary>
        /// 判断两个直线是否相交
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool Intersect(SimplePolyline line)
        {
            for (int i = 0; i <= _pts.Count - 1 - 1; i++)
            {
                for (int j = 0; j <= line.Points.Count - 1 - 1; j++)
                {
                    if (Intersect(_pts[i], _pts[i + 1], line.Points[j], line.Points[j + 1])) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断两条直线段是否相交
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="pt3"></param>
        /// <param name="pt4"></param>
        /// <returns></returns>
        public bool Intersect(SimplePoint pt1, SimplePoint pt2, SimplePoint pt3, SimplePoint pt4)
        {
            //不相交即在同侧，有以下两种情况为不相交
            if (DirV3(pt1, pt2, pt3) * DirV3(pt1, pt2, pt4) > 0) return false;
            if (DirV3(pt3, pt4, pt1) * DirV3(pt3, pt4, pt2) > 0) return false;

            return true;
        }

        /// <summary>
        /// 判断点在直线的下或下及左或右
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="pt3"></param>
        /// <returns></returns>
        private double DirV3(SimplePoint pt1, SimplePoint pt2, SimplePoint pt3)
        {
            return pt1.X * pt3.Y + pt2.X * pt1.Y + pt3.X * pt2.Y - pt1.X * pt2.Y - pt2.X * pt3.Y - pt3.X * pt1.Y;
        }
        /// <summary>
        /// 获取增长指定长度后的线段
        /// </summary>
        /// <param name="Dis"></param>
        /// <returns></returns>
        public SimplePolyline GetLineGrow(double Dis)
        {
            if (_pts.Count <= 1) return null;
            double anglen = Math.Atan2(_pts[_pts.Count - 1].Y - _pts[_pts.Count - 2].Y, _pts[_pts.Count - 1].X - _pts[_pts.Count - 2].X);
            SimplePoint ptn = new SimplePoint();
            ptn.X = _pts[_pts.Count - 1].X + Dis * Math.Cos(anglen);
            ptn.Y = _pts[_pts.Count - 1].Y + Dis * Math.Sin(anglen);

            double angle1 = Math.Atan2(_pts[1].Y - _pts[0].Y, _pts[1].X - _pts[0].X);
            SimplePoint pt1 = new SimplePoint();
            pt1.X = _pts[0].X - Dis * Math.Cos(angle1);
            pt1.Y = _pts[0].Y - Dis * Math.Sin(angle1);

            SimplePolyline newline = new SimplePolyline();
            newline._pts.Add(pt1);
            foreach (SimplePoint pt in _pts)
            {
                newline._pts.Add(pt);
            }
            newline._pts.Add(ptn);
            return newline;
        }
        /// <summary>
        /// 获得平行线
        /// </summary>
        /// <param name="Dis"></param>
        /// <returns></returns>
        public SimplePolyline GetPXLine(double Dis)
        {
            if (_pts.Count == 0) return null;
            SimplePolyline newline = new SimplePolyline();
            double angle = Math.Atan2(_pts[1].Y - _pts[0].Y, _pts[1].X - _pts[0].X);
            if (angle < 0) angle = 2 * Math.PI + angle;

            SimplePoint pt = new SimplePoint();
            pt.X = _pts[0].X - Dis * Math.Sin(angle);
            pt.Y = _pts[0].Y + Dis * Math.Cos(angle);
            newline.Points.Add(pt);

            for (int i = 1; i <= _pts.Count - 2; i++)
            {
                pt = new SimplePoint();
                pt = NextPoint(_pts[i - 1], _pts[i], _pts[i + 1], Dis);
                newline.Points.Add(pt);
            }

            angle = Math.Atan2(_pts[_pts.Count - 1].Y - _pts[_pts.Count - 2].Y, _pts[_pts.Count - 1].X - _pts[_pts.Count - 2].X);
            if (angle < 0) angle = 2 * Math.PI + angle;

            pt = new SimplePoint();
            pt.X = _pts[_pts.Count - 1].X - Dis * Math.Sin(angle);
            pt.Y = _pts[_pts.Count - 1].Y + Dis * Math.Cos(angle);
            newline.Points.Add(pt);

            return newline;
        }

        /// <summary>
        /// 得到平行线的下一个点
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="pt3"></param>
        /// <param name="Dis"></param>
        /// <returns></returns>
        private SimplePoint NextPoint(SimplePoint pt1, SimplePoint pt2, SimplePoint pt3, double Dis)
        {
            double Eparam = 0.001;
            double K1 = 0;
            double K2 = 0;
            double angle1 = Math.Atan2(pt2.Y - pt1.Y, pt2.X - pt1.X);
            double angle2 = Math.Atan2(pt3.Y - pt2.Y, pt3.X - pt2.X);
            if (angle1 < 0) angle1 = 2 * Math.PI + angle1;
            if (angle2 < 0) angle2 = 2 * Math.PI + angle2;

            SimplePoint pt = new SimplePoint();
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

                if (Math.Abs(K1 - K2) < Eparam)
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

    }

    /// <summary>
    /// 简单面
    /// </summary>
    public class SimplePolygon
    {
        private List<SimplePoint> _pts;
        public SimplePolygon()
        {
            _pts = new List<SimplePoint>();
        }

        public List<SimplePoint> Points
        {
            get
            {
                return _pts;
            }
        }

        /// <summary>
        /// 判断点是否在面内
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool IsPointIn(SimplePoint pt)
        {
            bool result = false;
            int count = 0;
            double maxx = 0, minx = 0, maxy = 0, miny = 0;
            if (_pts.Count > 0)
            {
                maxx = minx = _pts[0].X;
                maxy = miny = _pts[0].Y;

                foreach (SimplePoint lspt in _pts)
                {
                    if (maxx < lspt.X) maxx = lspt.X;
                    if (minx > lspt.X) minx = lspt.X;
                    if (maxy < lspt.Y) maxy = lspt.Y;
                    if (miny > lspt.Y) miny = lspt.Y;
                }
            }
            if (pt != null)
            {
                //首先判断是否在面的外框范围内
                if (pt.X < minx || pt.X > maxx
                || pt.Y < miny || pt.Y > maxy)
                {
                    return result;
                }
                else
                {
                    int i, j;
                    j = _pts.Count - 1;
                    SimplePoint point1 = new SimplePoint();
                    SimplePoint point2 = new SimplePoint();
                    double tempValue;
                    for (i = 0; i < _pts.Count; i++)
                    {
                        point1.X = _pts[i].X;
                        point1.Y = _pts[i].Y;

                        point2.X = _pts[j].X;
                        point2.Y = _pts[j].Y;

                        if ((point1.X < pt.X && point2.X >= pt.X)
                        || (point2.X < pt.X && point1.X >= pt.X))
                        {
                            tempValue = point1.Y + (pt.X - point1.X) / (point2.X - point1.X) * (point2.Y - point1.Y);
                            if (tempValue < pt.Y)
                            {
                                count++;
                            }
                            else if (tempValue == pt.Y)
                            {
                                count = -1;
                                break;
                            }
                        }
                        j = i;
                    }
                }
            }
            if (count == -1)
            {
                result = true;//点在线段上
            }
            else
            {
                int tempI = count % 2;
                if (tempI == 0)//为偶数
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }
    }

    public enum BufferType
    {
        miBufferTypeRoad = 1,
        miBufferTypeRoute = 2,
        miBufferTypeFence = 3,
    }


    /// <summary>
    /// 网格坐标系定义
    /// </summary>
    public static class GridCellCoord
    {
        /// <summary>
        /// 依据经纬度坐标获取网格上的左下角点
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static string GetCellID(string lon, string lat)
        {
            int indflag = lon.IndexOf("-");
            lon = lon.Replace("-", "");
            int du = 0;
            double fen = 0;
            int ind = lon.IndexOf(".");
            if (ind == -1)
            {
                ind = lon.Length;
            }
            if ((ind - 3 + 1) >= 1)
            {
                if (lon.Substring(0, ind - 3 + 1) != "") du = int.Parse(lon.Substring(0, ind - 3 + 1));
                if ((lon.Substring(ind - 2)) != "") fen = double.Parse(lon.Substring(ind - 2));
            }
            else
            {
                fen = double.Parse(lon);
            }

            int intlon = 0;
            if (indflag > -1)
                intlon = Convert.ToInt32(Math.Floor((-du - fen / 60) * 100));
            else
                intlon = Convert.ToInt32(Math.Floor((du + fen / 60) * 100));


            indflag = lat.IndexOf("-");
            lat = lat.Replace("-", "");

            du = 0;
            fen = 0;

            ind = lat.IndexOf(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            if (ind == -1)
            {
                ind = lat.Length;
            }

            if ((ind - 3 + 1) >= 1)
            {
                if (lat.Substring(0, ind - 3 + 1) != "") du = int.Parse(lat.Substring(0, ind - 3 + 1));
                if ((lat.Substring(ind - 2)) != "") fen = double.Parse(lat.Substring(ind - 2));
            }
            else
            {
                fen = double.Parse(lat);
            }

            int intlat = 0;
            if (indflag > -1) intlat = Convert.ToInt32(Math.Floor((-du - fen / 60) * 100));
            else intlat = Convert.ToInt32(Math.Floor((du + fen / 60) * 100));

            return GetCellID(intlon, intlat);
        }
        /// <summary>
        /// 依据左下角的坐标点获取出网格的编号
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static string GetCellID(int lon, int lat)
        {
            return lon.ToString() + "_" + lat.ToString();
        }
        /// <summary>
        /// 依据网格的编号和偏移值，获取取新的网格编号
        /// </summary>
        /// <param name="CellID"></param>
        /// <param name="offsetx"></param>
        /// <param name="offsety"></param>
        /// <returns></returns>
        public static string GetCellID(string CellID, int offsetx, int offsety)
        {
            string[] temp = CellID.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return GetCellID(int.Parse(temp[0]) + offsetx, int.Parse(temp[1]) + offsety);

        }

        /// <summary>
        /// 获得网格的左下角
        /// </summary>
        /// <param name="CellID"></param>
        /// <returns></returns>
        public static SimplePoint GetPoint(string CellID)
        {
            string[] temp = CellID.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            SimplePoint pt = new SimplePoint();
            pt.X = int.Parse(temp[0]);
            pt.Y = int.Parse(temp[1]);
            return pt;
        }

        /// <summary>
        /// 获得网格的边线
        /// </summary>
        /// <param name="CellID"></param>
        /// <returns></returns>
        public static SimplePolyline GetCellLine(string CellID)
        {
            SimplePolyline line = new SimplePolyline();
            SimplePoint pt = GetPoint(CellID);
            line.Points.Add(pt);

            SimplePoint newpt = new SimplePoint();
            newpt.X = pt.X + 1;
            newpt.Y = pt.Y;
            line.Points.Add(newpt);

            newpt = new SimplePoint();
            newpt.X = pt.X + 1;
            newpt.Y = pt.Y + 1;
            line.Points.Add(newpt);

            newpt = new SimplePoint();
            newpt.X = pt.X;
            newpt.Y = pt.Y + 1;
            line.Points.Add(newpt);

            line.Points.Add(pt);
            return line;
        }

        /// <summary>
        /// 将地址坐标转为定义的网格坐标
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static SimplePoint Geo2GridCoord(Road.Node node)
        {
            double lon = double.Parse(node.longitude) * 100;
            double lat = double.Parse(node.latitude) * 100;

            SimplePoint pt = new SimplePoint();
            pt.X = lon;
            pt.Y = lat;
            return pt;
        }
        /// <summary>
        /// 将GPS坐标转为网格定义的坐标点
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static SimplePoint GPS2GridCoord(string lon, string lat)
        {
            SimplePoint pt = new SimplePoint();
            int indflag = lon.IndexOf("-");
            lon = lon.Replace("-", "");
            int du = 0;
            double fen = 0;
            int ind = lon.IndexOf(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            if (ind == -1)
            {
                ind = lon.Length;
            }
            if ((ind - 3 + 1) >= 1)
            {
                if (lon.Substring(0, ind - 3 + 1) != "") du = int.Parse(lon.Substring(0, ind - 3 + 1));
                if ((lon.Substring(ind - 2)) != "") fen = double.Parse(lon.Substring(ind - 2));
            }
            else
            {
                fen = double.Parse(lon);
            }

            if (indflag > -1)
                pt.X = (-du - fen / 60) * 100;
            else
                pt.X = (du + fen / 60) * 100;


            indflag = lat.IndexOf("-");
            lat = lat.Replace("-", "");

            du = 0;
            fen = 0;

            ind = lat.IndexOf(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            if (ind == -1)
            {
                ind = lat.Length;
            }
            if ((ind - 3 + 1) >= 1)
            {
                if (lat.Substring(0, ind - 3 + 1) != "") du = int.Parse(lat.Substring(0, ind - 3 + 1));
                if ((lat.Substring(ind - 2)) != "") fen = double.Parse(lat.Substring(ind - 2));
            }
            else
            {
                fen = double.Parse(lat);
            }

            if (indflag > -1) pt.Y = (-du - fen / 60) * 100;
            else pt.Y = (du + fen / 60) * 100;

            return pt;
        }
    }
    /// <summary>
    /// 每个对象的缓冲区
    /// </summary>
    /// 
    public class GeometryBuffer
    {
        private SimplePolygon _polygon = null;
        //private const double csBufferWidth = 1.0 * 200 * 100 / (60 * 1852);

        public GeometryBuffer(BufferType btype, List<Gsafety.PTMS.MonitorAlert.Road.Node> LstNode,double csBufferWidth)
        {
            _polygon = new SimplePolygon();
            SimplePolyline line;
            SimplePolyline newline1;
            SimplePolyline newline2;

            switch (btype)
            {
                case BufferType.miBufferTypeRoad:
                    line = new SimplePolyline();
                    foreach (Gsafety.PTMS.MonitorAlert.Road.Node node in LstNode)
                    {
                        SimplePoint pt = GridCellCoord.Geo2GridCoord(node);
                        line.Points.Add(pt);
                    }

                    newline1 = line.GetPXLine(-csBufferWidth);
                    newline2 = line.GetPXLine(csBufferWidth);

                    for (int i = 0; i <= newline1.Points.Count - 1; i++)
                    {
                        SimplePoint pt = new SimplePoint();
                        pt.X = newline1.Points[i].X;
                        pt.Y = newline1.Points[i].Y;

                        _polygon.Points.Add(pt);
                    }

                    for (int i = newline2.Points.Count - 1; i >= 0; i--)
                    {
                        SimplePoint pt = new SimplePoint();
                        pt.X = newline2.Points[i].X;
                        pt.Y = newline2.Points[i].Y;

                        _polygon.Points.Add(pt);
                    }
                    break;
                case BufferType.miBufferTypeRoute:
                    line = new SimplePolyline();
                    foreach (Gsafety.PTMS.MonitorAlert.Road.Node node in LstNode)
                    {
                        SimplePoint pt = GridCellCoord.Geo2GridCoord(node);
                        line.Points.Add(pt);
                    }
                    line = line.GetLineGrow(csBufferWidth);

                    newline1 = line.GetPXLine(-csBufferWidth);
                    newline2 = line.GetPXLine(csBufferWidth);

                    for (int i = 0; i <= newline1.Points.Count - 1; i++)
                    {
                        SimplePoint pt = new SimplePoint();
                        pt.X = newline1.Points[i].X;
                        pt.Y = newline1.Points[i].Y;

                        _polygon.Points.Add(pt);
                    }

                    for (int i = newline2.Points.Count - 1; i >= 0; i--)
                    {
                        SimplePoint pt = new SimplePoint();
                        pt.X = newline2.Points[i].X;
                        pt.Y = newline2.Points[i].Y;

                        _polygon.Points.Add(pt);
                    }
                    break;
                case BufferType.miBufferTypeFence:
                    foreach (Gsafety.PTMS.MonitorAlert.Road.Node node in LstNode)
                    {
                        SimplePoint pt = GridCellCoord.Geo2GridCoord(node);
                        _polygon.Points.Add(pt);
                    }
                    break;
            }
        }

        public bool IsPointIn(string lon, string lat)
        {
            SimplePoint pt = GridCellCoord.GPS2GridCoord(lon, lat);
            return _polygon.IsPointIn(pt);
        }
        public SimplePolygon Polygon
        {
            get
            {
                return _polygon;
            }
        }


        /// <summary>
        /// 计算与polygon相交的网格
        /// </summary>
        /// <returns></returns>
        public Hashtable GetIntersectsCells()
        {
            Hashtable AllGrid = new Hashtable();
            Hashtable CrossRet = new Hashtable();
            try
            {
                if (_polygon.Points.Count == 0) return CrossRet;
                //1求出最小、最大的范围
                int xmin = Convert.ToInt32(Math.Floor(_polygon.Points[0].X));
                int xmax = Convert.ToInt32(Math.Floor(_polygon.Points[0].X));
                int ymin = Convert.ToInt32(Math.Floor(_polygon.Points[0].Y));
                int ymax = Convert.ToInt32(Math.Floor(_polygon.Points[0].Y));

                foreach (SimplePoint pt in _polygon.Points)
                {
                    if (xmin > Math.Floor(pt.X)) xmin = Convert.ToInt32(Math.Floor(pt.X));
                    if (xmax < Math.Floor(pt.X)) xmax = Convert.ToInt32(Math.Floor(pt.X));
                    if (ymin > Math.Floor(pt.Y)) ymin = Convert.ToInt32(Math.Floor(pt.Y));
                    if (ymax < Math.Floor(pt.Y)) ymax = Convert.ToInt32(Math.Floor(pt.Y));
                }
                //2计算出可能与polygon相交的所有的网格，签于下面算法，倒排有助于速度提高。
                //bool caninsert = false;
                for (int row = xmax; row >= xmin; row--)
                    for (int col = ymax; col >= ymin; col--)
                    {
                        AllGrid.Add(GridCellCoord.GetCellID(row, col), null);
                    }
                //3计算一定相交的网格
                //3.1找出polygon落入的网格
                foreach (SimplePoint pt in _polygon.Points)
                {
                    string cellid = Convert.ToInt32(Math.Floor(pt.X)) + "_" + Convert.ToInt32(Math.Floor(pt.Y));
                    AddToList(CrossRet, cellid);
                }
                //3.2找出落入polygon的网格
                foreach (DictionaryEntry de in AllGrid)
                {
                    if (!CrossRet.Contains(de.Key))
                    {
                        SimplePoint pt = GridCellCoord.GetPoint(de.Key.ToString());
                        if (_polygon.IsPointIn(pt))//左下角在面内
                        {
                            AddToList(CrossRet, de.Key.ToString());
                            AddToList(CrossRet, GridCellCoord.GetCellID(de.Key.ToString(), -1, 0));
                            AddToList(CrossRet, GridCellCoord.GetCellID(de.Key.ToString(), 0, -1));
                            AddToList(CrossRet, GridCellCoord.GetCellID(de.Key.ToString(), -1, -1));
                        }
                    }
                }

                //3.3找出相交的网格
                SimplePolyline line = new SimplePolyline();
                line.Points.AddRange(_polygon.Points);
                foreach (DictionaryEntry de in AllGrid)
                {
                    if (!CrossRet.Contains(de.Key))
                    {
                        if (line.Intersect(GridCellCoord.GetCellLine(de.Key.ToString())))
                        {
                            AddToList(CrossRet, de.Key.ToString());
                        }
                    }
                }

                /////4去除掉没有没有标识为true的网格
                //ArrayList akeys = new ArrayList(ret.Keys);
                //ArrayList avalues=new ArrayList(ret.Values);                
                ////foreach (DictionaryEntry de in ret)
                //for(int p=akeys.Count-1;p>-1;p--)
                //{
                //    DictionaryEntry de = new DictionaryEntry();
                //    de.Value = avalues[p];
                //    de.Key = akeys[p];
                //    if ((bool)(de.Value) == false)
                //    {
                //       // ret.Remove(de.Key);
                //    }
                //}
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("GetIntersectsCells :" + ex.Message);
            }
            finally
            {
                AllGrid.Clear();
            }
            return CrossRet;
        }

        private void AddToList(Hashtable ht, string key)
        {
            if (!ht.Contains(key)) ht.Add(key, null);
            //if (ht.ContainsKey(key)) ht[key] = value;
        }
    }

    /// <summary>
    /// 道路信息
    /// </summary>
    public class Road
    {

        private string _Id;
        private string _Name;
        private int _LimitedSpeed;
        private GeometryBuffer _Buffer;
        private List<Node> _RoadNode;

        public class Node
        {
            public string longitude;
            public string latitude;
        }

        public Road(string csID, string csName, int csLimitedSpeed, List<Node> NodeLst,double csBufferWidth)
        {
            _Id = csID;
            _Name = csName;
            _LimitedSpeed = csLimitedSpeed;
            _RoadNode = NodeLst;

            _Buffer = new GeometryBuffer(BufferType.miBufferTypeRoad, NodeLst, csBufferWidth);
        }


        public List<Node> RoadNode
        {
            get
            {
                return _RoadNode;
            }
        }

        public string Id
        {
            get
            {
                return _Id;
            }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
        }
        /// <summary>
        /// 限速
        /// </summary>
        public double LimitedSpeed
        {
            get
            {
                return _LimitedSpeed;
            }
        }
        /// <summary>
        /// 缓冲区
        /// </summary>
        public GeometryBuffer Buffer
        {
            get
            {
                return _Buffer;
            }
        }

    }

    /// <summary>
    /// 路线信息
    /// </summary>
    public class Route
    {
        private string _Id;
        private string _Name;
        private List<Gsafety.PTMS.MonitorAlert.Road.Node> _NodeLst;
        private GeometryBuffer _Buffer;

        public Route(string csID, string csName, List<Gsafety.PTMS.MonitorAlert.Road.Node> NodeLst,double csBufferWidth)
        {
            _Id = csID;
            _Name = csName;
            _NodeLst = NodeLst;
            _Buffer = new GeometryBuffer(BufferType.miBufferTypeRoute, NodeLst, csBufferWidth);
        }

        public string Id
        {
            get
            {
                return _Id;
            }
        }
        public string Name
        {
            get
            {
                return _Name;
            }
        }
        /// <summary>
        /// 缓冲区
        /// </summary>
        public GeometryBuffer Buffer
        {
            get
            {
                return _Buffer;
            }
        }
    }

    /// <summary>
    /// 围栏信息
    /// </summary>
    public class Fence
    {
        private string _Id;
        private string _Name;
        private GeometryBuffer _Buffer;
        private Hashtable _EnabledList=new Hashtable();
        private List<Gsafety.PTMS.MonitorAlert.Road.Node> _NodeLst;

        public Fence(string csID, string csName, string LimitedSpeed, int csFenceType, int action, string TimeLimit, List<Gsafety.PTMS.MonitorAlert.Road.Node> NodeLst, double csBufferWidth)
        {
            _Id = csID;
            _Name = csName;
            _NodeLst = NodeLst;
            OverSpeed = 0;
            UnderSpeed = 0;
            Action = 1;
            if (csFenceType == -1)
            {
                Action = action;
            }
            else if (csFenceType==1)//入围栏
            {
                Action += 8;
            }
            else if (csFenceType == 2)//出围栏
            {
                Action += 16;
            }
            else if (csFenceType == 3)//出入围栏
            {
                Action += 8 + 16;
            }
            if ((LimitedSpeed != null) && (LimitedSpeed != "") && (LimitedSpeed != "0"))
            {
                string[] temp = LimitedSpeed.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                UnderSpeed = int.Parse(temp[0]);
                OverSpeed = int.Parse(temp[1]);
                if (csFenceType > -1) //按fencetype计算
                {
                    if (UnderSpeed > 0) Action += 4;
                    if (OverSpeed > 0) Action += 2; 
                }
            }

            StartTime = "00:00:00";
            EndTime = "24:00:00";
            if ((TimeLimit != null) && (TimeLimit != ""))
            {
                string[] temp=TimeLimit.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                StartTime="00:00".Substring(0,5-temp[0].Length)+temp[0] + ":00";
                EndTime = "00:00".Substring(0, 5 - temp[1].Length) + temp[1] + ":00";

                if (csFenceType > -1)//按fencetype计算
                {
                    Action += 32;
                }
            }
            _Buffer = new GeometryBuffer(BufferType.miBufferTypeFence, NodeLst, csBufferWidth);

            GetEnableList(Action);
        }

        private void GetEnableList(int csAction)
        {
            if (csAction > Math.Pow(2, 5))
            {
                _EnabledList.Add("BIT5", true);
                csAction = csAction - (int)Math.Pow(2, 5);
            }
            else
            {
                _EnabledList.Add("BIT5", false);
                csAction = csAction - (int)Math.Pow(2, 5);
            }

            if (csAction > Math.Pow(2, 4))
            {
                _EnabledList.Add("BIT4", true);
                csAction = csAction - (int)Math.Pow(2, 4);
            }
            else
            {
                _EnabledList.Add("BIT4", false);
                csAction = csAction - (int)Math.Pow(2, 4);
            }

            if (csAction > Math.Pow(2, 3))
            {
                _EnabledList.Add("BIT3", true);
                csAction = csAction - (int)Math.Pow(2, 3);
            }
            else
            {
                _EnabledList.Add("BIT3", false);
                csAction = csAction - (int)Math.Pow(2, 3);
            }

            if (csAction > Math.Pow(2, 2))
            {
                _EnabledList.Add("BIT2", true);
                csAction = csAction - (int)Math.Pow(2, 2);
            }
            else
            {
                _EnabledList.Add("BIT2", false);
                csAction = csAction - (int)Math.Pow(2, 2);
            }

            if (csAction > Math.Pow(2, 1))
            {
                _EnabledList.Add("BIT1", true);
                csAction = csAction - (int)Math.Pow(2, 1);
            }
            else
            {
                _EnabledList.Add("BIT1", false);
                csAction = csAction - (int)Math.Pow(2, 1);
            }

            if (csAction > Math.Pow(2, 0))
            {
                _EnabledList.Add("BIT0", true);
                csAction = csAction - (int)Math.Pow(2, 0);
            }
            else
            {
                _EnabledList.Add("BIT0", false);
                csAction = csAction - (int)Math.Pow(2, 0);
            }

        }

        public string Id
        {
            get
            { return _Id; }
        }
        public string Name
        {
            get
            {
                return _Name;
            }
        }
        /// <summary>
        /// 围栏的缓冲区
        /// </summary>
        public GeometryBuffer Buffer
        {
            get
            {
                return _Buffer;
            }
        }

        public int OverSpeed
        {
            get;
            set;
        }
        public int UnderSpeed
        {
            get;
            set;
        }
        public string  StartTime
        {
            get;
            set;
        }
        public string EndTime
        {
            get;
            set;
        }
        public int Action
        {
            get;
            set;
        }

        public bool Alert(string key)
        {
            DateTime dt=DateTime.Now;
            DateTime StartDT = DateTime.Parse(dt.Date.ToShortDateString() + " " + StartTime);
            DateTime EndDT = DateTime.Parse(dt.Date.ToShortDateString() + " " + EndTime);
            //string temp="00:00:00".Substring(0,8-dt.ToLongTimeString().Length)+dt.ToLongTimeString();
            if ((_EnabledList.Contains(key)) && ((dt >= StartDT) && ( dt <= EndDT)))
            {
                return (bool)_EnabledList[key];
            }
            return false;
        }

    }


}
