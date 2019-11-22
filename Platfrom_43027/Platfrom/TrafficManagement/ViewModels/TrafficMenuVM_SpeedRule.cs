using Gsafety.PTMS.BasicPage.Views;
using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Framework.Command;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using Command = Gsafety.PTMS.ServiceReference.CommandManageService;
using System.Collections.Generic;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Traffic.Views;
using Gsafety.PTMS.Traffic.Models;
using System.Reflection;

namespace Gsafety.PTMS.Traffic.ViewModels
{
    public partial class TrafficMenuVm
    {
        private void InitSpeedRule()
        {
            AddSpeedRuleCommand = new ActionCommand<object>(obj => AddSpeedRuleAction());
            QuerySpeedRuleCommand = new ActionCommand<object>(obj => QuerySpeedRule());
            EditSpeedRuleCommand = new ActionCommand<object>(obj => EditSpeedRuleAction());
            SendSpeedRuleCommand = new ActionCommand<object>(obj => SendSpeedRuleAction());
            DeleteSpeedRuleCommand = new ActionCommand<object>(obj => DeleteSpeedRuleAction());
        }

        #region SpeedRule
        private string _querySpeedRuleText;
        public string QuerySpeedRuleText
        {
            get { return _querySpeedRuleText; }
            set
            {
                _querySpeedRuleText = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                {
                    RaisePropertyChanged(() => QuerySpeedRuleText);
                });
            }
        }

        public IActionCommand AddSpeedRuleCommand { get; private set; }
        public IActionCommand QuerySpeedRuleCommand { get; private set; }
        public IActionCommand EditSpeedRuleCommand { get; private set; }
        public IActionCommand SendSpeedRuleCommand { get; private set; }
        public IActionCommand DeleteSpeedRuleCommand { get; private set; }

        private void AddSpeedRuleAction()
        {
            SpeedRuleParameterSettingDetailWindow view = new SpeedRuleParameterSettingDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", "add" } });
            view.Show();
            view.Closed += (s, e) =>
            {
                QuerySpeedRuleCommand.Execute(null);
            };
        }

        private void QuerySpeedRule()
        {
            CommandManageServiceClient client = InitCommandManageClient();
            client.GetSpeedLimitListByNameAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, QuerySpeedRuleText, 1, -1);
        }

        private void EditSpeedRuleAction()
        {
            SpeedRuleParameterSettingDetailWindow view = new SpeedRuleParameterSettingDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", "update" }, { "view", SelectedSpeedLimit } });
            view.Show();
            view.Closed += (s, e) =>
            {
                QuerySpeedRuleCommand.Execute(null);
            };
        }

        private void SendSpeedRuleAction()
        {
            if (SelectedSpeedLimit == null) return;

            SendSpeedRuleDetailView childwindow = new SendSpeedRuleDetailView(string.Empty, new Dictionary<string, object>() { { "model", SelectedSpeedLimit } });
            childwindow.Closed += childwindow_Closed;
            childwindow.Show();
        }

        void childwindow_Closed(object sender, EventArgs e)
        {
            QuerySpeedRule();
        }

        private void DeleteSpeedRuleAction()
        {
            if (SelectedSpeedLimit != null)
            {
                var dialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
                dialogResult.Closed += dialogResult_Closed;
            }
        }

        private void dialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    CommandManageServiceClient client = InitCommandManageClient();
                    client.DeleteSpeedLimitByIDAsync(SelectedSpeedLimit.ID);
                }
            }
        }

        /// <summary>
        /// Route list
        /// </summary>
        PagedCollectionView _SpeedRuleSourcePage;
        public PagedCollectionView SpeedRuleSourcePage
        {
            get { return _SpeedRuleSourcePage; }
        }

        private Command.SpeedLimit _selectedSpeedLimit = new Command.SpeedLimit();
        public Command.SpeedLimit SelectedSpeedLimit
        {
            get { return _selectedSpeedLimit; }
            set
            {
                _selectedSpeedLimit = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectedSpeedLimit));

                SelectedSpeedLimitChange speedLimitChange = new SelectedSpeedLimitChange();
                speedLimitChange.SelectedSpeedLimit = SelectedSpeedLimit;
                EventAggregator.Publish<SelectedSpeedLimitChange>(speedLimitChange);
            }
        }

        private string _SpeedRuleCount = "";
        public string SpeedRuleCount
        {
            get { return _SpeedRuleCount; }
            set
            {
                _SpeedRuleCount = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SpeedRuleCount));
            }
        }

        private CommandManageServiceClient InitCommandManageClient()
        {
            CommandManageServiceClient client = ServiceClientFactory.Create<CommandManageServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetSpeedLimitListByNameCompleted += _client_GetSpeedLimitListByNameCompleted;
            client.DeleteSpeedLimitByIDCompleted += _client_DeleteSpeedLimitByIDCompleted;
            return client;
        }

        private void CloseClient(CommandManageServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }

        }

        void _client_GetSpeedLimitListByNameCompleted(object sender, GetSpeedLimitListByNameCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {

                        ObservableCollection<Command.SpeedLimit> speedRuleCollection = new ObservableCollection<Command.SpeedLimit>(e.Result.Result);
                        List<Command.SpeedLimit> speedList = speedRuleCollection.OrderByDescending(t => t.CreateTime).ToList();
                        SpeedRuleCount = speedList.Count.ToString();
                        _SpeedRuleSourcePage = new PagedCollectionView(speedList);
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SpeedRuleSourcePage));

                        if (e.Result.Result.Count > 0)
                        {
                            SelectedSpeedLimit = speedList[0];
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
                ApplicationContext.Instance.Logger.LogException("client_GetSpeedLimitList", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseClient(client);
            }
        }

        void _client_DeleteSpeedLimitByIDCompleted(object sender, DeleteSpeedLimitByIDCompletedEventArgs e)
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
                            QuerySpeedRuleCommand.Execute(null);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_DeleteSpeedLimitByID", ex);
            }
            finally
            {
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseClient(client);
            }
        }

        #endregion
    }
}
