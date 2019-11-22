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

using Gsafety.PTMS.ServiceReference.AppConfigService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.AppConfig.Views
{
    public partial class ConfigSettingValue_Control : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<ConfigTree> _dataModel;
        public ObservableCollection<ConfigTree> ItemSource
        {
            get { return _dataModel; }
            set
            {
                _dataModel = value;
                ChangedItems.Clear();
                LoadData();
            }

        }

        public List<ConfigTree> ChangedItems { get; private set; }
        
        public ConfigTree SelectedValue { get; private set; }

        public ConfigSettingValue_Control()
        {            
            InitializeComponent();
            ChangedItems = new List<ConfigTree>();
        }


        private void LoadData()
        {
            this.LayoutRoot.Children.Clear();
            foreach (var x in this.ItemSource)
            {
                var item = new ConfigSettingValue_ControlItem(this);
                item.DataModel = x;
                item.MouseLeftButtonUp += (s, e) =>
                    {
                        this.SelectedValue = x;
                        if (PropertyChanged != null)
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("SelectedValue"));
                        }
                    };
                this.LayoutRoot.Children.Add(item);
            }
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
