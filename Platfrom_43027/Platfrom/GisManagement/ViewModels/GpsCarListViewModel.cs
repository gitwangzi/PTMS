/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4d037c19-8662-44da-8ec4-af36d61f33bd      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: GisManagement.ViewModels
/////    Project Description:    
/////             Class Name: GpsCarListViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/5 17:07:34
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/5 17:07:34
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ESRI.ArcGIS.Client;
using GisManagement.Models;
using GisManagement.Views;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Gsafety.Common.CommMessage;


namespace GisManagement.ViewModels
{
    [ExportAsViewModel(GisName.GpsCarListViewModel)]
    public class GpsCarListViewModel : BaseViewModel,
        //IEventSink<ElementVisibleEventArgs>,
        //IEventSink<LocateAlarmInfo>,
        IPartImportsSatisfiedNotification
    {
        #region Definitions

        public SynchronizationContext syn;

        private VehicleServiceClient Vechcleclient = ServiceClientFactory.Create<VehicleServiceClient>();
        #endregion

        #region Initialization
        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="csMap"></param>
        public GpsCarListViewModel()
        {
            try
            {
                //_RequestGpsDataVechileView = new CollectionViewSource();
                //_RequestGpsDataVechileView.Source = MonitorList.VechileRealLocationElements.Elements;
                //_RequestGpsDataVechileView.Filter += _ViewCars_Filter;



                //_DisposedOneKeyAlarmVechileDataView = new CollectionViewSource();
                //_DisposedOneKeyAlarmVechileDataView.Source = MonitorList.AlarmHappenLocationElements.Elements;
                //_DisposedOneKeyAlarmVechileDataView.Filter += _ViewCars_Filter;

                //_DataGridDisplayLst = new List<GisMapDisplayView>();
                //_DataGridDisplayLst.Add(GisMapDisplayView.miRealTimeMonitor);
                //_DataGridDisplayLst.Add(GisMapDisplayView.miMonitor_Alarm);

                //_ElementDisplayLst = new List<ElementLayerDefine>();
                //_ElementDisplayLst.Add(ElementLayerDefine.miVERealLocation);
                //_ElementDisplayLst.Add(ElementLayerDefine.miVEOneKeyAlarm);

                //Vechcleclient.GetVehicleByCarNumberCompleted += client_GetVehicleByCarNumberCompleted;



            }
            catch (Exception ee)
            {
                ApplicationContext.Instance.Logger.LogException("GpsCarListViewModel", ee);
            }
        }

        protected override void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
        }

        #endregion

        #region Displayed in the list view definition
        private CollectionViewSource _RequestGpsDataVechileView;
        /// <summary>
        ///Filtered out by the filter-demand vehicles
        /// </summary>
        public ICollectionView RequestGpsDataVechileView
        {
            get
            {
                return _RequestGpsDataVechileView.View;
            }
        }

        private CollectionViewSource _OneKeyAlarmVechileDataView;
        /// <summary>
        ///Filtered out by the filter of a key police vehicles
        /// </summary>
        public ICollectionView OneKeyAlarmVechileDataView
        {
            get
            {
                return _OneKeyAlarmVechileDataView.View;
            }
        }
        private CollectionViewSource _DisposedOneKeyAlarmVechileDataView;
        /// <summary>
        /// Filtered out by the filter has processed a key vehicle alarm
        /// </summary>
        public ICollectionView DisposedOneKeyAlarmVechileDataView
        {
            get
            {
                return _DisposedOneKeyAlarmVechileDataView.View;
            }
        }
        /// <summary>
        /// Set the filter conditions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _ViewCars_Filter(object sender, FilterEventArgs e)
        {
            if (_FilterText == "")
            {
                e.Accepted = true;
                return;
            }
            GpsCar carvm = e.Item as GpsCar;

            if (UserSelectedFilterField == TranslateInfo.Translate(TranslateInfo.CarNo))
            {
                e.Accepted = (carvm.CarNo.Contains(_FilterText));
            }
            else if (UserSelectedFilterField == TranslateInfo.Translate(TranslateInfo.MDVRID))
            {
                e.Accepted = (carvm.UniqueID.Contains(_FilterText));
            }
            else if (UserSelectedFilterField == TranslateInfo.Translate(TranslateInfo.City))
            {
                e.Accepted = (carvm.Prov.Contains(_FilterText));
            }
            else
            {
                e.Accepted = true;
            }
        }

