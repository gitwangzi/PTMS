using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.AccountService;
using Gsafety.PTMS.ServiceReference.InstallStationService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;

namespace Gsafety.Ant.BaseInformation.ViewModels
{
    //[ExportAsViewModel(BaseInformationName.InstallPlaceBindingVm)]
    public class InstallPlaceBindingViewModel : ListViewModel<GUser>
    {
        #region 属性
        ObservableCollection<InstallStationUser> installStationUsers;

        InstallStation InstallStationModel;

        private string _title;
        /// <summary>
        /// 弹出窗体标题
        /// </summary>
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
                RaisePropertyChanged(() => this.Title);
            }
        }

        public event EventHandler<SaveResultArgs> OnSaveResult;


        #region property....
        private InstallStationUser currentInstallStationUser;
        public InstallStationUser CurrentInstallStationUser
        {
            get { return currentInstallStationUser; }
            set
            {
                currentInstallStationUser = value;
                RaisePropertyChanged(() => this.CurrentInstallStationUser);
            }
        }

        public IActionCommand ReturnCommand { get; protected set; }

        #endregion

        #endregion

        #region 构造函数

        /// <summary>
        /// initil
        /// </summary>        
        public InstallPlaceBindingViewModel()
        {
            this.Title = ApplicationContext.Instance.StringResourceReader.GetString("BindingTitle");
        }

        #endregion

        #region 方法

        /// <summary>
        /// 初始化安装服务客户端
        /// </summary>
        /// <returns></returns>
        private InstallStationServiceClient InitializeInstallStationServiceClient()
        {
            InstallStationServiceClient installClient = null;
            installClient = ServiceClientFactory.Create<InstallStationServiceClient>();
            installClient.GetInstallStationUserCompleted += installClient_GetInstallStationUserCompleted;
            installClient.SaveInstallStationUserCompleted += installClient_SaveInstallStationUserCompleted;
            return installClient;
        }

        private UserServiceClient InitializeUserServiceClient()
        {
            UserServiceClient client = null;
            client = ServiceClientFactory.Create<UserServiceClient>();
            client.GetInstallStationUserByPageCompleted += client_GetInstallStationUserByPageCompleted;
            return client;
        }

        void client_GetInstallStationUserByPageCompleted(object sender, GetInstallStationUserByPageCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        foreach (var guser in e.Result.Result)
                        {
                            foreach (var isu in installStationUsers)
                            {
                                if (isu.UserID == guser.ID)
                                {
                                    guser.IsChecked = true;
                                }
                            }
                        }

                        Data.loader_Finished(new BaseLib.Model.PagedResult<GUser>
                        {
                            Count = e.Result.TotalRecord,
                            Items = e.Result.Result,
                            PageIndex = currentIndex
                        });
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                            ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.userClient_GetInstallStationUserCompleted", e.Result.ExceptionMessage);
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                            ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.userClient_GetInstallStationUserCompleted", e.Result.ExceptionMessage);
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
                ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.userClient_GetInstallStationUserCompleted", ex);
            }
            finally
            {
                UserServiceClient client = sender as UserServiceClient;
                this.CloseUserClient(client);
            }
        }


        /// <summary>
        /// 保存添加
        /// </summary>
        /// <param name="name"></param>
        protected override void Add(string name)
        {
            try
            {
                InstallStationServiceClient installClient = this.InitializeInstallStationServiceClient();
                ObservableCollection<InstallStationUser> installStationUsers = new ObservableCollection<InstallStationUser>();
                foreach (GUser u in Data)
                {
                    if (u.IsChecked == true)
                    {
                        InstallStationUser isu = new InstallStationUser();
                        isu.ID = Guid.NewGuid().ToString();
                        isu.InstallStationID = InstallStationModel.ID;
                        isu.UserID = u.ID;
                        isu.CreateTime = DateTime.Now.ToUniversalTime();

                        bool exist = this.installStationUsers.Any(n => n.InstallStationID == isu.ID && n.UserID == isu.UserID);
                        if (!exist)
                        {
                            installStationUsers.Add(isu);
                        }

                    }
                    else
                    {
                        InstallStationUser isu = new InstallStationUser();
                        isu.ID = null;
                        isu.InstallStationID = InstallStationModel.ID;
                        isu.UserID = u.ID;
                        isu.CreateTime = DateTime.Now.ToUniversalTime();


                        var item = this.installStationUsers.FirstOrDefault(n => n.InstallStationID == isu.InstallStationID && n.UserID == isu.UserID);
                        if (item != null)
                        {
                            installStationUsers.Add(isu);
                        }
                    }
                }
                installClient.SaveInstallStationUserAsync(installStationUsers);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }

        public new void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                string action = viewParameters["action"].ToString();
                InstallStationModel = viewParameters["model"] as InstallStation;

                InstallStationServiceClient installClient = this.InitializeInstallStationServiceClient();
                installClient.GetInstallStationUserAsync(InstallStationModel.ID, ApplicationContext.Instance.AuthenticationInfo.ClientID);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        #region Completed....

        protected override void Query()
        {
            currentIndex = 1;
            Data.MoveToFirstPage();
        }

        protected override void InitPagination()
        {
            Data = new BaseLib.Model.PagedServerCollection<GUser>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                UserServiceClient installClient = InitializeUserServiceClient();
                installClient.GetInstallStationUserByPageAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, pageSize, pageIndex);
            });
        }

        private void installClient_SaveInstallStationUserCompleted(object sender, SaveInstallStationUserCompletedEventArgs e)
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
                            ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                                   e.Result.ExceptionMessage);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        }

                        return;
                    }
                    else
                    {
                        SaveResultArgs args = new SaveResultArgs();
                        args.Result = true;
                        if (OnSaveResult != null)
                        {
                            OnSaveResult(this, args);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.client_AddInstallStationCompleted", ex);
            }
            finally
            {
                InstallStationServiceClient client = sender as InstallStationServiceClient;
                CloseInstallClient(client);
            }
        }

        /// <summary>
        /// 关闭安装服务连接
        /// </summary>
        /// <param name="client"></param>
        private void CloseInstallClient(InstallStationServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }


        /// <summary>
        /// 关闭用户服务连接
        /// </summary>
        /// <param name="client"></param>
        private void CloseUserClient(UserServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }


        /// <summary>
        /// 获取安装用户之后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void installClient_GetInstallStationUserCompleted(object sender, PTMS.ServiceReference.InstallStationService.GetInstallStationUserCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        installStationUsers = e.Result.Result;

                        Query();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                            ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.installClient_GetInstallStationUserCompleted", e.Result.ExceptionMessage);
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                            ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.installClient_GetInstallStationUserCompleted", e.Result.ExceptionMessage);
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
                ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.installClient_GetInstallStationUserCompleted", ex);
            }
            finally
            {
                InstallStationServiceClient client = sender as InstallStationServiceClient;
                CloseInstallClient(client);
            }

        }

        #endregion

        #endregion

    }
}
