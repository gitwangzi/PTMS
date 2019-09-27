using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Ant.Installation.ViewModels.Suite;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Installation;
using Gsafety.PTMS.ServiceReference.DeviceAlertService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using System.Collections.ObjectModel;

namespace Gsafety.Ant.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.DeviceAlertManageVm)]
    public class DeviceAlertManagerViewModel : ListViewModel<DeviceAlert>
    {
        ObservableCollection<string> stations = null;
        public DeviceAlertManagerViewModel()
            : base()
        {
            try
            {
                InitVehicleStatus();
                stations = new ObservableCollection<string>();
                foreach (var item in ApplicationContext.Instance.AuthenticationInfo.Stations)
                {
                    stations.Add(item.ID);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("DeviceAlertManagerViewModel()", ex);
            }
        }

        private DeviceAlertServiceClient InitialClient()
        {
            DeviceAlertServiceClient _client = ServiceClientFactory.Create<DeviceAlertServiceClient>();
            _client.GetDeviceAlertListCompleted += _client_GetDeviceAlertListCompleted;
            return _client;
        }

        private void _client_GetDeviceAlertListCompleted(object sender, GetDeviceAlertListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        List<DeviceAlert> items = new List<DeviceAlert>();
                        foreach (var item in e.Result.Result)
                        {
                            item.AlertTime = item.AlertTime.Value.ToLocalTime();
                            switch ((int)item.AlertType)
                            {
                                case -1:
                                    item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_Full_N");
                                    items.Add(item);
                                    break;
                                case 0:
                                    item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("GNSSModelError");
                                    items.Add(item);
                                    break;
                                case 1:
                                    item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("GNSSNoAntenna");
                                    items.Add(item);
                                    break;
                                case 2:
                                    item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("GNSSCircuit");
                                    items.Add(item);
                                    break;
                                case 3:
                                    item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("PowerSourceNoVoltage");
                                    items.Add(item);
                                    break;
                                case 4:
                                    item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("PowerSourceNoPower");
                                    items.Add(item);
                                    break;
                                case 5:
                                    item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("LEDError");
                                    items.Add(item);
                                    break;
                                case 6:
                                    item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("TTSError");
                                    items.Add(item);
                                    break;
                                case 7:
                                    item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("VidiconError");
                                    items.Add(item);
                                    break;
                                default:
                                    item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_Full_N");
                                    items.Add(item);
                                    break;
                            }
                        }
                        Data.loader_Finished(new PagedResult<DeviceAlert>
                        {
                            Count = items.Count,
                            Items = items,
                            PageIndex = currentIndex
                        });

                        if (e.Result.Result.Count != 0)
                        {
                            SelectedItem = e.Result.Result[0];
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
                ApplicationContext.Instance.Logger.LogException("_client_GetMaintainApplicationListCompleted()", ex);
            }
            finally
            {
                DeviceAlertServiceClient client = sender as DeviceAlertServiceClient;
                CloseClient(client);
            }
        }

        private void CloseClient(DeviceAlertServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        private DeviceAlert selecteditem;

        public DeviceAlert SelectedItem
        {
            get { return selecteditem; }
            set
            {
                if (selecteditem != value)
                {
                    selecteditem = value;
                }

                RaisePropertyChanged(() => this.SelectedItem);
            }
        }


        /// <summary>
        /// 查询
        /// </summary>
        protected override void Query()
        {
            currentIndex = 1;

            Data.MoveToFirstPage();
        }

        /// <summary>
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {
            try
            {
                if (EndTime.HasValue)
                {
                    if (EndTime.Value > DateTime.Now.Date.AddDays(1))
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Rpt_EndTimeError"));
                        return;
                    }
                }

                Data = new PagedServerCollection<DeviceAlert>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    int searchByAlertType = (int)SearchByAlertType.Key;
                    DeviceAlertServiceClient client = InitialClient();
                    if (searchByAlertType == -1)
                    {
                        //查询数据
                        client.GetDeviceAlertListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, SearchByVehicleID, null, StartTime.Value.ToUniversalTime(), EndTime.Value.ToUniversalTime(), stations, pageIndex, pageSize);
                    }
                    else
                    {
                        //查询数据
                        client.GetDeviceAlertListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, SearchByVehicleID, searchByAlertType, StartTime.Value.ToUniversalTime(), EndTime.Value.ToUniversalTime(), stations, pageIndex, pageSize);
                    }


                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagination()", ex);
            }
        }

        //int currentVehicleIndex = 0;


        void InitVehicleStatus()
        {
            try
            {
                ZAlertTypes.Add(new ComboBoxBasicStruct<DeviceAlertTypes>()
                {
                    Key = DeviceAlertTypes.MAINTAIN_Full_N,
                    Value = ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_Full_N")
                });
                ZAlertTypes.Add(new ComboBoxBasicStruct<DeviceAlertTypes>()
                {
                    Key = DeviceAlertTypes.GNSSModelError,
                    Value = ApplicationContext.Instance.StringResourceReader.GetString("GNSSModelError")
                });
                ZAlertTypes.Add(new ComboBoxBasicStruct<DeviceAlertTypes>()
                {
                    Key = DeviceAlertTypes.GNSSNoAntenna,
                    Value = ApplicationContext.Instance.StringResourceReader.GetString("GNSSNoAntenna")
                });
                ZAlertTypes.Add(new ComboBoxBasicStruct<DeviceAlertTypes>()
                {
                    Key = DeviceAlertTypes.GNSSCircuit,
                    Value = ApplicationContext.Instance.StringResourceReader.GetString("GNSSCircuit")
                });
                ZAlertTypes.Add(new ComboBoxBasicStruct<DeviceAlertTypes>()
                {
                    Key = DeviceAlertTypes.PowerSourceNoVoltage,
                    Value = ApplicationContext.Instance.StringResourceReader.GetString("PowerSourceNoVoltage")
                });
                ZAlertTypes.Add(new ComboBoxBasicStruct<DeviceAlertTypes>()
                {
                    Key = DeviceAlertTypes.PowerSourceNoPower,
                    Value = ApplicationContext.Instance.StringResourceReader.GetString("PowerSourceNoPower")
                });
                ZAlertTypes.Add(new ComboBoxBasicStruct<DeviceAlertTypes>()
                {
                    Key = DeviceAlertTypes.LEDError,
                    Value = ApplicationContext.Instance.StringResourceReader.GetString("LEDError")
                });
                ZAlertTypes.Add(new ComboBoxBasicStruct<DeviceAlertTypes>()
                {
                    Key = DeviceAlertTypes.TTSError,
                    Value = ApplicationContext.Instance.StringResourceReader.GetString("TTSError")
                });
                ZAlertTypes.Add(new ComboBoxBasicStruct<DeviceAlertTypes>()
                {
                    Key = DeviceAlertTypes.VidiconError,
                    Value = ApplicationContext.Instance.StringResourceReader.GetString("VidiconError")
                });

                SearchByAlertType = ZAlertTypes[0];
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitVehicleStatus", ex);
            }
        }

        private ComboBoxBasicStruct<DeviceAlertTypes> vAlertType;
        /// <summary>
        /// 选中的车辆情况
        /// </summary>
        public ComboBoxBasicStruct<DeviceAlertTypes> VAlertType
        {
            get { return vAlertType; }
            set
            {
                vAlertType = value;
                RaisePropertyChanged(() => VAlertType);
            }
        }
        private List<ComboBoxBasicStruct<DeviceAlertTypes>> _zAlertTypes = new List<ComboBoxBasicStruct<DeviceAlertTypes>>();
        /// <summary>
        /// 车辆情况
        /// </summary>
        public List<ComboBoxBasicStruct<DeviceAlertTypes>> ZAlertTypes
        {
            get { return _zAlertTypes; }
            set
            {
                _zAlertTypes = value;
                RaisePropertyChanged(() => ZAlertTypes);
            }
        }


        #region searchby....
        private ComboBoxBasicStruct<DeviceAlertTypes> searchByAlertType;
        /// <summary>
        /// 
        /// </summary>
        public ComboBoxBasicStruct<DeviceAlertTypes> SearchByAlertType
        {
            get
            {
                return searchByAlertType;
            }
            set
            {
                this.searchByAlertType = value;
                RaisePropertyChanged(() => this.SearchByAlertType);
            }
        }

        private string searchByVehicleID;
        /// <summary>
        /// 
        /// </summary>
        public string SearchByVehicleID
        {
            get
            {
                return searchByVehicleID;
            }
            set
            {
                this.searchByVehicleID = value;
                RaisePropertyChanged(() => this.SearchByVehicleID);
            }
        }

        private DateTime? starttime = DateTime.Now.AddMonths(-1);
        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartTime
        {
            get
            {
                return starttime;
            }
            set
            {
                this.starttime = value;
                if (StartTime != null && EndTime != null)
                    ValidateBeginAndEndDate(ExtractPropertyName(() => StartTime), (DateTime)StartTime, ExtractPropertyName(() => EndTime), (DateTime)EndTime);
                RaisePropertyChanged(() => this.StartTime);
            }
        }

        private DateTime? endtime = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime
        {
            get
            {
                return endtime;
            }
            set
            {
                this.endtime = value;              
                if (StartTime != null && EndTime != null)
                    ValidateBeginAndEndDate(ExtractPropertyName(() => StartTime), (DateTime)StartTime, ExtractPropertyName(() => EndTime), (DateTime)EndTime);
                RaisePropertyChanged(() => this.EndTime);
            }
        }
        #endregion
    }
}
