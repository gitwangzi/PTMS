
using Gsafety.PTMS.Bases.Enums;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3c4454a8-e1d7-406f-82cb-4cc301114b64      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: ClearTrafficFeaturelayer
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/27 15:51:13
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/27 15:51:13
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

namespace Gsafety.Common.CommMessage
{
    /// <summary>
    /// ClearTrafficFeaturelayerselection
    /// </summary>
    public class ClearTrafficFeaturelayer
    {
        /// <summary>
        /// layer type0fence1route2station
        /// </summary>
        private  TrafficFeature _enumFeatureType = TrafficFeature.Traffic_PolygonFence;
        public TrafficFeature nType
        {
            get { return _enumFeatureType; }
            set { _enumFeatureType = value; }
        }
        /// <summary>
        /// clear over ,the layer whether see or can,t see
        /// </summary>
        private bool _bVisble = true;
        public bool bLayerVisble
        {
            get { return _bVisble; }
            set { _bVisble = value; }
        }
    }
}
