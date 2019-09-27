using BaseLib.ViewModels;
using GisManagement.Models;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BasicPage.Views;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using Jounce.Framework.Command;
using System;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HistoryQueryManagement.ViewModels
{
    [ExportAsViewModel(HistoryQueryName.HistoryQueryMainVm)]
    public partial class HistoryQueryMainPageViewModel : PTMSBaseViewModel,
        IEventSink<ClearMapIsExecuted>,
        IPartImportsSatisfiedNotification
    {
        public HistoryQueryMainPageViewModel()
        {
            try
            {
                InitalAlarm();
                InitalAlert();
                InitalDeviceAlert();
                this.IsAlarmAnswerNoteVisibility = Visibility.Collapsed;
                this.IsAlarmNoteVisibility = Visibility.Collapsed;
                this.IsAlertNoteVisibility = Visibility.Collapsed;
                this.ApealStatus = "";
                this.AlertAlertTime = "";
                this.AlertHandlePerson = "";
                this.Note = "";
                HistoricalVideoCommand = new ActionCommand<object>((obj) => HistoricalVideo_Event(obj));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void HistoricalVideo_Event(object vechileId)
        {
            var cvm = new HistoryVideoManageWindow(vechileId.ToString());
            cvm.Closed += cvm_Closed;
            cvm.Show();
        }

        void cvm_Closed(object sender, EventArgs e)
        {
            var window = sender as HistoryVideoManageWindow;
            if (window.DialogResult != true)
            {
                return;
            }

            if (window.SelectVideoInfoItems.Count > 0)
            {
                var mediaPlayerInfo = new MediaInfo();
                mediaPlayerInfo.IsHideProgressControl = false;
                mediaPlayerInfo.Orientation = Orientation.Vertical;
                mediaPlayerInfo.AutoPlay = false;
                mediaPlayerInfo.ShowHistoryLine = true;

                foreach (var item in window.SelectVideoInfoItems)
                {
                    var info = new MediaInfo.MediaInfoItem()
                    {
                        StartTime = item.Model.StartTime,
                        EndTime = item.Model.EndTime,
                        Url = item.Model.FileID,
                        Channel = (int)item.CameraInstallLocation,
                        IsRealVideo = false,
                        IsShowControlBar = false,
                        IsShowProcessBar = false,
                        ShowRemoveBtn = false,
                    };
                    mediaPlayerInfo.MediaInfoItems.Add(info);
                }

                mediaPlayerInfo.VehicleId = window.HistoryVideoManageContentViewModel.CarNo;
                EventAggregator.Publish<MediaInfo>(mediaPlayerInfo);
            }
        }

        public ICommand HistoricalVideoCommand { get; private set; }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                EventAggregator.Publish(GisManagement.GisName.MonitorGisView.AsViewNavigationArgs());
            }
            catch (Exception ex)
            {

            }

            RefreshGIS();

            RefreshDetail();

            ApplicationContext.Instance.CurrentGISName = GisManagement.GisName.MonitorGisView;

            object mview = ApplicationContext.Instance.MenuManager.Router.ViewQuery(HistoryQueryName.HistoryQueryMainV);
            Frame frame = (mview as UserControl).FindName("ContentFrame") as Frame;

            if (frame.CurrentSource == null)
                return;
            frame.Refresh();


        }

        public void Accordion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Accordion accordion = sender as Accordion;
            object obj = accordion.SelectedItem;
            AccordionItem item = obj as AccordionItem;
            selectedheader = item.Header.ToString();

            RefreshDetail();

            RefreshGIS();
        }

        private void RefreshDetail()
        {
            if (selectedheader == ApplicationContext.Instance.StringResourceReader.GetString("AlarmHistory"))
            {
                AlarmVisiblity = Visibility.Visible;
                AlertVisiblity = Visibility.Collapsed;
            }
            else if (selectedheader == ApplicationContext.Instance.StringResourceReader.GetString("AlertHistory"))
            {
                AlarmVisiblity = Visibility.Collapsed;
                AlertVisiblity = Visibility.Visible;
            }
        }

        private void RefreshGIS()
        {
            if (selectedheader == ApplicationContext.Instance.StringResourceReader.GetString("AlarmHistory"))
            {
                EventAggregator.Publish<GisDisplayControlEvent>(
                new GisDisplayControlEvent()
                {
                    Display = GisDisplayControlType.miMonitor_Alarm
                });
            }
            else if (selectedheader == ApplicationContext.Instance.StringResourceReader.GetString("AlertHistory"))
            {
                EventAggregator.Publish<GisDisplayControlEvent>(
                new GisDisplayControlEvent()
                {
                    Display = GisDisplayControlType.miMonitor_Alert
                });

            }
        }

        public string selectedheader = ApplicationContext.Instance.StringResourceReader.GetString("AlarmHistory");

        public void HandleEvent(ClearMapIsExecuted publishedEvent)
        {
            foreach (Gsafety.PTMS.ServiceReference.VehicleAlarmService.AlarmInfoEx ainfo in _allAlarms)
            {
                if (ainfo.IsMarkGraphic == true) ainfo.IsMarkGraphic = false;
            }

            foreach (Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx ainfo in _allAlerts)
            {
                if (ainfo.IsMarkGraphic == true) ainfo.IsMarkGraphic = false;
            }
        }

        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<ClearMapIsExecuted>(this);
        }
    }
}
