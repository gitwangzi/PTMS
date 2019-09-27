using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.Share;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Jounce.Framework.Command;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media.Imaging;
using System.Linq;
using Gsafety.Ant.Monitor.Views;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Gsafety.PTMS.ManagerManagement.ViewModels
{
    //[ExportAsViewModel(BaseInformationName.VehicleTypeDetailViewVm)]
    public class VehicleTypeDetailViewModel : DetailViewModel<VehicleType>
    {


        public ICommand AddImageCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public string AddId { get; set; }
        //VehicleTypeClient client = null;
        public event EventHandler<SaveResultArgs> OnSaveResult;

        public VehicleTypeDetailViewModel()
        {
            this.InilizeClient();
            AddImageCommand = new ActionCommand<object>(method => AddImage());
            AddCommand = new ActionCommand<object>(method => AddColor());
            EditCommand = new ActionCommand<object>(method => UpdateColor());
            DeleteCommand = new ActionCommand<object>(method => DeleteColor());
            AddId = Guid.NewGuid().ToString();

            //ResetCommand = new ActionCommand<object>(method => Clear());
        }

        BitmapImage _ImageSource = new BitmapImage(new Uri("/ExternalResource;component/Images/onLineTaxi.png", UriKind.RelativeOrAbsolute));
        public BitmapImage ImageSource
        {
            get
            {
                return _ImageSource;
            }
            set
            {
                _ImageSource = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ImageSource));
            }
        }

        private ObservableCollection<VehicleTypeColor> _speedcolorlist = new ObservableCollection<VehicleTypeColor>();
        public ObservableCollection<VehicleTypeColor> SpeedColorList
        {
            get { return _speedcolorlist; }
            set
            {
                _speedcolorlist = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SpeedColorList));
            }
        }

        private VehicleTypeColor _speedcolor;
        public VehicleTypeColor SpeedColor
        {
            get { return _speedcolor; }
            set
            {
                _speedcolor = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SpeedColor));
            }
        }

        public void AddColor()
        {

            
            SpeedColorData item = new SpeedColorData()
            {               
                ID = Guid.NewGuid().ToString(),
                MinSpeed = 0,
                MaxSpeed = 0,
                FillColorParm =Colors.Red
               
            };
            SpeedColorEdit mge = new SpeedColorEdit();
            mge.Edit(item);
            mge.Closed += (m, n) =>
            {
                if (mge.DialogResult == true)
                {
                   
                    string color = Convert.ToString(mge.EditSpeedColorData.FillColorParm);
                    VehicleTypeColor data = new VehicleTypeColor();
                    data.ID = mge.EditSpeedColorData.ID;
                    data.TypeName = Name;
                    data.MaxSpeed = mge.EditSpeedColorData.MaxSpeed;
                    data.MinSpeed = mge.EditSpeedColorData.MinSpeed;
                    data.Color = color;

                  
                        if (action == "update")
                        {
                            VehicleTypeClient client = InilizeClient();
                            try
                            {
                                data.TypeID = InitialModel.ID;
                                ObservableCollection<VehicleTypeColor> colorlist = new ObservableCollection<VehicleTypeColor>();

                                colorlist.Add(data);
                               
                                client.InsertVehicleTypeColorCompleted += ((nsender, ne) =>
                                {
                                    if (!SpeedColorList.Contains(data))
                                    {
                                        SpeedColorList.Add(data);
                                        SpeedColor = null;

                                        ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.SpeedColorList.Add(data);                                        

                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SpeedColor));
                                        SpeedColor = SpeedColorList[0];
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SpeedColor));
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SpeedColorList));
                                    }

                                });
                                client.InsertVehicleTypeColorAsync(colorlist);
                            }
                            catch (Exception ex)
                            {

                                ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.client_AddInstallStationCompleted", ex);
                            }
                            finally
                            {                             
                              
                                if (client != null)
                                {
                                    client.CloseAsync();
                                    client = null;
                                }
                            }


                    }
                    if (action == "add")
                    {
                        data.TypeID = AddId;
                        SpeedColorList.Add(data);
                        SpeedColor = null;

                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SpeedColor));
                        SpeedColor = SpeedColorList[0];
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SpeedColor));
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SpeedColorList));
                    }

                    

                }
            };

            
        
        
        }

        public Color ToColor(string code)
        {
            
            if (!code.StartsWith("#"))
            {
                return Colors.Red;
            }
            else
            {
                code = code.Replace("#", string.Empty);
                int v = int.Parse(code, System.Globalization.NumberStyles.HexNumber);
                return new Color()
                {
                    A = Convert.ToByte((v>>24)&255),
                    R = Convert.ToByte((v>>16)&255),
                    G = Convert.ToByte((v>>8)&255),
                    B = Convert.ToByte((v>>0)&255)
                
                
                };
                
            
            }
            return Colors.Red;
        }

        public void UpdateColor()
        {
            if (SpeedColor != null)
            {

                

                SpeedColorData item = new SpeedColorData()
                {
                    ID = SpeedColor.ID,
                    MinSpeed = SpeedColor.MinSpeed,
                    MaxSpeed = SpeedColor.MaxSpeed,
                    FillColorParm = ToColor(SpeedColor.Color)

                };
                SpeedColorEdit mge = new SpeedColorEdit();
                mge.Edit(item);
                mge.Closed += (m, n) =>
                {
                    if (mge.DialogResult == true)
                    {
                        if (action == "update")
                        {
                            VehicleTypeClient client = InilizeClient();
                            try
                            {
                                string color = Convert.ToString(mge.EditSpeedColorData.FillColorParm);
                                VehicleTypeColor data = new VehicleTypeColor();
                                data.ID = mge.EditSpeedColorData.ID;
                                data.TypeID = SpeedColor.TypeID;
                                data.MaxSpeed = mge.EditSpeedColorData.MaxSpeed;
                                data.MinSpeed = mge.EditSpeedColorData.MinSpeed;
                                data.Color = color;

                                client.UpdateVehicleTypeColorCompleted += ((nsender, ne) =>
                                    {
                                        SpeedColor.MaxSpeed = mge.EditSpeedColorData.MaxSpeed;
                                        SpeedColor.MinSpeed = mge.EditSpeedColorData.MinSpeed;
                                        SpeedColor.Color = color;
                                        SpeedColor.TypeName = Name;

                                        var oldlist = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.SpeedColorList.Where(x => x.ID == SpeedColor.ID).FirstOrDefault();

                                        if (oldlist != null)
                                        {
                                            ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.SpeedColorList.Remove(oldlist);
                                            ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.SpeedColorList.Add(SpeedColor);

                                        
                                        }
                                      
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SpeedColor));
                                    });
                                client.UpdateVehicleTypeColorAsync(data);
                            }
                            catch (Exception ex)
                            {

                                ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.client_AddInstallStationCompleted", ex);
                            }
                            finally
                            {

                                if (client != null)
                                {
                                    client.CloseAsync();
                                    client = null;
                                }
                            }
                        }

                        if (action == "add")
                        {
                            string color = Convert.ToString(mge.EditSpeedColorData.FillColorParm);


                            SpeedColor.MaxSpeed = mge.EditSpeedColorData.MaxSpeed;
                            SpeedColor.MinSpeed = mge.EditSpeedColorData.MinSpeed;
                            SpeedColor.Color = color;


                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SpeedColor));
                        }
                    }
                };
            }



        }


        public void DeleteColor()
        {
            if (SpeedColor != null)
            {
                string Caption = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption);

                ChildWindow result;

                result = (SelfMessageBox)MessageBoxHelper.ShowDialog(Caption, ApplicationContext.Instance.StringResourceReader.GetString("IsDelete"), MessageDialogButton.OkAndCancel);

                result.Closed += (a, b) =>
                {
                    if (result.DialogResult == true)
                    {
                        if (action == "update")
                        {
                            VehicleTypeClient client = InilizeClient();
                            try
                            {
                                client.DeleteVehicleTypeColorCompleted += ((nsender, ne) =>
                                {

                                    SpeedColorList.Remove(SpeedColor);
                                    SpeedColor = null;
                                    if (SpeedColorList.Count > 0)
                                    {
                                        SpeedColor = SpeedColorList[0];
                                    }


                                    ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.SpeedColorList.Remove(SpeedColor);

                                    
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SpeedColor));
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SpeedColorList));

                                });
                                client.DeleteVehicleTypeColorAsync(SpeedColor.ID);
                            }
                            catch (Exception ex)
                            {

                                ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.client_AddInstallStationCompleted", ex);
                            }
                            finally
                            {

                                if (client != null)
                                {
                                    client.CloseAsync();
                                    client = null;
                                }
                            }
                        }
                        if (action == "add")
                        {
                            SpeedColorList.Remove(SpeedColor);
                            SpeedColor = null;
                            if (SpeedColorList.Count > 0)
                            {
                                SpeedColor = SpeedColorList[0];
                            }
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SpeedColor));
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SpeedColorList));
                        
                        }
                    }
                };
            }



        }


        public string Base64Image = null;

        ///

        public void AddImage()
        {
            System.IO.FileInfo fileInfo;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = " Files(*.png,*.jpg,*.jpeg)|*.png;*.jpg;*.jpeg|All files(*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {

                fileInfo = openFileDialog.File;
                if (!fileInfo.Name.EndsWith("png") && !fileInfo.Name.EndsWith("jpg") && !fileInfo.Name.EndsWith("jpeg"))
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), MessageDialogButton.Ok);
                    return;
                }

                FileStream fs = null;
                try
                {
                    using ( fs = new FileStream(fileInfo.FullName, FileMode.Open))
                    {
                       
                        byte[] data = new byte[(int)fs.Length];

                        fs.Read(data, 0, data.Length);

                        Base64Image = Convert.ToBase64String(data);
                     
                        BitmapImage image = new BitmapImage();

                        image.SetSource(fs);

                        ImageSource = image;                        
                       
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ImageSource));
                    }
                }
                catch
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_CloseFile"), MessageDialogButton.Ok);
                    return;
                }

                try
                {
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
             
            }

        }

        //public static string ImageToBase64(string path)
        //{

        //    try 
        //    {
        //        BitmapImage image = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
               
        //    }
        //    catch(Exception ex)
        //    {
        //        return null;
        //    }
        
        //}
       
        private VehicleTypeClient InilizeClient()
        {
            VehicleTypeClient client = ServiceClientFactory.Create<VehicleTypeClient>();
            client.InsertVehicleTypeCompleted += client_InsertVehicleTypeCompleted;
            client.UpdateVehicleTypeCompleted += client_UpdateVehicleTypeCompleted;
            client.GetVehicleTypeColorListCompleted += client_GetVehicleTypeColorListCompleted;
            return client;
        }

        void client_GetVehicleTypeColorListCompleted(object sender, GetVehicleTypeColorListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        SpeedColorList = new ObservableCollection<VehicleTypeColor>(e.Result.Result);
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SpeedColorList));
                        if (SpeedColorList.Count() > 0)
                        {
                            SpeedColor = SpeedColorList[0];
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SpeedColor));
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
                ApplicationContext.Instance.Logger.LogException("client_GetByNameVehicleTypeListCompleted", ex);
            }
            finally
            {
                VehicleTypeClient client = sender as VehicleTypeClient;

                if (client != null)
                {
                    client.CloseAsync();
                    client = null;
                }
            }
          
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
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                        IsReadOnly = true;
                        SaveButtonVisibility = Visibility.Collapsed;
                        ResertButtonVisibility = Visibility.Collapsed;
                        ColorVisible = Visibility.Collapsed;

                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                        InitialModel = viewParameters["model"] as VehicleType;
                        this.ViewVehicleTypeInfo();
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Detail");
                        break;
                    case "update":
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                        IsReadOnly = false;
                        SaveButtonVisibility = Visibility.Visible;
                        ResertButtonVisibility = Visibility.Visible;
                        ColorVisible = Visibility.Visible;
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                        InitialModel = viewParameters["model"] as VehicleType;
                        CurrentModel = new VehicleType();
                        this.ViewVehicleTypeInfo();
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Edit");
                        break;
                    case "add":
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                        CurrentModel = new VehicleType();
                        IsReadOnly = false;
                        SaveButtonVisibility = Visibility.Visible;
                        ResertButtonVisibility = Visibility.Visible;
                        ColorVisible = Visibility.Visible;
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                        Clear();
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Add");
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

        private void Clear()
        {
            if (action == "add")
            {
                Name = string.Empty;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Name));
                Description = string.Empty;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Description));
                
            }
            else
            {
                ViewVehicleTypeInfo();
            }
        }

        protected override void Reset()
        {
            Clear();
        }

        //protected override void Return()
        //{
        //    EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.VehicleTypeManageViewV, new System.Collections.Generic.Dictionary<string, object>() { { "action", "return" }, { "model", CurrentModel } }));
        //}

        protected override void OnCommitted()
        {
            try
            {

                CurrentModel.Name = Name;
                CurrentModel.Description = Description;
                if (action.Equals("update"))
                {
                    CurrentModel.ID = InitialModel.ID;
                    CurrentModel.Valid = InitialModel.Valid;
                    CurrentModel.CreateTime = InitialModel.CreateTime;
                    CurrentModel.Creator = InitialModel.Creator;
                    CurrentModel.ClientID = InitialModel.ClientID;
                    CurrentModel.Image = Base64Image;
                    Update();
                    //Return();
                }
                else
                {
                    CurrentModel.ID = AddId;
                    CurrentModel.Valid = 1;
                    CurrentModel.CreateTime = DateTime.Now.ToUniversalTime();
                    CurrentModel.Creator = ApplicationContext.Instance.AuthenticationInfo.Account;
                    CurrentModel.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                    CurrentModel.Image = Base64Image;
                    Add();
                    //Return();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected void Add()
        {
            VehicleTypeClient client = InilizeClient();
            client.InsertVehicleTypeAsync(CurrentModel);
            if (SpeedColorList.Count > 0)
            {
                client.InsertVehicleTypeColorAsync(SpeedColorList);
            }
        }
        protected void Update()
        {
            VehicleTypeClient client = InilizeClient();
            client.UpdateVehicleTypeAsync(CurrentModel);
        }

        private void client_InsertVehicleTypeCompleted(object sender, InsertVehicleTypeCompletedEventArgs e)
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

                        Clear();
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
                VehicleTypeClient client = sender as VehicleTypeClient;
          
                if (client != null)
                {
                    client.CloseAsync();
                    client = null;
                }
            }
        }


        void client_UpdateVehicleTypeCompleted(object sender, UpdateVehicleTypeCompletedEventArgs e)
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
                        Clear();
                    }
                    else
                    {
                        SaveResultArgs args = new SaveResultArgs();
                        args.Result = true;

                        if (OnSaveResult != null)
                        {
                            OnSaveResult(this, args);
                        }
                        foreach (var vehicleInfo in ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList)
                        {
                            if (vehicleInfo != null)
                            {
                                if (!string.IsNullOrEmpty(vehicleInfo.VehicleTypeDescribe))
                                {

                                    if (vehicleInfo.VehicleTypeDescribe == CurrentModel.Name)
                                    {
                                        vehicleInfo.VehicleTypeImage = CurrentModel.Image;
                                    }
                                }


                            }
                        }
                      
                    }
                }

            }
            catch (Exception ex)
            {
                this.InilizeClient();
                ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.client_AddInstallStationCompleted", ex);
            }
            finally
            {
                // client.AddInstallStationCompleted -= client_AddInstallStationCompleted;
                VehicleTypeClient client = sender as VehicleTypeClient;
                if (client != null)
                {
                    client.CloseAsync();
                    client = null;
                }
            }
        }

        public void ViewVehicleTypeInfo()
        {


            
            Name = InitialModel.Name;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Name));
            Description = InitialModel.Description;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Description));

            VehicleTypeClient client = InilizeClient();
            client.GetVehicleTypeColorListAsync(InitialModel.ID);
            if (InitialModel.Image != null)
            {

                byte[] data = Convert.FromBase64String(InitialModel.Image);

                BitmapImage image = new BitmapImage();

                image.SetSource(new MemoryStream(data));

                ImageSource = image;

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ImageSource));
            }
        }

      

        // public ICommand ResetCommand { get; set; }


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


        public Visibility colorVisible;
        /// <summary>
        /// 
        /// </summary>
        public Visibility ColorVisible
        {
            get
            {
                return this.colorVisible;
            }
            set
            {
                this.ColorVisible = value;
                RaisePropertyChanged(() => this.ColorVisible);
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


        #region property...

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value == null ? null : value.Trim();
                ValidateName(ExtractPropertyName(() => Name), _name);
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value == null ? null : value.Trim();
                //ValidateName(ExtractPropertyName(() => Description), _description);
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

        #endregion

    }
}

