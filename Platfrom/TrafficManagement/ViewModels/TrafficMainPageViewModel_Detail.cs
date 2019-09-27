using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Traffic.Models;
using Jounce.Core.Command;
using Jounce.Framework.Command;
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
using Command = Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.CommandManageService;

namespace Gsafety.PTMS.Traffic.ViewModels
{
    public partial class TrafficMainPageViewModel
    {
        public IActionCommand BtnSearchCommand { get; private set; }

        string _id = string.Empty;

        Gsafety.PTMS.ServiceReference.TrafficManageService.TrafficManageServiceClient client = null;

        TrafficFeature _feature;

        string _title = string.Empty;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        string _searchtext = string.Empty;

        public string SearchText
        {
            get { return _searchtext; }
            set
            {
                _searchtext = value;
                RaisePropertyChanged(() => SearchText);
            }
        }

        Visibility fenceVisibility = Visibility.Visible;

        public Visibility FenceVisibility
        {
            get { return fenceVisibility; }
            set
            {
                fenceVisibility = value;
                RaisePropertyChanged(() => FenceVisibility);
            }
        }

        Visibility routeVisibility = Visibility.Visible;

        public Visibility RouteVisibility
        {
            get { return routeVisibility; }
            set
            {
                routeVisibility = value;
                RaisePropertyChanged(() => RouteVisibility);
            }
        }

        Visibility noSpeedRuleVisibility = Visibility.Visible;
        public Visibility NoSpeedRuleVisibility
        {
            get { return noSpeedRuleVisibility; }
            set
            {
                noSpeedRuleVisibility = value;
                RaisePropertyChanged(() => NoSpeedRuleVisibility);
            }
        }

        Visibility speedRuleVisibility = Visibility.Collapsed;
        public Visibility SpeedRuleVisibility
        {
            get { return speedRuleVisibility; }
            set
            {
                speedRuleVisibility = value;
                RaisePropertyChanged(() => SpeedRuleVisibility);
            }
        }

        string _name = string.Empty;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }
        string _maxspeed = string.Empty;

        public string MaxSpeed
        {
            get { return _maxspeed; }
            set
            {
                _maxspeed = value;
                RaisePropertyChanged(() => MaxSpeed);
            }
        }
        string _duration = string.Empty;

