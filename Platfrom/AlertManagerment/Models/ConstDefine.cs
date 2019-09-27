/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 003234ef-1a30-41da-9ed4-0355f6c786eb      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Models
/////    Project Description:    
/////             Class Name: ConstDefine
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/26 10:58:13
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/26 10:58:13
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

namespace Gsafety.PTMS.Alert.Models
{
    /// <summary>
    /// constant definition in GIS system
    /// </summary>
    public class ConstDefine
    {
        public static string cmType = "CmType";
        public static string Normal = "Normal";

        /// <summary>
        /// in the animation process,draw the route time interval,when a large number of vehicles need
        /// to draw a line ,this time should be adjusted longer
        /// </summary>
        public static int UpdateDisplayInterval = 200;
        /// <summary>
        /// The Maximum record showed in the log
        /// </summary>
        public static int MaxRecordNum = 100;
        ///// <summary>
        ///// field of vision when automatic positioning
        ///// </summary>
        //public static double AutoLocateResolution = 28;
        ///// <summary>
        ///// Minimum vision
        ///// </summary>
        //public static double MinResolution = 1.75;
        /// <summary>
        /// The minimum offset value recorded history field of vision
        /// </summary>
        public static double MinOffsetFactorToWriteHistroy = 0.01;
        /// <summary>
        ///  Temporary drawing layer such as length measurement
        /// </summary>
    }

    public static class TranslateInfo
    {
        public static string GpsTime = "GpsTime";
        public static string CarNo = "CarNo";
        public static string Speed = "Speed";
        public static string Lon = "Lon";
        public static string Lat = "Lat";
        public static string Dir = "Dir";
        public static string FilterField = "FilterField";

        public static string Animation = "Animation";
        public static string Trajectory = "Trajectory";
        public static string interval = "interval";
        public static string MDVRID = "MDVRID";
        public static string ANTGPSID = "ANTGPSID";
        public static string RecDateTime = "RecDateTime";


        public static string GpsValid = "GpsValid";
        public static string GpsInValid = "GpsInValid";
        public static string NonGps = "NonGps";
        public static string ScaleValid = "ScaleValid";

        public static string QueryFindError = "QueryFailed";
        public static string ReadQueryConfigError = "ReadQueryConfigError";
        public static string SearchTextIsNull = "SearchTextIsNull";
        public static string AllProv = "AllProv";
        public static string RemoveCarTip = "RemoveCarTip";
        public static string Tip = "Tip";
        public static string RecMessage = "RecMessage";

        public static string SendMessage = "SendMessage";
        public static string Province = "Province";
        public static string City = "City";
        public static string Company = "Company";

        public static string Translate(string str)
        {
            //SR sr = new SR();
            //return sr.GetString(str, System.Threading.Thread.CurrentThread.CurrentCulture);
            //return ApplicationContext.Instance.StringResourceReader.GetString(str);
            return null;
        }


        public static string BtnClearDisplayTip = "ClearDisplayTip";

        public static string BtnGlobeTip = "GlobeTip";

        public static string BtnNextExtentTip = "NextExtentTip";

        public static string BtnPrevExtentTip = "PrevExtentTip";

        public static string BtnAreaTip = "AreaTip";

        public static string BtnLengthTip = "LengthTip";

        public static string InValidChar = "InValidChar";

        public static string TooShortInput = "TooShortInput";

        public static string BtnOverMap = "OverMapTip";

        public static string FullScreenTip = "FullScreenTip";
    }
}
