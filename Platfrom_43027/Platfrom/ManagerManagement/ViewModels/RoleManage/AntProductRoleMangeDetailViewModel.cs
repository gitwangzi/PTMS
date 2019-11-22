using BaseLib.ViewModels;
using Jounce.Core.ViewModel;
using System.Collections.Generic;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Bases.Models;
using System.Linq;
using Gsafety.PTMS.ServiceReference.AccountService;
using Jounce.Core.View;
using Jounce.Core.Command;
using System;
using Jounce.Framework.Command;
using System.Windows;
using Gsafety.PTMS.Share;
using System.Reflection;
using Gsafety.Common.Controls;
using Gsafety.Common.CommMessage;

namespace Gsafety.PTMS.Manager.ViewModels.RoleManage
{
    [ExportAsViewModel(ManagerName.AntProductRoleMangeDetailVm)]
    public class AntProductRoleMangeDetailViewModel : DetailViewModel<Role>
    {
        //private RoleServiceClient _client;
        private bool _enable;
        public bool RoleSelectEnable
        {
            get { return _enable; }
            set
            {
                _enable = value;
                RaisePropertyChanged(() => RoleSelectEnable);
            }
        }

        public List<EnumModel> RoleCategoryItems { get; set; }
        public event EventHandler<SaveResultArgs> OnSaveResult;
        #region CurrentRoleCategory
        private EnumModel _currentRoleCategory;
        public EnumModel CurrentRoleCategory
        {
            get
            {
                return _currentRoleCategory;
            }
            set
            {
                if (_currentRoleCategory != value)
                {
                    _currentRoleCategory = value;
                    RaisePropertyChanged(() => CurrentRoleCategory);
                }
            }
        }


        #endregion

        public AntProductRoleMangeDetailViewModel()
        {
            try
            {
                InitRoleCategoryItems();
                CurrentRoleCategory = RoleCategoryItems.FirstOrDefault();
            }
            catch (System.Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private RoleServiceClient InitServiceClient()
        {
            RoleServiceClient _client = ServiceClientFactory.Create<RoleServiceClient>();
            ServiceClientFactory.CreateMessageHeader(_client.InnerChannel);
            _client.UpdateRoleCompleted += _client_UpdateRoleCompleted;
            _client.InsertRoleCompleted += _client_InsertRoleCompleted;
            return _client;
        }

        void _client_InsertRoleCompleted(object sender, InsertRoleCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("DriverInfoDetailViewModel.client_AddDriverInfoCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        private static void CloseClient(object sender)
        {
            RoleServiceClient _client = sender as RoleServiceClient;
            if (_client != null)
            {
                _client.CloseAsync();
                _client = null;
            }
        }

        void _client_UpdateRoleCompleted(object sender, UpdateRoleCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("DriverInfoDetailViewModel.client_AddDriverInfoCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        protected override void ValidateAll()
        {
            ValidateName(ExtractPropertyName(() => Name), _name);
        }

        protected override void OnCommitted()
        {
            CurrentModel.RoleCategory = (short)CurrentRoleCategory.EnumValue;
            CurrentModel.Name = Name;
            CurrentModel.Description = Description;
            LogOperate log = new LogOperate();
            log.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
            log.ID = Guid.NewGuid().ToString();
            log.OperateTime = DateTime.Now.ToUniversalTime();
            log.OperatorID = ApplicationContext.Instance.AuthenticationInfo.UserID;
            log.OperatorName = ApplicationContext.Instance.AuthenticationInfo.Account;
            switch (action)
            {
                case "update":
                    CurrentModel.ClientID = InitialModel.ClientID;
                    CurrentModel.CreateTime = InitialModel.CreateTime.ToUniversalTime();
                    CurrentModel.Creator = InitialModel.Creator;
                    CurrentModel.ID = InitialModel.ID;
                    CurrentModel.IsPredefined = InitialModel.IsPredefined;
                    log.OperateContent = ApplicationContext.Instance.StringResourceReader.GetString("UpdateRole") + ":" + CurrentModel.Name;
                    RoleServiceClient _client = InitServiceClient();

                    bool IsUpdateRole = false;
                    if(OriginRoleCategory !=CurrentModel.RoleCategory)
                    {
                        IsUpdateRole = true;
                    }
                    _client.UpdateRoleAsync(CurrentModel, IsUpdateRole,log);
                    break;

                case "add":
                    log.OperateContent = ApplicationContext.Instance.StringResourceReader.GetString("AddRole") + ":" + CurrentModel.Name;
                    CurrentModel.Creator = ApplicationContext.Instance.AuthenticationInfo.UserID;
                    RoleServiceClient _client2 = InitServiceClient();
                    _client2.InsertRoleAsync(CurrentModel, log);
                    break;
            }
        }

        private void InitRoleCategoryItems()
        {
            var adapter = new EnumAdapter<RoleCategory>();
            var categorys = adapter.GetEnumInfos();

            RoleCategoryItems = new List<EnumModel>();

            foreach (var item in categorys)
            {
                if (item.Value == (int)RoleCategory.SuperPower || item.Value == (int)RoleCategory.ClientAdmin || item.Value == (int)RoleCategory.MaintainAdmin || item.Value == (int)RoleCategory.SecurityAdmin)
                {
                    continue;
                }

                RoleCategoryItems.Add(new EnumModel()
                {
                    EnumValue = item.Value,
                    EnumName = item.Name,
                    ShowName = item.LocalizedString,
                });
            }
        }

        private int OriginRoleCategory { get; set; }

        public new void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                action = viewParameters["action"].ToString();

                switch (action)
                {
                    case "view":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_View");
                        IsReadOnly = true;
                        RoleSelectEnable = false;
                        ViewVisibility = Visibility.Collapsed;
                        InitialModel = viewParameters["model"] as Role;
                        InitialFromInitialModel();
                        break;
                    case "update":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                        IsReadOnly = false;
                        RoleSelectEnable = true;
                        ViewVisibility = Visibility.Visible;
                        InitialModel = viewParameters["model"] as Role;
                        InitialFromInitialModel();

                        OriginRoleCategory = CurrentRoleCategory.EnumValue;
                        CurrentModel = new Role();
                        break;
                    case "add":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                        IsReadOnly = false;
                        RoleSelectEnable = true;
                        ViewVisibility = Visibility.Visible;
                        CurrentModel = new Role()
                        {
                            ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID,
                            Creator = ApplicationContext.Instance.AuthenticationInfo.Account,
                            ID = Guid.NewGuid().ToString(),
                            IsPredefined = false,
                        };
                        Reset();
                        break;
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        protected override void Reset()
        {
            Name = string.Empty;
            Description = string.Empty;
        }

        public void InitialFromInitialModel()
        {
            Name = InitialModel.Name;
            CurrentRoleCategory = RoleCategoryItems.FirstOrDefault(t => t.EnumValue == InitialModel.RoleCategory);
            Description = InitialModel.Description;
        }

        protected override void Return()
        {
            base.Return();

            EventAggregator.Publish(new ViewNavigationArgs(ManagerName.AntProductRoleManageV, new Dictionary<string, object>() { { "action", "return" } }));
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value == null ? null : value.Trim();
                ValidateName(ExtractPropertyName(() => Name), _name);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Name));
            }
        }
        private void ValidateName(string prop, string value)
        {
            ValidateRequire(prop, value);
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Description));
            }
        }
    }
}
