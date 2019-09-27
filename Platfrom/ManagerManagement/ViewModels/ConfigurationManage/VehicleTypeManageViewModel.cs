using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Manager;
using Gsafety.PTMS.Manager.Views.ConfigurationManage;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Input;


namespace Gsafety.PTMS.ManagerManagement.Views.ViewModels
{
    [ExportAsViewModel(ManagerName.VehicleTypeManageViewVm)]
    public class VehicleTypeMangeViewModel : ListViewModel<VehicleType>
    {

        //VehicleTypeClient client = null;
        VehicleType CurrentModel { get; set; }
        /// <summary>
        /// 初始化内容
        /// </summary>
        public VehicleTypeMangeViewModel()
            : base()
        {
            try
            {
                this.InlitiziClient();
                BtnDetailCommand = new ActionCommand<object>(method => DetailAction("view"));

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleTypeMangeViewModel()", ex);
            }
        }

        /// <summary>
        /// 初始化客户端
        /// </summary>
        private VehicleTypeClient InlitiziClient()
        {
            VehicleTypeClient client = ServiceClientFactory.Create<VehicleTypeClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);

            client.DeleteVehicleTypeByIDCompleted += client_DeleteVehicleTypeByIDCompleted;
            client.GetByNameVehicleTypeListCompleted += client_GetByNameVehicleTypeListCompleted;
            return client;
        }


        private void DetailAction(string name)
        {
            VehicleTypeDetailWindow window = new VehicleTypeDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentVehicleType } });
            window.Show();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        protected override void Update(string name)
        {
            VehicleTypeDetailWindow window = new VehicleTypeDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentVehicleType } });
            window.Closed += window_Closed;
            window.Show();
        }
        /// <summary>
        /// add 
        /// </summary>
        /// <param name="name"></param>
        protected override void Add(string name)
        {
            VehicleTypeDetailWindow window = new VehicleTypeDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentVehicleType } });
            window.Closed += window_Closed;
            window.Show();
        }

        void window_Closed(object sender, EventArgs e)
        {
            VehicleTypeDetailWindow window = sender as VehicleTypeDetailWindow;
            if (window.DialogResult == true)
            {
                Data.RefreshPage();
            }
        }

        protected override void Delete()
        {
            if (CurrentVehicleType != null)
            {
                var dialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
                dialogResult.Closed += dialogResult_Closed;
            }
        }

        private void dialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog.DialogResult == true)
            {
                VehicleTypeClient client = InlitiziClient();

                client.DeleteVehicleTypeByIDAsync(CurrentVehicleType.ID);
            }
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
        /// 按类型名查询
        /// </summary>
        protected override void InitPagination()
        {
            try
            {
                Data = new BaseLib.Model.PagedServerCollection<VehicleType>((pageIndex, pageSize) =>
                    {
                        pageSize = PageSizeValue;
                        System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                        VehicleTypeClient client = InlitiziClient();

                        client.GetByNameVehicleTypeListAsync(pageIndex, pageSize, ApplicationContext.Instance.AuthenticationInfo.ClientID, SearchByName);
                    });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogError("InitPagedServerCollection()", ex.ToString());
            }
        }

        void client_GetByNameVehicleTypeListCompleted(object sender, GetByNameVehicleTypeListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        int row = 0;
                        foreach (VehicleType item in e.Result.Result)
                        {
                            row = row + 1;
                            item.Row = row;
                            item.CreateTime = item.CreateTime.ToLocalTime();
                        }
                        Data.loader_Finished(new BaseLib.Model.PagedResult<VehicleType>()
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
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_GetByNameVehicleTypeListCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }

        }

        private void CloseClient(object sender)
        {
            VehicleTypeClient client = sender as VehicleTypeClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        private void client_DeleteVehicleTypeByIDCompleted(object sender, DeleteVehicleTypeByIDCompletedEventArgs e)
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
                        CurrentVehicleType = null;
                        Data.RefreshPage();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_DeleteInstallStationCompleted", ex);
                this.InlitiziClient();
            }
            finally
            {
                CloseClient(sender);
            }
        }


        private VehicleType currentVehicleType;

        public VehicleType CurrentVehicleType
        {
            get { return currentVehicleType; }
            set
            {
                currentVehicleType = value;
                RaisePropertyChanged(() => this.CurrentVehicleType);
            }
        }

        public ICommand BtnDetailCommand { get; set; }

        private string searchByName;
        /// <summary>
        /// 
        /// </summary>
        public string SearchByName
        {
            get
            {
                return searchByName;
            }
            set
            {
                this.searchByName = value;
                RaisePropertyChanged(() => this.SearchByName);
            }
        }
    }
}

