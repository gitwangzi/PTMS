/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ab9d8f7d-582f-4acf-a517-8fbe0277fad8      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: GisManagement.Models
/////    Project Description:    
/////             Class Name: GPSDataEventArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/5 17:17:01
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/5 17:17:01
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
using Gsafety.Common.Converts;

namespace GisManagement.Models
{
    /// <summary>
    /// Received GPS data and make the appropriate format conversion
    /// </summary>
    public class GPSDataEventArgs
    {
        private string _CmType;

        /// <summary>
        /// Command word
        /// </summary>
        public string CmType
        {
            get
            {
                return _CmType;
            }
        }

        public GPSDataEventArgs()
        {
            _CmType = "Normal";
        }
        /// <summary>
        /// Device ID (MDVRID \ ANTGPSID)
        /// </summary>
        public string DvId { get; set; }
        /// <summary>
        /// Message ID
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// After conversion system using latitude
        /// </summary>
        public string Lat
        {
            get
            {
                //RecLat into the system can handle the format
                DisplayLatConvert con = new DisplayLatConvert();
                return con.ConvertBack(_RecLat, null, null, null).ToString();
            }
        }
        /// <summary>
        /// After the conversion system uses the longitude
        /// </summary>
        public string Lon
        {
            get
            {
                DisplayLonConvert con = new DisplayLonConvert();
                return con.ConvertBack(_RecLon, null, null, null).ToString();
            }
        }

        private string _RecLat;
        /// <summary>
        ///Received latitude
        /// </summary>
        public string RecLat
        {
            get
            {
                return _RecLat;
            }
            set
            {
                _RecLat = value;
            }
        }
        private string _RecLon;
        /// <summary>
        /// Longitude received
        /// </summary>
        public string RecLon
        {
            get
            {
                return _RecLon;
            }
            set
            {
                _RecLon = value;
            }

        }
        /// <summary>
        /// speed 
        /// </summary>
        public string V { get; set; }
        /// <summary>
        /// Direction
        /// </summary>
        public string Dir { get; set; }

        /// <summary>
        /// GPS time
        /// </summary>
        public String GpsTime { get; set; }

        /// <summary>
        /// Gps coordinates are valid
        /// </summary>
        public string Valid { get; set; }
    }

    public class NotifyCancelVehicleState : EventArgs
    {
        public ElementLayerDefine DisplayType { get; set; }
        public string CarNo { get; set; }
    }
}
