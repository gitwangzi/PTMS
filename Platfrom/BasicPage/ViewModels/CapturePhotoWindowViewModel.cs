using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gsafety.Common.Controls;
using Message = Gsafety.PTMS.ServiceReference.MessageServiceExt;
using Gsafety.PTMS.ServiceReference.VedioService;
using Jounce.Core.Event;
using CameraInstallLocationEnum = Gsafety.PTMS.ServiceReference.DeviceInstallService.CameraInstallLocationEnum;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.BasicPage.Views;
namespace Gsafety.PTMS.BasicPage.ViewModels
{
    public class CapturePhotoWindowViewModel : BaseViewModel, IEventSink<Message.TakePictureMessageResponse>
    {
        public ICommand LastCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand TakePhotoCommand { get; private set; }
        public ICommand BattchTakePhotoCommand { get; private set; }
        public ICommand QueryCommand { get; private set; }
        public ICommand MarkCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand CancellCommand { get; private set; }
        public ICommand DoubleClickCommand { get; private set; }
        public ICommand MoreCommand { get; private set; }

        //0 表示单拍，非0表示连拍
        private int takePictureType = 0;
        private int SelectChanelCount = 0;
        private string _mdvrCoreID;
        string serverConfig = "http://" + ApplicationContext.Instance.ServerConfig.VideoServiceFileServerIP + ":" +
                                          ApplicationContext.Instance.ServerConfig.VideoServiceFileServerPort + "/";
        private ObservableCollection<string> deleteList = new ObservableCollection<string>();
        public CapturePhotoWindowViewModel(string mdvrCoreID)
        {
            try
            {
                _mdvrCoreID = mdvrCoreID;
                InitResolution();
                //InitChannelList();
                InitIsMarkList();
                InitOrderList();
                InitPhotoList();
                this.Img_Url = "/ExternalResource;component/Images/VideoBackground.jpg";
                LastCommand = new ActionCommand<object>(LastAction);
                NextCommand = new ActionCommand<object>(NextAction);
                TakePhotoCommand = new ActionCommand<object>(TakePhotoAction);
                BattchTakePhotoCommand = new ActionCommand<object>(BattchTakePhotoAction);
                QueryCommand = new ActionCommand<object>(QueryAction);
                MarkCommand = new ActionCommand<object>(MarkAction);
                DeleteCommand = new ActionCommand<object>(DeleteAction);
                EditCommand = new ActionCommand<object>(EditAction);
                CancellCommand = new ActionCommand<object>(CancellAction);
                DoubleClickCommand = new ActionCommand<object>(DoubleClickAction);
                MoreCommand = new ActionCommand<object>(MoreAction);
                ApplicationContext.Instance.EventAggregator.Subscribe<Message.TakePictureMessageResponse>(this);

                VehicleInfo = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.getVehicleByMdvrid(mdvrCoreID);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public Vehicle VehicleInfo { get; set; }

        private List<CheckBox> checkBoxList = new List<CheckBox>();
        private void TakePhotoAction(object obj)
        {
            try
            {
                takePictureType = 0;
                AlreadyPhotoNum = 0;
                checkBoxList = obj as List<CheckBox>;
                if (checkBoxList.Any(t => t.IsChecked == true))
                {
                    Message.TakePictureArgs arg = new Message.TakePictureArgs();
                    arg.UserID = ApplicationContext.Instance.AuthenticationInfo.UserID;
                    arg.Mdvr_Core_Sn = _mdvrCoreID;

                    //arg.Channel = new ObservableCollection<int>(checkBoxList.Where(t => t.IsChecked == true).Select(p => Convert.ToInt32(p.Content)));

                    arg.Channel = new ObservableCollection<int>();
                    var selectCameras = checkBoxList.Where(t => t.IsChecked == true).Select(t => t.Tag as CameraInfo).ToList();
                    foreach (var item in selectCameras)
                    {
                        arg.Channel.Add(int.Parse(item.ChannelID));
                    }

                    arg.Cmd = 1;
                    arg.Interval = 0;
                    arg.Resolution = this.SelectResolution.EnumValue;
                    arg.Quality = 1;
                    arg.Brightness = 255;
                    arg.Contrast = 127;
                    arg.Saturation = 127;
                    arg.Color = 255;
                    ApplicationContext.Instance.MessageClient.SendTakePictureCMD(arg);
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"),
                            ApplicationContext.Instance.StringResourceReader.GetString("Tip_SelectCamera"), MessageDialogButton.Ok);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        DateTime battchStartTime;
        private int TotalInterval = 0;
        DateTime battchEndTime = DateTime.Now.AddSeconds(-1);
        private void BattchTakePhotoAction(object obj)
        {
            try
            {
                if (DateTime.Now > battchEndTime)
                {
                    SelectChanelCount = checkBoxList.Where(t => t.IsChecked == true).Count();
                    battchStartTime = DateTime.Now;
                    TotalInterval = int.Parse(TotalPhotoNum) * SelectChanelCount * int.Parse(TakePhotoInterval);
                    battchEndTime = battchStartTime.AddSeconds(TotalInterval);

                    takePictureType = 1;
                    AlreadyPhotoNum = 0;
                    checkBoxList = obj as List<CheckBox>;
                    if (checkBoxList.Any(t => t.IsChecked == true))
                    {
                        Message.TakePictureArgs arg = new Message.TakePictureArgs();
                        arg.UserID = ApplicationContext.Instance.AuthenticationInfo.UserID;
                        arg.Mdvr_Core_Sn = _mdvrCoreID;

                        //arg.Channel = new ObservableCollection<int>(checkBoxList.Where(t => t.IsChecked == true).Select(p => Convert.ToInt32(p.Content)));

                        arg.Channel = new ObservableCollection<int>();
                        var selectCameras = checkBoxList.Where(t => t.IsChecked == true).Select(t => t.Tag as CameraInfo).ToList();
                        foreach (var item in selectCameras)
                        {
                            arg.Channel.Add(int.Parse(item.ChannelID));
                        }

                        arg.Cmd = int.Parse(this.TotalPhotoNum);
                        arg.Interval = int.Parse(this.TakePhotoInterval);
                        arg.Resolution = this.SelectResolution.EnumValue;
                        arg.Quality = 1;
                        arg.Brightness = 255;
                        arg.Contrast = 127;
                        arg.Saturation = 127;
                        arg.Color = 255;
                        ApplicationContext.Instance.MessageClient.SendTakePictureCMD(arg);
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"),
                                ApplicationContext.Instance.StringResourceReader.GetString("Tip_SelectCamera"), MessageDialogButton.Ok);
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"),
                                ApplicationContext.Instance.StringResourceReader.GetString("Tip_ContinueShooting"), MessageDialogButton.Ok);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public void HandleEvent(Message.TakePictureMessageResponse publishedEvent)
        {
            try
            {
                var temp = publishedEvent.Photo;
                if (temp != null)
                {
                    var photo = new Gsafety.PTMS.ServiceReference.VedioService.Photo();
                    photo.ID = temp.ID;
                    photo.ClientID = temp.ClientID;
                    photo.DeviceSn = temp.DeviceSn;
                    photo.DeviceType = temp.DeviceType;
                    photo.ChannelID = temp.ChannelID;
                    photo.Img_Size = temp.Img_Size;
                    photo.Img_Url = serverConfig + temp.Img_Url;
                    photo.MiniImg_Url = serverConfig + temp.MiniImg_Url;
                    photo.Note = temp.Note;
                    photo.Status = temp.Status;
                    photo.IsChecked = false;
                    photo.Create_Time = temp.Create_Time.ToLocalTime();

                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => PhotoList.Insert(0, photo));
                }

                if (takePictureType == 1)
                {
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => AlreadyPhotoNum += 1);
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => TotalRecord += 1);
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => ShowRecord += 1);
                }
                else
                {
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => ShowRecord += 1);
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => TotalRecord += 1);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private int CurrentPageNum = 1;
        private int _totalRecord = 0;
        public int TotalRecord
        {
            get { return _totalRecord; }
            set
            {
                _totalRecord = value;
                RaisePropertyChanged(() => TotalRecord);
            }
        }

        private int _showRecord = 0;
        public int ShowRecord
        {
            get { return _showRecord; }
            set
            {
                _showRecord = value;
                RaisePropertyChanged(() => ShowRecord);
            }
        }

        private void QueryAction(object obj)
        {
            try
            {
                CurrentPageNum = 1;
                this.ShowRecord = 0;
                QueryPhotoFileListArgs arg = new QueryPhotoFileListArgs();
                arg.MdvrCoreSn = this._mdvrCoreID;
                arg.DeviceType = 0;
                arg.ChannelID = SelectChannel.EnumValue;
                arg.Start_Time = Convert.ToDateTime(this.SelectDate.Value.Date.Add(new TimeSpan(StartTime.Value.Hour, StartTime.Value.Minute, StartTime.Value.Second))).ToUniversalTime();
                arg.End_Time = Convert.ToDateTime(this.SelectDate.Value.Date.Add(new TimeSpan(EndTime.Value.Hour, EndTime.Value.Minute, EndTime.Value.Second))).ToUniversalTime();
                arg.Status = (PhotoStatusEnum)IsSelectMark.EnumValue;
                arg.PageNum = CurrentPageNum;
                arg.PageSize = 10;
                arg.Note = NoteKeyWords;
                arg.OrderBy = SelectOrder.EnumValue;

                VedioServiceClient client = InitClient();
                client.GetPhotoListAsync(arg);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void MoreAction(object obj)
        {
            try
            {
                CurrentPageNum += 1;
                if ((CurrentPageNum * 10 - TotalRecord) > 10)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"),
                        ApplicationContext.Instance.StringResourceReader.GetString("NoMorePictures"), MessageDialogButton.Ok);
                    return;
                }

                QueryPhotoFileListArgs arg = new QueryPhotoFileListArgs();
                arg.MdvrCoreSn = this._mdvrCoreID;
                arg.DeviceType = 0;
                arg.ChannelID = SelectChannel.EnumValue;
                arg.Start_Time = Convert.ToDateTime(this.SelectDate.Value.Date.Add(new TimeSpan(StartTime.Value.Hour, StartTime.Value.Minute, StartTime.Value.Second))).ToUniversalTime();
                arg.End_Time = Convert.ToDateTime(this.SelectDate.Value.Date.Add(new TimeSpan(EndTime.Value.Hour, EndTime.Value.Minute, EndTime.Value.Second))).ToUniversalTime();
                arg.Status = (PhotoStatusEnum)IsSelectMark.EnumValue;
                arg.PageNum = CurrentPageNum;
                arg.PageSize = 10;
                arg.Note = NoteKeyWords;
                arg.OrderBy = SelectOrder.EnumValue;

                VedioServiceClient client = InitClient();
                client.GetPhotoListAsync(arg);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void LastAction(object obj)
        {
            var index = PhotoList.IndexOf(SelectPhoto);
            if (index > 0)
            {
                SelectPhoto = PhotoList[index - 1];
            }
        }

        private void NextAction(object obj)
        {
            var index = PhotoList.IndexOf(SelectPhoto);
            if (index < PhotoList.Count)
            {
                SelectPhoto = PhotoList[index + 1];
            }
        }

        private void MarkAction(object obj)
        {
            try
            {
                if (PhotoList.Any(t => t.IsChecked == true))
                {

                    PhotoMarkWindow markWindow = new PhotoMarkWindow(PhotoList.Where(t => t.IsChecked == true).ToList());
                  
                    markWindow.Show();
                    markWindow.Closed += markWindow_Closed;
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("SelectMarkPhoto"), MessageDialogButton.Ok);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void markWindow_Closed(object sender, EventArgs e)
        {
            PhotoMarkWindow markWindow = sender as PhotoMarkWindow;
            foreach (var item in PhotoList)
            {
                if (markWindow.IsAction == 1 && item.IsChecked == true)
                {
                    if (markWindow.Status == 1)
                    {
                        item.Status = true;
                    }
                    else
                    {
                        item.Status = false;
                    }

                    item.Note = markWindow.MarkContent;
                }
            }
        }

        private void DeleteAction(object obj)
        {
            if (PhotoList.Any(t => t.IsChecked == true))
            {
                var dialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
                dialogResult.Closed += dialogResult_Closed;
            }
            else
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"),
                               ApplicationContext.Instance.StringResourceReader.GetString("SelectDeletePhoto"), MessageDialogButton.Ok);
            }
        }

        void dialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    deleteList.Clear();
                    deleteList = new ObservableCollection<string>(PhotoList.Where(t => t.IsChecked == true).Select(p => p.ID));
                    VedioServiceClient client = InitClient();
                    client.DeletePhotoAsync(deleteList);
                }
            }
        }

        private void EditAction(object obj)
        {
            this.IsStackPanelVisibility = true;
            foreach (var item in PhotoList)
            {
                item.IsVisibility = true;
            }
        }

        private void CancellAction(object obj)
        {
            this.IsStackPanelVisibility = false;
            foreach (var item in PhotoList)
            {
                item.IsChecked = false;
                item.IsVisibility = false;
            }

            this.IsAllChecked = false;
        }

        private void DoubleClickAction(object obj)
        {
            PhotoMaxWindow maxWindow = new PhotoMaxWindow(SelectPhoto);
            maxWindow.Show();
        }

        private bool _isStackPanelVisibility = false;
        public bool IsStackPanelVisibility
        {
            get { return _isStackPanelVisibility; }
            set
            {
                _isStackPanelVisibility = value;
                RaisePropertyChanged(() => IsStackPanelVisibility);
            }
        }

        #region Photo Binding

        private string _img_Url;
        public string Img_Url
        {
            get { return _img_Url; }
            set
            {
                _img_Url = value;
                RaisePropertyChanged(() => Img_Url);
            }
        }

        private string _createTime;
        public string CreateTime
        {
            get { return _createTime; }
            set
            {
                _createTime = value;
                RaisePropertyChanged(() => CreateTime);
            }
        }

        private string _channelID;
        public string ChannelID
        {
            get { return _channelID; }
            set
            {
                _channelID = value;
                RaisePropertyChanged(() => ChannelID);
            }
        }

        private string _imgSize;
        public string ImgSize
        {
            get { return _imgSize; }
            set
            {
                _imgSize = value;
                RaisePropertyChanged(() => ImgSize);
            }
        }

        private string _note;
        public string Note
        {
            get { return _note; }
            set
            {
                _note = value;
                RaisePropertyChanged(() => Note);
            }
        }
        #endregion

        #region InitResolution
        private List<EnumModel> _resolutionList = new List<EnumModel>();
        public List<EnumModel> ResolutionList
        {
            get { return _resolutionList; }
            set
            {
                _resolutionList = value;
                RaisePropertyChanged(() => ResolutionList);
            }
        }

        private EnumModel _selectResolution = new EnumModel();
        public EnumModel SelectResolution
        {
            get { return _selectResolution; }
            set
            {
                _selectResolution = value;
                RaisePropertyChanged(() => SelectResolution);
            }
        }

        private void InitResolution()
        {
            ResolutionList.Add(new EnumModel()
            {
                EnumValue = 5,
                EnumName = "R_Five",
                ShowName = "176 * 144",
            });
            ResolutionList.Add(new EnumModel()
            {
                EnumValue = 1,
                EnumName = "R_one",
                ShowName = "320 * 240",
            });
            ResolutionList.Add(new EnumModel()
            {
                EnumValue = 6,
                EnumName = "R_Six",
                ShowName = "353 * 288",
            });
            ResolutionList.Add(new EnumModel()
            {
                EnumValue = 7,
                EnumName = "R_Seven",
                ShowName = "704 * 288",
            });
            ResolutionList.Add(new EnumModel()
            {
                EnumValue = 2,
                EnumName = "R_two",
                ShowName = "640 * 480",
            });
            ResolutionList.Add(new EnumModel()
            {
                EnumValue = 8,
                EnumName = "R_Eight",
                ShowName = "704 * 576",
            });
            ResolutionList.Add(new EnumModel()
            {
                EnumValue = 3,
                EnumName = "R_Three",
                ShowName = "800 * 600",
            });
            ResolutionList.Add(new EnumModel()
            {
                EnumValue = 4,
                EnumName = "R_Four",
                ShowName = "1024 * 768",
            });

            this.SelectResolution = ResolutionList[6];
        }
        #endregion

        #region TakePhotos Binding
        private int _alreadyPhotoNum = 0;
        public int AlreadyPhotoNum
        {
            get { return _alreadyPhotoNum; }
            set
            {
                _alreadyPhotoNum = value;
                RaisePropertyChanged(() => AlreadyPhotoNum);
            }
        }

        private string _totalPhotoNum = "5";
        public string TotalPhotoNum
        {
            get { return _totalPhotoNum; }
            set
            {
                int result;
                if (int.TryParse(value, out result))
                {
                    if (result > 10)
                    {
                        result = 10;
                    }
                    _totalPhotoNum = result.ToString();
                }
                RaisePropertyChanged(() => TotalPhotoNum);
            }
        }

        private string _takePhotoInterval = "5";
        public string TakePhotoInterval
        {
            get { return _takePhotoInterval; }
            set
            {
                int result;
                if (int.TryParse(value, out result))
                {
                    if (result > 60)
                    {
                        result = 60;
                    }
                    _takePhotoInterval = result.ToString();
                }

                RaisePropertyChanged(() => TakePhotoInterval);
            }
        }

        #endregion

        private string _noteKeyWords;
        public string NoteKeyWords
        {
            get { return _noteKeyWords; }
            set
            {
                _noteKeyWords = value;
                RaisePropertyChanged(() => NoteKeyWords);
            }
        }

        private Photo _selectPhoto = new Photo();
        public Photo SelectPhoto
        {
            get { return _selectPhoto; }
            set
            {
                _selectPhoto = value;
                RaisePropertyChanged(() => SelectPhoto);
                if (SelectPhoto != null)
                {
                    BindSelectPhoto();
                }
            }
        }

        private bool _isAllChecked = false;
        public bool IsAllChecked
        {
            get { return _isAllChecked; }
            set
            {
                _isAllChecked = value;
                RaisePropertyChanged(() => IsAllChecked);
                SetPhotoChecked();
            }
        }

        private ObservableCollection<Photo> _photoList = new ObservableCollection<Photo>();

        public ObservableCollection<Photo> PhotoList
        {
            get { return _photoList; }
            set
            {
                _photoList = value;
                RaisePropertyChanged(() => PhotoList);
            }
        }

        private void BindSelectPhoto()
        {
            if (SelectPhoto != null)
            {
                this.Img_Url = SelectPhoto.Img_Url;
                //this.ImgSize = SelectPhoto.Img_Size;
                this.ChannelID = SelectPhoto.ChannelID.ToString();
                this.CreateTime = SelectPhoto.Create_Time.ToString("yyyy-MM-dd HH:mm:ss");
                this.Note = SelectPhoto.Note;
            }
        }

        private void SetPhotoChecked()
        {
            foreach (var item in PhotoList)
            {
                item.IsChecked = this.IsAllChecked;
            }
        }

        #region InitChannelList Status Order

        DateTime? _selectDate = DateTime.Now;
        public DateTime? SelectDate
        {
            get
            {
                return _selectDate;
            }
            set
            {
                if (value != _selectDate)
                {
                    _selectDate = value;
                    RaisePropertyChanged(() => this.StartTime);
                }
            }
        }

        DateTime? _starttime = DateTime.Now.Date;
        public DateTime? StartTime
        {
            get
            {
                return _starttime;
            }
            set
            {
                if (value != _starttime)
                {
                    _starttime = value;
                    RaisePropertyChanged(() => this.StartTime);
                }
            }
        }
        DateTime? _endtime = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
        public DateTime? EndTime
        {
            get
            {
                return _endtime;
            }
            set
            {
                if (value != _endtime)
                {
                    _endtime = value;
                    RaisePropertyChanged(() => this.EndTime);
                }
            }
        }

        private List<EnumModel> _channelList = new System.Collections.Generic.List<EnumModel>();

        public List<EnumModel> ChannelList
        {
            get { return _channelList; }
            set
            {
                _channelList = value;
                RaisePropertyChanged(() => ChannelList);
            }
        }

        private EnumModel _selectChannel = new EnumModel();

        public EnumModel SelectChannel
        {
            get { return _selectChannel; }
            set
            {
                _selectChannel = value;
                RaisePropertyChanged(() => SelectChannel);
            }
        }

        public List<CameraInfo> CameraInstalledInfos { get; set; }

        public void InitChannelList()
        {
            var adapter = new EnumAdapter<Gs.PTMS.Common.Data.Enum.CameraInstallLocationEnum>();
            var categorys = adapter.GetEnumInfos();
            foreach (var item in categorys)
            {
                foreach (var camera in CameraInstalledInfos)
                {
                    if (item.Value == (short)camera.InstallLocation)
                    {
                        var model = new EnumModel();
                        model.EnumName = item.Name;
                        //model.ShowName = item.Value.ToString() + " " + item.LocalizedString;
                        //model.EnumValue = item.Value;
                        model.ShowName = camera.ChannelID+" " +item.LocalizedString;                       
                        model.EnumValue = int.Parse(camera.ChannelID);
                        ChannelList.Add(model);
                    }
                }
            }

            ChannelList.Insert(0, new EnumModel()
            {
                EnumName = "",
                EnumValue = 99,
                ShowName = ApplicationContext.Instance.StringResourceReader.GetString("All")
            });

            SelectChannel = ChannelList[0];
        }

        private List<EnumModel> _isMarkList = new System.Collections.Generic.List<EnumModel>();

        public List<EnumModel> IsMarkList
        {
            get { return _isMarkList; }
            set
            {
                _isMarkList = value;
                RaisePropertyChanged(() => IsMarkList);
            }
        }

        private EnumModel _isSelectMark = new EnumModel();

        public EnumModel IsSelectMark
        {
            get { return _isSelectMark; }
            set
            {
                _isSelectMark = value;
                RaisePropertyChanged(() => IsSelectMark);
            }
        }

        private void InitIsMarkList()
        {
            var adapter = new EnumAdapter<PhotoStatusEnum>();
            var categorys = adapter.GetEnumInfos();
            foreach (var item in categorys)
            {
                var model = new EnumModel();
                model.EnumName = item.Name;
                model.ShowName = item.LocalizedString;
                model.EnumValue = item.Value;
                IsMarkList.Add(model);
            }
            IsMarkList.Reverse();

            this.IsSelectMark = IsMarkList[0];
        }

        private List<EnumModel> _orderList = new System.Collections.Generic.List<EnumModel>();
        public List<EnumModel> OrderList
        {
            get { return _orderList; }
            set
            {
                _orderList = value;
                RaisePropertyChanged(() => OrderList);
            }
        }

        private EnumModel _selectOrder = new EnumModel();

        public EnumModel SelectOrder
        {
            get { return _selectOrder; }
            set
            {
                _selectOrder = value;
                RaisePropertyChanged(() => SelectOrder);
            }
        }

        private void InitOrderList()
        {
            OrderList.Add(new EnumModel()
            {
                EnumName = "OrderByTime",
                ShowName = ApplicationContext.Instance.StringResourceReader.GetString("OrderByTime"),
                EnumValue = 0
            });
            OrderList.Add(new EnumModel()
            {
                EnumName = "Channel",
                ShowName = ApplicationContext.Instance.StringResourceReader.GetString("Channel"),
                EnumValue = 1
            });

            SelectOrder = OrderList[0];
        }

        #endregion

        private VedioServiceClient InitClient()
        {
            VedioServiceClient client = ServiceClientFactory.Create<VedioServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetPhotoListCompleted += client_GetPhotoListCompleted;
            client.DeletePhotoCompleted += client_DeletePhotoCompleted;
            return client;
        }

        void client_DeletePhotoCompleted(object sender, DeletePhotoCompletedEventArgs e)
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
                    string deletePhotoId = SelectPhoto.ID;
                    foreach (var item in deleteList)
                    {
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => PhotoList.Remove(PhotoList.FirstOrDefault(t => t.ID == item)));
                        if (item == deletePhotoId)
                        {
                            SelectPhoto = new Photo();
                        }
                    }
                    this.TotalRecord -= deleteList.Count;
                    this.ShowRecord -= deleteList.Count;
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

        void client_GetPhotoListCompleted(object sender, GetPhotoListCompletedEventArgs e)
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
                    this.TotalRecord = e.Result.TotalRecord;
                    this.ShowRecord += e.Result.Result.Count;
                    if (CurrentPageNum == 1)
                    {
                        this.PhotoList.Clear();
                    }
                    var tempList = result.Result.ToList();
                    foreach (var item in tempList)
                    {
                        item.Img_Url = serverConfig + item.Img_Url;
                        item.MiniImg_Url = serverConfig + item.MiniImg_Url;
                        item.Create_Time = item.Create_Time.ToLocalTime();
                        item.IsVisibility = false;
                    }

                    foreach (var item in tempList)
                    {
                        PhotoList.Add(item);
                    }

                    if (e.Result.TotalRecord == 0)
                    {
                        SelectPhoto = new Photo();
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

        private void CloseClient(VedioServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }

        private void InitPhotoList()
        {
            try
            {
                QueryPhotoFileListArgs arg = new QueryPhotoFileListArgs();
                arg.MdvrCoreSn = this._mdvrCoreID;
                arg.DeviceType = 0;
                arg.ChannelID = 99;
                arg.Start_Time = DateTime.UtcNow.Date;
                arg.End_Time = DateTime.UtcNow;
                arg.Status = PhotoStatusEnum.All;
                arg.PageNum = 1;
                arg.PageSize = 10;
                arg.Note = string.Empty;
                arg.OrderBy = 0;

                VedioServiceClient client = InitClient();
                client.GetPhotoListAsync(arg);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
