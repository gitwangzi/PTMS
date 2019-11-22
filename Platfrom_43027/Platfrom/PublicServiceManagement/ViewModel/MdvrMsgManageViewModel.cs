using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.BasicPage.Views;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.Enums;
using Gsafety.PTMS.ServiceReference.PublicService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using PublicServiceManagement.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;

namespace PublicServiceManagement.ViewModel
{
    [ExportAsViewModel(PublicServiceName.MdvrMsgManageVm)]
    public class MdvrMsgManageViewModel : ListViewModel<RunMdvrMessage>
    {


        #region MyRegion
        //public RunMdvrMessageClient client = null;

        private RunMdvrMessageClient InitServiceClient()
        {
            RunMdvrMessageClient client = ServiceClientFactory.Create<RunMdvrMessageClient>();
            client.GetRunMdvrMessageListCompleted += client_GetRunMdvrMessageListCompleted;
            client.DeleteRunMdvrMessageByIDCompleted += client_DeleteRunMdvrMessageByIDCompleted;
            return client;
        }



        #region complted.....

        void client_DeleteRunMdvrMessageByIDCompleted(object sender, DeleteRunMdvrMessageByIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.Result)
                    {
                        Data.RefreshPage();
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("MdvrMsgManageViewModel", ex);
            }
            finally
            {
                RunMdvrMessageClient client = sender as RunMdvrMessageClient;
                if (client != null)
                {
                    client.CloseAsync();
                    client = null;
                }
            }
        }

