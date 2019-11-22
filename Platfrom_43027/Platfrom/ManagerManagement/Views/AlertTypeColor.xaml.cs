/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 781be104-be28-4f6e-936f-4133c0c82626      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views
/////    Project Description:    
/////             Class Name: AlertTypeColor
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/15 18:49:50
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/15 18:49:50
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.ServiceReference.AppConfigService;
using Gsafety.PTMS.Share;
using Gsafety.Common.CommMessage;
namespace Gsafety.PTMS.Manager.Views
{
    public partial class AlertTypeColor : ChildWindow
    {
        AppConfigManagerClient appClient = ServiceClientFactory.Create<AppConfigManagerClient>();
        private string SceName = string.Empty;
        public AlertTypeColor(string desc, string color, string secName)
        {

            InitializeComponent();
            RouteColor.DataContext = PredefinedColors.PredefinedColorCollection;
            this.Desc.Text = ApplicationContext.Instance.StringResourceReader.GetString(desc);
            SceName = secName;
            RentColor = PredefinedColors.PredefinedColorCollection.Where(x => x.Name == color).FirstOrDefault();
            RouteColor.SelectedItem = RentColor;
            appClient.UpdateAllSectionCompleted += appClient_UpdateAllSectionCompleted;
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;

        }
        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        void appClient_UpdateAllSectionCompleted(object sender, UpdateAllSectionCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            if (e.Result.Result)
            {
                ResultAction(SceName, RentColor.Name);
            }
        }
        private PredefinedColor _RentColor;

        public PredefinedColor RentColor
        {
            get { return _RentColor; }
            set { _RentColor = value; }
        }
        public Action<string, string> ResultAction;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            RentColor = this.RouteColor.SelectedItem as PredefinedColor;
            dic.Add(SceName, RentColor.Name);
            appClient.UpdateAllSectionAsync(dic);
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}

