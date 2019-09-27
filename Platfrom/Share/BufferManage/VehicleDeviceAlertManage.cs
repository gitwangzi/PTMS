
using Gsafety.PTMS.ServiceReference.MessageServiceExt;
using Gsafety.PTMS.ServiceReference.OrganizationService;
using Gsafety.PTMS.ServiceReference.VehicleAlertService;
using Jounce.Core.Event;
using Jounce.Core.Model;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: fe2a24ee-9929-48d2-9cbd-56c020db5982      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share.BufferManage
/////    Project Description:    
/////             Class Name: VehicleAlertManage
/////          Class Version: v1.0.0.0
/////            Create Time: 9/12/2013 2:14:03 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/12/2013 2:14:03 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Gsafety.PTMS.ServiceReference.DeviceAlertService;

namespace Gsafety.PTMS.Share
{
    public class VehicleDeviceAlertManage : BaseNotify,
        IEventSink<Gsafety.PTMS.ServiceReference.MessageServiceExt.DeviceAlertEx>,
        IPartImportsSatisfiedNotification
    {
        #region fields

        [Import]
        public IEventAggregator _EventAggregator { get; set; }
        private ObservableCollection<Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertEx> _VehicleDeviceAlertInfos;
        DeviceAlertServiceClient _VehicleDeviceAlertServiceClient;

        #endregion

        #region Attributes

        public ObservableCollection<Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertEx> VehicleDeviceAlert
        {
            get { return _VehicleDeviceAlertInfos; }
        }


        #endregion
        Object obj = new object();
        public VehicleDeviceAlertManage()
        {
            _VehicleDeviceAlertInfos = new ObservableCollection<Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertEx>();
            CompositionInitializer.SatisfyImports(this);
        }

        public void DataLoading()
        {
            //GetDeviceAlertInfo();
        }

        public void OnImportsSatisfied()
        {
            _EventAggregator.SubscribeOnDispatcher<Gsafety.PTMS.ServiceReference.MessageServiceExt.DeviceAlertEx>(this);

        }

        public void HandleEvent(Gsafety.PTMS.ServiceReference.MessageServiceExt.DeviceAlertEx alarmInfo)
        {
            AlertInfoChange(alarmInfo);
        }

        private void AlertInfoChange(Gsafety.PTMS.ServiceReference.MessageServiceExt.DeviceAlertEx alertInfo)
        {
            if (alertInfo == null)
                return;

            Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertEx vehicleAlert = new Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertEx();
            if (alertInfo.SuiteStatus == (int)Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuiteStatus.Abnormal
              || alertInfo.SuiteStatus == (int)Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuiteStatus.WaitingMaintenance
              || alertInfo.SuiteStatus == (int)Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuiteStatus.Running)
            {
                bool hasalert = _VehicleDeviceAlertInfos.Any(n => n.VehicleId == alertInfo.VehicleId);
                vehicleAlert.VehicleId = alertInfo.VehicleId;
                vehicleAlert.AlertTime = alertInfo.AlertTime.HasValue ? alertInfo.AlertTime.Value.ToLocalTime() : alertInfo.AlertTime;
                vehicleAlert.AlertType = alertInfo.AlertType;
                vehicleAlert.Direction = alertInfo.Direction;
                vehicleAlert.GpsTime = alertInfo.GpsTime;
                vehicleAlert.GpsValid = alertInfo.GpsValid;
                vehicleAlert.Id = alertInfo.Id;
                vehicleAlert.Latitude = alertInfo.Latitude;
                vehicleAlert.SuiteInfoId = alertInfo.SuiteInfoId;
                vehicleAlert.Longitude = alertInfo.Longitude;
                vehicleAlert.SuiteId = alertInfo.SuiteId;
                //Organization Organization = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.AuthorityVehicleOrgs.FirstOrDefault(n => n.ID == alertInfo.OrganizationId);
                //vehicleAlert.OrganizationName = Organization.Name;
                //vehicleAlert.OrganizationId = Organization.ID;
                if (!string.IsNullOrEmpty(alertInfo.MdvrCoreId))
                {
                    vehicleAlert.MdvrCoreId = alertInfo.MdvrCoreId;
                }

                vehicleAlert.Speed = alertInfo.Speed.ToString();
                vehicleAlert.SuiteStatus = alertInfo.SuiteStatus;


                var vehicle = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.FirstOrDefault(t => t.VehicleId == vehicleAlert.VehicleId);
                if (vehicle != null)
                {
                    //vehicleAlert.IsOnLine = vehicle.IsOnLine;
                    vehicleAlert.VehicleOwner = vehicle.Owner;
                    vehicleAlert.Province = vehicle.ProvinceName;
                    vehicleAlert.VehicleInfo = vehicle;
                    vehicleAlert.OrganizationName = vehicle.OrganizationName;
                    vehicleAlert.OrganizationId = vehicle.OrganizationID;
                }

                lock (obj)
                {
                    _VehicleDeviceAlertInfos.Insert(0, vehicleAlert);
                }

                _EventAggregator.Publish<Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertEx>(vehicleAlert);

                if (!hasalert)
                {
                    AlertGisArgs args = new AlertGisArgs();
                    args.Alert = 1;
                    args.VehicleID = alertInfo.VehicleId;

                    _EventAggregator.Publish<AlertGisArgs>(args);
                }
            }
        }

        private void GetDeviceAlertInfo()
        {
            _VehicleDeviceAlertServiceClient = ServiceClientFactory.Create<DeviceAlertServiceClient>();
            _VehicleDeviceAlertServiceClient.GetDeviceAlertEx1Completed += _VehicleDeviceAlertServiceClient_GetDeviceAlertEx1Completed;
            var page = new Gsafety.PTMS.ServiceReference.DeviceAlertService.PagingInfo()
            {
                PageIndex = 1,
                PageSize = -1,
            };
            _VehicleDeviceAlertServiceClient.GetDeviceAlertEx1Async("", "", null, DateTime.UtcNow.AddMonths(-5), DateTime.UtcNow, page);
            ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "VehicleAlertManage", "begin get alerts");
            ApplicationContext.Instance.BusyInfo.InitLoadingNum++;

        }

