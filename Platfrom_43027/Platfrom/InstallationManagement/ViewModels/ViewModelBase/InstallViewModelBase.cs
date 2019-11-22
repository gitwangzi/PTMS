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
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.ServiceReference.SecuritySuiteService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.ServiceReference.WorkingSuiteService;
using Gsafety.PTMS.ServiceReference.VedioService;
using Gsafety.PTMS.ServiceReference.AppConfigService;
using System.Reflection;
using Gsafety.PTMS.Share;
using System.Collections.Generic;


namespace Gsafety.PTMS.Installation.ViewModels
{
    public class InstallViewModelBase : BaseEntityViewModel
    {
        protected int step = 0;
        protected string _InstallID = string.Empty;

        protected DeviceInstallServiceClient deviceInstallServiceClient = null;

        public IActionCommand NextCommand { get; protected set; }
        public IActionCommand QuitCommand { get; protected set; }
        public IActionCommand GetCommand { get; protected set; }

        public InstallViewModelBase()
        {
            try
            {
                deviceInstallServiceClient = ServiceClientFactory.Create<DeviceInstallServiceClient>();
                NextCommand = new ActionCommand<object>(obj => NextPage());
                QuitCommand = new ActionCommand<object>(obj => Quit());
                GetCommand = new ActionCommand<object>(obj => Get());
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InstallViewModelBase()", ex);
            }
        }

        protected virtual void GoNextPage()
        {

        }

        string _ImageSource = null;
        public string ImageSource
        {
            get
            {
                return _ImageSource;
            }
            set
            {
                _ImageSource = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ImageSource));
            }
        }

        protected bool isGetMessage = true;
        public bool IsGetMessage
        {
            get
            {
                return isGetMessage;
            }
            set
            {
                isGetMessage = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsGetMessage));
            }
        }

        protected bool isFinished = true;
        public bool IsFinished
        {
            get
            {
                return isFinished;
            }
            set
            {
                isFinished = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsFinished));
            }
        }

        InstallationInfo _InstallInfoResult = null;
        public InstallationInfo InstallInfo
        {
            get { return _InstallInfoResult; }
            set
            {
                _InstallInfoResult = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => InstallInfo));
            }
        }

        protected virtual void NextPage() { }
        protected virtual void Quit() { }

        protected virtual void Get() { }
        protected virtual void ResetData() { }

        protected override void DeactivateView(string viewName)
        {
            base.DeactivateView(viewName);
            InstallInfo = null;
        }
    }
}

