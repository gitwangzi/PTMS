using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.ServiceReference.OrganizationService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using OrganizationEx = Gsafety.PTMS.Bases.Models.OrganizationEx;

namespace Gsafety.Ant.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.VehicleOrganizationDetailVm)]
    public class VehicleOrganizationDetailViewModel : DetailViewModel<Organization>
    {
        #region 属性

        private string _title;
        /// <summary>
        /// 当前界面的标题
        /// </summary>
        public string CurrentViewTitle
        {
            get { return this._title; }
            set
            {
                this._title = value;
                RaisePropertyChanged(() => this.CurrentViewTitle);
            }
        }

        private string id;
        /// <summary>
        /// 主键编号
        /// </summary>
        public string Id { get; set; }

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
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged(() => Name);
            }
        }
        private string contact;
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact
        {
            get { return contact; }
            set
            {
                contact = value;
                RaisePropertyChanged(() => Contact);
            }
        }
        private string email;
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                RaisePropertyChanged(() => Email);
            }
        }
        private string phone;
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                RaisePropertyChanged(() => Phone);
            }
        }

        /// <summary>
        /// 要进行的操作行为
        /// </summary>
        public string OperationAction { get; set; }

        private OrganizationClient client = null;

        private OrganizationEx organizationEx;

        /// <summary>
        /// 临时组织机构结构
        /// </summary>
        private OrganizationEx OrganizationEx
        {
            get { return this.organizationEx; }
            set
            {
                this.organizationEx = value;
                RaisePropertyChanged(() => this.OrganizationEx);
            }
        }

        private Visibility resertButtonVisibility;
        /// <summary>
        /// 重置按钮是否可见
        /// </summary>
        public Visibility ResertButtonVisibility
        {
            get { return resertButtonVisibility; }
            set
            {
                resertButtonVisibility = value;
                RaisePropertyChanged(() => ResertButtonVisibility);
            }
        }

        private Visibility saveButtonVisibility;
        /// <summary>
        /// 保存按钮是否可见
        /// </summary>
        public Visibility SaveButtonVisibility
        {
            get { return saveButtonVisibility; }
            set
            {
                saveButtonVisibility = value;
                RaisePropertyChanged(() => SaveButtonVisibility);
            }
        }



        #endregion

        #region 构造函数

        public VehicleOrganizationDetailViewModel()
        {
            this.OperationAction = "";
            this.InlitizeViewModel();
            this.InlitizeClient();
        }

        #endregion

        #region 方法


        private void InlitizeViewModel()
        {
            this.SaveButtonVisibility = Visibility.Collapsed;
            this.ResertButtonVisibility = Visibility.Collapsed;
            this.ClearAll();
            this.OrganizationEx = new OrganizationEx();
        }

        private void InlitizeClient()
        {
            this.client = ServiceClientFactory.Create<OrganizationClient>();
            this.client.UpdateOrganizationCompleted += client_UpdateOrganizationCompleted;
            this.client.InsertOrganizationCompleted += client_InsertOrganizationCompleted;
        }

        void client_InsertOrganizationCompleted(object sender, InsertOrganizationCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == true)
                {
                    return;
                }
                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    return;
                }
                if (e.Result.IsSuccess == false)
                {
                    if (string.IsNullOrEmpty(e.Result.ErrorMsg))
                    {
                        //MessageBox.Show(e.Result.ErrorMsg);
                        MessageBoxHelper.ShowDialog(
                            ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                            ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ErrorMsg);
                    }
                    else
                    {
                        //MessageBox.Show("操作服务异常");
                        MessageBoxHelper.ShowDialog(
                            ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                            ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError),
                            MessageDialogButton.OkAndCancel);
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ExceptionMessage);
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatedSuccessed),
                        MessageDialogButton.OkAndCancel);
                }
            }
            catch (Exception ex)
            {
                this.InlitizeClient();
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                this.Close();
            }
        }

        private void Close()
        {
            if (this.client != null)
            {
                this.client.CloseAsync();
            }
            this.client = null;
        }

        void client_UpdateOrganizationCompleted(object sender, UpdateOrganizationCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == true)
                {
                    return;
                }
                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    return;
                }
                if (e.Result.IsSuccess == false)
                {
                    if (string.IsNullOrEmpty(e.Result.ErrorMsg))
                    {
                        //MessageBox.Show(e.Result.ErrorMsg);
                        MessageBoxHelper.ShowDialog(
                            ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                            ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ErrorMsg);
                    }
                    else
                    {
                        //MessageBox.Show("操作服务异常");
                        MessageBoxHelper.ShowDialog(
                            ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                            ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError),
                            MessageDialogButton.OkAndCancel);
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ExceptionMessage);
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatedSuccessed),
                        MessageDialogButton.OkAndCancel);

                }


            }
            catch (Exception ex)
            {
                this.InlitizeClient();
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                this.Close();
            }
        }


        /// <summary>
        /// 重置
        /// </summary>
        private void Clear()
        {
            this.Name = "";
            this.Contact = "";
            this.Phone = "";
            this.Email = "";
        }

        protected override void Reset()
        {
            this.Clear();
        }


        private void ClearAll()
        {
            this.ParentOrgId = "";
            this.ParentOrgName = "";
            this.Name = "";
            this.Contact = "";
            this.Phone = "";
            this.Email = "";
        }

        private void InlitizeData()
        {
            this.Id = this.OrganizationEx.Organization.ID;
            this.ParentOrgId = string.IsNullOrEmpty(this.OrganizationEx.Organization.ParentID) == true ? "" : this.OrganizationEx.Organization.ParentID;
            this.parentOrgName = this.OrganizationEx.Organization.Name;
            this.Name = this.OrganizationEx.Organization.Name;
            this.Contact = this.OrganizationEx.Organization.Contact;
            this.Phone = this.OrganizationEx.Organization.Phone;
            this.Email = this.OrganizationEx.Organization.Email;
        }


        protected override void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            //当前需要进行的业务动作，添加、修改、查看详细

            if (viewParameters.Count == 0)
            {
                this.action = "";
            }
            else
            {
                this.action = viewParameters["action"].ToString();
            }
            this.ClearAll();
            switch (this.action)
            {
                case "Add":

                    this.OperationAction = "Add";
                    IsReadOnly = false;
                    this.OrganizationEx = viewParameters["data"] as OrganizationEx;
                    if (this.OrganizationEx == null)
                    {
                        this.ParentOrgId = "0";
                    }
                    else
                    {
                        this.ParentOrgId = this.OrganizationEx.Organization.ParentID;
                    }
                    break;
                case "Update":
                    IsReadOnly = false;
                    this.OperationAction = "Update";
                    this.OrganizationEx = viewParameters["data"] as OrganizationEx;
                    this.InlitizeData();
                    break;
                case "View":
                    IsReadOnly = true;
                    this.OperationAction = "View";
                    this.OrganizationEx = viewParameters["data"] as OrganizationEx;
                    this.InlitizeData();
                    break;
            }

        }

        protected override void OnCommitted()
        {
            try
            {
                if (this.client == null)
                {
                    this.InlitizeClient();
                }
                Organization dataObj = new Organization();
                dataObj.Name = this.Name;
                dataObj.Contact = this.Contact;
                dataObj.Email = this.Email;
                dataObj.Phone = this.Phone;
                dataObj.Valid = 1;
                dataObj.ParentID = this.ParentOrgId;
                dataObj.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                if (this.OperationAction == "Add")
                {
                    dataObj.ID = Guid.NewGuid().ToString();
                    dataObj.Creator = ApplicationContext.Instance.AuthenticationInfo.UserID;
                    client.InsertOrganizationAsync(dataObj, ApplicationContext.Instance.AuthenticationInfo.UserID);
                }
                else if (this.OperationAction == "Update")
                {
                    dataObj.ID = this.Id;
                    this.client.UpdateOrganizationAsync(dataObj);
                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            //finally
            //{
            //    if (this.client != null)
            //    {
            //        this.client.CloseAsync();
            //    }
            //}

        }

        #endregion


    }
}
