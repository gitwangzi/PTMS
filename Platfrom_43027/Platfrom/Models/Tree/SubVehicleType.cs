/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: e79be883-074b-4712-96ee-1651e962ae31      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: TEST(JiangJ)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Bases.Models
/////    Project Description:    
/////             Class Name: SubVehicleType
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/31 12:53:43
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/31 12:53:43
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
using System.Linq;
using System.Collections.ObjectModel;
using Jounce.Core.Model;
using Gsafety.PTMS.Bases.Enums;

namespace Gsafety.PTMS.Bases.Models
{
    public class SubVehicleType:BaseNotify,IModelEntity
    {
        public SubCity _parent;
        public BasicVehicleType _basicVT;
        public ObservableCollection<SubVehicle> Vehicles { get; set; }

        public VehicleType VehicleType 
        {
            get { return _basicVT.VehicleType; } 
            set { _basicVT.VehicleType=value; } 
        }

        public string Name
        {
            get { return _basicVT.Name; }
            set { _basicVT.Name = value; }
        }

        public string Code
        {
            get;
            set;
        }

        public SubVehicleType( BasicVehicleType bvt)
        {
            _basicVT = bvt;
            Vehicles = new ObservableCollection<SubVehicle>();
            CheckedCommand = new ActionCommand<object>((obj) => Check_Event(obj));
        }

        bool _isExpanded;
        bool _isSelected;
        bool _IsChecked;

        public bool IsExpanded
        {
            get
            {
                return this._isExpanded;
            }
            set
            {
                this._isExpanded = value;
                RaisePropertyChanged(() => this.IsExpanded);
            }
        }

        public bool IsSelected
        {
            get
            {
                return this._isSelected;
            }
            set
            {
                this._isSelected = value;
                RaisePropertyChanged(() => this.IsSelected);
            }
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
                RaisePropertyChanged(() => IsChecked);

                foreach (var item in Vehicles)
                {
                    item.IsChecked = IsChecked;
                }
            }
        }

        public bool IsMonitor
        {
            get
            {
                if (Vehicles == null || Vehicles.Count == 0)
                {
                    return false;
                }
                else
                {

                    foreach (var item in Vehicles)
                    {
                        if (item._basicV.IsMonitor == null || (bool)item._basicV.IsMonitor)
                            return false;
                    }
                }
                return true;
            }
        }

        public string VehiclesDescription
        {
            get
            {
                return string.Format("({0}/{1})", VehiclesOnLineCount, VehiclesCount);
            }
        }

        /// <summary>
        /// VehiclesCount
        /// </summary>
        public string VehiclesCount
        {
            get
            {
                if (Vehicles == null)
                    return "0";
                return string.Format("{0}", Vehicles.Count);
            }
        }

        /// <summary>
        /// VehiclesOnLineCount
        /// </summary>
        public string VehiclesOnLineCount
        {

            get
            {
                if (Vehicles == null)
                    return "0";
                return string.Format("{0}", Vehicles.Where(item => item._basicV.IsOnLine).Count());
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

        public ICommand CheckedCommand;


        private void Check_Event(object sender)
        {
            if (IsChecked)
            {
                if (sender.ToString().Equals("0"))
                {
                    foreach (var item in Vehicles)
                    {
                        item.IsChecked = false;
                    }
                }
            }
        }

        public bool RemoveVehicle(string MdvrID)
        {
            SubVehicle vehicle = Vehicles.Where(item => item._basicV.MDVRSN.Trim().Equals(MdvrID)).FirstOrDefault();
            if (null != vehicle)
            {
                bool isok = Vehicles.Remove(vehicle);
                this.refresh();
                return isok;
            }
            return false;
        }

        public SubVehicle GetVehicle(string vehicleId)
        {
            return Vehicles.Where(item => item._basicV.VehicleId.Equals(vehicleId)).FirstOrDefault();
        }

        public SubVehicle GetMdvrIDVehicle(string MdvrID)
        {
            return Vehicles.Where(item => item._basicV.MDVRSN.Trim().Equals(MdvrID)).FirstOrDefault();
        }

        internal void refresh()
        {
            RaisePropertyChanged(() => VehiclesCount);
            RaisePropertyChanged(() => VehiclesOnLineCount);
            RaisePropertyChanged(() => VehiclesDescription);
            if (null != _parent) { _parent.refresh(); }
        }


        public ObservableCollection<IModelEntity> GetChilds()
        {
            return new ObservableCollection<IModelEntity>(Vehicles);
        }
    }
}
