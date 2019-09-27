/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 17086bb5-dcc6-43a5-91ed-82b4b2e7356e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: GisManagement.Models
/////    Project Description:    
/////             Class Name: ConstDefine
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/5 11:23:46
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/5 11:23:46
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
using Gsafety.PTMS.Share;

namespace GisManagement.Models
{
    /// <summary>
    /// GIS systerm 
    /// </summary>
    public class ConstDefine
    {
        public static string cmType = "CmType";
        public static string Normal = "Normal";

        /// <summary>
        /// In the animation process, the route plotted against time interval.
        /// When a large number of vehicles need to draw lines, this time should be adjusted long
        /// </summary>
        public static int UpdateDisplayInterval = 200;
        /// <summary>
        /// Shows the number of log records up to
        /// </summary>
        public static int MaxRecordNum = 100;
        ///// <summary>
        ///// 
        ///// </summary>
        //public static double AutoLocateResolution = 28;
        ///// <summary>
        ///// 
        ///// </summary>
        //public static double MinResolution = 1.1943285668550503;
        ///// <summary>
        ///// 
        ///// </summary>
        //public static double MaxResolution = 961.787;
        /// <summary>
        /// The default map size
        /// </summary>
        //public static double DefaultScale = 3635093.2022;
        public static double DefaultScale = 3779.52; 
        /// <summary>
        /// Record the minimum offset value in Historical Perspective
        /// </summary>
        public static double MinOffsetFactorToWriteHistroy = 0.01;

        #region 临时图层
        /// <summary>
        /// Temporary draw length measured Layers
        /// </summary>
        public static string TempGraphicLayer = "MeasureGraphicsLayer";
        /// <summary>
        /// Plot layer
        /// </summary>
        public static string TempDrawLayer = "MyGraphicsLayer";

        public static string TempDrawQueryLayer = "MyDrawGraphicsLayer";

        public static string MyDrawQueryGraphicsLayer = "MyDrawQueryGraphicsLayer";
        #endregion

        /// <summary>
        /// Demand vehicle element layer
        /// </summary>
        public static string RealLocationElementLayerName = "RealLocationElement";
        /// <summary>
        /// Demand vehicle graphics layers
        /// </summary>
        public static string RealLocationGraphicsLayerName = "RealLocationGraphics";

        /// <summary>
        /// Vehicle alarm element layers have been processed
        /// </summa>
        public static string AlertHappenLocationElementLayerName = "AlertHappenLocationElement";
        /// <summary>
        /// Vehicle graphics layers have been processed alarms
        /// </summary>
        public static string AlertHappenLocationGraphicsLayerName = "AlertHappenLocationGraphics";
        /// <summary>
        /// AlarmHappenLocationElement
        /// </summary>
        public static string AlarmHappenLocationElementLayerName = "AlarmHappenLocationElement";
        /// <summary>
        /// AlarmHappenLocationGraphics
        /// </summary>
        public static string AlarmHappenLocationGraphicsLayerName = "AlarmHappenLocationGraphics";


        /// <summary>
        /// GPS vehicle history data
        /// </summary>
        public static string GpsHisDataElementLayerName = "GpsHisDataElement";
        /// <summary>
        ///  History GPS Graphics Layer
        /// </summary>
        public static string GpsHisDataGraphicsLayerName = "GpsHisDataGraphics";

        public static string GpsHisDataSingleElementLayerName = "GpsHisDataSingleElement";

