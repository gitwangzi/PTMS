/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 17a22f15-00c4-4e6a-a4cb-9cb0c8163792      
/////             clrversion: 4.0.30319.18063
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: GJSY(dengzl)
/////======================================================================
/////           Project Name: Gsafety.Ant.Bases.Models.TreeStructEx
/////    Project Description:    
/////             Class Name: VehicleEx
/////          Class Version: v1.0.0.0
/////            Create Time: 11/4/2014 2:49:54 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 11/4/2014 2:49:54 PM
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
using Gsafety.PTMS.Bases.Enums;
using Jounce.Core.Model;
using Gsafety.PTMS.Bases.Librarys;

namespace Gsafety.PTMS.Bases.Models
{
    public class VehicleEx : MonityEntityBase
    {
        private Vehicle _vehicleInfo;
        public Vehicle VehicleInfo
        {
            get
            {
                return _vehicleInfo;
            }
            set
            {
                _vehicleInfo = value;

                this.VehicleCount = 1;
                this.VehicleOnlineCount = _vehicleInfo.IsOnLine ? 1 : 0;
            }
        }
        private bool _IsChecked;

        public VehicleEx(Vehicle vehicle)
        {
            VehicleInfo = vehicle;
            this.IsLeaf = true;
            VehicleInfo.onOffLineChangeEvent += () =>
            {
                RaisePropertyChanged(() => VehicleInfo.IsOnLine);

                this.VehicleOnlineCount = VehicleInfo.IsOnLine ? 1 : 0;

                var org = Parent as OrganizationEx;
                if (org != null)
                {
                    org.Update();
                }
            };
        }

        public string Name
        {
            get
            {
                return VehicleInfo.Name;
            }
        }

        public string VehicleId
        {
            get
            {
                return VehicleInfo.VehicleId;
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
            }
        }

        public string UniqueId
        {
            get
            {
                return VehicleInfo.UniqueId;
            }
            set
            {
                if (VehicleInfo == null)
                {
                    VehicleInfo = new Vehicle();
                    VehicleInfo.UniqueId = value;
                    RaisePropertyChanged(() => UniqueId);
                }
                else
                {
                    VehicleInfo.UniqueId = value;
                    RaisePropertyChanged(() => UniqueId);
                }
            }
        }
    }
}