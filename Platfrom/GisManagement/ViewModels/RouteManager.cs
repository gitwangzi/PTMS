/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 02a4b2d4-298b-49d1-b660-db844c86e9ca      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: GisManagement.ViewModels
/////    Project Description:    
/////             Class Name: RouteManager
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/5 15:05:54
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/5 15:05:54
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Collections;
using ESRI.ArcGIS.Client;
using GisManagement.Models;
using GisManagement.Views;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using Gsafety.Common.CommMessage;
using Jounce.Core.Event;
using Jounce.Core.Model;
using Jounce.Core.ViewModel;


namespace GisManagement.ViewModels
{
    #region Map data binding
    /// <summary>
    /// In order to meet the global display a collection of several ArcGIS shows the definition is to bind
    /// </summary>
    public class RouteManager : BaseNotify
    {
        public ObservableCollection<Graphic> DisplayedVechileRealLocationGraphics
        {
            get
            {
                return MonitorList.VechileRealLocationGraphics.Graphics;
            }
        }

        public ObservableCollection<UIElement> DisplayedVechileRealLocationElements
        {
            get
            {

                return MonitorList.VechileRealLocationElements.Elements;
            }
        }


        public ObservableCollection<UIElement> DisplayedGpsHisDataSingleVechileElements
        {
            get
            {

                return MonitorList.GpsHisDataSingleVechileElements.Elements;
            }
        }
        public ObservableCollection<Graphic> DisplayedAlarmHappenLocationGraphics
        {
            get
            {
                return MonitorList.AlarmHappenLocationGraphics.Graphics;
            }
        }

        public ObservableCollection<UIElement> DisplayedAlarmHappenLocationElements
        {
            get
            {

                return MonitorList.AlarmHappenLocationElements.Elements;
            }
        }

        public ObservableCollection<Graphic> DisplayedGpsHisDataVechileGraphics
        {
            get
            {
                return MonitorList.GpsHisDataVechileGraphics.Graphics;
            }
        }

        public ObservableCollection<UIElement> DisplayedGpsHisDataVechileElements
        {
            get
            {

                return MonitorList.GpsHisDataVechileElements.Elements;
            }
        }



        public ObservableCollection<UIElement> DisplayedAlertHappenLocationElements
        {
            get
            {
                return MonitorList.AlertHappenLocationElements.Elements;
            }
        }

        public ObservableCollection<Graphic> DisplayedAlertHappenLocationGraphics
        {
            get
            {
                return MonitorList.AlertHappenLocationGraphics.Graphics;
            }
        }

        public ObservableCollection<UIElement> DisplayedVideoReplayGPSRouteElements
        {
            get
            {
                return MonitorList.VedioReplayGPSRoutelements.Elements;
            }
        }

        public ObservableCollection<Graphic> DisplayedVideoReplayGPSRouteGraphics
        {
            get
            {
                return MonitorList.VedioReplayGPSRouteGraphics.Graphics;
            }
        }

        /// <summary>
        /// Traffic Management
        /// </summary>
        public ObservableCollection<Graphic> TrafficManagerGraphics
        {
            get { return MonitorList.FenceGraphicHelp.GraphicList; }
        }
        /// <summary>
        /// Traffic management layers under daily monitoring, display of vehicles associated fence line sites, etc.
        /// </summary>
        public ObservableCollection<Graphic> MonitorTrafficManagerGraphics
        {
            get { return MonitorList.MonitorGraphicHelp.GraphicList; }
        }
        private string _baseLayerUrl;
        public string BaseLayerUrl
        {
            get { return _baseLayerUrl; }
            set
            {
                _baseLayerUrl = value;
                RaisePropertyChanged("BaseLayerUrl");
            }
        }
    }
    #endregion

    #region Monitoring global definition list
    /// <summary>
    /// Watchlist
    /// </summary>
    public static class MonitorList
    {
        private static VechileElementLst _RequestGpsDataVechileElements = new VechileElementLst();
        /// <summary>
        /// Demand for real-time vehicle data
        /// </summary>
        public static VechileElementLst VechileRealLocationElements
        {
            get
            {
                return _RequestGpsDataVechileElements;
            }
        }
        private static VechileGrapicLst _VechileRealLocationGraphics = new VechileGrapicLst();
        /// <summary>
        /// Real-time on-demand vehicle routing
        /// </summary>
        public static VechileGrapicLst VechileRealLocationGraphics
        {
            get
            {
                return _VechileRealLocationGraphics;
            }
        }


        private static VechileElementLst _AlarmHappenLocationElements = new VechileElementLst();
        /// <summary>
        ///  vehicle data processed A alarm button
        /// </summary>
        public static VechileElementLst AlarmHappenLocationElements
        {
            get
            {
                return _AlarmHappenLocationElements;
            }
        }
        private static VechileGrapicLst _AlarmHappenLocationGraphics = new VechileGrapicLst();
        /// <summary>
        /// Vehicle routes have handled a key alarm
        /// </summary>
        public static VechileGrapicLst AlarmHappenLocationGraphics
        {
            get
            {
                return _AlarmHappenLocationGraphics;
            }
        }


