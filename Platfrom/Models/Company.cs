///////Copyright (C) Gsafety 2013 .All Rights Reserved.
///////======================================================================
///////                   Guid: 185ecba4-c64a-423b-96b7-839cf02204d0      
///////             clrversion: 4.0.30319.17929
///////Registered organization: Microsoft
///////           Machine Name: PC-DENGZL
///////                 Author: TEST(dengzl)
///////======================================================================
///////           Project Name: Gsafety.PTMS.Share
///////    Project Description:    
///////             Class Name: Company
///////          Class Version: v1.0.0.0
///////            Create Time: 8/12/2013 9:57:44 AM
///////      Class Description:  
///////======================================================================
///////          Modified Time: 8/12/2013 9:57:44 AM
///////            Modified by:
///////   Modified Description: 
///////======================================================================
//using System;
//using System.Collections.ObjectModel;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
//using System.Linq;

//namespace Gsafety.PTMS.Bases.Models
//{
//    public class Company : MonitorEntity
//    {
//        bool _IsChecked;

//        #region Attributes

//        public bool IsExpanded { get; set; }
//        public bool IsSelected { get; set; }
//        public City Parent { get; set; }
//        public ObservableCollection<Vehicle> Vehicles { get; set; }

//        public bool IsMonitor 
//        { 
//            get
//            {
//                if (Vehicles == null || Vehicles.Count == 0)
//                {
//                    return false;
//                }
//                else
//                {

//                    foreach (var item in Vehicles)
//                    {
//                        if ( item.IsMonitor == null || (bool)item.IsMonitor)
//                            return false;
//                    }
//                }
//                return true;
//            }
//        }

//        /// <summary>
//        /// VehiclesOnLineCount
//        /// </summary>
//        public string VehiclesOnLineCount
//        {

//            get
//            {
//                if (Vehicles == null)
//                    return "0";
//                return string.Format("{0}", Vehicles.Where(item => item.IsOnLine).Count());
//            }

//        }

//        /// <summary>
//        /// VehiclesCount
//        /// </summary>
//        public string VehiclesCount
//        {
//            get
//            {
//                if (Vehicles == null)
//                    return "0";
//                return string.Format("{0}", Vehicles.Count);
//            }
//        }

//        public string VehiclesDescription
//        {
//            get
//            {
//                return string.Format("({0}/{1})", VehiclesOnLineCount, VehiclesCount);
//            }
//        }

//        public override bool IsChecked
//        {
//            get
//            {
//                return _IsChecked;
//            }
//            set
//            {
//                _IsChecked = value;
//                RaisePropertyChanged(() => IsChecked);
//                foreach (var item in Vehicles)
//                {
//                    item.IsChecked = IsChecked;
//                }
//            }
//        }

//        public Visibility IsVisibleToChoose
//        {
//            get
//            {
//                if(Vehicles == null || Vehicles.Count == 0)
//                    return Visibility.Collapsed;
//               if(Vehicles.Where(item=> item.IsVisibleToChoose == Visibility.Visible).Count() > 0)
//                   return Visibility.Visible;
//                return Visibility.Collapsed;
//            }
           
//        }

//        #endregion

//        public Company()
//        {
//            Vehicles = new ObservableCollection<Vehicle>();
//            DiscriptionVisibility = Visibility.Visible;
//            FunctionKeyVisibility = Visibility.Collapsed;
//            CheckVisibility = Visibility.Visible;
//        }
       

//        public Vehicle GetVehicle(string vehicleId)
//        {
//            return Vehicles.Where(item => item.VehicleId.Equals(vehicleId)).FirstOrDefault();
//        }

//        public override ObservableCollection<MonitorEntity> GetChilds()
//        {
//            return new ObservableCollection<MonitorEntity>(Vehicles);
//        }

//        public override ObservableCollection<MonitorEntity> GetCompanyChilds()
//        {
//            return new ObservableCollection<MonitorEntity>(Vehicles);
//        }

//        internal void refresh()
//        {
//            RaisePropertyChanged(() => VehiclesCount);
//            RaisePropertyChanged(() => VehiclesOnLineCount);
//            RaisePropertyChanged(() => VehiclesDescription);
//            if (null != Parent) { Parent.refresh(); }
//        }


//    }
//}
