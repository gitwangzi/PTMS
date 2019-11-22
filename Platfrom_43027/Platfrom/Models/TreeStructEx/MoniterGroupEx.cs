using Jounce.Core.Model;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 31b81bf7-b466-4631-9b55-727278dedecb      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Bases.Models.TreeStructEx
/////    Project Description:    
/////             Class Name: MoniterGroupEx
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/7 11:29:43
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/7 11:29:43
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.ObjectModel;
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
namespace Gsafety.PTMS.Bases.Models
{
    public class MoniterGroupEx : BaseNotify, IMonitorEntity
    {

        #region Fields

        private Visibility _DiscriptionVisibility;
        private Visibility _FunctionKeyVisibility;
        private Visibility _CheckVisibility;
        private ICommand _CheckedComand;
        private bool _IsChecked;
        private ObservableCollection<VehicleEx> _VehicleExs;
        private string _GroupName;

        #endregion

        #region Attributes

        public Visibility DiscriptionVisibility
        {
            get
            {
                return _DiscriptionVisibility;
            }
            set
            {
                _DiscriptionVisibility = value;
            }
        }

        public Visibility FunctionKeyVisibility
        {
            get
            { return Visibility.Collapsed; }
            set
            { _FunctionKeyVisibility = value; }
        }

        public Visibility CheckVisibility
        {
            get
            {
                return _CheckVisibility;
            }
            set
            {
                _CheckVisibility = value;
            }
        }
        public string GroupName
        {
            get
            {
                return _GroupName;
            }
            set
            {
                if (value != _GroupName)
                {
                    _GroupName = value;
                    RaisePropertyChanged(() => GroupName);
                }
            }
        }
        public string Name
        {
            get
            {
                return GroupInfo.GroupName;
            }
        }

        public ICommand CheckedCommand
        {
            get
            {
                return _CheckedComand;
            }
            set
            {
                _CheckedComand = value;
            }
        }
        public ObservableCollection<VehicleEx> Vehicles
        {
            get { return _VehicleExs; }
            set { _VehicleExs = value; }
        }
        public bool IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                _IsChecked = value;
            }
        }
        public Visibility IsVisibleToChoose
        {
            get
            {
                if (_VehicleExs == null || _VehicleExs.Count == 0)
                    return Visibility.Collapsed;
                if (_VehicleExs.Count() > 0)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        MoniterGroupEx GroupInfo;
        #endregion
        public MoniterGroupEx()
        {
            DiscriptionVisibility = Visibility.Visible;
            FunctionKeyVisibility = Visibility.Collapsed;
        }
        public MoniterGroupEx(MoniterGroupEx group, ObservableCollection<VehicleEx> _vehicleExs, Visibility _isvisibleToChoose)
        {
            GroupInfo = group;
            ID = GroupInfo.ID;
            Vehicles = _vehicleExs;
            foreach (var item in Vehicles)
            {
                //item.IsVisibleToChoose = _isvisibleToChoose;
            }
            DiscriptionVisibility = Visibility.Visible;
            FunctionKeyVisibility = Visibility.Collapsed;
        }
        public System.Collections.ObjectModel.ObservableCollection<IMonitorEntity> GetChilds()
        {
            return new ObservableCollection<IMonitorEntity>(_VehicleExs);
        }

        public string Note { get; set; }

        public string CreateUser { get; set; }

        public short? GroupIndex { get; set; }

        public string ID
        {
            get;
            set;
        }
    }
}
