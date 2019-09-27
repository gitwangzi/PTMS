/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: 83c68fbe-8327-4e28-adb8-0effd6b1399f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: TEST(JiangJ)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Bases.Models
/////    Project Description:    
/////             Class Name: SubVehicle
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/31 12:54:23
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/31 12:54:23
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
using Jounce.Framework.Command;
using System.Collections.ObjectModel;
using Jounce.Core.Model;
using Gsafety.PTMS.Bases.Enums;
namespace Gsafety.PTMS.Bases.Models
{
    public class SubVehicle:BaseNotify,IModelEntity
    {
        public BasicVehicle _basicV;
        public SubVehicle(BasicVehicle bv)
        {
            _basicV = bv;
            CheckedCommand = new ActionCommand<object>((obj) => Check_Event(obj));
        }

        public string Name
        {
            get { return _basicV.Name; }
            set{ }
        }

        public string Code
        {
            get { return "nocode"; }
            set { }
        }

        public VehicleType VehicleType { get { return _basicV.VehicleType; } set { _basicV.VehicleType = value; } }

        public bool IsOnLine
        {
            get { return _basicV.IsOnLine; }
            set
            {
                _basicV.IsOnLine = value;
            }
        }

        private Visibility _discriptionVisibility = Visibility.Visible;
        Visibility IModelEntity.DiscriptionVisibility
        {
            get { return _discriptionVisibility; }
            set { _discriptionVisibility = value; }
        }

        private Visibility _FunctionKeyVisibility = Visibility.Visible;
        Visibility IModelEntity.FunctionKeyVisibility
        {
            get { return _FunctionKeyVisibility; }
            set { _FunctionKeyVisibility = value; }
        }

        private Visibility _checkVisibility = Visibility.Collapsed;
        Visibility IModelEntity.CheckVisibility
        {
            get { return _checkVisibility; }
            set { _checkVisibility = value; }
        }

        private void Check_Event(object sender)
        {
            if ((bool)IsChecked)
            {
                IsChecked = false;
            }
            else
            {
                IsChecked = true;
            }
        }

        public ICommand CheckedCommand;

        private bool _IsChecked = false;
        public bool IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                _IsChecked = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }

        private bool _IsExpanded = false;
        public bool IsExpanded
        {
            get
            {
                return _IsExpanded;
            }
            set
            {
                _IsExpanded = value;
                RaisePropertyChanged(() => _IsExpanded);
            }
        
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }

        
        public new ObservableCollection<IModelEntity> GetChilds()
        {
            return null;
        }
    }
}
