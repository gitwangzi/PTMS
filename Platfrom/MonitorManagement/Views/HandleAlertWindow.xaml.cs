using Gsafety.Common.Controls;
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

namespace Gsafety.Ant.Monitor.Views
{
    public partial class HandleAlertWindow : ChildWindow
    {
        VehicleAlertServiceClient vehicleAlertServiceClient = ServiceClientFactory.Create<VehicleAlertServiceClient>();

        public string AlertId { get; set; }
        public DateTime CurrentDate { get; set; }
        public string VehicleId
        {
            get
            {
                return txtVehicle.Text;
            }
            set
            {
                txtVehicle.Text = value;
            }
        }

        public string SuiteId
        {
            get
            {
                return txtSuiteID.Text;
            }
            set
            {
                txtSuiteID.Text = value;
            }
        }

        public string Memo
        {
            get
            {
                return txtContent.Text;
            }
            set
            {
                txtContent.Text = value;
            }
        }

        public HandleAlertWindow()
        {
            InitializeComponent();
            vehicleAlertServiceClient.InsertBusinessAlertHandleCompleted += vehicleAlertServiceClient_InsertBusinessAlertHandleCompleted;
        }

        void vehicleAlertServiceClient_InsertBusinessAlertHandleCompleted(object sender, InsertBusinessAlertHandleCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.DialogResult = true;
            }
        }


        void vehicleAlertServiceClient_AddVechileAlertTreatmentCompleted(object sender, AddVechileAlertTreatmentCompletedEventArgs e)
        {
            
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            BusinessAlertHandle alerthandler = new BusinessAlertHandle();
            alerthandler.BusinessAlertID = AlertId;
            alerthandler.ID = Guid.NewGuid().ToString();
            alerthandler.HandleUser = ApplicationContext.Instance.AuthenticationInfo.UserID;
            CurrentDate = DateTime.Now;
            alerthandler.HandleTime = CurrentDate.ToUniversalTime();
            alerthandler.Content = Memo;

            vehicleAlertServiceClient.InsertBusinessAlertHandleAsync(alerthandler);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