        /// <summary>
        /// Plot layer traffic management
        /// </summary>
        public static string TrafficGraphicLayerName = "TrafficGraphicLayer";
        /// <summary>
        /// Site Traffic Management Layer
        /// </summary>
        public static string TrafficFeatureStopLayerName = "AntAppFeatureMapLayer_BusStop";
        /// <summary>
        /// Traffic Management Line Layers
        /// </summary>
        public static string TrafficFeatureRouteLayerName = "AntAppFeatureMapLayer_Rout";
        /// <summary>
        /// Traffic management layer fence
        /// </summary>
        public static string TrafficFeatureFenceLayerName = "AntAppFeatureMapLayer_Fence";
        /// <summary>
        /// When switching layer region marked selected areas
        /// </summary>
        public static string MarkDistGraphicsLayer = "MarkDistGraphicsLayer";
        /// <summary>
        /// UTM projection system WGS_1984_UTM_Zone_17S
        /// </summary>
        public static int GisUTMProjectCoorSystemWKID = 32717;
        /// <summary>
        /// Temporary traffic management layers, temporary label elements to draw when
        /// </summary>
        public static string TrafficTempGraphicsLayer = "TrafficTempGraphicsLayer";
        /// <summary>
        /// max message count
        /// </summary>
        public static int MaxSeedMeesageCount = 100;
    }

    /// <summary>
    /// The current definition of visible layers vehicle
    /// </summary>
    public enum ElementLayerDefine { miVERealLocation = 1, miVEAlarmHappenLocation = 2, miVEAlertHappenLocation = 3, miVETraffic = 4, miVEHisData=5 }

    
    public static class TranslateInfo
    {
        public static string GpsTime = "GIS_GpsTime";
        public static string CarNo = "GIS_CarNo";
        public static string Speed = "GIS_Speed";
        public static string Lon = "GIS_Lon";
        public static string Lat = "GIS_Lat";
        public static string Dir = "GIS_Dir";
        public static string FilterField = "GIS_FilterField";

        public static string Animation = "GIS_Animation";
        public static string Trajectory = "GIS_Trajectory";
        public static string interval = "GIS_interval";
        public static string MDVRID = "GIS_MDVRID";
        public static string RecDateTime = "GIS_RecDateTime";


        public static string GpsValid = "GIS_GpsValid";
        public static string GpsInValid = "GIS_GpsInValid";
        public static string NonGps = "GIS_NonGps";
        public static string ScaleValid = "GIS_ScaleValid";
        public static string ScaleInValid = "GIS_ScaleInValid";
        public static string ScaleAbove = "GIS_AboveScale";
        public static string ScaleBelow = "GIS_BelowScale";

        public static string QueryFindError = "GIS_QueryFailed";
        public static string ReadQueryConfigError = "GIS_ReadQueryConfigError";
        public static string SearchTextIsNull = "GIS_SearchTextIsNull";
        public static string AllProv = "GIS_AllProv";
        public static string RemoveCarTip = "GIS_RemoveCarTip";
        public static string Tip = "GIS_Tip";
        public static string RecMessage = "GIS_RecMessage";

        public static string SendMessage = "GIS_SendMessage";
        public static string Province = "GIS_Province";
        public static string City = "GIS_City";
        public static string BtnClearDisplayTip = "GIS_ClearDisplayTip";

        public static string BtnGlobeTip = "GIS_GlobeTip";
        
        public static string BtnNextExtentTip = "GIS_NextExtentTip";

        public static string BtnPrevExtentTip = "GIS_PrevExtentTip";

        public static string BtnAreaTip = "GIS_AreaTip";

        public static string BtnLengthTip = "GIS_LengthTip";

        public static string InValidChar = "GIS_InValidChar";

        public static string TooShortInput = "GIS_TooShortInput";

        public static string BtnOverMap = "GIS_OverMapTip";


        public static string FullScreenTip = "GIS_FullScreenTip";

        public static string Translate(string str)
        {
            //SR sr = new SR();
            //return sr.GetString(str, System.Threading.Thread.CurrentThread.CurrentCulture);
            string TransStr=ApplicationContext.Instance.StringResourceReader.GetString(str);
            if (TransStr == "") return str;
            return TransStr;
            
            //return null;
        }
    }

    public static class GPSState
    {
        public static bool Valid(string  csvalid)
        {
            //2016.8.2
            if ((csvalid == "N")|| (csvalid==null)) return false;
            //if ((str == "A") || (str == "V")) return true;
            return true;
        }
    }
  
}
