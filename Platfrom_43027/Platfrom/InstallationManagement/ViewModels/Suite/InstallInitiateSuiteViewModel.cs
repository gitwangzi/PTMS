using BaseLib.ViewModels;
using Gsafety.Ant.Installation.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.BscDevSuiteService;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 09f0dcfa-5ed4-4803-87b7-0d3ae65dad77      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.ViewModels
/////    Project Description:    
/////             Class Name: DeviceSelftestViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/17 15:32:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/17 15:32:14
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Threading;
//using Gsafety.PTMS.ServiceReference.MessageServiceExt;

namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.InstallInitiateSuiteVm)]
    public class InstallInitiateSuiteViewModel : InstallSuiteViewModelBase
    {
        public ObservableCollection<CameraInfoEx> CameraInfoList { get; set; }

        private ObservableCollection<InitSettingItem> _initSettingItems = new ObservableCollection<ViewModels.InitSettingItem>();
        public ObservableCollection<InitSettingItem> InitSettingItems
        {
            get { return _initSettingItems; }
            set
            {
                _initSettingItems = value;
                RaiseErrorsChanged(() => InitSettingItems);
            }
        }

        private InitSettingItem _setAlarmParaItem;

        private DispatcherTimer _timer = new DispatcherTimer();

        private int _tickCount = 0;
        public int TickCount
        {
            get { return _tickCount; }
            set
            {
                _tickCount = value;
                RaisePropertyChanged(() => TickCount);
            }
        }

        private int _tryCountWhenNull = 3;

        private BscDevSuitePartServiceClient _bscDevSuitePartServiceClient;

        public InstallInitiateSuiteViewModel()
        {
            try
            {
                step = 4;
                ImageSource = "Step04.png";

                CameraInfoList = new ObservableCollection<CameraInfoEx>();

                _bscDevSuitePartServiceClient = ServiceClientFactory.Create<BscDevSuitePartServiceClient>();
                _bscDevSuitePartServiceClient.GetCameraListBySuiteInfoIDCompleted += _bscDevSuitePartServiceClient_GetCameraListBySuiteInfoIDCompleted;
                deviceInstallServiceClient.SubmitForStep4Completed += deviceInstallServiceClient_SubmitForStep4Completed;
                deviceInstallServiceClient.GetAlarmParaCommandResultCompleted += deviceInstallServiceClient_GetAlarmParaCommandResultCompleted;

                _timer.Interval = TimeSpan.FromSeconds(2);
                _timer.Tick += _timer_Tick;

                InitSettingItem();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InstallInitiateSuiteViewModel()", ex);
            }
        }

        private void InitSettingItem()
        {
            try
            {
                InitSettingItems.Clear();
                TickCount = 60;
                _tryCountWhenNull = 2;
                _setAlarmParaItem = new InitSettingItem()
                {
                    //Content = "设置一键报警后主动上传视频",
                    Content = ApplicationContext.Instance.StringResourceReader.GetString("SetOneKeyAlertedUploadVedio"),
                    CommandState = CommandStateEnum.UnDelivered,
                    Command = new ActionCommand<object>(obj => SendAutoUploadAlarmCommand(obj as InitSettingItem)),
                    Enable = false
                };
                InitSettingItems.Add(_setAlarmParaItem);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void deviceInstallServiceClient_GetAlarmParaCommandResultCompleted(object sender, GetAlarmParaCommandResultCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == true)
                {
                    StopAndEnable();
                    return;
                }

                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    return;
                }

                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }

                    if (result.ExceptionMessage != null)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), result.ExceptionMessage);
                    }
                }
                else
                {
                    if (result.Result == null)
                    {
                        if (_tryCountWhenNull <= 1)
                        {
                            StopAndEnable();
                            _setAlarmParaItem.CommandState = CommandStateEnum.UnDelivered;
                            _tryCountWhenNull = 2;
                        }
                        _tryCountWhenNull--;
                    }
                    else
                    {
                        _setAlarmParaItem.CommandState = result.Result.SuccessFlag;
                        if (_setAlarmParaItem.CommandState == CommandStateEnum.Succeed)
                        {
                            _timer.Stop();
                        }
                        else if (_setAlarmParaItem.CommandState == CommandStateEnum.Failed)
                        {
                            StopAndEnable();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void StopAndEnable()
        {
            _timer.Stop();
            _setAlarmParaItem.Enable = true;
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            TickCount--;
            deviceInstallServiceClient.GetAlarmParaCommandResultAsync(InstallInfo.Id);
            if (TickCount == 0)
            {
                StopAndEnable();
                _setAlarmParaItem.CommandState = CommandStateEnum.Failed;
            }
        }

        private void SendAutoUploadAlarmCommand(InitSettingItem obj)
        {
            try
            {
                _setAlarmParaItem.Enable = false;

                var param = new Gsafety.PTMS.ServiceReference.MessageServiceExt.SetAlarmPara()
                {
                    CommandID = Guid.NewGuid().ToString(),
                    MDVRID = InstallInfo.DeviceCoreId,
                    SuccessFlag = Gsafety.PTMS.ServiceReference.MessageServiceExt.CommandStateEnum.Delivering,
                    InstallationDetailID = InstallInfo.Id,
                    AlarmBeforeTime = ApplicationContext.Instance.ServerConfig.AlarmParamAlarmBeforeTime,
                    AlarmEndTime = ApplicationContext.Instance.ServerConfig.AlarmParamAlarmEndTime,
                    RelatedData = ApplicationContext.Instance.ServerConfig.AlarmParamRelatedData,
                    ChannelList = new ObservableCollection<int>() { 0, 1, 2, 3, 4, 5, 6, 7 },
                };
                _setAlarmParaItem.CommandState = CommandStateEnum.Delivering;
                _setAlarmParaItem.CommandID = param.CommandID;

                ApplicationContext.Instance.MessageClient.SetAlarmParaCommand(param);

                TickCount = 60;
                _timer.Start();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void deviceinstallserviceClient_GetInstallationDetailCompleted(object sender, GetInstallationDetailCompletedEventArgs e)
        {
            base.deviceinstallserviceClient_GetInstallationDetailCompleted(sender, e);

            RefreshPage();
        }

        private void RefreshPage()
        {
            try
            {
                InitSettingItem();

                IsFinished = true;
                IsGetMessage = true;

                _bscDevSuitePartServiceClient.GetCameraListBySuiteInfoIDAsync(InstallInfo.DeviceKey);

                _timer.Start();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            //deviceInstallServiceClient.GetAlarmParaCommandResultAsync(InstallInfoResultInfo.Id);
        }

        void _bscDevSuitePartServiceClient_GetCameraListBySuiteInfoIDCompleted(object sender, GetCameraListBySuiteInfoIDCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == true)
                {
                    return;
                }

                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), PTMSBaseViewModel.ServerError, MessageDialogButton.Ok);

                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    return;
                }

                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(result.ErrorMsg), MessageDialogButton.Ok);

                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }

                    if (result.ExceptionMessage != null)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), result.ExceptionMessage);
                    }
                }
                else
                {
                    CameraInfoList.Clear();
                    var list = e.Result.Result.OrderBy(t => t.Name).ToList();
                    foreach (var camera in list)
                    {
                        var cameraInfoEx = new CameraInfoEx()
                        {
                            SuitPartID = camera.ID,
                            SuitPartSn = camera.PartSn,
                            ChannelID = ""
                        };
                        cameraInfoEx.PropertyChanged += cameraInfoEx_PropertyChanged;

                        CameraInfoList.Add(cameraInfoEx);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void deviceInstallServiceClient_SubmitForStep4Completed(object sender, SubmitForStep4CompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == true)
                {
                    return;
                }

                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), PTMSBaseViewModel.ServerError, MessageDialogButton.Ok);

                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    return;
                }

                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(result.ErrorMsg), MessageDialogButton.Ok);

                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }

                    if (result.ExceptionMessage != null)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), result.ExceptionMessage);
                    }
                }
                else
                {
                    GoNextPage();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private bool CheckEditInfo()
        {
            if (_setAlarmParaItem.CommandState != CommandStateEnum.Succeed)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("PleaseInitDevice"));
                return false;
            }

            if (CameraInfoList.Any(t => string.IsNullOrEmpty(t.ChannelID)))
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("PleaseSelectCameraInstallPlace"));
                return false;
            }

            return true;
        }

        void cameraInfoEx_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName != "ChannelID")
                {
                    return;
                }

                var currentItem = sender as CameraInfoEx;
                var list = CameraInfoList.Where(t => t.ChannelID == currentItem.ChannelID && t != currentItem).ToList();
                foreach (var item in list)
                {
                    item.ChannelID = "";
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void GoNextPage()
        {
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.InstallSuiteFunctionCheckV, new Dictionary<string, object>() { { "ID", _InstallID } }));
        }

        protected override void NextPage()
        {
            try
            {
                if (CheckEditInfo() == false)
                {
                    return;
                }

                var cameraInfo = CameraInfoList.Select(t => new CameraInfo()
                {
                    SuitPartID = t.SuitPartID,
                    ChannelID = t.ChannelID,
                    InstallLocation = t.InstallLocation.Value
                }).ToList();

                deviceInstallServiceClient.SubmitForStep4Async(InstallInfo.Id, new ObservableCollection<CameraInfo>(cameraInfo));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SubmitForStep4Async Nextpage", ex);
            }
        }

        protected override void DeactivateView(string viewName)
        {
            try
            {
                base.DeactivateView(viewName);

                _timer.Stop();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }

    public class CameraInfoEx : INotifyPropertyChanged
    {
        public CameraInstallLocationEnum? InstallLocation
        {
            get
            {
                if (string.IsNullOrEmpty(ChannelID))
                {
                    return null;
                }
                else
                {
                    return (CameraInstallLocationEnum)int.Parse(ChannelID);
                }
            }
        }

        private string _channelID;
        public string ChannelID
        {
            get { return _channelID; }
            set
            {
                if (_channelID != value)
                {
                    _channelID = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ChannelID"));
                        PropertyChanged(this, new PropertyChangedEventArgs("InstallLocation"));
                    }
                }
            }
        }

        public string SuitPartID { get; set; }

        public string SuitPartSn { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class InitSettingItem : INotifyPropertyChanged
    {
        public SetAlarmPara Param { get; set; }

        public string CommandID { get; set; }

        public string Content { get; set; }

        private CommandStateEnum _state;
        public CommandStateEnum CommandState
        {
            get { return _state; }
            set
            {
                _state = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CommandState"));
                }
            }
        }

        public ICommand Command { get; set; }

        private bool _enable;
        public bool Enable
        {
            get
            {
                return _enable;
            }
            set
            {
                _enable = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Enable"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
