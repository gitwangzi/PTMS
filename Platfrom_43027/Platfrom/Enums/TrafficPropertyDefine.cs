/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: f5c579ad-983d-4b75-82c8-00a1e19f4f64      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share.Enum
/////    Project Description:    
/////             Class Name: FENCE_Type
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/3 8:40:01
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/3 8:40:01
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

namespace Gsafety.PTMS.Bases.Enums
{
    public enum FENCE_RegionProperty
    {

        //0：根据时间；1：限速；2：进区域报警给驾驶员；3：进区域报警给平台；4：出区域报警给驾驶员；5：出区域报警给平台；6：北纬；7：南纬；8：东经；9：西经；
        /// <summary>
        /// 0
        /// </summary>
        Time_Limit=0,
        /// <summary>
        /// 1
        /// </summary>
        Speed_Limit=1,
        /// <summary>
        /// 2
        /// </summary>
        In_AlertToDriver=2,
        /// <summary>
        /// 3
        /// </summary>
        In_AlertToPlatform = 3,
        /// <summary>
        /// 4
        /// </summary>
        Out_AlertToDriver = 4,
        /// <summary>
        /// 5
        /// </summary>
        Out_AlertToPlatform = 5,
        /// <summary>
        /// 6
        /// </summary>
        North_Lat = 6,
        /// <summary>
        /// 7
        /// </summary>
        South_Lat = 7,
        /// <summary>
        /// 8
        /// </summary>
        East_Lon=8,
        /// <summary>
        /// 9
        /// </summary>
        West_Lon=9
    }

    public enum Route_RouteProperty
    {
        //0：根据时间；2：进路线报警给驾驶员；3：进路线报警给平台；4：出路线报警给驾驶员；5：出路线报警给平台；
        Time_Limit = 0,

        In_AlertToDriver = 2,
        In_AlertToPlatform = 3,
        Out_AlertToDriver = 4,
        Out_AlertToPlatform = 5,
    }

    public enum Route_RouteSegmentProperty
    {
        //1：限速；2：北纬；3：南纬；4：东经；5：西经；
        Speed_Limit = 1,

        North_Lat = 2,
        South_Lat = 3,
        East_Lon = 4,
        West_Lon = 5
    }
}
