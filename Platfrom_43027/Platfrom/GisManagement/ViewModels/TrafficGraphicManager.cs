/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d0fd3ea1-230a-4598-b191-2bf38d7532e2      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: GisManagement.ViewModels
/////    Project Description:    
/////             Class Name: TrafficGraphicManager
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/23 16:47:13
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/23 16:47:13
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections;
using ESRI.ArcGIS.Client;
using Jounce.Core.Model;
using Jounce.Core.ViewModel;
using Gsafety.PTMS.Share;
using ESRI.ArcGIS.Client.Geometry;

namespace GisManagement.ViewModels
{
    /// <summary>
    /// graphic type enum
    /// </summary>
    public enum TrafficeGraphictype
    {
        /// <summary>
        /// fence
        /// </summary>
        fence = 1,
        /// <summary>
        /// fenceinfo -textsymbol
        /// </summary>
        FenceTextInfo = 2,
        /// <summary>
        /// control point
        /// </summary>
        ControlPoint = 3,
        /// <summary>
        /// control point info
        /// </summary>
        ControlPointTextInfo = 4,
        /// <summary>
        /// bus route,long route
        /// </summary>
        Route = 5,
        /// <summary>
        /// route info
        /// </summary>
        RouteTextInfo = 6,
        /// <summary>
        /// stop
        /// </summary>
        BusStop = 7,
        /// <summary>
        /// stop info
        /// </summary>
        StopTextInfo = 8,
        /// <summary>
        /// Travel plan monitoring point, when plotted using a single monitoring point
        /// </summary>
        ScheDulePoint = 9,
        /// <summary>
        /// Travel plan monitoring point text message, when plotted using a single monitoring point
        /// </summary>
        ScheDulePointTextInfo = 10,
        /// <summary>
        /// Travel plan
        /// </summary>
        ScheDule = 11,
        /// <summary>
        /// Use of the monitoring point travel plan with six distinction is plotted with the plan
        /// </summary>
        ScheDulePointEx = 12,
        /// <summary>
        /// Travel plans textinformation
        /// </summary>
        ScheDuleTextInfo = 13

    }
    /// <summary>
    /// type-pid-cid
    /// </summary>
    public class Type_OIDPair
    {
        public TrafficeGraphictype graphicType;
        public string ParentID;
        public string ChildID;
    }

