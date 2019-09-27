/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a9ad1728-b991-4384-b8bf-bd1699c22c25      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: GisManagement
/////    Project Description:    
/////             Class Name: GisBinding
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/5 13:40:05
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/5 13:40:05
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
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
using Jounce.Core.ViewModel;

namespace GisManagement
{
    public class GisBinding
    {

        //[Export]
        //public ViewModelRoute Binding22
        //{
        //    get { return ViewModelRoute.Create(GisName.GisViewModel, GisName.gis2); }
        //}

        [Export]
        public ViewModelRoute BindingGIS
        {
            get { return ViewModelRoute.Create(GisName.GisViewModel, GisName.MonitorGisView); }
        }

        [Export]
        public ViewModelRoute BindingSpatialQuery
        {
            get { return ViewModelRoute.Create(GisName.SpatialQueryViewModel, GisName.SpatialQuery); }
        }

        [Export]
        public ViewModelRoute BindingGPSList
        {
            get { return ViewModelRoute.Create(GisName.GpsCarListViewModel, GisName.GpsCarList); }
        }

        [Export]
        public ViewModelRoute BindingGPSHis
        {
            get { return ViewModelRoute.Create(GisName.GpsCarHisDataViewModel, GisName.GpsCarHisDataViewMonitor); }
        }


        //[Export]
        //public ViewModelRoute BindingAntProductGISView
        //{
        //    get
        //    {
        //        return ViewModelRoute.Create(GisName.GisViewModel, GisName.AntProductMonitorGisV);
        //    }
        //}
    }
}