        void client_GetRunMdvrMessageListCompleted(object sender, GetRunMdvrMessageListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        var adapter = new EnumAdapter<MdvrMsgTypeEnum>();
                        var categorys = adapter.GetEnumInfos();
                        foreach (var item in e.Result.Result)
                        {
                            //item.MsgType = categorys.FirstOrDefault(x => x.Value == item.MessageType).LocalizedString;
                            int state = (int)item.MessageType;
                            switch (state)
                            {
                                case 0:
                                    item.MsgType = ApplicationContext.Instance.StringResourceReader.GetString("TrafficMessage");
                                    break;
                                case 1:
                                    item.MsgType = ApplicationContext.Instance.StringResourceReader.GetString("WeatherMessage");
                                    break;
                                case 2:
                                    item.MsgType = ApplicationContext.Instance.StringResourceReader.GetString("TimeMessage");
                                    break;
                                default:
                                    item.MsgType = ApplicationContext.Instance.StringResourceReader.GetString("NUll");
                                    break;
                            }
                            item.CreateTime = item.CreateTime.ToLocalTime();
                        }

                        Data.loader_Finished(new BaseLib.Model.PagedResult<RunMdvrMessage>()
                        {
                            Count = e.Result.TotalRecord,
                            Items = e.Result.Result,//数据列表
                            PageIndex = currentIndex
                        });
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception er)
            {
                ApplicationContext.Instance.Logger.LogException("MdvrMsgManageViewModel", er);
            }
            finally
            {
                RunMdvrMessageClient client = sender as RunMdvrMessageClient;
                if (client != null)
                {
                    client.CloseAsync();
                    client = null;
                }
            }
        }

        #endregion

        /// <summary>
        /// 初始化内容
        /// </summary>
        public MdvrMsgManageViewModel()
            : base()
        {
            //InitServiceClient();
            InitVehicleServiceClient();

            var adapter = new EnumAdapter<MdvrMsgTypeEnum>();
            var categorys = adapter.GetEnumInfos();

            MsgTypes.Add(new ComboBoxBasicStruct<EnumModel>()
            {
                Key = new EnumModel() { EnumValue = -1 },
                Value = ApplicationContext.Instance.StringResourceReader.GetString("All"),
            });

            MsgTypes.Add(new ComboBoxBasicStruct<EnumModel>()
            {
                Key = new EnumModel() { EnumValue = 0 },
                Value = ApplicationContext.Instance.StringResourceReader.GetString("TrafficMessage"),
            });

            MsgTypes.Add(new ComboBoxBasicStruct<EnumModel>()
            {
                Key = new EnumModel() { EnumValue = 1 },
                Value = ApplicationContext.Instance.StringResourceReader.GetString("WeatherMessage"),
            });
            MsgTypes.Add(new ComboBoxBasicStruct<EnumModel>()
            {
                Key = new EnumModel() { EnumValue = 2 },
                Value = ApplicationContext.Instance.StringResourceReader.GetString("TimeMessage"),
            });

            MsgTypeSelected = MsgTypes[0];
        }

        /// <summary>
        /// 删除
        /// </summary>
        protected override void Delete()
        {
            InitServiceClient();

            var dialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
            dialogResult.Closed += dialogResult_Closed;
        }
        private void dialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    RunMdvrMessageClient client = InitServiceClient();
                    client.DeleteRunMdvrMessageByIDAsync(CurrentSelected.ID);
                }
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        protected override void Update(string name)
        {
            MdvrMsgDetailWindow view = new MdvrMsgDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "view", CurrentSelected } });
            view.Show();
            view.Closed += (s, e) =>
            {
                Data.RefreshPage();
            };
        }

        protected override void Add(string name)
        {
            MdvrMsgDetailWindow view = new MdvrMsgDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name } });
            view.Show();
            view.Closed += (s, e) =>
            {
                Data.RefreshPage();
            };
        }

        /// <summary>
        /// 查询
        /// </summary>
        protected override void Query()
        {
            currentIndex = 1;
            Data.MoveToFirstPage();
        }

        /// <summary>
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {

            try
            {
                Data = new BaseLib.Model.PagedServerCollection<RunMdvrMessage>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    RunMdvrMessageClient client = InitServiceClient();

                    PagingInfo page = new PagingInfo();
                    page.PageIndex = pageIndex;
                    page.PageSize = BaseCommon.PageSizeList[3];
                    int type = MsgTypeSelected.Key == null ? -1 : Convert.ToInt16(MsgTypeSelected.Key.EnumValue);
                    client.GetRunMdvrMessageListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, Title, type, Content, pageIndex, pageSize);
                });

                VehicleData = new PagedServerCollection<MdvrMessageVehicle>((pageIndex, pageSize) =>
                {
                    pageSize = BaseCommon.PageSizeList[3];
                    System.Threading.Interlocked.Exchange(ref currentVehicleIndex, pageIndex);

                    RunMdvrmessageVehicleClient _client = InitialVehicleServiceClient();
                    //查询数据
                    _client.GetRunMdvrmessageVehicleListAsync(CurrentSelected.ID, pageIndex, pageSize);
                });


            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        #region Field

        RunMdvrMessage currentSelected;
        public RunMdvrMessage CurrentSelected
        {
            get { return currentSelected; }
            set
            {
                if (currentSelected != value)
                {
                    currentSelected = value;
                    if (currentSelected != null)
                    {
                        VehicleData.MoveToFirstPage();
                    }
                }
                RaisePropertyChanged(() => CurrentSelected);
            }
        }

        #endregion

        #endregion

        #region 车辆规则操作



        public ICommand BtnAddVechileCommand { get; set; }
        public ICommand BtnDeliverAllVechileCommand { get; set; }
        public ICommand BtnDeleteVechileCommand { get; set; }
        public ICommand BtnDeliverVechileCommand { get; set; }
        public ICommand BtnRefreshVechileCommand { get; set; }

        public void InitVehicleServiceClient()
        {
            BtnAddVechileCommand = new ActionCommand<object>(method => AddVechile("add"));
            BtnDeleteVechileCommand = new ActionCommand<object>(method => DeleteVechile());
            BtnDeliverAllVechileCommand = new ActionCommand<object>(method => DeliverAllVechile("update"));
            BtnDeliverVechileCommand = new ActionCommand<object>(method => DeliverVechile("update"));
            BtnRefreshVechileCommand = new ActionCommand<object>(method => RefreshVehicle());
        }

        private void RefreshVehicle()
        {
            VehicleData.RefreshPage();
        }

        private void DeliverVechile(string p)
        {
            if (CurVehicleSelected != null)
            {
                var deliverdialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("IsSendRules"), MessageDialogButton.OkAndCancel);
                deliverdialogResult.Closed += deliverdialogResult_Closed;
            }
        }

        void deliverdialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    ObservableCollection<MdvrMessageVehicle> vehicles = new ObservableCollection<MdvrMessageVehicle>();
                    vehicles.Add(CurVehicleSelected);
                    RunMdvrmessageVehicleClient client = InitialVehicleServiceClient();
                    client.DeliverRunMdvrmessageToVehicleAsync(vehicles);
                }
            }
        }

        private void AddVechile(string p)
        {
            if (CurrentSelected != null)
            {
                RunMdvrmessageVehicleClient client = InitialVehicleServiceClient();
                client.GetAllRunMdvrmessageVehicleListBySpeedIDAsync(CurrentSelected.ClientId, CurrentSelected.ID);
            }
        }


        private void DeleteVechile()
        {
            if (CurVehicleSelected != null)
            {

                var dialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
                dialogResult.Closed += dialogResult_Closed2;
            }
        }

        private void dialogResult_Closed2(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    RunMdvrmessageVehicleClient client = InitialVehicleServiceClient();
                    client.DeleteRunMdvrmessageVehicleByIDAsync(CurVehicleSelected.ID);
                }
            }
        }


        private void CloseVehicleServiceClient(RunMdvrmessageVehicleClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        private void DeliverAllVechile(string p)
        {
            if (VehicleData.ItemCount != 0)
            {
                var sendalldialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("DownLoadAll"), MessageDialogButton.OkAndCancel);
                sendalldialogResult.Closed += sendalldialogResult_Closed;
            }
            else
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("SelectCar"), MessageDialogButton.Ok);
            }
        }

        void sendalldialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    try
                    {
                        ObservableCollection<MdvrMessageVehicle> vehicles = new ObservableCollection<MdvrMessageVehicle>();
                        foreach (MdvrMessageVehicle item in VehicleData)
                        {
                            if (item.Status == (short)CommandStateEnum.UnDelivered)
                            {
                                vehicles.Add(item);
                            }
                        }
                        RunMdvrmessageVehicleClient client = InitialVehicleServiceClient();
                        client.DeliverRunMdvrmessageToVehicleAsync(vehicles);
                    }
                    catch (Exception ex)
                    {
                        ApplicationContext.Instance.Logger.LogException("sendalldialogResult", ex);
                    }
                }
            }
        }

        private RunMdvrmessageVehicleClient InitialVehicleServiceClient()
        {
            RunMdvrmessageVehicleClient _client = ServiceClientFactory.Create<RunMdvrmessageVehicleClient>();
            _client.GetRunMdvrmessageVehicleListCompleted += _client_GetRunMdvrmessageVehicleListCompleted;//列表
            _client.DeleteRunMdvrmessageVehicleByIDCompleted += _client_DeleteRunMdvrmessageVehicleByIDCompleted;//删除车辆
            _client.InsertRunMdvrmessageVehicleCompleted += _client_InsertRunMdvrmessageVehicleCompleted;
            _client.DeliverRunMdvrmessageToVehicleCompleted += _client_DeliverRunMdvrmessageToVehicleCompleted;
            _client.GetAllRunMdvrmessageVehicleListBySpeedIDCompleted += _client_GetAllRunMdvrmessageVehicleListBySpeedIDCompleted;

            return _client;
        }

        void _client_GetAllRunMdvrmessageVehicleListBySpeedIDCompleted(object sender, GetAllRunMdvrmessageVehicleListBySpeedIDCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result.IsSuccess)
                {
                    Func<Vehicle, bool> filter = t => t.MDVROnline.HasValue;
                    VehicleSelectWindow selectwindow = new VehicleSelectWindow(filter);
                    foreach (var v in e.Result.Result)
                    {
                        foreach (var item in selectwindow.VehicleList)
                        {
                            if (item.VehicleId == v.VehicleId)
                            {
                                item.IsChecked = true;
                                break;
                            }
                        }
                    }

                    selectwindow.Closed += selectwindow_Closed;
                    selectwindow.Show();
                }
                else
                {
                    if (string.IsNullOrEmpty(e.Result.ErrorMsg))
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                    }
                }
            }
            else
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
            }
        }

        void _client_DeliverRunMdvrmessageToVehicleCompleted(object sender, DeliverRunMdvrmessageToVehicleCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
                else
                {
                    if (e.Result.IsSuccess == false)
                    {
                        if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ErrorMsg);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                            ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ExceptionMessage);
                        }
                    }
                    else
                    {
                        if (e.Result.Result)
                        {
                            VehicleData.RefreshPage();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("_client_DeliverSpeedLimitToVehicle", ex);
            }
            finally
            {
                RunMdvrmessageVehicleClient client = sender as RunMdvrmessageVehicleClient;
                CloseVehicleServiceClient(client);
            }
        }

        void _client_InsertRunMdvrmessageVehicleCompleted(object sender, InsertRunMdvrmessageVehicleCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        Data.RefreshPage();
                        VehicleData.RefreshPage();
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("_client_InsertHeartbeatVehicleCompleted()", ex);
            }
            finally
            {
                RunMdvrmessageVehicleClient client = sender as RunMdvrmessageVehicleClient;
                CloseVehicleServiceClient(client);
            }
        }

        void _client_DeleteRunMdvrmessageVehicleByIDCompleted(object sender, DeleteRunMdvrmessageVehicleByIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
                else
                {
                    if (e.Result.IsSuccess == false)
                    {
                        if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ErrorMsg);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                            ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ExceptionMessage);
                        }
                    }
                    else
                    {
                        if (e.Result.Result)
                        {
                            if (CurrentSelected != null)
                            {
                                VehicleData.RefreshPage();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("_client_DeleteHeartbeatVehicleByIDCompleted()", ex);
            }
            finally
            {
                RunMdvrmessageVehicleClient client = sender as RunMdvrmessageVehicleClient;
                CloseVehicleServiceClient(client);
            }
        }

        void _client_GetRunMdvrmessageVehicleListCompleted(object sender, GetRunMdvrmessageVehicleListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        foreach (MdvrMessageVehicle ite in e.Result.Result)
                        {
                            int state = (int)ite.Status;
                            switch (state)
                            {
                                case 0:
                                    ite.IsChecked = true;
                                    ite.ShowState = ApplicationContext.Instance.StringResourceReader.GetString("ALARM_UnSend");
                                    break;
                                case 1:
                                    ite.IsChecked = true;
                                    ite.ShowState = ApplicationContext.Instance.StringResourceReader.GetString("WaitDown");
                                    break;
                                case 2:
                                    ite.IsChecked = false;
                                    ite.ShowState = ApplicationContext.Instance.StringResourceReader.GetString("Downing");
                                    break;
                                case 3:
                                    ite.IsChecked = true;
                                    ite.ShowState = ApplicationContext.Instance.StringResourceReader.GetString("DownSuccess");
                                    break;
                                case 4:
                                    ite.IsChecked = true;
                                    ite.ShowState = ApplicationContext.Instance.StringResourceReader.GetString("DownError");
                                    break;
                            }
                            ite.CreateTime = ite.CreateTime.ToLocalTime();
                        }

                        VehicleData.loader_Finished(new PagedResult<MdvrMessageVehicle>
                        {
                            Count = e.Result.TotalRecord,
                            Items = e.Result.Result,
                            PageIndex = currentVehicleIndex
                        });
                        if (VehicleData.TotalItemCount > 0)
                        {
                            CurrentSelected.IsVisible = false;
                        }
                        else
                        {
                            CurrentSelected.IsVisible = true;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("_client_GetHeartbeatVehicleListByHeartBeatIDCompleted()", ex);
            }
            finally
            {
                RunMdvrmessageVehicleClient client = sender as RunMdvrmessageVehicleClient;
                CloseVehicleServiceClient(client);
            }
        }

        int currentVehicleIndex = 1;

        void selectwindow_Closed(object sender, EventArgs e)
        {
            try
            {
                VehicleSelectWindow selectwindow = sender as VehicleSelectWindow;
                if (selectwindow.DialogResult == true)
                {

                    ObservableCollection<MdvrMessageVehicle> vehicles = new ObservableCollection<MdvrMessageVehicle>();
                    List<VehicleEx> list = selectwindow.SelectVehicleList;
                    foreach (var item in list)
                    {
                        MdvrMessageVehicle hv = new MdvrMessageVehicle();
                        hv.ID = Guid.NewGuid().ToString();
                        hv.VehicleId = item.VehicleId;
                        hv.MessageId = CurrentSelected.ID;
                        hv.CreateTime = DateTime.Now.ToUniversalTime();
                        hv.Content = CurrentSelected.Content;
                        hv.MessageType = currentSelected.MessageType;
                        hv.Status = (short)CommandStateEnum.UnDelivered;

                        vehicles.Add(hv);
                    }
                    if (vehicles.Count > 0)
                    {
                        RunMdvrmessageVehicleClient client = InitialVehicleServiceClient();
                        client.InsertRunMdvrmessageVehicleAsync(vehicles);
                    }


                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        /// <summary>
        /// 列表数据源
        /// </summary>
        public PagedServerCollection<MdvrMessageVehicle> VehicleData
        {
            get;
            set;
        }

        MdvrMessageVehicle curVehicleSelected;

        public MdvrMessageVehicle CurVehicleSelected
        {
            get { return curVehicleSelected; }
            set
            {
                curVehicleSelected = value;
                RaisePropertyChanged(() => CurVehicleSelected);
            }
        }

        string _content;

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                RaisePropertyChanged(() => Content);
            }
        }

        string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }


        private List<ComboBoxBasicStruct<EnumModel>> _msgTypes = new List<ComboBoxBasicStruct<EnumModel>>();
        /// <summary>
        /// 
        /// </summary>
        public List<ComboBoxBasicStruct<EnumModel>> MsgTypes
        {
            get { return _msgTypes; }
            set
            {
                _msgTypes = value;
                RaisePropertyChanged(() => MsgTypes);
            }
        }

        private ComboBoxBasicStruct<EnumModel> msgTypeSelected = new ComboBoxBasicStruct<EnumModel>();

        /// <summary>
        /// 
        /// </summary>
        public ComboBoxBasicStruct<EnumModel> MsgTypeSelected
        {
            get
            {
                return msgTypeSelected;
            }
            set
            {
                msgTypeSelected = value;
                RaisePropertyChanged(() => MsgTypeSelected);
            }
        }

        #endregion

    }
}
