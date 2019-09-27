using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BaseInformation;
//using Gsafety.PTMS.BaseInformation.Views;
using Gsafety.PTMS.ServiceReference.ChauffeurService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.ObjectModel;
using System.Reflection;


namespace Gsafety.Ant.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.DriverInfoBindingVm)]
    public class DriverInfoBindingViewModel : ListViewModel<VVehicle>
    {
        private string _title;
        /// <summary>
        /// 界面标题
        /// </summary>
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        private string _vehicleNum = string.Empty;
        public string VehicleNum
        {
            get
            {
                return this._vehicleNum;
            }
            set
            {
                this._vehicleNum = value;

                RaisePropertyChanged(() => VehicleNum);
            }
        }

        ObservableCollection<ChauffeurVehicle> chauffeurVehicles;

        Chauffeur ChauffeurModel;
        public event EventHandler<SaveResultArgs> OnSaveResult;


        /// <summary>
        /// initil
        /// </summary>        
        public DriverInfoBindingViewModel()
        {
            this.Title = "";
            this.Title = ApplicationContext.Instance.StringResourceReader.GetString("BindingTitle");

            ReturnCommand = new ActionCommand<object>(obj => Return());
        }

        /// <summary>
        /// 初始化驾驶员服务客户端
        /// </summary>
        /// <returns></returns>
        private ChauffeurServiceClient InitializeChauffeurServiceClient()
        {
            ChauffeurServiceClient chauffeurClient = null;
            chauffeurClient = ServiceClientFactory.Create<ChauffeurServiceClient>();
            chauffeurClient.GetChauffeurVehicleCompleted += chauffeurClient_GetChauffeurVehicleCompleted;
            chauffeurClient.SaveChauffeurVehicleCompleted += chauffeurClient_SaveChauffeurVehicleCompleted;
            return chauffeurClient;
        }

        /// <summary>
        /// 初始化车辆服务服务客户端
        /// </summary>
        /// <returns></returns>
        private VehicleServiceClient InitializeVehicleServiceClient()
        {
            VehicleServiceClient vehicleClient = null;
            vehicleClient = ServiceClientFactory.Create<VehicleServiceClient>();
            ServiceClientFactory.CreateMessageHeader(vehicleClient.InnerChannel);
            vehicleClient.GetChauffeurVehiclePageListCompleted += vehicleClient_GetChauffeurVehiclePageListCompleted;
            return vehicleClient;
        }


        /// <summary>
        /// 保存添加
        /// </summary>
        /// <param name="name"></param>
        protected override void Add(string name)
        {
            try
            {
                ObservableCollection<ChauffeurVehicle> checkechauffeurVehicle = new ObservableCollection<ChauffeurVehicle>();

                foreach (VVehicle u in Data)
                {
                    if (u.IsChecked == true)
                    {
                        ChauffeurVehicle cv = new ChauffeurVehicle();
                        cv.ID = Guid.NewGuid().ToString();
                        cv.ChauffeurID = ChauffeurModel.ID;
                        cv.VehicleID = u.Vehicles.VehicleId;
                        cv.CreateTime = DateTime.Now.ToUniversalTime();
                        cv.Activate = 1;
                        checkechauffeurVehicle.Add(cv);
                    }
                    else
                    {
                        ChauffeurVehicle cv = new ChauffeurVehicle();
                        cv.ID = null;
                        cv.ChauffeurID = ChauffeurModel.ID;
                        cv.VehicleID = u.Vehicles.VehicleId;
                        cv.CreateTime = DateTime.Now.ToUniversalTime();
                        cv.Activate = 1;
                        //chauffeurClient.SaveChauffeurVehicleAsync(cv);
                        checkechauffeurVehicle.Add(cv);

                    }
                }
                ChauffeurServiceClient chauffeurClient = this.InitializeChauffeurServiceClient();
                chauffeurClient.SaveChauffeurVehicleAsync(checkechauffeurVehicle);

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void Query()
        {
            try
            {
                currentIndex = 1;
                Data.MoveToFirstPage();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {
            try
            {
                Data = new BaseLib.Model.PagedServerCollection<VVehicle>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                    Gsafety.PTMS.ServiceReference.VehicleService.PagingInfo page = new Gsafety.PTMS.ServiceReference.VehicleService.PagingInfo();
                    page.PageIndex = pageIndex;
                    page.PageSize = pageSize;
                    VehicleServiceClient vehicleClient = this.InitializeVehicleServiceClient();
                    vehicleClient.GetChauffeurVehiclePageListAsync(page, ApplicationContext.Instance.AuthenticationInfo.ClientID, VehicleNum);
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }


        public new void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                string action = viewParameters["action"].ToString();
                ChauffeurModel = viewParameters["model"] as Chauffeur;
                ChauffeurServiceClient chauffeurClient = this.InitializeChauffeurServiceClient();
                chauffeurClient.GetChauffeurVehicleAsync(ChauffeurModel.ID, ApplicationContext.Instance.AuthenticationInfo.ClientID);

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.DriverInfoV, new System.Collections.Generic.Dictionary<string, object>() { { "action", "return" }, { "model", ChauffeurModel } }));
        }

        private void chauffeurClient_SaveChauffeurVehicleCompleted(object sender, SaveChauffeurVehicleCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.client_AddInstallStationCompleted", ex);
            }
            finally
            {
                ChauffeurServiceClient client = sender as ChauffeurServiceClient;
                this.CloseChauffeurClient(client);
            }
        }

        /// <summary>
        /// 关闭驾驶员服务连接
        /// </summary>
        /// <param name="client"></param>
        private void CloseChauffeurClient(ChauffeurServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }


        private void vehicleClient_GetChauffeurVehiclePageListCompleted(object sender, PTMS.ServiceReference.VehicleService.GetChauffeurVehiclePageListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    Data.loader_Finished(new BaseLib.Model.PagedResult<VVehicle>()
                    {
                        Count = e.Result.TotalRecord,
                        Items = e.Result.Result,//数据列表
                        PageIndex = currentIndex
                    });
                    if (chauffeurVehicles != null)
                    {
                        foreach (VVehicle vehicle in Data)
                        {
                            //VVehicle g = new VVehicle();
                            //g.Vehicles = vehicle;

                            foreach (var cvs in chauffeurVehicles)
                            {
                                if (cvs.VehicleID == vehicle.Vehicles.VehicleId) //已被绑定驾驶员的汽车
                                {
                                    vehicle.IsChecked = true;
                                }
                            }
                            //AllVehicles.Add(g);
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
                ApplicationContext.Instance.Logger.LogException("vehicleClient_GetChauffeurVehiclePageListCompleted", ex);
            }
            finally
            {
                VehicleServiceClient client = sender as VehicleServiceClient;
                this.CloseVehicleServiceClient(client);
            }
        }

        private void CloseVehicleServiceClient(VehicleServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }


        private void chauffeurClient_GetChauffeurVehicleCompleted(object sender, PTMS.ServiceReference.ChauffeurService.GetChauffeurVehicleCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        chauffeurVehicles = e.Result.Result;

                        foreach (VVehicle vehicle in Data)
                        {
                            //VVehicle g = new VVehicle();
                            //g.Vehicles = vehicle;

                            foreach (var cvs in chauffeurVehicles)
                            {
                                if (cvs.VehicleID == vehicle.Vehicles.VehicleId) //已被绑定驾驶员的汽车
                                {
                                    vehicle.IsChecked = true;
                                }
                            }
                            //AllVehicles.Add(g);
                        }
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("chauffeurClient_GetChauffeurVehicleCompleted", ex);
            }
            finally
            {
                ChauffeurServiceClient client = sender as ChauffeurServiceClient;
                this.CloseChauffeurClient(client);
            }

        }



        private ObservableCollection<VVehicle> allVehicles;

        public ObservableCollection<VVehicle> AllVehicles
        {
            get { return allVehicles; }
            set
            {
                allVehicles = value;
                RaisePropertyChanged(() => this.AllVehicles);
            }
        }

        public IActionCommand ReturnCommand { get; protected set; }

    }

}
