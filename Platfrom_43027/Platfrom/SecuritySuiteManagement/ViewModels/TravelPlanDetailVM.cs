using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Data;

namespace Gsafety.PTMS.SecuritySuite.ViewModels
{
    [ExportAsViewModel(SecuritySuiteName.TravelPlanDetailVM)]
    public class TravelPlanDetailVM : BaseViewModel
    {
        TrafficManageServiceClient trafficManageClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
        public ICommand ReturnCommand { get; set; }
        public ObservableCollection<TravelDetail> TravelDetailList { get; set; }
        public PagedCollectionView PagedTravelDetailList { get; set; }

        public TravelPlanResult SelectedPlanResult { get; set; }

        public List<int> PageSizeList { get; set; }
        public int PageSizeValue { get; set; }

        public TravelPlanDetailVM()
        {
            ReturnCommand = new ActionCommand<object>(x => ReturnAction());
            PageSizeList = Gsafety.PTMS.BaseInformation.BaseInformationCommon.PageSizeList;
            PageSizeValue = PageSizeList[0];
            trafficManageClient.GetTravelPlanResultDetailListCompleted += trafficManageClient_GetTravelPlanResultDetailListCompleted;
        }

        void trafficManageClient_GetTravelPlanResultDetailListCompleted(object sender, GetTravelPlanResultDetailListCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                TravelDetailList = e.Result.Result;
                PagedTravelDetailList = new PagedCollectionView(TravelDetailList);
                PagedTravelDetailList.PageSize = PageSizeValue;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => this.TravelDetailList));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => this.PagedTravelDetailList));
                if (TravelDetailList == null || TravelDetailList.Count == 0)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_Notice"), MessageBoxButton.OK);
                }
            }
        }

        private void ReturnAction()
        {
            EventAggregator.Publish(new ViewNavigationArgs(SecuritySuiteName.TravelPlanImplementationV));
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            SelectedPlanResult = viewParameters["PlanID"] as TravelPlanResult;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => this.SelectedPlanResult));
            //trafficManageClient.GetTravelPlanResultDetailListAsync(SelectedPlanResult.ID);
        }
    }
}
