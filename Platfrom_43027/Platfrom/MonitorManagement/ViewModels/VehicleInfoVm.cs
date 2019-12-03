using Gsafety.Common.Controls;
using Gsafety.PTMS.BasicPage.Views;
using Gsafety.PTMS.ServiceReference.ChauffeurService;
using Gsafety.PTMS.ServiceReference.VehicleMonitorService;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
namespace Gsafety.PTMS.Monitor.ViewModels
{
    [ExportAsViewModel(MonitorName.VehicleInfoViewModle)]
    public class VehicleInfoVm : BaseViewModel,
        IEventSink<Gsafety.PTMS.Bases.Models.Vehicle>,
        IEventSink<OpenState>,
        IPartImportsSatisfiedNotification
    {
        private Gsafety.PTMS.Bases.Models.Vehicle _CurrentVehicle;
        private bool _IsVisual = false;
        private int _SelectItemIndex;

        private ChauffeurServiceClient client;
        private VehicleMonitorServiceClient monitorClient;

        private ObservableCollection<Chauffeur> _chauffeurList;
        /// <summary>
        /// 驾驶员列表
        /// </summary>
        public ObservableCollection<Chauffeur> ChauffeurList
        {
            get
            {
                return this._chauffeurList;
            }
            set
            {
                this._chauffeurList = value;
                RaisePropertyChanged(() => this.ChauffeurList);
            }
        }

        private Visibility _isNoteVisibility;
        /// <summary>
        /// 备注标签的显示和隐藏控制属性
        /// </summary>
        public Visibility IsNoteVisibility
        {
            get
            {
                return this._isNoteVisibility;
            }
            set
            {
                this._isNoteVisibility = value;
                RaisePropertyChanged(() => this.IsNoteVisibility);
            }
        }


        public VehicleInfoVm()
        {
            try
            {
                m_IsOpen = true;
                _IsVisual = true;
                this.IsNoteVisibility = Visibility.Collapsed;
                this.ChauffeurList = new ObservableCollection<Chauffeur>();
                this.InilitClient();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void InitiMonitorClient()
        {
            monitorClient = ServiceClientFactory.Create<VehicleMonitorServiceClient>();
            monitorClient.GetLastMonitorGPSCompleted += MonitorClient_GetLastMonitorGPSCompleted;
        }

        private void MonitorClient_GetLastMonitorGPSCompleted(object sender, GetLastMonitorGPSCompletedEventArgs e)
        {
            try
            {
                if ((e.Result != null) && (e.Result.Result != null))
                {
                    if(e.Result.Result.Source == 0)
                    {
                        this.GPSSource = ApplicationContext.Instance.StringResourceReader.GetString("CarKit");
                    }

                    if(e.Result.Result.Source == 1)
                    {
                        this.GPSSource = "AVL";
                    }
                    RaisePropertyChanged(() => GPSSource);
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        private void InilitClient()
        {
            client = ServiceClientFactory.Create<ChauffeurServiceClient>();
            client.GetChauffeurByVehicleCompleted += client_GetChauffeurByVehicleCompleted;
        }

        void client_GetChauffeurByVehicleCompleted(object sender, GetChauffeurByVehicleCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    return;
                }
                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                }
                if (e.Result.IsSuccess == false)
                {
                    if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                    {
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ErrorMsg);
                    }
                    else
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ExceptionMessage);
                    }
                }
                else
                {
                    this.ChauffeurList = new ObservableCollection<Chauffeur>();
                    //this.ChauffeurList.Clear();
                    if (e.Result.Result.Any())
                    {
                        if (this.ChauffeurList.Any())
                        {
                            this.ChauffeurList.Clear();
                        }
                        this.ChauffeurList = e.Result.Result;
                    }
                    else
                    {
                        if (this.ChauffeurList.Any())
                        {
                            this.ChauffeurList.Clear();
                        }
                        this.ChauffeurList.Insert(0, new Chauffeur()
                        {
                            ID = Guid.NewGuid().ToString(),
                            Name = ApplicationContext.Instance.StringResourceReader.GetString("Null"),
                            Phone = ApplicationContext.Instance.StringResourceReader.GetString("Null"),
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleInfoVm.client_GetChauffeurVehicleCompleted", ex);
                this.InilitClient();
            }
            
        }

        public int SelectItemIndex
        {
            get { return _SelectItemIndex; }
            set
            {
                _SelectItemIndex = value;
            }
        }

        private bool m_IsOpen = true;

        private string districeCode;
        /// <summary>
        /// 行政区划名称
        /// </summary>
        public string DistrictCode
        {
            get
            {
                return this.districeCode;
            }
            set
            {
                this.districeCode = value;
                RaisePropertyChanged(() => this.DistrictCode);
            }
        }


        public bool IsOpen
        {
            get { return m_IsOpen; }
            set
            {
                //m_IsOpen = value;
            }
        }

        public bool IsVisual
        {
            get { return _IsVisual; }
            set
            {
                //if (IsOpen)
                //{
                //    IsOpen = _IsVisual = false;
                //}
                //else
                //{
                //    IsOpen = _IsVisual = true;
                //}
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsVisual));

            }
        }

        private string _gpsSource;
        public string GPSSource
        {
            get { return _gpsSource; }
            set
            {
                _gpsSource = value;
                RaisePropertyChanged(() => GPSSource);
            }
        }


        public Gsafety.PTMS.Bases.Models.Vehicle CurrentVehicle
        {
            get { return _CurrentVehicle; }
            set
            {
                _CurrentVehicle = value;
                _SelectItemIndex = 0;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentVehicle));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectItemIndex));
                RaisePropertyChanged("PlayVideoButtonVisible");
                RaisePropertyChanged("VehicleId");

            }
        }
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                SelectItemIndex = 0;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectItemIndex));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex); 
            }
        }

        public void HandleEvent(Gsafety.PTMS.Bases.Models.Vehicle publishedEvent)
        {
            try
            {
                if (string.IsNullOrEmpty(publishedEvent.VehicleId))
                {
                    CurrentVehicle = null;
                    return;
                }
                CurrentVehicle = publishedEvent;

                if (!string.IsNullOrEmpty(CurrentVehicle.Note))
                {
                    this.IsNoteVisibility = Visibility.Visible;
                }
                else
                {
                    this.IsNoteVisibility = Visibility.Collapsed;
                }

                if (!string.IsNullOrEmpty(CurrentVehicle.DistrictCode))
                {

                    if (CurrentVehicle.DistrictCode.Length == 5)
                    {

                        string provicecode = CurrentVehicle.DistrictCode.Substring(0, 2);
                        var province = ApplicationContext.Instance.BufferManager.DistrictManager.Provinces.FirstOrDefault(n => n.Code == provicecode);

                        if (province != null)
                        {
                            CurrentVehicle.ProvinceName = province.Name;
                        }
                        var city = ApplicationContext.Instance.BufferManager.DistrictManager.Cities.FirstOrDefault(n => n.Code == CurrentVehicle.DistrictCode);
                        if (city != null)
                        {
                            CurrentVehicle.CityName = city.Name;
                        }
                        this.DistrictCode = CurrentVehicle.ProvinceName + "/" + CurrentVehicle.CityName;
                    }

                }

                if(this.monitorClient == null)
                {
                    this.InitiMonitorClient();
                }
                this.monitorClient.GetLastMonitorGPSAsync(CurrentVehicle.VehicleId);

                if (this.client == null)
                {
                    this.InilitClient();
                }
                this.client.GetChauffeurByVehicleAsync(CurrentVehicle.VehicleId, ApplicationContext.Instance.AuthenticationInfo.ClientID);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
                this.InilitClient();
            }
        }

        public void OnImportsSatisfied()
        {
            try
            {
                EventAggregator.SubscribeOnDispatcher<Gsafety.PTMS.Bases.Models.Vehicle>(this);
                EventAggregator.SubscribeOnDispatcher<OpenState>(this);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public void HandleEvent(OpenState publishedEvent)
        {
            //IsVisual = publishedEvent.State;
        }

        #region 视频播放
        private ICommand _playVideoCommand;
        public ICommand PlayVideoCommand
        {
            get
            {
                if (_playVideoCommand == null)
                {
                    _playVideoCommand = new ActionCommand<object>(a => Play(a));
                }
                return _playVideoCommand;
            }
        }

        private void Play(object suiteID)
        {
            var cameraWindow = new CameraSelectWindow(suiteID as string, 4);
            cameraWindow.Closed += cameraWindow_Closed;
            cameraWindow.Show();
        }

        void cameraWindow_Closed(object sender, EventArgs e)
        {
            var winodw = sender as CameraSelectWindow;
            if (winodw.DialogResult == true && winodw.SelectResult.Count > 0)
            {
                var info = new MediaInfo()
                {
                    MediaInfoItems = winodw.SelectResult,
                    IsHideProgressControl = true
                };

                ApplicationContext.Instance.EventAggregator.Publish<MediaInfo>(info);
            }
        }


        #endregion
    }


    /// <summary>
    /// Forms open state
    /// </summary>
    public class OpenState
    {

        public bool State { get; set; }

    }
}
