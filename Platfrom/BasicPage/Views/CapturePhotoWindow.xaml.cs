using Gsafety.Common.Controls;
using Gsafety.PTMS.BasicPage.ViewModels;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Gsafety.PTMS.BasicPage.Views
{
    public partial class CapturePhotoWindow : ChildWindow
    {
        private List<CheckBox> CameraCheckBoxList = new List<CheckBox>();

        private CapturePhotoWindowViewModel viewModel;
        private ObservableCollection<CameraInfo> CameraInfos = new ObservableCollection<CameraInfo>();
        public CapturePhotoWindow(string mdvrCoreID)
        {
            InitializeComponent();
            viewModel = new CapturePhotoWindowViewModel(mdvrCoreID);
            this.DataContext = viewModel;

            CameraCheckBoxList.Add(OuterBehind);
            CameraCheckBoxList.Add(OuterBefore);
            CameraCheckBoxList.Add(OuterRight);
            CameraCheckBoxList.Add(OuterLeft);
            CameraCheckBoxList.Add(InnerLeftDriver);
            CameraCheckBoxList.Add(InnerRightDriver);
            CameraCheckBoxList.Add(InnerCenter);
            CameraCheckBoxList.Add(InnerBehind);
            InitClient();
            GetCameraInstallLocationInfo(mdvrCoreID);

            this.MouseRightButtonUp += CapturePhotoWindow_MouseRightButtonUp;
            this.MouseRightButtonDown += CapturePhotoWindow_MouseRightButtonDown;
            comboStatus.DropDownOpened += PopupHandler.OnDropDown;
            comboStatus.DropDownClosed += PopupHandler.OnDropDown;
            IsMark.DropDownOpened += PopupHandler.OnDropDown;
            IsMark.DropDownClosed += PopupHandler.OnDropDown;
            channel.DropDownOpened += PopupHandler.OnDropDown;
            channel.DropDownClosed += PopupHandler.OnDropDown;
            orderBy.DropDownOpened += PopupHandler.OnDropDown;
            orderBy.DropDownClosed += PopupHandler.OnDropDown;
            tp_EndTime.DropDownOpened += PopupHandler.OnTimerDropDown;
            tp_EndTime.DropDownClosed += PopupHandler.OnTimerDropDown;
            tp_StartTime.DropDownOpened += PopupHandler.OnTimerDropDown;
            tp_StartTime.DropDownClosed += PopupHandler.OnTimerDropDown;
        }

        void CapturePhotoWindow_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        void CapturePhotoWindow_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void takePhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.TakePhotoCommand.Execute(CameraCheckBoxList);
        }

        private void battchTakePhoto_Click(object sender, RoutedEventArgs e)
        {
            viewModel.BattchTakePhotoCommand.Execute(CameraCheckBoxList);
        }

        private void img_MouseEnter(object sender, MouseEventArgs e)
        {
            this.imgDetal.Visibility = System.Windows.Visibility.Visible;
        }

        private void img_MouseLeave(object sender, MouseEventArgs e)
        {
            this.imgDetal.Visibility = System.Windows.Visibility.Collapsed;
        }

        private DeviceInstallServiceClient InitClient()
        {
            DeviceInstallServiceClient deviceClient = ServiceClientFactory.Create<DeviceInstallServiceClient>();
            ServiceClientFactory.CreateMessageHeader(deviceClient.InnerChannel);
            deviceClient.GetCameraInfoByMdvrIDCompleted += deviceClient_GetCameraInfoByMdvrIDCompleted;
            return deviceClient;
        }

        private void GetCameraInstallLocationInfo(string mdvrCoreID)
        {
            DeviceInstallServiceClient client = InitClient();
            client.GetCameraInfoByMdvrIDAsync(mdvrCoreID);
        }

        void deviceClient_GetCameraInfoByMdvrIDCompleted(object sender, GetCameraInfoByMdvrIDCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return;
            }

            if (e.Error != null)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                return;
            }

            try
            {
                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }

                    if (result.ExceptionMessage != null)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), result.ExceptionMessage);
                    }
                }

                if (result.Result != null)
                {
                    CameraInfos = result.Result;
                    viewModel.CameraInstalledInfos = CameraInfos.ToList();
                    viewModel.InitChannelList();
                    SetCameraVisibility();
                }
            }
            catch (System.Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                DeviceInstallServiceClient client = sender as DeviceInstallServiceClient;
                CloseClient(client);
            }
        }

        private void CloseClient(DeviceInstallServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }

            client = null;
        }

        private void SetCameraVisibility()
        {
            foreach (var item in CameraInfos)
            {
                switch (item.InstallLocation)
                {
                    case CameraInstallLocationEnum.InnerBehind:
                        InnerBehind.Visibility = Visibility.Visible;
                        InnerBehind.Tag = item;
                        break;
                    case CameraInstallLocationEnum.InnerCenter:
                        InnerCenter.Visibility = Visibility.Visible;
                        InnerCenter.Tag = item;
                        break;
                    case CameraInstallLocationEnum.InnerLeftDriver:
                        InnerLeftDriver.Visibility = Visibility.Visible;
                        InnerLeftDriver.Tag = item;
                        break;
                    case CameraInstallLocationEnum.InnerRightDriver:
                        InnerRightDriver.Visibility = Visibility.Visible;
                        InnerRightDriver.Tag = item;
                        break;
                    case CameraInstallLocationEnum.OuterBefore:
                        OuterBefore.Visibility = Visibility.Visible;
                        OuterBefore.Tag = item;
                        break;
                    case CameraInstallLocationEnum.OuterBehind:
                        OuterBehind.Visibility = Visibility.Visible;
                        OuterBehind.Tag = item;
                        break;
                    case CameraInstallLocationEnum.OuterLeft:
                        OuterLeft.Visibility = Visibility.Visible;
                        OuterLeft.Tag = item;
                        break;
                    case CameraInstallLocationEnum.OuterRight:
                        OuterRight.Visibility = Visibility.Visible;
                        OuterRight.Tag = item;
                        break;
                }
            }
        }

        private void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                viewModel.DoubleClickCommand.Execute(null);
            }
        }

        private void photoInterval_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.photoInterval.Text == "0")
            {
                this.photoInterval.Text = "1";
            }
        }

        private void txtNore_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            viewModel.MoreCommand.Execute(null);
        }

        private void txtCancell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            viewModel.CancellCommand.Execute(null);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                viewModel.DoubleClickCommand.Execute(null);
            }
        }

        private void txtNore_MouseMove(object sender, MouseEventArgs e)
        {
            //SolidColorBrush solidColorBrush = new SolidColorBrush();
            //solidColorBrush.Color = "#36a3dc".ToColor();
            //this.txtNore.Foreground = solidColorBrush;

            this.txtNore.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 191, 255));
        }

        private void txtNore_MouseLeave(object sender, MouseEventArgs e)
        {
            this.txtNore.Foreground = new SolidColorBrush(Colors.White);
        }

        private void img_MouseMove(object sender, MouseEventArgs e)
        {
            this.imgDetal.Visibility = System.Windows.Visibility.Visible;
        }
    }
}

