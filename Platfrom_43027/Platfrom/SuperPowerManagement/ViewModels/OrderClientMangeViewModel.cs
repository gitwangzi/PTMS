using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Common.Enum;
using Gsafety.PTMS.ServiceReference.OrderClientService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.SuperPowerManagement;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using SuperPowerManagement.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace SuperPowerManagement.ViewModels
{
    [ExportAsViewModel(SuperPowerName.OrderClientManageVm)]
    public class OrderClientMangeViewModel : ListViewModel<OrderClientEx>
    {
        /// <summary>
        /// 提供服务接口的对象
        /// </summary>

        public OrderClientEx CurrentOrderClient { get; set; }

        public Visibility isReadOnly;
        /// <summary>
        /// 用来控制前台的控件根据登陆者的权限的显示与隐藏
        /// 默认值Visible，Collapsed
        /// </summary>
        public Visibility IsReadOnly
        {
            get
            {
                return this.isReadOnly;
            }
            set
            {
                this.isReadOnly = value;
                RaisePropertyChanged(() => this.IsReadOnly);
            }
        }

        public Visibility roleVisibility;
        /// <summary>
        /// 
        /// </summary>
        public Visibility RoleVisibility
        {
            get
            {
                return this.roleVisibility;
            }
            set
            {
                this.roleVisibility = value;
                RaisePropertyChanged(() => this.RoleVisibility);
            }
        }

        #region Command BtnEvent


        public ICommand OpenBtnCommand { get; set; }

        public ICommand CloseBtnCommand { get; set; }

        public ICommand RecoveryPwdBtnCommand { get; set; }
        #endregion

        /// <summary>
        /// 初始化内容
        /// </summary>
        public OrderClientMangeViewModel()
            : base()
        {
            try
            {
                InitialOrderClientStatus();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("OrderClientMangeViewModel()", ex);
            }

            //注册事件
            OpenBtnCommand = new ActionCommand<object>(method => OpenPromission());
            CloseBtnCommand = new ActionCommand<object>(method => ClosePromission());
            RecoveryPwdBtnCommand = new ActionCommand<object>(method => RecoveryPwd());

            this.RoleVisibility = Visibility.Visible;
        }

        private OrderClientServiceClient InitialOrderClient()
        {
            OrderClientServiceClient client = ServiceClientFactory.Create<OrderClientServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.ResetPasswordCompleted += client_ResetPasswordCompleted;
            client.SetOrderClientStatusCompleted += client_SetOrderClientStatusCompleted;
            client.GetOrderClientExListCompleted += client_GetOrderClientExListCompleted;
            return client;
        }

        /// <summary>
        /// 密码重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_ResetPasswordCompleted(object sender, ResetPasswordCompletedEventArgs e)
        {

            try
            {
                if (e.Cancelled)
                {
                    return;
                }
                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                }
                if (e.Result.IsSuccess == false)
                {
                    if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                    {
                        MessageBoxHelper.ShowDialog(
                            ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), e.Result.ErrorMsg);
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ErrorMsg);
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ExceptionMessage);
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ResertPwdSuccessed));
                    Data.RefreshPage();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_ResetPasswordCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        /// <summary>
        /// 关闭客户端与服务端连接
        /// </summary>
        private void CloseClient(object sender)
        {
            OrderClientServiceClient client = sender as OrderClientServiceClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        /// <summary>
        /// 帐户状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_SetOrderClientStatusCompleted(object sender, SetOrderClientStatusCompletedEventArgs e)
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
                        Data.RefreshPage();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_SetOrderClientStatusCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }

        }


        /// <summary>
        /// 密码重置
        /// </summary>
        /// <returns></returns>
        private void RecoveryPwd()
        {
            new ResetPasswordClientView(CurrentOrderClient.ID, CurrentOrderClient.UserName).Show();
        }

        /// <summary>
        /// 暂停帐号
        /// </summary>
        /// <returns></returns>
        private void ClosePromission()
        {
            try
            {
                OrderClientServiceClient client = InitialOrderClient();
                LogManager log = new LogManager();
                log.ManagerID = ApplicationContext.Instance.AuthenticationInfo.UserID;
                log.Manager = ApplicationContext.Instance.AuthenticationInfo.Account;
                log.Content = ApplicationContext.Instance.StringResourceReader.GetString("ClientUserInActiveStatus");
                log.ClientID = CurrentOrderClient.ID;
                log.ClientName = CurrentOrderClient.Name;
                log.ID = Guid.NewGuid().ToString();
                client.SetOrderClientStatusAsync(CurrentOrderClient.ID, false, log);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// 帐号开启
        /// </summary>
        /// <returns></returns>
        private void OpenPromission()
        {
            try
            {
                OrderClientServiceClient client = InitialOrderClient();
                LogManager log = new LogManager();
                log.ManagerID = ApplicationContext.Instance.AuthenticationInfo.UserID;
                log.Manager = ApplicationContext.Instance.AuthenticationInfo.Account;
                log.Content = ApplicationContext.Instance.StringResourceReader.GetString("ClientUserActiveStatus");
                log.ClientID = CurrentOrderClient.ID;
                log.ClientName = CurrentOrderClient.Name;
                log.ID = Guid.NewGuid().ToString();
                client.SetOrderClientStatusAsync(CurrentOrderClient.ID, true, log);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        /// <summary>
        /// 编辑帐户
        /// </summary>
        /// <returns></returns>
        protected override void Update(string name)
        {
            OrderClientDetailWindow detailWindow = new OrderClientDetailWindow(SuperPowerName.AddCloudAccountView, new Dictionary<string, object>() { { "action", name }, { "view", CurrentOrderClient } });
            detailWindow.Show();
            detailWindow.Closed += (s, e) =>
            {
                Data.RefreshPage();
            };
        }

        /// <summary>
        /// 数据查询完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_GetOrderClientExListCompleted(object sender, GetOrderClientExListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        int row = 0;
                        foreach (OrderClientEx item in e.Result.Result)
                        {
                            row = row + 1;
                            item.Row = row;
                            item.StatusStr = item.Status == StatusEnum.Normal
                                ? ApplicationContext.Instance.StringResourceReader.GetString("OrderClientStatus_Run")
                                : ApplicationContext.Instance.StringResourceReader.GetString("OrderClientStatus_Suspended");
                            if (item._platformversion == (short)VersionEnum.Basic)
                            {
                                item.VersionStr = ApplicationContext.Instance.StringResourceReader.GetString("VersionEnum_Basic");
                            }
                            else if (item._platformversion == (short)VersionEnum.Standard)
                            {
                                item.VersionStr =
                                    ApplicationContext.Instance.StringResourceReader.GetString("VersionEnum_Standard");
                            }

                            if (RoleVisibility == Visibility.Visible)
                            {
                                if (item.Status == StatusEnum.Normal)
                                {
                                    item.ResumeVisibility = Visibility.Collapsed;
                                    item.SuspendedVisibility = Visibility.Visible;
                                }
                                else
                                {
                                    item.ResumeVisibility = Visibility.Visible;
                                    item.SuspendedVisibility = Visibility.Collapsed;
                                }
                            }
                            else
                            {
                                item.ResumeVisibility = Visibility.Collapsed;
                                item.SuspendedVisibility = Visibility.Collapsed;
                            }

                        }

                        ObservableCollection<OrderClientEx> result = new ObservableCollection<OrderClientEx>();
                        foreach (var item in e.Result.Result)
                        {
                            item.BeginTime = item.BeginTime.ToLocalTime();
                            item.EndTime = item.EndTime.ToLocalTime();
                            result.Add(item);
                        }


                        Data.loader_Finished(new PagedResult<OrderClientEx>
                        {
                            Count = e.Result.TotalRecord,
                            Items = result,
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
                ApplicationContext.Instance.Logger.LogException("OrderClientMangeViewModel()", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }


        /// <summary>
        /// 初始化使用状态
        /// </summary> 
        private void InitialOrderClientStatus()
        {
            StatusList = new Dictionary<int, string>();
            StatusList.Add(-1, ApplicationContext.Instance.StringResourceReader.GetString("All"));
            StatusList.Add(0, ApplicationContext.Instance.StringResourceReader.GetString("ClientUserActiveStatus"));
            StatusList.Add(1, ApplicationContext.Instance.StringResourceReader.GetString("ClientUserInActiveStatus"));
        }

        protected override void Add(string name)
        {
            OrderClientDetailWindow detailWindow = new OrderClientDetailWindow(SuperPowerName.AddCloudAccountView, new Dictionary<string, object>() { { "action", name } });
            detailWindow.Closed += detailWindow_Closed;
            detailWindow.Show();
        }

        void detailWindow_Closed(object sender, EventArgs e)
        {
            Data.RefreshPage();
        }

        /// <summary>
        /// 电击查询按钮
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

                Data = new PagedServerCollection<OrderClientEx>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                    OrderClientManagerQueryModel obj = new OrderClientManagerQueryModel();
                    obj.PageIndex = currentIndex;
                    obj.PageSize = PageSizeValue;
                    obj.Name = Company;
                    if (UsedStatus == -1)
                    {
                        obj.Status = null;
                    }
                    else
                    {
                        obj.Status = UsedStatus;
                    }

                    OrderClientServiceClient client = InitialOrderClient();

                    //查询数据
                    client.GetOrderClientExListAsync(obj);
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagination()", ex);
            }
        }

        /// <summary>
        /// 使用状态下拉
        /// </summary>
        private Dictionary<int, string> statusList;
        public Dictionary<int, string> StatusList
        {
            get { return statusList; }
            set { statusList = value; }
        }
        private string company;
        public string Company
        {
            get { return company; }
            set
            {
                company = value;
                RaisePropertyChanged(() => Company);
            }
        }
        private int usedStatus;
        public int UsedStatus
        {
            get { return usedStatus; }
            set
            {
                usedStatus = value;
                RaisePropertyChanged(() => UsedStatus);
            }
        }
        private DateTime openBeginTime = DateTime.Now;
        public DateTime OpenBeginTime
        {
            get { return openBeginTime; }
            set { openBeginTime = value; }
        }
        private DateTime openEndTime = DateTime.Now;
        public DateTime OpenEndTime
        {
            get { return openEndTime; }
            set { openEndTime = value; }
        }
        private DateTime closeBeginTime = DateTime.Now;
        public DateTime CloseBeginTime
        {
            get { return closeBeginTime; }
            set { closeBeginTime = value; }
        }
        private DateTime closeEndTime = DateTime.Now;
        public DateTime CloseEndTime
        {
            get { return closeEndTime; }
            set { closeEndTime = value; }
        }
    }
}
