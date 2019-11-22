using BaseLib.ViewModels;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using PublicServiceManagement.Views;
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
using Gsafety.PTMS.ServiceReference.PublicService;
using Gsafety.Common.Controls;
using BaseLib.Model;
using Jounce.Framework.Command;
using System.Collections.ObjectModel;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.BasicPage.Views;
using Gsafety.PTMS.Bases.Models;
using System.Collections.Generic;
using System.Reflection;

namespace PublicServiceManagement.ViewModel
{
    [ExportAsViewModel(PublicServiceName.PhoneMsgManageVm)]
    public class PhoneMsgManageViewModel : ListViewModel<RunAppMessage>
    {
        // public RunAppMessageClient client = null;


        private RunAppMessageClient InitServiceClient()
        {
            RunAppMessageClient client = ServiceClientFactory.Create<RunAppMessageClient>();
            client.GetRunAppMessageListCompleted += client_GetRunAppMessageListCompleted;
            client.DeleteRunAppMessageByIDCompleted += client_DeleteRunAppMessageByIDCompleted;
            return client;
        }

        void client_DeleteRunAppMessageByIDCompleted(object sender, DeleteRunAppMessageByIDCompletedEventArgs e)
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
                            Data.RefreshPage();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("PhoneMsgManageViewModel", ex);
            }
            finally
            {
                RunAppMessageClient client = sender as RunAppMessageClient;
                if (client != null)
                {
                    client.CloseAsync();
                    client = null;
                }
            }
        }

        void client_GetRunAppMessageListCompleted(object sender, GetRunAppMessageListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        foreach (var item in e.Result.Result)
                        {
                            int state = (int)item.MessageType;
                            switch (state)
                            {

                                case 1:
                                    item.ShowMsgType = ApplicationContext.Instance.StringResourceReader.GetString("WeatherMessage");
                                    break;
                                case 2:
                                    item.ShowMsgType = ApplicationContext.Instance.StringResourceReader.GetString("TrafficMessage");
                                    break;
                                case 3:
                                    item.ShowMsgType = ApplicationContext.Instance.StringResourceReader.GetString("MaintainMessage");
                                    break;
                                case 4:
                                    item.ShowMsgType = ApplicationContext.Instance.StringResourceReader.GetString("Security");
                                    break;
                                case 5:
                                    item.ShowMsgType = ApplicationContext.Instance.StringResourceReader.GetString("Notice");
                                    break;
                            }
                        }

                        Data.loader_Finished(new PagedResult<RunAppMessage>()
                        {
                            Count = e.Result.TotalRecord,
                            Items = e.Result.Result,
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
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("PhoneMsgManageViewModel", ex);
            }
            finally
            {
                RunAppMessageClient client = sender as RunAppMessageClient;
                if (client != null)
                {
                    client.CloseAsync();
                    client = null;
                }
            }
        }
        /// <summary>
        /// 初始化内容
        /// </summary>
        public PhoneMsgManageViewModel()
            : base()
        {
            try
            {
                InitVehicleServiceClient();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("RunAppMessageMangeViewModel()", ex);
            }
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        protected override void Update(string name)
        {
            PhoneMsgDetailWindow view = new PhoneMsgDetailWindow("update", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentSelected } });
            view.Show();
            view.Closed += (s, e) => { Data.RefreshPage(); };
        }
        protected override void Add(string name)
        {
            PhoneMsgDetailWindow view = new PhoneMsgDetailWindow("add", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentSelected } });
            view.Show();
            view.Closed += (s, e) => { Data.RefreshPage(); };
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
                Data = new BaseLib.Model.PagedServerCollection<RunAppMessage>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    RunAppMessageClient client = InitServiceClient();

                    client.GetRunAppMessageListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, Title, pageIndex, pageSize);
                });

                VehicleData = new PagedServerCollection<AppMessageVehicle>((pageIndex, pageSize) =>
                {
                    pageSize = BaseCommon.PageSizeList[3];
                    System.Threading.Interlocked.Exchange(ref currentVehicleIndex, pageIndex);

                    AppMessageVehicleClient _client = InitialVehicleServiceClient();
                    //查询数据
                    _client.GetAppMessageVehicleListByAppIDAsync(CurrentSelected.ClientId, CurrentSelected.ID, pageIndex, pageSize);
                });

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        protected override void Delete()
        {

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
                    RunAppMessageClient client = InitServiceClient();
                    client.DeleteRunAppMessageByIDAsync(CurrentSelected.ID);
                }
            }
        }

        #region Field



        RunAppMessage currentSelected;
        public RunAppMessage CurrentSelected
        {
            get { return currentSelected; }
            set
            {
                //currentSelected = value;
                //RaisePropertyChanged(() => CurrentSelected);
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
                var deliverdialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("SendInfomation"), MessageDialogButton.OkAndCancel);
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
                    ObservableCollection<AppMessageVehicle> vehicles = new ObservableCollection<AppMessageVehicle>();
                    vehicles.Add(CurVehicleSelected);
                    AppMessageVehicleClient client = InitialVehicleServiceClient();
                    client.DeliverAppMessageToVehicleAsync(vehicles);
                }
            }
        }

        private void AddVechile(string p)
        {
            if (CurrentSelected != null)
            {
                AppMessageVehicleClient client = InitialVehicleServiceClient();
                client.GetAllAppMessageVehicleListByAppIDAsync(CurrentSelected.ClientId, CurrentSelected.ID);
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
                    AppMessageVehicleClient client = InitialVehicleServiceClient();
                    client.DeleteAppMessageVehicleByIDAsync(CurVehicleSelected.ID);
                }
            }
        }


        private void CloseVehicleServiceClient(AppMessageVehicleClient client)
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
            try
            {
                SelfMessageBox dialog = sender as SelfMessageBox;
                if (dialog != null)
                {
                    if (dialog.DialogResult == true)
                    {
                        try
                        {
                            ObservableCollection<AppMessageVehicle> vehicles = new ObservableCollection<AppMessageVehicle>();
                            foreach (AppMessageVehicle item in VehicleData)
                            {
                                if (item.Status == (short)CommandStateEnum.UnDelivered)
                                {
                                    vehicles.Add(item);
                                }
                            }
                            AppMessageVehicleClient client = InitialVehicleServiceClient();
                            client.DeliverAppMessageToVehicleAsync(vehicles);
                        }
                        catch (Exception ex)
                        {
                            ApplicationContext.Instance.Logger.LogException("sendalldialogResult", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private AppMessageVehicleClient InitialVehicleServiceClient()
        {
            AppMessageVehicleClient _client = ServiceClientFactory.Create<AppMessageVehicleClient>();
            _client.GetAllAppMessageVehicleListByAppIDCompleted += _client_GetAllAppMessageVehicleListByAppIDCompleted;
            _client.GetAppMessageVehicleListByAppIDCompleted += _client_GetAppMessageVehicleListByAppIDCompleted;
            _client.DeleteAppMessageVehicleByIDCompleted += _client_DeleteAppMessageVehicleByIDCompleted;
            _client.DeliverAppMessageToVehicleCompleted += _client_DeliverAppMessageToVehicleCompleted;
            _client.InsertAppMessageVehicleCompleted += _client_InsertAppMessageVehicleCompleted;

            return _client;
        }


        private void _client_GetAllAppMessageVehicleListByAppIDCompleted(object sender, GetAllAppMessageVehicleListByAppIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        Func<Vehicle, bool> filter = t => t.MobileOnline.HasValue;
                        VehicleSelectWindow selectwindow = new VehicleSelectWindow(filter);
                        foreach (var v in e.Result.Result)
                        {
                            foreach (var item in selectwindow.VehicleList)
                            {
                                if (item.VehicleId == v.VehicleID)
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
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("_client_DeliverAppMessageToVehicle", ex);
            }
            finally
            {
                AppMessageVehicleClient client = sender as AppMessageVehicleClient;
                CloseVehicleServiceClient(client);
            }

        }

        private void _client_DeliverAppMessageToVehicleCompleted(object sender, DeliverAppMessageToVehicleCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("_client_DeliverAppMessageToVehicle", ex);
            }
            finally
            {
                AppMessageVehicleClient client = sender as AppMessageVehicleClient;
                CloseVehicleServiceClient(client);
            }
        }

        private void _client_InsertAppMessageVehicleCompleted(object sender, InsertAppMessageVehicleCompletedEventArgs e)
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
                        if (e.Result.Result == true)
                        {
                            Data.RefreshPage();
                            VehicleData.RefreshPage();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("_client_InsertHeartbeatVehicleCompleted()", ex);
            }
            finally
            {
                AppMessageVehicleClient client = sender as AppMessageVehicleClient;
                CloseVehicleServiceClient(client);
            }
        }

        private void _client_DeleteAppMessageVehicleByIDCompleted(object sender, DeleteAppMessageVehicleByIDCompletedEventArgs e)
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
                AppMessageVehicleClient client = sender as AppMessageVehicleClient;
                CloseVehicleServiceClient(client);
            }
        }

        private void _client_GetAppMessageVehicleListByAppIDCompleted(object sender, GetAppMessageVehicleListByAppIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        foreach (AppMessageVehicle ite in e.Result.Result)
                        {
                            if (ite.Status == 0)
                            {
                                ite.ShowState = ApplicationContext.Instance.StringResourceReader.GetString("ALARM_UnSend");
                            }
                            else
                            {
                                ite.ShowState = ApplicationContext.Instance.StringResourceReader.GetString("DownSuccess");
                            }

                            ite.CreateTime = ite.CreateTime.ToLocalTime();
                        }

                        VehicleData.loader_Finished(new PagedResult<AppMessageVehicle>
                        {
                            Count = e.Result.TotalRecord,
                            Items = e.Result.Result,
                            PageIndex = currentVehicleIndex
                        });
                        if (VehicleData.TotalItemCount > 0)
                        {
                            CurrentSelected.CanDelete = false;
                        }
                        else
                        {
                            CurrentSelected.CanDelete = true;
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
                AppMessageVehicleClient client = sender as AppMessageVehicleClient;
                CloseVehicleServiceClient(client);
            }
        }

        int currentVehicleIndex = 1;

        void selectwindow_Closed(object sender, EventArgs e)
        {
            VehicleSelectWindow selectwindow = sender as VehicleSelectWindow;
            if (selectwindow.DialogResult == true)
            {

                ObservableCollection<AppMessageVehicle> vehicles = new ObservableCollection<AppMessageVehicle>();
                List<VehicleEx> list = selectwindow.SelectVehicleList;
                foreach (var item in list)
                {
                    AppMessageVehicle hv = new AppMessageVehicle();
                    hv.ID = Guid.NewGuid().ToString();
                    hv.VehicleID = item.VehicleId;
                    hv.MessageID = CurrentSelected.ID;
                    hv.CreateTime = DateTime.Now.ToUniversalTime();
                    hv.Message = CurrentSelected.Message;
                    hv.MessageType = currentSelected.MessageType;
                    hv.Status = (short)CommandStateEnum.UnDelivered;

                    vehicles.Add(hv);
                }
                if (vehicles.Count > 0)
                {
                    AppMessageVehicleClient client = InitialVehicleServiceClient();
                    client.InsertAppMessageVehicleAsync(vehicles, CurrentSelected);
                }
            }
        }


        /// <summary>
        /// 列表数据源
        /// </summary>
        public PagedServerCollection<AppMessageVehicle> VehicleData
        {
            get;
            set;
        }

        AppMessageVehicle curVehicleSelected;

        public AppMessageVehicle CurVehicleSelected
        {
            get { return curVehicleSelected; }
            set
            {
                curVehicleSelected = value;
                RaisePropertyChanged(() => CurVehicleSelected);
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

        #endregion



    }
}
