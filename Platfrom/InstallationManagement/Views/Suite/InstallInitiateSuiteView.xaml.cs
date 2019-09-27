using Gsafety.PTMS.Constants;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2192fb41-8374-47f7-be7f-5465bf737913      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Views
/////    Project Description:    
/////             Class Name: DeviceSelftestView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/17 14:25:27
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/17 14:25:27
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.InstallInitiateSuiteV)]
    [ExportViewToRegion(InstallationName.InstallInitiateSuiteV, ViewContainer.InstallContainer)]
    public partial class InstallInitiateSuiteView : UserControl
    {
        private Dictionary<string, CheckBox> _checkDic = new Dictionary<string, CheckBox>();

        public InstallInitiateSuiteView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(dataGrid);
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(CameraListDataGrid);
            _checkDic = busCanvas.Children.Select(t => t as CheckBox).ToDictionary(t => t.Content as string, t => t);

            this.Loaded += InstallInitiateSuiteView_Loaded;
        }

        void InstallInitiateSuiteView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (var item in _checkDic.Values)
            {
                item.IsChecked = false;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach(string item in e.AddedItems)
            {
                if(string.IsNullOrEmpty(item))
                {
                    continue;
                }
                _checkDic[item].IsChecked = true;
            }

            foreach(string item in e.RemovedItems)
            {
                if(string.IsNullOrEmpty(item))
                {
                    continue;
                }
                _checkDic[item].IsChecked = false;
            }
        }
    }
}
