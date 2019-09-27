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
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Share;
using System.Collections.ObjectModel;
using Gsafety.PTMS.ServiceReference.VehicleAlertService;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using Gsafety.PTMS.Bases.Enums;
using System.Reflection;
using Gsafety.Common.Controls;

namespace Gsafety.PTMS.SecuritySuite.ViewModels
{
    [ExportAsViewModel(SecuritySuiteName.VehicleElectronicFenceVM)]
    public class VehicleElectronicFenceVM : BaseViewModel
    {
        TrafficManageServiceClient trafficManageClient;
        VehicleAlertServiceClient vehicleAlertClient;

        public ICommand QueryCommand { get; set; }
        public string VehicleID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ObservableCollection<VehicleFenceAlert> VehicleFenceAlertList { get; set; }
        public PagedCollectionView PagedVehicleFenceAlertList { get; set; }
        public List<int> PageSizeList { get; set; }

        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (PagedVehicleFenceAlertList != null)
                {
                    PagedVehicleFenceAlertList.PageSize = value;
                }
            }
        }
        public Fence SelectedFence { get; set; }

        private ObservableCollection<Fence> _fenceList;
        public ObservableCollection<Fence> FenceList
        {
            get
            {
                return this._fenceList;
            }
            set
            {
                this._fenceList = value;
                RaisePropertyChanged(() => this.FenceList);
            }
        }

        /// <summary>
        /// waiting
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

        public FenceAlert SelectedFenceAlert { get; set; }

        public ObservableCollection<FenceAlert> FenceAlerts { get; set; }

        public VehicleElectronicFenceVM()
        {
            try
            {
                trafficManageClient = ServiceClientFactory.Create<TrafficManageServiceClient>();
                vehicleAlertClient = ServiceClientFactory.Create<VehicleAlertServiceClient>();
                QueryCommand = new ActionCommand<object>(x => QueryAction());
                //UpdateCommand = new ActionCommand<object>(obj => UpdateAction());
                StartDate = DateTime.Now.AddDays(-1);
                EndDate = DateTime.Now;
                PageSizeList = Gsafety.PTMS.BaseInformation.BaseInformationCommon.PageSizeList;
                PageSizeValue = PageSizeList[0];

                FenceAlerts = new ObservableCollection<FenceAlert>();

                FenceAlerts.Add(new FenceAlert { Id = null, Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_All") });
                //FenceAlerts.Add(new FenceAlert { Id = (int)BusinessAlertType.InFence, Name = ApplicationContext.Instance.StringResourceReader.GetString("Fence_Enter") });
                //FenceAlerts.Add(new FenceAlert { Id = (int)BusinessAlertType.OutFence, Name = ApplicationContext.Instance.StringResourceReader.GetString("Fence_Out") });
                SelectedFenceAlert = FenceAlerts[0];
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => this.FenceAlerts));

                vehicleAlertClient.GetVehicleFenceAlertCompleted += vehicleAlertClient_GetVehicleFenceAlertCompleted;
                trafficManageClient.GetAllLFenceCompleted += trafficManageClient_GetAllLFenceCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleElectronicFenceVM()", ex);
            }
        }

        protected override void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                ServiceClientFactory.CreateMessageHeader(trafficManageClient.InnerChannel);
                trafficManageClient.GetAllLFenceAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleElectronicFenceVm ActivateView", ex);
            }
        }
    

        void vehicleAlertClient_GetVehicleFenceAlertCompleted(object sender, GetVehicleFenceAlertCompletedEventArgs e)
        {
            try
            {

                if (e.Result != null)
                {
                    VehicleFenceAlertList = e.Result.Result;

                    if (VehicleFenceAlertList == null || VehicleFenceAlertList.Count == 0)
                    {
                        //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_Notice"), MessageBoxButton.OK);
                    }
                    PagedVehicleFenceAlertList = new PagedCollectionView(VehicleFenceAlertList);
                    PagedVehicleFenceAlertList.PageSize = this.PageSizeValue;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => this.VehicleFenceAlertList));
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => this.PagedVehicleFenceAlertList));

                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("vehicleAlertClient_GetVehicleFenceAlertCompleted()", ex);
            }         
        }

        private void QueryAction()
        {
            try
            {
                string carNumber = VehicleID;
                if (carNumber != null)
                {
                    carNumber = carNumber.ToUpper().Trim();
                }
                DateTime searchEndDate = EndDate;
                searchEndDate = searchEndDate.AddDays(1);

                DateTime dtStart = Convert.ToDateTime(StartDate.ToShortDateString());
                DateTime dtEnd = Convert.ToDateTime(EndDate.ToShortDateString()).AddDays(1);
                string strSelectFence = null;
                short? strSelectFenceAlert = null;
                if (SelectedFenceAlert.Id != null)
                    strSelectFenceAlert = SelectedFenceAlert.Id;
                else
                    strSelectFenceAlert = -1;
                if (SelectedFence != null && SelectedFence.OBJECTID != 0)
                    strSelectFence = SelectedFence.OBJECTID.ToString();
                ServiceClientFactory.CreateMessageHeader(vehicleAlertClient.InnerChannel);
               
                vehicleAlertClient.GetVehicleFenceAlertAsync(carNumber, strSelectFence, (short)strSelectFenceAlert, dtStart, dtEnd);
                IsBusy = true;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleElectronicFenceVm QueryAction", ex);
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_Notice"), MessageDialogButton.Ok);
            }

        }
        void trafficManageClient_GetAllLFenceCompleted(object sender, GetAllLFenceCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    ObservableCollection<Fence> listFence = e.Result.Result;
                    FenceList = new ObservableCollection<Fence>(listFence.Distinct(new FenceCompare()));
                    Fence item = new Fence { OBJECTID = 0, NAME = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_All") };
                    FenceList.Insert(0, item);
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => this.FenceList));
                    SelectedFence = FenceList[0];
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => this.SelectedFence));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("trafficManageClient_GetAllLFenceCompleted()", ex);
            }
        }

    }
    public class FenceAlert
    {
        public short? Id { get; set; }
        public string Name { get; set; }
    }
    /// <summary>
    /// class FenceCompare
    /// </summary>
    public class FenceCompare : IEqualityComparer<Fence>
    {
        public bool Equals(Fence x, Fence y)
        {
            return x.OBJECTID == y.OBJECTID;
        }

        public int GetHashCode(Fence s)
        {
            return s.OBJECTID.GetHashCode();
        }
    }
}
