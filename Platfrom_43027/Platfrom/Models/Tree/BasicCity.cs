/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: 0a5bb8f8-e071-46f3-a9c7-2d738a294e3f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: TEST(JiangJ)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Bases.Models
/////    Project Description:    
/////             Class Name: BasicCity
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/31 11:42:26
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/31 11:42:26
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
using Gsafety.PTMS.Bases.Enums;
using System.Linq;

namespace Gsafety.PTMS.Bases.Models
{
    public class BasicCity 
    {
        public BasicProvince Parent;
        public BasicCity(BasicProvince parent)
        {
            this.Parent = parent;
            if (_VehicleTypes == null)
            {
                _VehicleTypes = new ObservableCollection<BasicVehicleType>();
            }
        }
        public string Code { get; set; }
        public string Name { get; set; }

        public ObservableCollection<BasicVehicleType> _VehicleTypes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicle"></param>
        public BasicVehicle AddVehicle(Gsafety.PTMS.ServiceReference.VehicleService.Vehicle vehicle)
        {
            BasicVehicle vehicleModle = new BasicVehicle();
            vehicleModle.VehicleId = vehicle.VehicleId;
            vehicleModle.MDVRSN= vehicle.MDVR_SN;
            vehicleModle.BrandModel = vehicle.BrandModel;
            vehicleModle.EngineId = vehicle.EngineId;
            vehicleModle.Owner = vehicle.Owner;
            vehicleModle.StartYear = vehicle.StartYear;
            vehicleModle.VehicleSn = vehicle.VehicleSn;
            vehicleModle.CityCode = vehicle.CityCode;
            vehicleModle.CityName = vehicle.CityName;
            vehicleModle.ProvinceName = vehicle.ProvinceName;
            vehicleModle.IsOnLine = vehicle.IsOnLine == null ? false : vehicle.IsOnLine == 1 ;
            BasicVehicleType vt = GetVehicleTypeInfo(vehicle);//new VT then add VT in _VehicleTypes then return this vt
            vehicleModle.Parent = vt;
            vehicleModle.VehicleType = vt.VehicleType;


            vt.Vehicles.Add(vehicleModle);
            //vt.refresh();

            return vehicleModle;
        }

        private BasicVehicleType GetVehicleTypeInfo(Gsafety.PTMS.ServiceReference.VehicleService.Vehicle vehicle)
        {
            VehicleType vtype = new VehicleType();
            switch (vehicle.Type)
            {
                case Gsafety.PTMS.ServiceReference.VehicleService.VehicleType.Bus:
                    vtype = VehicleType.Bus;
                    break;
                case Gsafety.PTMS.ServiceReference.VehicleService.VehicleType.Flota:
                    vtype = VehicleType.Flota;
                    break;
                case Gsafety.PTMS.ServiceReference.VehicleService.VehicleType.Taxi:
                    vtype = VehicleType.Taxi;
                    break;
            }
            BasicVehicleType vehicleTypeInfo;
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

        private BasicVehicleType AddVehicleType(VehicleType vehicleType)
        {
            BasicVehicleType vehicleTypeInfo = new BasicVehicleType(this);
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
    }
}
