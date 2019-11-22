/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: 98ce11e0-e22b-447d-8b17-e78c74e54793      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: TEST(JiangJ)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Bases.Models
/////    Project Description:    
/////             Class Name: SubProvice
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/31 10:42:59
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/31 10:42:59
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

namespace Gsafety.PTMS.Bases.Models
{
    public class SubProvice :  BaseNotify ,IModelEntity
    {
        public ObservableCollection<SubCity> _Citys;
        public BasicProvince _basicP;

        public SubProvice(BasicProvince bp)
        {
            _basicP = bp;
            _Citys = new ObservableCollection<SubCity>();
            CheckCommand=new ActionCommand<object>((obj) => Check_Event(obj));
        }

        public string Name
        {
            get { return _basicP.Name; }
            set { _basicP.Name = value; }
        }

        public string Code
        {
            get { return _basicP.Code; }
            set { _basicP.Code = value; }
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

        public ICommand CheckCommand;
       
        private bool _isExpanded;
        private bool _isSelected;
        bool _IsChecked;
          
        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                _isExpanded = value;
                RaisePropertyChanged(() => IsExpanded);
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
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
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
                foreach (var item in _Citys)
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
                if (_Citys == null || _Citys.Count == 0)
                    return Visibility.Collapsed;
                if (_Citys.Where(item => item.IsVisibleToChoose == Visibility.Visible).Count() > 0)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        private void Check_Event(object obj)
        {
            if (IsChecked)
            {
                foreach (var item in _Citys)
                {
                    item.CheckedCommand.Execute(obj);
                }
            }
        }

        public SubVehicle GetVehicle(string vehicleId)
        {
            foreach (var city in _Citys)
            {
                SubVehicle vehicle = city.GetVehicle(vehicleId);
                if (vehicle != null)
                    return vehicle;
            }
            return null;
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
                if (_Citys == null)
                    return "0";
                Func<SubCity, int> ans = (w => int.Parse(w.VehiclesCount));
                return string.Format("{0}", _Citys.Sum(ans));
            }
        }

        /// <summary>
        /// VehiclesOnLineCount
        /// </summary>
        public string VehiclesOnLineCount
        {

            get
            {
                if (_Citys == null)
                    return "0";
                Func<SubCity, int> ans = (w => int.Parse(w.VehiclesOnLineCount));
                return string.Format("{0}", _Citys.Sum(ans));
            }
        }

        public void UiRefresh()
        {
            RaisePropertyChanged(() => VehiclesCount);
            RaisePropertyChanged(() => VehiclesOnLineCount);
            RaisePropertyChanged(() => VehiclesDescription);
            //ApplicationContext.Instance.BufferManager.DistrictManager.refresh();
        }

        public ObservableCollection<IModelEntity> GetChilds()
        {
            return new ObservableCollection<IModelEntity>(_Citys);
        }
    }
}
