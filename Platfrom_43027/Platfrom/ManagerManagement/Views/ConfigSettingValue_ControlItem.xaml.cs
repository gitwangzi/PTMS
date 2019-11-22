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
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Manager.Model;
using Gsafety.PTMS.Manager;

namespace Gsafety.PTMS.AppConfig.Views
{
    public partial class ConfigSettingValue_ControlItem : UserControl
    {
        public ConfigSettingValue_ControlItem(ConfigSettingValue_Control parent)
            : this()
        {
            Value_Control = parent;
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        public  ConfigSettingValue_Control Value_Control { get; private set; }

        private SectionTypeModel _typeControlMode;

        private ConfigTree _dataModel;
        public ConfigTree DataModel
        {
            get { return _dataModel; }
            set
            {
                
                _dataModel = value;
                this.LayoutRoot.DataContext = value;
                LoadData();
            }

        }

        private void ChildWindow_MouseRightButtonDown(object sender,

      System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        public ConfigSettingValue_ControlItem()
        {
            InitializeComponent();
            _typeControlMode = new SectionTypeModel();
        }

        private void LoadData()
        {
            if (this.DataModel == null) return;
            var typeItem = (TypeModelItem)_typeControlMode.FindItem(this.DataModel.Value.SectionType);
           var define =(ItemTypeDefine)Activator.CreateInstance(typeItem.Control);
           var control = define.CreateControl(typeItem.Tag,DataModel.Value.SectionValue,x =>
           {
               DataModel.Value.SectionValue = x;
               if (this.Value_Control != null)
               {
                   if (!this.Value_Control.ChangedItems.Any(y => y.Value.Id == this.DataModel.Value.Id))
                   {
                       this.Value_Control.ChangedItems.Add(this.DataModel);
                   }
               }
           });
            
           this.sp_valueContiner.Children.Add(control);
        }
    }
}