        public string Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                RaisePropertyChanged(() => Duration);
            }
        }
        string _starttime = string.Empty;

        public string StartTime
        {
            get { return _starttime; }
            set
            {
                _starttime = value;
                RaisePropertyChanged(() => StartTime);
            }
        }
        string _endtime = string.Empty;

        public string EndTime
        {
            get { return _endtime; }
            set
            {
                _endtime = value;
                RaisePropertyChanged(() => EndTime);
            }
        }
        string _address = string.Empty;

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                RaisePropertyChanged(() => Address);
            }
        }
        string _regionproperty = string.Empty;
        public string RegionProperty
        {
            get { return _regionproperty; }
            set
            {
                _regionproperty = value;
                RaisePropertyChanged(() => RegionProperty);
            }
        }
        string _routeproperty = string.Empty;

        public string RouteProperty
        {
            get { return _routeproperty; }
            set
            {
                _routeproperty = value;
                RaisePropertyChanged(() => RouteProperty);
            }
        }
        string _routesegmentproperty = string.Empty;

        public string RouteSegmentProperty
        {
            get { return _routesegmentproperty; }
            set
            {
                _routesegmentproperty = value;
                RaisePropertyChanged(() => RouteSegmentProperty);
            }
        }
        string _routewidth = string.Empty;

        public string RouteWidth
        {
            get { return _routewidth; }
            set
            {
                _routewidth = value;
                RaisePropertyChanged(() => RouteWidth);
            }
        }

        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.Data != null)
                {
                    this.Data.PageSize = pageSizeValue;
                }
            }
        }

        private void Reset()
        {
            Name = string.Empty;
            MaxSpeed = string.Empty;
            Duration = string.Empty;
            StartTime = string.Empty;
            EndTime = string.Empty;
        }

        public int TotalCount
        {
            get
            {
                return this.Data.TotalItemCount;
            }
        }

        protected int currentIndex = 1;

        public PagedServerCollection<TrafficeVehicle> Data
        {
            get;
            set;
        }

        public PagedServerCollection<TrafficeVehicle> FenceData
        {
            get;
            set;
        }

        public PagedServerCollection<TrafficeVehicle> RouteData
        {
            get;
            set;
        }

        public PagedServerCollection<TrafficeVehicle> SpeedLimitData
        {
            get;
            set;
        }

        public void HandleEvent(ShowFenceInfoArgs publishedEvent)
        {
            Data = FenceData;

            if (publishedEvent.selectEleFence != null)
            {
                if (_id != publishedEvent.selectEleFence.ID)
                {
                    _id = publishedEvent.selectEleFence.ID;
                    Name = publishedEvent.selectEleFence.Name;
                    Address = publishedEvent.selectEleFence.Address;

                    if (publishedEvent.selectEleFence.RegionProperty != null)
                    {
                        List<string> lst = publishedEvent.selectEleFence.RegionProperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                        IsControlTime = (lst.IndexOf((((int)(FENCE_RegionProperty.Time_Limit))).ToString()) > -1);
                        InFenceOrRouteAlarm = (lst.IndexOf((((int)(FENCE_RegionProperty.In_AlertToPlatform))).ToString()) > -1);
                        OutFenceOrRouteAlarm = (lst.IndexOf((((int)(FENCE_RegionProperty.Out_AlertToPlatform))).ToString()) > -1);
                        IsControlSpeed = (lst.IndexOf((((int)(FENCE_RegionProperty.Speed_Limit))).ToString()) > -1);
                    }
                    else
                    {
                        IsControlTime = false;
                        InFenceOrRouteAlarm = false;
                        OutFenceOrRouteAlarm = false;
                        IsControlSpeed = false;
                    }

                    if (IsControlSpeed)
                    {
                        MaxSpeed = publishedEvent.selectEleFence.MaxSpeed.ToString();
                        Duration = publishedEvent.selectEleFence.OverSpeedDuration.ToString();
                    }
                    else
                    {
                        MaxSpeed = "-";
                        Duration = "-";
                    }
                    if (IsControlTime)
                    {
                        DateTime date = DateTime.Parse(publishedEvent.selectEleFence.StartTime);
                        StartTime = date.ToLocalTime().ToString();
                        date = DateTime.Parse(publishedEvent.selectEleFence.EndTime);
                        EndTime = date.ToLocalTime().ToString();
                    }
                    else
                    {
                        StartTime = "-";
                        EndTime = "-";
                    }


                    Search();
                }
            }
            else
            {
                _id = string.Empty;
                Reset();
                RegionProperty = string.Empty;
            }
        }

        public void HandleEvent(SelectedRouteChange publishedEvent)
        {
            Data = RouteData;

            if (publishedEvent.SelectedRoute != null)
            {
                if (_id != publishedEvent.SelectedRoute.ID)
                {
                    _id = publishedEvent.SelectedRoute.ID;
                    Name = publishedEvent.SelectedRoute.Name;
                    RouteWidth = publishedEvent.SelectedRoute.Width.ToString();

                    if (publishedEvent.SelectedRoute.RouteSegmentProperty != null)
                    {
                        List<string> lst = publishedEvent.SelectedRoute.RouteSegmentProperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                        IsControlSpeed = (lst.IndexOf((((int)(Route_RouteSegmentProperty.Speed_Limit))).ToString()) > -1);
                    }
                    else
                    {
                        IsControlSpeed = false;
                    }

                    if (publishedEvent.SelectedRoute.RouteProperty != null)
                    {
                        List<string> lst = publishedEvent.SelectedRoute.RouteProperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                        IsControlTime = (lst.IndexOf((((int)(Route_RouteProperty.Time_Limit))).ToString()) > -1);
                        InFenceOrRouteAlarm = (lst.IndexOf((((int)(Route_RouteProperty.In_AlertToPlatform))).ToString()) > -1);
                        OutFenceOrRouteAlarm = (lst.IndexOf((((int)(Route_RouteProperty.Out_AlertToPlatform))).ToString()) > -1);
                    }
                    else
                    {
                        IsControlTime = false;
                        InFenceOrRouteAlarm = false;
                        OutFenceOrRouteAlarm = false;
                    }

                    if (IsControlSpeed)
                    {
                        MaxSpeed = publishedEvent.SelectedRoute.MaxSpeed.ToString();
                        Duration = publishedEvent.SelectedRoute.OverSpeedDuration.ToString();
                    }
                    else
                    {
                        MaxSpeed = "-";
                        Duration = "-";
                    }
                    if (IsControlTime)
                    {
                        DateTime date = DateTime.Parse(publishedEvent.SelectedRoute.StartTime);
                        StartTime = date.ToLocalTime().ToString();
                        date = DateTime.Parse(publishedEvent.SelectedRoute.EndTime);
                        EndTime = date.ToLocalTime().ToString();
                    }
                    else
                    {
                        StartTime = "-";
                        EndTime = "-";
                    }

                    Search();
                }
            }
            else
            {
                _id = string.Empty;
                Reset();
                RouteProperty = string.Empty;
                RouteSegmentProperty = string.Empty;
            }
        }

        public void HandleEvent(SelectedSpeedLimitChange publishedEvent)
        {
            Data = SpeedLimitData;

            if (publishedEvent.SelectedSpeedLimit != null)
            {
                if (_id != publishedEvent.SelectedSpeedLimit.ID)
                {
                    _id = publishedEvent.SelectedSpeedLimit.ID;
                    Name = publishedEvent.SelectedSpeedLimit.Name;
                    MaxSpeed = publishedEvent.SelectedSpeedLimit.MaxSpeed.ToString();
                    Duration = publishedEvent.SelectedSpeedLimit.Duration.ToString();
                    Search();
                }
            }
            else
            {
                _id = string.Empty;
                RegionProperty = string.Empty;
            }
        }

        private void InitialDetail()
        {
            BtnSearchCommand = new ActionCommand<object>(obj => Search());
            InitialClient();
            InitPagination();
        }

        private void InitialClient()
        {
            client = ServiceClientFactory.Create<TrafficManageServiceClient>();
            client.GetFenceQueueListByFenceIDCompleted += client_GetFenceQueueListByFenceIDCompleted;
            client.GetRouteQueueByRouteIDCompleted += client_GetRouteQueueByRouteIDCompleted;
        }

        private Command.CommandManageServiceClient InitialVehicleServiceClient()
        {
            Command.CommandManageServiceClient _client = ServiceClientFactory.Create<Command.CommandManageServiceClient>();
            _client.GetVehicleSpeedListBySpeedIDCompleted += _client_GetVehicleSpeedListBySpeedIDCompleted;
            return _client;
        }

        private void CloseCommandManageServiceClient(Command.CommandManageServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        private bool _InFenceOrRouteAlarm;
        public bool InFenceOrRouteAlarm
        {
            get
            {
                return _InFenceOrRouteAlarm;

            }
            set
            {
                _InFenceOrRouteAlarm = value;
                RaisePropertyChanged("InFenceOrRouteAlarm");
            }
        }

        private bool _OutFenceOrRouteAlarm;
        public bool OutFenceOrRouteAlarm
        {
            get
            {
                return _OutFenceOrRouteAlarm;
            }
            set
            {
                _OutFenceOrRouteAlarm = value;
                RaisePropertyChanged("OutFenceOrRouteAlarm");
            }
        }

        private bool _IsControlSpeed;
        public bool IsControlSpeed
        {
            get
            {
                return _IsControlSpeed;
            }
            set
            {
                _IsControlSpeed = value;
                RaisePropertyChanged("IsControlSpeed");
            }
        }

        private bool _IsControlTime;
        public bool IsControlTime
        {
            get
            {
                return _IsControlTime;
            }
            set
            {
                _IsControlTime = value;
                RaisePropertyChanged("IsControlTime");
            }
        }

        void client_GetRouteQueueByRouteIDCompleted(object sender, GetRouteQueueByRouteIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        List<TrafficeVehicle> items = new List<TrafficeVehicle>();
                        foreach (var item in e.Result.Result)
                        {
                            TrafficeVehicle tv = new TrafficeVehicle();
                            tv.VehicleID = item.VehicleID;
                            tv.Status = (short)item.Status;
                            switch (tv.Status)
                            {
                                case 0:
                                    tv.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("NoDown");
                                    break;
                                case 1:
                                    tv.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("WaitDown");
                                    break;
                                case 2:
                                    tv.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("Downing");
                                    break;
                                case 3:
                                    tv.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("DownSuccess");
                                    break;
                                case 4:
                                    tv.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("DownError");
                                    break;

                            }
                            items.Add(tv);
                        }
                        RouteData.loader_Finished(new BaseLib.Model.PagedResult<TrafficeVehicle>()
                        {
                            Count = e.Result.TotalRecord,
                            Items = items,//数据列表
                            PageIndex = currentIndex
                        });

                        if (type == 1)
                        {
                            Data = RouteData;
                            RaisePropertyChanged(() => Data);
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
                InitialClient();
                ApplicationContext.Instance.Logger.LogException("client_GetByNameDevGpsList", ex);
            }
        }

        void client_GetFenceQueueListByFenceIDCompleted(object sender, GetFenceQueueListByFenceIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess == true)
                    {
                        List<TrafficeVehicle> items = new List<TrafficeVehicle>();
                        foreach (var item in e.Result.Result)
                        {
                            TrafficeVehicle tv = new TrafficeVehicle();
                            tv.VehicleID = item.VehicleID;

                            tv.Status = item.Status;
                            switch (tv.Status)
                            {
                                case 0:
                                    tv.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("NoDown");
                                    break;
                                case 1:
                                    tv.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("WaitDown");
                                    break;
                                case 2:
                                    tv.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("Downing");
                                    break;
                                case 3:
                                    tv.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("DownSuccess");
                                    break;
                                case 4:
                                    tv.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("DownError");
                                    break;

                            }

                            items.Add(tv);
                        }
                        FenceData.loader_Finished(new BaseLib.Model.PagedResult<TrafficeVehicle>()
                        {
                            Count = e.Result.TotalRecord,
                            Items = items,//数据列表
                            PageIndex = currentIndex
                        });

                        if (type == 0)
                        {
                            Data = FenceData;
                            RaisePropertyChanged(() => Data);
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
                InitialClient();
                ApplicationContext.Instance.Logger.LogException("client_GetByNameDevGpsList", ex);
            }

        }

        void _client_GetVehicleSpeedListBySpeedIDCompleted(object sender, Command.GetVehicleSpeedListBySpeedIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess == true)
                    {
                        List<TrafficeVehicle> items = new List<TrafficeVehicle>();
                        foreach (var ite in e.Result.Result)
                        {
                            TrafficeVehicle tv = new TrafficeVehicle();
                            tv.VehicleID = ite.VehicleID;
                            tv.Status = (short)ite.Status;
                            switch (tv.Status)
                            {
                                case 0:
                                    tv.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("NoDown");
                                    break;
                                case 1:
                                    tv.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("WaitDown");
                                    break;
                                case 2:
                                    tv.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("Downing");
                                    //ite.VehicleBtnEnable = false;
                                    break;
                                case 3:
                                    tv.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("DownError");
                                    break;
                                case 4:
                                    tv.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("DownSuccess");
                                    //ite.VehicleBtnEnable = false;
                                    break;
                            }

                            items.Add(tv);
                        }

                        SpeedLimitData.loader_Finished(new PagedResult<TrafficeVehicle>
                        {
                            Count = e.Result.TotalRecord,
                            Items = items,//数据列表
                            PageIndex = currentIndex
                        });

                        if (type == 2)
                        {
                            Data = SpeedLimitData;
                            RaisePropertyChanged(() => Data);
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
                ApplicationContext.Instance.Logger.LogException("_client_GetHeartbeatVehicleListByHeartBeatIDCompleted()", ex);
            }
            finally
            {
                Command.CommandManageServiceClient client = sender as Command.CommandManageServiceClient;
                CloseCommandManageServiceClient(client);
            }
        }

        protected void InitPagination()
        {
            try
            {
                FenceData = new BaseLib.Model.PagedServerCollection<TrafficeVehicle>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                    PagingInfo page = new PagingInfo();
                    page.PageIndex = pageIndex;
                    page.PageSize = pageSize;

                    client.GetFenceQueueListByFenceIDAsync(_id, ApplicationContext.Instance.AuthenticationInfo.ClientID, SearchText, currentIndex, pageSizeValue);
                });

                RouteData = new BaseLib.Model.PagedServerCollection<TrafficeVehicle>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                    PagingInfo page = new PagingInfo();
                    page.PageIndex = pageIndex;
                    page.PageSize = pageSize;

                    client.GetRouteQueueByRouteIDAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, _id, SearchText, currentIndex, pageSizeValue);
                });

                SpeedLimitData = new BaseLib.Model.PagedServerCollection<TrafficeVehicle>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                    PagingInfo page = new PagingInfo();
                    page.PageIndex = pageIndex;
                    page.PageSize = pageSize;

                    Command.CommandManageServiceClient client = InitialVehicleServiceClient();
                    client.GetVehicleSpeedListBySpeedIDAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, _id, SearchText, currentIndex, pageSizeValue);
                });

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        private void Search()
        {
            Data.MoveToFirstPage();
        }

        public void HandleEvent(TrafficFeature publishedEvent)
        {
            _feature = publishedEvent;
            switch (publishedEvent)
            {
                case TrafficFeature.Traffic_PolygonFence:
                    SpeedRuleVisibility = Visibility.Collapsed;
                    NoSpeedRuleVisibility = Visibility.Visible;
                    FenceVisibility = Visibility.Visible;
                    RouteVisibility = Visibility.Collapsed;
                    RaisePropertyChanged(() => FenceVisibility);
                    RaisePropertyChanged(() => RouteVisibility);
                    type = 0;
                    FenceData.RefreshPage();
                    break;
                case TrafficFeature.Traffic_Route:
                    SpeedRuleVisibility = Visibility.Collapsed;
                    NoSpeedRuleVisibility = Visibility.Visible;
                    FenceVisibility = Visibility.Collapsed;
                    RouteVisibility = Visibility.Visible;
                    RaisePropertyChanged(() => FenceVisibility);
                    RaisePropertyChanged(() => RouteVisibility);
                    type = 1;
                    RouteData.RefreshPage();
                    break;
                case TrafficFeature.Traffic_SpeedLimit:
                    SpeedRuleVisibility = Visibility.Visible;
                    FenceVisibility = Visibility.Collapsed;
                    RouteVisibility = Visibility.Collapsed;
                    NoSpeedRuleVisibility = Visibility.Collapsed;
                    type = 2;
                    SpeedLimitData.RefreshPage();
                    break;
                default:
                    break;
            }
        }

        int type = 0;
    }

    public class TrafficeVehicle
    {
        string _vehicleid = string.Empty;

        public string VehicleID
        {
            get { return _vehicleid; }
            set { _vehicleid = value; }
        }

        int _status = 0;

        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        string _showStatus;
        public string ShowStatus
        {
            get { return _showStatus; }
            set { _showStatus = value; }
        }

    }
}
