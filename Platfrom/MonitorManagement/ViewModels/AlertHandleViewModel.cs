using BaseLib.Model;
using Gsafety.Ant.Monitor.Models;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Monitor;
using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using Gsafety.PTMS.ServiceReference.VehicleAlertService;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Ant.Monitor.ViewModels
{
    [ExportAsViewModel(MonitorName.MonitorAlertHandleViewModel)]
    public class AlertHandleViewModel : BaseViewModel, IEventSink<MonitorAlertInfoDisplay>, IPartImportsSatisfiedNotification, IEventSink<AlertHandleDisplayArgs>
    {
        ICommand okcommand = null;

        public ICommand OKCommand
        {
            get { return okcommand; }
            set { okcommand = value; }
        }
        ICommand cancelcommand = null;

        public ICommand CancelCommand
        {
            get { return cancelcommand; }
            set { cancelcommand = value; }
        }
        public AlertHandleViewModel()
        {
            okcommand = new ActionCommand<string>(obj => HandleAlert(obj));
            cancelcommand = new ActionCommand<string>(obj => Cancel(obj));


        }

        private void Cancel(string obj)
        {
            IsVisual = false;
        }

        private void HandleAlert(string obj)
        {
            try
            {
                BusinessAlertHandle alerthandler = new BusinessAlertHandle();
                alerthandler.BusinessAlertID = alertinfo.Id;
                alerthandler.ID = Guid.NewGuid().ToString();
                alerthandler.HandleUser = ApplicationContext.Instance.AuthenticationInfo.Account;
                alerthandler.HandleTime = HandleTime.Value.ToUniversalTime();
                alerthandler.Content = Note;

                VehicleAlertServiceClient vehicleAlertServiceClient = InilitClient();
                vehicleAlertServiceClient.InsertBusinessAlertHandleAsync(alerthandler);

                IsCommitEnable = false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        private bool _iscommitenable = true;
        public bool IsCommitEnable
        {
            get
            {
                return _iscommitenable;
            }
            set
            {
                _iscommitenable = value;
                RaisePropertyChanged(() => IsCommitEnable);
            }
        }

        private bool m_IsOpen = true;
        public bool IsOpen
        {
            get
            {
                return m_IsOpen;
            }
            set
            {
                m_IsOpen = value;
            }
        }

        private bool _IsVisual = true;
        public bool IsVisual
        {
            get
            {
                return _IsVisual;
            }
            set
            {
                if (IsOpen != value)
                {
                    _IsVisual = value;
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsVisual));
            }
        }

        private string _vehicleid;
        /// <summary>
        /// 
        /// </summary>
        public string VehicleID
        {
            get
            {
                return this._vehicleid;
            }
            set
            {
                this._vehicleid = value;
                RaisePropertyChanged(() => VehicleID);
            }
        }

        private string _note;
        /// <summary>
        /// 
        /// </summary>
        public string Note
        {
            get
            {
                return this._note;
            }
            set
            {
                this._note = value;
                RaisePropertyChanged(() => Note);
            }
        }


        private string _handler;
        /// <summary>
        /// 
        /// </summary>
        public string Handler
        {
            get
            {
                return this._handler;
            }
            set
            {
                this._handler = value;
                RaisePropertyChanged(() => Handler);
            }
        }
        private DateTime? _alerttime;
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AlertTime
        {
            get
            {
                return this._alerttime;
            }
            set
            {
                this._alerttime = value;
                RaisePropertyChanged(() => AlertTime);
            }
        }
        private DateTime? _handletime;
        /// <summary>
        /// 
        /// </summary>
        public DateTime? HandleTime
        {
            get
            {
                return this._handletime;
            }
            set
            {
                this._handletime = value;
                RaisePropertyChanged(() => HandleTime);
            }
        }

        string alertid = string.Empty;

        BusinessAlertEx alertinfo = null;

        private VehicleAlertServiceClient InilitClient()
        {
            VehicleAlertServiceClient vehicleAlertServiceClient = ServiceClientFactory.Create<VehicleAlertServiceClient>();
            vehicleAlertServiceClient.InsertBusinessAlertHandleCompleted += vehicleAlertServiceClient_InsertBusinessAlertHandleCompleted;

            return vehicleAlertServiceClient;
        }

        void vehicleAlertServiceClient_InsertBusinessAlertHandleCompleted(object sender, InsertBusinessAlertHandleCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
                else
                {
                    if (e.Result.IsSuccess == false)
                    {
                        if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ErrorMsg);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                            ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ExceptionMessage);
                        }
                    }
                    else
                    {
                        EventAggregator.Publish<AlertHandleResult>(e.Result.Result);
                        IsVisual = false;
                    }
                }
            }
            finally
            {
                VehicleAlertServiceClient vehicleAlertServiceClient = sender as VehicleAlertServiceClient;
                if (vehicleAlertServiceClient != null)
                {
                    vehicleAlertServiceClient.CloseAsync();
                    vehicleAlertServiceClient = null;
                }
            }

        }

        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<AlertHandleDisplayArgs>(this);
            EventAggregator.SubscribeOnDispatcher<MonitorAlertInfoDisplay>(this);
        }

        public void HandleEvent(AlertHandleDisplayArgs publishedEvent)
        {
            IsVisual = publishedEvent.Show;
        }

        public void HandleEvent(MonitorAlertInfoDisplay publishedEvent)
        {
            try
            {
                if (publishedEvent.DisPlayInfo != null)
                {
                    alertinfo = publishedEvent.DisPlayInfo;
                    alertid = publishedEvent.DisPlayInfo.Id;

                    Handler = ApplicationContext.Instance.AuthenticationInfo.Account;
                    VehicleID = publishedEvent.DisPlayInfo.VehicleId;
                    AlertTime = publishedEvent.DisPlayInfo.AlertTime;

                    if (publishedEvent.DisPlayInfo.Status != 0)
                    {
                        IsCommitEnable = false;
                        Note = publishedEvent.DisPlayInfo.Note;
                        HandleTime = publishedEvent.DisPlayInfo.HandleTime;
                    }
                    else
                    {
                        HandleTime = DateTime.Now;
                        Note = string.Empty;
                        IsCommitEnable = true;
                    }
                }
                else
                {
                    IsCommitEnable = false;
                    alertid = string.Empty;
                    AlertTime = null;
                    Handler = null;
                    HandleTime = null;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
