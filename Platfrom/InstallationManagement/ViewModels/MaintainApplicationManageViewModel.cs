using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Installation.Views;
using Gsafety.PTMS.ServiceReference.MaintainService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Reflection;
namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.MaintainApplcationManagementVm)]
    public class MaintainApplicationManageViewModel : ListViewModel<MaintainApplication>
    {
        /// <summary>
        /// 初始化内容
        /// </summary>
        public MaintainApplicationManageViewModel()
            : base()
        {
            try
            {
                InitInstallStation();
            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("MaintainApplicationMangeViewModel()", ex);
            }
        }

        #region method.....
        bool _canselectvehicle = false;
        public bool CanSelectVehicle
        {
            get
            {
                return _canselectvehicle;
            }
            set
            {
                _canselectvehicle = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CanSelectVehicle));
            }
        }

        private MaintainApplicationClient InitialClient()
        {
            MaintainApplicationClient _client = ServiceClientFactory.Create<MaintainApplicationClient>();
            _client.GetMaintainApplicationListCompleted += _client_GetMaintainApplicationListCompleted;
            _client.DeleteMaintainApplicationByIDCompleted += _client_DeleteMaintainApplicationByIDCompleted;
            _client.GetInstallStationListCompleted += _client_GetInstallStationListCompleted;
            return _client;
        }

        void _client_DeleteMaintainApplicationByIDCompleted(object sender, DeleteMaintainApplicationByIDCompletedEventArgs e)
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
                        SelectedItem = null;
                        Data.RefreshPage();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("_client_DeleteMaintainApplicationByIDCompleted", ex);

            }
            finally
            {
                MaintainApplicationClient client = sender as MaintainApplicationClient;
                CloseClient(client);
            }
        }

        private void CloseClient(MaintainApplicationClient client)
        {
            if(client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        void _client_GetMaintainApplicationListCompleted(object sender, GetMaintainApplicationListCompletedEventArgs e)
        {
            try
            {
                if(e.Result != null)
                {

                    List<MaintainApplication> items = new List<MaintainApplication>();
                    foreach(var item in e.Result.Result)
                    {
                       
                       item.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("HasApplication");
                       items.Add(item);
                        
                       item.CreateTime = item.CreateTime.ToLocalTime();
                    }
                    Data.loader_Finished(new PagedResult<MaintainApplication>
                    {
                        Count = items.Count,
                        Items = items,
                        PageIndex = currentIndex
                    });

                    if(e.Result.Result.Count != 0)
                    {
                        SelectedItem = e.Result.Result[0];
                    }
                }
            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("_client_GetMaintainApplicationListCompleted()", ex);
            }
            finally
            {
                MaintainApplicationClient client = sender as MaintainApplicationClient;
                CloseClient(client);
            }
        }


        protected override void Add(string name)
        {
            MaintainApplicationDetailWindow detailWindow = new MaintainApplicationDetailWindow(string.Empty, new Dictionary<string, object>() { { "action", name } });
            detailWindow.Closed += detailWindow_Closed;
            detailWindow.Show();
        }

        void detailWindow_Closed(object sender, EventArgs e)
        {
            MaintainApplicationDetailWindow window = sender as MaintainApplicationDetailWindow;
            if(window != null)
            {
                if(window.DialogResult == true)
                {
                    Data.RefreshPage();
                }
            }
        }



        protected override void Update(string actionName)
        {
            try
            {
                if (SelectedItem != null)
                {
                    SelectedItem.ZInstallStation = ZInstallStation;
                    MaintainApplicationDetailWindow detailWindow = new MaintainApplicationDetailWindow(string.Empty, new Dictionary<string, object>() { { "action", actionName }, { "model", SelectedItem } });
                    detailWindow.Closed += detailWindow_Closed;
                    detailWindow.Show();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private MaintainApplication selecteditem;

        public MaintainApplication SelectedItem
        {
            get { return selecteditem; }
            set
            {
                if(selecteditem != value)
                {
                    selecteditem = value;
                }

                RaisePropertyChanged(() => this.SelectedItem);
            }
        }

        protected override void Delete()
        {
            if(SelectedItem != null)
            {

                var dialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
                dialogResult.Closed += dialogResult_Closed;
            }
        }

        private void dialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if(dialog != null)
            {
                if(dialog.DialogResult == true)
                {
                    MaintainApplicationClient client = InitialClient();
                    client.DeleteMaintainApplicationByIDAsync(SelectedItem.ID);
                }
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
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {
            try
            {
                Data = new PagedServerCollection<MaintainApplication>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                    MaintainApplicationClient client = InitialClient();
                    //查询数据
                    client.GetMaintainApplicationListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID,0, SearchByApplicant, SearchByVehicleID, SearchByContact, pageIndex, pageSize);
                });
            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagination()", ex);
            }
        }

        public string Name
        {
            get;
            set;
        }


        private Dictionary<string, string> _installStation = new Dictionary<string, string>();
        public Dictionary<string, string> ZInstallStation
        {
            get { return _installStation; }
            set
            {
                _installStation = value;

            }
        }

        void InitInstallStation()
        {
            try
            {
                MaintainApplicationClient client = InitialClient();
                client.GetInstallStationListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);
            }
            catch(Exception)
            {
                throw;
            }
        }

        private void _client_GetInstallStationListCompleted(object sender, GetInstallStationListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        foreach (var item in e.Result.Result)
                        {
                            ZInstallStation.Add(item.ID, item.Name);
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
                ApplicationContext.Instance.Logger.LogException("MaintainApplicationManageViewModel", ex);
            }
            finally
            {
                MaintainApplicationClient client = sender as MaintainApplicationClient;
                if (client != null)
                {
                    client.CloseAsync();
                    client = null;
                }
            }

        }
        #endregion



        #region searchby....
        private string searchByApplicant;
        /// <summary>
        /// 
        /// </summary>
        public string SearchByApplicant
        {
            get
            {
                return searchByApplicant;
            }
            set
            {
                this.searchByApplicant = value;
                //Validate(ExtractPropertyName(() => SearchByApplicant), searchByICard, SearchByICard);
                RaisePropertyChanged(() => this.SearchByApplicant);
            }
        }

        private string searchByVehicleID;
        /// <summary>
        /// 
        /// </summary>
        public string SearchByVehicleID
        {
            get
            {
                return searchByVehicleID;
            }
            set
            {
                this.searchByVehicleID = value;
                //Validate(ExtractPropertyName(() => SearchByApplicant), searchByICard, SearchByICard);
                RaisePropertyChanged(() => this.SearchByVehicleID);
            }
        }

        private string searchByContact;
        /// <summary>
        /// 
        /// </summary>
        public string SearchByContact
        {
            get
            {
                return searchByContact;
            }
            set
            {
                this.searchByContact = value;
                //Validate(ExtractPropertyName(() => SearchByApplicant), searchByICard, SearchByICard);
                RaisePropertyChanged(() => this.SearchByContact);
            }
        }

        #endregion
    }
}