        private static VechileElementLst _AlertHappenLocationElements = new VechileElementLst();
        /// <summary>
        /// Alarm vehicle data processed
        /// </summary>
        public static VechileElementLst AlertHappenLocationElements
        {
            get
            {
                return _AlertHappenLocationElements;
            }
        }

        private static VechileGrapicLst _AlertHappenLocationGraphics = new VechileGrapicLst();
        /// <summary>
        /// Vehicle routes have handled a key alarm
        /// </summary>
        public static VechileGrapicLst AlertHappenLocationGraphics
        {
            get
            {
                return _AlertHappenLocationGraphics;
            }
        }

        private static VechileElementLst _VedioReplayGPSRouteElements = new VechileElementLst();
        /// <summary>
        /// 回放视频时的车辆位置
        /// </summary>
        public static VechileElementLst VedioReplayGPSRoutelements
        {
            get
            {
                return _VedioReplayGPSRouteElements;
            }
        }

        public static VechileGrapicLst _VedioReplayGPSRouteGraphics = new VechileGrapicLst();
        /// <summary>
        /// 回放视频时车辆的轨迹
        /// </summary>
        public static VechileGrapicLst VedioReplayGPSRouteGraphics
        {
            get
            {
                return _VedioReplayGPSRouteGraphics;
            }
        }

        private static VechileElementLst _GpsHisDataVechileElements = new VechileElementLst();
        /// <summary>
        /// GPS vehicle history data under
        /// </summary>
        public static VechileElementLst GpsHisDataVechileElements
        {
            get
            {
                return _GpsHisDataVechileElements;
            }
        }

        private static VechileElementLst _GpsHisDataSingleVechileElements = new VechileElementLst();
        /// <summary>
        /// GPS vehicle history data under
        /// </summary>
        public static VechileElementLst GpsHisDataSingleVechileElements
        {
            get
            {
                return _GpsHisDataSingleVechileElements;
            }
        }

        private static VechileGrapicLst _GpsHisDataVechileGraphics = new VechileGrapicLst();
        /// <summary>
        /// Vehicle GPS data under the historical route
        /// </summary>
        public static VechileGrapicLst GpsHisDataVechileGraphics
        {
            get
            {
                return _GpsHisDataVechileGraphics;
            }
        }

        /// <summary>
        /// Traffic Management graphic layer management classes help
        /// </summary>
        private static TrafficGraphicHelpVm _fenceGraphicHelp = new TrafficGraphicHelpVm();
        public static TrafficGraphicHelpVm FenceGraphicHelp
        {
            get { return _fenceGraphicHelp; }
        }

        private static MonitorTrafficGraphicHelpVm _MonitorGraphicHelp = new MonitorTrafficGraphicHelpVm();
        public static MonitorTrafficGraphicHelpVm MonitorGraphicHelp
        {
            get { return _MonitorGraphicHelp; }
        }
    }
    #endregion

    #region Base class monitoring list
    /// <summary>
    /// The management track of the vehicle, and GraphicLayer binding.
    /// </summary>
    public class VechileGrapicLst : BaseNotify
    {
        private ObservableCollection<Graphic> _Graphics = new ObservableCollection<Graphic>();
        public ObservableCollection<Graphic> Graphics
        {
            get
            {
                return _Graphics;
            }
            set
            {
                _Graphics = value;
                RaisePropertyChanged("Graphics");
            }
        }

        private ObservableCollection<string> CarUniqueIDList = new ObservableCollection<string>();

        /// <summary>
        /// Add graphics, if there is coverage
        /// </summary>
        /// <param name="graphic"></param>
        /// <param name="csUniqueID"></param>
        public void AddGraphic(Graphic graphic, string csUniqueID)
        {
            int ind = CarUniqueIDList.IndexOf(csUniqueID);
            if (ind > -1)
            {
                Graphics[ind] = graphic;
            }
            else
            {
                CarUniqueIDList.Add(csUniqueID);
                Graphics.Add(graphic);
            }
        }
        public void RemoveGraphic(string csUniqueID)
        {
            int ind = CarUniqueIDList.IndexOf(csUniqueID);
            if (ind > -1)
            {
                CarUniqueIDList.RemoveAt(ind);
                Graphics.RemoveAt(ind);
            }
        }

