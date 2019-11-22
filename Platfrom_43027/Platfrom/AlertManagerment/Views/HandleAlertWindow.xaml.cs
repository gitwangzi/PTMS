using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.ServiceReference.VehicleAlertService;
using Gsafety.PTMS.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Alert.Views
{
    public partial class HandleAlertWindow : ChildWindow
    {
        VehicleAlertServiceClient vehicleAlertServiceClient = ServiceClientFactory.Create<VehicleAlertServiceClient>();
        #region prop
        public DateTime? CurrentDate { get; set; }
        public VehicleAlertDetail VehicleAlertDetailModel { get; set; }
        public string AlertId { get; set; }

        public string VehicleId { get; set; }

        public string SuiteId { get; set; }

        public Action<bool> ResultAction;

        private int _AlertType;
        public int VEAlertType
        {
            get { return _AlertType; }
            set { _AlertType = value; }
        }

        private DateTime _AlertTime;
        public DateTime AlertTime
        {
            get { return _AlertTime; }
            set { _AlertTime = value; }
        }

        private string _MDvrid;
        public string MDvrid
        {
            get { return _MDvrid; }
            set { _MDvrid = value; }
        }
        #endregion

        public HandleAlertWindow()
        {
            InitializeComponent();
            vehicleAlertServiceClient.GetVehicleAlertDetailCompleted += vehicleAlertServiceClient_GetVehicleAlertDetailCompleted;
            vehicleAlertServiceClient.AddVechileAlertTreatmentCompleted += vehicleAlertServiceClient_AddVechileAlertTreatmentCompleted;
            CurrentDate = DateTime.Now;
            this.DataContext = this;
        }

        #region wcf completed
        void vehicleAlertServiceClient_AddVechileAlertTreatmentCompleted(object sender, AddVechileAlertTreatmentCompletedEventArgs e)
        {
            if (e.Result.Result)
            {
                ResultAction(true);
            }
            else
            {
                ResultAction(false);
            }
           
        }

        void vehicleAlertServiceClient_GetVehicleAlertDetailCompleted(object sender, GetVehicleAlertDetailCompletedEventArgs e)
        {
            VehicleAlertDetailModel = e.Result.Result;
            if (VehicleAlertDetailModel != null)
            {
                this.OKButton.IsEnabled = true;
                this.VehicleId = VehicleAlertDetailModel.VehicleId;
                this.SuiteId = VehicleAlertDetailModel.SuiteId;
            }
            this.DataContext = VehicleAlertDetailModel;
        }
        #endregion

        #region
        public void GetVehicleDetail(string vehicleAlertId,string id,DateTime TIME)
        {
            this.AlertId = vehicleAlertId;
            this.AlertTime = TIME;
            MDvrid = id;
            vehicleAlertServiceClient.GetVehicleAlertDetailAsync(vehicleAlertId);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            ApplicationContext.Instance.MessageManager.SendCompleteAlertMessage(new CompleteAlert() { AlertTime = AlertTime, MdvrCoreId = MDvrid });
            vehicleAlertServiceClient.AddVechileAlertTreatmentAsync(new VehicleAlertTreatment
            {
                AlertId = this.AlertId,
                MDVRID=this.MDvrid,
                Alerttype=this.VEAlertType,
                Content = this.txtContext.Text,
                DisposeStaff = ApplicationContext.Instance.AuthenticationInfo.UserName,
                AlertTime=this.AlertTime
            });
            this.DialogResult = true;

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        #endregion
    }
}