        /// <summary>
        /// Support filtering fields
        /// </summary>
        public List<string> FilterFieldList
        {
            get
            {
                List<string> lst = new List<string>();
                lst.Add(TranslateInfo.Translate(TranslateInfo.CarNo));
                //lst.Add(TranslateInfo.Translate(TranslateInfo.MDVRID));
                //lst.Add(TranslateInfo.Translate(TranslateInfo.ANTGPSID));
                lst.Add(TranslateInfo.Translate(TranslateInfo.City));
                UserSelectedFilterField = TranslateInfo.Translate(TranslateInfo.CarNo);
                return lst;
            }
        }
        private string _UserSelectedFilterField;
        /// <summary>
        /// User-selected filter fields
        /// </summary>
        public string UserSelectedFilterField
        {
            get
            {
                return _UserSelectedFilterField;
            }
            set
            {
                _UserSelectedFilterField = value;
                FilterText = "";
            }
        }
        private string _FilterText;
        /// <summary>
        /// Filter text entered by the user
        /// </summary>
        public string FilterText
        {
            get
            {
                return _FilterText;
            }
            set
            {
                _FilterText = value;
                RaisePropertyChanged("FilterText");
                DoFilter();
            }
        }


        /// <summary>
        /// Monitor vehicle with all queries need to see the vehicle
        /// </summary>
        /// <returns></returns>
        private void DoFilter()
        {
            _RequestGpsDataVechileView.View.Refresh();
            _OneKeyAlarmVechileDataView.View.Refresh();
            _DisposedOneKeyAlarmVechileDataView.View.Refresh();
        }

        #endregion


        /// <summary>
        /// Latitude and longitude coordinates into projected coordinates
        /// </summary>
        /// <returns></returns>
        public static ESRI.ArcGIS.Client.Geometry.MapPoint GetProjCoord(double csLon, double csLat)
        {
            ESRI.ArcGIS.Client.Geometry.MapPoint pt = new ESRI.ArcGIS.Client.Geometry.MapPoint(csLon, csLat);
            //return ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(pt);
            return Gsafety.Common.Transform.GeographicToWebMercator(pt);
        }

        /// <summary>
        /// Projection coordinates into latitude and longitude coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static ESRI.ArcGIS.Client.Geometry.MapPoint GetGeoCoord(double x, double y)
        {
            ESRI.ArcGIS.Client.Geometry.MapPoint pt = new ESRI.ArcGIS.Client.Geometry.MapPoint(x, y);
            //return ESRI.ArcGIS.Client.Bing.Transform.WebMercatorToGeographic(pt);
            return Gsafety.Common.Transform.WebMercatorToGeographic(pt);
        }


        #region Display Control

        private int _DisplayIndex;
        public int DisplayIndex
        {
            get
            {
                return _DisplayIndex;
            }
            set
            {
                _DisplayIndex = value;
                RaisePropertyChanged("DisplayIndex");
            }
        }


        private List<ElementLayerDefine> _ElementDisplayLst;
        public List<ElementLayerDefine> ElementDisplayLst
        {
            get
            {
                return _ElementDisplayLst;
            }
            set
            {
                _ElementDisplayLst = value;
            }
        }
        #endregion



        #region Subscribe and handling receives messages
        /// <summary>
        /// Subscribe to event
        /// </summary>
        public void OnImportsSatisfied()
        {
            //EventAggregator.SubscribeOnDispatcher<DisposedAlarmInfo>(this);
            //EventAggregator.SubscribeOnDispatcher<CancelDisposedAlarmInfo>(this);
            //EventAggregator.SubscribeOnDispatcher<LocateAlarmInfo>(this);
            //EventAggregator.SubscribeOnDispatcher<ElementVisibleEventArgs>(this);
            //Subscribe alarms
        }




        /// <summary>
        /// The current state record
        /// </summary>
        /// <param name="publishedEvent"></param>
        //public void HandleEvent(ElementVisibleEventArgs publishedEvent)
        //{
        //    ElementDisplayLst = publishedEvent.VisibleLst;
        //}



