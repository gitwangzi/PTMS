/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e5791c77-9156-4ad9-8a4b-f5f868d9ab98      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share
/////    Project Description:    
/////             Class Name: NavigateManage
/////          Class Version: v1.0.0.0
/////            Create Time: 7/24/2013 5:35:01 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/24/2013 5:35:01 PM
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
using System.Linq;

namespace Gsafety.PTMS.Share
{
    public class NavigateManage
    {
        
        private List<Frame> _Frames = new List<Frame>();

        public NavigateManage()
        { 
        }

        public void AddFrame(Frame frame)
        {
            if (frame == null || string.IsNullOrEmpty(frame.Name))
                return;
            if (_Frames.Count == 0)
                _Frames.Add(frame);
            else
            {
                if (_Frames.Where(item => item.Name.Equals(frame.Name)).Count() > 0)
                    return;
                _Frames.Add(frame);
            }
        }

        public void Navigate(string viewName, string frameName)
        {
            var frame = _Frames.Where(item => item.Name.Equals(frameName)).FirstOrDefault();
            if (frame != null)
                (frame as Frame).Navigate(new Uri(string.Format("/{0}",viewName),UriKind.Relative));
        }
    }
}
