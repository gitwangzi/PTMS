using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.VedioService;
using Gsafety.PTMS.Share;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.BasicPage.Views
{
    public partial class PhotoMarkWindow : ChildWindow
    {
        private List<string> photoList = new List<string>();
        public int Status { get; set; }
        public string MarkContent { get; set; }
        public int IsAction { get; set; }
        public PhotoMarkWindow(List<Photo> list)
        {
            InitializeComponent();
            IsAction = 0;
            Status = 0;
            photoList = list.Select(p => p.ID).ToList();
            if (list.Any(t => t.Note != null && t.Note != ""))
            {
                this.cancellBtn.Visibility = Visibility.Visible;
            }
            else
            {
                this.cancellBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            if (this.markContent.Text != string.Empty)
            {
                Status = 1;
                IsAction = 1;
                this.MarkContent = this.markContent.Text;
                VedioServiceClient client = InitClient();
                client.SetPhotoMarkAsync(new ObservableCollection<string>(photoList), Status, this.markContent.Text);
            }
            else
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"),
                    ApplicationContext.Instance.StringResourceReader.GetString("MarkContentEmpty"), MessageDialogButton.Ok);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cancellBtn_Click(object sender, RoutedEventArgs e)
        {
            Status = 0;
            IsAction = 1;
            this.MarkContent = string.Empty;
            VedioServiceClient client = InitClient();
            client.SetPhotoMarkAsync(new ObservableCollection<string>(photoList), Status, string.Empty);
        }

        private VedioServiceClient InitClient()
        {
            VedioServiceClient client = ServiceClientFactory.Create<VedioServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.SetPhotoMarkCompleted += client_SetPhotoMarkCompleted;
            return client;
        }

        private void CloseClient(VedioServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }

        void client_SetPhotoMarkCompleted(object sender, SetPhotoMarkCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == true)
                {
                    return;
                }

                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), e.Error.ToString(), MessageDialogButton.Ok);
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    return;
                }

                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), result.ErrorMsg, MessageDialogButton.Ok);

                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }

                    if (result.ExceptionMessage != null)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), result.ExceptionMessage);
                    }
                }
                else
                {
                    if (Status == 0)
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"),
                            ApplicationContext.Instance.StringResourceReader.GetString("CancellMarkSuccess"), MessageDialogButton.Ok);
                        this.Close();
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"),
                            ApplicationContext.Instance.StringResourceReader.GetString("MarkSuccess"), MessageDialogButton.Ok);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                VedioServiceClient client = sender as VedioServiceClient;
                CloseClient(client);
            }
        }
    }
}

