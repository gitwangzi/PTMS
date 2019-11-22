using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.Manager;
using Gsafety.PTMS.ServiceReference.OrganizationService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using System;
using System.Reflection;
using System.Windows;

namespace Gsafety.Ant.BaseInformation.ViewModels.OrganizationViewModel
{
    /// <summary>
    /// 添加组织机构
    /// </summary>
    [ExportAsViewModel(BaseInformationName.AntProductAddVehicleDepartmentVm)]
    public class AddVehicleDepartmentViewModel : DetailViewModel<Organization>
    {
        private string parentOrgName;
        /// <summary>
        /// 父级名称
        /// </summary>
        public string ParentOrgName
        {
            get { return parentOrgName; }
            set
            {
                parentOrgName = value;
                RaisePropertyChanged(() => ParentOrgName);
            }
        }

        private string parentOrgId;
        /// <summary>
        /// 父级主键编号
        /// </summary>
        public string ParentOrgId
        {
            get { return this.parentOrgId; }
            set
            {
                this.parentOrgId = value;
                RaisePropertyChanged(() => this.ParentOrgId);
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                ValidateName(ExtractPropertyName(() => Name), name);
                RaisePropertyChanged(() => Name);
            }
        }
        private string contact;
        public string Contact
        {
            get { return contact; }
            set
            {
                contact = value;
                ValidateContact(ExtractPropertyName(() => Contact), contact);
                RaisePropertyChanged(() => Contact);
            }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                ValidateEmail(ExtractPropertyName(() => Email), this.email);
                RaisePropertyChanged(() => Email);
            }
        }
        private string phone;
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                ValidatePhone(ExtractPropertyName(() => Phone), this.phone);
                RaisePropertyChanged(() => Phone);
            }
        }

        private Organization currentModel;
        /// <summary>
        /// 当前组织机构
        /// </summary>
        public Organization CurrentOrganization
        {
            get { return this.currentModel; }
            set
            {
                this.currentModel = value;
                RaisePropertyChanged(() => this.CurrentOrganization);
            }
        }

        private Visibility saveVisibility;

        public Visibility SaveButtonVisibility
        {
            get { return this.saveVisibility; }
            set
            {
                this.saveVisibility = value;
                RaisePropertyChanged(() => this.SaveButtonVisibility);
            }

        }

        private Visibility resertVisibility;

        public Visibility ResertButtonVisibility
        {
            get { return this.resertVisibility; }
            set
            {
                this.resertVisibility = value;
                RaisePropertyChanged(() => this.ResertButtonVisibility);
            }
        }



        /// <summary>
        /// 初始化数据
        /// </summary>
        public AddVehicleDepartmentViewModel()
        {
            this.SaveButtonVisibility = Visibility.Visible;
            this.ResertButtonVisibility = Visibility.Visible;
        }


        /// <summary>
        /// 初始化服务客户端方法
        /// </summary>
        /// <returns></returns>
        private OrganizationClient InitClient()
        {
            OrganizationClient client = ServiceClientFactory.Create<OrganizationClient>();
            client.UpdateOrganizationCompleted += client_UpdateOrganizationCompleted;
            client.InsertOrganizationCompleted += client_InsertOrganizationCompleted;

            return client;
        }

        /// <summary>
        /// 事件
        /// </summary>
        public event EventHandler<SaveResultArgs> OnSaveResult;

        protected string operation = string.Empty;

        /// <summary>
        /// 激活View方法
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="viewParameters"></param>
        public new void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                action = viewParameters["action"].ToString();
                this.ParentOrgId = "";
                this.ParentOrgName = "";
                Reset();

                switch (action)
                {
                    case "view":
                        this.Title = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Detail);
                        operation = "view";
                        IsReadOnly = true;
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                        this.SaveButtonVisibility = Visibility.Collapsed;
                        this.ResertButtonVisibility = Visibility.Collapsed;
                        this.parentOrgName = viewParameters["ParentName"].ToString();
                        this.ParentOrgId = viewParameters["ParentId"].ToString();
                        this.CurrentOrganization = viewParameters["data"] as Organization;
                        ViewOrganization();
                        break;

                    //update
                    case "Update":
                        this.Title = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Edit);
                        this.SaveButtonVisibility = Visibility.Visible;
                        this.ResertButtonVisibility = Visibility.Visible;
                        operation = "Update";
                        IsReadOnly = false;
                        JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                        this.parentOrgName = viewParameters["ParentName"].ToString();
                        this.ParentOrgId = viewParameters["ParentId"].ToString();
                        this.CurrentOrganization = viewParameters["data"] as Organization;
                        JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ParentOrgName));
                        ViewOrganization();
                        break;

                    case "addOrg":
                        this.Title = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Add);
                        this.SaveButtonVisibility = Visibility.Visible;
                        this.ResertButtonVisibility = Visibility.Visible;
                        operation = "addOrg";
                        if (viewParameters.Keys.Contains("ParentId"))
                        {
                            this.parentOrgName = viewParameters["ParentName"].ToString();
                            this.ParentOrgId = viewParameters["ParentId"].ToString();
                        }
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ParentOrgName));
                        IsReadOnly = false;
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }

        /// <summary>
        /// 查看当前用户信息
        /// </summary>
        protected void ViewOrganization()
        {
            try
            {
                if (this.CurrentOrganization != null)
                {
                    this.Name = this.CurrentOrganization.Name;
                    this.Phone = this.CurrentOrganization.Phone;
                    this.Email = this.CurrentOrganization.Email;
                    this.Contact = this.CurrentOrganization.Contact;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        /// <summary>
        /// 提交新数据
        /// </summary>
        protected override void OnCommitted()
        {
            OrganizationClient client = InitClient();
            try
            {
                Organization dataObj = new Organization();
                dataObj.Name = this.Name;
                dataObj.Contact = this.Contact;
                dataObj.Email = this.Email;
                dataObj.Phone = this.Phone;
                dataObj.Valid = 1;
                dataObj.ParentID = string.IsNullOrEmpty(this.ParentOrgId) ? "0" : this.ParentOrgId;
                dataObj.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                if (operation == "Update")
                {
                    dataObj.ID = this.CurrentOrganization.ID;
                    dataObj.CreateTime = CurrentOrganization.CreateTime.ToUniversalTime();
                    dataObj.Creator = CurrentOrganization.Creator;
                    client.UpdateOrganizationAsync(dataObj);
                }
                else if (operation == "addOrg")
                {
                    dataObj.ID = Guid.NewGuid().ToString();
                    dataObj.Creator = ApplicationContext.Instance.AuthenticationInfo.UserID;
                    dataObj.CreateTime = DateTime.Now.ToUniversalTime();
                    client.InsertOrganizationAsync(dataObj, ApplicationContext.Instance.AuthenticationInfo.UserID);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                this.CloseClient(client);
            }
        }


        /// <summary>
        /// 关闭组织机构服务客户端方法
        /// </summary>
        /// <param name="client">要关闭的对象</param>
        private void CloseClient(OrganizationClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }

        void client_UpdateOrganizationCompleted(object sender, UpdateOrganizationCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("client_UpdateOrganizationCompleted", ex);
            }
            finally
            {
                OrganizationClient client = sender as OrganizationClient;
                CloseClient(client);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_InsertOrganizationCompleted(object sender, InsertOrganizationCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.client_AddInstallStationCompleted", ex);
            }
            finally
            {
                OrganizationClient client = sender as OrganizationClient;
                CloseClient(client);
            }
        }


        /// <summary>
        /// 重置
        /// </summary>
        protected override void Reset()
        {
            if (action == "addOrg")
            {
                Name = string.Empty;
                JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Name));
                Contact = string.Empty;
                JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Contact));
                Email = string.Empty;
                JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Email));
                Phone = string.Empty;
                JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Phone));
            }
            else
            {
                ViewOrganization();
            }
        }
        /// <summary>
        /// 返回
        /// </summary>
        protected override void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs(ManagerName.AntProductVehicleDepartmentV, new System.Collections.Generic.Dictionary<string, object>() { { "action", "return" } }));
        }


        private void ValidateParentOrgName(string prop, string value)
        {
            ValidateRequire(prop, value);
        }

        private void ValidateName(string prop, string value)
        {
            ValidateRequire(prop, value);
        }

        private void ValidateContact(string prop, string value)
        {

        }

        private void ValidateEmail(string prop, string value)
        {
            ValidateEmailFormat(prop, value);
        }

    }

}
