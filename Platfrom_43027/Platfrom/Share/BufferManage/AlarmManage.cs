/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 710157ff-0521-4d4a-81cc-6397f6b094f4      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share.BufferManage
/////    Project Description:    
/////             Class Name: AlarmManage
/////          Class Version: v1.0.0.0
/////            Create Time: 9/12/2013 11:41:08 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/12/2013 11:41:08 AM
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
using Jounce.Core.Model;
using System.Collections.ObjectModel;
using Jounce.Core.Event;
using System.ComponentModel.Composition;
using MessageService = Gsafety.PTMS.ServiceReference.MessageService;
using VehicleAlarmService = Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using Gsafety.PTMS.ServiceReference.SecuritySuiteService;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.ServiceReference.MessageService;
using System.Threading;
using Gsafety.PTMS.Share.Model;
namespace Gsafety.PTMS.Share
{
    public class AlarmManage : BaseNotify, IEventSink<RemoveDeviceSuiteAlertNotify>,
        IEventSink<Gsafety.PTMS.ServiceReference.MessageServiceExt.AlarmInfoEx>,
        IEventSink<Gsafety.PTMS.ServiceReference.MessageServiceExt.CompleteAlarm>,
        IPartImportsSatisfiedNotification
    {


        [Import]
        public IEventAggregator _EventAggregator { get; set; }
        private ObservableCollection<VehicleAlarmService.AlarmInfoEx> _AlarmInfos;

        private ObservableCollection<VehicleAlarmService.AlarmInfoEx> _RealTimeAlarmInfo;
        private VehicleAlarmService.VehicleAlarmServiceClient vehicleAlarmServiceClient;

        Object obj = new object();

        public ObservableCollection<VehicleAlarmService.AlarmInfoEx> AllAlarmInfo
        {
            get
            {
                return _AlarmInfos;
            }
        }

        public AlarmManage()
        {
            _AlarmInfos = new ObservableCollection<VehicleAlarmService.AlarmInfoEx>();
            _RealTimeAlarmInfo = new ObservableCollection<VehicleAlarmService.AlarmInfoEx>();
            CompositionInitializer.SatisfyImports(this);
        }

        public void DataLoading()
        {
            GetUnhandledAlarmInfo();
        }

        public void OnImportsSatisfied()
        {
            _EventAggregator.SubscribeOnDispatcher<Gsafety.PTMS.ServiceReference.MessageServiceExt.AlarmInfoEx>(this);
            _EventAggregator.SubscribeOnDispatcher<Gsafety.PTMS.ServiceReference.MessageServiceExt.CompleteAlarm>(this);
            _EventAggregator.SubscribeOnDispatcher<RemoveDeviceSuiteAlertNotify>(this);
        }

        /// <summary>
        /// Alarm information processing
        /// </summary>
        /// <param name="messageAlarmInfo"></param>
        public void HandleEvent(Gsafety.PTMS.ServiceReference.MessageServiceExt.AlarmInfoEx messageAlarmInfo)
        {
            VehicleAlarmService.AlarmInfoEx alarmInfo = new VehicleAlarmService.AlarmInfoEx();
            if (messageAlarmInfo.SuiteStatus == (int)Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuiteStatus.Abnormal
                || messageAlarmInfo.SuiteStatus == (int)Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuiteStatus.WaitingMaintenance
                || messageAlarmInfo.SuiteStatus == (int)Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuiteStatus.Running)
            {
                bool hasalarm = _AlarmInfos.Any(n => n.AlarmStatus == 1 && n.VehicleId == alarmInfo.VehicleId);
                alarmInfo.AlarmGuid = messageAlarmInfo.ID;
                alarmInfo.AlarmStatus = messageAlarmInfo.AlarmStatus;
                alarmInfo.AlarmTime = messageAlarmInfo.AlarmTime.Value.ToLocalTime();
                alarmInfo.City = messageAlarmInfo.City;
                alarmInfo.Direction = messageAlarmInfo.Direction;
                alarmInfo.OwnerPhone = messageAlarmInfo.OwnerPhone;
                alarmInfo.VehicleOwner = messageAlarmInfo.VehicleOwner;
                alarmInfo.DistrictCode = messageAlarmInfo.DistrictCode;
                alarmInfo.GpsTime = messageAlarmInfo.GpsTime;
                alarmInfo.GpsValid = messageAlarmInfo.GpsValid;
                alarmInfo.ID = messageAlarmInfo.ID;
                alarmInfo.Latitude = messageAlarmInfo.Latitude;
                alarmInfo.Longitude = messageAlarmInfo.Longitude;
                alarmInfo.Speed = messageAlarmInfo.Speed;
                alarmInfo.ButtonNum = messageAlarmInfo.ButtonNum;
                alarmInfo.MdvrCoreId = messageAlarmInfo.MdvrCoreId;
                alarmInfo.SuiteID = messageAlarmInfo.SuiteID;
                alarmInfo.SuiteInfoID = messageAlarmInfo.SuiteInfoID;
                alarmInfo.VehicleId = messageAlarmInfo.VehicleId;
                alarmInfo.TransferStatus = messageAlarmInfo.TransferStatus;
                alarmInfo.AppealStatus = messageAlarmInfo.AppealStatus;
                alarmInfo.DisposalStatus = messageAlarmInfo.DisposalStatus;
                alarmInfo.Source = messageAlarmInfo.Source;
                alarmInfo.IsAlive = true;
                alarmInfo.AlarmMobile = messageAlarmInfo.AlarmMobile;

                var vehicleInfo = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.FirstOrDefault(t => t.VehicleId == alarmInfo.VehicleId);
                if (vehicleInfo != null)
                {
                    alarmInfo.VehicleInfo = vehicleInfo;
                    //alarmInfo.IsOnLine = vehicleInfo.IsOnLine;
                }

                if (messageAlarmInfo.User == ApplicationContext.Instance.AuthenticationInfo.UserID)
                {
                    alarmInfo.IsDesignated = true;
                }
                _AlarmInfos.Insert(0, alarmInfo);
                _RealTimeAlarmInfo.Insert(0, alarmInfo);
                _EventAggregator.Publish<VehicleAlarmService.AlarmInfoEx>(alarmInfo);
                RaisePropertyChanged("AllAlarmInfo");

                if (!hasalarm)
                {
                    AlarmGisArgs args = new AlarmGisArgs();
                    args.Alarm = 1;
                    args.VehicleID = alarmInfo.VehicleId;

                    _EventAggregator.Publish<AlarmGisArgs>(args);
                }
            }
        }

        public void HandleManualAlarm(VehicleAlarmService.AlarmInfoEx alarminfo)
        {
            alarminfo.IsAlive = true;
            alarminfo.AlarmTime = alarminfo.AlarmTime.Value.ToLocalTime();
            var vehicleInfo = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.FirstOrDefault(t => t.VehicleId == alarminfo.VehicleId);
            if (vehicleInfo != null)
            {
                //alarminfo.IsOnLine = vehicleInfo.IsOnLine;
                alarminfo.VehicleInfo = vehicleInfo;
            }

            alarminfo.IsDesignated = true;
            _AlarmInfos.Insert(0, alarminfo);
            _RealTimeAlarmInfo.Insert(0, alarminfo);
            _EventAggregator.Publish<VehicleAlarmService.AlarmInfoEx>(alarminfo);
            RaisePropertyChanged("AllAlarmInfo");

            if (alarminfo.TransferStatus == 4)
            {
                PTMS.ServiceReference.MessageServiceExt.AlarmInfoEx alarminfoex = new PTMS.ServiceReference.MessageServiceExt.AlarmInfoEx();
                alarminfoex.AdditionalInfo = alarminfo.AdditionalInfo;
                alarminfoex.AlarmGuid = alarminfo.AlarmGuid;
                alarminfoex.AlarmStatus = alarminfo.AlarmStatus;
                alarminfoex.AlarmTime = alarminfo.AlarmTime;
                alarminfoex.AppealStatus = alarminfo.AppealStatus;
                alarminfoex.ButtonNum = alarminfo.ButtonNum;
                alarminfoex.City = alarminfo.City;
                alarminfoex.ClientId = alarminfo.ClientId;
                alarminfoex.Direction = alarminfo.Direction;
                alarminfoex.DisposalStatus = alarminfo.DisposalStatus;
                alarminfoex.DistrictCode = alarminfo.DistrictCode;
                alarminfoex.GpsTime = alarminfo.GpsTime;
                alarminfoex.GpsValid = alarminfo.GpsValid;
                alarminfoex.Height = alarminfo.Height;
                alarminfoex.ID = alarminfo.ID;
                alarminfoex.Latitude = alarminfo.Latitude;
                alarminfoex.Longitude = alarminfo.Longitude;
                alarminfoex.MdvrCoreId = alarminfo.MdvrCoreId;
                alarminfoex.OperationLincese = alarminfo.OperationLincese;
                alarminfoex.Organizations = alarminfo.Organizations;
                alarminfoex.OwnerPhone = alarminfo.OwnerPhone;
                alarminfoex.Province = alarminfo.Province;
                alarminfoex.Source = alarminfo.Source;
                alarminfoex.Speed = alarminfo.Speed;
                alarminfoex.SuiteID = alarminfo.SuiteID;
                alarminfoex.SuiteInfoID = alarminfo.SuiteInfoID;
                alarminfoex.SuiteStatus = alarminfo.SuiteStatus;
                alarminfoex.TransferStatus = alarminfo.TransferStatus;
                alarminfoex.User = ApplicationContext.Instance.AuthenticationInfo.UserID;
                alarminfoex.VehicleId = alarminfo.VehicleId;
                alarminfoex.VehicleOwner = alarminfo.VehicleOwner;
                alarminfoex.IncidentAddress = alarminfo.IncidentAddress;
                alarminfoex.IncidentLevel = alarminfo.IncidentLevel;
                alarminfoex.IncidentType = alarminfo.IncidentType;
                alarminfoex.AlarmContent = alarminfo.AlarmContent;
                alarminfoex.VehicleType = alarminfo.VehicleInfo.VehicleTypeDescribe;
                alarminfoex.UserName = ApplicationContext.Instance.AuthenticationInfo.Account;
                ApplicationContext.Instance.MessageClient.TransferAlarm(alarminfoex);
            }
        }

        public void FireIfNoAlarm(string VehicleId)
        {
            bool hasalarm = _AlarmInfos.Any(n => n.AlarmStatus == 1 && n.VehicleId == VehicleId);

            if (!hasalarm)
            {
                AlarmGisArgs args = new AlarmGisArgs();
                args.Alarm = 0;
                args.VehicleID = VehicleId;

                _EventAggregator.Publish<AlarmGisArgs>(args);
            }
        }
        /// <summary>
        /// Get a key alarm untreated
        /// </summary>
        private void GetUnhandledAlarmInfo()
        {
            vehicleAlarmServiceClient = ServiceClientFactory.Create<VehicleAlarmService.VehicleAlarmServiceClient>();
            vehicleAlarmServiceClient.GetUnHandledAlarmsCompleted += vehicleAlarmServiceClient_GetUnHandledAlarmsCompleted;
            ObservableCollection<string> organizations = new ObservableCollection<string>();
            foreach (var item in ApplicationContext.Instance.AuthenticationInfo.Organizations)
            {
                organizations.Add(item.ID);
            }
            vehicleAlarmServiceClient.GetUnHandledAlarmsAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, organizations);
          
            ApplicationContext.Instance.BufferManager.MonitorGroupManager.DataLoading();

            
            ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "AlarmManager", "begin to get unhandledalarms");
            ApplicationContext.Instance.BusyInfo.InitLoadingNum++;
        }

        void vehicleAlarmServiceClient_GetUnHandledAlarmsCompleted(object sender, VehicleAlarmService.GetUnHandledAlarmsCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "AlarmManager", "get unhandledalarms error");
                    ApplicationContext.Instance.BusyInfo.InitLoadingNum--;
                    vehicleAlarmServiceClient.CloseAsync();
                    ApplicationContext.Instance.Logger.LogException(GetType().FullName, e.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }

            try
            {
                ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "AlarmManager", "end get unhandledalarms");
                if (e.Result != null && e.Result.Result.Count > 0)
                {
                    if (_AlarmInfos.Count == 0)
                    {
                        _AlarmInfos = e.Result.Result;

                        foreach (var item in _AlarmInfos)
                        {
                            item.IsAlive = false;
                            if (item.User == ApplicationContext.Instance.AuthenticationInfo.UserID)
                            {
                                item.IsDesignated = true;
                            }
                            else
                            {
                                item.IsDesignated = false;
                            }
                            item.AlarmTime = item.AlarmTime.Value.ToLocalTime();
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
                    else
                    {

                    }
                }

                //ApplicationContext.Instance.BufferManager.MonitorGroupManager.DataLoading();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }
            finally
            {
                ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "AlarmManager", "get unhandledalarms finish");
                ApplicationContext.Instance.BusyInfo.InitLoadingNum--;
            }
        }

        public bool HasAlarm(string vehicleID)
        {
            if (_AlarmInfos.Count != 0)
            {
                bool result = _AlarmInfos.Any(n => n.VehicleId == vehicleID && n.AppealStatus == 0);

                return result;
            }

            return false;
        }

        /// <summary>
        /// Consumer alarm complete the information
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(Gsafety.PTMS.ServiceReference.MessageServiceExt.CompleteAlarm publishedEvent)
        {
            if (publishedEvent != null)
            {
                if (publishedEvent.HandlerID != ApplicationContext.Instance.AuthenticationInfo.UserID)
                {
                    VehicleAlarmService.AlarmInfoEx deleteinfo = null;
                    DateTime time = publishedEvent.AlarmTime.ToLocalTime();
                    foreach (var item in AllAlarmInfo)
                    {
                        if (item.MdvrCoreId == publishedEvent.MdvrCoreId && time == item.AlarmTime.Value)
                        {
                            deleteinfo = item;
                            break;
                        }
                    }
                    lock (obj)
                    {
                        AllAlarmInfo.Remove(deleteinfo);
                        ApplicationContext.Instance.EventAggregator.Publish<AlarmCountChange>(new AlarmCountChange());
                    }
                }
            }
        }

        /// <summary>
        /// Cancel all the way to the visibility of the video Road video
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(RemoveDeviceSuiteAlertNotify publishedEvent)
        {
            if (publishedEvent.RemovedAlarmFlag)
            {
                foreach (var item in AllAlarmInfo)
                {
                    if (item.MdvrCoreId == publishedEvent.MdvrCoreSN)
                    {
                        //item.EnableVisible = false;
                    }
                }
                RaisePropertyChanged("AllAlarmInfo");
            }
        }
    }
}
