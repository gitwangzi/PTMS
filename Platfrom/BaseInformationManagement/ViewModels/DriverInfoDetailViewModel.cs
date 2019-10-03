using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.ServiceReference.ChauffeurService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;

namespace Gsafety.Ant.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.DriverInfoDetailVm)]
    public class DriverInfoDetailViewModel : DetailViewModel<Chauffeur>
    {
        protected string action;

        public event EventHandler<SaveResultArgs> OnSaveResult;

        public DriverInfoDetailViewModel()
        {
            this.InilitizeViewModel();
        }

        private ChauffeurServiceClient InilitizeViewModel()
        {
            ChauffeurServiceClient client = ServiceClientFactory.Create<ChauffeurServiceClient>();
            client.AddChauffeurCompleted += client_AddChauffeurCompleted;
            client.UpdateChauffeurCompleted += client_UpdateChauffeurCompleted;
            return client;
        }

        public new void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
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
                        SaveButtonVisibility = Visibility.Collapsed;
                        ResertButtonVisibility = Visibility.Collapsed;

                        ViewVisibility = Visibility.Collapsed;
                        InitialModel = viewParameters["model"] as Chauffeur;
                        InitialFromInitialModel();
                        break;
                    case "update":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        SaveButtonVisibility = Visibility.Visible;
                        ResertButtonVisibility = Visibility.Visible;

                        InitialModel = viewParameters["model"] as Chauffeur;
                        InitialFromInitialModel();
                        CurrentModel = new Chauffeur();
                        break;
                    case "add":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        SaveButtonVisibility = Visibility.Visible;
                        ResertButtonVisibility = Visibility.Visible;
                        CurrentModel = new Chauffeur();
                        Reset();
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

        protected override void Reset()
        {
            try
            {
                if (InitialModel == null)
                {
                    Name = string.Empty;
                    ICardID = string.Empty;
                    Address = string.Empty;
                    DriverLicense = string.Empty;
                    Phone = string.Empty;
                    CellPhone = string.Empty;
                    Email = string.Empty;
                    Note = string.Empty;
                }
                else
                {
                    InitialFromInitialModel();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public void InitialFromInitialModel()
        {
            Name = InitialModel.Name;
            ICardID = InitialModel.ICardID;
            Address = InitialModel.Address;
            DriverLicense = InitialModel.DriverLicense;
            Phone = InitialModel.Phone;
            CellPhone = InitialModel.CellPhone;
            Email = InitialModel.Email;
            Note = InitialModel.Note;
        }

        protected override void ValidateAll()
        {
            ValidateName(ExtractPropertyName(() => Name), _name);
            ValidateDistrictCode(ExtractPropertyName(() => ICardID), _iCardID);
            ValidateAddress(ExtractPropertyName(() => Address), _address);
            ValidateDirector(ExtractPropertyName(() => DriverLicense), _driverLicense);
            ValidateTelePhone(ExtractPropertyName(() => Phone), _phone);
            ValidateContact(ExtractPropertyName(() => CellPhone), CellPhone);
            ValidateEmail(ExtractPropertyName(() => Email), _email);

        }
        protected override void OnCommitted()
        {
            try
            {
                CurrentModel.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                CurrentModel.Name = Name;
                CurrentModel.ICardID = ICardID;
                CurrentModel.DriverLicense = DriverLicense;
                CurrentModel.Address = Address;
                CurrentModel.Phone = Phone;
                CurrentModel.CellPhone = CellPhone;
                CurrentModel.Email = Email;
                CurrentModel.Note = Note;

                if (action.Equals("update"))
                {
                    CurrentModel.ID = InitialModel.ID;
                    CurrentModel.Valid = InitialModel.Valid;
                    CurrentModel.CreateTime = InitialModel.CreateTime.ToUniversalTime();
                    CurrentModel.Creator = InitialModel.Creator;
                    Update();

                }
                else
                {
                    CurrentModel.ID = Guid.NewGuid().ToString();
                    CurrentModel.Valid = 1;
                    CurrentModel.CreateTime = DateTime.Now.ToUniversalTime();
                    CurrentModel.Creator = ApplicationContext.Instance.AuthenticationInfo.Account;
                    Add();

                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        protected override void Return()
        {
            base.Return();
            EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.DriverInfoV, new System.Collections.Generic.Dictionary<string, object>() { { "action", "return" }, { "model", CurrentModel } }));
        }


        protected void Add()
        {
            ChauffeurServiceClient client = InilitizeViewModel();
            client.AddChauffeurAsync(CurrentModel);
        }

        protected void Update()
        {
            ChauffeurServiceClient client = InilitizeViewModel();
            client.UpdateChauffeurAsync(CurrentModel);
        }


        private void client_AddChauffeurCompleted(object sender, AddChauffeurCompletedEventArgs e)
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
                //InilitizeViewModel();
                ApplicationContext.Instance.Logger.LogException("DriverInfoDetailViewModel.client_AddDriverInfoCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        private static void CloseClient(object sender)
        {
            ChauffeurServiceClient client = sender as ChauffeurServiceClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        private void client_UpdateChauffeurCompleted(object sender, UpdateChauffeurCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("DriverInfoDetailViewModel.client_AddDriverInfoCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }


        #region property check

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
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Name))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }
        private string _iCardID;
        public string ICardID
        {
            get { return _iCardID; }
            set
            {
                _iCardID = value == null ? null : value.Trim();
                ValidateDistrictCode(ExtractPropertyName(() => ICardID), _iCardID);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ICardID));
            }
        }
        private void ValidateDistrictCode(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(ICardID))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value == null ? null : value.Trim();
                ValidateAddress(ExtractPropertyName(() => Address), _address);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Address));
            }
        }
        private void ValidateAddress(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Address))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }

        private string _driverLicense;
        public string DriverLicense
        {
            get { return _driverLicense; }
            set
            {
                _driverLicense = value == null ? null : value.Trim();
                ValidateDirector(ExtractPropertyName(() => DriverLicense), _driverLicense);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DriverLicense));
            }
        }
        private void ValidateDirector(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(DriverLicense))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value == null ? null : value.Trim();
                ValidateTelePhone(ExtractPropertyName(() => Phone), _phone);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Phone));
            }
        }
        private void ValidateTelePhone(string prop, string value)
        {
            ClearErrors(prop);


            if (string.IsNullOrEmpty(value))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
            else
            {
                if (value.Length > 12)
                {
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PhoneOverLength"));
                }
                else
                {
                    long result = 0;

                    if (!long.TryParse(value, out result))
                    {
                        base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
                    }
                }
            }

        }
        private string _cellphone;
        public string CellPhone
        {
            get { return _cellphone; }
            set
            {
                _cellphone = value == null ? null : value.Trim();
                ValidateContact(ExtractPropertyName(() => CellPhone), _cellphone);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CellPhone));
            }
        }
        private void ValidateContact(string prop, string value)
        {
            ClearErrors(prop);

            if (string.IsNullOrEmpty(value))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
            else
            {
                if (value.Length > 12)
                {
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
                }
                else
                {
                    long result = 0;

                    if (!long.TryParse(value, out result))
                    {
                        base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
                    }
                }
            }
        }


        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value == null ? null : value.Trim();
                ValidateEmail(ExtractPropertyName(() => Email), _email);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Email));
            }
        }
        private void ValidateEmail(string prop, string value)
        {
            ClearErrors(prop);

            if (string.IsNullOrEmpty(value))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
           
            if (!string.IsNullOrEmpty(value) && !Regex.IsMatch(value, "\\w{1,}@\\w{1,}\\.\\w{1,}", RegexOptions.IgnoreCase))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"));//格式非法
            }
        }

        private string _note;
        public string Note
        {
            get { return _note; }
            set
            {
                _note = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Note));
            }
        }

        #endregion

        #region Button Visible
        public Visibility saveButtonVisibility;
        /// <summary>
        /// 
        /// </summary>
        public Visibility SaveButtonVisibility
        {
            get
            {
                return this.saveButtonVisibility;
            }
            set
            {
                this.saveButtonVisibility = value;
                RaisePropertyChanged(() => this.SaveButtonVisibility);
            }
        }

        public Visibility resertButtonVisibility;
        /// <summary>
        /// 
        /// </summary>
        public Visibility ResertButtonVisibility
        {
            get
            {
                return this.saveButtonVisibility;
            }
            set
            {
                this.resertButtonVisibility = value;
                RaisePropertyChanged(() => this.ResertButtonVisibility);
            }
        }

        #endregion
    }
}
