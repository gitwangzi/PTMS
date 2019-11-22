
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

namespace Gsafety.PTMS.Share
{
    public class VehicleAlertManage : BaseNotify,
        IEventSink<Gsafety.PTMS.ServiceReference.MessageServiceExt.BusinessAlertEx>,
        IEventSink<CompleteAlert>,
        IPartImportsSatisfiedNotification
    {
        #region fields

        [Import]
        public IEventAggregator _EventAggregator { get; set; }
        private ObservableCollection<Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx> _VehicleAlertInfos;
        VehicleAlertServiceClient _VehicleAlertServiceClient;

        #endregion

        #region Attributes

        public ObservableCollection<Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx> VehicleAlert
        {
            get { return _VehicleAlertInfos; }
        }


        #endregion
        Object obj = new object();
        public VehicleAlertManage()
        {
            _VehicleAlertInfos = new ObservableCollection<Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx>();
            CompositionInitializer.SatisfyImports(this);
        }

        public void DataLoading()
        {
            GetAlertInfo();
        }

        public void OnImportsSatisfied()
        {
            _EventAggregator.SubscribeOnDispatcher<Gsafety.PTMS.ServiceReference.MessageServiceExt.BusinessAlertEx>(this);
            _EventAggregator.SubscribeOnDispatcher<Gsafety.PTMS.ServiceReference.MessageServiceExt.CompleteAlert>(this);
        }

        public void HandleEvent(Gsafety.PTMS.ServiceReference.MessageServiceExt.BusinessAlertEx alarmInfo)
        {
            AlertInfoChange(alarmInfo);
        }

        private void AlertInfoChange(Gsafety.PTMS.ServiceReference.MessageServiceExt.BusinessAlertEx alertInfo)
        {
            if (alertInfo == null)
                return;

            Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx vehicleAlert = new Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx();
            if (alertInfo.SuiteStatus == (int)Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuiteStatus.Abnormal
              || alertInfo.SuiteStatus == (int)Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuiteStatus.WaitingMaintenance
              || alertInfo.SuiteStatus == (int)Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuiteStatus.Running)
            {
                bool hasalert = _VehicleAlertInfos.Any(n => n.Status == 0 && n.VehicleId == alertInfo.VehicleId);
                vehicleAlert.VehicleId = alertInfo.VehicleId;
                vehicleAlert.AlertTime = alertInfo.AlertTime.HasValue ? alertInfo.AlertTime.Value.ToLocalTime() : alertInfo.AlertTime;
                vehicleAlert.AlertType = alertInfo.AlertType;
                vehicleAlert.Direction = alertInfo.Direction;
                vehicleAlert.GpsTime = alertInfo.GpsTime;
                vehicleAlert.GpsValid = alertInfo.GpsValid;
                vehicleAlert.VehicleType = alertInfo.VehicleType;
                vehicleAlert.Id = alertInfo.Id;
                vehicleAlert.Latitude = alertInfo.Latitude;
                vehicleAlert.SuiteInfoID = alertInfo.SuiteInfoID;
                vehicleAlert.Longitude = alertInfo.Longitude;
                vehicleAlert.SuiteID = alertInfo.SuiteID;
                Organization Organization = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.AuthorityVehicleOrgs.FirstOrDefault(n => n.ID == alertInfo.OrganizationId);
                vehicleAlert.OrganizationName = Organization.Name;
                vehicleAlert.OrganizationId = alertInfo.OrganizationId;
                if (!string.IsNullOrEmpty(alertInfo.MdvrCoreId))
                {
                    vehicleAlert.MdvrCoreId = alertInfo.MdvrCoreId;
                }

                vehicleAlert.Speed = alertInfo.Speed.ToString();
                vehicleAlert.Status = alertInfo.Status;
                vehicleAlert.Province = alertInfo.Province;
                vehicleAlert.City = alertInfo.City;

                var vehicle = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.FirstOrDefault(t => t.VehicleId == vehicleAlert.VehicleId);
                if (vehicle != null)
                {
                    //vehicleAlert.IsOnLine = vehicle.IsOnLine;
                    vehicleAlert.VehicleInfo = vehicle;
                }

                lock (obj)
                {
                    _VehicleAlertInfos.Insert(0, vehicleAlert);
                }

                _EventAggregator.Publish<Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx>(vehicleAlert);

                if (!hasalert)
                {
                    AlertGisArgs args = new AlertGisArgs();
                    args.Alert = 1;
                    args.VehicleID = alertInfo.VehicleId;

                    _EventAggregator.Publish<AlertGisArgs>(args);
                }
            }
        }

        private void GetAlertInfo()
        {
            _VehicleAlertServiceClient = ServiceClientFactory.Create<VehicleAlertServiceClient>();
            _VehicleAlertServiceClient.GetUnHandleAlertByClientCompleted += _VehicleAlertServiceClient_GetUnHandleAlertByClientCompleted;
            ObservableCollection<string> organizations = new ObservableCollection<string>();
            foreach (var item in ApplicationContext.Instance.AuthenticationInfo.Organizations)
            {
                organizations.Add(item.ID);
            }
            _VehicleAlertServiceClient.GetUnHandleAlertByClientAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, organizations);
            ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "VehicleAlertManage", "begin get alerts");
            ApplicationContext.Instance.BusyInfo.InitLoadingNum++;

        }

        void _VehicleAlertServiceClient_GetUnHandleAlertByClientCompleted(object sender, GetUnHandleAlertByClientCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    _VehicleAlertServiceClient.CloseAsync();
                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "VehicleAlertManage", "get alerts error");
                    ApplicationContext.Instance.BusyInfo.InitLoadingNum--;
                }
                else
                {
                    if (_VehicleAlertInfos == null || _VehicleAlertInfos.Count == 0)
                    {
                        _VehicleAlertInfos = e.Result.Result;
                        foreach (var item in _VehicleAlertInfos)
                        {
                            if (item.AlertTime.HasValue)
                            {
                                item.AlertTime = item.AlertTime.Value.ToLocalTime();
                            }

                            foreach (var vehicle in ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList)
                            {
                                if (vehicle.VehicleId == item.VehicleId)
                                {
                                    item.VehicleOwner = vehicle.Owner;
                                    item.Province = vehicle.ProvinceName;
                                    item.City = vehicle.CityName;
                                    //item.IsOnLine = vehicle.IsOnLine;
                                    item.VehicleInfo = vehicle;

                                    break;
                                }
                            }

                            if (string.IsNullOrEmpty(item.Province))
                            {
                                if (item.DistrictCode.Length == 5)
                                {
                                    string provicecode = item.DistrictCode.Substring(0, 2);
                                    var province = ApplicationContext.Instance.BufferManager.DistrictManager.Provinces.FirstOrDefault(n => n.Code == provicecode);
                                    if (province != null)
                                    {
                                        item.Province = province.Name;
                                    }

                                    var city = ApplicationContext.Instance.BufferManager.DistrictManager.Cities.FirstOrDefault(n => n.Code == item.DistrictCode);
                                    if (city != null)
                                    {
                                        item.City = city.Name;
                                    }

                                }
                                else if (item.DistrictCode.Length == 2)
                                {
                                    var province = ApplicationContext.Instance.BufferManager.DistrictManager.Provinces.FirstOrDefault(n => n.Code == item.DistrictCode);
                                    if (province != null)
                                    {
                                        item.Province = province.Name;
                                    }
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
            bool hasalert = _VehicleAlertInfos.Any(n => n.Status == 0 && n.VehicleId == VehicleId);

            if (!hasalert)
            {
                AlertGisArgs args = new AlertGisArgs();
                args.Alert = 0;
                args.VehicleID = VehicleId;

                _EventAggregator.Publish<AlertGisArgs>(args);
            }
        }

        public void HandleEvent(CompleteAlert publishedEvent)
        {
            try
            {
                if (publishedEvent != null)
                {
                    List<Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx> list = new List<Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx>();
                    foreach (var item in VehicleAlert)
                    {
                        if (item.Id == publishedEvent.AlertID)
                        {
                            list.Add(item);
                        }

                    }
                    lock (obj)
                    {
                        foreach (var item in list)
                        {
                            VehicleAlert.Remove(item);
                        }
                    }

                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleAlert));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleAlertManage", ex);
            }
        }

        public bool HasAlert(string vehicleID)
        {
            if (_VehicleAlertInfos.Count != 0)
            {
                bool result = _VehicleAlertInfos.Any(n => n.VehicleId == vehicleID && n.Status == 0);

                return result;
            }

            return false;
        }
    }
}
