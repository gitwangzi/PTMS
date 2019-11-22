using Gsafety.PTMS.Bases.Enums;
using Gsafety.Common.CommMessage.Controls;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 26e2641a-a3ad-4282-b75f-f1a257624729      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: MarkTrafficGraphic
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/23 18:15:05
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/23 18:15:05
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
using Gsafety.PTMS.ServiceReference.TrafficManageService;

namespace Gsafety.Common.CommMessage
{
    /// <summary>
    /// 标绘交通管理相关对象
    /// </summary>
    public class MarkTrafficGraphic
    {

        public TrafficFeature nType;
        /// <summary>
        /// 显示开关 true:false移除
        /// </summary>
        public bool bShow;
        /// <summary>
        /// 标绘的符号样式
        /// </summary>
        public SymbolParams MarkSymbolParm;


        public TrafficFence TrafficFence { get; set; }
        public TrafficRoute TrafficRoute { get; set; }
        public string parentId { get; set; }
        public string childId { get; set; }

    }
}