    /// <summary>
    /// Traffic Management graphic layer management classes help
    /// </summary>
    public class TrafficGraphicHelpVm : BaseViewModel
    {
        /// <summary>
        /// Fence graphic collection
        /// </summary>
        private ObservableCollection<Graphic> _Graphics = new ObservableCollection<Graphic>();
        public ObservableCollection<Graphic> GraphicList
        {
            get { return _Graphics; }
            set
            {
                _Graphics = value;
                RaisePropertyChanged("Graphics");
            }
        }
        /// <summary>
        /// Find parent ID number for the specified ID collection
        /// </summary>
        /// <param name="graphicOID"></param>
        /// <param name="graphicType"></param>
        /// <returns></returns>
        private List<int> FindFromGraphicIDListByParentID(string parentID, TrafficeGraphictype graphicType)
        {
            List<int> listResult = new List<int>();
            if (_graphicIDList != null)
            {
                for (int i = 0; i < _graphicIDList.Count; i++)
                {
                    if (_graphicIDList[i].ParentID == parentID && _graphicIDList[i].graphicType == graphicType)
                    {
                        listResult.Add(i);
                    }
                }
            }
            return listResult;
        }
        /// <summary>
        /// According to a collection of back child ID ID
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="graphicType"></param>
        /// <returns></returns>
        private List<int> FindFromGraphicIDListByChildID(string parentID, TrafficeGraphictype graphicType)
        {
            List<int> listResult = new List<int>();
            if (_graphicIDList != null)
            {
                for (int i = 0; i < _graphicIDList.Count; i++)
                {
                    if (_graphicIDList[i].ParentID == parentID && _graphicIDList[i].graphicType == graphicType)
                    {
                        listResult.Add(i);
                    }
                }
            }
            return listResult;
        }
        /// <summary>
        /// Find a unique value
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="strchildID"></param>
        /// <param name="graphicType"></param>
        /// <returns></returns>
        private int FindFromGraphicIDByParentidAndChildID(string parentID, string strchildID, TrafficeGraphictype graphicType)
        {
            if (_graphicIDList != null)
            {
                for (int i = 0; i < _graphicIDList.Count; i++)
                {
                    if (_graphicIDList[i].ParentID == parentID && _graphicIDList[i].ChildID == strchildID && _graphicIDList[i].graphicType == graphicType)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// OID fence collection
        /// </summary>
        private ObservableCollection<Type_OIDPair> _graphicIDList = new ObservableCollection<Type_OIDPair>();
        /// <summary>
        /// Added to the graphic container
        /// </summary>
        /// <param name="graphic"></param>
        /// <param name="fenceOID"></param>
        public void AddGraphic(Graphic graphic, string strParentID, string ChildID, TrafficeGraphictype graphicType)
        {
            int ind = FindFromGraphicIDByParentidAndChildID(strParentID, ChildID, graphicType);
            if (ind > -1)
            {
                GraphicList[ind] = graphic;
            }
            else
            {
                Type_OIDPair keyvaluePair = new Type_OIDPair();
                keyvaluePair.ParentID = strParentID;
                keyvaluePair.ChildID = ChildID;
                keyvaluePair.graphicType = graphicType;
                _graphicIDList.Add(keyvaluePair);
                GraphicList.Add(graphic);
            }
        }
        /// <summary>
        /// Remove the graphic based on parent ID
        /// </summary>
        /// <param name="fenceOID"></param>
        public void RemoveGraphicByParentID(string parentID, TrafficeGraphictype graphicType)
        {
            List<int> listResult = FindFromGraphicIDListByParentID(parentID, graphicType);
            if (listResult != null && listResult.Count > 0)
            {
                listResult.Sort();
                listResult.Reverse();
                for (int i = 0; i < listResult.Count; i++)
                {
                    _graphicIDList.RemoveAt(listResult[i]);
                    GraphicList.RemoveAt(listResult[i]);
                }
            }
        }
        /// <summary>
        /// According to delete the child ID
        /// </summary>
        /// <param name="graphicOID"></param>
        /// <param name="graphicType"></param>
        public void RemoveGraphicByChildID(string childID, TrafficeGraphictype graphicType)
        {
            List<int> listResult = FindFromGraphicIDListByChildID(childID, graphicType);
            if (listResult != null && listResult.Count > 0)
            {
                listResult.Sort();
                listResult.Reverse();
                for (int i = 0; i < listResult.Count; i++)
                {
                    _graphicIDList.RemoveAt(listResult[i]);
                    GraphicList.RemoveAt(listResult[i]);
                }
            }
        }
        /// <summary>
        /// Obtain a unique value based on the child-parent ID
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="childID"></param>
        /// <param name="graphicType"></param>
        public void RemoveGraphicByParentIDAndChildID(string parentID, string childID, TrafficeGraphictype graphicType)
        {
            int i = FindFromGraphicIDByParentidAndChildID(parentID, childID, graphicType);
            if (i >= 0)
            {
                _graphicIDList.RemoveAt(i);
                GraphicList.RemoveAt(i);
            }
        }
        /// <summary>
        /// By Type clear all graphic
        /// </summary>
        /// <param name="graphicType"></param>
        public void RemoveGraphic(TrafficeGraphictype graphicType)
        {
            for (int i = _graphicIDList.Count - 1; i >= 0; i--)
            {
                if (_graphicIDList[i].graphicType == graphicType)
                {
                    _graphicIDList.RemoveAt(i);
                    GraphicList.RemoveAt(i);
                }
            }
        }
        /// <summary>
        /// clear all graphic
        /// </summary>
        public void Clear()
        {
            _graphicIDList.Clear();
            GraphicList.Clear();
        }


        public Graphic GetGraphics(string ParentID, string ChildID, TrafficeGraphictype graphicType)
        {
            int i = FindFromGraphicIDByParentidAndChildID(ParentID, ChildID, graphicType);
            if (i >= 0)
            {
                return GraphicList[i];
            }
            return null;
        }


        internal Envelope GetExtent()
        {
            if (GraphicList.Count == 0)
            {
                return null;
            }

            Envelope extent = GraphicList[0].Geometry.Extent;

            for (int j = 1; j < GraphicList.Count - 1; j++)
            {
                extent = extent.Union(GraphicList[j].Geometry.Extent);
            }

            return extent;
        }
    }

    public class CarGraphic
    {
        public string CarNum { get; set; }
        public Graphic TrafficGraphic { get; set; }
        public string TrafficID { get; set; }
        public string ChildTrafficID { get; set; }
    }

    /// <summary>
    /// type-oid,
    /// </summary>
    public class Type_OIDPairEx
    {
        public TrafficeGraphictype graphicType;
        public string CarNum;
        public string TrafficID;
        public string ChildTrafficID;
    }
    /// <summary>
    /// Traffic Management graphic layer management classes help
    /// </summary>
    public class MonitorTrafficGraphicHelpVm : BaseViewModel
    {
        /// <summary>
        ///Fence graphic collection
        /// </summary>
        private ObservableCollection<Graphic> _Graphics = new ObservableCollection<Graphic>();
        public ObservableCollection<Graphic> GraphicList
        {
            get { return _Graphics; }
            set
            {
                _Graphics = value;
                RaisePropertyChanged("Graphics");
            }
        }
        /// <summary>
        /// According to traffic rules ID
        /// </summary>
        /// <param name="graphicOID"></param>
        /// <param name="graphicType"></param>
        /// <returns></returns>
        private List<int> FindFromGraphicIDListByTrfficID(string strTrfficID, TrafficeGraphictype graphicType)
        {
            List<int> listResult = new List<int>();
            if (_graphicIDList != null)
            {
                for (int i = 0; i < _graphicIDList.Count; i++)
                {
                    if (_graphicIDList[i].TrafficID == strTrfficID && _graphicIDList[i].graphicType == graphicType)
                    {
                        listResult.Add(i);
                    }
                }
            }
            return listResult;
        }
        /// <summary>
        /// According to the license plate number of queries
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="graphicType"></param>
        /// <returns></returns>
        private List<int> FindFromGraphicIDListByCarNumID(string strCarNum, TrafficeGraphictype graphicType)
        {
            List<int> listResult = new List<int>();
            if (_graphicIDList != null)
            {
                for (int i = 0; i < _graphicIDList.Count; i++)
                {
                    if (_graphicIDList[i].CarNum == strCarNum && _graphicIDList[i].graphicType == graphicType)
                    {
                        listResult.Add(i);
                    }
                }
            }
            return listResult;
        }
        /// <summary>
        /// Find a unique value
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="strchildID"></param>
        /// <param name="graphicType"></param>
        /// <returns></returns>
        private int FindFromGraphicIDByParentidAndChildID(string strCarNum, string parentID, string strchildID, TrafficeGraphictype graphicType)
        {
            if (_graphicIDList != null)
            {
                for (int i = 0; i < _graphicIDList.Count; i++)
                {
                    if (_graphicIDList[i].CarNum == strCarNum && _graphicIDList[i].TrafficID == parentID && _graphicIDList[i].ChildTrafficID == strchildID && _graphicIDList[i].graphicType == graphicType)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// OID  collection
        /// </summary>
        private ObservableCollection<Type_OIDPairEx> _graphicIDList = new ObservableCollection<Type_OIDPairEx>();
        /// <summary>
        /// Added to the graphic container
        /// </summary>
        /// <param name="graphic"></param>
        /// <param name="fenceOID"></param>
        public void AddGraphic(Graphic graphic, string strCarNum, string strfficID, string ChildID, TrafficeGraphictype graphicType)
        {
            int ind = FindFromGraphicIDByParentidAndChildID(strCarNum, strfficID, ChildID, graphicType);
            if (ind > -1)
            {
                GraphicList[ind] = graphic;
            }
            else
            {
                Type_OIDPairEx keyvaluePair = new Type_OIDPairEx();
                keyvaluePair.CarNum = strCarNum;
                keyvaluePair.TrafficID = strfficID;
                keyvaluePair.ChildTrafficID = ChildID;
                keyvaluePair.graphicType = graphicType;
                _graphicIDList.Add(keyvaluePair);
                GraphicList.Add(graphic);
            }
        }
        /// <summary>
        /// Remove the graphic (CarNum) based on parent ID
        /// </summary>
        /// <param name="fenceOID"></param>
        public void RemoveGraphicByParentID(string parentID, TrafficeGraphictype graphicType)
        {
            List<int> listResult = FindFromGraphicIDListByCarNumID(parentID, graphicType);
            if (listResult != null && listResult.Count > 0)
            {
                listResult.Sort();
                listResult.Reverse();
                for (int i = 0; i < listResult.Count; i++)
                {
                    _graphicIDList.RemoveAt(listResult[i]);
                    GraphicList.RemoveAt(listResult[i]);
                }
            }
        }
        /// <summary>
        /// Delete (traffic rules ID) based on the sub ID
        /// </summary>
        /// <param name="graphicOID"></param>
        /// <param name="graphicType"></param>
        public void RemoveGraphicByChildID(string childID, TrafficeGraphictype graphicType)
        {
            try
            {
                List<int> listResult = FindFromGraphicIDListByTrfficID(childID, graphicType);
                if (listResult != null && listResult.Count > 0)
                {
                    listResult.Sort();
                    listResult.Reverse();
                    for (int i = 0; i < listResult.Count; i++)
                    {
                        _graphicIDList.RemoveAt(listResult[i]);
                        GraphicList.RemoveAt(listResult[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        /// <summary>
        /// Obtain a unique value based on the child-parent ID
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="childID"></param>
        /// <param name="graphicType"></param>
        public void RemoveGraphicByParentIDAndChildID(string strCarNum, string trafficID, string childID, TrafficeGraphictype graphicType)
        {
            int i = FindFromGraphicIDByParentidAndChildID(strCarNum, trafficID, childID, graphicType);
            if (i >= 0)
            {
                _graphicIDList.RemoveAt(i);
                GraphicList.RemoveAt(i);
            }
        }
        /// <summary>
        /// By Type clear all graphic
        /// </summary>
        /// <param name="graphicType"></param>
        public void RemoveGraphic(TrafficeGraphictype graphicType)
        {
            try
            {
                for (int i = _graphicIDList.Count - 1; i >= 0; i--)
                {
                    if (_graphicIDList[i].graphicType == graphicType)
                    {
                        _graphicIDList.RemoveAt(i);
                        GraphicList.RemoveAt(i);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        /// <summary>
        /// clear all graphic
        /// </summary>
        public void Clear()
        {
            _graphicIDList.Clear();
            GraphicList.Clear();
        }
        /// <summary>
        /// According to the license plate number and traffic management
        /// </summary>
        /// <param name="strCarNum"></param>
        /// <param name="strTrafficID"></param>
        /// <param name="graphicType"></param>
        /// <returns></returns>
        public Graphic GetGraphicByParentidAndChildID(string strCarNum, string strTrafficID, string childID, TrafficeGraphictype graphicType)
        {
            if (_graphicIDList != null)
            {
                for (int i = 0; i < _graphicIDList.Count; i++)
                {
                    if (_graphicIDList[i].CarNum == strCarNum && _graphicIDList[i].TrafficID == strTrafficID && _graphicIDList[i].ChildTrafficID == childID && _graphicIDList[i].graphicType == graphicType)
                    {
                        if (i >= 0)
                        {
                            return GraphicList[i];
                        }
                    }
                }
            }
            return null;
        }

        public List<CarGraphic> GetGraphicByChildID(string strTrafficID, TrafficeGraphictype graphicType)
        {
            try
            {
                List<CarGraphic> listResult = new List<CarGraphic>();
                if (_graphicIDList != null)
                {
                    for (int i = 0; i < _graphicIDList.Count; i++)
                    {
                        if (_graphicIDList[i].TrafficID == strTrafficID && _graphicIDList[i].graphicType == graphicType)
                        {
                            if (i >= 0)
                            {
                                CarGraphic car = new CarGraphic();
                                car.CarNum = _graphicIDList[i].CarNum;
                                car.TrafficGraphic = GraphicList[i];
                                car.ChildTrafficID = _graphicIDList[i].ChildTrafficID;
                                car.TrafficID = _graphicIDList[i].TrafficID;
                                listResult.Add(car);
                            }
                        }
                    }
                }
                return listResult;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
                return null;
            }
        }
    }
}
