/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9c70ce8b-0bd4-4e12-9908-faa72f767b72      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: GisManagement.Models
/////    Project Description:    
/////             Class Name: MapEventArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/5 17:15:12
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/5 17:15:12
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Share;

namespace GisManagement.Models
{
    /// <summary>
    /// Need to handle commands issued to the map
    /// </summary>    
    public class MapEventArgs : EventArgs
    {
        /// <summary>
        /// locate
        /// </summary>
        public const string MapOperateLocate = "Locate";

        public const string MapOperateLocateGeometry = "LocateGeometry";
        /// <summary>
        /// pan to
        /// </summary>
        public const string MapOperatePanTo = "PanTo";
        /// <summary>
        /// draw graphics
        /// </summary>
        public const string MapOperateDrawGraphics = "DrawGraphics";
        /// <summary>
        /// trace car
        /// </summary>
        public const string MapOperateTraceCar = "TraceCar";

        public const string MapOperateLocateByUniqueID = "LocateByUniqueID";

        public const string MapOperateTraceByUniqueID = "TraceByUniqueID";

        /// <summary>
        /// set layer visble
        /// </summary>
        public const string MapOperateElementVisible = "ElementVisible";

        /// <summary>
        /// DisplayMonitorGridVisible
        /// </summary>
        public const string DisplayMonitorGridVisible = "DisplayMonitorGridVisible";

        public const string ReplayHisGpsData = "ReplayHisGpsData";

        public const string MapRefresh = "MapRefresh";
        /// <summary>
        /// Operate
        /// </summary>
        public string Operate { get; set; }
    }

    /// <summary>
    /// 清除图层事件
    /// </summary>
    public class ClearMapIsExecuted
    {

    }
    /// <summary>
    /// Locate Event
    /// </summary>
    public class LocateEventArgs : MapEventArgs
    {
        public LocateEventArgs()
        {
            Operate = MapOperateLocate;
        }

        public double Lon { get; set; }
        public double Lat { get; set; }
        public string UniqueID { get; set; }
        public string VehicleID { get; set; }
        public ElementLayerDefine VE { get; set; }
    }

    /// <summary>
    /// locate geometry
    /// </summary>
    public class LocateGeometryEventArgs : MapEventArgs
    {
        public LocateGeometryEventArgs()
        {
            Operate = MapOperateLocateGeometry;
        }
        public ESRI.ArcGIS.Client.Geometry.Geometry LocateGeometry;

        public ElementLayerDefine VE { get; set; }
    }
    /// <summary>
    /// draw message
    /// </summary>
    public class DrawGraphicsEventArgs : MapEventArgs
    {
        public DrawGraphicsEventArgs()
        {
            Operate = MapOperateDrawGraphics;
        }
        public ESRI.ArcGIS.Client.Graphic Graphic { get; set; }
        public string LayerName { get; set; }
    }


    /// <summary>
    /// ElementVisibleEventArgs
    /// </summary>
    public class ElementVisibleEventArgs : MapEventArgs
    {
        public ElementVisibleEventArgs()
        {
            Operate = MapOperateElementVisible;
            _VisibleLst = new List<ElementLayerDefine>();
        }
        private List<ElementLayerDefine> _VisibleLst;
        public List<ElementLayerDefine> VisibleLst
        {
            get
            {
                return _VisibleLst;
            }
            set
            {
                _VisibleLst = value;
            }
        }
    }


    public class RefreshEvent : MapEventArgs
    {
        public RefreshEvent()
        {
            Operate = MapRefresh;
        }
    }
    public class ReplayHisGpsDataEvent : MapEventArgs
    {
        public ReplayHisGpsDataEvent()
        {
            Operate = ReplayHisGpsData;
        }
        public string CarNo
        {
            get;
            set;
        }

        public VehicleType CarStyle
        {
            get;
            set;
        }

        public DateTime QueryStartTime { get; set; }
        public DateTime QueryEndTime { get; set; }

        public bool WithGpsData { get; set; }//Whether the data has been brought Gps

        public bool IsAlarm { get; set; }//Whether the alarm data

        public List<GPSDataEventArgs> HisData
        {
            get;
            set;
        }




        public string AlarmUID { get; set; }//alarm id
    }


    public enum GisDisplayControlType { miMonitor_RealTime = 1, miMonitor_Alarm = 2, miTraffic = 3, miHistory_Alarm = 4, miMonitor_Alert = 5, miHistory_Alert = 6 }
    public class GisDisplayControlEvent : EventArgs
    {
        public GisDisplayControlType Display { get; set; }
        public string DisplayGroupName { get; set; }
    }
}
