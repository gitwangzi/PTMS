/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: fbc284e5-dabd-4611-839e-f357d33146ad      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MainPage.ViewModels
/////    Project Description:    
/////             Class Name: MessageNotifitionVm
/////          Class Version: v1.0.0.0
/////            Create Time: 9/2/2013 10:22:46 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/2/2013 10:22:46 AM
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
using Jounce.Core.ViewModel;
using System.Collections.ObjectModel;
using Gsafety.PTMS.Share;
using System.Windows.Data;
using Jounce.Core.Event;
using System.ComponentModel.Composition;
using VehicleAlarmService = Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using MessageService = Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.ServiceReference.VehicleAlertService;
using Gsafety.Common.CommMessage;
using System.IO;
using System.Reflection;
using Gsafety.Common;
using Jounce.Framework;
using System.ComponentModel;

namespace Gsafety.PTMS.MainPage.ViewModels
{
    [ExportAsViewModel(MainPageName.MessageNotifitionVm)]
    public class MessageNotifitionVm :
        BaseViewModel,
        IEventSink<MessageService.AlarmInfo>,
        IEventSink<MessageService.AlertBaseModel>,
        IPartImportsSatisfiedNotification
    {
        #region Fields

        private const string _AlarmVedioResource = "Gsafety.PTMS.MainPage.Sound.alarm.mp3";  
        private const string _AlertVeidoResource = "Gsafety.PTMS.MainPage.Sound.alarm.mp3"; 
        private AsyncOperation asyncOper;
        PagedCollectionView _AlarmInfoPagedCV;
        PagedCollectionView _AlertInfoPagedCV;
        private bool _AlarmAutoDisplay = false;
        private bool _AlertAutoDisplay = false;
        private bool _AlarmAutoMusic = false;
        private bool _AlertAutoMusic = false;
        private int _CurrentSelectedIndex = 0;
        MediaElement _VedioPlay = new MediaElement();
        Stream _AlarmVedioStream;
        Stream _AlertVeidoStream;

        #endregion

        #region Attributes

        public PagedCollectionView AlarmInfoPagedCV
        {
            get { return _AlarmInfoPagedCV; }
        }

        public PagedCollectionView AlertInfoPagedCV
        {
            get { return _AlertInfoPagedCV; }
        }

        public string AlarmCount
        {

            get { return string.Format("({0})", AlarmInfoPagedCV.TotalItemCount); }
        }

        public string AlertCount
        {
            get { return string.Format("({0})", AlertInfoPagedCV.TotalItemCount); }
        }

        public bool AlarmAutoDisplay
        {
            get { return _AlarmAutoDisplay; }
            set { _AlarmAutoDisplay = value; }
        }

        public bool AlertAutoDisplay
        {
            get { return _AlertAutoDisplay; }
            set { _AlertAutoDisplay = value; }
        }

        public bool AlarmAutoMusic
        {
            get { return _AlarmAutoMusic; }
            set { _AlarmAutoMusic = value; }
        }

        public bool AlertAutoMusic
        {
            get { return _AlertAutoMusic; }
            set { _AlertAutoMusic = value; }
        }


        public int CurrentSelectedIndex
        {
            get { return _CurrentSelectedIndex; }
            set { _CurrentSelectedIndex = value; }
        }

        #endregion

        public MessageNotifitionVm()
        {
            asyncOper = AsyncOperationManager.CreateOperation(null);
            //_AlarmInfoPagedCV = new PagedCollectionView(ApplicationContext.Instance.BufferManager.AlarmManager.RealTimeAlarmInfo);
            //_AlertInfoPagedCV = new PagedCollectionView(ApplicationContext.Instance.BufferManager.VehicleAlertManager.RealTimeAlertInfos);
        }

        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<MessageService.AlarmInfo>(this);
            EventAggregator.SubscribeOnDispatcher<MessageService.AlertBaseModel>(this);
        }

        public void HandleEvent(MessageService.AlarmInfo alarmInfo)
        {

            try
            {
                asyncOper.Post(result =>
                    {
                        try
                        {
                            CurrentSelectedIndex = 0;
                            JounceHelper.ExecuteOnUI(() =>
                                   {
                                       RaisePropertyChanged(() => CurrentSelectedIndex);
                                       RaisePropertyChanged(() => AlarmCount);
                                   }
                                );
                        }
                        catch
                        {
                        }
                    }, null);


                if (AlarmAutoDisplay)
                {
                    EventAggregator.Publish<MessageNotifitionActiveteParamter>(new MessageNotifitionActiveteParamter(true));
                }

                if (AlarmAutoMusic)
                {
                    if (_AlarmVedioStream == null)
                    {
                        _AlarmVedioStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_AlarmVedioResource);
                    }
                    Play(_AlarmVedioStream);
                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public void HandleEvent(MessageService.AlertBaseModel alarmInfo)
        {
            asyncOper.Post(result =>
            {
                try
                {
                    CurrentSelectedIndex = 1;
                    JounceHelper.ExecuteOnUI(() =>
                    {
                        RaisePropertyChanged(() => CurrentSelectedIndex);
                        RaisePropertyChanged(() => AlertCount);
                    });
                }
                catch
                {
                }
            }, null);
            if (AlertAutoDisplay)
            {
                EventAggregator.Publish<MessageNotifitionActiveteParamter>(new MessageNotifitionActiveteParamter(true));
            }
            if (AlertAutoMusic)
            {
                if (_AlarmVedioStream == null)
                {
                    _AlertVeidoStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_AlertVeidoResource);
                }
                Play(_AlertVeidoStream);
            }
        }

        private void Play(Stream vedioStream)
        {
            try
            {
                if (vedioStream == null)
                    return;
                _VedioPlay.Stop();
                _VedioPlay.Volume = 1.0;
                _VedioPlay.SetSource(vedioStream);
                _VedioPlay.Position = TimeSpan.Zero;
                _VedioPlay.Play();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


    }
}
