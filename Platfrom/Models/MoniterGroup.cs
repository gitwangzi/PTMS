/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5cc97143-360f-4e71-9ef4-6705d1cad67d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZANGDS
/////                 Author: TEST(zangds)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Monitor.Models
/////    Project Description:    
/////             Class Name: MoniterGrop
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/13 10:10:30
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/13 10:10:30
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
using Jounce.Core.Model;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Gsafety.PTMS.Bases.Models
{
    public class MoniterGroup : BaseNotify,IMonitorEntity
    {
        private string _GroupName;
        private string m_flag;


        public string CreateUser { get; set; }
        public short? GroupIndex { get; set; }

        public string ID { get; set; }
        private string _Note;

        [RegularExpression(@"^.{0,200}", ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
      ErrorMessageResourceName = "MONITOR_GroupNodeConditionDetail")]
        public string Note
        {
            get { return _Note; }
            set
            {

                Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "Note" });
                _Note = value;
                RaisePropertyChanged(() => GroupName);

            }
        }

        [Required(ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
            ErrorMessageResourceName = "MONITOR_RequiredError")]

        [RegularExpression(@"^.{1,20}", ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
             ErrorMessageResourceName = "MONITOR_GroupNameConditionDetail")]
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

                    Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "GroupName" });
                    _GroupName = value;
                    RaisePropertyChanged(() => GroupName);
                }
            }
        }

        public  string Name
        {
            get { return _GroupName; }

        }

        private ObservableCollection<VehicleEx> m_Vehicles = new ObservableCollection<VehicleEx>();

        public ObservableCollection<VehicleEx> Vehicles
        {
            get { return m_Vehicles; }
            set { m_Vehicles = value; }
        }

        public string Flag
        {
            get { return m_flag; }
            set
            {
                m_flag = value;
                RaisePropertyChanged(() => Flag);
            }
        }

        public bool _IsChecked;
        public  bool IsChecked
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

        public MoniterGroup()
        {
            DiscriptionVisibility = Visibility.Visible;
            FunctionKeyVisibility = Visibility.Collapsed;
        }

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

        private Visibility _DiscriptionVisibility;
        private Visibility _FunctionKeyVisibility;
        private Visibility _CheckVisibility;

        public ObservableCollection<IMonitorEntity> GetChilds()
        {
            return new ObservableCollection<IMonitorEntity>(m_Vehicles);
        }

        public VehicleEx AddVehicle(Vehicle vehicle)
        {
            VehicleEx vehicleEx = new VehicleEx(vehicle);
            Vehicles.Add(vehicleEx);
            return vehicleEx;
        }
    }
}
