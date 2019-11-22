/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ebe8e57b-7f1d-449c-b241-a8ae8d3d0a45      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGJINCAI
/////                 Author: TEST(JinCaiWang)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract.Data
/////    Project Description:    
/////             Class Name: AlarmInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 11:07:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/8 11:07:41
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
using System.Windows.Navigation;
 
using System.Collections.ObjectModel;
using Gsafety.PTMS.ServiceReference.AppConfigService;
using Jounce.Core.View;
using Jounce.Regions.Core;
using Gsafety.PTMS.Manager;
using Gsafety.PTMS.Share;

namespace Gsafety.PTMS.AppConfig.Views
{
    //  [
    //ExportAsView(
    //    ManagerName.SettingAppConfigValue
    //    , Category = ManagerName.CategoryName
    //    , MenuName = ManagerName.SettingManageMenuName
    //    , MenuTitle = "MANAGER_SettingAppConfigValue"
    //    , ToolTip = "Click to view some text."
    //    , Url = "/SettingAppConfigValue"
    //    , Order = 2
    //    ),
    //ExportViewToRegion(
    //       ManagerName.SettingAppConfigValue
    //       , ManagerName.ManagerContainer
    //       )

    //  ]
    public partial class SettingAppConfigValue : Page
    {
        private AppConfigManagerClient _client;

        private ObservableCollection<ConfigTree> _dataModel;
        public ObservableCollection<ConfigTree> DataModel
        {
            get { return _dataModel; }
            set
            {
                _dataModel = value;
                this.LayoutRoot.DataContext = value;
            }

        }
        private bool _isInitPage;


        public SettingAppConfigValue()
        {
            InitializeComponent();
            Loaded += SettingAppConfigValue_Loaded;
            DataModel = new ObservableCollection<ConfigTree>();

        }

        void SettingAppConfigValue_Loaded(object sender, RoutedEventArgs e)
        {
            InitializePage();
        }


        private void InitializePage()
        {
            if (!_isInitPage)
            {
                _client = ServiceClientFactory.Create<AppConfigManagerClient>();
                LoadDbData();
            }
            _isInitPage = true;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            InitializePage();
        }

        private void LoadDbData()
        {
            this.bi_root.IsBusy = true;
            _client.GetAllSectionsCompleted += (s, args) =>
            {
                if (args.Result != null)
                {

                    DataModel = args.Result;
                }

                Dispatcher.BeginInvoke(() => this.bi_root.IsBusy = false);

            };
            _client.GetAllSectionsAsync();

        }

        private void lb_cata_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var currentItem = (ConfigTree)lb_cata.SelectedValue;
            if (currentItem == null)
            {
                return;
            }
            cfv_detail.ItemSource = currentItem.Children;

        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            this.bi_root.IsBusy = this.cfv_detail.ChangedItems.Count > 0;
            _client.UpdateSectionCompleted += (s, arg) =>
            {
                this.cfv_detail.ChangedItems.Remove((ConfigTree)arg.UserState);
                this.bi_root.IsBusy = this.cfv_detail.ChangedItems.Count > 0;
            };
            for (int i = this.cfv_detail.ChangedItems.Count - 1; i >= 0; i--)
            {
                _client.UpdateSectionAsync(this.cfv_detail.ChangedItems[i], this.cfv_detail.ChangedItems[i]);
                
            }
        }

    }
}