        public void RemoveGraphics(string PreID)
        {
            for (int i = CarUniqueIDList.Count - 1; i >= 0; i--)
            {
                if (CarUniqueIDList[i].IndexOf(PreID) > -1)
                {
                    CarUniqueIDList.RemoveAt(i);
                    Graphics.RemoveAt(i);
                }
            }
        }
        public Graphic GetGraphics(string csUniqueID)
        {
            int ind = CarUniqueIDList.IndexOf(csUniqueID);
            if (ind > -1)
            {
                return Graphics[ind];
            }
            return null;
        }
        /// <summary>
        /// Returns a collection point
        /// </summary>
        /// <param name="csUniqueID"></param>
        /// <returns></returns>
        public List<Graphic> GetGraphis(string csUniqueID, out List<string> listIDs)
        {
            List<Graphic> list = new List<Graphic>();
            List<string> listID = new List<string>();
            for (int i = 0; i < CarUniqueIDList.Count; i++)
            {
                if (CarUniqueIDList[i].IndexOf(csUniqueID) > -1 && CarUniqueIDList[i] != csUniqueID)
                {
                    list.Add(Graphics[i]);
                    listID.Add(CarUniqueIDList[i]);
                }
            }
            listIDs = listID;
            return list;
        }
        /// <summary>
        /// Remove the set of points
        /// </summary>
        /// <param name="PreID"></param>
        public void RemoveGraphicsEx(string PreID)
        {
            for (int i = CarUniqueIDList.Count - 1; i >= 0; i--)
            {
                if (CarUniqueIDList[i].IndexOf(PreID) > -1 && CarUniqueIDList[i] != PreID)
                {
                    CarUniqueIDList.RemoveAt(i);
                    Graphics.RemoveAt(i);
                }
            }
        }

        public void Clear()
        {
            CarUniqueIDList.Clear();
            Graphics.Clear();
        }
    }

    /// <summary>
    /// Management static class car, bound with ElementLayer
    /// Because ArcGIS bound Jounce not have both, had adopted an unwise move.
    /// </summary>
    public class VechileElementLst : BaseViewModel
    {
        private ObservableCollection<UIElement> _Elements = new ObservableCollection<UIElement>();
        public ObservableCollection<UIElement> Elements
        {
            get
            {
                return _Elements;
            }
            set
            {
                _Elements = value;
            }
        }
        /// <summary>
        /// Demand Vehicles
        /// </summary>
        /// <param name="car"></param>
        /// <param name="pt"></param>
        public void AddCars(GpsCar car, ESRI.ArcGIS.Client.Geometry.MapPoint pt)
        {
            if (!Elements.Contains(car))
            {
                ElementLayer.SetEnvelope(car, new ESRI.ArcGIS.Client.Geometry.Envelope(pt, pt));
                Elements.Add(car);
            }
        }
        /// <summary>
        /// Remove Vehicle Demand
        /// </summary>
        /// <param name="car"></param>
        public void RemoveCars(GpsCar car)
        {
            if (Elements.Contains(car))
            {
                ElementLayer.SetEnvelope(car, null);
                Elements.Remove(car);
            }
        }

        public void Clear()
        {
            for (int i = Elements.Count - 1; i >= 0; i--)
            {
                ElementLayer.SetEnvelope((Elements[i] as GpsCar), null);
                Elements.RemoveAt(i);
            }
        }
        /// <summary>
        /// Remove Vehicle Demand
        /// </summary>
        /// <param name="csUniqueID"></param>
        public void RemoveCars(string csUniqueID)
        {
            foreach (GpsCar gpscar in Elements)
            {
                if (gpscar.UniqueID == csUniqueID)
                {
                    ElementLayer.SetEnvelope(gpscar, null);
                    Elements.Remove(gpscar);
                    break;
                }
            }
        }

        /// <summary>
        /// Remove all vehicles beginning with a character
        /// </summary>
        /// <param name="csUniqueID"></param>
        public void RemoveCarsByPre(string csUniqueID)
        {
            for (int i = Elements.Count - 1; i >= 0; i--)
            {
                if ((Elements[i] as GpsCar).UniqueID.IndexOf(csUniqueID) > -1)
                {
                    ElementLayer.SetEnvelope((Elements[i] as GpsCar), null);
                    Elements.RemoveAt(i);
                }
            }
        }
        /// <summary>
        /// Whether to draw a vehicle or event on the layers, etc.
        /// </summary>
        /// <param name="csUniqueID"></param>
        /// <returns></returns>
        public bool Exists(string csUniqueID)
        {
            foreach (GpsCar gpscar in Elements)
            {
                if (gpscar.UniqueID == csUniqueID)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get the vehicle object
        /// </summary>
        /// <param name="MDVRID"></param>
        /// <returns></returns>
        public UIElement GetCarUIElement(string csCarNo)
        {
            foreach (GpsCar gpscar in Elements)
            {
                if (gpscar.CarNo == csCarNo)
                {
                    return gpscar;
                }
            }
            return null;
        }
        /// <summary>
        /// Based MDVRID get gpscar
        /// </summary>
        /// <param name="csUniqueID"></param>
        /// <returns></returns>
        public GpsCar GetCarUIElementByUniqueID(string csUniqueID)
        {
            foreach (GpsCar gpscar in Elements)
            {
                if (gpscar.UniqueID == csUniqueID)
                {
                    return gpscar;
                }
            }
            return null;
        }

        public GpsCar GetCarUIElementByUniqueIDAndGpsTime(string carNum, string strGPSTime)
        {
            foreach (GpsCar gpscar in Elements)
            {
                if (gpscar.UniqueID == carNum && gpscar.GpsTime == strGPSTime)
                {
                    return gpscar;
                }
            }
            return null;
        }
    }
    #endregion
}
