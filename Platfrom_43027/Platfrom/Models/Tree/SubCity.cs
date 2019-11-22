/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: 5ccd5a54-5bc7-4243-a907-f93259668e12      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: TEST(JiangJ)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Bases.Models
/////    Project Description:    
/////             Class Name: SubCity
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/31 11:41:38
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/31 11:41:38
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
using System.Collections.ObjectModel;
using Jounce.Framework.Command;
using Gsafety.PTMS.Bases.Enums;
using System.Linq;
using Jounce.Core.Model;


namespace Gsafety.PTMS.Bases.Models
{
    public class SubCity : BaseNotify,IModelEntity
    {
        public BasicCity _basicC;
        public ObservableCollection<SubVehicleType> _VehicleTypes;

        public SubCity(BasicCity bc)
        {
            this._basicC = bc;
            this._VehicleTypes = new ObservableCollection<SubVehicleType>();
            CheckedCommand = new ActionCommand<object>((obj) => Check_Event(obj)); 
        }

        public string Name
        {
            get { return _basicC.Name; }
            set { _basicC.Name = value; }
        }

        public string Code
        {
            get { return _basicC.Code;}
            set { _basicC.Code = value; }
        }
        private Visibility _discriptionVisibility = Visibility.Visible;
        Visibility IModelEntity.DiscriptionVisibility
        {
            get { return _discriptionVisibility; }
            set { _discriptionVisibility = value; }
        }

        private Visibility _FunctionKeyVisibility = Visibility.Collapsed;
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
                foreach (var item in _VehicleTypes)
                {
                    item.IsChecked = _IsChecked;
                }
                RaisePropertyChanged(() => IsChecked);



            }
        }

        public Visibility IsVisibleToChoose
        {
            get
            {
                if (string.IsNullOrEmpty(VehiclesCount) || VehiclesCount == "0")
                    return Visibility.Collapsed;
                if (int.Parse(VehiclesCount) > 0)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }

        }

        public SubVehicle AddVehicle(BasicVehicle vehicle)
        {
            SubVehicleType vt = GetVehicleTypeInfo(vehicle);
            SubVehicle vehicleModle = new SubVehicle(vehicle);
            vehicleModle._basicV.VehicleId = vehicle.VehicleId;
            vehicleModle._basicV.MDVRSN = vehicle.MDVRSN;
            vehicleModle._basicV.BrandModel = vehicle.BrandModel;
            vehicleModle._basicV.EngineId = vehicle.EngineId;
            vehicleModle._basicV.Owner = vehicle.Owner;
            vehicleModle._basicV.StartYear = vehicle.StartYear;
            vehicleModle._basicV.VehicleSn = vehicle.VehicleSn;
            vehicleModle._basicV.CityCode = vehicle.CityCode;
            vehicleModle._basicV.CityName = vehicle.CityName;
            vehicleModle._basicV.ProvinceName = vehicle.ProvinceName;
            vehicleModle._basicV.IsOnLine = vehicle.IsOnLine == null ? false : vehicle.IsOnLine;
            vehicleModle._basicV.VehicleType = vt.VehicleType;

            vt.Vehicles.Add(vehicleModle);
            vt.refresh();

            return vehicleModle;
        }

        public SubVehicle GetVehicle(string vehicleID)
        {
            foreach (var vehicleTypes in _VehicleTypes)
            {
                SubVehicle vehicle = vehicleTypes.GetVehicle(vehicleID);
                if (vehicle != null)
                    return vehicle;
            }
            return null;
        }

        public bool RemoveVehicle(string MDVRID)
        {
            foreach (var vehicleTypes in _VehicleTypes)
            {
                if (vehicleTypes.RemoveVehicle(MDVRID))
                {
                    if (vehicleTypes.Vehicles.Count == 0)
                    {
                        return _VehicleTypes.Remove(vehicleTypes);
                    }
                    return true;
                }
            }
            return false;
        }

        private SubVehicleType GetVehicleTypeInfo(BasicVehicle vehicle)
        {
            VehicleType vtype = vehicle.VehicleType;
            
            SubVehicleType vehicleTypeInfo;
            if (_VehicleTypes != null && _VehicleTypes.Count > 0)
            {
                vehicleTypeInfo = _VehicleTypes.Where(item => item.VehicleType == vtype).FirstOrDefault();

                if (vehicleTypeInfo == null)
                {
                    vehicleTypeInfo = AddVehicleType(vtype);
                }
            }
            else
            {
                vehicleTypeInfo = AddVehicleType(vtype);
            }
            return vehicleTypeInfo;

        }

        private SubVehicleType AddVehicleType(VehicleType vehicleType)
        {
            SubVehicleType vehicleTypeInfo = new SubVehicleType(null);
            vehicleTypeInfo.VehicleType = vehicleType;
            vehicleTypeInfo.Name = vehicleType.ToString();
            if (_VehicleTypes.Count == 0)
            {
                _VehicleTypes.Add(vehicleTypeInfo);
            }
            else if (_VehicleTypes.Count == 2)
            {

                if (vehicleTypeInfo.VehicleType == VehicleType.Taxi)
                {
                    _VehicleTypes.Insert(0, vehicleTypeInfo);
                }
                else if (vehicleTypeInfo.VehicleType == VehicleType.Bus)
                {
                    _VehicleTypes.Insert(1, vehicleTypeInfo);
                }
                else
                {
                    _VehicleTypes.Add(vehicleTypeInfo);
                }

            }
            else
            {
                if (vehicleTypeInfo.VehicleType == VehicleType.Taxi)
                {
                    _VehicleTypes.Insert(0, vehicleTypeInfo);
                }
                else if (vehicleTypeInfo.VehicleType == VehicleType.Bus && _VehicleTypes[0].VehicleType == VehicleType.Flota)
                {
                    _VehicleTypes.Insert(0, vehicleTypeInfo);
                }
                else
                {
                    _VehicleTypes.Add(vehicleTypeInfo);
                }

            }
            return vehicleTypeInfo;
        }

        public string VehiclesDescription
        {
            get
            {
                return string.Format("({0}/{1})", VehiclesOnLineCount, VehiclesCount);
            }
        }

        /// <summary>
        /// 已有车辆
        /// </summary>
        public string VehiclesCount
        {
            get
            {
                if (_VehicleTypes == null)
                    return "0";
                Func<SubVehicleType, int> ans = (w => int.Parse(w.VehiclesCount));
                return string.Format("{0}", _VehicleTypes.Sum(ans));
            }
        }

        /// <summary>
        /// 在线数量
        /// </summary>
        public string VehiclesOnLineCount
        {

            get
            {
                if (_VehicleTypes == null)
                    return "0";
                Func<SubVehicleType, int> ans = (w => int.Parse(w.VehiclesOnLineCount));
                return string.Format("{0}", _VehicleTypes.Sum(ans));
            }

        }

        public void refresh()
        {
            RaisePropertyChanged(() => VehiclesCount);
            RaisePropertyChanged(() => VehiclesOnLineCount);
            RaisePropertyChanged(() => VehiclesDescription);
            //if (null != Parent) { Parent.refresh(); }
        }

        public ObservableCollection<IModelEntity> GetChilds()
        {
            return new ObservableCollection<IModelEntity>(_VehicleTypes);
        }

        private void Check_Event(object obj)
        {
            if (IsChecked)
            {
                foreach (var item in _VehicleTypes)
                {
                    item.CheckedCommand.Execute(obj);
                }
            }
        }
    }
}
