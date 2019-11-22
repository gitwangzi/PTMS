using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
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
using Jounce.Framework;
using Gsafety.PTMS.Traffic.Views;
using Jounce.Core.View;
using System.Collections.Generic;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.Bases.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Jounce.Core.Event;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.Traffic.Models;


namespace Gsafety.PTMS.Traffic.ViewModels
{
    [ExportAsViewModel(TrafficName.TrafficMainPageViewModel)]
    public partial class TrafficMainPageViewModel : BaseViewModel,
        IEventSink<SetTrafficePageBusyArgs>, IEventSink<ShowFenceInfoArgs>, IEventSink<SelectedRouteChange>, IEventSink<SelectedSpeedLimitChange>, IEventSink<TrafficFeature>,
                IPartImportsSatisfiedNotification
    {
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);

            EventAggregator.Publish(TrafficName.TrafficMenuV.AsViewNavigationArgs());
            //EventAggregator.Publish(GisManagement.GisName.GpsCarHisDataViewMonitor.AsViewNavigationArgs());
            EventAggregator.Publish(TrafficName.TrafficManagerDetailInfoView.AsViewNavigationArgs());
            //ApplicationContext.Instance.CurrentGISName = GisManagement.GisName.TrafficGisView;
            EventAggregator.Publish(TrafficName.SendVehicleDetailV.AsViewNavigationArgs());
            object mview = ApplicationContext.Instance.MenuManager.Router.ViewQuery(TrafficName.TrafficMainPage);
            Frame frame = (mview as UserControl).FindName("ContentFrame") as Frame;
            if (frame.CurrentSource == null)
                return;
            frame.Refresh();
        }

        public TrafficMainPageViewModel()
        {
            PageSizeValue = 10;
            InitialDetail();
            InitPagination();
        }

        /// <summary>
        /// Wait state
        /// </summary>
        private bool _IsBusy = false;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                _IsBusy = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsBusy));
            }
        }

        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<SetTrafficePageBusyArgs>(this);
            EventAggregator.SubscribeOnDispatcher<ShowFenceInfoArgs>(this);
            EventAggregator.SubscribeOnDispatcher<SelectedRouteChange>(this);
            EventAggregator.SubscribeOnDispatcher<SelectedSpeedLimitChange>(this);
            EventAggregator.SubscribeOnDispatcher<TrafficFeature>(this);
        }

        public void HandleEvent(SetTrafficePageBusyArgs publishedEvent)
        {
            IsBusy = publishedEvent.bBusy;
            //if (IsBusy == false)
            //{
            //    EventAggregator.Publish<RefreshTrafficSelectStatus>(new RefreshTrafficSelectStatus() { });
            //}
        }
    }
}
