using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.ServiceReference.OrganizationService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Bases.Models;

namespace Gsafety.PTMS.Manager.ViewModels.OrganizationViewModel
{
    [ExportAsViewModel(ManagerName.AntProductAddVehicleDepartmentVm)]
    public class AddVehicleDepartmentViewModel : DetailViewModel<OrganizationEx>
    {
        /**
         * 组织机构名称
         * 联系人
         * Email
         * 电话
         * 是否可用
         * **/

        //
        /// <summary>
        /// 初始化数据
        /// </summary>
        public AddVehicleDepartmentViewModel()
        {
            client = ServiceClientFactory.Create<OrganizationClient>();

        }

        protected string operation = string.Empty;
        protected string action;
        private OrganizationClient client = null;

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            action = viewParameters["action"].ToString();

            switch (action)
            {
                case "view":
                    operation = "view";
                    IsReadOnly = true;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                    InitialModel = viewParameters["data"] as OrganizationEx;
                    ViewOrganization();
                    break;
                case "viewOrg":
                    operation = "viewOrg";
                    IsReadOnly = false;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                    InitialModel = viewParameters["data"] as OrganizationEx;
                    ViewOrganization();
                    CurrentModel = new OrganizationEx();
                    break;
                case "addOrg":
                    Reset();
                    operation = "addOrg";
                    InitialModel = viewParameters["data"] as OrganizationEx;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ParentOrgName));
                    IsReadOnly = false;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 查询当前用户信息
        /// </summary>
        protected void ViewOrganization()
        {
            try
            {
                if (InitialModel != null)
                {
                    //Name = InitialModel.Name;
                    //Phone = InitialModel.Phone;
                    //Email = InitialModel.Email;
                    //Contact = InitialModel.Contact;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 增加新数据
        /// </summary>
        protected override void OnCommitted()
        {
            try
            {
                Organization dataObj = new Organization();
                dataObj.Name = this.Name;
                dataObj.Contact = this.Contact;
                dataObj.Email = this.Email;
                dataObj.Phone = this.Phone;
                dataObj.Valid = 1;
                //dataObj.ParentID = string.IsNullOrEmpty(InitialModel.ParentID) ? "0" : InitialModel.ParentID;
                dataObj.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;

                if (operation == "viewOrg")
                {
                    //dataObj.ID = InitialModel.ID;
                    client.UpdateOrganizationCompleted += client_UpdateOrganizationCompleted;
                    client.UpdateOrganizationAsync(dataObj);
                }
                else if (operation == "addOrg")
                {
                    dataObj.ID = Guid.NewGuid().ToString();
                    dataObj.Creator = ApplicationContext.Instance.AuthenticationInfo.UserID;
                    client.InsertOrganizationCompleted += client_InsertOrganizationCompleted;
                    client.InsertOrganizationAsync(dataObj, ApplicationContext.Instance.AuthenticationInfo.UserID);
                }
            }
            catch (Exception)
            {
            }
        }

        void client_UpdateOrganizationCompleted(object sender, UpdateOrganizationCompletedEventArgs e)
        {
            try
            {
                SaveResultArgs args = new SaveResultArgs();

                if (e.Cancelled)
                {
                    return;
                }
                if (e.Error != null)
                {
                    args.Result = false;
                    //MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                    args.Message = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError);
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                }
                if (e.Result.IsSuccess == false)
                {
                    if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                    {
                        args.Result = false;
                        //MessageBoxHelper.ShowDialog(LProxy.Caption, e.Result.ErrorMsg);
                        args.Message = e.Result.ErrorMsg;
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ErrorMsg);
                    }
                    else
                    {
                        args.Result = false;
                        //MessageBoxHelper.ShowDialog(LProxy.Caption, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        args.Message =
                            ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError);
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ExceptionMessage);
                    }
                }
                else
                {
                    args.Result = true;
                    // MessageBoxHelper.ShowDialog(LProxy.Caption, LProxy.OperatedSuccessed);
                }



            }
            catch (Exception ex)
            {
                //InilitizeViewModel();
                ApplicationContext.Instance.Logger.LogException("DriverInfoDetailViewModel.client_AddDriverInfoCompleted", ex);
            }
            finally
            {
                // client.AddInstallStationCompleted -= client_AddInstallStationCompleted;
                if (this.client != null)
                {
                    this.client.CloseAsync();
                }
                this.client = null;
                // InilitizeViewModel();
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
                SaveResultArgs args = new SaveResultArgs();

                if (e.Cancelled)
                {
                    return;
                }
                if (e.Error != null)
                {
                    args.Result = false;
                    //MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                    args.Message = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError);
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                }
                if (e.Result.IsSuccess == false)
                {
                    if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                    {
                        args.Result = false;
                        //MessageBoxHelper.ShowDialog(LProxy.Caption, e.Result.ErrorMsg);
                        args.Message = e.Result.ErrorMsg;
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ErrorMsg);
                    }
                    else
                    {
                        args.Result = false;
                        //MessageBoxHelper.ShowDialog(LProxy.Caption, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        args.Message =
                            ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError);
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ExceptionMessage);
                    }
                }
                else
                {
                    args.Result = true;
                    // MessageBoxHelper.ShowDialog(LProxy.Caption, LProxy.OperatedSuccessed);
                }



            }
            catch (Exception ex)
            {

                ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.client_AddInstallStationCompleted", ex);
            }
            finally
            {
                // client.AddInstallStationCompleted -= client_AddInstallStationCompleted;
                if (this.client != null)
                {
                    this.client.CloseAsync();
                }
                this.client = null;

            }
        }


        /// <summary>
        /// 重置
        /// </summary>
        protected override void Reset()
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
        /// <summary>
        /// 返回
        /// </summary>
        protected override void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs(ManagerName.AntProductVehicleDepartmentV, new System.Collections.Generic.Dictionary<string, object>() { { "action", "return" } }));
        }

        #region Field

        private string parentOrgName;
        public string ParentOrgName
        {
            get { return parentOrgName; }
            set
            {
                parentOrgName = value;
                RaisePropertyChanged(() => ParentOrgName);
            }
        }
        private string name;
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
        public string Contact
        {
            get { return contact; }
            set
            {
                contact = value;
                RaisePropertyChanged(() => Contact);
                //ValidateContact(ExtractPropertyName(() => Contact), contact);
                //Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Contact));
            }
        }
        private string email;
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
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                RaisePropertyChanged(() => Phone);
            }
        }

        #endregion


        #region VehicleField

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
            ValidateRequire(prop, value);
        }

        private void ValidateEmail(string prop, string value)
        {
            ValidateRequire(prop, value);
            if (!string.IsNullOrEmpty(value))
            {
                ValidateEmailFormat(prop, value);
            }
        }

        private void ValidatePhone(string prop, string value)
        {
            ValidateRequire(prop, value);
        }

        #endregion


    }

}
