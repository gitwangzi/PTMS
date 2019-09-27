using Gsafety.Common.Utilities;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 725f3305-e91c-4617-9aa6-524fc62f294e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Views
/////    Project Description:    
/////             Class Name: BaseInfoMenu
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/9 10:14:24
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/9 10:14:24
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.BaseInformation.Views
{
    public partial class BaseInfoMenu : UserControl
    {
        List<HyperlinkButton> _NavigationButton;

        public List<HyperlinkButton> NavigationButtons
        {
            get
            {
                if (_NavigationButton == null || _NavigationButton.Count == 0)
                {
                    VisualTreeExtedHelper vtHelper = new VisualTreeExtedHelper();
                    _NavigationButton = vtHelper.GetChildObjects<HyperlinkButton>(this.LayoutRoot, "");
                }
                return _NavigationButton;
            }
        }

        public BaseInfoMenu()
        {
            InitializeComponent();
        }
    }
}
