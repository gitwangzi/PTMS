using Gsafety.PTMS.Enums;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: cfa3260e-0881-4706-b7d6-bf3ea8138779      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share
/////    Project Description:    
/////             Class Name: ServerConfigInfor
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 15:04:46
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/8 15:04:46
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

namespace Gsafety.PTMS.Share
{
    public class ServerConfigInfo
    {
        public string ServiceUrlConfig { get; set; }

        public string LayersSearchParams { get; set; }

        public string MapInitExtent { get; set; }

        public string OverMapMaximumExtent { get; set; }

        public string GisBaseMapUrl { get; set; }

        public string DomGisBaseMapUrl { get; set; }

        public string GisFeatureMapUrl { get; set; }

        public string ReportServerUrl { get; set; }

        public string FacilitySpeed { get; set; }

        public string DateFormat { get; set; }

        public string LongDateFormat { get; set; }

        public string MapSubType { get; set; }

        public bool Bias { get; set; }

        public string BiasType { get; set; }

        public double BiasX { get; set; }

        public double BiasY { get; set; }

        public string Layers { get; set; }

        public string MatrixSet { get; set; }

        public string TileMatrixSet { get; set; }

        /// <summary>
        /// TileLayer Type
        /// BingMap
        /// GoogleMap
        /// ArcGisMap
        /// ......
        /// </summary>
        public BaseMapTypeEnum BaseMapType { get; set; }

        /// <summary>
        /// BingMap Key
        /// </summary>
        public string BingKey { get; set; }

        private string _MinResolution;

        public string MinResolution
        {
            get
            {
                return _MinResolution.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
            }

            set
            {
                _MinResolution = value;
            }
        }
        private string _MaxResolution;

        public string MaxResolution
        {
            get
            {
                return _MaxResolution.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
            }

            set
            {
                _MaxResolution = value;
            }

        }
        private string _AutoLocateResolution;

        public string AutoLocateResolution
        {
            get
            {
                return _AutoLocateResolution.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
            }

            set
            {
                _AutoLocateResolution = value;
            }
        }

        private string _GisGeometryServiceUrl = "";
        public string GisGeometryServiceUrl
        {
            get { return _GisGeometryServiceUrl; }
            set { _GisGeometryServiceUrl = value; }
        }

        /// <summary>
        /// gis query privince and city config for the funtion of zoom to dist
        /// </summary>
        private string _DistQueryGisID = "";
        public string DistQueryGisID
        {
            get { return _DistQueryGisID; }
            set { _DistQueryGisID = value; }
        }

        public string VideoServiceFileServerIP { get; set; }

        public string VideoServiceFileServerPort { get; set; }

        public string PictureServiceFileServerIP { get; set; }

        public string PictureServiceFileServerPort { get; set; }

        public string RTSPServiceIP { get; set; }
        public string RTSPServicePort { get; set; }
        public string RTSPStreamChannel { get; set; }

        //报警参数
        public int AlarmParamAlarmBeforeTime { get; set; }
        public int AlarmParamAlarmEndTime { get; set; }
        public int AlarmParamRelatedData { get; set; }


        public int DefaultVideoConnectTimeOut
        {
            set
            {
                Gsafety.PTMS.Media.MediaStreamFacadeParameters.DefaultStartTimeout = TimeSpan.FromSeconds(value);
            }
        }

        private DisplayParameter _displayParameter = new DisplayParameter();
        public DisplayParameter DisplayParameter
        {
            get
            {
                return _displayParameter;
            }
        }

        public string Culture { get; set; }

        public string GoogleAddress { get; set; }

        public string MapLanguage { get; set; }

        public string EnglishHelpUrl { get; set; }

        public string ChineseHelpUrl { get; set; }

        public string SpanishHelpUrl { get; set; }

        public bool Authenticate { get; set; }
    }
}
