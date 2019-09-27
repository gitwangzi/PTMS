using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.ChauffeurService;
using Gsafety.PTMS.Share;
using System;
using System.Reflection;

namespace Gsafety.Ant.BaseInformation.ViewModels.OrganizationViewModel
{
    public class VehicleBindingDriverViewModel : ListViewModel<Chauffeur>
    {
        private string chuffeurNameH;
        /// <summary>
        /// 
        /// </summary>
        public string ChuffeurNameH
        {
            get
            {
                return chuffeurNameH;
            }
            set
            {
                this.chuffeurNameH = value;
                RaisePropertyChanged(() => this.ChuffeurNameH);
            }
        }


        private string driverLicenseH;
        /// <summary>
        /// 
        /// </summary>
        public string DriverLicenseH
        {
            get
            {
                return driverLicenseH;
            }
            set
            {
                this.driverLicenseH = value;
                RaisePropertyChanged(() => this.DriverLicenseH);
            }
        }

        private string iCardIDH;
        /// <summary>
        /// 
        /// </summary>
        public string ICardIDH
        {
            get
            {
                return iCardIDH;
            }
            set
            {
                this.iCardIDH = value;
                RaisePropertyChanged(() => this.ICardIDH);
            }
        }

        private string phoneH;
        /// <summary>
        /// 
        /// </summary>
        public string PhoneH
        {
            get
            {
                return phoneH;
            }
            set
            {
                this.phoneH = value;
                RaisePropertyChanged(() => this.PhoneH);
            }
        }

        private string emailH;
        /// <summary>
        /// 
        /// </summary>
        public string EmailH
        {
            get
            {
                return emailH;
            }
            set
            {
                this.emailH = value;
                RaisePropertyChanged(() => this.EmailH);
            }
        }

        private string addressH;
        /// <summary>
        /// 
        /// </summary>
        public string AddressH
        {
            get
            {
                return addressH;
            }
            set
            {
                this.addressH = value;
                RaisePropertyChanged(() => this.AddressH);
            }
        }

        private string operatorH;
        /// <summary>
        /// 
        /// </summary>
        public string OperatorH
        {
            get
            {
                return operatorH;
            }
            set
            {
                this.operatorH = value;
                RaisePropertyChanged(() => this.OperatorH);
            }
        }

        public string VehicleId = null;
        public VehicleBindingDriverViewModel(string vehicleId)
            : base()
        {
            VehicleId = vehicleId;
            Query();
        }



        /// <summary>
        /// 初始化服务客户端方法
        /// </summary>
        /// <returns></returns>
        private ChauffeurServiceClient InitialClient()
        {
            ChauffeurServiceClient client = ServiceClientFactory.Create<ChauffeurServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);

            client.GetChauffeurByVehicleCompleted += client_GetChauffeurByVehicleCompleted;

            return client;
        }

        /// <summary>
        /// 是否服务客户端
        /// </summary>
        /// <param name="client"></param>
        private void CloseClient(ChauffeurServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        /// <summary>
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {
            try
            {
                Data = new BaseLib.Model.PagedServerCollection<Chauffeur>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                    ChauffeurServiceClient client = InitialClient();
                    client.GetChauffeurByVehicleAsync(VehicleId, ApplicationContext.Instance.AuthenticationInfo.ClientID);
                });

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        void client_GetChauffeurByVehicleCompleted(object sender, GetChauffeurByVehicleCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        Data.loader_Finished(new BaseLib.Model.PagedResult<Chauffeur>()
                        {
                            Count = e.Result.TotalRecord,
                            Items = e.Result.Result,//数据列表
                            PageIndex = currentIndex
                        });
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
                ApplicationContext.Instance.Logger.LogException("VehicleBindingDriverViewModel", ex);
            }
            finally
            {
                ChauffeurServiceClient client = sender as ChauffeurServiceClient;
                CloseClient(client);
            }
        }

    }
}