        void _VehicleDeviceAlertServiceClient_GetDeviceAlertEx1Completed(object sender, GetDeviceAlertEx1CompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    _VehicleDeviceAlertServiceClient.CloseAsync();
                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "VehicleAlertManage", "get alerts error");
                    ApplicationContext.Instance.BusyInfo.InitLoadingNum--;
                }
                else
                {
                    if (_VehicleDeviceAlertInfos == null || _VehicleDeviceAlertInfos.Count == 0)
                    {
                        _VehicleDeviceAlertInfos = e.Result.Result;
                        foreach (var item in _VehicleDeviceAlertInfos)
                        {
                            if (item.AlertTime.HasValue)
                            {
                                item.AlertTime = item.AlertTime.Value.ToLocalTime();
                            }

                            foreach (var vehicle in ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList)
                            {
                                if (vehicle.VehicleId == item.VehicleId)
                                {
                                    //item.VehicleOwner = vehicle.Owner;
                                    item.Province = vehicle.ProvinceName;
                                    //item.City = vehicle.CityName;
                                    //item.IsOnLine = vehicle.IsOnLine;
                                    //item.VehicleInfo = vehicle;
                                    //item.OrganizationId = vehicle.OrganizationID;
                                    //item.OrganizationName = vehicle.OrganizationName;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleAlertManage", ex);
            }
            finally
            {
                ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "VehicleAlertManage", "get alerts finish");
                ApplicationContext.Instance.BusyInfo.InitLoadingNum--;
            }
        }

        public void FireIfNoAlarm(string VehicleId)
        {
            bool hasalert = _VehicleDeviceAlertInfos.Any(n => n.VehicleId == VehicleId);

            if (!hasalert)
            {
                AlertGisArgs args = new AlertGisArgs();
                args.Alert = 0;
                args.VehicleID = VehicleId;

                _EventAggregator.Publish<AlertGisArgs>(args);
            }
        }


        public bool HasAlert(string vehicleID)
        {
            if (_VehicleDeviceAlertInfos.Count != 0)
            {
                bool result = _VehicleDeviceAlertInfos.Any(n => n.VehicleId == vehicleID);

                return result;
            }

            return false;
        }
    }
}
