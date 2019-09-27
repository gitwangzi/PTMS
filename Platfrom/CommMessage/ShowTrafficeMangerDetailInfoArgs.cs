/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: eef1d1a4-f263-4804-846c-a8977ab1b787      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: ShowTrafficeMangerDetailInfoArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/25 11:16:02
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/25 11:16:02
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
    public class ShowTrafficeMangerDetailInfoArgs
    {
        private bool _isSpeedClick = false;
        public bool bShow { get; set; }

        public bool IsSpeedClick
        {
            get { return _isSpeedClick; }
            set
            {
                _isSpeedClick = value;
            }
        }

    }
}
