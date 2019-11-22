using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.AccountService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Gsafety.PTMS.Manager.ViewModels.OrganizationViewModel
{
    /// <summary>
    /// 用户部门明细ViewModel
    /// </summary>
    [ExportAsViewModel(ManagerName.AntProductUserDepartmentDetailVm)]
    public class AntProductUserDepartmentDetailViewModel : DetailViewModel<UsrDepartment>
    {
        /// <summary>
        /// 用户操作标示
        /// 新增
        /// 修改
        /// 查看明细
        /// </summary>
        private string _action;

        private string _parentName;
        /// <summary>
        /// 父级名称
        /// </summary>
        public string ParentName
        {
            get { return this._parentName; }
            set
            {
                this._parentName = value;
                RaisePropertyChanged(() => this.ParentName);
            }
        }

        private string name;
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value == null ? null : value.Trim();
                ValidateRequire(ExtractPropertyName(() => this.Name), this.name);
                RaisePropertyChanged(() => this.Name);
            }
        }

        private string contact;

        public string Contact
        {
            get { return this.contact; }
            set
            {
                this.contact = value == null ? null : value.Trim();
                ValidateRequire(ExtractPropertyName(() => this.Contact), this.contact);
                RaisePropertyChanged(() => this.Contact);
            }
        }

        private string email;

        public string Email
        {
            get { return this.email; }
            set
            {
                this.email = value == null ? null : value.Trim();
                this.ValidateEmail(ExtractPropertyName(() => this.Email), this.email);
                RaisePropertyChanged(() => this.Email);
            }
        }

        private string phone;

        public string Phone
        {
            get { return this.phone; }
            set
            {
                this.phone = value == null ? null : value.Trim();
                ValidatePhone(ExtractPropertyName(() => this.Phone), this.phone);
                RaisePropertyChanged(() => this.Phone);
            }
        }

        public event EventHandler<SaveResultArgs> OnSaveResult;


        public AntProductUserDepartmentDetailViewModel()
        {
            try
            {
                this.IsReadOnly = false;
                this.ViewVisibility = Visibility.Visible;
                this.InlizeDataProperty();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("AntProductUserDepartmentDetailViewModel()", ex);
            }
        }

        private UsrDepartmentClient InitServiceClient()
        {
            UsrDepartmentClient client = ServiceClientFactory.Create<UsrDepartmentClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            //新增事件完成
            client.InsertUsrDepartmentCompleted += client_InsertUsrDepartmentCompleted;
            client.UpdateUsrDepartmentCompleted += client_UpdateUsrDepartmentCompleted;
            client.GetUsrDepartmentCompleted += client_GetUsrDepartmentCompleted;
            return client;
        }

        protected override void ValidateAll()
        {
            ValidateRequire(ExtractPropertyName(() => this.Name), this.name);
            ValidateRequire(ExtractPropertyName(() => this.Contact), this.contact);
            this.ValidateEmail(ExtractPropertyName(() => this.Email), this.email);

            ValidatePhone(ExtractPropertyName(() => this.Phone), this.Phone);
        }

        /// <summary>
        /// 验证Email是否合法
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateEmail(string prop, string value)
        {
            ValidateEmailFormat(prop, value);
        }

        protected override void ValidatePhone(string prop, string value)
        {
            ValidateRequire(ExtractPropertyName(() => this.Phone), this.Phone);
            if (!string.IsNullOrEmpty(value))
            {
                base.ValidatePhone(prop, value);
            }
        }

        private void InlizeDataProperty()
        {
            if (action == "add")
            {
                this.Name = "";
                this.Contact = "";
                this.Email = "";
                this.Phone = "";
            }
            else
            {
                this.Name = CurrentModel.Name;
                this.Contact = CurrentModel.Contact;
                this.Email = CurrentModel.Email;
                this.Phone = CurrentModel.Phone;
            }
        }

        public new void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                this._action = "";
                this._action = viewParameters["action"].ToString();
                this.CurrentModel = new UsrDepartment();
                this.InlizeDataProperty();
                switch (_action)
                {
                    case "add":
                        this.IsReadOnly = false;
                        this.ViewVisibility = Visibility.Visible;
                        this.Title = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Add);
                        this.CurrentModel = new UsrDepartment()
                        {
                            ID = Guid.NewGuid().ToString(),
                            ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID,
                            Creator = ApplicationContext.Instance.AuthenticationInfo.Account,
                        };

                        if (viewParameters.ContainsKey("departmentParentId"))
                        {
                            this.CurrentModel.ParentID = viewParameters["departmentParentId"].ToString();
                        }
                        if (viewParameters.ContainsKey("departmentParentName"))
                        {
                            this.ParentName = viewParameters["departmentParentName"].ToString();
                        }
                        break;
                    case "update":
                        this.IsReadOnly = false;
                        this.ViewVisibility = Visibility.Visible;
                        this.Title = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Edit);
                        //父级节点名称
                        this.ParentName = viewParameters["departmentParentName"].ToString();
                        string id = viewParameters["departmentId"].ToString();
                        UsrDepartmentClient client = InitServiceClient();
                        client.GetUsrDepartmentAsync(id);
                        break;
                    case "view":
                        this.IsReadOnly = true;
                        this.ViewVisibility = Visibility.Collapsed;
                        this.Title = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Detail);
                        //父级节点名称
                        this.ParentName = viewParameters["departmentParentName"].ToString();
                        string departmentId = viewParameters["departmentId"].ToString();
                        UsrDepartmentClient client2 = InitServiceClient();
                        client2.GetUsrDepartmentAsync(departmentId);
                        break;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_UpdateUsrDepartmentCompleted(object sender, UpdateUsrDepartmentCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                CloseDepartmentClient(sender);
            }
        }

        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_InsertUsrDepartmentCompleted(object sender, InsertUsrDepartmentCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                CloseDepartmentClient(sender);
            }
        }

        void client_GetUsrDepartmentCompleted(object sender, GetUsrDepartmentCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == true)
                {
                    return;
                }
                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ServerError);
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    return;
                }
                if (e.Result.IsSuccess == false)
                {
                    if (string.IsNullOrEmpty(e.Result.ErrorMsg))
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), e.Result.ErrorMsg);
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), e.Result.ErrorMsg);
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("OperatorServiceError"));
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Result.ExceptionMessage);
                    }
                }
                else
                {
                    this.InlitizeCurrentModel(e.Result.Result);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                CloseDepartmentClient(sender);
            }
        }

        private static void CloseDepartmentClient(object sender)
        {
            UsrDepartmentClient client = sender as UsrDepartmentClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        /// <summary>
        /// 重置方法
        /// </summary>
        protected override void Reset()
        {
            InlizeDataProperty();
        }

        /// <summary>
        /// 返回方法
        /// </summary>
        protected override void Return()
        {
            base.Return();
            this.EventAggregator.Publish(new ViewNavigationArgs(ManagerName.AntProductUserDepartmentV));

        }

        protected override void OnCommitted()
        {
            try
            {
                this.CurrentModel.Name = this.Name;
                this.CurrentModel.Contact = this.Contact;
                this.CurrentModel.Email = this.Email;
                this.CurrentModel.Phone = this.Phone;
                switch (this._action)
                {
                    case "add":
                        UsrDepartmentClient client = InitServiceClient();
                        client.InsertUsrDepartmentAsync(this.CurrentModel);
                        break;
                    case "update":
                        UsrDepartmentClient client2 = InitServiceClient();
                        client2.UpdateUsrDepartmentAsync(this.CurrentModel);
                        break;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// 初始化当前模型
        /// </summary>
        /// <param name="department"></param>
        private void InlitizeCurrentModel(UsrDepartment department)
        {
            try
            {
                this.CurrentModel.ID = department.ID;
                this.CurrentModel.ClientID = department.ClientID;
                this.CurrentModel.ParentID = department.ParentID;
                this.CurrentModel.Creator = ApplicationContext.Instance.AuthenticationInfo.Account;
                this.CurrentModel.CreateTime = DateTime.Now;
                this.CurrentModel.Name = department.Name;
                this.CurrentModel.Contact = department.Contact;
                this.CurrentModel.Email = department.Email;
                this.CurrentModel.Phone = department.Phone;
                RaisePropertyChanged(() => this.CurrentModel);
                this.Name = department.Name;
                this.Contact = department.Contact;
                this.Email = department.Email;
                this.Phone = department.Phone;

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("AntProductUserDepartmentDetailViewModel.InlitizeCurrentModel", ex);
            }
        }
    }
}
