using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.DistrictService;
using Gsafety.PTMS.ServiceReference.InstallStationService;
using Gsafety.PTMS.Share;
using Jounce.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Gsafety.Ant.BaseInformation.ViewModels
{
    public class InstallPlaceDetailViewModel : DetailViewModel<InstallStation>
    {
        public event EventHandler<SaveResultArgs> OnSaveResult;

        public InstallPlaceDetailViewModel()
        {
            InitProvinces();
        }

        private InstallStationServiceClient InitialClient()
        {
            InstallStationServiceClient client = ServiceClientFactory.Create<InstallStationServiceClient>();
            client.AddInstallStationCompleted += client_AddInstallStationCompleted;
            client.UpdateInstallStationCompleted += client_UpdateInstallStationCompleted;

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
                        IsEnable = false;
                        SaveButtonVisibility = Visibility.Collapsed;
                        ResertButtonVisibility = Visibility.Collapsed;

                        ViewVisibility = Visibility.Collapsed;
                        InitialModel = viewParameters["model"] as InstallStation;
                        InitialFromInitialModel();

                        break;
                    case "update":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                        IsReadOnly = false;
                        IsEnable = true;
                        ViewVisibility = Visibility.Visible;
                        SaveButtonVisibility = Visibility.Visible;
                        ResertButtonVisibility = Visibility.Visible;

                        InitialModel = viewParameters["model"] as InstallStation;
                        InitialFromInitialModel();
                        CurrentModel = new InstallStation();
                        break;
                    case "add":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                        IsReadOnly = false;
                        IsEnable = true;
                        ViewVisibility = Visibility.Visible;
                        SaveButtonVisibility = Visibility.Visible;
                        ResertButtonVisibility = Visibility.Visible;
                        CurrentModel = new InstallStation();
                        Reset();

                        Province = Provinces.FirstOrDefault();
                        City = Cities.FirstOrDefault();

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
                if (action == "update")
                {
                    InitialFromInitialModel();
                }
                else
                {
                    Name = string.Empty;
                    Address = string.Empty;
                    Director = string.Empty;
                    DirectorPhone = string.Empty;
                    Contact = string.Empty;
                    ContactPhone = string.Empty;
                    Email = string.Empty;
                    Note = string.Empty;
                    Province = Provinces[0];
                    City = Cities[0];
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public void InitialFromInitialModel()
        {
            try
            {
                if (InitialModel.DistrictCode.Length == 2)
                {
                    Province = Provinces.FirstOrDefault(x => x.Code == InitialModel.DistrictCode);
                }

                Name = InitialModel.Name;
                Address = InitialModel.Address;
                Director = InitialModel.Director;
                DirectorPhone = InitialModel.DirectorPhone;
                Contact = InitialModel.Contact;
                ContactPhone = InitialModel.ContactPhone;
                Email = InitialModel.Email;
                Note = InitialModel.Note;

                if (InitialModel.DistrictCode.Length == 5)
                {
                    Province = Provinces.FirstOrDefault(x => x.Code == InitialModel.DistrictCode.Substring(0, 2));
                    JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Province));

                    City = Cities.FirstOrDefault(x => x.Code == InitialModel.DistrictCode);
                    JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => City));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void ValidateAll()
        {
            ValidateName(ExtractPropertyName(() => Name), _name);
            ValidateAddress(ExtractPropertyName(() => Address), _address);
            ValidateDirector(ExtractPropertyName(() => Director), _director);
            ValidateDirectorPhone(ExtractPropertyName(() => DirectorPhone), _directorphone);
            ValidateContactPhone(ExtractPropertyName(() => ContactPhone), _contactphone);
            ValidateEmail(ExtractPropertyName(() => Email), _email);

        }
        protected override void OnCommitted()
        {
            try
            {
                CurrentModel.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                CurrentModel.Name = Name;
                CurrentModel.DistrictCode = City.Code;
                CurrentModel.Address = Address;
                CurrentModel.Director = Director;
                CurrentModel.DirectorPhone = DirectorPhone;
                CurrentModel.Contact = Contact;
                CurrentModel.ContactPhone = ContactPhone;
                CurrentModel.Email = Email;
                CurrentModel.Note = Note;

                if (action.Equals("update"))
                {
                    if (CurrentModel.Name != InitialModel.Name ||
                        CurrentModel.DistrictCode != InitialModel.DistrictCode ||
                        CurrentModel.Address != InitialModel.Address ||
                        CurrentModel.Director != InitialModel.Director ||
                        CurrentModel.DirectorPhone != InitialModel.DirectorPhone ||
                        CurrentModel.Contact != InitialModel.Contact ||
                        CurrentModel.ContactPhone != InitialModel.ContactPhone ||
                        CurrentModel.Email != InitialModel.Email ||
                        CurrentModel.Note != InitialModel.Note)
                    {

                        CurrentModel.ID = InitialModel.ID;
                        CurrentModel.Valid = InitialModel.Valid;
                        CurrentModel.CreateTime = InitialModel.CreateTime.ToUniversalTime();
                        Update();
                    }
                    else
                    {
                        OnSaveResult(this, new SaveResultArgs() { Result = true });
                    }
                }
                else
                {
                    CurrentModel.ID = Guid.NewGuid().ToString();
                    CurrentModel.Valid = 1;
                    CurrentModel.CreateTime = DateTime.Now.ToUniversalTime();
                    Add();

                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected void Add()
        {
            InstallStationServiceClient client = InitialClient();
            client.AddInstallStationAsync(CurrentModel);
        }

        protected void Update()
        {
            InstallStationServiceClient client = InitialClient();
            client.UpdateInstallStationAsync(CurrentModel);
        }


        private void client_AddInstallStationCompleted(object sender, AddInstallStationCompletedEventArgs e)
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
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.client_AddInstallStationCompleted", ex);
            }
            finally
            {
                InstallStationServiceClient client = sender as InstallStationServiceClient;
                CloseClient(client);
            }
        }

        private void CloseClient(InstallStationServiceClient client)
        {
            if(client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_UpdateInstallStationCompleted(object sender, UpdateInstallStationCompletedEventArgs e)
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
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.client_UpdateInstallStationCompleted", ex);
            }
            finally
            {
                InstallStationServiceClient client = sender as InstallStationServiceClient;
                CloseClient(client);
            }
        }

        #region ci pro

        protected void InitProvinces()
        {
            var _provices = ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 2).ToList().OrderBy(x => x.Name);
            foreach(var item in _provices)
            {
                Provinces.Add(item);
            }
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Provinces));
        }

        protected void InitCities(string provinceCode)
        {
            if(provinceCode != null)
            {
                City = null;
                Cities.Clear();
                var _cities = ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 5 && x.Code.StartsWith(provinceCode)).ToList().OrderBy(x => x.Name);
                foreach(var item in _cities)
                {
                    Cities.Add(item);
                }
                JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Cities));

                if (cities.Count != 0)
                {
                    City = cities[0];
                }
            }
        }

        private District province;
        /// <summary>
        /// 省级区划
        /// </summary>
        public District Province
        {
            get { return province; }
            set
            {
                if (province != value)
                {
                    province = value;
                    RaisePropertyChanged(() => Province);

                    if (province != null)
                    {
                        InitCities(province.Code);
                        City = City;
                    }
                }
            }
        }

        private ObservableCollection<District> provinces = new ObservableCollection<District>();
        /// <summary>
        /// 所有省份
        /// </summary>
        public ObservableCollection<District> Provinces
        {
            get { return provinces; }
            set
            {
                provinces = value;
                RaisePropertyChanged(() => Provinces);
            }
        }


        private District city;
        /// <summary>
        /// 市级区划
        /// </summary>
        public District City
        {
            get { return city; }
            set
            {
                city = value;
                try
                {
                    if(City == null)
                    {
                        base.SetError(ExtractPropertyName(() => City), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
                        RaisePropertyChanged(() => City);
                    }
                    else
                    {
                        ClearErrors(ExtractPropertyName(() => City));
                        RaisePropertyChanged(() => City);
                    }
                }
                catch(Exception ex)
                {
                    ApplicationContext.Instance.Logger.LogException("CityProperty", ex);
                }
            }
        }

        private void ValidateCity(string prop, string value)
        {
            ValidateRequire(prop, value);
        }

        private ObservableCollection<District> cities = new ObservableCollection<District>();
        /// <summary>
        /// 所有市列表
        /// </summary>
        public ObservableCollection<District> Cities
        {
            get { return cities; }
            set
            {
                cities = value;
                RaisePropertyChanged(() => Cities);
            }
        }
        #endregion


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
            ValidateRequire(prop, value);
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
            //ValidateRequire(prop, value);
        }

        private string _director;
        public string Director
        {
            get { return _director; }
            set
            {
                _director = value == null ? null : value.Trim();
                ValidateDirector(ExtractPropertyName(() => Director), _director);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Director));
            }
        }

        private void ValidateDirector(string prop, string value)
        {
            //ClearErrors(prop);
            //if (string.IsNullOrEmpty(Director))
            //{
            //    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            //}
        }

        private string _directorphone;
        public string DirectorPhone
        {
            get { return _directorphone; }
            set
            {
                _directorphone = value == null ? null : value.Trim();
                ValidateDirectorPhone(ExtractPropertyName(() => DirectorPhone), _directorphone);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DirectorPhone));
            }
        }

        private void ValidateDirectorPhone(string prop, string value)
        {
            ClearErrors(prop);

            if(!string.IsNullOrEmpty(value))
            {
                if(value.Length > 12)
                {
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
                }
                else
                {
                    long result = 0;

                    if(!long.TryParse(value, out result))
                    {
                        base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
                    }
                }
            }
        }

        private string _contact;
        public string Contact
        {
            get { return _contact; }
            set
            {
                _contact = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Contact));
            }
        }

        private string _contactphone;
        public string ContactPhone
        {
            get { return _contactphone; }
            set
            {
                _contactphone = value == null ? null : value.Trim();
                ValidateContactPhone(ExtractPropertyName(() => ContactPhone), _contactphone);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ContactPhone));
            }
        }

        private void ValidateContactPhone(string prop, string value)
        {
            ClearErrors(prop);
            if(!string.IsNullOrEmpty(value))
            {
                if(value.Length > 12)
                {
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PhoneOverLength"));
                }
                else
                {
                    long result = 0;

                    if(!long.TryParse(value, out result))
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
            base.ValidateEmailFormat(prop, value);
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