        /// <summary>
        /// Positioning police intelligence information processing
        /// </summary>
        /// <param name="publishedEvent"></param>
        //public void HandleEvent(LocateAlarmInfo publishedEvent)
        //{
        //    try
        //    {
        //        GpsCar car = MonitorList.OneKeyAlarmVechileElements.GetCarUIElementByMDVRID(publishedEvent.MDVRID) as GpsCar;
        //        if (car == null)//
        //        {
        //            car = MonitorList.OneKeyAlarmDisposedVechileElements.GetCarUIElementByMDVRID(publishedEvent.MDVRID) as GpsCar;
        //            if (car == null)
        //            {
        //                //添加
        //                car = AddCar(VisibleElementDefine.miVEDisposedOneKeyAlarm, publishedEvent.CarNo, publishedEvent.MDVRID, "", VehicleType.miNone);
        //                Vechcleclient.GetVehicleByCarNumberAsync(publishedEvent.CarNo);
        //            }
        //            //
        //            GPSDataEventArgs args = new GPSDataEventArgs();
        //            args.DvId = publishedEvent.MDVRID;
        //            args.Dir = publishedEvent.Dir;
        //            args.GpsTime = publishedEvent.GpsTime;
        //            args.RecLat = publishedEvent.RecLat;
        //            args.RecLon = publishedEvent.RecLon;
        //            args.V = publishedEvent.V;
        //            args.Valid = publishedEvent.Valid;

        //            Draw(car, args, 1, false, false);

        //        }
        //        if ((car != null) && (car.HasDraw))
        //        {
        //            //car.IsTrack = true;
        //            SetSelectedCar(car);
        //            Locate(car.Lon, car.Lat);
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        //ApplicationContext.Instance.Logger.LogException("GpsCarListViewModel", ee);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publishedEvent"></param>
        //public void HandleEvent(DisposedAlarmInfo publishedEvent)
        //{
        //    try
        //    {
        //        GpsCar car = MonitorList.OneKeyAlarmDisposedVechileElements.GetCarUIElementByMDVRID(publishedEvent.MDVRID) as GpsCar;
        //        if (car == null)
        //        {
        //            car = AddCar(VisibleElementDefine.miVEDisposedOneKeyAlarm, publishedEvent.CarNo, publishedEvent.MDVRID, "", VehicleType.miNone);
        //            Vechcleclient.GetVehicleByCarNumberAsync(publishedEvent.CarNo);//
        //        }
        //        GPSDataEventArgs args = new GPSDataEventArgs();
        //        args.DvId = publishedEvent.MDVRID;
        //        args.Dir = publishedEvent.Dir;
        //        args.GpsTime = publishedEvent.GpsTime;
        //        args.RecLat = publishedEvent.RecLat;
        //        args.RecLon = publishedEvent.RecLon;
        //        args.V = publishedEvent.V;
        //        args.Valid = publishedEvent.Valid;

        //        Draw(car, args, 1, false, false);
        //        if (publishedEvent.Locate)
        //        {
        //            if ((car != null) && (car.HasDraw))
        //            {
        //                //car.IsTrack = true;
        //                SetSelectedCar(car);
        //                Locate(car.Lon, car.Lat);
        //            }
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        ApplicationContext.Instance.Logger.LogException("GpsCarListViewModel", ee);
        //    }
        //}




        /// <summary>
        /// 
        /// </summary>
        /// <param name="publishedEvent"></param>
        //public void HandleEvent(CancelDisposedAlarmInfo publishedEvent)
        //{
        //    try
        //    {
        //        if (publishedEvent.CancelAll)
        //        {
        //            foreach (GpsCar car in MonitorList.OneKeyAlarmDisposedVechileElements.Elements)
        //            {
        //                RemoveCarByMDVRID(VisibleElementDefine.miVEDisposedOneKeyAlarm, car.MDVRID);
        //            }
        //        }
        //        else
        //        {
        //            RemoveCarByMDVRID(VisibleElementDefine.miVEDisposedOneKeyAlarm, publishedEvent.MDVRID);
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        ApplicationContext.Instance.Logger.LogException("GpsCarListViewModel", ee);
        //    }
        //}




        #endregion


    }
}
